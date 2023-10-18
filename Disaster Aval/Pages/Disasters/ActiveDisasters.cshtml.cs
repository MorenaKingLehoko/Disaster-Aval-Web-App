using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Disaster_Aval.Pages.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Load_Donations
{
    public class ActiveDisastersModel : PageModel
    {
        //disaster Name
        public string DisasterDiscription { get; set; }
        public string Location { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AidRequired { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            ActiveDisastersModel activeDisastersModel = new ActiveDisastersModel();

            // Getting the values from the Form
            activeDisastersModel.DisasterDiscription = Request.Form["DisasterDiscription"];
            activeDisastersModel.Location = Request.Form["Location"];
            activeDisastersModel.StartDate = Request.Form["StartDate"];
            activeDisastersModel.EndDate = Request.Form["EndDate"];
            activeDisastersModel.AidRequired = Request.Form["AidRequired"];

           
            

            string ConnectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO DAF_Disasters VALUES (@DisasterDescription, @Location, @StartDate, @EndDate, @AidRequired)";
                cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@DisasterDescription", activeDisastersModel.DisasterDiscription);
                cmd.Parameters.AddWithValue("@Location", activeDisastersModel.Location);
                cmd.Parameters.AddWithValue("@StartDate", activeDisastersModel.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", activeDisastersModel.EndDate);
                cmd.Parameters.AddWithValue("@AidRequired", activeDisastersModel.AidRequired);

                int a = cmd.ExecuteNonQuery();
                conn.Close();

                if (a == 0)
                {
                    return RedirectToPage("LoginFailed", activeDisastersModel);
                }
                else
                {
                    return RedirectToPage("CaptureSuccess", activeDisastersModel);
                }
            }
        }
    }
}


