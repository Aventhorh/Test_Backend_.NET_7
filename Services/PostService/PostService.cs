using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Backend_NET_7.Services.PostService
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
            var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetPostDto>(post);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> UpdatePost(UpdatePostDto updatedPost)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id == updatedPost.Id);
                if (post is null) throw new Exception($"Post with Id '{updatedPost.Id}' not found.");

                post.Name = updatedPost.Name;
                post.Description = updatedPost.Description;
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetPostDto>(post);
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