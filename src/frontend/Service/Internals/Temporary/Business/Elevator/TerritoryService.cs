// using System.Net;
// using System.Net.Http.Json;
// using WeeControl.Common.SharedKernel;
// using WeeControl.Common.SharedKernel.Contexts.Temporary.DataTransferObjects;
// using WeeControl.Common.SharedKernel.RequestsResponses;
// using WeeControl.Frontend.AppService.Contexts.Temporary.Models;
// using WeeControl.Frontend.AppService.Interfaces;
//
// namespace WeeControl.Frontend.AppService.Contexts.Business.Elevator;
//
// internal class TerritoryService
// {
//     private readonly IGuiInterface device;
//     private readonly IServerOperation server;
//     private readonly string uriString;
//
//     public List<TerritoryModel> ListOfTerritories { get; }
//     
//     public TerritoryService(IGuiInterface device, IServerOperation server)
//     {
//         this.device = device;
//         this.server = server;
//         uriString = server.GetFullAddress(ApiRouting.Essential.Routes.Territory);
//         ListOfTerritories = new List<TerritoryModel>();
//     }
//
//     public async Task GetListOfTerritories()
//     {
//         var response = await server.Send(
//             new HttpRequestMessage
//             {
//                 RequestUri = new Uri(uriString),
//                 Version = new Version("1.0"),
//                 Method = HttpMethod.Get
//             });
//         
//         if (response.IsSuccessStatusCode)
//         {
//             var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<TerritoryDto>>>();
//             ListOfTerritories.Clear();
//             var list = content?.Payload;
//             if (list != null && list.Any())
//             {
//                 foreach (var t in list)
//                 {
//                     ListOfTerritories.Add(new TerritoryModel(t));
//                 }
//             }
//             
//             return;
//         }
//
//         switch (response.StatusCode)
//         {
//             case HttpStatusCode.BadGateway:
//                 await device.DisplayAlert("Check your internet connection");
//                 break;
//             case HttpStatusCode.Unauthorized:
//             case HttpStatusCode.Forbidden:
//                 await device.DisplayAlert("You are not authorized");
//                 break;
//             default:
//                 await device.DisplayAlert("Unexpected Error Occured");
//                 throw new ArgumentOutOfRangeException(response.StatusCode.ToString());
//         }
//     }
//
//     public async Task AddOrUpdateTerritory(TerritoryModel modelDto)
//     {
//         var response = await server.Send(
//             new HttpRequestMessage
//             {
//                 RequestUri = new Uri(uriString),
//                 Version = new Version("1.0"),
//                 Method = HttpMethod.Put
//             }, new TerritoryDto(null, modelDto));
//
//         switch (response.StatusCode)
//         {
//             case HttpStatusCode.OK:
//                 await device.DisplayAlert("Saved");
//                 break;
//             case HttpStatusCode.BadGateway:
//                 await device.DisplayAlert("Check your internet connection");
//                 break;
//             case HttpStatusCode.Unauthorized:
//             case HttpStatusCode.Forbidden:
//                 await device.DisplayAlert("You are not authorized");
//                 break;
//             default:
//                 await device.DisplayAlert("Unexpected Error Occured");
//                 throw new ArgumentOutOfRangeException(response.StatusCode.ToString());
//         }
//     }
// }