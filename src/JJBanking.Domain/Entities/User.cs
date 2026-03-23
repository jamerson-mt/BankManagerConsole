using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JJBanking.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Cpf { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // relacionamento com as contas do usuário
        public virtual Account? Account { get; set; }
    }
}
