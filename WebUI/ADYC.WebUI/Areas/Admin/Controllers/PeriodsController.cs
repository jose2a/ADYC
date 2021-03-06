﻿using ADYC.WebUI.Controllers;
using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    [SelectedTab("periods")]
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

            // Add properties to layout
            AddPageHeader("Periods", "List of all periods");

            AddBreadcrumb("Periods", "");

            return View(periods);
        }
    }
}