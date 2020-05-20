using Asterisks.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asterisks.Managers
{
    // Manages the spawning of Asterisks
    public class AsteriskManager
    {
        private float _timer;
        private List<Texture2D> _textures;
        private List<Color> _colours;
        public bool SpawnNew { get; set; }
        public int MaxAsterisks { get; set; }
        public float SpawnTimer { get; set; }
        public AsteriskManager(ContentManager content)
        {
            _textures = new List<Texture2D>()
            {
                content.Load<Texture2D>("Asterisks/SmallAsterisk"),
                content.Load<Texture2D>("Asterisks/MediumAsterisk"),
                content.Load<Texture2D>("Asterisks/BigAsterisk")
            };

            _colours = new List<Color>()
            {
                Color.White, Color.CadetBlue, Color.BlanchedAlmond, Color.DarkOrange, Color.OliveDrab, Color.DarkOrchid
            };

            MaxAsterisks = 18;
            SpawnTimer = 3f;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            SpawnNew = false;

            if (_timer > SpawnTimer)
            {
                SpawnNew = true;
                _timer = 0f;
            }
        }

        public List<Asterisk> GetAsterisks(Asterisk parent = null)
        {
            var asteriskList = new List<Asterisk>();

            // If a parent asteroid was just destroyed, spawning 2 children
            if (parent != null)
            {
                if (parent.Size != Asterisk.AsteriskSize.Small)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int direction = Game1.Random.NextDouble() >= 0.5 ? 1 : -1;
                        asteriskList.Add(new Asterisk(_textures[(int)parent.Size - 1])
                        {
                            Colour = _colours[Game1.Random.Next(_colours.Count)],
                            Size = parent.Size - 1,
                            Health = 1,
                            Layer = 0.2f,
                            Position = parent.Position,
                            Velocity = new Vector2((float)Game1.Random.NextDouble() * Game1.Random.Next(1, 3), (float)Game1.Random.NextDouble() * Game1.Random.Next(1, 3)),
                            AngleVelocity = (float)Game1.Random.NextDouble() * direction / 100
                        });
                    }
                }
            }
            else // Spawn a random new big asterisk
            {
                int direction = Game1.Random.NextDouble() >= 0.5 ? 1 : -1;
                var bigTexture = _textures[2];
                int randX = Game1.Random.Next(2);
                int randY = Game1.Random.Next(2);
                asteriskList.Add(new Asterisk(_textures[2])
                {
                    Colour = _colours[Game1.Random.Next(_colours.Count)],
                    Size = Asterisk.AsteriskSize.Large,
                    Health = 1,
                    Layer = 0.2f,
                    Position = new Vector2(randX == 0 ? _textures[2].Width / 2 : Game1.ScreenWidth - _textures[2].Width / 2, randY == 0 ? _textures[2].Height / 2 : Game1.ScreenHeight - _textures[2].Height / 2),
                    Velocity = new Vector2((float)Game1.Random.NextDouble() * Game1.Random.Next(1,3), (float)Game1.Random.NextDouble() * Game1.Random.Next(1, 3)),
                    AngleVelocity = (float)Game1.Random.NextDouble() * direction / 100
                });
            }
            return asteriskList;
        }
    }
}
