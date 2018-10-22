﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeEscape;
using Menu_buttons;

namespace MainMenu
{
    public class MainMenu : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public bool runGame { get; set; }
        Texture2D menuBackground, bG_A, bG_B, bA_A, bA_B, bO_A, bO_B, bW_A, bW_B;
        bool mouseLock = false;

        List<Menu_Button> knefel;


        public MainMenu()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.IsFullScreen = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            runGame = false;
            knefel = new List<Menu_Button>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menuBackground = Content.Load<Texture2D>("Main_Menu/Menu_background");

            // Buttons Load

            bG_A = Content.Load<Texture2D>("Main_Menu/Graj_buttnon_A");
            bG_B = Content.Load<Texture2D>("Main_Menu/Graj_buttnon_B");

            bA_A = Content.Load<Texture2D>("Main_Menu/Autorzy_buttnon_A");
            bA_B = Content.Load<Texture2D>("Main_Menu/Autorzy_buttnon_B");

            bO_A = Content.Load<Texture2D>("Main_Menu/Opcje_buttnon_A");
            bO_B = Content.Load<Texture2D>("Main_Menu/Opcje_buttnon_B");

            bW_A = Content.Load<Texture2D>("Main_Menu/Wyjscie_buttnon_A");
            bW_B = Content.Load<Texture2D>("Main_Menu/Wyjscie_buttnon_B");

            int xOffset = 130;
            int yOffset = 400;
            int yPadding = 65;

            knefel.Add(new Menu_Button(bG_A, bG_B, new Rectangle(xOffset, yOffset, bG_A.Width, bG_B.Height))); // i = 0  PLAY button
            knefel.Add(new Menu_Button(bA_A, bA_B, new Rectangle(xOffset, yOffset + yPadding, bG_A.Width, bG_B.Height)));
            knefel.Add(new Menu_Button(bO_A, bO_B, new Rectangle(xOffset, yOffset + yPadding * 2, bG_A.Width, bG_B.Height)));
            knefel.Add(new Menu_Button(bW_A, bW_B, new Rectangle(xOffset, yOffset + yPadding * 3, bG_A.Width, bG_B.Height))); // i = 3  EXIT Button

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePos = new Point(mouseState.X, mouseState.Y);
            var keyboardState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                runGame = true;
                Exit();
            }

            for (int i = 0; i < knefel.Count; i++)
            {
                if (knefel[0].IsOn(mousePos) && mouseState.LeftButton == ButtonState.Pressed && !mouseLock)
                {
                    runGame = true;
                    Exit();
                }

                if (knefel[3].IsOn(mousePos) && mouseState.LeftButton == ButtonState.Pressed && !mouseLock)
                {
                    Exit();
                }

                if (knefel[i].IsOn(mousePos) == true)
                {
                    knefel[i].OnClick(true);
                }
                else
                {
                    knefel[i].OnClick(false);
                }



            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(menuBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            for (int i = 0; i < 4; i++)
            {
                knefel[i].Draw(spriteBatch);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
