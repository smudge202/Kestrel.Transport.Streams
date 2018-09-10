using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Atlas.AspNetCore.Server.Kestrel.Transport.Pipelines;

namespace WebApplication1.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public async Task OnGetAsync() 
	        => Message = await Pipeline.GetAsync("/", HttpContext.RequestAborted);
    }
}
