using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCat(Category category);
    }
}