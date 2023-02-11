
using mtsDAL.Data;
using mtsDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mtsDAL.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string HolderUserId { get; set; }
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
        public AccountTypes AccountType { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Balance { get; set; }

        public ICollection<Transaction> Transaction { get; set; }


        public static explicit operator Account(CreateAccountView v)
        {
            try
            {
                var model = new Account
                {
                    HolderUserId = v.HolderUserId,
                    BankName = v.BankName,
                    BranchName = v.BranchName,
                    SwiftCode = v.SwiftCode,
                    AccountType = !string.IsNullOrEmpty(v.AccountType) ? Enum.Parse<AccountTypes>(v.AccountType.Replace(' ', '_')) : AccountTypes.Current,
                   
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
