﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public DateTime Created { get; set; } = new();

        public string Barcode { get; init; } = default!;

        public int Stock { get; set; }
    }
}
