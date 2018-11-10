using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace game.Particles
{
    public class ParticleSystem
    {
        private ParticleSystem() { }
        private static ParticleSystem instance;
        public static ParticleSystem Instance
        {
            get
            {
                if (instance == null)
                    instance = new ParticleSystem();
                return instance;
            }
        }

        private List<ParticleEmitter> emitters = new List<ParticleEmitter>();
        private Dictionary<ParticleShape, Texture2D> textures;

        public void Initialize(ContentManager contentManager)
        {
            textures = new Dictionary<ParticleShape, Texture2D>()
            {
                { ParticleShape.Square, contentManager.Load<Texture2D>("blank") },
                { ParticleShape.Circle, contentManager.Load<Texture2D>("circle") }
            };
        }

        public void Update(GameTime gameTime)
        {
            foreach (ParticleEmitter emitter in emitters.ToArray())
                emitter.Update(gameTime);

            for (int i = 0; i < emitters.Count; i++)
            {
                if (emitters[i].EmitterLifeTime <= 0)
                {
                    emitters.RemoveAt(i);
                    i--;
                }
            }
        }

        public Texture2D GetTexture(ParticleShape particleShape)
        {
            if (!textures.ContainsKey(particleShape))
                throw new ArgumentException("Shape doesnt exist in our dictionary");

            return textures[particleShape];
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (ParticleEmitter emitter in emitters)
                emitter.Draw(spriteBatch, gameTime);
        }

        public void AddEmitter(ParticleEmitter emitter)
        {
            emitters.Add(emitter);
        }
    }
}
