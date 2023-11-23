using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Stats
{
    public class TotalGoodsModel : PageModel
    {
        public int TotalGoodsReceived { get; set; }

        public void OnGet()
        {
            // Your connection string
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Your SQL query
                string sqlQuery = "SELECT COUNT(ItemID) AS TotalGoodsReceived FROM DAF_Items";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // Execute the query and get the result
                    TotalGoodsReceived = (int)command.ExecuteScalar();
                }
            }
        }
    }
}