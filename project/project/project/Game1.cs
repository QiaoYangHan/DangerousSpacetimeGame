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
        private SpriteFont courierNewBiger;
        Texture2D flag;
        Rectangle shipRectangle;
        Rectangle eRect;
        int playerCnt;
        int lives;
        Planet bluePlanet;
        Planet gasPlanet;
        Planet aquaGasPlanet;
        Planet marblePlanet;
        Vector2 shift;

        public int score;
        int fakeTime = 0;

        Ship ship;
        Player player;
        Player player2;
        List<EnemyChaser> ecs = new List<EnemyChaser>();
        List<EnemyEvader> ees = new List<EnemyEvader>();
        List<EnemyTeleport> ets = new List<EnemyTeleport>();
        List<Planet> ps = new List<Planet>();
        List<Flag> flags = new List<Flag>();
        Texture2D liveIcon;
        Texture2D healthBar, blood;
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

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 500;
            //this.graphics.IsFullScreen = true;

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
            courierNewBiger = Content.Load<SpriteFont>(@"Fonts\CourierNewBiger");
            liveIcon = Content.Load<Texture2D>(@"Sprites/shipIconSmaller");
            healthBar = Content.Load<Texture2D>(@"Sprites/healthBar");
            myBackground = new ScrollingBackground();
            blood = Content.Load<Texture2D>(@"Sprites/healthBar1");
            flag = Content.Load<Texture2D>(@"Sprites/flag");
            Texture2D background11 = Content.Load<Texture2D>(@"Sprites/bg11");
            Texture2D background12 = Content.Load<Texture2D>(@"Sprites/bg12");
            Texture2D background13 = Content.Load<Texture2D>(@"Sprites/bg13");
            Texture2D background21 = Content.Load<Texture2D>(@"Sprites/bg21");
            Texture2D background22 = Content.Load<Texture2D>(@"Sprites/bg22");
            Texture2D background23 = Content.Load<Texture2D>(@"Sprites/bg23");
            Texture2D background31 = Content.Load<Texture2D>(@"Sprites/bg31");
            Texture2D background32 = Content.Load<Texture2D>(@"Sprites/bg32");
            Texture2D background33 = Content.Load<Texture2D>(@"Sprites/bg33");
            myBackground.Load(GraphicsDevice, background11, background12, background13, background21, background22, background23, background31, background32, background33);
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
                            if (shift.X > -1900)
                            {
                                shift.X -= 6;
                                myBackground.Update(3);
                                bluePlanet.ShiftPos(3);
                                gasPlanet.ShiftPos(3);
                                aquaGasPlanet.ShiftPos(3);
                                marblePlanet.ShiftPos(3);
                                foreach (Flag flag in flags)
                                {
                                    flag.ShiftPos(3);
                                }
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
                    else if (playerCnt == 2)
                    {
                        if (ship.GetPosition().X > 1900 / 6)
                            ship.MoveLeft(player, player2, elapsed);
                        else
                        {
                            myBackground.Update(3);
                            bluePlanet.ShiftPos(3);
                            gasPlanet.ShiftPos(3);
                            aquaGasPlanet.ShiftPos(3);
                            marblePlanet.ShiftPos(3);
                            foreach (Flag flag in flags)
                            {
                                flag.ShiftPos(3);
                            }
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
                            if (shift.X < 1900)
                            {
                                shift.X += 6;
                                myBackground.Update(4);
                                bluePlanet.ShiftPos(4);
                                gasPlanet.ShiftPos(4);
                                aquaGasPlanet.ShiftPos(4);
                                marblePlanet.ShiftPos(4);
                                foreach (Flag flag in flags)
                                {
                                    flag.ShiftPos(4);
                                }
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
                    else if (playerCnt == 2)
                    {
                        if (ship.GetPosition().X < 5 * 1900 / 6)
                            ship.MoveRight(player, player2, elapsed);
                        else
                        {
                            myBackground.Update(4);
                            bluePlanet.ShiftPos(4);
                            gasPlanet.ShiftPos(4);
                            aquaGasPlanet.ShiftPos(4);
                            marblePlanet.ShiftPos(4);
                            foreach (Flag flag in flags)
                            {
                                flag.ShiftPos(4);
                            }
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
                            if (shift.Y > -950)
                            {
                                shift.Y -= 6;
                                myBackground.Update(1);
                                bluePlanet.ShiftPos(1);
                                gasPlanet.ShiftPos(1);
                                aquaGasPlanet.ShiftPos(1);
                                marblePlanet.ShiftPos(1);
                                foreach (Flag flag in flags)
                                {
                                    flag.ShiftPos(1);
                                }
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
                    else if (playerCnt == 2)
                    {
                        if (ship.GetPosition().Y > 950 / 4)
                            ship.MoveUp(player, player2, elapsed);
                        else
                        {
                            myBackground.Update(1);
                            bluePlanet.ShiftPos(1);
                            gasPlanet.ShiftPos(1);
                            aquaGasPlanet.ShiftPos(1);
                            marblePlanet.ShiftPos(1);
                            foreach (Flag flag in flags)
                            {
                                flag.ShiftPos(1);
                            }
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
                            if (shift.Y < 950)
                            {
                                shift.Y += 6;
                                myBackground.Update(2);
                                bluePlanet.ShiftPos(2);
                                gasPlanet.ShiftPos(2);
                                aquaGasPlanet.ShiftPos(2);
                                marblePlanet.ShiftPos(2);
                                foreach (Flag flag in flags)
                                {
                                    flag.ShiftPos(2);
                                }
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
                    else if (playerCnt == 2)
                    {
                        if (ship.GetPosition().Y < 3 * 950 / 4)
                            ship.MoveDown(player, player2, elapsed);
                        else
                        {
                            myBackground.Update(2);
                            bluePlanet.ShiftPos(2);
                            gasPlanet.ShiftPos(2);
                            aquaGasPlanet.ShiftPos(2);
                            marblePlanet.ShiftPos(2);
                            foreach (Flag flag in flags)
                            {
                                flag.ShiftPos(2);
                            }
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
                if (input.C)
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
                
            }
            else if (gamestate == GameStates.RunningPlayer)
            {
                if (input.Comma)
                {
                    if (player.playerRectangle.Intersects(ship.controlConsole.consoleRec))
                        gamestate = GameStates.CP1RP2;
                    if (player.isSpace())
                    {
                        player.setShip();
                        player.position = ship.position + new Vector2(70, 0);
                        player.playerRectangle = new Rectangle((int)player.position.X, (int)player.position.Y, player.texture.Width, player.texture.Height);
                    }
                    if (player.playerRectangle.Intersects(ship.teleportConsole.consoleRec))
                    {
                        foreach (Planet p in ps)
                        {
                            if (Vector2.Distance(ship.GetPosition(), p.position) < ship.range)
                            {
                                player.position = p.position + new Vector2(p.Texture.Width * 0.5f - 20, -20);
                                player.setSpace();
                                p.Captured = true;
                            }
                        }
                    }
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
                    else if (player.isSpace())
                    {
                        flags.Add(new Flag(flag,player.position));
                    }
                }
                if (player.playerRectangle.Intersects(ship.shield.consoleRec))
                {
                        ship.shield.UpdateShield();
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
                        //GameSetUp(2);
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
                //aquaGasPlanet.Update(ship, score);
                //gasPlanet.Update(ship, score);
                //bluePlanet.Update(ship, score);
                if(Vector2.Distance(ship.GetPosition(), bluePlanet.position) < 1200 && !(bluePlanet.Captured))
                {
                    fakeTime++;
                    if (fakeTime > 300)
                    {
                        ecs.Add(new EnemyChaser(Content, new Vector2(bluePlanet.position.X + bluePlanet.Texture.Width/2, bluePlanet.position.Y + bluePlanet.Texture.Height/2)));
                        fakeTime = 0;
                    }
                }
                if (Vector2.Distance(ship.GetPosition(), gasPlanet.position) < 1200 && !(gasPlanet.Captured))
                {
                    fakeTime++;
                    if (fakeTime > 300)
                    {
                        ecs.Add(new EnemyChaser(Content, new Vector2(gasPlanet.position.X + gasPlanet.Texture.Width/2, gasPlanet.position.Y + gasPlanet.Texture.Height/2)));
                        fakeTime = 0;
                    }
                }
                if (Vector2.Distance(ship.GetPosition(), marblePlanet.position) < 1200 && !(marblePlanet.Captured))
                {
                    fakeTime++;
                    if (fakeTime > 300)
                    {
                        ecs.Add(new EnemyChaser(Content, new Vector2(marblePlanet.position.X + marblePlanet.Texture.Width / 2, marblePlanet.position.Y + marblePlanet.Texture.Height / 2)));
                        fakeTime = 0;
                    }
                }
                if (Vector2.Distance(ship.GetPosition(), aquaGasPlanet.position) < 1200 && !(aquaGasPlanet.Captured))
                {
                    fakeTime++;
                    if (fakeTime > 300)
                    {
                        ecs.Add(new EnemyChaser(Content, new Vector2(aquaGasPlanet.position.X + aquaGasPlanet.Texture.Width/2, aquaGasPlanet.position.Y + aquaGasPlanet.Texture.Height/2)));
                        fakeTime = 0;
                    }
                }
                for (int i = 0; i < ees.Count; i++)
                {
                    for (int j = 0; j < ship.bullets.Count; j++)
                    {
                        if (i >= 0)
                        {
                            eRect = new Rectangle((int)ees[i].Position.X, (int)ees[i].Position.Y, ees[i].Texture.Width, ees[i].Texture.Height);
                            if (ship.bullets[j].rect.Intersects(eRect))
                            {
                                ship.bullets.RemoveAt(j);
                                j--;
                                ees.RemoveAt(i);
                                i--;
                                score++;
                            }
                        }
                    }
                }
                for (int i = 0; i < ecs.Count; i++)
                {
                    if (i >= 0)
                    {
                        eRect = new Rectangle((int)ecs[i].Position.X, (int)ecs[i].Position.Y, ecs[i].Texture.Width, ecs[i].Texture.Height);
                        for (int j = 0; j < ship.bullets.Count; j++)
                        {

                            if (ship.bullets[j].rect.Intersects(eRect))
                            {
                                ship.bullets.RemoveAt(j);
                                j--;
                                ecs.RemoveAt(i);
                                i--;
                                score++;
                            }
                        }
                    }
                }
                for (int i = 0; i < ets.Count; i++)
                {
                    if (i >= 0)
                    {
                        eRect = new Rectangle((int)ets[i].Position.X, (int)ets[i].Position.Y, ets[i].Texture.Width, ets[i].Texture.Height);
                        for (int j = 0; j < ship.bullets.Count; j++)
                        {
                            if (ship.bullets[j].rect.Intersects(eRect))
                            {
                                ship.bullets.RemoveAt(j);
                                j--;
                                ets.RemoveAt(i);
                                i--;
                                score++;
                            }
                        }
                    }
                }
                for (int i = 0; i < ecs.Count; i++)
                {
                    if (i >= 0)
                    {
                        eRect = new Rectangle((int)ecs[i].Position.X, (int)ecs[i].Position.Y, ecs[i].Texture.Width, ecs[i].Texture.Height);
                        Rectangle lRect = CalculateBoundingRectangle(ship.laser.laserRect, ship.laser.laserMatrix);
                        if (shipRectangle.Intersects(eRect))
                        {
                            ecs.RemoveAt(i);
                            i--;
                            ship.Hurt();
                            score++;
                        }
                            
                        else if (ship.laser.shouldUpdate && lRect.Intersects(eRect))
                        {
                            ecs.RemoveAt(i);
                            i--;
                            score++;
                        }
                    }
                }
                for (int i = 0; i < ets.Count; i++)
                {
                    if (i >= 0)
                    {
                        Rectangle lRect = CalculateBoundingRectangle(ship.laser.laserRect, ship.laser.laserMatrix);
                        eRect = new Rectangle((int)ets[i].Position.X, (int)ets[i].Position.Y, ets[i].Texture.Width, ets[i].Texture.Height);
                        if (shipRectangle.Intersects(eRect))
                        {
                            ets.RemoveAt(i);
                            i--;
                            ship.Hurt();
                            score++;
                        }
                        else if (lRect.Intersects(eRect))
                        {
                            ets.RemoveAt(i);
                            i--;
                            score++;
                        }
                    }
                }
                for (int i = 0; i < ees.Count; i++)
                {
                    if (i >= 0)
                    {
                        eRect = new Rectangle((int)ees[i].Position.X, (int)ees[i].Position.Y, ees[i].Texture.Width, ees[i].Texture.Height);
                        Rectangle lRect = CalculateBoundingRectangle(ship.laser.laserRect, ship.laser.laserMatrix);
                        if (shipRectangle.Intersects(eRect))
                        {
                            ees.RemoveAt(i);
                            i--;
                            ship.Hurt();
                            score++;
                        }
                        else if (ship.laser.shouldUpdate && lRect.Intersects(eRect))
                        {
                            ees.RemoveAt(i);
                            i--;
                            score++;
                        }
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
            foreach (EnemyChaser ec in ecs)
            {
                ec.Draw(spriteBatch);
            }
            bluePlanet.Draw(spriteBatch);
            gasPlanet.Draw(spriteBatch);
            aquaGasPlanet.Draw(spriteBatch);
            marblePlanet.Draw(spriteBatch);
            if (gamestate == GameStates.CP1RP2 || gamestate == GameStates.RunningPlayer
                || gamestate == GameStates.CP1TP2 || gamestate == GameStates.RP1CP2 
                || gamestate == GameStates.RP1TP2 || gamestate == GameStates.TP1CP2
                || gamestate == GameStates.TP1RP2 || gamestate == GameStates.SL)
            {
                

                if (ship.health > 0)
                {
                    ship.Draw(spriteBatch);
                    player.Draw(gameTime, spriteBatch);
                    spriteBatch.Draw(blood, new Rectangle((int)ship.GetPosition().X - 55, (int)ship.GetPosition().Y - 153, (ship.health * healthBar.Width) / 100, healthBar.Height), new Rectangle(0, 45, healthBar.Width, 44), Color.Navy);
                    spriteBatch.Draw(healthBar, new Rectangle((int)ship.GetPosition().X - 55, (int)ship.GetPosition().Y - 153, healthBar.Width, healthBar.Height), Color.White);
                    
                }
                bool win = false;
                foreach (Planet planet in ps)
                {
                    if (!(planet.Captured))
                        win = false;
                }
                if (win)
                {
                    spriteBatch.DrawString(courierNewBiger, "WINNER", new Vector2(950, 475), Color.Yellow);
                }
                if ( ship.health <=0 & !win)
                {
                    spriteBatch.DrawString(courierNewBiger, "GAME OVER", new Vector2(950, 475), Color.Red);
                }
                foreach (Flag flag in flags)
                {
                    spriteBatch.Draw(flag.img, flag.position, Color.White);
                }
                if (playerCnt == 2)
                    player2.Draw(gameTime, spriteBatch);

                foreach (EnemyEvader ee in ees)
                {
                    ee.Draw(spriteBatch);
                }

                

                foreach (EnemyTeleport et in ets)
                {
                    et.Draw(spriteBatch);
                }
                
                spriteBatch.DrawString(courierNewBiger, "Score: ", new Vector2(1500, 10), Color.Yellow);
                spriteBatch.DrawString(courierNewBiger, (score*1000).ToString(), new Vector2(1650, 10), Color.Tomato);
                //for (int i = 0; i < lives; i++)
                //{
                //    spriteBatch.Draw(liveIcon, new Rectangle(100 + i * (liveIcon.Width + 10), 10, liveIcon.Width, liveIcon.Height), Color.HotPink);
                //}
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
        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            //  Getting the corners
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop), Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop), Vector2.Max(leftBottom, rightBottom));

            //  And we have a rectangle
            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        private void GameSetUp()
        {
            lives = 3;
            ship = new Ship(Content, graphics);
            playerCnt = 1;
            shift = new Vector2(0, 0);
            player = new Player(new Vector2(ship.GetPosition().X, ship.GetPosition().Y - 100), Content, 1);
            bluePlanet = new Planet(Content);
            gasPlanet = new Planet(Content, 1);
            aquaGasPlanet = new Planet(Content, 2);
            marblePlanet = new Planet(Content, 3);
            ps.Add(bluePlanet);
            ps.Add(gasPlanet);
            ps.Add(aquaGasPlanet);
            ps.Add(marblePlanet);
            ees.Clear();
            ecs.Clear();
            ets.Clear();
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), -1800, -400, marblePlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), -1700, 100, marblePlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), -1800, -50, marblePlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), -1650, 280, marblePlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), -1450, 500, marblePlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), -1820, 450, marblePlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 3600, 1800, aquaGasPlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 3000, 1800, aquaGasPlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 3600, 1500, aquaGasPlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 2500, -400, gasPlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 2300, -500, gasPlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 2000, -600, gasPlanet));
            ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 2500, -700, gasPlanet));
            ets.Add(new EnemyTeleport(Content, new Vector2(1900, 950)));
            ets.Add(new EnemyTeleport(Content, new Vector2(1900, 950)));
            //ets.Add(new EnemyTeleport(Content, new Vector2(1900, 950)));
        }

        //private void GameSetUp(int playerAmnt)
        //{
        //    ship = new Ship(Content, graphics);
        //    player = new Player(new Vector2(ship.GetPosition().X, ship.GetPosition().Y - 100), Content, 1);
        //    playerCnt = playerAmnt;
        //    if(playerAmnt > 1)
        //        player2 = new Player(new Vector2(ship.GetPosition().X-70, ship.GetPosition().Y - 110), Content, 2);
        //    ees.Clear();
        //    ecs.Clear();
        //    ets.Clear();
        //    //ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 20, 30));
        //    ees.Add(new EnemyEvader(Content, new Vector2(1900, 950), 800, 30));
        //    ecs.Add(new EnemyChaser(Content, new Vector2(1900, 950)));
        //    ets.Add(new EnemyTeleport(Content, new Vector2(1900, 950)));
        //}
    }
}
