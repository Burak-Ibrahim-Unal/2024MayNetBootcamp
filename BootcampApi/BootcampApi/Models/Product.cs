namespace BootcampApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!; // null olamaz uyarısı...
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; } = new();
    }
}
