using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace EbayOrdersDownloaderConsole.DAL
{
    public class DetailRecord : DALProvider
    {
        public string InvoiceID;
        public string PurchaseID;
        public string VolumeID;
        public string VolumeName;
        public string SourceCode;
        public string ProductSKUCode;
        public string ProductDescription;
        public decimal? Quantity;
        public double? UnitPrice;
        public double? ExtendedPrice;
        public string CouponCodes;
        public decimal? I_StatusCode;
        public DateTime? I_ShipDate;
        public string I_Tracking;
        public decimal? I_ShippingMethod;
        public int? I_SyncWithShop;
        public float? I_OriginalCost;
        public decimal? I_OriginalQTY;
        public decimal? Commision;

        public static void Add(IEnumerable<DetailRecord> detailRecordList)
        {
            var command = BuildCommand("[dbo].[PrDetailRecord_Add]");
            CommandExecute(command, cmd =>
            {
                foreach (var detailRecord in detailRecordList)
                {
                    cmd.Parameters.Clear();
                    FillCommand((SqlCommand)cmd
                                    , CreateParameter("InvoiceID", detailRecord.InvoiceID)
                                    , CreateParameter("PurchaseID", detailRecord.PurchaseID)
                                    , CreateParameter("VolumeID", detailRecord.VolumeID)
                                    , CreateParameter("VolumeName", detailRecord.VolumeName)
                                    , CreateParameter("SourceCode", detailRecord.SourceCode)
                                    , CreateParameter("ProductSKUCode", detailRecord.ProductSKUCode)
                                    , CreateParameter("ProductDescription", detailRecord.ProductDescription)
                                    , CreateParameter("Quantity", detailRecord.Quantity)
                                    , CreateParameter("UnitPrice", detailRecord.UnitPrice)
                                    , CreateParameter("ExtendedPrice", detailRecord.ExtendedPrice)
                                    , CreateParameter("CouponCodes", detailRecord.CouponCodes)
                                    , CreateParameter("I_StatusCode", detailRecord.I_StatusCode)
                                    , CreateParameter("I_ShipDate", detailRecord.I_ShipDate)
                                    , CreateParameter("I_Tracking", detailRecord.I_Tracking)
                                    , CreateParameter("I_ShippingMethod", detailRecord.I_ShippingMethod)
                                    , CreateParameter("I_SyncWithShop", detailRecord.I_SyncWithShop)
                                    , CreateParameter("I_OriginalCost", detailRecord.I_OriginalCost)
                                    , CreateParameter("I_OriginalQTY", detailRecord.I_OriginalQTY)
                                    , CreateParameter("Commision", detailRecord.Commision)
                                );
                    cmd.ExecuteNonQuery();
                }
            });
        }
    }
}
