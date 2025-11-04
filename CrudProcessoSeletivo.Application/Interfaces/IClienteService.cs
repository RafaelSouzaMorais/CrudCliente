using CrudProcessoSeletivo.Application.DTOs;

namespace CrudProcessoSeletivo.Application.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> GetAllAsync();
        Task<ClienteDto?> GetByIdAsync(Guid id);
        Task<ClienteDto> CreateAsync(CreateClienteDto dto);
        Task UpdateAsync(Guid id, UpdateClienteDto dto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
