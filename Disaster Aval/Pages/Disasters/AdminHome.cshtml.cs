using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Disaster_Aval.Pages.Disasters
{

    //  a service interface
    public interface IAdminService
    {
        List<string> GetActiveDisasters();
        
    }

    // Implementing the service
    public class AdminService : IAdminService
    {
        // Implementing the methods based on your requirements
        public List<string> GetActiveDisasters()
        {
            // Logic to retrieve active disasters from the database or any other source
            return new List<string> { "Disaster1", "Disaster2" };
        }
    }

    
    public class AdminHomeModel : PageModel
    {
        private readonly IAdminService adminService;

        public AdminHomeModel(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public List<string> ActiveDisasters { get; set; }

        public void OnGet()
        {
            // Retrieve active disasters using the injected service
            ActiveDisasters = adminService.GetActiveDisasters();
        }
    }
}
