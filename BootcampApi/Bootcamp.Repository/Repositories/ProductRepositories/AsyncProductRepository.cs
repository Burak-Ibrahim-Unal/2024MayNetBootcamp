using Bootcamp.Repository.Context;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bootcamp.Repository.Repositories.ProductRepositories
{
    public class AsyncProductRepository : GenericRepository<Product>, IAsyncProductRepository
    {
        public AsyncProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task UpdateProductName(string name, int id)
        {
            var product = await GetById(id);
            product!.Name = name;
            await Update(product);
        }
    }
}
