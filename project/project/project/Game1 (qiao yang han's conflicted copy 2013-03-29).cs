/*
 * Patrick Gryczka
 * Qiao Yang Han
 * CS3113
 */


/* 
 * CHANGE LOG
 * >>We should keep track of whatever we do just so we each know what's been changed.
 * 
 * 2/22
 * Basic Menu
 * Ship moving
 * 3/29
 * Player popping up on screen
 * one Console popping up on screen and in position
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace project
{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static GameStates gamestate;
        private Input input;
        private Menu menu;
        private SpriteFont courierNew;
        private SpriteFont courierNewBig;

        Ship ship;
        Player player;

        public enum GameStates
        {
            Menu,
            RunningShip,
            RunningPlayer,
            End
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1900;
            graphics.PreferredBackBufferHeight = 950;

            menu = new Menu();
            gamestate = GameStates.Menu;            

            graphics.ApplyChanges();

            GameSetUp();

            input = new Input();
            base.Initialize();
        }

       
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            courierNew = Content.Load<SpriteFont>(@"Fonts\CourierNew");
            courierNewBig = Content.Load<SpriteFont>(@"Fonts\CourierNewBig");
        }

       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            input.Update();

            if (gamestate == GameStates.RunningShip)
            {
                if (input.Left)
                    ship.MoveLeft();
                else if (input.Right)
                    ship.MoveRight();
                if (input.Up)
                    ship.MoveUp();
                else if (input.Down)
                    ship.MoveDown();
            }
            else if (gamestate == GameStates.Menu)
            {
                if (input.Down)
                {
                    menu.Iterator++;
                }
                else if (input.Up)
                {
                    menu.Iterator--;
                }

                if (input.MenuSelect)
                {
                    if (menu.Iterator == 0)
                    {
                        gamestate = GameStates.RunningShip;
                        GameSetUp();
                    }
                    else if (menu.Iterator == 1)
                    {
                        this.Exit();
                    }
                    menu.Iterator = 0;
                }
            }
            else if (gamestate == GameStates.End)
            {
                if (input.MenuSelect)
                {
                    gamestate = GameStates.Menu;
                }
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (gamestate == GameStates.RunningShip)
            {
                ship.Draw(spriteBatch);
                
                player.Draw(spriteBatch);
            }
            else if (gamestate == GameStates.Menu)
            {
                menu.DrawMenu(spriteBatch, 1900, courierNewBig, courierNew);
            }
            else if (gamestate == GameStates.End)
            {
                menu.DrawEndScreen(spriteBatch, 1900, courierNew);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void GameSetUp()
        {
            ship = new Ship(Content, new Vector2(1900, 950));
            player = new Player(new Vector2(ship.GetPosition().X, ship.GetPosition().Y + ship.GetHeight()), Content);
        }
    }
}
