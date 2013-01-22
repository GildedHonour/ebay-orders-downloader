using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbayOrdersDownloaderConsole.DAL
{
    public class OrderStatus : DALProvider
    {
        public string OrderID;
        public int Status;
        public string Notes;
        public string Tracking;
        public int ShipMethod;
        public string ShipDate;
        public int Synced;
        public int ProcessedBy;
        public string PONum;
        public int Vendor;
        public string InvoiceNum;

        internal static void Add(OrderStatus orderStatus)
        {
            var command = BuildCommand("[dbo].[PrOrderStatus_Add]"
                    , CreateParameter("OrderID", orderStatus.OrderID, false)
                    , CreateParameter("Status", orderStatus.Status, false)
                    , CreateParameter("Notes", orderStatus.Notes, false)
                    , CreateParameter("Tracking", orderStatus.Tracking, false)
                    , CreateParameter("ShipMethod", orderStatus.ShipMethod, false)
                    , CreateParameter("ShipDate", orderStatus.ShipDate, false)
                    , CreateParameter("Synced", orderStatus.Synced, false)
                    , CreateParameter("ProcessedBy", orderStatus.ProcessedBy, false)
                    , CreateParameter("PONum", orderStatus.PONum, false)
                    , CreateParameter("Vendor", orderStatus.Vendor, false)
                    , CreateParameter("InvoiceNum", orderStatus.InvoiceNum, false)
                    );
            ExecuteNonQuery(command);
        }
    }
}
