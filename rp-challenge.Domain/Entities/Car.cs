namespace rp_challenge.Domain.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        private Car() { }

        public Car(string brand, string model, decimal price)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Model cannot be empty", nameof(brand));
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty", nameof(model));

            if (decimal.IsPositive(price))
                throw new ArgumentException("Price cannot be empty", nameof(price));

            Brand = brand;
            Model = model;
            Price = price;
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
        public static Car Create(string brand, string model, decimal price)
        {
            return new Car(brand, model, price);
        }

        public void Update(string brand, string model, decimal price)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand cannot be empty", nameof(brand));
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty", nameof(model));

            if (decimal.IsPositive(price))
                throw new ArgumentException("Price cannot be empty", nameof(model));




            Brand = brand;
            Model = model;
            Price = price;
            Updated = DateTime.UtcNow;
        }

        // Format DAO object
        public static Car FromDatabase(int id, string brand, string model, decimal price, DateTime created, DateTime updated)
        {
            return new Car
            {
                Id = id,
                Brand = brand,
                Model = model,
                Price = price,
                Created = created,
                Updated = updated
            };
        }
    }
}
