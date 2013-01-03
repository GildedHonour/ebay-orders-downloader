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

namespace EbayOrdersDownloaderConsole
{
    class Program
    {
        private static ApiContext _apiContext = GetApiContext();
        private static Timer _timer = new Timer(_ => OnTimer(), null, _dueTime, Timeout.Infinite);
        private static readonly int _dueTime = int.Parse(ConfigurationManager.AppSettings["dueTime"]) * 10;

        static void Main(string[] args)
        {
            //_apiContext = GetApiContext();
            //_timer = new Timer(_ => OnTimer(), null, _dueTime, Timeout.Infinite);
            Console.ReadLine();
        }

        private static void OnTimer()
        {
            using (var context = new OrderManagerEntities())
            {
                GetOrdersCall getOrdersApiCall = new GetOrdersCall(_apiContext);
                OrderTypeCollection orders = getOrdersApiCall.GetOrders(
                        new TimeFilter { TimeFrom = DateTime.Now.AddDays(-111), TimeTo = DateTime.Now },
                        TradingRoleCodeType.Seller,
                        OrderStatusCodeType.Completed
                    );
                foreach (OrderType order in orders)
                {
                    //if not exist in db then save the order to db
                    if (context.HeaderRecords.SingleOrDefault(x => x.OrderID == order.OrderID) == null)
                    {
                        var fetchedItem = new ItemType();
                        fetchedItem.CategoryMappingAllowed = true;
                        fetchedItem.ProductListingDetails = new ProductListingDetailsType();
                        fetchedItem.ProductListingDetails.IncludePrefilledItemInformation = true;
                        string orderID = order.OrderID.Substring(0, order.OrderID.IndexOf("-"));
                        GetItemCall getItemApiCall = new GetItemCall(_apiContext);
                        getItemApiCall.IncludeItemCompatibilityList = true;
                        getItemApiCall.IncludeItemSpecifics = true;
                        fetchedItem = getItemApiCall.GetItem(orderID);
                        
                        #region Header record
                        var headerRecord = new HeaderRecords
                        {
                            OrderID = order.OrderID,
                            InvoiceID = order.ShippingDetails.SellingManagerSalesRecordNumber.ToString(),
                            OrderDate = order.ExternalTransaction[0].ExternalTransactionTime.ToShortDateString(), //? + todo!
                            Email = order.TransactionArray[0].Buyer.Email, //? invalid request
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
                            Commision = order.ExternalTransaction[0].FeeOrCreditAmount.Value.ToString() //each of the Item FinalValueFee
                        };
                        context.HeaderRecords.AddObject(headerRecord);
                        #endregion

                        var detailRecord = new DetailRecord
                        {
                            InvoiceID = order.OrderID,
                            PurchaseID = order.TransactionArray[0].ShippingDetails.SellingManagerSalesRecordNumber.ToString(),
                            VolumeID = null,
                            VolumeName = null,
                            SourceCode = order.TransactionArray[0].Item.Site.ToString(),
                            ProductSKUCode = fetchedItem.SKU, 
                            ProductDescription = order.TransactionArray[0].Item.Title,
                            Quantity = order.TransactionArray[0].QuantityPurchased,
                            UnitPrice = order.TransactionArray[0].TransactionPrice.Value,
                            ExtendedPrice = order.TransactionArray[0].TransactionPrice.Value * order.TransactionArray[0].QuantityPurchased,
                            CouponCodes = "",//?
                            I_StatusCode = null,//?
                            I_ShipDate = null,//?
                            I_Tracking = "",//?
                            I_ShippingMethod = null,//?
                            I_SyncWithShop = null,//?
                            I_OriginalCost = null,//?
                            I_OriginalQTY = null,//?
                            Commision = null//?
                        };
                        context.DetailRecord.AddObject(detailRecord);

                        var orderStatus = new OrderStatus
                        {
                            OrderID = order.OrderID,
                            Status = 1
                        };

                        var orderMessage = new OrderMessage
                        {
                            InvoiceID = order.OrderID,
                            To = "",//?
                            From = "",//?
                            Message = "",//?
                            DeliveryDate = ""//?
                        };
                        context.OrderMessage.AddObject(orderMessage);

                        //does it have to be used and changed?
                        //var status = new Status
                        //{

                        //};

                        context.OrderStatus.AddObject(orderStatus);
                        context.SaveChanges();
                        context.AcceptAllChanges();
                    }
                }

                RestartTimer();
            }
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
