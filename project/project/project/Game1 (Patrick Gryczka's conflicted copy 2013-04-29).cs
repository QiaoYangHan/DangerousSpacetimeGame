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
        ContentManager content;

        public static GameStates gamestate;
        GameTime gameTime;
        private Input input;
        private Menu menu;
        private SpriteFont courierNew;
        private SpriteFont courierNewBig;
        Rectangle shipRectangle;
        Rectangle eRect;
        int playerCnt;
        int lives;
        Planet planet;


        Ship ship;
        Player player;
        Player player2;
        List<EnemyChaser> ecs = new List<EnemyChaser>();
        List<EnemyEvader> ees = new List<EnemyEvader>();
        List<EnemyTeleport> ets = new List<EnemyTeleport>();

        Texture2D liveIcon;
        Texture2D healthBar;
        private ScrollingBackground myBackground;

        public enum GameStates
        {
            Menu,
            RunningPlayer,
            //RunningPlayer1 ControlConsolePlayer2
            RP1CP2,
            //RunningPlayer1 TurretPlayer2
            RP1TP2,
            //ControlConsolePlayer1 RunningPlayer2
            CP1RP2,
            //ControlConsolePlayer1 TurretPlayer2
            CP1TP2,
            //TurretPlayer1 RunningPlayer2
            TP1RP2,
            //TurretPlayer1 ControlPlayer2
            TP1CP2,
            //LaserPlayer1
            LP1,
            //ShootLaser
            SL,
            End
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = new ContentManager(Services);
        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1900;
            graphics.PreferredBackBufferHeight = 950;
            //this.graphics.IsFullScreen = true;

            menu = new Menu();
            gamestate = GameStates.Menu;

            graphics.ApplyChanges();
            planet = new Planet(content);

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
            liveIcon = Content.Load<Texture2D>(@"Sprites/shipIconSmaller");
            healthBar = Content.Load<Texture2D>(@"Sprites/healthBar");
            myBackground = new ScrollingBackground();
            Texture2D background = Content.Load<Texture2D>(@"Sprites/background");
            myBackground.Load(GraphicsDevice, background);
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


            KeyboardState keyboardState = Keyboard.GetState();
            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your update logic here
            input.Update();
            if (gamestate == GameStates.CP1RP2)
            {
                if (input.LeftMouse)
                    ship.Shoot();
                ship.UpdateBullets();
                if (input.Left)
                {
                    if (playerCnt == 1)
                    {
                        if (ship.GetPosition().X > 1900 / 6)
                            ship.MoveLeft(player, elapsed);
                        else
                        {
                            myBackground.Update(3);
                            foreach (EnemyChaser ec in ecs)
                            {
                                ec.ShiftPos(3);
                            }
                            foreach (EnemyEvader ee in ees)
                            {
                                ee.ShiftPos(3);
                            }
                            foreach (EnemyTeleport et in ets)
                            {
                                et.ShiftPos(3);
                            }
                        }
                    }
                    else if (playerCnt == 2)
                    {
                        if (ship.GetPosition().X > 1900 / 6)
                            ship.MoveLeft(player, player2, elapsed);
                        else
                        {
                            myBackground.Update(3);
                            foreach (EnemyChaser ec in ecs)
                            {
                                ec.ShiftPos(3);
                            }
                            foreach (EnemyEvader ee in ees)
                            {
                                ee.ShiftPos(3);
                            }
                            foreach (EnemyTeleport et in ets)
                            {
                                et.ShiftPos(3);
                            }
                        }
                    }
                }
                if (input.Right)
                {
                    if (playerCnt == 1)
                    {
                        if (ship.GetPosition().X < 5 * 1900 / 6)
                            ship.MoveRight(player, elapsed);
                        else
                        {
                            myBackground.Update(4);
                            foreach (EnemyChaser ec in ecs)
                            {
                                ec.ShiftPos(4);
                            }
                            foreach (EnemyEvader ee in ees)
                            {
                                ee.ShiftPos(4);
                            }
                            foreach (EnemyTeleport et in ets)
                            {
                                et.ShiftPos(4);
                            }
                        }
                    }
                    else if (playerCnt == 2)
                    {
                        if (ship.GetPosition().X < 5 * 1900 / 6)
                            ship.MoveRight(player, player2, elapsed);
                        else
                        {
                            myBackground.Update(4);
                            foreach (EnemyChaser ec in ecs)
                            {
                                ec.ShiftPos(4);
                            }
                            foreach (EnemyEvader ee in ees)
                            {
                                ee.ShiftPos(4);
                            }
                            foreach (EnemyTeleport et in ets)
                            {
                                et.ShiftPos(4);
                            }
                        }
                    }
                }
                if (input.Up)
                {
                    if (playerCnt == 1)
                    {
                        if (ship.GetPosition().Y > 950 / 4)
                            ship.MoveUp(player, elapsed);
                        else
                        {
                            myBackground.Update(1);
                            foreach (EnemyChaser ec in ecs)
                            {
                                ec.ShiftPos(1);
                            }
                            foreach (EnemyEvader ee in ees)
                            {
                                ee.ShiftPos(1);
                            }
                            foreach (EnemyTeleport et in ets)
                            {
                                et.ShiftPos(1);
                            }
                        }
                    }
                    else if (playerCnt == 2)
                    {
                        if (ship.GetPosition().Y > 950 / 4)
                            ship.MoveUp(player, player2, elapsed);
                        else
                        {
                            myBackground.Update(1);
                            foreach (EnemyChaser ec in ecs)
                            {
                                ec.ShiftPos(1);
                            }
                            foreach (EnemyEvader ee in ees)
                            {
                                ee.ShiftPos(1);
                            }
                            foreach (EnemyTeleport et in ets)
                            {
                                et.ShiftPos(1);
                            }
                        }
                    }
                }
                if (input.Down)
                {
                    if (playerCnt == 1)
                    {
                        if (ship.GetPosition().Y < 3 * 950 / 4)
                            ship.MoveDown(player, elapsed);
                        else
                        {
                            myBackground.Update(2);
                            foreach (EnemyChaser ec in ecs)
                            {
                                ec.ShiftPos(2);
                            }
                            foreach (EnemyEvader ee in ees)
                            {
                                ee.ShiftPos(2);
                            }
                            foreach (EnemyTeleport et in ets)
                            {
                                et.ShiftPos(2);
                            }
                        }
                    }
                    else if (playerCnt == 2)
                    {
                        if (ship.GetPosition().Y < 3 * 950 / 4)
                            ship.MoveDown(player, player2, elapsed);
                        else
                        {
                            myBackground.Update(2);
                            foreach (EnemyChaser ec in ecs)
                            {
                                ec.ShiftPos(2);
                            }
                            foreach (EnemyEvader ee in ees)
                            {
                                ee.ShiftPos(2);
                            }
                            foreach (EnemyTeleport et in ets)
                            {
                                et.ShiftPos(2);
                            }
                        }
                    }
                }
                ship.turret.Update(0, 0, elapsed);
                if (input.Comma)
                {
                    if (playerCnt == 1)
                    {
                        gamestate = GameStates.RunningPlayer;
                        player.position.Y -= 5;
                    }
                    else if (playerCnt == 2)
                    {
                        gamestate = GameStates.RunningPlayer;
                        player.position.Y -= 5;
                        player2.position.Y -= 5;
                    }
                }

                foreach (EnemyEvader ee in ees)
                {
                    ee.Update(ship);
                }
                foreach (EnemyTeleport et in ets)
                {
                    et.Update(ship);
                }
                foreach (EnemyChaser ec in ecs)
                {
                    ec.Update(ship);
                }
            }
            else if (gamestate == GameStates.RP1CP2)
            {
                if (input.Left)
                {
                    ship.MoveLeft(player, player2, elapsed);
                }
                if (input.Right)
                {
                    ship.MoveRight(player, player2, elapsed);
                }
                if (input.Up)
                {
                    ship.MoveUp(player, player2, elapsed);
                }
                if (input.Down)
                {
                    ship.MoveDown(player, player2, elapsed);
                }
                ship.turret.Update(0, 0, elapsed);
                if (input.Tab)
                {
                    gamestate = GameStates.RunningPlayer;
                    player.position.Y -= 5;
                    player2.position.Y -= 5;
                }

                foreach (EnemyEvader ee in ees)
                {
                    ee.Update(ship);                   
                }
                foreach (EnemyTeleport et in ets)
                {
                    et.Update(ship);
                }
                foreach (EnemyChaser ec in ecs)
                {
                    ec.Update(ship);
                }
                //for(int i = 0; i < ees.Count; i++)
                //{
                //    for (int j = 0; j < ship.bullets.Count; j++)
                //    {
                //        if (new Rectangle((int)ees[i].Position.X, (int)ees[i].Position.Y, ees[i].Texture.Width, ees[i].Texture.Height).Intersects(new Rectangle((int)ship.bullets[j].Position.X, (int)ship.bullets[i].Position.Y, ship.bullets[i].texture.Width, ship.bullets[i].texture.Height)))
                //        {
                //            ship.bullets.RemoveAt(j);
                //            ees.RemoveAt(i);
                //        }
                //    }
                //}
                //for(int i = 0; i < ecs.Count; i++)
                //{
                //    for (int j = 0; j < ship.bullets.Count; j++)
                //    {
                //        if (ecs[i].rect.Intersects(ship.bullets[j].rect))
                //        {
                //            ship.bullets.RemoveAt(j);
                //            ecs.RemoveAt(i);
                //        }
                //    }
                //}
                //for(int i = 0; i < ets.Count; i++)
                //{
                //    for (int j = 0; j < ship.bullets.Count; j++)
                //    {
                //        if (ets[i].rect.Intersects(ship.bullets[j].rect))
                //        {
                //            ship.bullets.RemoveAt(j);
                //            ecs.RemoveAt(i);
                //        }
                //    }
                //}
            }
            else if (gamestate == GameStates.RunningPlayer)
            {
                if (input.Comma)
                {
                    if (player.playerRectangle.Intersects(ship.controlConsole.consoleRec))
                        gamestate = GameStates.CP1RP2;
                   

                    //else if (player.playerRectangle.Intersects(ship.turret.rec))
                    //    gamestate = GameStates.TP1RP2;
                    //else if (player.playerRectangle.Intersects(ship.laser.rec))
                    //    gamestate = GameStates.LP1;
                }

                else if (input.Q || input.E)
                {
                    if (player.playerRectangle.Intersects(ship.laserConsole.consoleRec))
                    {
                        if (input.E)
                        {
                            ship.laser.clockWise = true;
                        }
                        gamestate = GameStates.SL;
                        ship.laser.shouldUpdate = true;
                        ship.laser.Shoot(Content);
                    }
                }
                if (player.playerRectangle.Intersects(ship.shield.consoleRec))
                {
                    if (input.Eheld)
                    {
                        ship.shield.UpdateShield(1);
                    }
                    else if(input.Qheld)
                    {
                        ship.shield.UpdateShield(2);
                    }
                }
                

                else if (input.Tab)
                {
                    if (player2.playerRectangle.Intersects(ship.controlConsole.consoleRec))
                        gamestate = GameStates.RP1TP2;
                    else if (player2.playerRectangle.Intersects(ship.turret.rec))
                        gamestate = GameStates.RP1TP2;
                }
                
                player.Update(gameTime, keyboardState);
                if (playerCnt == 2)
                    player2.Update(gameTime, keyboardState);
                
                foreach (EnemyEvader ee in ees)
                {
                    ee.Update(ship);
                }
                foreach (EnemyTeleport et in ets)
                {
                    et.Update(ship);
                }
                foreach (EnemyChaser ec in ecs)
                {
                    ec.Update(ship);
                }
                foreach (Bullet bullet in ship.bullets)
                {
                    bullet.Update();
                }
            }
            else if (gamestate == GameStates.SL)
            {
                if (!ship.laser.shouldUpdate)
                {
                    gamestate = GameStates.RunningPlayer;    
                }

                ship.laser.Update(0, 0);

                foreach (EnemyEvader ee in ees)
                {
                    ee.Update(ship);
                }
                foreach (EnemyTeleport et in ets)
                {
                    et.Update(ship);
                }
                foreach (EnemyChaser ec in ecs)
                {
                    ec.Update(ship);
                }
            }
            else if (gamestate == GameStates.Menu)
            {
                if (input.Down)
                {
                    menu.Iterator++;
                }
                if (input.Up)
                {
                    menu.Iterator--;
                }

                if (input.MenuSelect)
                {
                    if (menu.Iterator == 0)
                    {
                        gamestate = GameStates.RunningPlayer;
                        GameSetUp();
                    }
                    else if (menu.Iterator == 1)
                    {
                        gamestate = GameStates.RunningPlayer;
                        GameSetUp(2);
                    }
                    else if (menu.Iterator == 2)
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

            //if (gamestate == GameStates.CP1TP2 || gamestate == GameStates.RP1TP2)
            //{
            //    if(lef
            //}
            // Collision Detection
            shipRectangle = new Rectangle((int)ship.GetPosition().X - 190, (int)ship.GetPosition().Y - 190, ship.shipRing.Width, ship.shipRing.Height);
            if (gamestate == GameStates.RunningPlayer
                || gamestate == GameStates.CP1RP2)
            {
                for (int i = 0; i < ecs.Count; i++)
                {
                    eRect = new Rectangle((int)ecs[i].Position.X, (int)ecs[i].Position.Y, ecs[i].Texture.Width, ecs[i].Texture.Height);
                    if (shipRectangle.Intersects(eRect))
                    {
                        ecs.RemoveAt(i);
                    }
                }
                for (int i = 0; i < ets.Count; i++)
                {
                    eRect = new Rectangle((int)ets[i].Position.X, (int)ets[i].Position.Y, ets[i].Texture.Width, ets[i].Texture.Height);
                    if (shipRectangle.Intersects(eRect))
                    {
                        ets.RemoveAt(i);
                    }
                }
                for (int i = 0; i < ees.Count; i++)
                {
                    eRect = new Rectangle((int)ees[i].Position.X, (int)ees[i].Position.Y, ees[i].Texture.Width, ees[i].Texture.Height);
                    if (shipRectangle.Intersects(eRect))
                    {
                        ees.RemoveAt(i);
                    }
                }
            }

            // TODO: Add your game logic here.
            //myBackground.Update(elapsed * 1000);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            myBackground.Draw(spriteBatch);
            planet.Draw(spriteBatch);
            if (gamestate == GameStates.CP1RP2 || gamestate == GameStates.RunningPlayer
                || gamestate == GameStates.CP1TP2 || gamestate == GameStates.RP1CP2 
                || gamestate == GameStates.RP1TP2 || gamestate == GameStates.TP1CP2
                || gamestate == GameStates.TP1RP2 || gamestate == GameStates.SL)
            {
                ship.Draw(spriteBatch);

               //if (ship.health > 0)
                //{
                    player.Draw(spriteBatch);
                //}

                if (playerCnt == 2)
                    player2.Draw(spriteBatch);

                foreach (EnemyEvader ee in ees)
                {
                    ee.Draw(spriteBatch);
                }

                foreach (EnemyChaser ec in ecs)
                {
                    ec.Draw(spriteBatch);
                }

                foreach (EnemyTeleport et in ets)
                {
                    et.Draw(spriteBatch);
                }
                spriteBatch.DrawString(courierNewBig, "Lives: ", new Vector2(10, 10), Color.Yellow);
                for (int i = 0; i < lives; i++)
                {
                    spriteBatch.Draw(liveIcon, new Rectangle(100 + i * (liveIcon.Width + 10), 10, liveIcon.Width, liveIcon.Height), Color.HotPink);
                }
                spriteBatch.Draw(healthBar, new Rectangle(10, 80, (ship.health * healthBar.Width) / 100, 44), new Rectangle(0, 45, healthBar.Width, 44), Color.Yellow);
                spriteBatch.Draw(healthBar, new Rectangle(10, 80, healthBar.Width, healthBar.Height), Color.HotPink);
            }

            else if (gamestate == GameStates.Menu)
            {
                ship.Draw(spriteBatch);
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
            lives = 3;
            ship = new Ship(Content, graphics);
            playerCnt = 1;
            player = new Player(new Vector2(ship.GetPosition().X, ship.GetPosition().Y - 100), Content, 1);
            ees.Clear();
            ecs.Clear();
            ets.Clear();
            //ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 20, 30));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 800, 30));
            ecs.Add(new EnemyChaser(Content, new Vector2(1900, 950)));
            ecs.Add(new EnemyChaser(Content, new Vector2(1900, 950)));
            ecs.Add(new EnemyChaser(Content, new Vector2(1900, 950)));
            ecs.Add(new EnemyChaser(Content, new Vector2(1900, 950)));
            ets.Add(new EnemyTeleport(Content, new Vector2(1900, 950)));
        }

        private void GameSetUp(int playerAmnt)
        {
            ship = new Ship(Content, graphics);
            player = new Player(new Vector2(ship.GetPosition().X, ship.GetPosition().Y - 100), Content, 1);
            playerCnt = playerAmnt;
            if(playerAmnt > 1)
                player2 = new Player(new Vector2(ship.GetPosition().X-70, ship.GetPosition().Y - 110), Content, 2);
            ees.Clear();
            ecs.Clear();
            ets.Clear();
            //ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 20, 30));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 800, 30));
            ecs.Add(new EnemyChaser(Content, new Vector2(1900, 950)));
            ets.Add(new EnemyTeleport(Content, new Vector2(1900, 950)));
        }
    }
}
