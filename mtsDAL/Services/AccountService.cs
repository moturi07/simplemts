using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mtsDAL.Data;
using mtsDAL.Models;
using mtsDAL.Services.IServices;
using mtsDAL.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace mtsDAL.Services
{
    public class AccountService : IAccountService
    {
        private static Random random = new Random();
        private readonly ApplicationDbContext _appdbcontext;
        private readonly ILogger<AccountService> _logger;
        public AccountService(ILogger<AccountService> logger, ApplicationDbContext appdbcontext)
        {
            _appdbcontext = appdbcontext;
            _logger = logger;
        }

        /// <summary>
        ///  Method to create a transaction
        /// </summary>
        public async Task<TransactionView> CreateTransactionAsync(CreateTransaction transaction)
        {
            try
            {
                _logger.LogInformation("Create Transaction {@model}", transaction);
                string error = string.Empty;

                //create transaction response
                var _transactionview = new TransactionView();

                //check if the transaction you are about to create is null or not
                if (transaction != null)
                {
                    //Compute transaction charges
                    decimal totalcharges = ((transaction.SenderAmount * transaction.Tax) / 100) + (transaction.SenderAmount * (transaction.Tarrif / 100)) + transaction.Charges;
                    //compute totoal amount
                    decimal totalamount = transaction.SenderAmount + totalcharges;
                    //check if transaction checks are all valid
                    error = TransactionIsValid(transaction);
                    //get the sender details
                    var sender_data = await _appdbcontext.Accounts.Where(q => q.AccountNumber == transaction.SenderAccountNumber && q.Active).AsQueryable().AsNoTracking().FirstOrDefaultAsync();
                    if (sender_data == null)
                    {
                        error = "The sender account does not exist, is dormant or suspended";
                    }
                    else if (totalamount > sender_data.Balance)
                    {
                        error = "The sender balance is insufficient";
                    }

                    //get the receiver details
                    var receiver_data = await _appdbcontext.Accounts.Where(q => q.AccountNumber == transaction.ReceiverAccountNumber && q.Active).AsQueryable().AsNoTracking().FirstOrDefaultAsync();
                    if (receiver_data == null)
                    {
                        error = "The Receiver account does not exist, is dormant or suspended";
                    }

                    //if all transaction checks are okay, we proceed with the transaction
                    if (string.IsNullOrEmpty(error))
                    {
                        var _transaction = (Transaction)transaction;
                        _transaction.ReferenceNumber = GenerateReferenceNumber(7);
                        _transaction.ReceiverAmount = transaction.SenderAmount - transaction.Charges;
                        //_transaction.CreatedBy = userId;
                        _appdbcontext.Transactions.Add(_transaction);    
                        //check if transaction is created
                        int rowsAffected = await _appdbcontext.SaveChangesAsync();
                        if (rowsAffected > 0)
                        {
                            //debit sender account and update the database
                            await UpdateAccountBalance(transaction.SenderAccountNumber, sender_data.Balance - transaction.SenderAmount);
                            //credit receiver details and update the data
                            await UpdateAccountBalance(transaction.ReceiverAccountNumber, (receiver_data.Balance + transaction.SenderAmount)-transaction.Charges);
                            //return the transaction details
                            _transactionview = (TransactionView)_transaction;
                        }
                        else
                        {
                            _transactionview.ErrorMessage = "Unable to create the Transaction";
                        }                        
                    }
                    else
                    {
                        _transactionview.ErrorMessage = error;
                    }
                }
                else
                {
                    _transactionview.ErrorMessage = "Transaction Details Missing";
                }
                return _transactionview;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error Creating Transaction ", ex.Message);
                throw new Exception("Error: " + ex);
            }
        }

        /// <summary>
        ///  Method to get account details by id
        /// </summary>
        public async Task<ListResult<AccountView>> GetAccountDataAsync(int id)
        {
            try
            {
                var listResult = new ListResult<AccountView>();
                var query = _appdbcontext.Accounts.Where(q => q.AccountNumber == id);
                var _accountView = new AccountView();
                var total = await query.CountAsync();

                var items = await query
                .Skip((0) * 20)
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync();

                if (items?.Count > 0)
                    listResult.Items = items.Select(s => (AccountView)s).ToList();

                listResult.PaginationInfo = new PaginationInfo
                {
                    CurrentPage = 1,
                    PageSize = 20,
                    Total = total
                };
                return listResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  Method to Create an account
        /// </summary>
        public async Task<int> SaveAccountDataAsync(CreateAccountView model)
        {
            try
            {
                _logger.LogInformation("Create Account {@model}", model);                
                var _model = new Account();
                _model = (Account)model;
                _model.AccountNumber = (int)GenerateAccountNumber(9);
                //_model.CreatedBy = userId;
                _appdbcontext.Accounts.Add(_model);
                await _appdbcontext.SaveChangesAsync();
                return _model.AccountNumber;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Create Account error", ex.Message);
                throw new Exception("Error: " + ex);
            }
        }


        /// <summary>
        ///  Private Method to check generate account numbers
        /// </summary>
        public static long GenerateAccountNumber(int length)
        {
            const string chars = "0123456789";
            var sd = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return long.Parse(sd);
        }

        /// <summary>
        ///  Private Method to check generate transaction reference
        /// </summary>
        private static string GenerateReferenceNumber(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        ///  Private Method to check validity of a transaction
        /// </summary>
        private string TransactionIsValid(CreateTransaction transaction)
        {
            try
            {
                if (transaction.SenderAmount<1)
                    return "The sender amount should be more than 0.";
                if (transaction.SenderAccountNumber<1)
                    return "The sender Account Number is missing.";
                if (String.IsNullOrEmpty(transaction.SenderBankSwiftCode))
                    return "The sender Bank is missing.";
                if (transaction.ReceiverAccountNumber<1)
                    return "The Receiver Account.";
                if (String.IsNullOrEmpty(transaction.ReceiverBankSwiftCode))
                    return "The Receiver Bank is missing.";
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.GetBaseException().Message;
            }
        }

        /// <summary>
        ///  Private Method to update account balance given a specific id with the given amount
        /// </summary>
        private async Task<string> UpdateAccountBalance(int AccountNumber, decimal amount)
        {
            try
            {
                var data = await _appdbcontext.Accounts.Where(q => q.AccountNumber == AccountNumber && q.Active).AsQueryable().AsNoTracking().FirstOrDefaultAsync();
                if (data == null)
                {
                    return "The Receiver account does not exist";
                }
                else
                {
                    data.Balance = amount;
                    _appdbcontext.Accounts.Update(data);
                    int rwsa = await _appdbcontext.SaveChangesAsync();
                    return null;
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.GetBaseException().Message;
            }
        }

        /// <summary>
        ///  Method to get all accounts
        /// </summary>
        public async Task<ListResult<AccountView>> GetAccountDataAsync()
        {
            try
            {
                var listResult = new ListResult<AccountView>();
                var query = _appdbcontext.Accounts;
                var _accountView = new AccountView();
                var total = await query.CountAsync();

                var items = await query
                .Skip((0) * 20)
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync();

                if (items?.Count > 0)
                    listResult.Items = items.Select(s => (AccountView)s).ToList();

                listResult.PaginationInfo = new PaginationInfo
                {
                    CurrentPage = 1,
                    PageSize = 20,
                    Total = total
                };
                return listResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
