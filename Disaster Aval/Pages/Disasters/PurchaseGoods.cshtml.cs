using Disaster_Aval.Pages.Donation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;

namespace Disaster_Aval.Pages.Disasters
{
    public class PurchaseGoodsModel : PageModel
    {
        [DisplayName("How Much Are you gonna spend on the Purchase?:")]
        public string? amountSpent { get; set; }
        [DisplayName("Which Disaster Account are you Spending from?:")]
        public string? DisasterName { get; set; }
        public class Disaster
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public decimal AvailableMoney { get; set; }
            // public string ItemName { get; set; }



        }

        public List<Disaster> Disasters { get; set; }
        public IActionResult OnPost()
        {
            // for user input for amount spent and disaster name
            decimal amountSpent = Convert.ToDecimal(Request.Form["amountSpent"]);
            string disasterName = Request.Form["DisasterName"];

            //  SQL query to retrieve SUM(Don.DonationAmount) for the specified disaster
            decimal totalDonationAmount = GetDonationAmountForDisaster(disasterName);

            // Subtracting the user-entered amount from the totalDonationAmount
            //100
            decimal newTotalDonationAmount = totalDonationAmount - amountSpent;

            // Updating the database with the new totalDonationAmount for the specified disaster
            UpdateTotalDonationAmountForDisaster(disasterName, newTotalDonationAmount);

            // Redirecting to a success page or perform other actions
            return RedirectToPage("AddItem");
        }

        //this method returns the donation amount for the dissatster
        private decimal GetDonationAmountForDisaster(string disasterName)
        {
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            // Defining my SQL query to retrieve the donation amount for the specified disaster
            string sqlQuery = "SELECT\r\n    max(Don.NewTotalDonationAmount) AS TotalNewDonationAmount\r\nFROM\r\n    [dbo].[DAF_Disasters] D\r\nLEFT JOIN\r\n    [dbo].[DAF_Donations] Don ON D.DisasterID = Don.DisasterID\r\nWHERE\r\n    D.Name = @DisasterName AND Don.NewTotalDonationAmount IS NOT NULL;";


            // Executing the query to get the specific donation amount
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@DisasterName", disasterName);
                connection.Open();
                var result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
                return 0;
            }
        }



        private void UpdateTotalDonationAmountForDisaster(string disasterName, decimal newTotalDonationAmount)
        {
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // Defining my SQL query to update the total donation amount for the specified disaster
            //with the new column NewTotalDonationAmount
            string sqlUpdateQuery = "UPDATE d\r\nSET d.NewTotalDonationAmount = @NewTotalDonationAmount\r\nFROM [dbo].[DAF_Donations] d\r\nJOIN [dbo].[DAF_Disasters] dd ON d.DisasterID = dd.DisasterID\r\nWHERE dd.Name = @DisasterName AND d.DonationType = 'Monetary';\r\n";


            // Execut the query to update the total donation amount
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sqlUpdateQuery, connection))
            {
                command.Parameters.AddWithValue("@NewTotalDonationAmount", newTotalDonationAmount);
                command.Parameters.AddWithValue("@DisasterName", disasterName);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void OnGet()
        {



            //  database connection string
            string connectionString = "Server=tcp:djpromo123.database.windows.net,1433;Initial Catalog=DjPromoDatabase;Persist Security Info=False;User ID=Admin1;Password=Storedghast!68;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //Z point
                //  SQL query to retrieve disaster data
                string sqlQuery = "SELECT\r\n    D.DisasterID,\r\n    D.Name AS DisasterName,\r\n    MAX(Don.NewTotalDonationAmount) AS TotalDonationAmount\r\nFROM\r\n    [dbo].[DAF_Disasters] D\r\nLEFT JOIN\r\n    [dbo].[DAF_Donations] Don ON D.DisasterID = Don.DisasterID\r\nWHERE\r\n    Don.NewTotalDonationAmount IS NOT NULL\r\nGROUP BY\r\n    D.DisasterID, D.Name\r\nORDER BY\r\n    D.Name;";

                //   string sqlQuery = "SELECT\r\n    D.DisasterID,\r\n    D.Name AS DisasterName,\r\n    SUM(Don.DonationAmount) AS TotalDonationAmount\r\nFROM\r\n    [dbo].[DAF_Disasters] D\r\nLEFT JOIN\r\n    [dbo].[DAF_Donations] Don ON D.DisasterID = Don.DisasterID\r\nGROUP BY\r\n    D.DisasterID, D.Name\r\nHAVING\r\n    SUM(Don.DonationAmount) > 0\r\nORDER BY\r\n    D.Name;\r\n";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Disasters = new List<Disaster>();
                        while (reader.Read())
                        {
                            // Mapping database columns to Disaster properties
                            var disaster = new Disaster
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                AvailableMoney = reader.GetDecimal(2),
                                // ItemName = reader.GetString(3),
                            };

                            Disasters.Add(disaster);
                        }
                    }
                }
            }
        }

        

       
    }
}

       