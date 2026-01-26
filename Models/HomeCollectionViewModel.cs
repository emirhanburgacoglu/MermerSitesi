using MermerSitesi.Models; // Product modelinin olduğu yeri tanıttık

namespace MermerSitesi.ViewModels
{
    public class HomeCollectionViewModel
    {
        public List<Product> Travertines { get; set; }
        public List<Product> Marbles { get; set; }
        public List<Product> Limestones { get; set; }
        public List<Product> Onyxes { get; set; }
    }
}