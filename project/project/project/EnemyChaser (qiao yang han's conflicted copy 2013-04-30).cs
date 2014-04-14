using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace project
{
    class EnemyChaser
    {
        enum State
        {
            Chasing,
            Caught,
            Wander
        }

        const float MaxSpeed = 5.0f;
        const float TurnSpeed = .1f;
        const float Hysteresis = 15f;
        const float ChaseDistance = 500f;
        const float CaughtDistance = 60f;
        public Texture2D Texture;
        Vector2 TextureCenter;
        public Vector2 Position;
        State state = State.Wander;
        float Orientation;
        Vector2 ScreenSize; 
        Vector2 WanderDirection;
        public Rectangle rect;
        Vector2 origin;

        Random random = new Random();

        public EnemyChaser(ContentManager content, Vector2 screenSize)
        {
            Texture = content.Load<Texture2D>(@"Sprites/chaser");
            TextureCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Position = screenSize;
            //if (random.NextDouble() < .25)
            //{
            //    Position = new Vector2(random.Next(1800, 1900), random.Next(950));
            //}
            //else if (random.NextDouble() < .5)
            //{
            //    Position = new Vector2(random.Next(0, 100), random.Next(950));
            //}
            //else if (random.NextDouble() < .75)
            //{
            //    Position = new Vector2(random.Next(1900), random.Next(0,50));
            //}
            //else
            //{
            //    Position = new Vector2(random.Next(1900), random.Next(900,950));
            //}
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Orientation = 3;
            ScreenSize = screenSize;
            rect = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Update(Ship ship)
        {
            float ChaseThreshold = ChaseDistance;
            float CaughtThreshold = CaughtDistance;

            if (state == State.Wander)
            {
                ChaseThreshold -= Hysteresis / 2;
            }

            else if (state == State.Chasing)
            {
                ChaseThreshold += Hysteresis / 2;
                CaughtThreshold -= Hysteresis / 2;
            }

            else if (state == State.Caught)
            {
                CaughtThreshold += Hysteresis / 2;
            }

            float distanceFromCat = Vector2.Distance(Position, ship.GetPosition());
            if (distanceFromCat > ChaseThreshold)
            {
                state = State.Wander;
            }
            else if (distanceFromCat > CaughtThreshold)
            {
                state = State.Chasing;
            }
            else
            {
                state = State.Caught;
            }
            float currentSpeed;
            if (state == State.Chasing)
            {
                // the  wants to chase the cat, so it will just use the TurnToFace
                // function to turn towards the cat's position. Then, when the 
                // moves forward, he will chase the cat.
                Orientation = TurnToFace(Position, ship.GetPosition(), Orientation,
                    TurnSpeed);
                currentSpeed = MaxSpeed;
            }
            else if (state == State.Wander)
            {
                // wander works just like the mouse's.
                Wander(Position, ref WanderDirection, ref Orientation,
                    TurnSpeed);
                currentSpeed = .25f * MaxSpeed;
            }
            else
            {
                currentSpeed = 0.0f;
            }
            Vector2 heading = new Vector2(
                (float)Math.Cos(Orientation), (float)Math.Sin(Orientation));
            Position += heading * currentSpeed;
        }

        private void Wander(Vector2 position, ref Vector2 wanderDirection,
            ref float orientation, float turnSpeed)
        {
            wanderDirection.X +=
               MathHelper.Lerp(-.25f, .25f, (float)random.NextDouble());
            wanderDirection.Y +=
                MathHelper.Lerp(-.25f, .25f, (float)random.NextDouble());

            if (wanderDirection != Vector2.Zero)
            {
                wanderDirection.Normalize();
            }

            orientation = TurnToFace(position, position + wanderDirection, orientation,
               .15f * turnSpeed);

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

            orientation = TurnToFace(position, screenCenter, orientation,
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

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Position, Color.White);
            batch.Draw(Texture, Position, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0.0f);
        }

        public void ShiftPos(int i)
        {
            if (i == 1)
            {
                Position.Y += 10;
            }
            if (i == 2)
            {
                Position.Y -= 10;
            }
            if (i == 3)
            {
                Position.X += 10;
            }
            if (i == 4)
            {
                Position.X -= 10;
            }
        }
    }
}
