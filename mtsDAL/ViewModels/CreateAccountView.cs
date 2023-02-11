using mtsDAL.Data;
using mtsDAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtsDAL.ViewModels
{
    public class CreateAccountView
    {
        public string HolderUserId { get; set; }
        public string BankName { get; set; } = "Pezesha";
        public string BranchName { get; set; }
        public string SwiftCode { get; set; }
        public string AccountType { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Balance { get; set; }


        public static explicit operator CreateAccountView(Account v)
        {
            try
            {
                var model = new CreateAccountView
                {
                    HolderUserId = v.HolderUserId,
                    BankName = v.BankName,
                    BranchName = v.BranchName,
                    SwiftCode = v.SwiftCode,
                    AccountType = Enum.GetName(typeof(AccountTypes), v.AccountType),
                    Balance = v.Balance
                };
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
