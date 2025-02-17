using Microsoft.AspNetCore.Identity;

namespace TFTrainer.Models
{
    public class User : IdentityUser
    {        
        public string Name { get; set; }               
    }
}
