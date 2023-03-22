using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Backend_NET_7.Services.PostService
{
    public interface IPostService
    {
        Task<ServiceResponse<List<GetPostDto>>> GetAllPosts();
        Task<ServiceResponse<GetPostDto>> GetPostById(int id);
        Task<ServiceResponse<List<GetPostDto>>> AddPost(AddPostDto newPost);
        Task<ServiceResponse<GetPostDto>> UpdatePost(UpdatePostDto updatedPost);
        Task<ServiceResponse<List<GetPostDto>>> DeletePost(int id);
    }
}