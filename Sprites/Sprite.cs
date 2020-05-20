using Asterisks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Asterisks.Managers;

namespace Asterisks.Sprites
{
    public class Sprite : Component, ICloneable
    {
        protected float _layer { get; set; }
        protected Vector2 _origin { get; set; }
        protected Vector2 _position { get; set; }
        protected float _rotation { get; set; }
        protected float _scale { get; set; }
        protected Texture2D _texture;
        public List<Sprite> Children { get; set; }
        public Color Colour { get; set; }
        public bool IsRemoved { get; set; }

        public float Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Rectangle Rectangle
        {
            get
            {
                if (_texture != null)
                {
                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
                }

                throw new Exception("Undefined sprite");
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
            }
        }

        public readonly Color[] TextureData;

        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                  Matrix.CreateRotationZ(_rotation) *
                  Matrix.CreateTranslation(new Vector3(Position, 0));
            }
        }

        public Sprite Parent;

        // The sprite's collision box.
        public Rectangle CollisionArea
        {
            get
            {
                return new Rectangle(Rectangle.X, Rectangle.Y, MathHelper.Max(Rectangle.Width, Rectangle.Height), MathHelper.Max(Rectangle.Width, Rectangle.Height));
            }
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
            Children = new List<Sprite>();
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            Colour = Color.White;
            TextureData = new Color[_texture.Width * _texture.Height];
            _texture.GetData(TextureData);
        }

        public Sprite()
        {
            _texture = null;
            Children = new List<Sprite>();
            Colour = Color.White;
            TextureData = null;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, Position, null, Colour, _rotation, Origin, 1f, SpriteEffects.None, Layer);
        }

        // Math to check if two sprites intersect
        public bool Intersects(Sprite sprite)
        {
            if (this.TextureData == null)
                return false;

            if (sprite.TextureData == null)
                return false;

            var transformAToB = this.Transform * Matrix.Invert(sprite.Transform);

            var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            var yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            for (int yA = 0; yA < this.Rectangle.Height; yA++)
            {
                var posInB = yPosInB;

                for (int xA = 0; xA < this.Rectangle.Width; xA++)
                {
                    var xB = (int)Math.Round(posInB.X);
                    var yB = (int)Math.Round(posInB.Y);

                    if (0 <= xB && xB < sprite.Rectangle.Width &&
                        0 <= yB && yB < sprite.Rectangle.Height)
                    {
                        var colourA = this.TextureData[xA + yA * this.Rectangle.Width];
                        var colourB = sprite.TextureData[xB + yB * sprite.Rectangle.Width];

                        if (colourA.A != 0 && colourB.A != 0)
                        {
                            return true;
                        }
                    }
                    posInB += stepX;
                }
                yPosInB += stepY;
            }
            return false;
        }

        public object Clone()
        {
            var sprite = this.MemberwiseClone() as Sprite;

            return sprite;
        }
    }
}
