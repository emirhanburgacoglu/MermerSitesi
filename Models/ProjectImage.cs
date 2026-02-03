namespace MermerSitesi.Models
{
    public class ProjectImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } // Resmin yolu

        // Hangi projeye ait olduğu
        public int ProjectItemId { get; set; }
        public ProjectItem ProjectItem { get; set; }
    }
}