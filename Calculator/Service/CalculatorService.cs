namespace Calculator.Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Cacher.Model;
    using Calculator.Model;

    public class CalculatorService
    {
        public void Calculate(string[] args, List<SummonerStats> summoners)
        {
            var summonerName = args[0];
            var gainsAveraged = 0;
            var lossesAveraged = 0;
            double serverGain = 0;
            double serverLoss = 0;
            var filteredSummoners = summoners.Where(x => x.LpLog.Count > 1);
            
            foreach (var summoner in filteredSummoners)
            {
                var summonerAverage = CalculateIncrements(summoners, summoner.Name);
                
                var lpGains = summonerAverage.Where(x => x.IsLpGain);
                if (lpGains.Count() != 0)
                {
                    serverGain += summonerAverage.Where(x => x.IsLpGain).Average(x => x.Value); 
                    gainsAveraged++;
                }

                var lpLosses = summonerAverage.Where(x => !x.IsLpGain);
                if (lpLosses.Count() != 0)
                {
                    serverLoss += summonerAverage.Where(x => !x.IsLpGain).Average(x => x.Value);
                    lossesAveraged++;
                }
            }

            var serverAverageGain = Math.Round(serverGain / gainsAveraged);
            var serverAverageLoss = Math.Round(serverLoss / lossesAveraged);

            var increments = CalculateIncrements(summoners, summonerName);

            var averageGain = Math.Round(increments.Where(x => x.IsLpGain).Average(x => x.Value));
            var averageLoss = Math.Round(increments.Where(x => !x.IsLpGain).Average(x => x.Value));

            Console.WriteLine("Server average gain = " + serverAverageGain + " and average loss = " + serverAverageLoss);
            Console.WriteLine("Your average gain = " + averageGain + " and average loss = " + averageLoss);
        }

        private List<LpIncrement> CalculateIncrements(IEnumerable<SummonerStats> summoners, string summonerName)
        {
            var summoner = summoners.First(x => x.Name.Equals(summonerName));
            var summonerLpLog = summoner.LpLog.Select(int.Parse).ToList();
            return LpIncrementCalculator(summonerLpLog);

        }

        private List<LpIncrement> LpIncrementCalculator(List<int> lpLog)
        {
            var lpIncrementLog = new List<LpIncrement>();
            for (var i = 0; i < lpLog.Count() - 1; i++)
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