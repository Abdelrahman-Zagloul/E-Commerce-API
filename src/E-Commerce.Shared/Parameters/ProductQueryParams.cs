namespace E_Commerce.Shared.Parameters
{
    public class ProductQueryParams
    {
        private const int MaxPageSize = 10;
        private const int defaultPageSize = 5;
        private int _pageSize = defaultPageSize;

        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public string? searchTerm { get; set; }
        public ProductSortingOption? sortingOption { get; set; }


        public int PageIndex { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
