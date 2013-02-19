using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbayOrdersDownloader.DAL
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
                    , CreateParameter("OrderID", orderStatus.OrderID)
                    , CreateParameter("Status", orderStatus.Status)
                    , CreateParameter("Notes", orderStatus.Notes)
                    , CreateParameter("Tracking", orderStatus.Tracking)
                    , CreateParameter("ShipMethod", orderStatus.ShipMethod)
                    , CreateParameter("ShipDate", orderStatus.ShipDate)
                    , CreateParameter("Synced", orderStatus.Synced)
                    , CreateParameter("ProcessedBy", orderStatus.ProcessedBy)
                    , CreateParameter("PONum", orderStatus.PONum)
                    , CreateParameter("Vendor", orderStatus.Vendor)
                    , CreateParameter("InvoiceNum", orderStatus.InvoiceNum)
                    );
            ExecuteNonQuery(command);
        }
    }
}
