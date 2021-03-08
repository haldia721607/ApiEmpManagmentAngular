using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.Enum
{
    public enum FileStoreModeOptions
    {
        SingleImageSaveToFolder,
        MultipleImageSaveToFolder,
        SingleImageSaveToDatabase,
        MultipleImageSaveToDatabase,
        SingleFileSaveToFolder,
        MultipleFileSaveToFolder,
        SingleFileSaveToDatabase,
        MultipleFileSaveToDatabase,
        ExcelFileDataSaveToDatabase
    }
}
