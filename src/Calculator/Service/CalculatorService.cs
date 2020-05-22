namespace Calculator.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cacher.Model;
    using Calculator.Model;

    public class CalculatorService
    {
        private DatabaseService _databaseService;

        public CalculatorService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void Calculate(string[] args)
        {
            var summonerName = args[0];
            var summoners = _databaseService.GetAll();
            var chosenSummoner = summoners.First(x => x.Name.Equals(summonerName));
            var filteredSummoners = FilterSummoners(summoners, chosenSummoner);

            var (serverAverageGain, serverAverageLoss) = CalculateAverages(filteredSummoners);
            var (playerAverageGain, playerAverageLoss) = CalculateAverages(new[] {chosenSummoner});

            Console.WriteLine("Server avg gain = " + serverAverageGain + " and avg loss = " + serverAverageLoss);
            Console.WriteLine("Your avg gain = " + playerAverageGain + " and avg loss = " + playerAverageLoss);
        }

        private static IEnumerable<SummonerStats> FilterSummoners(IEnumerable<SummonerStats> summoners,
            SummonerStats chosenSummoner)
        {
            return summoners
                .Where(x => x.LpLog.Count > 1)
                .Where(x => x.Rank.Equals(chosenSummoner.Rank));
        }

        private (double gainAverage, double lossAverage) CalculateAverages(IEnumerable<SummonerStats> summoners)
        {
            var gainsAveraged = 0;
            var lossesAveraged = 0;
            double serverGain = 0;
            double serverLoss = 0;

            foreach (var summoner in summoners)
            {
                var summonerAverage = CalculateLpIncrements(summoners, summoner.Name);

                var lpGains = summonerAverage.Where(x => x.IsLpGain);
                if (lpGains.Count() != 0)
                {
                    serverGain += summonerAverage.Where(x => x.IsLpGain).Average(x => x.Value);
                    gainsAveraged++;
                }

                var lpLosses = summonerAverage.Where(x => !x.IsLpGain);
                if (!lpLosses.Any()) continue;
                {
                    serverLoss += summonerAverage.Where(x => !x.IsLpGain).Average(x => x.Value);
                    lossesAveraged++;
                }
            }

            var gainAverage = Math.Round(serverGain / gainsAveraged);
            var lossAverage = Math.Round(serverLoss / lossesAveraged);

            return (gainAverage, lossAverage);
        }

        private IEnumerable<LpIncrement> CalculateLpIncrements(IEnumerable<SummonerStats> summoners,
            string summonerName)
        {
            var summoner = summoners.First(x => x.Name.Equals(summonerName));
            var summonerLpLog = summoner.LpLog.Select(int.Parse).ToList();
            var lpIncrements = CreateLpIncrements(summonerLpLog);
            return lpIncrements.Where(x => x.Value != 75).ToList();
        }

        private IEnumerable<LpIncrement> CreateLpIncrements(IReadOnlyList<int> lpLog)
        {
            var lpIncrementLog = new List<LpIncrement>();
            for (var i = 0; i < lpLog.Count - 1; i++)
            {
                var gainOrLoss = lpLog[i] - lpLog[i + 1];
                lpIncrementLog.Add(gainOrLoss > 0
                    ? new LpIncrement(false, gainOrLoss)
                    : new LpIncrement(true, Math.Abs(gainOrLoss)));
            }

            return lpIncrementLog;
        }
    }
}