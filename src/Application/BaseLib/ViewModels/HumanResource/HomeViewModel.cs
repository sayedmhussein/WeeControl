using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1;

namespace WeeControl.Applications.BaseLib.ViewModels.HumanResource
{
    public class HomeViewModel : ObservableObject
    {
        public ObservableCollection<EmployeeDto> EmployeeList { get; set; }

        public HomeViewModel()
        {
        }

        public void PopulateEmployeeList()
        {

        }
    }
}
