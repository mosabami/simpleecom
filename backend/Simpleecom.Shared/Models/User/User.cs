

namespace Simpleecom.Shared.Models.User
{
    public class User : Item
    {
        public User(string name, string email)
        {
            this.Name = name;
            this.Email = email;
            PartitionKey = email;
        }

        public User(string name, string email, string partitionKey)
        {
            this.Name = name;
            this.Email = email;
            PartitionKey = partitionKey;
        }

        public string Name { get; set; }

        public string Email { get; set; }
        public string PartitionKey { get; set; }

        protected override string GetPartitionKeyValue() => PartitionKey;
    }
}
