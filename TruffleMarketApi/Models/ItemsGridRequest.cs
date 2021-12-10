namespace TruffleMarketApi.Models
{
    public class ItemsGridRequest
    {
        public ColumnFiltersModel ColumnFilters { get; set; }
        public List<ColumnSortModel> Sort { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
