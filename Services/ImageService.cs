using Microsoft.AspNetCore.Http;
using ReelJunkies.Services.Interfaces;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReelJunkies.Services
{
    public class ImageService : IImageService

    {

        private readonly IHttpClientFactory _httpClient;

        public ImageService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        //get image from database
        public string DecodeImage(byte[] poster, string contentType)
        {
            if (poster == null)
            {
                return null;
            }

            var posterImage = Convert.ToBase64String(poster);
            return $"data:{contentType};base64,{posterImage}";
        }

        //store image from form to DB as a byte array

        public async Task<byte[]> EncodeImageAsync(IFormFile poster)
        {

            if (poster == null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            await poster.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }


        //store image from url to DB as a byte array
        public async Task<byte[]> EncodeImageURLAsync(string imageURL)
        {
            var client = _httpClient.CreateClient();
            var respons = await client.GetAsync(imageURL);

            using Stream strean = await respons.Content.ReadAsStreamAsync();
            var memoryStream = new MemoryStream();
            await strean.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
