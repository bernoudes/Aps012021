using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models.ViewModel
{
    [NotMapped]
    public class MessageBoxViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
