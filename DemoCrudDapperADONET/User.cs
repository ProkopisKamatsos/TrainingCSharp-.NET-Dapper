using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCrudDapperADONET
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;

    }
}
