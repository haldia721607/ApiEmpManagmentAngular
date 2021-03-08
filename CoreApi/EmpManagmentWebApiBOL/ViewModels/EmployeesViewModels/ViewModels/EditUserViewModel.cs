using EmpManagmentWebApiBOL.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            this.Claims = new List<string>();
            this.Roles = new List<string>();
            this.Countries = new List<Country>();
            this.States = new List<State>();
            this.Cities = new List<City>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        [ForeignKey("State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
        public List<Country> Countries { get; set; }
        public List<State> States { get; set; }
        public List<City> Cities { get; set; }
    }
}
