using LearningMongoDB.Core.Entities;
using LearningMongoDB.Infra.Persistence;
using LearningMongoDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace LearningMongoDB.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly MongoConfig _config;
        public BooksController(IOptions<MongoConfig> options)
        {
            _config = options.Value;
        }

        [HttpPost]
        [SwaggerOperation("Create book")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public IActionResult Post(CreateBookInputModel model)
        {
            var book = new Book(model.Title, model.Author);
            var collection = GetCollection();
            
            collection.InsertOne(book);
            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation("Get all books")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public IActionResult GetAll() 
        {
            var collection = GetCollection();

            var books = collection.Find(new BsonDocument()).ToList();

            return Ok(books);
        }

        [HttpPost("{id}/reviews")]
        [SwaggerOperation("Create book review")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public IActionResult PostReview(string id, CreateBookReviewModel model)
        {
            var bookReview = new BookReview(model.Rating, model.Comment, model.Username);
            var filter = Builders<Book>.Filter.And(
                Builders<Book>.Filter.Eq(o => o.Id, id)
            );
            var definition = Builders<Book>.Update.Push(o => o.Reviews, bookReview);
            var collection = GetCollection();

            collection.UpdateOne(filter, definition);

            return NoContent();
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get book by ID")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public IActionResult GetById(string id)
        {
            var collection = GetCollection();

            var result = collection.Find(o => o.Id.Equals(id)).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        public IMongoCollection<Book> GetCollection() 
        {
            var client = new MongoClient(_config.ConnectionString);
            var database = client.GetDatabase(_config.Database);

            var collection = database.GetCollection<Book>("books");

            return collection;
        }
    }
}
