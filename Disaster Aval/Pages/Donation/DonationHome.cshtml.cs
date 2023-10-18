using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Donation
{
    public class DonationHomeModel : PageModel
    {
        public class Disaster
        {

            public int DisasterID { get; set; }
            [DisplayName("Disaster Type")]
            public string Name { get; set; }
            [DisplayName("Aids Needed")]
            public string AidsNeeded { get; set; }
        }

        public List<Disaster> Disasters { get; set; }

        public void OnGet()
        {
            Disasters = new List<Disaster>();

            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT DisasterID, Name, Aids FROM DAF_Disasters";
               // string query = "SELECT DisasterID, Name, Aids AS 'Aids Required' FROM DAF_Disasters";
                //string query = "SELECT DisasterID, Name, Aids AS 'Aids Required' FROM DAF_Disasters";



                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Disasters.Add(new Disaster
                        {
                            DisasterID = (int)reader["DisasterID"],
                            Name = (string)reader["Name"],
                            AidsNeeded = (string)reader["Aids"]
                        });
                    }
                }
            }
        }
    }
}
