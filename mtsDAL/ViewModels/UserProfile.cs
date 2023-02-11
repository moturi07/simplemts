using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mtsDAL.ViewModels
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
}
