using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.Domain.Entities
{
    public class Product : BaseEntity<Guid>
    {
        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public DateTime Created { get; set; } = new();

        public string Barcode { get; init; } = default!;

        public int Stock { get; set; }
    }
}
