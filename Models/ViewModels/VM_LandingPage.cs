using ReelJunkies.Models.Database;
using ReelJunkies.Models.TmDb;
using System.Collections.Generic;

namespace ReelJunkies.Models.ViewModels
{
    public class VM_LandingPage
    {
        public List<Collection> CustomCollections { get; set; }
        public MovieSearch NowPlaying { get; set; }
        public MovieSearch Popular { get; set; }
        public MovieSearch TopRated { get; set; }
        public MovieSearch Upcomming { get; set; }
        public MovieSearch Horror { get; set; }
        public TvSearch TvPopular { get; set; }

        public TvSearch NetflixOriginals { get; set; }
        public TvSearch NetflixGems { get; set; }
    }
}
