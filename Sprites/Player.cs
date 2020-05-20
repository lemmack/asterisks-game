using Asterisks.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asterisks.Sprites
{
    public class Player : Ship, ICollidable
    {
        private float _shootTimer = 0;
        public bool IsDead
        {
            get { return Life <= 0; }
        }
        public Input Input;

        public Score Score;
        public Player(Texture2D texture) : base(texture)
        {
            BaseSpeed = 0.9f;
            BaseTurnSpeed = 0.8f;
            ShootDelay = 0.8f;
            ProjectileSpeed = 4.6f;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsDead)
                return;
            var rightAngle = Math.PI / 2;
            DirectionVector = new Vector2((float)Math.Cos(Angle-rightAngle), (float)Math.Sin(Angle-rightAngle));

            // Drag Calculations
            Velocity *= 0.8f;
            AngleVelocity *= 0.8f;

            // Player movement
            if (Keyboard.GetState().IsKeyDown(Input.Forward))
                Velocity += DirectionVector * BaseSpeed;
            if (Keyboard.GetState().IsKeyDown(Input.Back))
                Velocity -= DirectionVector * BaseSpeed;
            if (Keyboard.GetState().IsKeyDown(Input.Left))
                AngleVelocity -= MathHelper.ToRadians(1) * BaseTurnSpeed;
            if (Keyboard.GetState().IsKeyDown(Input.Right))
                AngleVelocity += MathHelper.ToRadians(1) * BaseTurnSpeed;

            _shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(Keyboard.GetState().IsKeyDown(Input.Shoot) && _shootTimer > ShootDelay)
            {
                Shoot();
                _shootTimer = 0f;
            }

            // Update position and rotation
            Position += Velocity;
            Angle += AngleVelocity;
            _rotation = Angle;

            // Wrap the player's position around the screen.
            while (Position.X > Game1.ScreenWidth)
                Position -= new Vector2(Game1.ScreenWidth,0f);
            while (Position.X < 0)
                Position += new Vector2(Game1.ScreenWidth, 0f);
            while (Position.Y > Game1.ScreenHeight)
                Position -= new Vector2(0f, Game1.ScreenHeight);
            while (Position.Y < 0)
                Position += new Vector2(0f, Game1.ScreenHeight);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDead)
                return;

            base.Draw(gameTime, spriteBatch);
        }

        public override void OnCollide(Sprite sprite)
        {
            if (IsDead)
                return;

            if (sprite is Asterisk)
                Life--;
        }
    }
}
