using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Backend_NET_7.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<bool>> Register(UserDto newUser);
        Task<ServiceResponse<UserLoginDto>> Login(UserDto newUser);
        Task<ServiceResponse<bool>> Delete(int id);
        Task<ServiceResponse<UserUpdateDto>> Update(int id, UserDto request);
    }
}