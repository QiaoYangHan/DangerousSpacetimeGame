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
    class Laser
    {
        public Vector2 position;
        private Vector2 originalPosition;
        private Vector2 shipPos;
        public Vector2 direction;
        public Texture2D Texture;
        private Texture2D conTexture;
        int xShift;
        int yShift;
        public float rotation;
        private float angle = MathHelper.PiOver4;
        const float ShipRotateSpd = .05f;
        List<Bullet> Shots = new List<Bullet>();
        public Rectangle rec;
        private Vector2 origin;
        private Vector2 localVector;
        public Matrix laserMatrix;
        private Matrix localPosition;
        public bool shouldUpdate;
        public bool clockWise;
        public Rectangle laserRect;

        public List<Bullet> bullets = new List<Bullet>();

        MouseState prevMouseState;

        public Laser(ContentManager content, Ship aShip, int anXShift, int aYShift)
        {
            xShift = anXShift;
            yShift = aYShift;
            position.X = aShip.GetPosition().X + (float)xShift;
            position.Y = aShip.GetPosition().Y + (float)yShift;
            
            Texture = content.Load<Texture2D>(@"Sprites/lazer");
            conTexture = content.Load<Texture2D>(@"Sprites/console");
            rec = new Rectangle((int)position.X, (int)position.Y, conTexture.Width, conTexture.Height);
            laserRect = new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            shipPos.X = aShip.GetPosition().X;
            shipPos.Y = aShip.GetPosition().Y;
            rotation = 0f;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            prevMouseState = Mouse.GetState();
            localVector = new Vector2(0, position.Y - shipPos.Y - 55);
            shouldUpdate = false;
            clockWise = false;
        }

        private Matrix GetShipWorldMatrix()
        {
            return Matrix.CreateScale(1f, 1f, 1f) *
                   Matrix.CreateRotationZ(0) *
                   Matrix.CreateTranslation(new Vector3(shipPos, 0f));
        }

        public void Shoot(ContentManager cm)
        {
            Bullet aShot = new Bullet(cm, this);
            bullets.Add(aShot);
        }

        public void Update(float x, float y)
        {
            position.X += x;
            position.Y += y;
            shipPos.X += x;
            shipPos.Y += y;
            rec = new Rectangle((int)position.X, (int)position.Y, conTexture.Width, conTexture.Height);
            if ((rotation > - 2 * MathHelper.Pi && rotation < 2 * MathHelper.Pi) && shouldUpdate)
            {
                if (clockWise)
                {
                    rotation += 0.015f;
                }
                else
                {
                    rotation -= 0.015f;
                }
                laserMatrix = Matrix.CreateRotationZ(rotation) * GetShipWorldMatrix();
                position = Vector2.Transform(localVector, laserMatrix);
            }
            else
            {
                rotation = 0f;
                shouldUpdate = false;
                clockWise = false;
            }
            direction = new Vector2((float)Math.Cos(rotation - MathHelper.PiOver2), (float)Math.Sin(rotation - MathHelper.PiOver2));
            foreach (Bullet bullet in bullets)
            {
                bullet.Position = position + 186f * direction;
                bullet.Rotation = rotation;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (shouldUpdate)
            {
                foreach (Bullet bullet in bullets)
                {
                    bullet.Draw(spritebatch);
                }
                spritebatch.Draw(Texture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
            }

            //spritebatch.Draw(conTexture, rec, Color.AntiqueWhite);
        }
    }
}
