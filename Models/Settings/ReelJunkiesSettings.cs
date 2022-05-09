namespace ReelJunkies.Models.Settings
{
    public class ReelJunkiesSettings
    {
        public string TmDbApiKey { get; set; }
        public string DefaultYouTubeKey { get; set; }
        public string DefaultPosterSize { get; set; }

        public string DefaultCastImage { get; set; }

        public string DefaultBackdropSize { get; set; }

        public string PosterUnavailable { get; set; }
        public DefaultCollection DefaultCollection { get; set; }

        public DefaultCredentials DefaultCredentials { get; set; }


    }
}