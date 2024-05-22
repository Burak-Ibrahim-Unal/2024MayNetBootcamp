using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.ApplicationService.ProductService.DTOs
{
    public record ProductDto(Guid Id, string Name, decimal Price,string Barcode, string Created);
}
