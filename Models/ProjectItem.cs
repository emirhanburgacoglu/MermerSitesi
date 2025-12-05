using System.ComponentModel.DataAnnotations;

namespace MermerSitesi.Models
{
    public class ProjectItem // Artık Blog değil, ProjectItem!
    {
        [Key]
        public int Id { get; set; }

        // Türkçe Başlık/İçerik
        [Display(Name = "Proje Adı (TR)")]
        public string TitleTr { get; set; }

        [Display(Name = "Açıklama (TR)")]
        public string ContentTr { get; set; }

        // İngilizce Başlık/İçerik
        [Display(Name = "Project Name (EN)")]
        public string TitleEn { get; set; }

        [Display(Name = "Content (EN)")]
        public string ContentEn { get; set; }

        [Display(Name = "Resim URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Tarih")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}