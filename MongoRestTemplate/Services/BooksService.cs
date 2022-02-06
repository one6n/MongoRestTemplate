using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoRestTemplate.Models;

namespace MongoRestTemplate.Services
{
    public class BooksService
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Book> _booksCollection;

        public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            _client = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            _database = _client.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            _booksCollection = _database.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
        }
        public async Task<List<Book>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Book updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
