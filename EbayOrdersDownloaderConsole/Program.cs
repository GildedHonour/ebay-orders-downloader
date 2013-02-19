using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Util;
using eBay.Service.Call;
using System.Transactions;
using EbayOrdersDownloaderConsole.DAL;
using System.Linq;

namespace EbayOrdersDownloaderConsole
{
    class Program
    {
        private static ApiContext apiContext;
        private static Timer timer;
        private static readonly int dueTime = int.Parse(ConfigurationManager.AppSettings["dueTime"]) * 10;
        private const int DaysAgo = -3;

        static void Main(string[] args)
        {
            apiContext = GetApiContext();
            timer = new Timer(_ => OnTimer(), null, dueTime, Timeout.Infinite);
            Console.ReadLine();
            Console.WriteLine("done!");
        }

        private static void OnTimer()
        {
            GetOrdersCall getOrdersApiCall = new GetOrdersCall(apiContext);
            OrderTypeCollection orders = null;
            try
            {
                orders = getOrdersApiCall.GetOrders(new TimeFilter { TimeFrom = DateTime.Now.AddDays(DaysAgo), TimeTo = DateTime.Now }, TradingRoleCodeType.Seller, OrderStatusCodeType.Completed);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to connect to the server");
                return;
            }

            int counter = 0;
            var ordersList = new List<HeaderRecords>();
            bool orderInserted = false;
            foreach (OrderType order in orders)
            {
                try
                {
                    if (!DAL.HeaderRecords.Exists(order.OrderID))
                    {
                        orderInserted = true;
                        #region Header record
                        var headerRecord = new HeaderRecords
                        {
                            OrderID = order.OrderID,
                            InvoiceID = order.ShippingDetails.SellingManagerSalesRecordNumber.ToString(),
                            OrderDate = order.PaidTime.ToShortDateString(),
                            Email = order.TransactionArray[0].Buyer.Email, //? it gives the value of "invalid request"
                            BilltoFirstName = order.ShippingAddress.Name,
                            BilltoLastName = order.ShippingAddress.Name,
                            BilltoCompanyName = order.BuyerUserID,
                            DeliveryMethod = order.ShippingServiceSelected.ShippingService == "ShippingMethodStandard" ? "1" : "5",
                            ShippingAndHandling = order.ShippingServiceSelected.ShippingServiceCost.Value.ToString(),
                            Tax = order.ShippingDetails.SalesTax.SalesTaxAmount.Value.ToString(),
                            Discount = "0",
                            OverallTotal = order.Total.Value.ToString(),
                            ShiptoFirstName = order.ShippingAddress.Name,
                            ShiptoLastName = order.ShippingAddress.Name,
                            ShiptoCompanyName = order.BuyerUserID,
                            ShiptoStreetAddress = order.ShippingAddress.Street1,
                            ShiptoOptionalAddress = order.ShippingAddress.Street2,
                            ShiptoCity = order.ShippingAddress.CityName,
                            ShiptoState = order.ShippingAddress.StateOrProvince,
                            ShiptoZip = order.ShippingAddress.PostalCode,
                            ShiptoCountry = order.ShippingAddress.CountryName,
                            ShiptoPhone = order.ShippingAddress.Phone,
                            CatalogName = "EBAY",
                            Source = "EBAY"
                        };
                        #endregion

                        List<DAL.DetailRecord> detailRecordList = new List<DAL.DetailRecord>();
                        foreach (TransactionType transactionItem in order.TransactionArray)
                        {
                            #region Detail record
                            var dr = new DAL.DetailRecord
                            {
                                InvoiceID = headerRecord.InvoiceID,
                                PurchaseID = transactionItem.ShippingDetails.SellingManagerSalesRecordNumber.ToString(),
                                SourceCode = transactionItem.Item.Site.ToString(),
                                ItemID = transactionItem.Item.ItemID,
                                ProductDescription = transactionItem.Item.Title,
                                Quantity = transactionItem.QuantityPurchased,
                                UnitPrice = transactionItem.TransactionPrice.Value,
                                ExtendedPrice = transactionItem.TransactionPrice.Value * transactionItem.QuantityPurchased
                            };

                            detailRecordList.Add(dr);
                            #endregion
                        }

                        headerRecord.ProductSubtotal = detailRecordList.Sum(x => x.ExtendedPrice).ToString();
                        headerRecord.Commision = GetCommission(headerRecord.ProductSubtotal);
                        try
                        {
                            #region Inserting objects in database
                            using (var scope = new TransactionScope())
                            {
                                DAL.HeaderRecords.Add(headerRecord);
                                SetSkuCodeForDetailsRecord(detailRecordList);

                                DAL.DetailRecord.Add(detailRecordList);
                                headerRecord.Items.AddRange(detailRecordList);

                                var orderStatus = new DAL.OrderStatus { OrderID = order.OrderID, Status = 1 };
                                DAL.OrderStatus.Add(orderStatus);

                                var orderMessage = new DAL.OrderMessage { InvoiceID = headerRecord.InvoiceID };
                                DAL.OrderMessage.Add(orderMessage);

                                scope.Complete();
                                Console.WriteLine("Order #: " + counter + " has been inserted");
                            }
                            #endregion
                        }

                        catch (TransactionAbortedException)
                        {
                            Console.WriteLine("TransactionAbortedException has been thrown");
                        }

                        ordersList.Add(headerRecord);
                    }
                    else
                    {
                        Console.WriteLine("Order #: " + counter + " has been skipped");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("An error occured at the step #: {0}; message: {1}", counter, e.Message));
                }

                finally
                {
                    counter++;
                }
            }

            if (orderInserted)
            {
                CreateXmlFile(ordersList);
            }

            RestartTimer();
        }

        private static void SetSkuCodeForDetailsRecord(List<DetailRecord> detailRecordList)
        {
            List<string> skuList = ListingsHelper.GetMPN(detailRecordList.Select(x => x.ItemID));
            for (int i = 0; i < detailRecordList.Count; i++)
            {
                detailRecordList[i].ProductSKUCode = skuList[i];
            }
        }

        /// <summary>
        /// Restarts timer
        /// </summary>
        private static void RestartTimer()
        {
            timer.Change(dueTime, 0);
        }

        /// <summary>
        /// Generates file name for resulting xml file
        /// </summary>
        /// <returns></returns>
        private static string GenerateXmlFileName()
        {
            string uniqueFileName = string.Format("{0}-{1}-{2}__{3}-{4}-{5}",
                                                  DateTime.Now.Year, DateTime.Now.Month,
                                                  DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute,
                                                  DateTime.Now.Second);
            uniqueFileName += ".xml";
            string donePath = ConfigurationManager.AppSettings["donePath"];
            string fullFileName;
            if (!donePath.Contains(":"))
            {
                fullFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, donePath, uniqueFileName);
            }
            else
            {
                fullFileName = Path.Combine(donePath, uniqueFileName);
            }

            return fullFileName;
        }

        /// <summary>
        /// Creates resulting xml file
        /// </summary>
        /// <param name="orders">The list of orders</param>
        private static void CreateXmlFile(IEnumerable<HeaderRecords> orders)
        {
            using (var stream = new FileStream(GenerateXmlFileName(), FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (XmlWriter writter = new XmlTextWriter(stream, null))
                {
                    writter.WriteStartDocument();
                    writter.WriteStartElement("orders");
                    writter.WriteAttributeString("dateTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    writter.WriteAttributeString("total", orders.Count().ToString(CultureInfo.InvariantCulture));
                    foreach (var orderItem in orders)
                    {
                        writter.WriteStartElement("order");
                        writter.WriteAttributeString("id", orderItem.OrderID);
                        writter.WriteAttributeString("invoiceID", orderItem.InvoiceID);
                        writter.WriteAttributeString("comission", orderItem.Commision);
                        writter.WriteAttributeString("subTotal", orderItem.ProductSubtotal);
                        foreach (var _item in orderItem.Items)
                        {
                            writter.WriteStartElement("item");
                            writter.WriteElementString("invoiceID", _item.InvoiceID);
                            writter.WriteElementString("purchaseID", _item.PurchaseID);
                            writter.WriteElementString("sku", _item.ProductSKUCode);
                            writter.WriteEndElement();
                        }

                        writter.WriteEndElement();
                    }

                    writter.WriteEndElement();
                    writter.WriteEndDocument();
                }
            }
        }

        /// <summary>
        /// Returns api context
        /// </summary>
        /// <returns>Instance of ApiContext</returns>
        private static ApiContext GetApiContext()
        {
            if (apiContext != null)
            {
                return apiContext;
            }
            else
            {
                apiContext = new ApiContext();
                apiContext.SoapApiServerUrl = ConfigurationManager.AppSettings["apiServerUrl.Production"];
                ApiCredential apiCredential = new ApiCredential();
                apiCredential.eBayToken = ConfigurationManager.AppSettings["apiToken.Production"];
                apiContext.ApiCredential = apiCredential;
                apiContext.Site = SiteCodeType.US;
                bool result;
                if (bool.TryParse(ConfigurationManager.AppSettings["enableLogging"], out result) && result)
                {
                    apiContext.ApiLogManager = new ApiLogManager();
                    string logFullFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "log.txt");
                    apiContext.ApiLogManager.ApiLoggerList.Add(new FileLogger(logFullFileName, true, true, true));
                    apiContext.ApiLogManager.EnableLogging = true;
                }

                return apiContext;
            }
        }

        /// <summary>
        /// Get the commision for the certain price
        /// </summary>
        /// <param name="orderTotalPriceAsString">The total price as a string</param>
        /// <returns>The comission as a string</returns>
        private static string GetCommission(string orderTotalPriceAsString)
        {
            decimal fixedValue;
            if (!decimal.TryParse(ConfigurationManager.AppSettings["commissionFixedX"], out fixedValue) || fixedValue <= 0)
            {
                throw new ArgumentNullException("The value of commissionFixedX is invalid.");
            }

            decimal percentValue;
            if (!decimal.TryParse(ConfigurationManager.AppSettings["commissionPercentY"], out percentValue) || percentValue <= 0)
            {
                throw new ArgumentNullException("The value of commissionPercentY is invalid.");
            }

            decimal orderTotalPrice = decimal.Parse(orderTotalPriceAsString);
            return Math.Round(0 - (fixedValue + (orderTotalPrice / 100 * percentValue)), 2).ToString();
        }
    }
}
