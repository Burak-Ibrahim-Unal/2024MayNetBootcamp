﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Service.ProductService.DTOs
{
    public record ProductCreateRequestDto(string Name, decimal Price);
}
