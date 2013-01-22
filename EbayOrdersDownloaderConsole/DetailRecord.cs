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

        public static void Add(List<DetailRecord> detailRecordList)
        {
            var command = BuildCommand("[dbo].[PrDetailRecord_Add]");
            CommandExecute(command, cmd =>
            {
                foreach (var detailRecord in detailRecordList)
                {
                    cmd.Parameters.Clear();
                    FillCommand((SqlCommand)cmd
                                    , CreateParameter("InvoiceID", detailRecord.InvoiceID, false)
                                    , CreateParameter("PurchaseID", detailRecord.PurchaseID, false)
                                    , CreateParameter("VolumeID", detailRecord.VolumeID, false)
                                    , CreateParameter("VolumeName", detailRecord.VolumeName, false)
                                    , CreateParameter("SourceCode", detailRecord.SourceCode, false)
                                    , CreateParameter("ProductSKUCode", detailRecord.ProductSKUCode, false)
                                    , CreateParameter("ProductDescription", detailRecord.ProductDescription, false)
                                    , CreateParameter("Quantity", detailRecord.Quantity, false)
                                    , CreateParameter("UnitPrice", detailRecord.UnitPrice, false)
                                    , CreateParameter("ExtendedPrice", detailRecord.ExtendedPrice, false)
                                    , CreateParameter("CouponCodes", detailRecord.CouponCodes, false)
                                    , CreateParameter("I_StatusCode", detailRecord.I_StatusCode, false)
                                    , CreateParameter("I_ShipDate", detailRecord.I_ShipDate, false)
                                    , CreateParameter("I_Tracking", detailRecord.I_Tracking, false)
                                    , CreateParameter("I_ShippingMethod", detailRecord.I_ShippingMethod, false)
                                    , CreateParameter("I_SyncWithShop", detailRecord.I_SyncWithShop, false)
                                    , CreateParameter("I_OriginalCost", detailRecord.I_OriginalCost, false)
                                    , CreateParameter("I_OriginalQTY", detailRecord.I_OriginalQTY, false)
                                    , CreateParameter("Commision", detailRecord.Commision, false)
                                );
                    cmd.ExecuteNonQuery();
                }
            });
        }
    }
}
