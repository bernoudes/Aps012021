using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models.ViewModel
{
    [NotMapped]
    public class OptionBoxViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string ControllerReturn { get; set; }
        public string MethodReturn { get; set; }
        public List<string> OptionsMessage { get; set; }
        public string ExtraData { get; set; }
    }
}
