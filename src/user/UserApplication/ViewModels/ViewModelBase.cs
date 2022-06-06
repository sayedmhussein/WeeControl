using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    private bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        protected set
        {
            isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected static bool Validate<T>(T obj, out ICollection<ValidationResult> results)    
    {    
        results = new List<ValidationResult>();
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj), "You should pass the validation object.");
        }

        return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);    
    }

    public string GetDisplayName(Type viewModel, string name)
    {
        MemberInfo? property = viewModel.GetProperty(name);
        var attribute = property?.GetCustomAttributes(typeof(DisplayNameAttribute), true)
            .Cast<DisplayNameAttribute>().Single();
        return attribute?.DisplayName ?? throw new ArgumentNullException(name, "Didn't specify display attribute");
    }
    
    public string GetDisplayName<T>(Expression<Func<T>> expression)
    {
        var property = ((MemberExpression) expression.Body).Member;

        var attribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), true)
            .Cast<DisplayNameAttribute>().Single();
        return attribute?.DisplayName ?? throw new ArgumentNullException(property.Name, "Didn't specify display attribute");
    }

    private readonly IDevice device;
    
    protected ViewModelBase(IDevice device)
    {
        this.device = device;
    }
    
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    protected async Task<HttpResponseMessage> SendMessageAsync<T>(HttpRequestMessage message, T? payload = null, bool accurateLocation = false) where T : class
    {
        if (message.Content is null && message.Method != HttpMethod.Get)
        {
            message.Content = await GetResponseDtoAsHttpContentAsync(accurateLocation, payload);
        }

        try
        { 
            UpdateHttpAuthorizationHeader(await device.Security.GetTokenAsync());
            return await device.Server.HttpClient.SendAsync(message);
        }
        catch (HttpRequestException)
        {
            return new HttpResponseMessage(HttpStatusCode.BadGateway);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    protected async Task<bool> RefreshTokenAsync()
    {
        if (await device.Security.IsAuthenticatedAsync() == false)
        {
            return false;
        }

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.Route)),
            Version = new Version("1.0"),
            Method = HttpMethod.Put
        };

        var response = await SendMessageAsync<object>(message);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
            var token = responseDto?.Payload?.Token;
            if (responseDto is not null && token is not null)
            {
                await device.Storage.SaveAsync(nameof(TokenDtoV1.Token), token);
                await device.Storage.SaveAsync(nameof(TokenDtoV1.FullName), responseDto?.Payload?.FullName ?? string.Empty);
                await device.Storage.SaveAsync(nameof(TokenDtoV1.PhotoUrl), responseDto?.Payload?.PhotoUrl ?? string.Empty);
                await device.Security.UpdateTokenAsync(token);
                return true;
            }
            else
            {
                throw new NullReferenceException("Response DTO from server is null");
            }
        }
        
        if (response.StatusCode != HttpStatusCode.BadGateway)
        {
            await device.Security.DeleteTokenAsync();
            await device.Storage.ClearAsync();
        }

        return false;
    }

    private Task<(double? Latitude, double? Longitude)> GetCurrentLocationAsync(bool accurate)
    {
        return accurate ? device.Location.GetAccurateLocationAsync() : device.Location.GetLastKnownLocationAsync();
    }
    
    private async Task<HttpContent> GetResponseDtoAsHttpContentAsync<T>(bool locationAccuracy, T? payload = null) where T : class
    {
        var location = await GetCurrentLocationAsync(locationAccuracy);
        var dto = payload == null ? 
            RequestDto.Create(device.DeviceId, location.Latitude, location.Longitude) : 
            RequestDto.Create(payload, device.DeviceId, location.Latitude, location.Longitude);
        
        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
    }
    
    private void UpdateHttpAuthorizationHeader(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;
        
        device.Server.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", token);
    }
}