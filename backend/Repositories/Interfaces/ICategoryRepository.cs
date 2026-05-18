using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetUserCategoriesAsync(Guid userId);
        Task<Category?> GetCategoryByIdAsync(Guid id, Guid userId);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
