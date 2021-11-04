using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic_Algorithm
{
    static class Operators
    {
        public static Random rand = new Random();

        public static Population Selection(this IEnumerable<Population> populations)
        {
            var bestPopulation = populations
                .OrderByDescending(p => p.Worth)
                .ThenBy(p => p.TotalWeight)
                .First();
            return bestPopulation;
        }

        //internal static Population CrossingOnePoint(this Population lhs, Population rhs, Rucksack rucksack)
        //{
        //    var halfElementsAmount = lhs.SelectedItems.Length / 2;
        //    var firstCross = new Population(
        //        rucksack,
        //        lhs.SelectedItems
        //            .Skip(halfElementsAmount)
        //            .Concat(rhs.SelectedItems.Take(halfElementsAmount))
        //            .ToArray());

        //    var secondCross = new Population(
        //        rucksack,
        //        lhs.SelectedItems
        //            .Take(halfElementsAmount)
        //            .Concat(rhs.SelectedItems.Skip(halfElementsAmount))
        //            .ToArray());

        //    return firstCross.Worth > secondCross.Worth ? firstCross : secondCross;
        //}

        internal static Population CrossingOnePoint(this Population lhs, Population rhs, Rucksack rucksack)
        {
            var halfElementsAmount = lhs.SelectedItems.Length / 2;
            var firstCross = new Population(
                rucksack,
                lhs.SelectedItems
                    .Skip(halfElementsAmount)
                    .Concat(rhs.SelectedItems.Take(halfElementsAmount))
                    .ToArray());

            var secondCross = new Population(
                rucksack,
                lhs.SelectedItems
                    .Take(halfElementsAmount)
                    .Concat(rhs.SelectedItems.Skip(halfElementsAmount))
                    .ToArray());

            return firstCross.Worth > secondCross.Worth ? firstCross : secondCross;
        }

        internal static Population MutationChance(this Population population, Rucksack rucksack, int mutationChance)
        {
            if (rand.Next(0, 100) > mutationChance) return population;

            var n = rand.Next(0, population.SelectedItems.Length);
            population.SelectedItems[n] = population.SelectedItems[n] == false;
            population = new Population(rucksack, population.SelectedItems);
            if (population.Worth == 0)
            {
                population.SelectedItems[n] = population.SelectedItems[n] == false;
            }

            return population;
        }

        internal static Population LocalUpgrade(this Population population)
        {
            population.SelectedItems[1] = true;
            return population;
        }
    }
}
