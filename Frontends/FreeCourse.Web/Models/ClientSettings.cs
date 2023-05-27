namespace FreeCourse.Web.Models
{
    public class ClientSettings
    {
        public Client WebMvcClient { get; set; }
        public Client WebMvcClientForUser { get; set; }
    }

    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
