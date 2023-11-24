using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Login
{
    public class LoginViewModel : PageModel
    {

        [DisplayName("ID")]
        [Required]
        public int ID { get; set; }

        [DisplayName("Your Name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Surname")]
        [Required]
        public string Surname { get; set; }

        [DisplayName("Strong Password")]
        [Required]
        public string Password { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }



        
        public IActionResult OnPost()
        {
            LoginViewModel loginViewModel = new LoginViewModel(); // a new instance of LoginViewModel

            //getting data from the form

            //converting to int
            loginViewModel.ID = int.Parse(Request.Form["ID"]);
            loginViewModel.Email = Request.Form["Email"];
            loginViewModel.Name = Request.Form["Name"];
            loginViewModel.Surname = Request.Form["Surname"];
            loginViewModel.Password = Request.Form["Password"];
            try
            {
                //database function here
                string ConnectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                SqlCommand cmd = new SqlCommand();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string insertQuery = "INSERT INTO DAF_USERS VALUES ('" + loginViewModel.Name + "','" + loginViewModel.Email + "','" + loginViewModel.Password + "','" + loginViewModel.ID + "','"+loginViewModel.Surname+"')";
                    cmd = new SqlCommand(insertQuery, conn);
                    int a = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (a == 0)
                    {
                        return RedirectToPage("LoginFailed", loginViewModel);
                    }
                    else
                    {
                        return RedirectToPage("LoginSucces", loginViewModel);
                    }
                }
           

            }catch(Exception ex)
            {
                return RedirectToPage("LoginFailed", loginViewModel);
            }






        }
    }
}


