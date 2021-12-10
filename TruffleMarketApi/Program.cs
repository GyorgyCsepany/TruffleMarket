using Microsoft.EntityFrameworkCore;
using TruffleMarketApi.Database;
using TruffleMarketApi.Database.Models;
using TruffleMarketApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

var connectionString = builder.Configuration.GetConnectionString("TruffleMarketDb");
builder.Services.AddDbContext<TruffleMarketDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/items", async (TruffleMarketDbContext db) =>
    await db.Items.ToListAsync());

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
