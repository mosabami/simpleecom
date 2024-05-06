using System.ComponentModel.DataAnnotations;

namespace Simpleecom.Shared.Models.User
{
    public class User : Item
    {
        public User(string email)
        {
            this.Email = email;
        }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public string Email { get; set; }

        private string _userId;
        public string? UserId
        {
            get
            {

                return _userId;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _userId = Guid.NewGuid().ToString();
                }
                else
                {
                    _userId = value;
                }
            }
        }

    }
}
