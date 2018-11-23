using System;
using System.Web.Mvc;

namespace ADYC.WebUI.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SelectedTabAttribute : ActionFilterAttribute
    {
        private string _selectedTab;

        public SelectedTabAttribute(string selectedTab)
        {
            this._selectedTab = selectedTab;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.SelectedTab = _selectedTab;
        }
    }
}