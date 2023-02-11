using mtsDAL.Data;
using mtsDAL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace mtsDAL.ViewModels
{
    public class CreateTransaction
    {
        public TransactionTypes TransactionType { get; set; }
        public string SenderBankName { get; set; } = "Pezesha";
        public string SenderBranchName { get; set; }
        public string SenderBankSwiftCode { get; set; }
        public int SenderAccountNumber { get; set; }

        [Range(10.0, Double.MaxValue, ErrorMessage = "The Sender Amount must be greater than {1}.")]
        public decimal SenderAmount { get; set; }
        public decimal Charges { get; set; } = 0;
        public decimal Tarrif { get; set; } = 0;
        public int Tax { get; set; } = 0;
        public string ReceiverBankName { get; set; } = "Pezesha";
        public string ReceiverBranchName { get; set; }
        public string ReceiverBankSwiftCode { get; set; }
        public int ReceiverAccountNumber { get; set; }


        public static explicit operator CreateTransaction(Transaction v)
        {
            try
            {
                var model = new CreateTransaction
                {
                    SenderAccountNumber = v.SenderAccountNumber,
                    SenderAmount = v.SenderAmount,
                    SenderBankName = v.SenderBankName,
                    SenderBranchName = v.SenderBranchName,
                    SenderBankSwiftCode = v.SenderBankSwiftCode,
                    Charges = v.Charges,
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
    }
}
