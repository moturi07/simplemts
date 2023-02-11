using mtsDAL.Data;
using mtsDAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mtsDAL.ViewModels
{
    public class TransactionView
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool Active { get; set; } = true;
        public string ReferenceNumber { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public string SenderBankName { get; set; } = "Pezesha";
        public string SenderBranchName { get; set; }
        public string SenderBankSwiftCode { get; set; }
        public int SenderAccountNumber { get; set; }
        public decimal SenderAmount { get; set; }        

        public decimal Charges { get; set; } = 0;
        public decimal Tarrif { get; set; } = 0;
        public int Tax { get; set; } = 0;
        public string ReceiverBankName { get; set; } = "Pezesha";
        public string ReceiverBranchName { get; set; }
        public string ReceiverBankSwiftCode { get; set; }
        public int ReceiverAccountNumber { get; set; }

        [Range(10.0, Double.MaxValue, ErrorMessage = "The Receiver Amount must be greater than {1}.")]
        public decimal ReceiverAmount { get; set; }

        public int AccountId { get; set; }

        public bool HasError { get => !string.IsNullOrEmpty(ErrorMessage); }

        public string ErrorMessage { get; set; }


        public static explicit operator TransactionView(CreateTransaction v)
        {
            try
            {
                var model = new TransactionView
                {
                    SenderAccountNumber = v.SenderAccountNumber,
                    SenderAmount = v.SenderAmount,
                    SenderBankName = v.SenderBankName,
                    SenderBranchName = v.SenderBranchName,
                    SenderBankSwiftCode = v.SenderBankSwiftCode,
                    Charges  = v.Charges,
                    Tarrif = v.Tarrif,
                    Tax = v.Tax,
                    ReceiverAccountNumber = v.ReceiverAccountNumber,
                    ReceiverBankName = v.ReceiverBankName,
                    ReceiverBranchName = v.ReceiverBranchName,
                    ReceiverBankSwiftCode = v.ReceiverBankSwiftCode,
                };
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static explicit operator TransactionView(Transaction v)
        {
            try
            {
                var model = new TransactionView
                {
                    Id = v.Id,
                    CreatedOn = v.CreatedOn,
                    CreatedBy = v.CreatedBy,
                    ApprovedBy = v.ApprovedBy,
                    SenderAccountNumber = v.SenderAccountNumber,
                    SenderAmount = v.SenderAmount,
                    SenderBankName = v.SenderBankName,
                    SenderBranchName = v.SenderBranchName,
                    SenderBankSwiftCode = v.SenderBankSwiftCode,
                    Charges = v.Charges,
                    Tarrif = v.Tarrif,
                    Tax = v.Tax,
                    ReceiverAccountNumber = v.ReceiverAccountNumber,
                    ReceiverAmount = v.ReceiverAmount,
                    ReceiverBankName = v.ReceiverBankName,
                    ReceiverBranchName = v.ReceiverBranchName,
                    ReceiverBankSwiftCode = v.ReceiverBankSwiftCode,
                    ReferenceNumber = v.ReferenceNumber
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
