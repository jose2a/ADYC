using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class ADYCBasedController : Controller
    {
        protected void AddErrorsFromAdycHttpExceptionToModelState(AdycHttpRequestException ahre, ModelStateDictionary modelState)
        {
            foreach (var error in ahre.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected string GetErrorsFromAdycHttpExceptionToString(AdycHttpRequestException ahre)
        {
            var errorString = "";

            foreach (var error in ahre.Errors)
            {
                errorString += error;
            }

            return errorString;
        }

        protected List<ScheduleViewModel> GetScheduleList(int offeringId, List<ScheduleViewModel> scheduleViewModelList, List<DayEnumViewModel> days)
        {
            var scheduleList = new List<ScheduleViewModel>();
            foreach (var d in days)
            {
                var sch = scheduleViewModelList.SingleOrDefault(s => s.Day == d.Id);

                if (sch != null)
                {
                    scheduleList.Add(sch);
                }
                else
                {
                    scheduleList.Add(new ScheduleViewModel
                    {
                        OfferingId = offeringId,
                        Day = d.Id,
                        StartTime = null,
                        EndTime = null
                    });
                }
            }

            return scheduleList;
        }

        protected static List<DayEnumViewModel> GetDayEnumViewModelList()
        {
            return ((IEnumerable<Day>)Enum.GetValues(typeof(Day))).Select(c => new DayEnumViewModel() { Id = (byte)c, Name = c.ToString() }).ToList();
        }
    }
}