using System.ComponentModel.DataAnnotations;

namespace MermerSitesi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        // Türkçe İsim
        [Required(ErrorMessage = "Lütfen ürün adını Türkçe giriniz.")]
        [Display(Name = "Ürün Adı (TR)")]
        public string NameTr { get; set; }

        // İngilizce İsim
        [Required(ErrorMessage = "Please enter the product name in English.")]
        [Display(Name = "Product Name (EN)")]
        public string NameEn { get; set; }

        // Flemenkçe (Hollandaca) İsim
        [Required(ErrorMessage = "Voer de productnaam in het Nederlands in.")]
        [Display(Name = "Product Naam (NL)")]
        public string NameNl { get; set; }

        // Ortak Özellikler
        [Display(Name = "Resim")]
        public string ImageUrl { get; set; } // Örn: /images/1.png

        [Display(Name = "Kategori")]
        public string Category { get; set; } // travertine, marble, limestone, onyx

        [Display(Name = "Sıra")]
        public int DisplayOrder { get; set; }
    }
}