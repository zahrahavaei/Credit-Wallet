using Credit_Wallet.Data;
using Credit_Wallet.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
       
        public UnitOfWork(ApplicationDbContext dbContext)
                       
        {
            _dbContext = dbContext;
          
        }

        public async  Task<int> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new WalletConcurrencyException("The Wallet Is Modified By Another Request", ex);
            }
            catch(DbUpdateException ex)
            {
                throw new DatabaseException("Data base update failed.", ex);
            }
        }
    }
}
