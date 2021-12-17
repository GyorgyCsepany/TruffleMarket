namespace TruffleMarketApi.Services.Item
{
    public class GridRequestModel
    {
        public ColumnFiltersModel ColumnFilters { get; set; }
        public List<ColumnSortModel> Sort { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
