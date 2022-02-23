using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [NotMapped]
    public class DeleteBoxModel
    {
        public string Message { get; set; }
        public bool GoDelete { get; set; }
        public bool IsDelete { get; set; }
        public string Error { get; set; }

    }
}
