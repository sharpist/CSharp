using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiverseTree
{
    class Program
    {
        static void Main(string[] args)
        {
            // new multiverse
            Tree tree = new Tree();
            Multiverse multiverse = tree.CreateMultiverse();

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
