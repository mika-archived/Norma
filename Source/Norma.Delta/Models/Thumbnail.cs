namespace Norma.Delta.Models
{
    public class Thumbnail
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public virtual Episode Episode { get; set; }
    }
}