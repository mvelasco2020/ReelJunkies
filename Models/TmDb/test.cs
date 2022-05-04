
public class Rootobject
{
    public bool adult { get; set; }
    public string backdrop_path { get; set; }
    public object belongs_to_collection { get; set; }
    public int budget { get; set; }
    public Genre[] genres { get; set; }
    public string homepage { get; set; }
    public int id { get; set; }
    public string imdb_id { get; set; }
    public string original_language { get; set; }
    public string original_title { get; set; }
    public string overview { get; set; }
    public float popularity { get; set; }
    public string poster_path { get; set; }
    public object[] production_companies { get; set; }
    public object[] production_countries { get; set; }
    public string release_date { get; set; }
    public int revenue { get; set; }
    public int runtime { get; set; }
    public object[] spoken_languages { get; set; }
    public string status { get; set; }
    public string tagline { get; set; }
    public string title { get; set; }
    public bool video { get; set; }
    public float vote_average { get; set; }
    public int vote_count { get; set; }
    public Similar similar { get; set; }
}

public class Similar
{
    public int page { get; set; }
    public Result[] results { get; set; }
    public int total_pages { get; set; }
    public int total_results { get; set; }
}

public class Result
{
    public bool adult { get; set; }
    public string backdrop_path { get; set; }
    public int[] genre_ids { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public string original_language { get; set; }
    public string original_title { get; set; }
    public string overview { get; set; }
    public float popularity { get; set; }
    public string poster_path { get; set; }
    public string release_date { get; set; }
    public bool video { get; set; }
    public float vote_average { get; set; }
    public int vote_count { get; set; }
}

public class Genre
{
    public int id { get; set; }
    public string name { get; set; }
}
