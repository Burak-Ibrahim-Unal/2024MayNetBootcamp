using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.ApplicationService.ProductService.Service
{
    public interface IProductService
    {
        Task UpdateProductName(string name, int id);
    }
}
