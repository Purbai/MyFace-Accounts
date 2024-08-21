using System.ComponentModel.DataAnnotations;

namespace MyFace.Models.Request
{
    public class CreateLoginRequest
    {
        [StringLength(70)]
        public string Username { get; set; }
        
        [StringLength(132)]
        public string Password { get; set; }
    }
}