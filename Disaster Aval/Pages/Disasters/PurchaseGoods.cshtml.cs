using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Disaster_Aval.Pages.Disasters
{
    public class PurchaseGoodsModel : PageModel
    {
        public class Disaster
        {
            public string? Name { get; set; }
            public decimal AvailableMoney { get; set; }
        }

        public List<Disaster> Disasters { get; set; }

        public void OnGet()
        {
            // Replace this connection string with your database connection string
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define your SQL query to retrieve disaster data
                string sqlQuery = "SELECT\r\n    D.Name AS DisasterName,\r\n    SUM(Don.DonationAmount) AS TotalDonationAmount\r\nFROM\r\n    [dbo].[DAF_Disasters] D\r\nLEFT JOIN\r\n    [dbo].[DAF_Donations] Don ON D.DisasterID = Don.DisasterID\r\nGROUP BY\r\n    D.DisasterID, D.Name\r\nHAVING\r\n    SUM(Don.DonationAmount) > 0\r\nORDER BY\r\n    D.Name;\r\n";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Disasters = new List<Disaster>();

                        while (reader.Read())
                        {
                            // Map database columns to Disaster properties
                            var disaster = new Disaster
                            {
                                Name = reader.GetString(0),
                                AvailableMoney = reader.GetDecimal(1)
                            };

                            Disasters.Add(disaster);
                        }
                    }
                }
            }
        }
    }
}
