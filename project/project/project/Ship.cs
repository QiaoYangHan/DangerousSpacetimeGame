using System;
using System.IO;
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
    class Ship
    {
        GameTime gameTime;
        //ship design
        public Texture2D shipRing;
        private Texture2D shipTile;
        private Player player;
        private ShipKernel shipKernel;

        private Texture2D healthBar;

        public float range = 700f;
        //basic attribute
        public Vector2 position;
        private float speed;
        private double direction;

        //collision detection
        private Rectangle shipRec;
        private Rectangle healthRec;
        
        //score
        private int points;
        
        //screen size
        //private float width, height;

        //console thing
        public Console controlConsole;
        public Console laserConsole;
        public Console teleportConsole;
        public Turret turret;
        public Laser laser;
        public Vector2 screenSize;
        public int consoleCtrl = 0;
        public Shield shield;

        bool alive;
        public int health;
        public List<Bullet> bullets = new List<Bullet>();
        ContentManager cm;

        public Ship(ContentManager content, GraphicsDeviceManager graphics)
        {
            cm = content;
            //sprite
            shipRing = content.Load<Texture2D>(@"Sprites/shipRing");
            shipTile = content.Load<Texture2D>(@"Sprites/shipTile");
            Texture2D consoleTile = content.Load<Texture2D>(@"Sprites/console");
            Texture2D laserTile = content.Load<Texture2D>(@"Sprites/laserConsole");
            Texture2D teleportTile = content.Load<Texture2D>(@"Sprites/teleportConsole");
            healthBar = content.Load<Texture2D>(@"Sprites/healthBar");

            position.X = graphics.PreferredBackBufferWidth / 2f;
            position.Y = graphics.PreferredBackBufferHeight / 2f;

            //ship kernel
            using (Stream fileStream = TitleContainer.OpenStream(@"Content\File\kernel.txt"))
                shipKernel = new ShipKernel(fileStream, shipTile, consoleTile, position);

            //attribute part1

            //attribute part2
            speed = 6f;
            direction = 0;
            points = 0;

            //rectangle bound
            shipRec = new Rectangle(0, 0, shipRing.Width, shipRing.Height);

            healthRec = new Rectangle(0, 0, healthBar.Width, healthBar.Height);

            //screen size
            screenSize.X = graphics.PreferredBackBufferWidth;
            screenSize.Y = graphics.PreferredBackBufferHeight;            
            
            controlConsole = new Console(content, this, -114, 66, consoleTile);
            laserConsole = new Console(content, this, -114, -114, laserTile);
            teleportConsole = new Console(content, this, 66, -114, teleportTile);
            shield = new Shield(content, this, 66, 66);

            turret = new Turret(content, this, 0, -220);
            laser = new Laser(content, this, 0, -135);

            alive = true;
            health = 100;
        }

        public void IncreaseSpeed()
        {
            speed += 0.6f;
        }

        public Rectangle GetRectangle()
        {
            return shipRec;
        }

        //point method
        public void IncrementPoints()
        {
            points++;
        }

        public int GetPoints()
        {
            return points;
        }

        public Vector2 GetPosition()
        {
            return position;
        }
        
        // width and height
        public float GetHeight()
        {
            return shipRing.Height;
        }

        public float GetWidth()
        {
            return shipRing.Width;
        }

        public void MoveUp(Player player, float elapsed)
        {
            if (position.Y > -950 + shipRing.Height/2)
            {
                position.Y -= speed;
                controlConsole.Update(0, -speed);
                laserConsole.Update(0, -speed);
                teleportConsole.Update(0, -speed);
                turret.Update(0, -speed, elapsed);
                laser.Update(0, -speed);
                player.position.Y -= speed;
                shipKernel.UpdatePosition(position);
                shield.Update(0, -speed);
            }

        }

        public void MoveUp(Player player, Player player2, float elapsed)
        {
            if (position.Y > -950 + shipRing.Height / 2)
            {
                position.Y -= speed;
                controlConsole.Update(0, -speed);
                laserConsole.Update(0, -speed);
                teleportConsole.Update(0, -speed);
                turret.Update(0, -speed, elapsed);
                laser.Update(0, -speed);
                player.position.Y -= speed;
                player2.position.Y -= speed;
                shipKernel.UpdatePosition(position);
                shield.Update(0, -speed);
            }
        }

        public void MoveDown(Player player, float elapsed)
        {
            if (position.Y < screenSize.Y - shipRing.Height/2)
            {
                position.Y += speed;
                controlConsole.Update(0, speed);
                laserConsole.Update(0, speed);
                teleportConsole.Update(0, speed);
                turret.Update(0, speed, elapsed);
                laser.Update(0, speed);
                player.position.Y += speed;
                shipKernel.UpdatePosition(position);
                shield.Update(0, speed);
            }
        }

        public void MoveDown(Player player, Player player2, float elapsed)
        {
            if (position.Y < screenSize.Y - shipRing.Height / 2)
            {
                position.Y += speed;
                controlConsole.Update(0, speed);
                laserConsole.Update(0, speed);
                teleportConsole.Update(0, speed);
                turret.Update(0, speed, elapsed);
                laser.Update(0, speed);
                player.position.Y += speed;
                player2.position.Y += speed;
                shipKernel.UpdatePosition(position);
                shield.Update(0, speed);
            }
        }

        public void MoveLeft(Player player, float elapsed)
        {
            if (position.X > 0 + shipRing.Width/2)
            {
                position.X -= speed;
                player.position.X -= speed;
                controlConsole.Update(-speed, 0);
                laserConsole.Update(-speed, 0);
                teleportConsole.Update(-speed, 0);
                turret.Update(-speed, 0, elapsed);
                laser.Update(-speed, 0);
                shipKernel.UpdatePosition(position);
                shield.Update(-speed, 0);
            }

        }

        public void MoveLeft(Player player, Player player2, float elapsed)
        {
            if (position.X > 0 + shipRing.Width / 2)
            {
                position.X -= speed;
                player.position.X -= speed;
                player2.position.X -= speed;
                controlConsole.Update(-speed, 0);
                laserConsole.Update(-speed, 0);
                teleportConsole.Update(-speed, 0);
                turret.Update(-speed, 0, elapsed);
                laser.Update(-speed, 0);
                shipKernel.UpdatePosition(position);
                shield.Update(-speed, 0);
            }

        }

        public void MoveRight(Player player, float elapsed)
        {
            if (position.X < screenSize.X - shipRing.Width/2)
            {
                position.X += speed;
                player.position.X += speed;
                controlConsole.Update(speed, 0);
                laserConsole.Update(speed, 0);
                teleportConsole.Update(speed, 0);
                turret.Update(speed, 0, elapsed);
                laser.Update(speed, 0);
                shipKernel.UpdatePosition(position);
                shield.Update(speed, 0);
            }
        }

        public void MoveRight(Player player, Player player2, float elapsed)
        {
            if (position.X < screenSize.X - shipRing.Width / 2)
            {
                position.X += speed;
                player.position.X += speed;
                player2.position.X += speed;
                controlConsole.Update(speed, 0);
                teleportConsole.Update(speed, 0);
                laserConsole.Update(speed, 0);
                turret.Update(speed, 0, elapsed);
                laser.Update(speed, 0);
                shipKernel.UpdatePosition(position);
                shield.Update(speed, 0);
            }
        }

        public void ResetPosition()
        {
            position.X = screenSize.X / 2f;
            position.Y = screenSize.Y / 2f;
        }

        public void Hurt()
        {
            health -= 10;
            if (health == 0 || health < 0)
                alive = false;
        }

        public void Hurt(int x)
        {
            {
                health -= 1;
                if (health == 0 || health < 0)
                    alive = false;
            }


        }

        public void Shoot()
        {
            Bullet aShot = new Bullet(cm, this);
            bullets.Add(aShot); 
        }

        public void UpdateBullets()
        {
            for (int i = 0; i < bullets.Count; i++)
                bullets[i].Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (alive)
            //{
            turret.Draw(spriteBatch);
            laser.Draw(spriteBatch);
            shipKernel.DrawKernel(spriteBatch);
            spriteBatch.Draw(shipRing, position, shipRec, Color.White, 0,
                new Vector2(shipRing.Width, shipRing.Height) * 0.5f, 1,
                SpriteEffects.None, 0);
            spriteBatch.Draw(healthBar, position + new Vector2(0, -143), healthRec, Color.White, 0,
                new Vector2(healthBar.Width, healthBar.Height) * 0.5f, 1,
                SpriteEffects.None, 0);
            controlConsole.Draw(spriteBatch);
            laserConsole.Draw(spriteBatch);
            teleportConsole.Draw(spriteBatch);
            shield.Draw(spriteBatch);
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
            //}
        }
    }
}