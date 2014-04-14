using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace project
{
    class EnemyTeleport
    {
        enum State
        {
            Telport,
            Shooting
        }

        const float MaxSpeed = 8.5f;
        const float TurnSpeed = 1000f;
        const float Hysteresis = 15f;
        const float EvadeDistance = 400f;
        public Texture2D Texture;
        Vector2 TextureCenter;
        public Vector2 Position;
        State state = State.Shooting;
        public float Orientation;
        Vector2 ScreenSize;
        List<Bullet> bullets = new List<Bullet>();
        int timeInShoot = 0;
        int timeInTele = 0;
        ContentManager cm;
        public Vector2 heading;
        public Rectangle rect;

        Random random = new Random();

        public EnemyTeleport(ContentManager content, Vector2 screenSize)
        {
            Texture = content.Load<Texture2D>(@"Sprites/teleporter");
            TextureCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Position = new Vector2(20, 30);
            Orientation = 3;
            ScreenSize = screenSize;
            cm = content;
            rect = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Update(Ship ship)
        {
            if (state == State.Shooting)
            {
                timeInShoot++;
                timeInTele = 0;
                Orientation = TurnToFace(Position, ship.GetPosition(), Orientation, TurnSpeed);
                if (timeInShoot == 50)
                    Shoot();
                if (timeInShoot > 150)
                    state = State.Telport;
            }
            else if (state == State.Telport)
            {
                timeInShoot = 0;
                timeInTele++;
                if (timeInTele == 30)
                {
                    Teleport(ship);
                    state = State.Shooting;
                }
            }

            heading = new Vector2((float)Math.Cos(Orientation), (float)Math.Sin(Orientation));
            
            for (int i = 0; i < bullets.Count; i++)
                bullets[i].Update();

            for (int i = 0; i < bullets.Count; i++)
                if (bullets[i].Outside())
                    bullets.RemoveAt(i);
            Rectangle shiprec = new Rectangle((int)ship.GetPosition().X - 190, (int)ship.GetPosition().Y - 190, ship.shipRing.Width, ship.shipRing.Height);
            for (int i = 0; i < bullets.Count; i++)
            {
                if (shiprec.Intersects(new Rectangle((int)bullets[i].Position.X, (int)bullets[i].Position.Y, bullets[i].texture.Width, bullets[i].texture.Height)))
                    bullets.RemoveAt(i);
            }
        }

        private void Teleport(Ship ship)
        {
            Position = new Vector2(random.Next(1850), random.Next(950));
            while (Vector2.Distance(Position, ship.GetPosition()) < 240f)
            {
                Position = new Vector2(random.Next(1850), random.Next(950));
            }
        }

        private static float TurnToFace(Vector2 position, Vector2 faceThis,
            float currentAngle, float turnSpeed)
        {
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            float desiredAngle = (float)Math.Atan2(y, x);

            //float difference = WrapAngle(desiredAngle - currentAngle);

            //difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            return desiredAngle;
        }

        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }

        public void Shoot()
        {
            Bullet aShot = new Bullet(cm, this);
            bullets.Add(aShot);  
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Position, Color.White);
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(batch);
            }
        }

        public void ShiftPos(int i)
        {
            if (i == 1)
            {
                Position.Y += 8;
                foreach (Bullet aBullet in bullets)
                {
                    aBullet.Position.Y += 10;
                }
            }
            if (i == 2)
            {
                Position.Y -= 8;
                foreach (Bullet aBullet in bullets)
                {
                    aBullet.Position.Y -= 10;
                }
            }
            if (i == 3)
            {
                Position.X += 8;
                foreach (Bullet aBullet in bullets)
                {
                    aBullet.Position.X += 8;
                }
            }
            if (i == 4)
            {
                Position.X -= 8;
                foreach (Bullet aBullet in bullets)
                {
                    aBullet.Position.X -= 8;
                }
            }
        }
    }
}
