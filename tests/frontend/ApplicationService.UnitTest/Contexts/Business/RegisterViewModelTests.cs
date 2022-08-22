// using System.Net;
// using System.Net.Http;
// using WeeControl.Frontend.ApplicationService.Contexts.Essential.Models;
// using WeeControl.Frontend.ApplicationService.Interfaces;
// using WeeControl.Frontend.ApplicationService.Services;
// using WeeControl.SharedKernel.RequestsResponses;
//
// namespace WeeControl.Frontend.ApplicationService.UnitTest.Contexts.Business;
//
// public class RegisterViewModelTests : ViewModelTestsBase
// {
//     public RegisterViewModelTests() : base(nameof(CustomerViewModel))
//     {
//     }
//     
//     
//
//     private UserRegisterModel GetRegisterDto()
//     {
//         return new UserRegisterModel()
//         {
//             TerritoryId = nameof(IUserModel.TerritoryId),
//             FirstName = nameof(IUserModel.FirstName),
//             LastName = nameof(IUserModel.LastName),
//             Email = nameof(IUserModel.Email) + "@email.com",
//             Username = nameof(IUserModel.Username),
//             Password = nameof(IUserModel.Password),
//             MobileNo = "0123456789",
//             Nationality = "TST"
//         };
//     }
//
//     private HttpContent GetResponseContent()
//     {
//         var dto =
//             ResponseDto.Create<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url"));
//         return GetJsonContent(dto);
//     }
//
//     private CustomerViewModel GetViewModel(IDevice device)
//     {
//         return new CustomerViewModel(device, new ServerOperationService(device));
//     }
// }