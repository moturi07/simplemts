using mtsDAL.Data;
using mtsDAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mtsDAL.ViewModels
{
    public class AccountView
    {
        public int Id { get; set; }
        public string AccountHolderId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get => !string.IsNullOrEmpty(UpdatedBy) ? (DateTime?)DateTime.Now : null; }
        public DateTime? ApprovedOn { get => !string.IsNullOrEmpty(ApprovedBy) ? (DateTime?)DateTime.Now : null; }
        public bool Active { get; set; } = true;
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string SwiftCode { get; set; }
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Balance { get; set; }


        public static explicit operator AccountView(Account acc)
        {
            try
            {
                var model = new AccountView
                {
                    Id = acc.Id,
                    CreatedOn = acc.CreatedOn,
                    CreatedBy = acc.CreatedBy,
                    ApprovedBy = acc.ApprovedBy,
                    BankName = acc.BankName,
                    BranchName = acc.BranchName,
                    SwiftCode = acc.SwiftCode,
                    AccountNumber = acc.AccountNumber,
                    AccountType = Enum.GetName(typeof(AccountTypes), acc.AccountType),
                    Balance = acc.Balance
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
