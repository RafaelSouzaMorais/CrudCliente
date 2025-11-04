using CrudProcessoSeletivo.Application.Interfaces;
using CrudProcessoSeletivo.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrudProcessoSeletivo.Pages.Clientes
{
    public class EditModel : PageModel
    {
        private readonly IClienteService _service;

        public EditModel(IClienteService service)
        {
            _service = service;
        }

        [BindProperty]
        public UpdateClienteDto Cliente { get; set; } = new();

        [BindProperty]
        public Guid ClienteId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var cliente = await _service.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ClienteId = id;
            Cliente = new UpdateClienteDto
            {
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _service.UpdateAsync(ClienteId, Cliente);
                TempData["Message"] = "Cliente atualizado com sucesso!";
                return RedirectToPage("./Index");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao atualizar cliente: {ex.Message}");
                return Page();
            }
        }
    }
}
