using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruffleMarketApi.Database;
using TruffleMarketApi.Services.Authentication;
using TruffleMarketApi.Services.Item;
using TruffleMarketApi.Services.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.Configure<JwtAuthenticationConfig>(builder.Configuration.GetSection("JwtAuthentication"));

var connectionString = builder.Configuration.GetConnectionString("TruffleMarketDb");
builder.Services.AddDbContext<TruffleMarketDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IItemService, ItemService>();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();


app.MapPost("/user", async (UserLoginOrRegisterModel loginOrRegisterModel, IUserService userService) =>
{
    var user = await userService.LoginOrRegister(loginOrRegisterModel);
    return user is null ? Results.StatusCode(401) : Results.Ok(user);
});

app.MapGet("/items", async ([FromQuery(Name = "FilterTruffle")] string filterTruffle,
                            [FromQuery(Name = "sortField")] string sortField,
                            [FromQuery(Name = "sortType")] string sortType,
                            [FromQuery(Name = "page")] int page,
                            [FromQuery(Name = "perPage")] int perPage,
                            IItemService itemService ) =>
    await itemService.GetItemsForGrid(filterTruffle, sortField, sortType, page, perPage));

app.MapGet("/items/{itemId}", async (int itemId, IItemService itemService) =>
{
    var item = await itemService.GetItemInfo(itemId);
    return item is null ? Results.NotFound() : Results.Ok(itemId);
});

app.MapPut("/items/{itemId}", async (ItemBidModel bidModel, IItemService itemService) =>
{
    var itemId = await itemService.BidforItem(bidModel);
    return itemId is null ? Results.NotFound() : Results.Ok(itemId);
}).RequireAuthorization();

app.MapPost("/item", async (ItemCreateModel model, IItemService itemService) =>
    await itemService.Offer(model)).RequireAuthorization();

app.MapPut("/items/batch", async (ItemBatchModel butchModel, IItemService itemService) =>
{
    var model = await itemService.BatchBid(butchModel);
    return model is null ? Results.NotFound() : Results.Ok(model);
}).RequireAuthorization();

app.MapGet("/user/bids", async (IItemService itemService) =>
    await itemService.GetItemsForBuyer()).RequireAuthorization();

app.MapGet("/user/offers", async (IItemService itemService) =>
    await itemService.GetItemsForSeller()).RequireAuthorization();

app.MapPut("/user/bid/close", async (BidCloseModel closeModel, IItemService itemService) =>
    await itemService.CloseBid(closeModel)).RequireAuthorization();

app.MapPut("/user/offer/close", async (OfferCloseModel closeModel, IItemService itemService) =>
    await itemService.CloseOffer(closeModel)).RequireAuthorization();

app.Run();
