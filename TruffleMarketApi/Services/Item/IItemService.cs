namespace TruffleMarketApi.Services.Item
{
    public interface IItemService
    {
        Task<GridResponseModel> GetItemsForGrid(string filterTruffle, string sortField, string sortType, int page, int perPage);
        Task<int> Offer(ItemCreateModel itemCreateModel);
        Task<ItemInfoModel> GetItemInfo(int itemId);
        Task<int?> BidforItem(ItemBidModel itemBidModel);
        Task<ItemKnapSackResultModel> BatchBid(ItemBatchModel itemButchModel);
    }
}
