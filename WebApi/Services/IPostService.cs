using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IPostService
    {
        Task AddComment(string id, CreateCommentDto createCommentDto);
        Task RemoveComment(string id, string commentId);
        Task<Post> Create(CreatePostDto createPostDto);
        Task<List<Post>> Get();
        Task<Post> Get(string id);
        Task Remove(string id);
        Task Update(string id, UpdatePostDto updatePostDto);
        Task<List<Post>> GetPostsByCommentAuthor(string authorId);
        Task<List<Post>> GetPostByAuthor(string authorId);
    }
}