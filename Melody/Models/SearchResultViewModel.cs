namespace Melody.Models
{
    public class SearchResultViewModel
    {
        public List<Album> Albums { get; set; } = new List<Album>();  // Initialize the list
        public List<Artist> Artists { get; set; } = new List<Artist>();
        public List<Song> Songs { get; set; } = new List<Song>();
    }

}
