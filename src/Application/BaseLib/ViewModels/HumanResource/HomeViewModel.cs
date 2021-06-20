using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using WeeControl.Applications.BaseLib.Entities.Territory;
using WeeControl.Applications.BaseLib.Interfaces;

namespace WeeControl.Applications.BaseLib.ViewModels.HumanResource
{
    public class HomeViewModel : ObservableObject
    {
        private readonly IAppDatabase database;

        public ObservableCollection<TerritoryDbo> Territories { get; set; }

        public HomeViewModel()
            : this(Ioc.Default.GetRequiredService<IAppDatabase>())
        {
        }

        public HomeViewModel(IAppDatabase database)
        {
            this.database = database;

            Territories = new ObservableCollection<TerritoryDbo>();
            Task.Run(async () => await PopulateEmployeeList());
        }

        public async Task PopulateEmployeeList()
        {
            await database.InitalizeTerritory();
            var list = await database.GetAsync<TerritoryDbo>().ConfigureAwait(false);

            list.ForEach(x => Territories.Add(x));
        }
    }
}
