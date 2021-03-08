using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string FileEncodingTypes { get; set; }
        public string FileStoreMode { get; set; }
        public string Path { get; set; }
        public Byte[] Data { get; set; }
    }
}
