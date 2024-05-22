using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.ApplicationService.ProductService.DTOs
{
    public record ProductNameUpdateRequestDto(Guid Id, string Name);
}
