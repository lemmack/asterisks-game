using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asterisks.Sprites
{
    public class Projectile : Sprite, ICollidable
    {
        private float _timer;
        public float LifeSpan { get; set; }
        public Vector2 Velocity { get; set; }
        public Projectile(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= LifeSpan)
                IsRemoved = true;

            Position += Velocity;
        }

        public void OnCollide(Sprite sprite)
        {
            if (sprite is Asterisk)
            {
                IsRemoved = true;
            }

        }
    }
}
