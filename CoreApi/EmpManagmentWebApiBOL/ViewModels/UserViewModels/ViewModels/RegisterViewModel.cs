using EmpManagmentWebApiBOL.ViewModels.UserViewModels.CustomValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.UserViewModels.ViewModels
{
    public class RegisterViewModel
    {
        //public RegisterViewModel()
        //{
        //    this.Countries = new List<CountryViewModels>();
        //    this.States = new List<StateViewModels>();
        //    this.Cities = new List<CityViewModels>();
        //}

        public string UserId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }

        //public List<CountryViewModels> Countries { get; set; }
        //public List<StateViewModels> States { get; set; }
        //public List<CityViewModels> Cities { get; set; }
        public bool TermsAndPolicy { get; set; }

    }
}
