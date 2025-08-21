namespace rp_challenge.Application.DTOs
{
    public class CarDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string stock { get; set; } = string.Empty;
        public Decimal Price { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class CreateCarDTO
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string stock { get; set; } = string.Empty;
        public Decimal Price { get; set; }
    }

    public class UpdateCarDTO
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string stock { get; set; } = string.Empty;
        public Decimal Price { get; set; }
    }
}
