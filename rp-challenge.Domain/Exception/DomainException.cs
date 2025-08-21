namespace rp_challenge.Domain.Exception
{
    public class CarNotFoundException : System.Exception
    {
        public CarNotFoundException(int id) : base($"Car with ID {id} was not found.")
        {
        }
    }

    public class CarAlreadyExistsException : System.Exception
    {
        public CarAlreadyExistsException(string model) : base($"Car {model} already exists.")
        {
        }
    }

    public class UserNotFoundException : System.Exception
    {
        public UserNotFoundException(int id) : base($"User with ID {id} was not found.")
        {
        }
    }

    public class UserAlreadyExistsException : System.Exception
    {
        public UserAlreadyExistsException(string email) : base($"User with email {email} already exists.")
        {
        }
    }

    public class InvalidCredentialsException : System.Exception
    {
        public InvalidCredentialsException() : base("Invalid email or password.")
        {
        }
    }
}
