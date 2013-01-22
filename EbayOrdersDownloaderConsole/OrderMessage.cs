using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbayOrdersDownloaderConsole.DAL
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
                     , CreateParameter("InvoiceID", orderMessage.InvoiceID, false)
                     , CreateParameter("To", orderMessage.To, false)
                     , CreateParameter("From", orderMessage.From, false)
                     , CreateParameter("Message", orderMessage.Message, false)
                     , CreateParameter("DeliveryDate", orderMessage.DeliveryDate, false)
                     );
            ExecuteNonQuery(command);
        }
    }
}
