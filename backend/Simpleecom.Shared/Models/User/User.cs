namespace Simpleecom.Shared.Models.User
{
    public class User : Item
    {
        public User(string firstName,string lastName, string email)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

    }
}
