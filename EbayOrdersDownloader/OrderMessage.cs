using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbayOrdersDownloader.DAL
{
    public class OrderMessage : DALProvider
    {
        public string InvoiceID;
        public string To;
        public string From;
        public string Message;
        public string DeliveryDate;

        internal static void Add(OrderMessage orderMessage)
        {
            var command = BuildCommand("[dbo].[PrOrderMessage_Add]"
                     , CreateParameter("InvoiceID", orderMessage.InvoiceID)
                     , CreateParameter("To", orderMessage.To)
                     , CreateParameter("From", orderMessage.From)
                     , CreateParameter("Message", orderMessage.Message)
                     , CreateParameter("DeliveryDate", orderMessage.DeliveryDate)
                     );
            ExecuteNonQuery(command);
        }
    }
}
