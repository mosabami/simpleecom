using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Simpleecom.Shared.Models.User
{
    public class User : Item
    {

        public User()
        {
        }
        public User(string email, string userId, Cart cart)
        {
            this.Email = email;
            this.UserId = userId;
            this.cart = cart;
        }

        public User(CreateUserDto userDto)
        {
            this.Email = userDto.Email;
            this.cart = new Cart();
            this.cart.UserId = this.Id;
            
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
                    { UserId = this.Id; }
                }
                else
                _cart = value;
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
