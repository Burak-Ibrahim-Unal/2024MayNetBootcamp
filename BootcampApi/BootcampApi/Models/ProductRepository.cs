using BootcampApi.Models;

namespace BootcampApi.Models
{
    public class ProductRepository
    {
        private readonly List<Product> _products =
        [
            new Product { Id = 1,Name="Prod1",Price=10},
            new Product { Id = 2,Name="Prod2",Price=20},
            new Product { Id = 3,Name="Prod3",Price=30 }
        ];

        //1.Yol
        //public IReadOnlyList<Product> Products()
        //{
        //    return _products;
        //}

        //2.Yol
        public IReadOnlyList<Product> GetProducts() => _products;

        public Product? GetById(int id) => _products.Find(x => x.Id == id);

        public void Delete(int id)
        {
            var product= GetById(id);

            _products.Remove(product!); //! null olamaz demek
        }
    }
}