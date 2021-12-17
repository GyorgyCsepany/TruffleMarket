﻿namespace TruffleMarketApi.Services.Item
{
    public interface IItemService
    {
        Task<GridResponseModel> GetItemsForGrid(GridRequestModel gridRequest);
        Task<int> Offer(ItemCreateModel itemCreateModel);
    }
}
