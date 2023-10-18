using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Login
{
    public class AdminLoginModel : PageModel
    {
        [DisplayName("Your Admin ID:")]
        [Required]
        public int AdminId { get; set; }


        [DisplayName("Password")]
        [Required]
        public string AdminPassword { get; set; }


        [DisplayName("Email")]
        public string AdminEmail { get; set; }

        [DisplayName("Admin Name")]
        public string AdminName { get; set; }
        [DisplayName("Surname")]
        public string AdminSurname { get; set; }
        public static string UserID;


        //public void OnGet()
        //{
        //}
        public IActionResult OnPost()
        {
            AdminLoginModel loginViewModel = new AdminLoginModel(); // a new instance of LoginViewModel

            //getting data from the form

            //converting to int

            //loginViewModel.AdminId = int.Parse(Request.Form["AdminId"]);
            loginViewModel.AdminEmail = Request.Form["AdminEmail"];
            loginViewModel.AdminPassword =  Request.Form["AdminPassword"];
            loginViewModel.AdminName = Request.Form["AdminName"];
            loginViewModel.AdminSurname = Request.Form["AdminSurname"];

            //database function here
            string ConnectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlCommand cmd = new SqlCommand();

            //from poe
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string Insert = "SELECT Name, Email, Password FROM DAF_USERS WHERE Name ='" + loginViewModel.AdminName + "'and Email ='" + loginViewModel.AdminEmail + "'and Password ='" + loginViewModel.AdminPassword + "'";
                    cmd = new SqlCommand(Insert, conn);
                    SqlDataAdapter Da = new SqlDataAdapter(Insert, conn);
                    DataTable dt = new DataTable();
                    Da.Fill(dt);
                    //assigning the userId Variable to the [0][0] index result from the databse using the datatable
                    UserID = dt.Rows[0][0].ToString();
                    cmd.ExecuteNonQuery();
                    SqlDataReader read = cmd.ExecuteReader();

                    //validting if the query ran succesfully user the Datareader and displaying pages accordinly
                    if (read.HasRows)
                    {
                        //if the query ran succesfully this page will be called
                        return RedirectToPage("/Disasters/AdminHome");
                    }
                    else
                    {
                        //other wise this page will be called
                        return RedirectToPage("LoginFailed", loginViewModel);
                    }
                }
            }
            catch (Exception)
            {
                //if any unhandled exeption is caught this page will be called
                return RedirectToPage("LoginFailed", loginViewModel);

            }
























            //database function here

            //string ConnectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            //SqlCommand cmd = new SqlCommand();
            //using (SqlConnection conn = new SqlConnection(ConnectionString))
            //{
            //    conn.Open();
            //    string insertQuery = "INSERT INTO DAF_USERS VALUES ('" + loginViewModel.AdminName + "','" + loginViewModel.AdminEmail + "','" + loginViewModel.AdminPassword + "','" + loginViewModel.AdminId + "','" + loginViewModel.AdminSurname + "')";
            //    cmd = new SqlCommand(insertQuery, conn);
            //    int a = cmd.ExecuteNonQuery();
            //    conn.Close();
            //    if (a == 0)
            //    {
            //        return RedirectToPage("LoginFailed", loginViewModel);
            //    }
            //    else
            //    {
            //        return RedirectToPage("/Disasters/AdminHome");
            //    }
        }
        }
    }


