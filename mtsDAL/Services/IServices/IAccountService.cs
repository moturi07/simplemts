using mtsDAL.ViewModels;
using System.Threading.Tasks;

namespace mtsDAL.Services.IServices
{
    public interface IAccountService
    {
        Task<ListResult<AccountView>> GetAccountDataAsync();
        Task<int> SaveAccountDataAsync(CreateAccountView model);
        Task<ListResult<AccountView>> GetAccountDataAsync(int id);
        Task<TransactionView> CreateTransactionAsync(CreateTransaction model);
    }
}
