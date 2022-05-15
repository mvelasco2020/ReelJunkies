using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using ReelJunkies.Enums;
using ReelJunkies.Models.Settings;
using ReelJunkies.Models.TmDb;
using ReelJunkies.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Text.Encodings.Web;

namespace ReelJunkies.Services
{
    public class TmDbMovieService : IRemoteMovieService
    {
        private readonly AppSettings _appSettings;

        //this needs to be registered in startup.cs
        private readonly IHttpClientFactory _httpClient;

        public TmDbMovieService(IOptions<AppSettings> appSettings,
                                IHttpClientFactory httpClient)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
        }

        public async Task<CombinedCredits> ActorCombinedCreditsAsync(int id)
        {
            //setup a default instance of movie search
            CombinedCredits ActorCombinedCredits = new();

            //assenble full request uri string
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/person/{id}/combined_credits";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.ReelJunkiesSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            //create a client and execute request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            try
            {

                var response = await client.SendAsync(request);

                //return the  Moviesearch object
                if (response.IsSuccessStatusCode)
                {
                    var deserialize = new DataContractJsonSerializer(typeof(CombinedCredits));
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    ActorCombinedCredits = (CombinedCredits)deserialize.ReadObject(responseStream);
                    ActorCombinedCredits.cast = ActorCombinedCredits.cast.Take(4).ToArray();
                    ActorCombinedCredits
                        .cast.ToList()
                        .ForEach(r =>
                        r.backdrop_path =
                        $"{_appSettings.TmDbSettings.BaseImagePath}" +
                        $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                        $"/{r.backdrop_path}");

                    ActorCombinedCredits
                        .cast.ToList()
                        .ForEach(r =>
                        r.poster_path =
                        $"{_appSettings.TmDbSettings.BaseImagePath}" +
                        $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                        $"/{r.poster_path}");

                }
            }
            catch (System.Exception)
            {

            }


            return ActorCombinedCredits;
        }

        public async Task<ActorDetail> ActorDetailAsync(int id)
        {
            ActorDetail actorDetail = new();

            //assenble full request uri string
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/person/{id}";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.ReelJunkiesSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            //create a client and execute request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            //return the  Moviesearch object
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var deserialize = new DataContractJsonSerializer(typeof(ActorDetail));
                actorDetail = deserialize.ReadObject(responseStream) as ActorDetail;
            }

            return actorDetail;
        }

        public async Task<MovieDetail> MovieDetailAsync(int id)
        {
            //setup a default instance of movie search
            MovieDetail movieDetail = new();

            //assenble full request uri string
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/movie/{id}";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.ReelJunkiesSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language },
                {"append_to_response", _appSettings.TmDbSettings.QueryOptions.AppendToResponse }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            //create a client and execute request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            //return the  Moviesearch object
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var deserialize = new DataContractJsonSerializer(typeof(MovieDetail));
                movieDetail = deserialize.ReadObject(responseStream) as MovieDetail;
            }

            return movieDetail;
        }

        public async Task<MovieSearch> MovieSearchAsync(MovieCategory category, int count, int page = 1)
        {
            //setup a default instance of movie search
            MovieSearch movieSearch = new();

            //assenble full request uri string
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/movie/{category}";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.ReelJunkiesSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language },
                {"append_to_response", _appSettings.TmDbSettings.QueryOptions.AppendToResponse },
                {"page", $"{page}" }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            //create a client and execute request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            try
            {

                var response = await client.SendAsync(request);

                //return the  Moviesearch object
                if (response.IsSuccessStatusCode)
                {
                    var deserialize = new DataContractJsonSerializer(typeof(MovieSearch));
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    movieSearch = (MovieSearch)deserialize.ReadObject(responseStream);
                    movieSearch.results = movieSearch.results.Take(count).ToArray();
                    movieSearch
                        .results.ToList()
                        .ForEach(r =>
                        r.poster_path =
                        $"{_appSettings.TmDbSettings.BaseImagePath}" +
                        $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                        $"/{r.poster_path}");

                }
            }
            catch (System.Exception)
            {

            }


            return movieSearch;

        }

        public async Task<MovieSearch> MovieSearchByGenre(string genre, int count)
        {
            //setup a default instance of movie search
            MovieSearch movieSearch = new();

            string encodedGenre = HttpUtility.UrlEncode(genre);

            //assenble full request uri string
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/discover/movie";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.ReelJunkiesSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language },
                {"include_adult", "false" },
                {"include_video", "false" },
                {"with_genres", $"{genre}" },
                {"page", _appSettings.TmDbSettings.QueryOptions.Page },

            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            //create a client and execute request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            try
            {
                var response = await client.SendAsync(request);

                //return the  Moviesearch object
                if (response.IsSuccessStatusCode)
                {
                    var deserialize = new DataContractJsonSerializer(typeof(MovieSearch));
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    movieSearch = (MovieSearch)deserialize.ReadObject(responseStream);
                    movieSearch.results = movieSearch.results.Take(count).ToArray();
                    movieSearch.results.ToList().ForEach(r =>
                                         r.poster_path =
                                         $"{_appSettings.TmDbSettings.BaseImagePath}" +
                                         $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                                         $"/{r.poster_path}");

                }
            }
            catch (System.Exception)
            {

            }
            return movieSearch;

        }

        public async Task<TvSearch> TVSearchAsync(TVCategory category, int count, int page)
        {
            //setup a default instance of movie search
            TvSearch tvSearch = new();

            //assenble full request uri string
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/tv/{category}";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.ReelJunkiesSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language },
                {"page", page.ToString()}
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            //create a client and execute request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            try
            {

                var response = await client.SendAsync(request);

                //return the  Moviesearch object
                if (response.IsSuccessStatusCode)
                {
                    var deserialize = new DataContractJsonSerializer(typeof(TvSearch));
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    tvSearch = (TvSearch)deserialize.ReadObject(responseStream);
                    tvSearch.results = tvSearch.results.Take(count).ToArray();
                    tvSearch
                        .results.ToList()
                        .ForEach(r =>
                        r.poster_path =
                        $"{_appSettings.TmDbSettings.BaseImagePath}" +
                        $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                        $"/{r.poster_path}");

                }
            }
            catch (System.Exception)
            {

            }


            return tvSearch;

        }

        public async Task<QueryAll> QueryAll(string queryString, int page)
        {
            QueryAll queryResult = new();

            string encodedQueryString = HttpUtility.UrlEncode(queryString);

            //assenble full request uri string
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/search/multi";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.ReelJunkiesSettings.TmDbApiKey },
                {"query",encodedQueryString },
                {"page", page.ToString() }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            //create a client and execute request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            try
            {
                var response = await client.SendAsync(request);

                //return the  Moviesearch object
                if (response.IsSuccessStatusCode)
                {
                    var deserialize = new DataContractJsonSerializer(typeof(QueryAll));
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    queryResult = (QueryAll)deserialize.ReadObject(responseStream);
                }
            }
            catch (System.Exception)
            {
            }
            return queryResult;
        }

        public async Task<TVDetail> TvDetailAsync(int id)
        {
            //setup a default instance of movie search
            TVDetail tvDetail = new();

            //assenble full request uri string
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/tv/{id.ToString()}";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.ReelJunkiesSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language },
                {"append_to_response", "videos,release_dates,credits,reviews,similar" }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            //create a client and execute request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            //return the  Moviesearch object
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var deserialize = new DataContractJsonSerializer(typeof(TVDetail));
                tvDetail = deserialize.ReadObject(responseStream) as TVDetail;
            }

            return tvDetail;
        }
    }
}
