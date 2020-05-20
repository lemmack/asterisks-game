using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asterisks.Sprites
{
    public class Ship: Sprite, ICollidable
    {
        public int Life { get; set; } = 1;
        public Projectile Projectile { get; set; }
        public float BaseSpeed { get; set; }
        public float BaseTurnSpeed { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; } = 0;
        public float AngleVelocity { get; set; }
        public Vector2 DirectionVector { get; set; }
        public float ShootDelay { get; set; }
        public float ProjectileSpeed { get; set; }
        public Ship(Texture2D texture) : base(texture)
        {
        }

        public void Shoot()
        {
            if (Projectile == null)
                return;

            var projectile = Projectile.Clone() as Projectile;
            projectile.Position = this.Position;
            projectile.Colour = this.Colour;
            projectile.Layer = 0.1f;
            projectile.LifeSpan = 5f;
            projectile.Velocity = DirectionVector * ProjectileSpeed;
            projectile.Parent = this;
            Children.Add(projectile);
        }

        public virtual void OnCollide(Sprite sprite)
        {
            throw new NotImplementedException();
        }
    }
}
