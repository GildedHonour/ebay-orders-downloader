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
                    , CreateParameter("OrderID", headerRecord.OrderID)
                    , CreateParameter("InvoiceID", headerRecord.InvoiceID)
                    , CreateParameter("OrderDate", headerRecord.OrderDate)
                    , CreateParameter("Email", headerRecord.Email)
                    , CreateParameter("ShopperID", headerRecord.ShopperID)
                    , CreateParameter("BilltoFirstName", headerRecord.BilltoFirstName)
                    , CreateParameter("BilltoLastName", headerRecord.BilltoLastName)

                    , CreateParameter("BilltoCompanyName", headerRecord.BilltoCompanyName)
                    , CreateParameter("BilltoStreetAddress", headerRecord.BilltoStreetAddress)
                    , CreateParameter("BilltoOptionalAddress", headerRecord.BilltoOptionalAddress)
                    , CreateParameter("BilltoCity", headerRecord.BilltoCity)
                    , CreateParameter("BilltoState", headerRecord.BilltoState)
                    , CreateParameter("BilltoZip", headerRecord.BilltoZip)
                    , CreateParameter("BilltoCountry", headerRecord.BilltoCountry)

                    , CreateParameter("BilltoRegion", headerRecord.BilltoRegion)
                    , CreateParameter("BilltoPhone", headerRecord.BilltoPhone)
                    , CreateParameter("DeliveryMethod", headerRecord.DeliveryMethod)
                    , CreateParameter("ProductSubtotal", headerRecord.ProductSubtotal)
                    , CreateParameter("ShippingAndHandling", headerRecord.ShippingAndHandling)
                    , CreateParameter("TaxMultiplier", headerRecord.TaxMultiplier)
                    , CreateParameter("Tax", headerRecord.Tax)

                    , CreateParameter("Discount", headerRecord.Discount)
                    , CreateParameter("OverallTotal", headerRecord.OverallTotal)
                    , CreateParameter("CreditCardType", headerRecord.CreditCardType)
                    , CreateParameter("CreditCardNumber", headerRecord.CreditCardNumber)
                    , CreateParameter("CreditCardExpiration", headerRecord.CreditCardExpiration)
                    , CreateParameter("CreditCardSecurityNumberCCV", headerRecord.CreditCardSecurityNumberCCV)
                    , CreateParameter("NameOnCreditCard", headerRecord.NameOnCreditCard)

                    , CreateParameter("ShiptoFirstName", headerRecord.ShiptoFirstName)
                    , CreateParameter("ShiptoLastName", headerRecord.ShiptoLastName)
                    , CreateParameter("ShiptoCompanyName", headerRecord.ShiptoCompanyName)
                    , CreateParameter("ShiptoStreetAddress", headerRecord.ShiptoStreetAddress)
                    , CreateParameter("ShiptoOptionalAddress", headerRecord.ShiptoOptionalAddress)
                    , CreateParameter("ShiptoCity", headerRecord.ShiptoCity)
                    , CreateParameter("ShiptoState", headerRecord.ShiptoState)

                    , CreateParameter("ShiptoZip", headerRecord.ShiptoZip)
                    , CreateParameter("ShiptoCountry", headerRecord.ShiptoCountry)
                    , CreateParameter("ShiptoRegion", headerRecord.ShiptoRegion)
                    , CreateParameter("ShiptoPhone", headerRecord.ShiptoPhone)
                    , CreateParameter("SHOPCOMCatalogID", headerRecord.SHOPCOMCatalogID)
                    , CreateParameter("CatalogName", headerRecord.CatalogName)
                    , CreateParameter("MultiplePaymentsQuantity", headerRecord.MultiplePaymentsQuantity)

                    , CreateParameter("CanSellNamePrivacyFlag", headerRecord.CanSellNamePrivacyFlag)
                    , CreateParameter("CanSendOffersPrivacyFlag", headerRecord.CanSendOffersPrivacyFlag)
                    , CreateParameter("ShopperComments", headerRecord.ShopperComments)
                    , CreateParameter("Source", headerRecord.Source)
                    , CreateParameter("PayPalTxnID", headerRecord.PayPalTxnID)
                    , CreateParameter("GiftMessage", headerRecord.GiftMessage)
                    , CreateParameter("Commision", headerRecord.Commision)
                );
            ExecuteNonQuery(command);
        }

        public static bool Exists(string orderID)
        {
            var command = BuildCommand("[dbo].[PrHeaderRecords_Add]", CreateParameter("OrderID", orderID));
            return ((int)ExecuteScalar(command)) == 1;
        }
    }
}
