using System.ComponentModel.DataAnnotations;

namespace MermerSitesi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        // Türkçe İsim
        [Display(Name = "Ürün Adı (TR)")]
        public string NameTr { get; set; }

        // İngilizce İsim
        [Display(Name = "Product Name (EN)")]
        public string NameEn { get; set; }

        // Ortak Özellikler
        [Display(Name = "Resim")]
        public string ImageUrl { get; set; } // Örn: /images/1.png

        [Display(Name = "Kategori")]
        public string Category { get; set; } // white, black, brown...

        [Display(Name = "Sıra")]
        public int DisplayOrder { get; set; }
    }
}