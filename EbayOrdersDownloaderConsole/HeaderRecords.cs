using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace EbayOrdersDownloaderConsole.DAL
{
    public class HeaderRecords : DALProvider
    {
        #region Public fields
        public string OrderID;
        public string InvoiceID;
        public string OrderDate;
        public string Email;
        public string ShopperID;
        public string BilltoFirstName;
        public string BilltoLastName;
        public string BilltoCompanyName;
        public string BilltoStreetAddress;
        public string BilltoOptionalAddress;
        public string BilltoCity;
        public string BilltoState;
        public string BilltoZip;
        public string BilltoCountry;
        public string BilltoRegion;
        public string BilltoPhone;
        public string DeliveryMethod;
        public string ProductSubtotal;
        public string ShippingAndHandling;
        public string TaxMultiplier;
        public string Tax;
        public string Discount;
        public string OverallTotal;
        public string CreditCardType;
        public string CreditCardNumber;
        public string CreditCardExpiration;
        public string CreditCardSecurityNumberCCV;
        public string NameOnCreditCard;
        public string ShiptoFirstName;
        public string ShiptoLastName;
        public string ShiptoCompanyName;
        public string ShiptoStreetAddress;
        public string ShiptoOptionalAddress;
        public string ShiptoCity;
        public string ShiptoState;
        public string ShiptoZip;
        public string ShiptoCountry;
        public string ShiptoRegion;
        public string ShiptoPhone;
        public string SHOPCOMCatalogID;
        public string CatalogName;
        public string MultiplePaymentsQuantity;
        public string CanSellNamePrivacyFlag;
        public string CanSendOffersPrivacyFlag;
        public string ShopperComments;
        public string Source;
        public string PayPalTxnID;
        public string GiftMessage;
        public string Commision;
        #endregion

        public static void Add(HeaderRecords headerRecord)
        {
            var command = BuildCommand("[dbo].[PrHeaderRecords_Add]"
                    , CreateParameter("OrderID", headerRecord.OrderID, false)
                    , CreateParameter("InvoiceID", headerRecord.InvoiceID, false)
                    , CreateParameter("OrderDate", headerRecord.OrderDate, false)
                    , CreateParameter("Email", headerRecord.Email, false)
                    , CreateParameter("ShopperID", headerRecord.ShopperID, false)
                    , CreateParameter("BilltoFirstName", headerRecord.BilltoFirstName, false)
                    , CreateParameter("BilltoLastName", headerRecord.BilltoLastName, false)

                    , CreateParameter("BilltoCompanyName", headerRecord.BilltoCompanyName, false)
                    , CreateParameter("BilltoStreetAddress", headerRecord.BilltoStreetAddress, false)
                    , CreateParameter("BilltoOptionalAddress", headerRecord.BilltoOptionalAddress, false)
                    , CreateParameter("BilltoCity", headerRecord.BilltoCity, false)
                    , CreateParameter("BilltoState", headerRecord.BilltoState, false)
                    , CreateParameter("BilltoZip", headerRecord.BilltoZip, false)
                    , CreateParameter("BilltoCountry", headerRecord.BilltoCountry, false)

                    , CreateParameter("BilltoRegion", headerRecord.BilltoRegion, false)
                    , CreateParameter("BilltoPhone", headerRecord.BilltoPhone, false)
                    , CreateParameter("DeliveryMethod", headerRecord.DeliveryMethod, false)
                    , CreateParameter("ProductSubtotal", headerRecord.ProductSubtotal, false)
                    , CreateParameter("ShippingAndHandling", headerRecord.ShippingAndHandling, false)
                    , CreateParameter("TaxMultiplier", headerRecord.TaxMultiplier, false)
                    , CreateParameter("Tax", headerRecord.Tax, false)

                    , CreateParameter("Discount", headerRecord.Discount, false)
                    , CreateParameter("OverallTotal", headerRecord.OverallTotal, false)
                    , CreateParameter("CreditCardType", headerRecord.CreditCardType, false)
                    , CreateParameter("CreditCardNumber", headerRecord.CreditCardNumber, false)
                    , CreateParameter("CreditCardExpiration", headerRecord.CreditCardExpiration, false)
                    , CreateParameter("CreditCardSecurityNumberCCV", headerRecord.CreditCardSecurityNumberCCV, false)
                    , CreateParameter("NameOnCreditCard", headerRecord.NameOnCreditCard, false)

                    , CreateParameter("ShiptoFirstName", headerRecord.ShiptoFirstName, false)
                    , CreateParameter("ShiptoLastName", headerRecord.ShiptoLastName, false)
                    , CreateParameter("ShiptoCompanyName", headerRecord.ShiptoCompanyName, false)
                    , CreateParameter("ShiptoStreetAddress", headerRecord.ShiptoStreetAddress, false)
                    , CreateParameter("ShiptoOptionalAddress", headerRecord.ShiptoOptionalAddress, false)
                    , CreateParameter("ShiptoCity", headerRecord.ShiptoCity, false)
                    , CreateParameter("ShiptoState", headerRecord.ShiptoState, false)

                    , CreateParameter("ShiptoZip", headerRecord.ShiptoZip, false)
                    , CreateParameter("ShiptoCountry", headerRecord.ShiptoCountry, false)
                    , CreateParameter("ShiptoRegion", headerRecord.ShiptoRegion, false)
                    , CreateParameter("ShiptoPhone", headerRecord.ShiptoPhone, false)
                    , CreateParameter("SHOPCOMCatalogID", headerRecord.SHOPCOMCatalogID, false)
                    , CreateParameter("CatalogName", headerRecord.CatalogName, false)
                    , CreateParameter("MultiplePaymentsQuantity", headerRecord.MultiplePaymentsQuantity, false)

                    , CreateParameter("CanSellNamePrivacyFlag", headerRecord.CanSellNamePrivacyFlag, false)
                    , CreateParameter("CanSendOffersPrivacyFlag", headerRecord.CanSendOffersPrivacyFlag, false)
                    , CreateParameter("ShopperComments", headerRecord.ShopperComments, false)
                    , CreateParameter("Source", headerRecord.Source, false)
                    , CreateParameter("PayPalTxnID", headerRecord.PayPalTxnID, false)
                    , CreateParameter("GiftMessage", headerRecord.GiftMessage, false)
                    , CreateParameter("Commision", headerRecord.Commision, false)
                );
            ExecuteNonQuery(command);
        }

        public static bool Exists(string orderID)
        {
            var command = BuildCommand("[dbo].[PrHeaderRecords_Add]", CreateParameter("OrderID", orderID, false));
            return ((int)ExecuteScalar(command)) == 1;
        }
    }
}
