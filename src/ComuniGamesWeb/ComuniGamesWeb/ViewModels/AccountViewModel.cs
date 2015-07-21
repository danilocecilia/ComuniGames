using ComuniGamesWeb.Models;
using ComuniGamesWeb.Models.DataContext;
using ComuniGamesWeb.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace ComuniGamesWeb.ViewModels
{
    public class AccountViewModel
    {
        public AccountViewModel()
        {
            Users = new Usuario();
            Users.Endereco = new Endereco();
        }

        public Usuario Users { get; set; }

        public UserProfile UserProfile { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string UploadImage { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Your custom error message")]
        public string ConfirmPassword { get; set; }
    }
}