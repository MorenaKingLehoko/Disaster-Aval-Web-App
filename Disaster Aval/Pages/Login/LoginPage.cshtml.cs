using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
namespace Disaster_Aval.Pages.Login
{
    public class LoginPageModel : PageModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public static string UserID;
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            LoginPageModel loginViewModel = new LoginPageModel(); 
            // a new instance of LoginViewModel

            //getting data from the form

           
            loginViewModel.Name = Request.Form["Name"];
            loginViewModel.Surname = Request.Form["Surname"];
            loginViewModel.Password = Request.Form["Password"];

            //database function here
            string ConnectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlCommand cmd = new SqlCommand();

            //from poe
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string Insert = "SELECT Username, Surname, Password FROM DisasterAval_Users WHERE Username ='" + loginViewModel.Name + "'and Surname ='" + loginViewModel.Surname + "'and Password ='" + loginViewModel.Password + "'";
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
                        return RedirectToPage("LoginSucces", loginViewModel);
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
        }
    }
}


                    //selecting the name and ID from the database where the name matches the data entered on the form

//Insert into <Table> what i get from the form
//           if (loginViewModel.ID== 1 && loginViewModel.Name == "Morena" && loginViewModel.Surname == "Lehoko" && loginViewModel.Password == "1234")         {



////                // Returning the LoginSuccess page if the login was successful and passing in the model to display the details
//               return RedirectToPage("LoginSucces", loginViewModel);
//           }
//           else
//           {
//               // If the login was not successful, you might want to return an error view or redirect to a different page.
//             return RedirectToPage("LoginFailed", loginViewModel);
//            }
//        }
//    }
//}
