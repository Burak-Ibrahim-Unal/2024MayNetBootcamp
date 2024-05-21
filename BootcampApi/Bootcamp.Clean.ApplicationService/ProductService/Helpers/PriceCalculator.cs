using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.ApplicationService.ProductService.Helpers
{
    public class PriceCalculator
    {
        public decimal CalculateKdv(decimal price, decimal tax)
        {
            return price * tax;
        }
    }
}
