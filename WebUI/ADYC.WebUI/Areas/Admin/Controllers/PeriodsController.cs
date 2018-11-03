using ADYC.WebUI.Controllers;
using ADYC.WebUI.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    public class PeriodsController : ADYCBasedController
    {
        private PeriodRepository _periodRepository;

        public PeriodsController()
        {
            _periodRepository = new PeriodRepository();
        }

        // GET: Admin/Periods
        public async Task<ActionResult> Index()
        {
            var periods = await _periodRepository.GetPeriods();

            return View(periods);
        }
    }
}