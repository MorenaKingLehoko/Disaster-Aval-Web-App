using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Disaster_Aval.Pages.Disasters
{
    public class DonationsListModel : PageModel
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

                // Defining My SQL query to retrieve donations
                string sqlQuery = "SELECT d.donationID, u.Name, dd.Name AS DisasterName, d.DonationAmount, d.DonationType\r\nFROM DAF_Donations d\r\nINNER JOIN DAF_Users u ON d.UserID = u.UserID\r\nINNER JOIN DAF_Disasters dd ON d.DisasterID = dd.DisasterID\r\nWHERE d.DonationType = 'Monetary';";

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
                                DonationAmount = reader.IsDBNull(3) ? (decimal?)null : reader.GetDecimal(3),
                                DonationItemID = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };

                            Donations.Add(donation);
                        }
                    }
                }
            }
        }
    }
}












