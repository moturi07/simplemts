# simplemts
This is a sample REST API that implements a simple money transfer between accounts feature.
The API has the following endpoints:
1. POST /accounts: creates a new account with a starting balance.
2. GET /accounts/{id}: retrieves the account information.
4. POST /transfers: transfers money from one account to another.

The API has also implementede the following checks:
1. An account can't have a negative balance.
2. The transfer amount must be greater than zero.
3. If the source account doesn't have enough funds, the transfer should fail.
4. The API should be able to handle concurrent requests and avoid race conditions.

The API uses sqlite database to store the accounts information and transactions which does not require any installation for it to run successifully.


To run the application please perform the steps below
1. Open powershell
2. Navigate to the project folder
3. build your application by running dotnet build command
4. run your application by dotnet run command
5. Your application should now run preferrably in "https://localhost:5001;http://localhost:5000"