using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels
{
    public class EmployeeListViewModel
    {
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string GenderName { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FileExtension { get; set; }
        public byte[] Data { get; set; }
    }
}
