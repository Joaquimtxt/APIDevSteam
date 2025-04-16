using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace APIDevSteam.Models
{
    public class Usuario : IdentityUser
    {
        public Usuario() : base()
        {
        }
        public string? Nome { get; set; }
        public DateOnly? DataNascimento { get; set; }
    }
}
