using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiverseTree
{
    class Program
    {
        static void Main(string[] args)
        {
            // new multiverse
            Multiverse multiverse = Tree.CreateMultiverse();

            // LINQ
            List<Citizen> citizens = multiverse.Universes
                ?.Where(universe => universe.Galaxies != null)
                .SelectMany(universe => universe.Galaxies)

                .Where(galaxy => galaxy.Planets != null)
                .SelectMany(galaxy => galaxy.Planets)

                .Where(planet => planet.Continents != null)
                .SelectMany(planet => planet.Continents)

                .Where(continent => continent.Countries != null)
                .SelectMany(continent => continent.Countries)

                .Where(country => country.Citizens != null)
                .SelectMany(country => country.Citizens)
                .ToList();

            // result
            citizens?.ForEach(citizen => Console.WriteLine($"{citizen.Name} {citizen.Family}"));
            /*
            Robin Black
            Kate Walken
            Alex Usov
            Tom Jones
            Elena Scheider
            */
        }
    }
}
