namespace CrudProcessoSeletivo.Application.DTOs
{
    public record class ClienteDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Telefone { get; set; }
    }
}
