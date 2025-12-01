using System.ComponentModel.DataAnnotations;

namespace MermerSitesi.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        // Türkçe Alanlar
        [Display(Name = "Başlık (TR)")]
        public string TitleTr { get; set; }

        [Display(Name = "İçerik (TR)")]
        public string ContentTr { get; set; }

        // İngilizce Alanlar
        [Display(Name = "Title (EN)")]
        public string TitleEn { get; set; }

        [Display(Name = "Content (EN)")]
        public string ContentEn { get; set; }

        // Ortak
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}