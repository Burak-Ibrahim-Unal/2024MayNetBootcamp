namespace Bootcamp.Web.TokenServices
{
    public record CustomTokenOption()
    {
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
    }
}