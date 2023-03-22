using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Backend_NET_7.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, DataContext context, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<bool>> Register(UserDto newUser)
        {
            var serviceResponse = new ServiceResponse<bool>();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

            var user = _mapper.Map<User>(newUser);
            user.PasswordHash = passwordHash;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            serviceResponse.Data = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<UserLoginDto>> Login(UserDto user)
        {
            var serviceResponse = new ServiceResponse<UserLoginDto>();
            var userInDb = await _context.Users.FirstOrDefaultAsync(c => c.Name == user.Name);

            if (userInDb == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found";
                return serviceResponse;
            }
            if (BCrypt.Net.BCrypt.Verify(user.Password, userInDb.PasswordHash))
            {
                var userToReturn = _mapper.Map<UserLoginDto>(userInDb);
                string token = CreateToken(user);
                serviceResponse.Token = token;
                serviceResponse.Data = userToReturn;
                serviceResponse.Success = true;
                serviceResponse.Message = "User logged in successfully";
                return serviceResponse;
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Wrong password";
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<bool>> Delete(int id)
        {
            var serviceResponse = new ServiceResponse<bool>();
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
            if (user is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found";
                return serviceResponse;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            serviceResponse.Data = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<UserUpdateDto>> Update(int id, UserDto request)
        {
            var serviceResponse = new ServiceResponse<UserUpdateDto>();
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
            if (user is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found";
                return serviceResponse;
            }
            else
            {
                user.Name = request.Name;
            }
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<UserUpdateDto>(user);
            return serviceResponse;
        }


        private string CreateToken(UserDto user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Token").Value!
            ));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}