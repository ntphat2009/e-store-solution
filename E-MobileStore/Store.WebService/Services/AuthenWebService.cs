using Newtonsoft.Json;
using Store.Domain.Entities;
using Store.WebService.APIs.Interfaces;
using Store.WebService.DTO;
using Store.WebService.Response;
using Store.WebService.Services.Interfaces;
using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services
{
    public class AuthenWebService : IAuthenWebService
    {
        private readonly IAuthenticationApi _authenApi;
        private readonly HttpClient _httpClient;

        public AuthenWebService(IAuthenticationApi authenApi)
        {
            _authenApi = authenApi;
            _httpClient = new HttpClient();
        }

        public async Task<UserVM> GetUser(string username)
        {
            try
            {
                var uri = _authenApi.GetUser(username);
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var responseApi = await response.Content.ReadAsStringAsync();
                    var content = JsonConvert.DeserializeObject<UserResponse>(responseApi);
                    if (content != null && content.result != null)
                    {
                        var result = content.result;
                        return new UserVM()
                        {
                            Email = result?.Email,
                            EmailConfirmed = result.EmailConfirmed,
                            FirstName = result.FirstName,
                            FullName = result.FullName,
                            LastName = result.LastName,
                            Id = result.Id,
                            PhoneNumber = result?.PhoneNumber,
                            UserName = result.UserName,
                        };
                    }
                }
                return new UserVM();

            }
            catch
            {
                return new UserVM();
            }
        }

        public async Task<string> SignIn(SignInDTO signInDTO)
        {
            try
            {
                var uri = _authenApi.SignIn();
                var jsonContent = JsonConvert.SerializeObject(signInDTO);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, httpContent);
                var responseApi = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<AuthenResponse>(responseApi);
                if (response.IsSuccessStatusCode)
                {
                    var result = content.result;
                    return result;
                }
                else
                {
                    return content.statusCode.ToString();
                }
            }
            catch
            {
                throw new Exception("SignIN error");
            }
        }

        public async Task<string> SignUp(SignUpDTO signUpDTO)
        {
            try
            {
                var uri = _authenApi.SignUp();
                var jsonContent = JsonConvert.SerializeObject(signUpDTO);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.StatusCode.ToString();
                    return result;
                }
                var responseApi = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<AuthenResponse>(responseApi);
                var errorMessage = content.result.ToString();
                return errorMessage;
            }
            catch
            {
                throw new Exception("SignIN error");
            }
        }
    }
}
