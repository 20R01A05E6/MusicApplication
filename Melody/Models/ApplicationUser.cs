using Microsoft.AspNetCore.Identity;

namespace Melody.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Firstname {  get; set; }
        public string Lastname { get; set; }
        public string Email {  get; set; }
    }

}
