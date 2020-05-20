using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asterisks.Managers;
using Asterisks.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asterisks.States
{
    public class GameState : State
    {
        public static AsteriskManager AsteriskManager { get; private set; }

        private SpriteFont _font;

        private Player _player;

        private ScoreManager _scoreManager;

        private List<Sprite> _sprites;

        public GameState(Game1 game, ContentManager content)
          : base(game, content)
        {
        }

        public override void LoadContent()
        {
            var playerTexture = _content.Load<Texture2D>("Ships/Player");
            var projectileTexture = _content.Load<Texture2D>("Projectile");

            _font = _content.Load<SpriteFont>("Font");

            _scoreManager = ScoreManager.Load();

            _sprites = new List<Sprite>()
      {
        new Sprite(_content.Load<Texture2D>("Background/Game"))
        {
          Layer = 0.0f,
          Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
        }
      };

            var playerProjectile = new Projectile(projectileTexture);

            _sprites.Add(new Player(playerTexture)
            {
                Colour = Color.White,
                Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
                Layer = 0.3f,
                Projectile = playerProjectile,
                Input = new Models.Input()
                {
                    Forward = Keys.W,
                    Back = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                    Shoot = Keys.Space,
                },
                Life = 1,
                Score = new Models.Score()
                {
                    PlayerName = "Player 1",
                    Value = 0,
                },
            });

            _player = (Player)_sprites.FirstOrDefault(c => c is Player);

            AsteriskManager = new AsteriskManager(_content);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            foreach (var sprite in _sprites)
            {
                sprite.Update(gameTime);
            }

            AsteriskManager.Update(gameTime);
            if (AsteriskManager.SpawnNew && _sprites.Where(c => c is Asterisk).Count() < AsteriskManager.MaxAsterisks)
            {
                var newBigAsterisks = AsteriskManager.GetAsterisks();
                foreach(var asterisk in newBigAsterisks)
                    _sprites.Add(asterisk);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            var collidableSprites = _sprites.Where(c => c is ICollidable);

            foreach (var spriteA in collidableSprites)
            {
                foreach (var spriteB in collidableSprites)
                {
                    if (spriteA == spriteB)
                        continue;

                    if (!spriteA.CollisionArea.Intersects(spriteB.CollisionArea))
                        continue;

                    if (spriteA.Intersects(spriteB))
                        ((ICollidable)spriteA).OnCollide(spriteB);
                }
            }

            // Add the children sprites to the list of sprites (ie projectiles) and spawn smaller Asterisks
            
            var spriteCount = _sprites.Count;
            for (int i=0; i < spriteCount; i++)
            {
                foreach (var child in _sprites[i].Children)
                    _sprites.Add(child);

                _sprites[i].Children = new List<Sprite>();

                if (_sprites[i] is Asterisk && ((Asterisk)_sprites[i]).NeedsChildren == true)
                {
                    var newChildrenAsterisks = AsteriskManager.GetAsterisks((Asterisk)_sprites[i]);
                    foreach (var asterisk in newChildrenAsterisks)
                        _sprites.Add(asterisk);
                }
            }

            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }

            if (_player.IsDead)
            {
                _scoreManager.Add(_player.Score);
                ScoreManager.Save(_scoreManager);
                _game.ChangeState(new HighscoresState(_game, _content));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var sprite in _sprites)
                sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin();

            spriteBatch.DrawString(_font, "Score: " + _player.Score.Value, new Vector2(10f, 10f), Color.Red);

            spriteBatch.End();
        }
    }
}
