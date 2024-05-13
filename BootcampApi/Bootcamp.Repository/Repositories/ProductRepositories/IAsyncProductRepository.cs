using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Repository.Repositories.ProductRepositories
{
    public interface IAsyncProductRepository : IGenericRepository<Product>
    {
        Task UpdateProductName(string name, int id);
    }
}
