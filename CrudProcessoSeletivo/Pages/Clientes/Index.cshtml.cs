using CrudProcessoSeletivo.Application.DTOs;
using CrudProcessoSeletivo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrudProcessoSeletivo.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        private readonly IClienteService _service;

        public IndexModel(IClienteService service)
        {
            _service = service;
        }

        public IEnumerable<ClienteDto> Clientes { get; set; } = new List<ClienteDto>();

        public async Task OnGetAsync()
        {
            Clientes = await _service.GetAllAsync();
        }
    }
}
