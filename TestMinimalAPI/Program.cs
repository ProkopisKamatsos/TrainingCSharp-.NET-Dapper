using Microsoft.AspNetCore.Mvc;
using TestMinimalAPI.Data;
using TestMinimalAPI.DTOs;
using TestMinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/api/coupon", () => Results.Ok(CouponStore.couponList));
app.MapGet("/api/coupon{id:int}", (int id) =>
{
    return Results.Ok(CouponStore.couponList.FirstOrDefault(c => c.Id == id));
}
);
app.MapPost("/api/coupon", ([FromBody] CouponCreateDTO couponCreateDTO) =>
{
    if (string.IsNullOrEmpty(couponCreateDTO.Name))
    {
        return Results.BadRequest("Invalid Id or Coupon Name");
    }
    if (CouponStore.couponList.FirstOrDefault(u => u.Name.ToLower() == couponCreateDTO.Name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon already exists");
    }
    Coupon coupon = new()
    {
        IsActive = couponCreateDTO.IsActive,
        Name = couponCreateDTO.Name,
        Percent = couponCreateDTO.Percent
    };
    coupon.Id = CouponStore.couponList.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupon);
    return Results.Created($"/api/coupon/{coupon.Id}", coupon);
});
app.MapPut("/api/coupon/{id:int}", (int id, [FromBody] CouponUpdateDTO updateDTO) =>
{
    var existingCoupon = CouponStore.couponList.FirstOrDefault(c => c.Id == id);
    if (existingCoupon is null)
        return Results.NotFound();
    existingCoupon.Name = updateDTO.Name;
    existingCoupon.Percent = updateDTO.Percent;
    existingCoupon.IsActive = updateDTO.IsActive;

    return Results.NoContent();

});
app.MapDelete("/api/coupon/{id:int}", (int id) =>
{
    var coupon = CouponStore.couponList.FirstOrDefault(c => c.Id == id);

    if (coupon is null)
        return Results.NotFound();

    CouponStore.couponList.Remove(coupon);

    return Results.NoContent();
});

app.UseHttpsRedirection();
app.Run();


