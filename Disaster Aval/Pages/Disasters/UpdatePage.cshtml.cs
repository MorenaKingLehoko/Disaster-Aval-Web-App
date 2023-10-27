using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Disasters
{
    public class UpdatePageModel : PageModel
    {
        public class Disaster
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public decimal AvailableMoney { get; set; }


        }
        public List<Disaster> Disasters { get; set; }
        public void OnGet()
        {



            // Replace this connection string with your database connection string
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            PurchaseGoodsModel A = new PurchaseGoodsModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define your SQL query to retrieve disaster data
                string sqlQuery = "SELECT D.DisasterID, D.Name AS DisasterName, Don.DonationAmount + @amountSpent AS TotalDonationAmount FROM [dbo].[DAF_Disasters] D LEFT JOIN [dbo].[DAF_Donations] Don ON D.DisasterID = Don.DisasterID WHERE Don.DonationAmount > 0 ORDER BY D.Name;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // Add the @amountSpent parameter
                    command.Parameters.AddWithValue("@amountSpent", A.amountSpent);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Disasters = new List<Disaster>();

                        while (reader.Read())
                        {
                            // Map database columns to Disaster properties
                            var disaster = new Disaster
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                AvailableMoney = reader.GetDecimal(2)
                            };

                            Disasters.Add(disaster);
                        }
                    }
                }
            }
        }
    }
}