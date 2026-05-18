using ExpenseTrackerAPI.DTOs.Category;

namespace ExpenseTrackerAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetUserCategoriesAsync(Guid userId);
        Task<CategoryDto?> GetCategoryByIdAsync(Guid id, Guid userId);
        Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryDto dto);
        Task<CategoryDto?> UpdateCategoryAsync(Guid id, Guid userId, UpdateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(Guid id, Guid userId);
    }
}
