using System.Collections.Generic;

namespace MultiverseTree
{
    struct Tree
    {
        public static Multiverse CreateMultiverse()
        {
            Multiverse multiverse = new Multiverse(); // object Multiverse
            var universes = new List<Universe>();     // collection List<Universe>
            multiverse.Universes = universes;         // initialization

            Universe universe = new Universe();
            var galaxies = new List<Galaxy>();
            universe.Galaxies = galaxies;
            universes.Add(universe); // Universe object has been added to List<Universe>

            Galaxy galaxy = new Galaxy();
            var planets = new List<Planet>();
            galaxy.Planets = planets;
            galaxies.Add(galaxy);

            Planet planet = new Planet();
            var continents = new List<Continent>();
            planet.Continents = continents;
            planets.Add(planet);

            Continent continent = new Continent();
            var countries = new List<Country>();
            continent.Countries = countries;
            continents.Add(continent);

            Country country = new Country();
            var citizens = new List<Citizen>()
            {
                new Citizen { Name = "Robin", Family = "Black" },
                new Citizen { Name = "Kate", Family = "Walken" },
                new Citizen { Name = "Alex", Family = "Usov" },
                new Citizen { Name = "Tom", Family = "Jones" },
                new Citizen { Name = "Elena", Family = "Scheider" }
            };
            country.Citizens = citizens;
            countries.Add(country);

            return multiverse;
        }
    }
}
