using AutoMapper;
using FreeCourse.Services.Catalog.Configurations;
using FreeCourse.Services.Catalog.Models;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    internal class CategoryService
    {
        //önce vt yolu nerden bağlanacağımız gibi bilgiler lazım
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMongoCollection<Category> categoryCollection, IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }
    }
}
