using System.ComponentModel.DataAnnotations;

namespace MermerSitesi.Models
{
    public class ProjectItem
    {
        [Key]
        public int Id { get; set; }

        // Proje İsimleri
        [Display(Name = "Proje Adı (TR)")]
        [Required(ErrorMessage = "Türkçe isim zorunludur.")]
        public string TitleTr { get; set; }

        [Display(Name = "Project Name (EN)")]
        public string TitleEn { get; set; }

        [Display(Name = "Project Naam (NL)")]
        public string TitleNl { get; set; }

        // Ülke Bilgisi (Yeni)
        [Display(Name = "Yapılan Ülke")]
        public string Country { get; set; }

        [Display(Name = "Resim URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Tarih")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;


        // YENİ: Projenin diğer detay resimleri
        public List<ProjectImage> Images { get; set; } = new List<ProjectImage>();


    }
}