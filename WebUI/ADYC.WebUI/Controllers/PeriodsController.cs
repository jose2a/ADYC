using ADYC.WebUI.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class PeriodsController : Controller
    {
        private PeriodRepository _periodRepository;

        public PeriodsController()
        {
            _periodRepository = new PeriodRepository();
        }

        // GET: CourseTypes
        public async Task<ActionResult> Index()
        {
            var periods = await _periodRepository.GetPeriodAsync();

            return View(periods);
        }
    }
}