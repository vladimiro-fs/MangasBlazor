namespace MangasBlazor.Services.Api
{
    using MangasBlazor.Models.DTOs;

    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategories();

        Task<CategoryDTO> GetCategory(int id);
    }
}
