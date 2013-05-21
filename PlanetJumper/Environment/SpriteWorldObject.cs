using System;
using amulware.Graphics;
using OpenTK;

namespace PlanetJumper.Environment
{
    public abstract class SpriteWorldObject<Env> : WorldObject<Env> where Env : GameEnvironment<Env>
    {
        protected Texture texture;

        public SpriteWorldObject(Env env, Texture texture)
            : base(env)
        {
            this.texture = texture;
        }
    }
}
