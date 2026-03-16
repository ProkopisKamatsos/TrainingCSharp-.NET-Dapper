using System;

namespace TestMinimalAPI.DTOs;

   public class CouponUpdateDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Percent { get; set; }
        public bool IsActive { get; set; }
    }
