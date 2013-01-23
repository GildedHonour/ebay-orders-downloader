using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace EbayOrdersDownloaderConsole.DAL
{
    public class ListingsHelper
    {
        public static List<string> GetMPN(IEnumerable<string> listingIDList)
        {
            List<string> mpnList = new List<string>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ebayMain"].ConnectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("[dbo].[PrListings_GetMPN]", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var listingID in listingIDList)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ListingID", listingID);
                    object mpnRaw = cmd.ExecuteScalar();
                    string mpn = mpnRaw == null ? null : mpnRaw.ToString();
                    mpnList.Add(mpn);
                }
            }

            return mpnList;
        }

    }
}
