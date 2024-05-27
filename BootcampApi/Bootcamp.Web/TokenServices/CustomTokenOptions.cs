namespace Bootcamp.Web.TokenServices
{
    public record CustomTokenOptions()
    {
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
    }
}