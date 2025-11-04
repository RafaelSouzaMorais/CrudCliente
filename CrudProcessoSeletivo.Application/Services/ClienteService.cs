using CrudProcessoSeletivo.Application.DTOs;
using CrudProcessoSeletivo.Application.Interfaces;
using CrudProcessoSeletivo.Domain.Entities;
using CrudProcessoSeletivo.Domain.Interfaces;

namespace CrudProcessoSeletivo.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            var clientes = await _repository.GetAllAsync();
            return clientes.Select(c => new ClienteDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone
            });
        }

        public async Task<ClienteDto?> GetByIdAsync(Guid id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null) return null;

            return new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone
            };
        }

        public async Task<ClienteDto> CreateAsync(CreateClienteDto dto)
        {
            var cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone
            };

            await _repository.CreateAsync(cliente);

            return new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone
            };
        }

        public async Task UpdateAsync(Guid id, UpdateClienteDto dto)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente com ID {id} não encontrado.");

            cliente.Nome = dto.Nome;
            cliente.Email = dto.Email;
            cliente.Telefone = dto.Telefone;

            await _repository.UpdateAsync(cliente);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            return cliente != null;
        }
    }
}
