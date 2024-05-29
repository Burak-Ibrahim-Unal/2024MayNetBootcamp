using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Repository.Token
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public DateTime Expire { get; set; }
        public Guid UserId { get; set; }
    }
}
