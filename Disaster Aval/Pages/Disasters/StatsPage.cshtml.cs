using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Disasters
{
    public class StatsPageModel : PageModel
    {
        public int TotalGoodsReceived { get; set; }
        public string DonationMessage { get; set; }
        public decimal TotalMonetaryDonations { get; set; }

        public class DisasterStats
        {
            public string DisasterName { get; set; }
            public string Location { get; set; }
            public decimal TotalMoneyDonations { get; set; }
            public int TotalGoodsDonations { get; set; }
        }
        public List<DisasterStats> ActiveDisasterStats { get; set; }
        public void OnGet()
        {
        
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                
                string sqlQuery = "SELECT COUNT(ItemID) AS TotalGoodsReceived FROM DAF_Items";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    
                    TotalGoodsReceived = (int)command.ExecuteScalar();
                }
            }
            //for total money
           // string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL query to get the total monetary donations
                string sqlQuery = "SELECT SUM(DonationAmount) AS TotalMonetaryDonations " +
                                  "FROM DAF_Donations " +
                                  "WHERE DonationType = 'Monetary'";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    
                    object result = command.ExecuteScalar();

                    // Check if the result is not null
                    if (result != null)
                    {
                        TotalMonetaryDonations = (decimal)result;
                    }
                    else
                    {
                        // Handling the case where the result is null (no donations or an error occurred)
                        TotalMonetaryDonations = 0;
                        DonationMessage = "No donations yet";
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT
                    d.DisasterID,
                    d.Name AS DisasterName,
                    d.Location,
                    COALESCE(SUM(DonationAmount), 0) AS TotalMoneyDonations,
                    COALESCE(SUM(CASE WHEN dn.DonationType = 'Goods' THEN 1 ELSE 0 END), 0) AS TotalGoodsDonations
                FROM
                    DAF_Disasters d
                LEFT JOIN
                    DAF_Donations dn ON d.DisasterID = dn.DisasterID
                GROUP BY
                    d.DisasterID, d.Name, d.Location;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ActiveDisasterStats = new List<DisasterStats>();

                        while (reader.Read())
                        {
                            var stats = new DisasterStats
                            {
                                DisasterName = reader["DisasterName"].ToString(),
                                Location = reader["Location"].ToString(),
                                TotalMoneyDonations = Convert.ToDecimal(reader["TotalMoneyDonations"]),
                                TotalGoodsDonations = Convert.ToInt32(reader["TotalGoodsDonations"])
                            };

                            ActiveDisasterStats.Add(stats);
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
    
