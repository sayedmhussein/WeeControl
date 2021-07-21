using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using WeeControl.Applications.BaseLib.Entities.Territory;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Territory.Entities.DtosV1;
using WeeControl.SharedKernel.Extensions;

namespace WeeControl.Applications.BaseLib.ViewModels.HumanResource
{
    public class TerritoryListViewModel : ObservableObject
    {
        #region Private
        private readonly IBasicDatabase database;
        private readonly IDevice device;
        private readonly IServerService server;
        #endregion

        #region Properties
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                SetProperty(ref searchText, value);
                RefreshTerritoryList();
            } 
        }

        private bool isRefreshing;
        public bool IsRefreshing {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        private TerritoryDbo selectedTerritory;
        public TerritoryDbo SelectedTerritory
        {
            get => selectedTerritory;
            set
            {
                OpenTerritoryDetail.Execute(value.Id);
                SetProperty(ref selectedTerritory, value);
            }
        }

        private List<TerritoryDbo> territoryList;
        public ObservableCollection<TerritoryDbo> TerritoryList { get; set; }
        #endregion

        #region Commands
        public IAsyncRelayCommand AddTerritoryCommand { get; }
        public IAsyncRelayCommand OpenTerritoryDetail { get; }
        public IAsyncRelayCommand RefreshCommand { get; }
        #endregion

        public TerritoryListViewModel()
            : this(
                  Ioc.Default.GetService<IDevice>(),
                  Ioc.Default.GetService<IServerService>(),
                  Ioc.Default.GetService<IBasicDatabase>()
                  )
        {
        }

        public TerritoryListViewModel(IDevice device, IServerService server, IBasicDatabase database)
        {
            this.device = device ?? throw new ArgumentNullException();
            this.server = server ?? throw new ArgumentNullException();
            this.database = database ?? throw new ArgumentNullException();

            TerritoryList = new ObservableCollection<TerritoryDbo>();

            AddTerritoryCommand = new AsyncRelayCommand(async () => await device.NavigateToPageAsync("TerritoryDetailPage"));

            OpenTerritoryDetail = new AsyncRelayCommand<Guid>(async (id) => await device.NavigateToPageAsync($"TerritoryDetailPage?id={id}"));

            RefreshCommand = new AsyncRelayCommand(PopulateCollection);
        }

        public async Task PopulateCollection()
        {
            await LoadDataFromServeAsync();

            territoryList = await database.GetAllAsync<TerritoryDbo>();

            RefreshTerritoryList();
        }

        public async Task LoadDataFromServeAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Version = new Version("1.0"),
                //Content = server.GetHttpContentAsJson(dto),
                RequestUri = server.GetUri(ApiRouteEnum.Territory)
            };

            try
            {
                var response = await server.GetHttpResponseMessageAsync(request);
                response.EnsureSuccessStatusCode();

                var list = new List<TerritoryDbo>();
                var responseDto = await response.Content.ReadAsAsync<IEnumerable<TerritoryDto>>();

                await database.InitAsync<TerritoryDbo>();
                await database.DeleteAllAsync<TerritoryDbo>();

                foreach (var x in responseDto)
                {
                    list.Add(x.ToDbo<TerritoryDto, TerritoryDbo>());
                }

                await database.InsertAsync<TerritoryDbo>(list);
            }
            catch
            { }
        }

        private void RefreshTerritoryList()
        {
            IsRefreshing = true;

            TerritoryList.Clear();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var item in territoryList)
                {
                    TerritoryList.Add(item);
                }
            }
            else
            {
                foreach (var item in territoryList.Where(x => x.Name.ToLower().Contains(SearchText.ToLower())))
                {
                    TerritoryList.Add(item);
                }
            }

            IsRefreshing = false;
        }
    }
}
