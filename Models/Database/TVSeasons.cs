namespace ReelJunkies.Models.Database
{
    public class TVSeasons
    {

        public int? TmDbId { get; set; }
        public string AirDate { get; set; }
        public TVEpisodes[] Episodes { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public byte[] PosterPath { get; set; }
        public int? SeasonNumber
        {
            get; set;
        }
    }
}
