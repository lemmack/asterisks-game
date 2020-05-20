﻿using Asterisks.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asterisks
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Random Random;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        private State _currentState;
        private State _nextState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Random = new Random();

            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, Content);
            _currentState.LoadContent();
            _nextState = null;
        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                // If there is a next state, change it to current and load it.
                _currentState = _nextState;
                _currentState.LoadContent();
                _nextState = null;
            }

            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _currentState.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}
