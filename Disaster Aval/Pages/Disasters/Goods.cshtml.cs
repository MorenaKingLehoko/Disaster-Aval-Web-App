using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Disasters
{
    public class GoodsModel : PageModel
    {
        public class Donation
        {
            public int? DonationID { get; set; }
            public string? UserID { get; set; }
            public string? DisasterID { get; set; }
            public decimal? DonationAmount { get; set; }
            public string? DonationItemID { get; set; }
        }
        public List<Donation> Donations { get; set; } 

        public void OnGet()
        {
            // Connecting to the database using your connection string
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define your SQL query here to retrieve Goods Donations
                string sqlQuery = "SELECT d.DonationID, u.Name AS Donator, dd.Name AS DisasterName, i.ItemName AS DonationItem " +
                                     "FROM DAF_Donations d " +
                                     "INNER JOIN DAF_Users u ON d.UserID = u.UserID " +
                                 "INNER JOIN DAF_Disasters dd ON d.DisasterID = dd.DisasterID " +
                                    "LEFT JOIN DAF_Items i ON d.DonationItemID = i.ItemID " +
                                     "WHERE d.DonationType = 'Goods';";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Donations = new List<Donation>();

                        while (reader.Read())
                        {
                            // Maping database columns to Donation properties
                            var donation = new Donation
                            {
                                DonationID = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0),
                                UserID = reader.IsDBNull(1) ? null : reader.GetString(1),
                                DisasterID = reader.IsDBNull(2) ? null : reader.GetString(2),
                                DonationItemID = reader.IsDBNull(3) ? null : reader.GetString(3)
                            };

                            Donations.Add(donation);
                        }
                    }
                }
            }
        }
    }
}
