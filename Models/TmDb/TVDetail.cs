namespace ReelJunkies.Models.TmDb
{
    public class TVDetail
    {

        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public Created_By[] created_by { get; set; }
        public int[]? episode_run_time { get; set; }
        public string first_air_date { get; set; }
        public TvGenre[] genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public bool in_production { get; set; }
        public string[] languages { get; set; }
        public string last_air_date { get; set; }
        public Last_Episode_To_Air last_episode_to_air { get; set; }
        public string name { get; set; }
        public object next_episode_to_air { get; set; }
        public Network[] networks { get; set; }
        public int? number_of_episodes { get; set; }
        public int? number_of_seasons { get; set; }
        public string[] origin_country { get; set; }
        public string original_language { get; set; }
        public string original_name { get; set; }
        public string overview { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public Production_Companies[] production_companies { get; set; }
        public Production_Countries[] production_countries { get; set; }
        public Season[] seasons { get; set; }
        public TvSpoken_Languages[] spoken_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string type { get; set; }
        public decimal vote_average { get; set; }
        public int? vote_count { get; set; }
        public TvVideos videos { get; set; }
        public TvCredits credits { get; set; }
        public TvReviews reviews { get; set; }
        public TvSimilar similar { get; set; }


    }

    public class Last_Episode_To_Air
    {
        public string air_date { get; set; }
        public int? episode_number { get; set; }
        public int? id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string production_code { get; set; }
        public int? runtime { get; set; }
        public int? season_number { get; set; }
        public string still_path { get; set; }
        public decimal vote_average { get; set; }
        public int? vote_count { get; set; }
    }

    public class TvVideos
    {
        public TvResult[] results { get; set; }
    }

    public class TvResult
    {
        public string iso_639_1 { get; set; }
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string site { get; set; }
        public int? size { get; set; }
        public string type { get; set; }
        public bool official { get; set; }
        public string published_at { get; set; }
        public string id { get; set; }
    }

    public class TvCredits
    {
        public Cast[] cast { get; set; }
        public Crew[] crew { get; set; }
    }

    public class TvCast
    {
        public bool adult { get; set; }
        public int? gender { get; set; }
        public int? id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public float popularity { get; set; }
        public string profile_path { get; set; }
        public string character { get; set; }
        public string credit_id { get; set; }
        public int? order { get; set; }
    }

    public class TvCrew
    {
        public bool adult { get; set; }
        public int? gender { get; set; }
        public int? id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public float popularity { get; set; }
        public string profile_path { get; set; }
        public string credit_id { get; set; }
        public string department { get; set; }
        public string job { get; set; }
    }

    public class TvReviews
    {
        public int? page { get; set; }
        public TvReviewResult[] results { get; set; }
        public int? total_pages { get; set; }
        public int? total_results { get; set; }
    }

    public class TvReviewResult
    {
        public string author { get; set; }
        public TvAuthor_Details author_details { get; set; }
        public string content { get; set; }
        public string created_at { get; set; }
        public string id { get; set; }
        public string updated_at { get; set; }
        public string url { get; set; }
    }

    public class TvAuthor_Details
    {
        public string name { get; set; }
        public string username { get; set; }
        public string avatar_path { get; set; }
        public decimal? rating { get; set; }
    }

    public class TvSimilar
    {
        public int? page { get; set; }
        public TvSimilarResult[] results { get; set; }
        public int? total_pages { get; set; }
        public int? total_results { get; set; }
    }

    public class TvSimilarResult
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public int[]? genre_ids { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string[] origin_country { get; set; }
        public string original_language { get; set; }
        public string original_name { get; set; }
        public string overview { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public string first_air_date { get; set; }
        public float vote_average { get; set; }
        public int? vote_count { get; set; }
    }



    public class TVCrew
    {
        public string job { get; set; }
        public string department { get; set; }
        public string credit_id { get; set; }
        public bool adult { get; set; }
        public int? gender { get; set; }
        public int? id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public float popularity { get; set; }
        public string profile_path { get; set; }
    }

    public class TvGuest_Stars
    {
        public string character { get; set; }
        public string credit_id { get; set; }
        public int? order { get; set; }
        public bool adult { get; set; }
        public int? gender { get; set; }
        public int? id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public float popularity { get; set; }
        public string profile_path { get; set; }
    }

    public class Created_By
    {
        public int? id { get; set; }
        public string credit_id { get; set; }
        public string name { get; set; }
        public int? gender { get; set; }
        public string profile_path { get; set; }
    }

    public class TvGenre
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Network
    {
        public string name { get; set; }
        public int? id { get; set; }
        public string logo_path { get; set; }
        public string origin_country { get; set; }
    }

    public class TvProduction_Companies
    {
        public int? id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public string origin_country { get; set; }
    }

    public class TvProduction_Countries
    {
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
    }

    public class Season
    {
        public string air_date { get; set; }
        public int? episode_count { get; set; }
        public int? id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public int? season_number { get; set; }
    }

    public class TvSpoken_Languages
    {
        public string english_name { get; set; }
        public string iso_639_1 { get; set; }
        public string name { get; set; }
    }


}
