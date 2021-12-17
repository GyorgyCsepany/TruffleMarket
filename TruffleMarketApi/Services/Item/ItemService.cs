using Microsoft.EntityFrameworkCore;
using TruffleMarketApi.Database;

namespace TruffleMarketApi.Services.Item
{
    public class ItemService : IItemService
    {
        private readonly TruffleMarketDbContext _dBContext;

        public ItemService(TruffleMarketDbContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<GridResponseModel> GetItemsForGrid(GridRequestModel gridRequest)
        {
            var filteredTruffle = gridRequest.ColumnFilters?.Truffle;
            var queryable = _dBContext.Item.Where(item => string.IsNullOrEmpty(filteredTruffle) || item.Truffle == filteredTruffle);

            var totalCount = await queryable.CountAsync();


            if (gridRequest.Sort != null && gridRequest.Sort.FirstOrDefault().Type != "none")
            {
                var sortProperty = gridRequest.Sort.FirstOrDefault().Field;
                var formattedSortProperty = Char.ToUpperInvariant(sortProperty[0]) + sortProperty[1..];

                if (gridRequest.Sort.FirstOrDefault().Type == "desc")
                {
                    queryable = queryable.OrderByDescending(item => EF.Property<Database.Models.Item>(item, formattedSortProperty));
                }
                else
                {
                    queryable = queryable.OrderBy(item => EF.Property<Database.Models.Item>(item, formattedSortProperty));
                }
            }

            if (totalCount > gridRequest.PerPage)
            {
                queryable = queryable
                    .Skip((gridRequest.Page - 1) * gridRequest.PerPage)
                    .Take(gridRequest.PerPage);
            }

            var rows = await queryable
                .Select(i => new ItemRowModel
                {
                    ItemId = i.ItemId,
                    Truffle = i.Truffle,
                    Weight = i.Weight,
                    Price = i.Price,
                    Origin = i.Origin,
                    PickingDate = i.PickingDate,
                    Certificated = i.Certificated,
                    Expiration =  i.Expiration,
                    SellerName = i.Seller.Name,
                    BuyerName = i.Buyer.Name,
                })
                .ToListAsync();

            return new GridResponseModel
            {
                TotalRows = totalCount,
                Rows = rows
            };
        }

        public async Task<int> Offer(ItemCreateModel itemCreateModel)
        {
            Database.Models.Item newItem = new()
            {
                Truffle = itemCreateModel.Truffle,
                Weight = itemCreateModel.Weight,
                Price = itemCreateModel.Price,
                Origin = itemCreateModel.Origin,
                PickingDate = itemCreateModel.PickingDate,
                Certificated = itemCreateModel.Certificated,
                Expiration = itemCreateModel.Expiration,
                SellerId  = itemCreateModel.SellerId
            };

            _dBContext.Item.Add(newItem);
            await _dBContext.SaveChangesAsync();

            return newItem.ItemId;
        }
    }
}
