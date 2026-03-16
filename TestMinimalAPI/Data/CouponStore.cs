using System;
using TestMinimalAPI.Models;

namespace TestMinimalAPI.Data;

public static class CouponStore
{
    public static List<Coupon> couponList = new List<Coupon>
    {
        new Coupon{Id=1 , IsActive=true , Name="10% off" , Percent=10 , Created=DateTime.Now , LastUpdated=DateTime.Now},
        new Coupon{Id=2 , IsActive=false , Name="20% off" , Percent=20 , Created=DateTime.Now , LastUpdated=DateTime.Now},
        new Coupon{Id=3 , IsActive=true , Name="30% off" , Percent=30 , Created=DateTime.Now , LastUpdated=DateTime.Now},
    };
}
