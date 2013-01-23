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
using System.Transactions;
using eBay.Service.Call;
using eBay.Service.Util;
using System.Data.SqlClient;

namespace EbayOrdersDownloaderConsole
{
    class Program
    {
        private static ApiContext _apiContext = GetApiContext();
        private static Timer _timer;
        private static readonly int _dueTime = int.Parse(ConfigurationManager.AppSettings["dueTime"]) * 10;

        static void Main(string[] args)
        {
            _apiContext = GetApiContext();
            _timer = new Timer(_ => OnTimer(), null, _dueTime, Timeout.Infinite);
            Console.ReadLine();
        }

        private static void OnTimer()
        {
            GetOrdersCall getOrdersApiCall = new GetOrdersCall(_apiContext);
            OrderTypeCollection orders = getOrdersApiCall.GetOrders(
                    new TimeFilter { TimeFrom = DateTime.Now.AddDays(-111), TimeTo = DateTime.Now },
                    TradingRoleCodeType.Seller,
                    OrderStatusCodeType.Completed
                );
            foreach (OrderType order in orders)
            {
                //if it doesn not exist in db then save it to db
                if (!DAL.HeaderRecords.Exists(order.OrderID)) 
                {
                    #region Header record
                    var headerRecord = new DAL.HeaderRecords
                    {
                        OrderID = order.OrderID,
                        InvoiceID = order.ShippingDetails.SellingManagerSalesRecordNumber.ToString(),
                        //OrderDate = order.ExternalTransaction[0].ExternalTransactionTime.ToShortDateString(), // it gives null
                        Email = order.TransactionArray[0].Buyer.Email, //? it gives the value of "invalid request"
                        ShopperID = null,
                        BilltoFirstName = order.ShippingAddress.Name,
                        BilltoLastName = order.ShippingAddress.Name,
                        BilltoCompanyName = order.BuyerUserID,
                        BilltoStreetAddress = null,
                        BilltoOptionalAddress = null,
                        BilltoCity = null,
                        BilltoState = null,
                        BilltoZip = null,
                        BilltoCountry = null,
                        BilltoRegion = null,
                        BilltoPhone = null,
                        DeliveryMethod = order.ShippingServiceSelected.ShippingService == "ShippingMethodStandard" ? "1" : "5",
                        ProductSubtotal = "", //?
                        ShippingAndHandling = order.ShippingServiceSelected.ShippingServiceCost.Value.ToString(),
                        TaxMultiplier = null, //?
                        Tax = order.ShippingDetails.SalesTax.SalesTaxAmount.Value.ToString(),
                        Discount = "0",
                        OverallTotal = order.Total.Value.ToString(), //?
                        CreditCardType = "",
                        CreditCardNumber = "",
                        CreditCardExpiration = "",
                        CreditCardSecurityNumberCCV = "",
                        NameOnCreditCard = "",
                        ShiptoFirstName = order.ShippingAddress.Name,
                        ShiptoLastName = order.ShippingAddress.Name,
                        ShiptoCompanyName = order.BuyerUserID,
                        ShiptoStreetAddress = order.ShippingAddress.Street1,
                        ShiptoOptionalAddress = order.ShippingAddress.Street2,
                        ShiptoCity = order.ShippingAddress.CityName,
                        ShiptoState = order.ShippingAddress.StateOrProvince,
                        ShiptoZip = order.ShippingAddress.PostalCode,
                        ShiptoCountry = order.ShippingAddress.CountryName,
                        ShiptoRegion = "", //?
                        ShiptoPhone = order.ShippingAddress.Phone,
                        SHOPCOMCatalogID = null,
                        CatalogName = "EBAY",
                        MultiplePaymentsQuantity = "", //?
                        CanSellNamePrivacyFlag = "", //?
                        CanSendOffersPrivacyFlag = "", //?
                        ShopperComments = "", //?
                        Source = "EBAY",
                        GiftMessage = "", //?
                        //Commision = order.ExternalTransaction[0].FeeOrCreditAmount.Value.ToString() // it gives null
                    };
                    #endregion

                    List<DAL.DetailRecord> detailRecordList = new List<DAL.DetailRecord>();
                    foreach (TransactionType transactionItem in order.TransactionArray)
                    {
                        var dr = new DAL.DetailRecord
                        {
                            InvoiceID = order.OrderID,
                            PurchaseID = transactionItem.ShippingDetails.SellingManagerSalesRecordNumber.ToString(),
                            VolumeID = null,
                            VolumeName = null,
                            SourceCode = transactionItem.Item.Site.ToString(),
                            ItemID = transactionItem.Item.ItemID,
                            ProductDescription = transactionItem.Item.Title,
                            Quantity = transactionItem.QuantityPurchased,
                            UnitPrice = transactionItem.TransactionPrice.Value,
                            ExtendedPrice = transactionItem.TransactionPrice.Value * transactionItem.QuantityPurchased,
                            CouponCodes = "",//?
                            I_StatusCode = 0,//?
                            I_ShipDate = null,//?
                            I_Tracking = "",//?
                            I_ShippingMethod = null,//?
                            I_SyncWithShop = null,//?
                            I_OriginalCost = null,//?
                            I_OriginalQTY = null,//?
                            Commision = null//?
                        };

                        detailRecordList.Add(dr);
                    }

                    DAL.HeaderRecords.Add(headerRecord);
                    List<string> skuList = DAL.ListingsHelper.GetMPN(detailRecordList.Select(x => x.ItemID));
                    for (int i = 0; i < detailRecordList.Count; i++)
                    {
                        detailRecordList[i].ProductSKUCode = skuList[i];
                        DAL.DetailRecord.Add(detailRecordList);
                    }

                    var orderStatus = new DAL.OrderStatus
                    {
                        OrderID = order.OrderID,
                        Status = 1
                    };
                    DAL.OrderStatus.Add(orderStatus);

                    var orderMessage = new DAL.OrderMessage
                    {
                        InvoiceID = order.OrderID,
                        To = "",//?
                        From = "",//?
                        Message = "",//?
                        DeliveryDate = ""//?
                    };
                    DAL.OrderMessage.Add(orderMessage);
                }
            }

            RestartTimer();
        }

        private static ApiContext GetApiContext()
        {
            if (_apiContext != null)
            {
                return _apiContext;
            }
            else
            {
                _apiContext = new ApiContext();
                _apiContext.SoapApiServerUrl = ConfigurationManager.AppSettings["apiServerUrl.Production"];
                ApiCredential apiCredential = new ApiCredential();
                apiCredential.eBayToken = ConfigurationManager.AppSettings["apiToken.Production"];
                _apiContext.ApiCredential = apiCredential;
                _apiContext.Site = SiteCodeType.US;
                bool result;
                if (bool.TryParse(ConfigurationManager.AppSettings["enableLogging"], out result) && result)
                {
                    _apiContext.ApiLogManager = new ApiLogManager();
                    string logFullFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "log.txt");
                    _apiContext.ApiLogManager.ApiLoggerList.Add(new FileLogger(logFullFileName, true, true, true));
                    _apiContext.ApiLogManager.EnableLogging = true;
                }

                return _apiContext;
            }
        }

        private static void RestartTimer()
        {
            _timer.Change(_dueTime, 0);
        }
    }
}
