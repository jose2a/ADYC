using ADYC.API.ViewModels;
using ADYC.IService;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace ADYC.API.Controllers
{
    [RoutePrefix("api/Periods")]
    public class PeriodsController : ADYCBasedApiController
    {
        private IPeriodService _periodService;

        public PeriodsController(IPeriodService periodService)
        {
            _periodService = periodService;
        }

        // GET api/Periods
        [Route("")]
        [ResponseType(typeof(IEnumerable<PeriodDto>))]
        public IHttpActionResult Get()
        {
            var periods = _periodService.GetAll();

            return base.Ok(periods
                .Select(p =>
                {
                    return GetPeriodDto(p);
                }));
        }

        // GET api/Periods/5
        [Route("{id}")]
        [ResponseType(typeof(PeriodDto))]
        public IHttpActionResult Get(int id)
        {
            var period = _periodService.Get(id);

            if (period != null)
            {
                return Ok(GetPeriodDto(period));
            }

            return NotFound();
        }

        // GET api/Periods/period 1
        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<PeriodDto>))]
        public IHttpActionResult GetByName(string name)
        {
            var periods = _periodService.FindByName(name);

            return Ok(periods
                .Select(p => {
                    return GetPeriodDto(p);
                }));
        }
    }
}
