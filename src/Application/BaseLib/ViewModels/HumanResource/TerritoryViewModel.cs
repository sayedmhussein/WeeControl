using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace WeeControl.Applications.BaseLib.ViewModels.HumanResource
{
    public class TerritoryViewModel : ObservableObject
    {
        #region Properties
        private Guid territoryid;
        public Guid TerritoryId
        {
            get => territoryid;
            set => SetProperty(ref territoryid, value);
        }

        private string territoryName;
        public string TerritoryName
        {
            get => territoryName;
            set => SetProperty(ref territoryName, value);
        }

        public ObservableCollection<string> CountryList { get; set; }

        private string territoryCountry;
        public string TerritoryCountry
        {
            get => territoryCountry;
            set => SetProperty(ref territoryCountry, value);
        }

        public ObservableCollection<string> TerritoryList { get; set; }

        private string reportTo;
        public string ReportTo
        {
            get => reportTo;
            set => SetProperty(ref reportTo, value);
        }
        #endregion

        public TerritoryViewModel()
        {
            CountryList = new ObservableCollection<string>();
            CountryList.Add("EGP");
            CountryList.Add("USA");
        }
    }
}
