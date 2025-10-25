using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyC.Sorteo.Domain.Entities
{

    public class LastTokenId
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [MaxLength(64)]
        public string Jti { get; set; } = string.Empty;

        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario? Usuario { get; set; }
    }
}
