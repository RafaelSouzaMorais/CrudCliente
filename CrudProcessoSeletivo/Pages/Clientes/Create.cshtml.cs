using CrudProcessoSeletivo.Application.DTOs;
using CrudProcessoSeletivo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrudProcessoSeletivo.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        private readonly IClienteService _service;

        public CreateModel(IClienteService service)
        {
            _service = service;
        }

        [BindProperty]
        public CreateClienteDto Cliente { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _service.CreateAsync(Cliente);
                TempData["Message"] = "Cliente criado com sucesso!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao criar cliente: {ex.Message}");
                return Page();
            }
        }
    }
}
