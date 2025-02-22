namespace MangasBlazor.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string? Name { get; set; }

        public string? IconCSS { get; set; }
    }
}
