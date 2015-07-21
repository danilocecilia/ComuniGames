using ComuniGamesWeb.Models;
using ComuniGamesWeb.Models.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ComuniGamesWeb.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Antiga")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação de nova senha não batem.")]
        public string ConfirmNewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

    }
}
