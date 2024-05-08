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

        [Required]
        public string Email { get; set; }

        private Cart _cart;
        public Cart cart 
        { 
            get
            {
                return _cart;
            }
            set
            {
                if (value == null)
                {
                    _cart = new Cart();
                    { UserId = base.Id;}
                }
                else
                _cart = value;
                _cart.UserId = base.Id;
            }
        }
        

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
