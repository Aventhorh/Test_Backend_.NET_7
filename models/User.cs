using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet7Learning.models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
        public string City { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}