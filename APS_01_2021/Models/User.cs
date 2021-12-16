using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public DateTime BornDate { get; set; }
        public string Email { get; set; }
    }
}
