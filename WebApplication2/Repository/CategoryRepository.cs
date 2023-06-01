using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Category> CreateCat(Category category)
        {
            var cat = new Category
            {
                Id = category.Id,
                Name = category.Name
            };
            await _appDbContext.AddAsync(cat);
            _appDbContext.SaveChanges();
            return cat;
        }
    }
}
