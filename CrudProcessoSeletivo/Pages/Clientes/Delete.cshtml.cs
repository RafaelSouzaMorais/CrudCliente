using CrudProcessoSeletivo.Application.DTOs;
using CrudProcessoSeletivo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrudProcessoSeletivo.Pages.Clientes
{
    public class DeleteModel : PageModel
    {
        private readonly IClienteService _service;

        public DeleteModel(IClienteService service)
        {
            _service = service;
        }

        public ClienteDto Cliente { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var cliente = await _service.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            Cliente = cliente;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["Message"] = "Cliente excluído com sucesso!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao excluir cliente: {ex.Message}");
                return Page();
            }
        }
    }
}
