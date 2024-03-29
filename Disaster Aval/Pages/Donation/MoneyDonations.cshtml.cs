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
    public class MoneyDonationsModel : PageModel
    {
        //class for monrtary donation
        [DisplayName("UserID To Remain Anonymous")]
        [Required]
        public int UserID { get; set; }
        [DisplayName("Allocate Money to Disatser, (Specify by DisasterID)")]
        [Required]
        public int DisasterID { get; set; }
        [DisplayName("How Much Are you donating R:")]
        [Required]
        public int Amount { get; set; }
        [DisplayName("Please Confirm Amount R:")]
        [Required]
        public int Confirm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int DisasterId { get; set; }
        public void OnGet()
        {
            
            ViewData["SelectedDisasterName"] = GetDisasterNameById(DisasterId);
        }

        public IActionResult OnPost()
        {
            MoneyDonationsModel MD = new MoneyDonationsModel();
            //  LoginPageModel LV = new LoginPageModel();

            // Getting data from the form
            MD.UserID = int.Parse(Request.Form["UserID"]);
            MD.DisasterID = int.Parse(Request.Form["DisasterID"]);
            MD.Amount = int.Parse(Request.Form["Amount"]);
            MD.Confirm = int.Parse(Request.Form["Confirm"]);


            // LV.ID = int.Parse(Request.Form["ID"]);

            try
            {
                // Database function here
                string ConnectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    // 1. Retrieve the current NewTotalDonationAmount
                    string getCurrentTotalQuery = "SELECT NewTotalDonationAmount FROM DAF_Donations WHERE DisasterID = @DisasterID";
                    SqlCommand getCurrentTotalCmd = new SqlCommand(getCurrentTotalQuery, conn);
                    getCurrentTotalCmd.Parameters.AddWithValue("@DisasterID", MD.DisasterID);

                    // Execute the query and retrieve the current total
                    decimal currentTotal = Convert.ToDecimal(getCurrentTotalCmd.ExecuteScalar());

                    // 2. Add the new donation amount to the current total
                    decimal newTotal = currentTotal + MD.Amount;

                    // 3. Update the NewTotalDonationAmount in the database
                    string updateTotalQuery = "UPDATE DAF_Donations SET NewTotalDonationAmount = @NewTotalDonationAmount WHERE DisasterID = @DisasterID";
                    SqlCommand updateTotalCmd = new SqlCommand(updateTotalQuery, conn);
                    updateTotalCmd.Parameters.AddWithValue("@NewTotalDonationAmount", newTotal);
                    updateTotalCmd.Parameters.AddWithValue("@DisasterID", MD.DisasterID);
                    updateTotalCmd.ExecuteNonQuery();

                    // Inserting a new donation and retrieve the DonationID
                    string donationInsertQuery = "INSERT INTO DAF_Donations (UserID, DisasterID, DonationType, DonationAmount, DonationItemID, NewTotalDonationAmount) " +
                        "VALUES (@UserID, @DisasterID, @DonationType, @DonationAmount, @DonationItemID, @NewTotalDonationAmount); " +
                        "SELECT SCOPE_IDENTITY();";

                    SqlCommand donationCmd = new SqlCommand(donationInsertQuery, conn);
                    donationCmd.Parameters.AddWithValue("@UserID", MD.UserID);
                    donationCmd.Parameters.AddWithValue("@DisasterID", MD.DisasterID);
                    donationCmd.Parameters.AddWithValue("@DonationType", "Monetary");
                    donationCmd.Parameters.AddWithValue("@DonationAmount", MD.Amount);
                    donationCmd.Parameters.AddWithValue("@DonationItemID", DBNull.Value);
                    donationCmd.Parameters.AddWithValue("@NewTotalDonationAmount", newTotal);

                    // Executing the query and retrieve the newly generated DonationID
                    int donationID = Convert.ToInt32(donationCmd.ExecuteScalar());

                    return RedirectToPage("DonationSuccess");
                }

            }
            catch (Exception ex)
            {
                return RedirectToPage("DonationFailed");
            }
        }
        private string GetDisasterNameById(int disasterId)
        {
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string query = "SELECT Name FROM DAF_Disasters WHERE DisasterID = @DisasterId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DisasterId", disasterId);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }
                }
            }

            // Return a default value or handle the case where the disaster name is not found
            return "Unknown Disaster";
        }
    }
}