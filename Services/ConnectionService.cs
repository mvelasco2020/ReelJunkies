﻿using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

namespace ReelJunkies.Services
{
    public class ConnectionService
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //this will only be available if app is running on heroku
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return
                string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);

        }

        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }
    }
}
