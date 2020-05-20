using Asterisks.Managers;
using Asterisks.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asterisks.Sprites
{
    public class Asterisk : Sprite, ICollidable
    {
        public enum AsteriskSize
        {
            Small,
            Medium,
            Large
        }
        public AsteriskSize Size { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngleVelocity { get; set; }
        public int Health { get; set; } = 1;
        public bool NeedsChildren { get; set; }
        public Asterisk(Texture2D texture) : base(texture)
        {
            NeedsChildren = false;
        }

        public override void Update(GameTime gameTime)
        {
            Angle += AngleVelocity;
            _rotation = Angle;
            Position += Velocity;

            while (Position.X > Game1.ScreenWidth)
                Position -= new Vector2(Game1.ScreenWidth, 0f);
            while (Position.X < 0)
                Position += new Vector2(Game1.ScreenWidth, 0f);
            while (Position.Y > Game1.ScreenHeight)
                Position -= new Vector2(0f, Game1.ScreenHeight);
            while (Position.Y < 0)
                Position += new Vector2(0f, Game1.ScreenHeight);

        }

        public void OnCollide(Sprite sprite)
        {
            if (sprite is Projectile)
            {
                Health--;

                if (Health <= 0)
                {
                    IsRemoved = true;

                    if (Size == AsteriskSize.Large || Size == AsteriskSize.Medium)
                    {
                        NeedsChildren = true;
                    }
                    ((Player)sprite.Parent).Score.Value++;
                }
            }
        }

    }
}
