using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Stats
{
    public class TotalMoneyModel : PageModel
    {
        public decimal TotalMonetaryDonations { get; set; }

        public void OnGet()
        {
            // Connection string to your database
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL query to get the total monetary donations
                string sqlQuery = "SELECT SUM(DonationAmount) AS TotalMonetaryDonations " +
                                  "FROM DAF_Donations " +
                                  "WHERE DonationType = 'Monetary'";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // Execute the query and store the result
                    object result = command.ExecuteScalar();

                    // Check if the result is not null
                    if (result != null)
                    {
                        TotalMonetaryDonations = (decimal)result;
                    }
                    else
                    {
                        // Handle the case where the result is null (no donations or an error occurred)
                        TotalMonetaryDonations = 0;
                    }
                }
            }
        }
    }
}