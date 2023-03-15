using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet7Learning.Dtos.User
{
    public class UserDto
    {
        public required string Name { get; set; }
        public required string SurName { get; set; }
        public required int Age { get; set; }
        public required string City { get; set; }
        public required string Password { get; set; }
    }
}