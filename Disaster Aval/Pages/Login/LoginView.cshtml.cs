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

        [DisplayName("Password")]
        [Required]
        public string Password { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }



        //public void OnGet()
        //{
        //    // You can use these properties in the OnGet method or elsewhere as needed
        //    // Initialize with default values if necessary


        //    ID = 0;
        //    Name = "";
        //    Surname = "";
        //    Password = "";
        //    Email = "";
        //}
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
           


                ////Insert into <Table> what i get from the form
                //if (loginViewModel.ID == 1 && loginViewModel.Name == "Morena" && loginViewModel.Surname == "Lehoko" && loginViewModel.Password == "1234")
                //{



                //    // Returning the LoginSuccess page if the login was successful and passing in the model to display the details
                //    return RedirectToPage("LoginSucces", loginViewModel);
                //}
                //else
                //{
                //    // If the login was not successful, you might want to return an error view or redirect to a different page.
                //    return RedirectToPage("LoginFailed", loginViewModel);
                //}
            }catch(Exception ex)
            {
                return RedirectToPage("LoginFailed", loginViewModel);
            }






        }
    }
}


