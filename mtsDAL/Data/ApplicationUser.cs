using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace mtsDAL.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string IdentificationNumber { get; set; }
        public string KRAPin { get; set; }
    }
}
