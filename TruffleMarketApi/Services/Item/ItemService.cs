using Microsoft.EntityFrameworkCore;
using TruffleMarketApi.Database;
using TruffleMarketApi.Database.Models;
using TruffleMarketApi.Services.User;

namespace TruffleMarketApi.Services.Item
{
    public class ItemService : IItemService
    {
        private readonly TruffleMarketDbContext _dBContext;
        private readonly IUserService _userService;

        public ItemService(TruffleMarketDbContext dBContext, IUserService userService)
        {
            _dBContext = dBContext;
            _userService = userService;
        }

        public async Task<GridResponseModel> GetItemsForGrid(string filterTruffle, string sortField, string sortType, int page, int perPage)
        {
            var queryable = _dBContext.Item
                .Where(item => (string.IsNullOrEmpty(filterTruffle) || item.Truffle == filterTruffle)
                               && item.Expiration > DateTimeOffset.UtcNow
                               && !item.ClosedBySeller);

            var totalCount = await queryable.CountAsync();

            if (!string.IsNullOrEmpty(sortField) && sortType != "none")
            {
                var sortProperty = sortField;
                var formattedSortProperty = Char.ToUpperInvariant(sortProperty[0]) + sortProperty[1..];

                if (sortType == "desc")
                {
                    queryable = queryable.OrderByDescending(item => EF.Property<ItemModel>(item, formattedSortProperty));
                }
                else
                {
                    queryable = queryable.OrderBy(item => EF.Property<ItemModel>(item, formattedSortProperty));
                }
            }

            if (totalCount > perPage)
            {
                queryable = queryable
                    .Skip((page - 1) * perPage)
                    .Take(perPage);
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
                    Expiration = i.Expiration
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
            ItemModel newItem = new()
            {
                Truffle = itemCreateModel.Truffle,
                Weight = itemCreateModel.Weight,
                Price = itemCreateModel.Price,
                Origin = itemCreateModel.Origin,
                PickingDate = itemCreateModel.PickingDate,
                Certificated = itemCreateModel.Certificated,
                Expiration = itemCreateModel.Expiration,
                Description = itemCreateModel.Description,
                SellerId = _userService.UserProfil.UserId
            };

            _dBContext.Item.Add(newItem);
            await _dBContext.SaveChangesAsync();

            return newItem.ItemId;
        }

        public async Task<ItemInfoModel> GetItemInfo(int itemId)
        {
            var item = await _dBContext.Item.Where(i => i.ItemId == itemId).Include(i => i.Seller).FirstOrDefaultAsync();

            if (item is null)
                return null;

            var itemModel = new ItemInfoModel
            {
                Description = item.Description,
                Certificated = item.Certificated,
                SellerRate = item.Seller.Rate,
                BuyerName = item.Buyer?.Name,
                SellerName = item.Seller.Name
            };

            return itemModel;
        }

        public async Task<int?> BidforItem(ItemBidModel itemBidModel)
        {
            var userId = _userService.UserProfil.UserId;

            var item = await _dBContext.Item
                .FirstOrDefaultAsync(i => i.ItemId == itemBidModel.ItemId
                    && itemBidModel.BidPrice > i.Price
                    && i.Expiration > DateTimeOffset.UtcNow
                    && i.SellerId != userId
                    && i.BuyerId != userId
                    && !i.ClosedBySeller);

            if (item is null)
                return null;

            item.BuyerId = userId;
            item.Price = itemBidModel.BidPrice;

            await _dBContext.SaveChangesAsync();

            return item.ItemId;
        }

        public async Task<List<BidRowModel>> GetItemsForBuyer()
        {
            var buyerBids = await _dBContext.Item
                .Where(i => i.BuyerId == _userService.UserProfil.UserId && !i.ClosedByBuyer)
                .Include(i => i.Seller)
                .Select(i => new BidRowModel
                {
                    ItemId = i.ItemId,
                    Truffle = i.Truffle,
                    Weight = i.Weight,
                    Price = i.Price,
                    Expiration = i.Expiration,
                    SellerId = i.SellerId,
                    SellerName = i.Seller.Name,
                    SellerEmail = i.Seller.Email,
                    SellerRate = i.Seller.Rate,
                    IsExecuted = i.ClosedBySeller || i.Expiration <= DateTimeOffset.UtcNow
                })
                .ToListAsync();

            return buyerBids;
        }


        public async Task CloseBid(BidCloseModel closeModel)
        {
            var item = await _dBContext.Item.FirstOrDefaultAsync(i => i.ItemId == closeModel.ItemId && i.BuyerId == _userService.UserProfil.UserId);

            if (item is null)
                return;

            if (closeModel.SellerRate != null)
                await _userService.RateUser(closeModel.SellerId, (double)closeModel.SellerRate);


            if (item.ClosedBySeller)
            {
                _dBContext.Item.Remove(item);
            }
            else
            { 
                item.ClosedByBuyer = true;
            }

            await _dBContext.SaveChangesAsync();
        }

        public async Task<List<OfferRowModel>> GetItemsForSeller()
        {
            var buyerBids = await _dBContext.Item
                .Where(i => i.SellerId == _userService.UserProfil.UserId && !i.ClosedBySeller)
                .Include(i => i.Buyer)
                .Select(i => new OfferRowModel
                {
                    ItemId = i.ItemId,
                    Truffle = i.Truffle,
                    Weight = i.Weight,
                    Price = i.Price,
                    Expiration = i.Expiration,
                    BuyerName = i.Buyer.Name,
                    BuyerEmail = i.Buyer.Email,
                    Status = i.Expiration <= DateTimeOffset.UtcNow ? i.BuyerId != null ? ItemStatus.Executed : ItemStatus.Failed : ItemStatus.Open
                })
                .ToListAsync();

            return buyerBids;
        }

        public async Task CloseOffer(OfferCloseModel closeModel)
        {
            var item = await _dBContext.Item.FirstOrDefaultAsync(i => i.ItemId == closeModel.ItemId && i.SellerId == _userService.UserProfil.UserId);

            if (item is null)
                return;

            if (item.BuyerId is null || item.ClosedByBuyer)
            {
                _dBContext.Item.Remove(item);
            }
            else
            {
                item.ClosedBySeller = true;
            }

            await _dBContext.SaveChangesAsync();
        }

        public async Task<ItemKnapSackResultModel> BatchBid(ItemBatchModel itemButchModel)
        {
            var buyerId = _userService.UserProfil.UserId;
            var items = await _dBContext.Item.Where(i => i.Truffle == itemButchModel.Truffle
                                                && i.Expiration > DateTimeOffset.UtcNow
                                                && i.BuyerId != buyerId
                                                && i.SellerId != buyerId
                                                && !i.ClosedBySeller)
                                                .ToListAsync();
            var itemCount = items.Count;

            var maxCapacity = itemButchModel.MaxTotalPrice;

            var matrix = KnapSack(maxCapacity, itemCount, items);
            var maxTotalWeight = matrix[itemCount, maxCapacity];

            if (maxTotalWeight < itemButchModel.MinTotalWeight)
                return null;

            var resultModel = await UpdateKnapSackItems(maxCapacity, itemCount, matrix, buyerId, items);

            resultModel.TotalWeight = maxTotalWeight;

            return resultModel;
        }


        private int[,] KnapSack(int maxCapacity, int itemCount, List<ItemModel> items)
        {
            int[,] matrix = new int[itemCount + 1, maxCapacity + 1];

            for (int i = 1; i <= itemCount; i++)
            {
                for (int w = 1; w <= maxCapacity; w++)
                {
                    var currentItem = items[i - 1];
                    var currentItemPrice = currentItem.Price + 1; 

                    if (currentItemPrice <= w)
                    {
                        matrix[i, w] = Math.Max(currentItem.Weight + matrix[i - 1, w - currentItemPrice], matrix[i - 1, w]);
                    }
                    else
                    {
                        matrix[i, w] = matrix[i - 1, w];
                    }
                }
            }

            return matrix;
        }

        private async Task<ItemKnapSackResultModel> UpdateKnapSackItems(int maxCapacity, int itemCount, int[,] matrix, int BuyerId, List<ItemModel> items)
        {
            int i = itemCount;
            int j = maxCapacity;
            var resultModel = new ItemKnapSackResultModel();  

            while (i > 0 && j > 0)
            {
                if (matrix[i, j] != matrix[i - 1, j])
                {
                    var includedItem = items[i - 1];

                    includedItem.BuyerId = BuyerId;
                    includedItem.Price++;

                    resultModel.IncludedItemsId.Add(includedItem.ItemId);
                    resultModel.TotalPrice += includedItem.Price;

                    j -= includedItem.Price;
                }

                i--;
            }

            await _dBContext.SaveChangesAsync();

            return resultModel;
        }

    }
}
