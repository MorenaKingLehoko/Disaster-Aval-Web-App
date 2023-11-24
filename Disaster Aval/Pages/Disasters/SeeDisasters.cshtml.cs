using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Disasters
{
    public class SeeDisastersModel : PageModel
    {
        public class Disaster
        {
            public string? DisasterID { get; set; }
            public string? Name { get; set; }
            public string? Location { get; set; }
            public DateTime? Startdate { get; set; }
            public DateTime? EndDate { get; set; }

        }

        public List<Disaster> Disasters { get; set; }
        //code to delete from databse
        public IActionResult OnPostDelete(string disasterId)
        {
            if (string.IsNullOrEmpty(disasterId))
            {
                
                return RedirectToPage("/Disasters/SeeDisasters");
            }

           
       
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Defining my SQL query to delete the disaster by ID
                string deleteQuery = "DELETE FROM DAF_Disasters WHERE DisasterID = @DisasterID";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    // Adding parameters to the SQL query
                    command.Parameters.AddWithValue("@DisasterID", disasterId);

                    // Execute the SQL command
                    command.ExecuteNonQuery();
                }
            }

            // Redirect back to the same page or wherever needed
            return RedirectToPage("/Disasters/SeeDisasters");
        }

        public void OnGet()
        {
            // Connecting to the database using your connection string
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Defining my SQL query here to retrieve all disasters
                string sqlQuery = "SELECT Name, Location,StartDate,EndDate FROM DAF_Disasters";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Disasters = new List<Disaster>();

                        while (reader.Read())
                        {
                            // Maping database columns to Disaster properties
                            var disaster = new Disaster
                            {
                               
                                Name = reader.IsDBNull(1) ? null : reader.GetString(0),
                                Location = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Startdate = reader.IsDBNull(1) ? null : reader.GetDateTime(2),
                                EndDate = reader.IsDBNull(1) ? null : reader.GetDateTime(3),

                               
                            };

                            Disasters.Add(disaster);
                        }
                    }
                }
            }
        }
    }
}
