using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace project
{
    class Bullet
    {
        public Texture2D texture;
        public Vector2 Position;
        public float Rotation;
        private Rectangle size;
        float Speed = 1f;

        Vector2 origin;
        Vector2 direction;
        bool outside;

        public Rectangle rect;

        public Bullet(ContentManager content, EnemyEvader ee)
        {
            texture = content.Load<Texture2D>(@"Sprites/bullet");
            Position = new Vector2(ee.Position.X + ee.Texture.Width * 0.5f, ee.Position.Y + ee.Texture.Height * 0.5f);
            Rotation = ee.Orientation;
            direction = ee.heading;
            size = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            rect = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
        }

        public Bullet(ContentManager content, EnemyTeleport et)
        {
            texture = content.Load<Texture2D>(@"Sprites/bullet");
            Position = new Vector2(et.Position.X + et.Texture.Width * 0.5f, et.Position.Y + et.Texture.Height * 0.5f);
            Rotation = et.Orientation;
            direction = et.heading;
            size = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public Bullet(ContentManager content, Laser laser)
        {
            texture = content.Load<Texture2D>(@"Sprites/laserShot");
            Position = laser.position + new Vector2(0, -texture.Height);
            Rotation = laser.rotation;
            direction = laser.direction;
            size = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public Bullet(ContentManager content, Ship ship)
        {
            texture = content.Load<Texture2D>(@"Sprites/shipBullet");
            Rotation = ship.turret.rotation;
            direction = 2 * ship.turret.direction;
            Position = new Vector2(ship.turret.bulletPosition.X, ship.turret.bulletPosition.Y);
            size = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public Vector2 GetOrigin()
        {
            return origin;
        }

        public void Update()
        {
            Position += direction * 12;
            rect = new Rectangle((int)Position.X, (int)Position.Y, (int)texture.Width, (int)texture.Height);
        }

        private void GetOutside()
        {
            if (Position.X < 0 || Position.X > 1900 || Position.Y < 0 || Position.Y > 950)
                outside = true;
        }

        public bool Outside()
        {
            GetOutside();
            return outside;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, Position, null, Color.White, Rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
