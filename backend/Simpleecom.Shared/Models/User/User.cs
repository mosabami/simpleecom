namespace Simpleecom.Shared.Models.User
{
    public class User : Item
    {
        public User(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }

        public string Name { get; set; }

        public string Email { get; set; }

        protected override string GetPartitionKeyValue() => Email;
    }
}
