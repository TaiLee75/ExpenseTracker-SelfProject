using ExpenseTrackerAPI.DTOs.Category;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Repositories.Interfaces;
using ExpenseTrackerAPI.Services.Interfaces;

namespace ExpenseTrackerAPI.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetUserCategoriesAsync(Guid userId)
        {
            var categories = await _categoryRepository.GetUserCategoriesAsync(userId);
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type,
                Icon = c.Icon,
                Color = c.Color
            });
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id, Guid userId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type,
                Icon = category.Icon,
                Color = category.Color
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type,
                Icon = dto.Icon,
                Color = dto.Color,
                UserId = userId
            };

            await _categoryRepository.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type,
                Icon = category.Icon,
                Color = category.Color
            };
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(Guid id, Guid userId, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);
            if (category == null) return null;

            category.Name = dto.Name;
            category.Icon = dto.Icon;
            category.Color = dto.Color;

            await _categoryRepository.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type,
                Icon = category.Icon,
                Color = category.Color
            };
        }

        public async Task<bool> DeleteCategoryAsync(Guid id, Guid userId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);
            if (category == null) return false;

            try
            {
                await _categoryRepository.DeleteCategoryAsync(category);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("Không thể xóa danh mục này vì đã có giao dịch sử dụng nó.");
            }
        }
    }
}
