using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Services
{
    public class PostService : IPostService
    {
        private readonly IMongoCollection<User> _usersCollections;
        private readonly IMongoCollection<Post> _postsCollections;


        public PostService(IMongoBlogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _usersCollections = database.GetCollection<User>(settings.UsersCollectionName);
            _postsCollections = database.GetCollection<Post>(settings.PostsCollectionName);
        }

        public async Task<List<Post>> Get()
        {
            return await (await _postsCollections.FindAsync(post => true)).ToListAsync();
        }

        public async Task<Post> Get(string id)
        {
            return (await _postsCollections.FindAsync(post => post.Id == id)).FirstOrDefault();
        }

        public async Task<Post> Create(CreatePostDto createPostDto)
        {
            var author = await (await _usersCollections.FindAsync(user => user.Id == createPostDto.AuthorId)).FirstOrDefaultAsync();
            var post = new Post
            {
                AuthorId = createPostDto.AuthorId,
                Author = author,
                Body = createPostDto.Body,
                Title = createPostDto.Title,
                CreatedOn = DateTime.Now
            };
            await _postsCollections.InsertOneAsync(post);
            return post;
        }

        public async Task Update(string id, UpdatePostDto updatePostDto)
        {
            var post = await Get(id);
            post.Title = updatePostDto.Title;
            post.Body = updatePostDto.Body;
            await _postsCollections.ReplaceOneAsync(post => post.Id == id, post);
        }

        public Task Remove(string id)
        {
            return _postsCollections.DeleteOneAsync(post => post.Id == id);
        }

        public async Task AddComment(string id, CreateCommentDto createCommentDto)
        {
            var author = await (await _usersCollections.FindAsync(user => user.Id == createCommentDto.AuthorId)).FirstOrDefaultAsync();

            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                AuthorId = createCommentDto.AuthorId,
                Author = author,
                Content = createCommentDto.Content,
                CreatedOn = DateTime.Now
            };

            await _postsCollections.UpdateOneAsync(post => post.Id == id, Builders<Post>.Update.Push(nameof(Post.Comments), comment));
        }

        public Task RemoveComment(string id, string commentId)
        {
            var filterCommentById = Builders<Comment>.Filter.Eq(nameof(Comment.Id), commentId);
            var updatePostComments = Builders<Post>.Update.PullFilter(nameof(Post.Comments), filterCommentById);
            return _postsCollections.UpdateOneAsync(post => post.Id == id, updatePostComments);
        }

        public async Task<List<Post>> GetPostsByCommentAuthor(string authorId)
        {
            var filter = Builders<Post>.Filter.Where(post => post.Comments.Any(e => e.AuthorId == authorId));
            return await (await _postsCollections.FindAsync(filter)).ToListAsync();
        }

        public async Task<List<Post>> GetPostByAuthor(string authorId)
        {
            return await (await _postsCollections.FindAsync(post => post.AuthorId == authorId)).ToListAsync();
        }
    }
}
