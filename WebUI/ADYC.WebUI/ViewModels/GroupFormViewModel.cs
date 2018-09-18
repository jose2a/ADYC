using ADYC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADYC.WebUI.ViewModels
{
    public class GroupFormViewModel
    {
        public bool IsNew { get; set; }

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Title
        {
            get
            {
                return IsNew ? "New Group" : "Edit Group";
            }
        }

        public GroupFormViewModel()
        {

        }

        public GroupFormViewModel(Group group)
        {
            Id = group.Id;
            Name = group.Name;
        }
    }
}