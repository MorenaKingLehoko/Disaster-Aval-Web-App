using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Disaster_Aval.Pages.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Aval.Pages.Donation
{
    public class GoodsDonationsModel1 : PageModel
    {



        [DisplayName("Item Name")]
        [Required]
        public string NumberOfItems { get; set; }


        [DisplayName("Category, eg Shelter, building material")]
        [Required]
        public string Category { get; set; }


        [DisplayName("Brief Description")]
        public string Desription { get; set; }
        [DisplayName("Allocate Goods to Disatser, (Specify By DisasterID)")]
        [Required]
        public int DisasterID { get; set; }
        [DisplayName("Your Admin ID (15)")]
        public int UserID { get; set; }


        public IActionResult OnPost()
        {
            GoodsDonationsModel1 GD = new GoodsDonationsModel1();
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


                // Inserting a new donation and retrieve the DonationID
                string donationInsertQuery = "INSERT INTO DAF_Donations (UserID, DisasterID, DonationType, DonationAmount, DonationItemID) " +
                    "VALUES (@UserID, @DisasterID, @DonationType, @DonationAmount, @DonationItemID); " +
                    "SELECT SCOPE_IDENTITY();";

                SqlCommand donationCmd = new SqlCommand(donationInsertQuery, conn);
                donationCmd.Parameters.AddWithValue("@UserID", GD.UserID);
                donationCmd.Parameters.AddWithValue("@DisasterID", GD.DisasterID);
                donationCmd.Parameters.AddWithValue("@DonationType", "Goods");
                donationCmd.Parameters.AddWithValue("@DonationAmount", DBNull.Value);
                donationCmd.Parameters.AddWithValue("@DonationItemID", DBNull.Value);

                // Executing the query and retrieve the newly generated DonationID
                int donationID = Convert.ToInt32(donationCmd.ExecuteScalar());



                //  Inserting each donated item with a placeholder DonationItemID
                //  string insertQuery = "INSERT INTO DAF_Items VALUES ('" + GD.NumberOfItems + "','" + GD.Desription + "','" + GD.Category + "')";
                string insertItemQuery = "INSERT INTO DAF_Items (ItemName, ItemDescription, ItemCategory, DonationID) VALUES (@ItemName, @ItemDescription, @ItemCategory, @DonationID); SELECT SCOPE_IDENTITY()";
                SqlCommand itemCmd = new SqlCommand(insertItemQuery, conn);
                itemCmd.Parameters.AddWithValue("@ItemName", GD.NumberOfItems);
                itemCmd.Parameters.AddWithValue("@ItemDescription", GD.Desription);
                itemCmd.Parameters.AddWithValue("@ItemCategory", GD.Category);
                itemCmd.Parameters.AddWithValue("@DonationID", donationID);

                //  Retrieve the newly generated ItemID
                int itemID = Convert.ToInt32(itemCmd.ExecuteScalar());



                //  Updating the DAF_Donations record with the obtained DonationItemID
                string updateDonationQuery = "UPDATE DAF_Donations SET DonationItemID = @ItemID WHERE DonationID = @DonationID";
                SqlCommand updateDonationCmd = new SqlCommand(updateDonationQuery, conn);
                updateDonationCmd.Parameters.AddWithValue("@ItemID", itemID);
                updateDonationCmd.Parameters.AddWithValue("@DonationID", donationID);
                updateDonationCmd.ExecuteNonQuery();



                conn.Close();

                // Redirecting to the success page or handle success as needed
                return RedirectToPage("PurchaseGoods");
            }
        }
        //catch (Exception ex)
        //{
        //    // Handle exceptions and errors
        //    return RedirectToPage("LoginFailed", GD);
        //}
    }

}




