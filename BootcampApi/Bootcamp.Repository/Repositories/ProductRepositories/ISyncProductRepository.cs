using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Repository.Repositories.ProductRepositories
{
    public interface ISyncProductRepository
    {
        IReadOnlyList<Product> GetAll();

        IReadOnlyList<Product> GetAllByPage(int page, int pageSize);

        void Update(Product product);

        void Create(Product product);

        Product? GetById(int id);

        void Delete(int id);

        bool IsExists(string productName);
    }
}
