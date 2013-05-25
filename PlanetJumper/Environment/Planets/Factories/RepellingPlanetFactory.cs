using System;
using OpenTK;

namespace PlanetJumper.Environment
{
    class RepellingPlanetFactory : IPlanetFactory
    {
        public Planet Create(PlanetGameEnvironment environment, Vector2 position, float radius)
        {
            return new RepellingPlanet(environment, position, radius);
        }
    }
}
