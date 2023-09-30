using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<string> AuthenticateAsync(LoginModelDto loginModel);
    }
}
