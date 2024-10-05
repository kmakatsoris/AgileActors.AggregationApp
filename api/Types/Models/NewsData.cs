namespace AgileActors.AggregationApp.Types.Models.NewsData
{
    public class ArticleSource
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Article
    {
        public ArticleSource Source { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }
    }

    public class NewsData
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public List<Article> Articles { get; set; }
    }
}