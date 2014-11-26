using Microsoft.AspNet.Mvc;
using Microsoft.Framework.ConfigurationModel;
using TreintijdenDashboard.Services;
using System.Threading.Tasks;

namespace TreintijdenDashboard.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            VertrektijdenService vertrektijdenService = new VertrektijdenService(_config);

            var vertrektijden = await vertrektijdenService.GetVertrektijden("Gv");

            return View(vertrektijden);
        }

        public async Task<IActionResult> Partial()
        {
            VertrektijdenService vertrektijdenService = new VertrektijdenService(_config);

            var vertrektijden = await vertrektijdenService.GetVertrektijden("Gv");

            return View(vertrektijden);
        }

      
    }
}