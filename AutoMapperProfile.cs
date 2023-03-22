using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Backend_NET_7
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, GetPostDto>();
            CreateMap<AddPostDto, Post>();
            CreateMap<UserDto, User>();
        }
    }
}