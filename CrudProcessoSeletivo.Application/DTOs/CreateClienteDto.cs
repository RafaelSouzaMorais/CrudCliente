using System.ComponentModel.DataAnnotations;

namespace CrudProcessoSeletivo.Application.DTOs
{
    public record class CreateClienteDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [Range(10000000, 999999999, ErrorMessage = "Telefone deve ter entre 8 e 9 dígitos")]
        public int Telefone { get; set; }
    }
}
