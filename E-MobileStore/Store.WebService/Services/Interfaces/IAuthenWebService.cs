using Store.Domain.Entities;
using Store.WebService.DTO;
using Store.WebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.WebService.Services.Interfaces
{
    public interface IAuthenWebService
    {
        Task<string> SignIn(SignInDTO signInDTO);
        Task<string> SignUp(SignUpDTO signUpDTO);
        Task<UserVM> GetUser(string username);
    }
}
