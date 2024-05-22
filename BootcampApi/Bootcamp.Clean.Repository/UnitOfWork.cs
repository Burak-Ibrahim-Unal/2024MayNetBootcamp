using Bootcamp.Clean.ApplicationService;
using Bootcamp.Clean.Repository.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.Repository.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        public void BeginTransaction()
        {
            _transaction = context.Database.BeginTransaction();
        }

        public int Commit()
        {
            return context.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return context.SaveChangesAsync();
        }

        public Task? TransactionCommitAsync()
        {
            return _transaction?.CommitAsync();
        }

        public Task? TransactionRollbackAsync()
        {
            return _transaction?.RollbackAsync();
        }
    }
}
