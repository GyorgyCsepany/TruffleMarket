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


app.MapPost("/user", async (UserLoginOrRegisterModel loginOrRegisterModel, IUserService userService) => {

    var user = await userService.LoginOrRegister(loginOrRegisterModel);
    return user is null ? Results.StatusCode(401) : Results.Ok(user);
});


app.MapPost("/items", async (GridRequestModel gridRequest, IItemService itemService ) =>
    await itemService.GetItemsForGrid(gridRequest));

app.MapPost("/item", async (ItemCreateModel model, IItemService itemService) => 
    await itemService.Offer(model)).RequireAuthorization();

app.Run();
