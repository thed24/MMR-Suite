using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;

[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace Cacher
{
    using System;
    using Cacher.Service;
    using RiotSharp;

    public class Function
    {
        private static readonly string ApiKey = Environment.GetEnvironmentVariable("API");
        private static readonly RiotService Connector = new RiotService(RiotApi.GetDevelopmentInstance(ApiKey, 10, 50));
        private static readonly DatabaseService DatabaseService = new DatabaseService();
        private static readonly SummonerController Controller = new SummonerController(Connector, DatabaseService);

        public static void Main()
        {
            var summoner = Environment.GetEnvironmentVariable("SUMMONER");
            Controller.ParseInput(summoner);
        }
    }
}