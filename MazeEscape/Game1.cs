﻿using System;
using System.Collections.Generic;
using MazeEscape.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ProjektTestowy.CustomClasses;
using Sounds;

namespace MazeEscape
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        private Camera camera;
        private Floor floor;
        private BasicEffect basicEffect;

        private List<Object3D> obj;

        SpriteFont _spr_font;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = AppConfig.ALLOW_RESIZING;
            graphics.PreferredBackBufferHeight = AppConfig.HEIGHT;
            graphics.PreferredBackBufferWidth = AppConfig.WIDTH;
            graphics.IsFullScreen = AppConfig.FULL_SCREEN;
            IsMouseVisible = AppConfig.IS_MOUSE_VISIBLE;
        }

        protected override void Initialize()
        {
            camera = new Camera(this, new Vector3(10, 1, 5), Vector3.Zero, 5);
            Components.Add(camera);

            floor = new Floor(GraphicsDevice, 50, 50);

            basicEffect = new BasicEffect(GraphicsDevice)
            {
                Alpha = 1,
                VertexColorEnabled = true,
                LightingEnabled = false,
            };

            obj = new List<Object3D>()
            {
                new Object3D(Content, camera, "Models/cross")
                {
                    Position = new Vector3(15, 1, 18),
                    Scale = new Vector3(0.01f),
                },
                new Object3D(Content, camera, "Models/cross")
                {
                    Position = new Vector3(18, 1, 10),
                    Scale = new Vector3(0.01f),
                },
                new Object3D(Content, camera, "Models/cross")
                {
                    Position = new Vector3(6, 1, 15),
                    Scale = new Vector3(0.01f),
                },
                new Object3D(Content, camera, "Models/axies")
                {
                    Position = new Vector3(0, 0, 0),
                    Scale = new Vector3(1.5f),
                    Rotation = new Vector3(MathHelper.ToRadians(-90),MathHelper.ToRadians(180),0)
                },
                new Object3D(Content, camera, "Models/stozek")
                {
                    Position = new Vector3(30, 5, 30),
                    Scale = new Vector3(0.05f)
                },
                new Object3D(Content, camera, "Models/WALL")
                {
                    Position = new Vector3(5, 0, 30),
                    Scale = new Vector3(0.01f)
                },
                new Object3D(Content, camera, "Models/WALL")
                {
                    Position = new Vector3(6, 0, 28),
                    Scale = new Vector3(0.002f),
                    Rotation = new Vector3(0,MathHelper.ToRadians(90),0)
                },
                new Object3D(Content, camera, "Models/WALL")
                {
                    Position = new Vector3(3, 0, 28),
                    Scale = new Vector3(0.002f),
                    Rotation = new Vector3(0,MathHelper.ToRadians(90),0)
                },
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _spr_font = Content.Load<SpriteFont>("Fonts/SpriteFontPL");
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            var gamepadState = GamePad.GetState(PlayerIndex.One);
            var mousePostion = new Point(mouseState.X, mouseState.Y);

            if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                camera.cameraSpeed = 20;
            }
            else if (keyboardState.IsKeyUp(Keys.LeftShift))
            {
                camera.cameraSpeed = 5;
            }

            if (gamepadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (keyboardState.IsKeyDown(Keys.M))
            {
                camera.AllowClimb = !camera.AllowClimb;
            }


            // Update
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var depthStencilState = new DepthStencilState
            {
                DepthBufferEnable = true
            };
            GraphicsDevice.DepthStencilState = depthStencilState;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            floor.Draw(camera, basicEffect);


            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (var object3D in obj)
            {
                object3D.Draw();
            }


            spriteBatch.Begin();
            {
                float xPos = 5f, inc = 25f;
                _total_frames++;

                spriteBatch.DrawString(_spr_font, $"FPS:{_fps}", new Vector2(5f, xPos), Color.Indigo, 0, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0);
                xPos += inc;

                spriteBatch.DrawString(_spr_font, $"Pozycja X: {camera.Position.X:F2}", new Vector2(5f, xPos), Color.DarkRed, 0, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0);
                xPos += inc;

                spriteBatch.DrawString(_spr_font, $"Pozycja Y: {camera.Position.Y:F2}", new Vector2(5f, xPos), Color.DarkGreen, 0, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0);
                xPos += inc;

                spriteBatch.DrawString(_spr_font, $"Pozycja Z: {camera.Position.Z:F2}", new Vector2(5f, xPos), Color.DarkBlue, 0, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0);
                xPos += inc;

                spriteBatch.DrawString(_spr_font, $"NoClip: {camera.AllowClimb}", new Vector2(5f, xPos), Color.Aqua, 0, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0);
                xPos += inc;

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
