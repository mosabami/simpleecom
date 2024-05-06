using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Simpleecom.Shared.Models.User
{
    public class User : Item
    {
        public User()
        {
        }

        public User(CreateUserDto userDto)
        {
            this.Email = userDto.Email;
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

                return base.Id;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _userId = string.Empty;
                }
                else
                {
                    _userId = value;
                }
            }
        }

    }
    public class CreateUserDto
    {
        [Required]
        public string Email { get; set; }
    }
}
