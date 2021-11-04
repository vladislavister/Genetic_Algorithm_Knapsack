using System;
using System.Collections.Generic;
using System.Linq;
namespace Genetic_Algorithm
{
    internal static class Program
    {
        private static readonly Random Randomizer = new Random();

        private const int KnapsackCapacity = 250;
        private const int ItemsAmount = 100;
        private const int MinItemsWorth = 2;
        private const int MaxItemsWorth = 30;
        private const int MinItemsWeight = 1;
        private const int MaxItemsWeight = 25;

        public static void Main()
        {
            var items = GenerateItems().ToArray();

            RunGenAlg(1, items);
            RunGenAlg(20, items);
            RunGenAlg(200, items);
            RunGenAlg(600, items);
            RunGenAlg(10000, items);
        }

        private static void RunGenAlg(int iterations, IEnumerable<Item> items)
        {
            var knapsack = new Rucksack(items, KnapsackCapacity);

            List<Population> bestPopulations = new List<Population>();
            for (var iterationNumber = 1; iterationNumber <= iterations; iterationNumber++)
            {
                var geneticProcessor = new GenAlg(knapsack, 100, 5)
                {
                    CurrentPopulation =
                    {
                        Iteration = iterationNumber
                    }
                };

                bestPopulations.Add(geneticProcessor.CurrentPopulation);
            }

            var bestPopulation = bestPopulations.OrderByDescending(p => p.WorthPercentage).First();

            Console.WriteLine($"Лучшая итерация: {bestPopulation.Iteration}" +
                $"\nСуммарный вес: {bestPopulation.TotalWeight}" +
                $"\nСуммарная ценность: {bestPopulation.Worth}" +
                $"\nЦенность в процентах: {bestPopulation.WorthPercentage}% ");
            Console.WriteLine("\nСредняя ценность в процентах: {0:####.####}%",
                bestPopulations.Average(p => p.WorthPercentage));

            Console.WriteLine("\n \n");
        }


        private static IEnumerable<Item> GenerateItems() =>
            Enumerable.Range(0, ItemsAmount)
                .Select(_ => new Item(
                    Randomizer.Next(MinItemsWorth, MaxItemsWorth),
                    Randomizer.Next(MinItemsWeight, MaxItemsWeight)));
    }
}
