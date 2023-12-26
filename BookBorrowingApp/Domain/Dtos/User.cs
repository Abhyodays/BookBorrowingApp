using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class User
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int tokens { get; set; }
    }
}
