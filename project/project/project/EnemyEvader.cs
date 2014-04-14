using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace project
{
    class EnemyEvader
    {
        enum State
        {
            Evading,
            Chasing,
            Guard,
            Shooting
        }

        const float MaxSpeed = 8.5f;
        const float TurnSpeed = .20f;
        const float Hysteresis = 15f;

        const float ChaseDistance = 1900f;
        const float CaughtDistance = 60f;

        const float EvadeDistance = 420f;

        const float range = 300f;

        public Texture2D Texture;
        Vector2 TextureCenter;
        public Vector2 Position;
        State state = State.Guard;
        public float Orientation;
        Vector2 WanderDirection;
        Vector2 ScreenSize;
        List<Bullet> bullets = new List<Bullet>();
        ContentManager cm;
        public Vector2 heading;
        int fakeTime = 0;
        float currentSpeed = 0;
        public Rectangle rect;
        Planet planet;

        Random random = new Random();

        public EnemyEvader(ContentManager content, Vector2 screenSize, int x, int y, Planet aPlanet)
        {
            planet = aPlanet;
            Texture = content.Load<Texture2D>(@"Sprites/evader");
            TextureCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Position = new Vector2(x, y);
            Orientation = 3;
            ScreenSize = screenSize;
            cm = content;
            rect = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Update(Ship ship)
        {
            float ChaseThreshold = ChaseDistance;
            float CaughtThreshold = CaughtDistance;
            
            float distanceFromShip = Vector2.Distance(Position, ship.GetPosition());

            if (Orientation < TurnToFace(Position, ship.GetPosition(), Orientation, TurnSpeed) && Orientation > TurnToFace(Position, ship.GetPosition(), Orientation, TurnSpeed))
            {
                state = State.Shooting;
                fakeTime++;
            }

            if(state == State.Shooting && fakeTime == 10)
            {
                Shoot();
                fakeTime = 0;
            }

            if (distanceFromShip > EvadeDistance + Hysteresis)
            {
                state = State.Guard;
            }

            else if (distanceFromShip < EvadeDistance - Hysteresis)
            {
                state = State.Evading;

                Shoot();
            }

            if (state == State.Evading)
            {
                Vector2 seekPosition = 2.5f * Position - ship.GetPosition();

                Orientation = TurnToFace(Position, seekPosition, Orientation, TurnSpeed);

                currentSpeed = MathHelper.Min(currentSpeed++, MaxSpeed);
            }

            if (state == State.Chasing)
            {
                ChaseThreshold += Hysteresis / 2;
                CaughtThreshold -= Hysteresis / 2;
            }

            else
            {
                Wander(Position, ref WanderDirection, ref Orientation, TurnSpeed);

                currentSpeed = .35f * MaxSpeed;
            }

            for (int i = 0; i < bullets.Count; i++)
                bullets[i].Update();

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Outside())
                    bullets.RemoveAt(i);                
            }

            Rectangle shiprec = new Rectangle((int)ship.GetPosition().X - 190, (int)ship.GetPosition().Y - 190, ship.shipRing.Width, ship.shipRing.Height);

            for (int i = 0; i < bullets.Count; i++)
            {
                if (shiprec.Intersects(new Rectangle((int)bullets[i].Position.X, (int)bullets[i].Position.Y, bullets[i].texture.Width, bullets[i].texture.Height)))
                {
                    bullets.RemoveAt(i);
                    ship.Hurt(1);
                }
            }

            heading = new Vector2((float)Math.Cos(Orientation), (float)Math.Sin(Orientation));
            Position += heading * currentSpeed;
        }

        private void Wander(Vector2 position, ref Vector2 wanderDirection,
            ref float orientation, float turnSpeed)
        {
            wanderDirection.X +=
               MathHelper.Lerp(-.85f, .85f, (float)random.NextDouble());
            wanderDirection.Y +=
                MathHelper.Lerp(-.85f, .85f, (float)random.NextDouble());

            if (wanderDirection != Vector2.Zero)
            {
                wanderDirection.Normalize();
            }

            orientation = TurnToFace(position, position + wanderDirection, orientation,
               .35f * turnSpeed);

            Vector2 screenCenter = Vector2.Zero;
            screenCenter.X = ScreenSize.X / 2;
            screenCenter.Y = ScreenSize.Y / 2;

            float distanceFromScreenCenter = Vector2.Distance(screenCenter, position);
            float MaxDistanceFromScreenCenter =
                Math.Min(screenCenter.Y, screenCenter.X);

            float normalizedDistance =
                distanceFromScreenCenter / MaxDistanceFromScreenCenter;

            float turnToCenterSpeed = .3f * normalizedDistance * normalizedDistance *
                turnSpeed;

            orientation = TurnToFace(position, new Vector2(planet.position.X + planet.Texture.Width/2, planet.position.Y + planet.Texture.Height/2), orientation,
                turnToCenterSpeed);
        }

        private static float TurnToFace(Vector2 position, Vector2 faceThis,
            float currentAngle, float turnSpeed)
        {
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            float desiredAngle = (float)Math.Atan2(y, x);

            float difference = WrapAngle(desiredAngle - currentAngle);

            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            return WrapAngle(currentAngle + difference);
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
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(batch);
            }
            batch.Draw(Texture, Position, Color.White);
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
