using System;
using OpenTK;

namespace PlanetJumper.Environment
{
    interface IPlanetFactory
    {
        Planet Create(PlanetGameEnvironment environment, Vector2 position, float radius);
    }
}
