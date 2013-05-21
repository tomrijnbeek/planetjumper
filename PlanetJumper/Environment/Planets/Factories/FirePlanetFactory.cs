using System;
using OpenTK;

namespace PlanetJumper.Environment
{
    class FirePlanetFactory : IPlanetFactory
    {
        public Planet Create(PlanetGameEnvironment environment, Vector2 position, float radius)
        {
            return new FirePlanet(environment, position, radius);
        }
    }
}
