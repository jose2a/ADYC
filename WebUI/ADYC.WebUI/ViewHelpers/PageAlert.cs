using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADYC.WebUI.ViewHelpers
{
    public enum PageAlertType
    {
        Error,
        Info,
        Warning,
        Success
    }

    public class PageAlert
    {
        public string Type { get; set; }

        public string ShortDesc { get; set; }
    }
}