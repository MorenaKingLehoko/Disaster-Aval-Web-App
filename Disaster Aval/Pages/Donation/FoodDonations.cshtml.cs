using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Donation
{
    public class FoodDonationsModel : PageModel
    {




        [DisplayName("Food Brand or Name")]
        [Required]
        public string NumberOfItems { get; set; }


        [DisplayName("Category, canned or microwavable")]
        [Required]
        public string Category { get; set; }


        [DisplayName("Brief Description")]
        public string Desription { get; set; }
        [DisplayName("Disaster ID")]
        public int DisasterID { get; set; }
        [DisplayName("UserID to remain Anonymous")]
        public int UserID { get; set; }


        public IActionResult OnPost()
        {
            ClothesDonationModel GD = new ClothesDonationModel();
            //  LoginPageModel LV = new LoginPageModel();

            // Getting data from the form
            GD.NumberOfItems = Request.Form["NumberOfItems"];
            GD.Category = Request.Form["Category"];
            GD.Desription = Request.Form["Desription"];
            GD.DisasterID = int.Parse(Request.Form["DisasterID"]);
            GD.UserID = int.Parse(Request.Form["UserID"]);

            // LV.ID = int.Parse(Request.Form["ID"]);


            // Database function here
            string ConnectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

               
                //  Inserting a new donation and retrieve the DonationID
                string donationInsertQuery = "INSERT INTO DAF_Donations (UserID, DisasterID, DonationType, DonationAmount, DonationItemID) " +
                    "VALUES (@UserID, @DisasterID, @DonationType, @DonationAmount, @DonationItemID); " +
                    "SELECT SCOPE_IDENTITY();";

                SqlCommand donationCmd = new SqlCommand(donationInsertQuery, conn);
                donationCmd.Parameters.AddWithValue("@UserID", GD.UserID);
                donationCmd.Parameters.AddWithValue("@DisasterID", GD.DisasterID);
                donationCmd.Parameters.AddWithValue("@DonationType", "Non-Perishable Foods");
                donationCmd.Parameters.AddWithValue("@DonationAmount", DBNull.Value);
                donationCmd.Parameters.AddWithValue("@DonationItemID", DBNull.Value);

                // Executing the query and retrieve the newly generated DonationID
                int donationID = Convert.ToInt32(donationCmd.ExecuteScalar());



                // Inserting each donated item with a placeholder DonationItemID
                //  string insertQuery = "INSERT INTO DAF_Items VALUES ('" + GD.NumberOfItems + "','" + GD.Desription + "','" + GD.Category + "')";
                string insertItemQuery = "INSERT INTO DAF_Items (ItemName, ItemDescription, ItemCategory, DonationID) VALUES (@ItemName, @ItemDescription, @ItemCategory, @DonationID); SELECT SCOPE_IDENTITY()";
                SqlCommand itemCmd = new SqlCommand(insertItemQuery, conn);
                itemCmd.Parameters.AddWithValue("@ItemName", GD.NumberOfItems); 
                itemCmd.Parameters.AddWithValue("@ItemDescription", GD.Desription); 
                itemCmd.Parameters.AddWithValue("@ItemCategory", GD.Category);
                itemCmd.Parameters.AddWithValue("@DonationID", donationID);

                //  Retrieving the newly generated ItemID
                int itemID = Convert.ToInt32(itemCmd.ExecuteScalar());




                //  Updating the DAF_Donations record with the obtained DonationItemID
                string updateDonationQuery = "UPDATE DAF_Donations SET DonationItemID = @ItemID WHERE DonationID = @DonationID";
                SqlCommand updateDonationCmd = new SqlCommand(updateDonationQuery, conn);
                updateDonationCmd.Parameters.AddWithValue("@ItemID", itemID);
                updateDonationCmd.Parameters.AddWithValue("@DonationID", donationID);
                updateDonationCmd.ExecuteNonQuery();



                conn.Close();

                // Redirecting to the success page or handle success as needed
                return RedirectToPage("DonationSuccess");
            }
        }
        //catch (Exception ex)
        //{
        //    // Handle exceptions and errors
        //    return RedirectToPage("LoginFailed", GD);
        //}
    }

}

