namespace NextechDemo.Application.Config
{
    public static class ApiConfig
    {
        /// <summary>
        ///     Collection of API Url
        /// </summary>
        public static class NewsConfig
        {
            public static string News() => "topstories.json?print=pretty";
            public static string NewsById(int id) => $"item/{id}.json?print=pretty";
        }
    }
}
