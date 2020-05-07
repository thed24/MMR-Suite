using System;
using System.IO;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using Cacher.Service;
using RiotSharp;

[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace Cacher
{
    public class Function
    {
        private static readonly string ApiKey = Environment.GetEnvironmentVariable("API");
        private static readonly RiotService Connector = new RiotService(RiotApi.GetDevelopmentInstance(ApiKey, 15, 90));
        private static readonly DatabaseService DatabaseService = new DatabaseService();
        private static readonly SummonerController Controller = new SummonerController(Connector, DatabaseService);

        public static void Main(Stream stream)
        {
            var summoner = Environment.GetEnvironmentVariable("SUMMONER");
            Controller.ParseInput(summoner);
        }
    }
}