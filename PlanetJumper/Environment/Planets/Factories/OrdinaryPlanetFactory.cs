using System;
using OpenTK;

namespace PlanetJumper.Environment
{
    class OrdinaryPlanetFactory : IPlanetFactory
    {
        public Planet Create(PlanetGameEnvironment environment, Vector2 position, float radius)
        {
            return new OrdinaryPlanet(environment, position, radius);
        }
    }
}
