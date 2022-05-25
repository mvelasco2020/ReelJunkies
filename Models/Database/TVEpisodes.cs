namespace ReelJunkies.Models.Database
{
    public class TVEpisodes
    {

        public class Episode
        {

            public int Id { get; set; }
            public string AirDate { get; set; }
            public int EpisodeNumber { get; set; }
            public int TmDbEpisodeid { get; set; }
            public string Name { get; set; }
            public string OverView { get; set; }
            public int Runtime { get; set; }
            public int SeasonNumber { get; set; }
            public string PosterPath { get; set; }
            public float Votes { get; set; }
        }

    }
}
