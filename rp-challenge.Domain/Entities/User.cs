namespace rp_challenge.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }

        private User() { }

        public User(string email, string name, string password
)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password hash cannot be empty", nameof(password));

            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format", nameof(email));


            Email = email;
            Name = name;
            Password = password;
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        public static User Create(string email, string name, string password)
        {
            return new User(email, name, password);
        }

        public void UpdatePassword(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password cannot be empty", nameof(passwordHash));

            Password = passwordHash;
            Updated = DateTime.UtcNow;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addres = new System.Net.Mail.MailAddress(email);
                return addres.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Format DAO object
        public static User FromDatabase(int id, string email, string username, string password, DateTime createdAt, DateTime updatedAt)
        {
            return new User
            {
                Id = id,
                Email = email,
                Name = username,
                Password = password,
                Created = createdAt,
                Updated = updatedAt
            };
        }
    }
}
