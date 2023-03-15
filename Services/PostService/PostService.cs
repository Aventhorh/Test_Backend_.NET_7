using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet7Learning.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PostService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> AddPost(AddPostDto newPost)
        {
            var serviceResponse = new ServiceResponse<List<GetPostDto>>();
            var post = _mapper.Map<Post>(newPost);

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _context.Posts.Select(c => _mapper.Map<GetPostDto>(c)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> GetAllPosts()
        {
            var serviceResponse = new ServiceResponse<List<GetPostDto>>();
            var dbPosts = await _context.Posts.ToListAsync();
            serviceResponse.Data = dbPosts.Select(c => _mapper.Map<GetPostDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> GetPostById(int id)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            var dbPost = await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetPostDto>(dbPost);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> UpdatePost(UpdatePostDto updatedPost)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            try
            {
                var dbPost = await _context.Posts.FirstOrDefaultAsync(c => c.Id == updatedPost.Id);
                if (dbPost is null) throw new Exception($"Post with Id '{updatedPost.Id}' not found.");

                dbPost.Name = updatedPost.Name;
                dbPost.Description = updatedPost.Description;

                serviceResponse.Data = _mapper.Map<GetPostDto>(dbPost);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> DeletePost(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetPostDto>>();
            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post is null) throw new Exception($"Post with Id '{id}' not found.");

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _context.Posts.Select(c => _mapper.Map<GetPostDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}