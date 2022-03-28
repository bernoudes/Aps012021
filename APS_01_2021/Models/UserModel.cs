using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APS_01_2021.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public DateTime BornDate { get; set; }
        public string Email { get; set; }
        public string StatusConnection { get; set; }
 
        [NotMapped]
        public string ConfirmPassword { get; set; }

        public string IsPasswordStrong(string password)
        {
            Regex regexPassNum = new ("(?=.*[0-9])");
            Regex regexSpecial = new ("(?=.*[!@#$&*])");
            Regex regexPassLowCase = new ("(?=.*[a-z])");
            Regex regexPassUpperCase = new ("(?=.*[A-Z])");
            Regex regexPassMany = new (".{8,10}");

            
            if (!regexPassNum.IsMatch(password))
            {
                return "NoNumber";
            }
            else if (!regexSpecial.IsMatch(password))
            {
                return "NoSpecial";
            }
            else if (!regexPassLowCase.IsMatch(password))
            {
                return "NoLowCase";
            }
            else if (!regexPassUpperCase.IsMatch(password))
            {
                return "NoUpperCase";
            }
            else if (!regexPassMany.IsMatch(password))
            {
                return "NoEnoughChar";
            }
            return "Success";
        }
        public bool IsConfirmPasswordConfirmed(string password, string confimpassword)
        {
            if (confimpassword == password)
            {
                return true;
            }
            return false;
        }
    }
}
