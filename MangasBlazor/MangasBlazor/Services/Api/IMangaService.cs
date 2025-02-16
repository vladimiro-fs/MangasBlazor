namespace MangasBlazor.Services.Api
{
    using MangasBlazor.Models.DTOs;

    public interface IMangaService
    {
        Task<IEnumerable<MangaDTO>> GetMangas();

        Task<MangaDTO> GetManga(int id);

        Task<MangaDTO> CreateMangas(MangaDTO mangaDto);

        Task<MangaDTO> UpdateManga(int id, MangaDTO mangaDto);

        Task<bool> DeleteManga(int id);

        Task<IEnumerable<MangaDTO>> GetMangaByCategory(int categoryId);
    }
}
