using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace Genetic_Algorithm
{
    public class GenAlg
    {
        public Random rand = new Random();
        public static Rucksack Rucksack { get; set; }
        public Population CurrentPopulation { get; set; }

        public GenAlg(Rucksack rucksack, int iterations, int mutationChance)
        {
            Rucksack = rucksack;
            CurrentPopulation = new Population(rucksack, new bool[Rucksack.Items.Length]);

            var bestPopulations = new List<Population>();
            var populations = GeneratePopulations();

            for (var iternumber = 1; iternumber <= iterations; iternumber++)
            {
                var newPopulation = populations.Selection()
                    .CrossingOnePoint(populations[rand.Next(populations.Count)], rucksack)
                    .MutationChance(rucksack, mutationChance)
                    .LocalUpgrade();

                var upgradedPopulation = new Population(rucksack, newPopulation.SelectedItems)
                { Iteration = iternumber };

                if (upgradedPopulation.Worth > CurrentPopulation.Worth)
                    CurrentPopulation = upgradedPopulation;

                Population.AddDelete(populations, upgradedPopulation);
                if (upgradedPopulation.Worth != 0 &&
                    !bestPopulations.Any(bp => bp.Worth == upgradedPopulation.Worth &&
                                               bp.TotalWeight == upgradedPopulation.TotalWeight))
                // as an optimization we can store 2, 4 or 8 best populations
                {
                    bestPopulations.Add(upgradedPopulation);
                }
            }
        }




        public static List<Population> GeneratePopulations()
        {
            var itemsAmount = Rucksack.Items.Length;
            var populations = new List<Population>();

            for (var i = 0; i < itemsAmount; i++)
            {
                var selectedItems = new bool[itemsAmount];
                selectedItems[i] = true;
                populations.Add(new Population(Rucksack, selectedItems));
            }
            return populations;
        }


    }

    public class Rucksack
    {
        public int Capacity { get; set; }
        public Item[] Items { get; set; }

        public Rucksack(IEnumerable<Item> items, int capacity)
        {
            Capacity = capacity;
            Items = items.ToArray();

        }
    }

    public class Item
    {
        public int Weight { get; set; }
        public int Cost { get; set; }
        public bool Selected { get; set; }

        public Item(int cost, int weight)
        {
            Cost = cost;
            Weight = weight;
        }


    }

    public class Population
    {
        public bool[] SelectedItems { get; set; }
        public int TotalWeight { get; set; }
        public int Worth { get; set; }
        public double WorthPercentage { get; set; }
        public int Iteration { get; set; }

        public Population(Rucksack rucksack, bool[] selectedItems)
        {
            SelectedItems = selectedItems;
            Worth = rucksack.Items.Where((_, i) => selectedItems[i]).Sum(t => t.Cost);

            for (int i = 0; i < rucksack.Items.Length; i++)
            {
                if (selectedItems[i])
                {
                    TotalWeight += rucksack.Items[i].Weight;
                }
            }
            if (TotalWeight > rucksack.Capacity)
            {
                Worth = 0;
            }
            WorthPercentage = Convert.ToDouble(TotalWeight) / Worth * 100;
        }

        public static void AddDelete(List<Population> populations, Population add)
        {
            populations.Add(add);
            var minWorth = populations.Select(p => p.Worth).Min();
            for (var i = 0; i < populations.Count; i++)
            {
                if (populations[i].Worth == minWorth)
                {
                    populations.RemoveAt(i);
                }
            }
        }


    }


}
