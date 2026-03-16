using System;

namespace TestMinimalAPI.DTOs;

   public class CouponCreateDTO
    {
        
        public string Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
    }
