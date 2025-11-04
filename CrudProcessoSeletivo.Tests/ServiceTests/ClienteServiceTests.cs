using CrudProcessoSeletivo.Application.DTOs;
using CrudProcessoSeletivo.Application.Services;
using CrudProcessoSeletivo.Domain.Interfaces;
using CrudProcessoSeletivo.Infrastructure.Data;
using CrudProcessoSeletivo.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

namespace CrudProcessoSeletivo.Tests.ServiceTests
{
    [TestClass]
    public class ClienteServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [TestMethod]
        public async Task CreateAsync_DeveCriarClienteComDtoValido()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            IClienteRepository repository = new ClienteRepository(context);
            var service = new ClienteService(repository);
            var createDto = new CreateClienteDto
            {
                Nome = "João Silva",
                Email = "joao@email.com",
                Telefone = 987654321
            };

            // Act
            var result = await service.CreateAsync(createDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("João Silva", result.Nome);
            Assert.AreEqual("joao@email.com", result.Email);
            Assert.AreNotEqual(Guid.Empty, result.Id);
        }

        [TestMethod]
        public async Task GetAllAsync_DeveRetornarTodosClientesComoDto()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            IClienteRepository repository = new ClienteRepository(context);
            var service = new ClienteService(repository);
            
            await service.CreateAsync(new CreateClienteDto 
            { 
                Nome = "Cliente 1", 
                Email = "cliente1@email.com", 
                Telefone = 111111111 
            });
            await service.CreateAsync(new CreateClienteDto 
            { 
                Nome = "Cliente 2", 
                Email = "cliente2@email.com", 
                Telefone = 222222222 
            });

            // Act
            var clientes = await service.GetAllAsync();

            // Assert
            Assert.AreEqual(2, clientes.Count());
            Assert.IsTrue(clientes.All(c => c is ClienteDto));
        }

        [TestMethod]
        public async Task GetByIdAsync_DeveRetornarClienteDtoCorreto()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            IClienteRepository repository = new ClienteRepository(context);
            var service = new ClienteService(repository);
            var createDto = new CreateClienteDto 
            { 
                Nome = "Maria Santos", 
                Email = "maria@email.com", 
                Telefone = 333333333 
            };
            var clienteCriado = await service.CreateAsync(createDto);

            // Act
            var result = await service.GetByIdAsync(clienteCriado.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Maria Santos", result.Nome);
            Assert.AreEqual("maria@email.com", result.Email);
        }

        [TestMethod]
        public async Task GetByIdAsync_DeveRetornarNullParaIdInexistente()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            IClienteRepository repository = new ClienteRepository(context);
            var service = new ClienteService(repository);

            // Act
            var result = await service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateAsync_DeveAtualizarClienteComSucesso()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            IClienteRepository repository = new ClienteRepository(context);
            var service = new ClienteService(repository);
            var createDto = new CreateClienteDto 
            { 
                Nome = "Pedro Oliveira", 
                Email = "pedro@email.com", 
                Telefone = 444444444 
            };
            var clienteCriado = await service.CreateAsync(createDto);

            var updateDto = new UpdateClienteDto
            {
                Nome = "Pedro Oliveira Silva",
                Email = "pedro.silva@email.com",
                Telefone = 555555555
            };

            // Act
            await service.UpdateAsync(clienteCriado.Id, updateDto);

            // Assert
            var updated = await service.GetByIdAsync(clienteCriado.Id);
            Assert.IsNotNull(updated);
            Assert.AreEqual("Pedro Oliveira Silva", updated.Nome);
            Assert.AreEqual("pedro.silva@email.com", updated.Email);
            Assert.AreEqual(555555555, updated.Telefone);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateAsync_DeveLancarExcecaoParaIdInexistente()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            IClienteRepository repository = new ClienteRepository(context);
            var service = new ClienteService(repository);
            var updateDto = new UpdateClienteDto
            {
                Nome = "Teste",
                Email = "teste@email.com",
                Telefone = 123456789
            };

            // Act
            await service.UpdateAsync(Guid.NewGuid(), updateDto);

            // Assert - espera-se exceção
        }

        [TestMethod]
        public async Task DeleteAsync_DeveRemoverCliente()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            IClienteRepository repository = new ClienteRepository(context);
            var service = new ClienteService(repository);
            var createDto = new CreateClienteDto 
            { 
                Nome = "Ana Costa", 
                Email = "ana@email.com", 
                Telefone = 666666666 
            };
            var clienteCriado = await service.CreateAsync(createDto);

            // Act
            await service.DeleteAsync(clienteCriado.Id);

            // Assert
            var deleted = await service.GetByIdAsync(clienteCriado.Id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public async Task ExistsAsync_DeveRetornarTrueParaClienteExistente()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            IClienteRepository repository = new ClienteRepository(context);
            var service = new ClienteService(repository);
            var createDto = new CreateClienteDto 
            { 
                Nome = "Teste", 
                Email = "teste@email.com", 
                Telefone = 777777777 
            };
            var clienteCriado = await service.CreateAsync(createDto);

            // Act
            var exists = await service.ExistsAsync(clienteCriado.Id);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task ExistsAsync_DeveRetornarFalseParaClienteInexistente()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            IClienteRepository repository = new ClienteRepository(context);
            var service = new ClienteService(repository);

            // Act
            var exists = await service.ExistsAsync(Guid.NewGuid());

            // Assert
            Assert.IsFalse(exists);
        }
    }
}
