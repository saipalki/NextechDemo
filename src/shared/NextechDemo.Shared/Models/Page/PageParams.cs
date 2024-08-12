namespace NextechDemo.Shared.Models
{
    /// <summary>
    /// Send page no and page size in query string of request
    /// </summary>
    public class PageParams
    {
        /// <summary>
        /// Limit the page size upto 200
        /// </summary>
        private const int maxPageSize = 200;
        /// <summary>
        /// Current page number, set to 1 as default
        /// </summary>
        public int PageNumber { get; set; } = 1;
        public string Search {  get; set; }                                                                                     
        /// <summary>
        /// No of records per page, set to 10 as default
        /// </summary>
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }
        /// <summary>
        /// Flag to check if full data set required
        /// </summary>
        public bool IsFullDataRequired { get; set; }
    }
}
