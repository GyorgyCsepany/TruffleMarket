using Microsoft.EntityFrameworkCore;
using TruffleMarketApi.Database;
using TruffleMarketApi.Database.Models;
using TruffleMarketApi.Models;
using TruffleMarketApi.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.Configure<JwtAuthenticationConfig>(builder.Configuration.GetSection("JwtAuthentication"));

var connectionString = builder.Configuration.GetConnectionString("TruffleMarketDb");
builder.Services.AddDbContext<TruffleMarketDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapPost("/login", async (UserModel userModel, TruffleMarketDbContext db, IJwtTokenService jwtTokenService) => {

    var user = await db.Users.FirstOrDefaultAsync(u => u.Name == userModel.Name && u.Password == userModel.Password);

    var userWithToken = jwtTokenService.GetUserWithToken(user);
    return userWithToken is null ? Results.StatusCode(401) : Results.Ok(userWithToken);
});

app.MapPost("/register", async (UserModel userModel, TruffleMarketDbContext db, IJwtTokenService jwtTokenService) => {

    db.Users.Add(userModel);
    await db.SaveChangesAsync();

    var userWithToken = jwtTokenService.GetUserWithToken(userModel);
    return userWithToken;
});

app.MapPost("/items", async (ItemsGridRequest gridRequest, TruffleMarketDbContext db) =>
{
    var filteredTruffle = gridRequest.ColumnFilters?.Truffle;
    var queryable = db.Items.Where(item => string.IsNullOrEmpty(filteredTruffle) || item.Truffle == filteredTruffle);

    var totalCount = await queryable.CountAsync();


    if (gridRequest.Sort != null && gridRequest.Sort.FirstOrDefault().Type != "none")
    {
        var sortProperty = gridRequest.Sort.FirstOrDefault().Field;
        var formattedSortProperty = Char.ToUpperInvariant(sortProperty[0]) + sortProperty[1..];

        if (gridRequest.Sort.FirstOrDefault().Type == "desc")
        { 
            queryable = queryable.OrderByDescending(item => EF.Property<Item>(item, formattedSortProperty));
        }else
        {
            queryable = queryable.OrderBy(item => EF.Property<Item>(item, formattedSortProperty));
        }
    }

    if (totalCount > gridRequest.PerPage)
    {
        queryable = queryable
            .Skip((gridRequest.Page - 1) * gridRequest.PerPage)
            .Take(gridRequest.PerPage);
    }

    var items = await queryable.ToListAsync();

    return new ItemsGridResponse
    {
        TotalRows = totalCount,
        Rows = items
    };
});

app.Run();
