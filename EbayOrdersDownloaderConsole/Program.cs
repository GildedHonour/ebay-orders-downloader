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
                OrderTypeCollection orders = getOrdersApiCall.GetOrders(new TimeFilter { TimeFrom = DateTime.Now.AddDays(-111), TimeTo = DateTime.Now }, TradingRoleCodeType.Seller, OrderStatusCodeType.Active /*TODO*/);
                foreach (OrderType order in orders)
                {
                    //if not exist in db then save the order to db
                    if (context.HeaderRecords.SingleOrDefault(x => x.OrderID == order.OrderID) == null)
                    {
                        GetItemCall getItemApiCall = new GetItemCall(_apiContext);
                        getItemApiCall.IncludeItemCompatibilityList = true;
                        getItemApiCall.IncludeItemSpecifics = true;

                        var fetchedItem = new ItemType();
                        fetchedItem.CategoryMappingAllowed = true;
                        fetchedItem.ProductListingDetails = new ProductListingDetailsType();
                        fetchedItem.ProductListingDetails.IncludePrefilledItemInformation = true;
                        string orderID = order.OrderID.Substring(0, order.OrderID.IndexOf("-"));
                        fetchedItem = getItemApiCall.GetItem(orderID);
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
