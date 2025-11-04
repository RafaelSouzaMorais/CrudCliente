using CrudProcessoSeletivo.Domain.Entities;
using CrudProcessoSeletivo.Infrastructure.Data;
using CrudProcessoSeletivo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrudProcessoSeletivo.Tests.RepositoryTests
{
    [TestClass]
    public class ClienteRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [TestMethod]
        public async Task CreateAsync_DeveAdicionarCliente()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new ClienteRepository(context);
            var cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao@email.com",
                Telefone = 987654321
            };

            // Act
            await repository.CreateAsync(cliente);

            // Assert
            var clienteAdicionado = await context.Clientes.FindAsync(cliente.Id);
            Assert.IsNotNull(clienteAdicionado);
            Assert.AreEqual("João Silva", clienteAdicionado.Nome);
            Assert.AreEqual("joao@email.com", clienteAdicionado.Email);
        }

        [TestMethod]
        public async Task GetAllAsync_DeveRetornarTodosClientes()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new ClienteRepository(context);
            
            await repository.CreateAsync(new Cliente 
            { 
                Id = Guid.NewGuid(), 
                Nome = "Cliente 1", 
                Email = "cliente1@email.com", 
                Telefone = 111111111 
            });
            await repository.CreateAsync(new Cliente 
            { 
                Id = Guid.NewGuid(), 
                Nome = "Cliente 2", 
                Email = "cliente2@email.com", 
                Telefone = 222222222 
            });

            // Act
            var clientes = await repository.GetAllAsync();

            // Assert
            Assert.AreEqual(2, clientes.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_DeveRetornarClienteCorreto()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new ClienteRepository(context);
            var clienteId = Guid.NewGuid();
            var cliente = new Cliente 
            { 
                Id = clienteId,
                Nome = "Maria Santos", 
                Email = "maria@email.com", 
                Telefone = 333333333 
            };
            await repository.CreateAsync(cliente);

            // Act
            var result = await repository.GetByIdAsync(clienteId);

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
            var repository = new ClienteRepository(context);

            // Act
            var result = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateAsync_DeveAtualizarCliente()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new ClienteRepository(context);
            var clienteId = Guid.NewGuid();
            var cliente = new Cliente 
            { 
                Id = clienteId,
                Nome = "Pedro Oliveira", 
                Email = "pedro@email.com", 
                Telefone = 444444444 
            };
            await repository.CreateAsync(cliente);

            // Act
            cliente.Nome = "Pedro Oliveira Silva";
            cliente.Email = "pedro.silva@email.com";
            cliente.Telefone = 555555555;
            await repository.UpdateAsync(cliente);

            // Assert
            var updated = await repository.GetByIdAsync(clienteId);
            Assert.IsNotNull(updated);
            Assert.AreEqual("Pedro Oliveira Silva", updated.Nome);
            Assert.AreEqual("pedro.silva@email.com", updated.Email);
            Assert.AreEqual(555555555, updated.Telefone);
        }

        [TestMethod]
        public async Task DeleteAsync_DeveRemoverCliente()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new ClienteRepository(context);
            var clienteId = Guid.NewGuid();
            var cliente = new Cliente 
            { 
                Id = clienteId,
                Nome = "Ana Costa", 
                Email = "ana@email.com", 
                Telefone = 666666666 
            };
            await repository.CreateAsync(cliente);

            // Act
            await repository.DeleteAsync(clienteId);

            // Assert
            var deleted = await repository.GetByIdAsync(clienteId);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public async Task DeleteAsync_NaoDeveLancarExcecaoParaIdInexistente()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new ClienteRepository(context);

            // Act & Assert
            await repository.DeleteAsync(Guid.NewGuid());
            // Se não lançar exceção, o teste passa
        }
    }
}
