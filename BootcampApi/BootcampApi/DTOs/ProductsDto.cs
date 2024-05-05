namespace BootcampApi.DTOs
{
    public record ProductDto(int Id,string Name,decimal Price,string CreatedDate);

    //public record ProductsDto
    //{
    //    public int Id { get; init; }
    //    public string Name { get; init; } = default!; // null olamaz uyarısı...
    //    public decimal Price { get; init; }
    //    public string CreatedDate { get; init; } = default!;
    //}
}
