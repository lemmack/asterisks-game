using Asterisks.Controls;
using Asterisks.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asterisks.States
{
    class MenuState : State
    {
        private List<Component> _components;

        private SpriteFont _font;

        public MenuState(Game1 game, ContentManager content) : base(game, content)
        {

        }

        public override void LoadContent()
        {
            var buttonTexture = _content.Load<Texture2D>("Button");
            _font = _content.Load<SpriteFont>("Font");

            _components = new List<Component>()
            {
                new Sprite(_content.Load<Texture2D>("Background/MainMenu"))
                {
                    Layer = 0.0f,
                    Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
                },
                new Button(buttonTexture, _font)
                {
                    Text = "Play",
                    Position = new Vector2(Game1.ScreenWidth/2, 310),
                    Click = new EventHandler(Button_Play_Clicked),
                    Layer = 0.1f
                },
                new Button(buttonTexture, _font)
                {
                    Text = "Highscores",
                    Position = new Vector2(Game1.ScreenWidth/2, 350),
                    Click = new EventHandler(Button_Highscores_Clicked),
                    Layer = 0.1f
                },
                new Button(buttonTexture, _font)
                {
                    Text = "Quit",
                    Position = new Vector2(Game1.ScreenWidth/2, 390),
                    Click = new EventHandler(Button_Quit_Clicked),
                    Layer = 0.1f
                }
            };
        }

        private void Button_Play_Clicked(Object sender, EventArgs args)
        {
            _game.ChangeState(new GameState(_game, _content));
        }

        private void Button_Highscores_Clicked(Object sender, EventArgs args)
        {
            _game.ChangeState(new HighscoresState(_game, _content));
        }

        private void Button_Quit_Clicked(Object sender, EventArgs args)
        {
            _game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(_font, "Use WASD to move and Space to shoot.", new Vector2(Game1.ScreenWidth / 2 - 134, 450), Color.Red);

            spriteBatch.End();
        }
    }
}
