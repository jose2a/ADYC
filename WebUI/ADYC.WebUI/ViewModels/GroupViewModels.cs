using ADYC.API.ViewModels;
using System.ComponentModel.DataAnnotations;

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

        public GroupFormViewModel(GroupDto group)
        {
            Id = group.Id;
            Name = group.Name;
        }
    }
}