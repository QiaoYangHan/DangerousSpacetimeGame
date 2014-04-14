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
    class Turret
    {
        public Vector2 position;
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
        private Matrix turretMatrix;
        private Matrix localPosition;

        private int times = 0;

        MouseState prevMouseState;

        public Turret(ContentManager content, Ship aShip, int anXShift, int aYShift)
        {
            xShift = anXShift;
            yShift = aYShift;
            position.X = aShip.GetPosition().X + (float)xShift;
            position.Y = aShip.GetPosition().Y + (float)yShift;
            Texture = content.Load<Texture2D>(@"Sprites/turret");
            conTexture = content.Load<Texture2D>(@"Sprites/console");
            rec = new Rectangle((int)position.X, (int)position.Y, conTexture.Width, conTexture.Height);
            shipPos.X = aShip.GetPosition().X;
            shipPos.Y = aShip.GetPosition().Y;
            rotation = 0.0f;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            prevMouseState = Mouse.GetState();
            localVector = new Vector2(0, position.Y - shipPos.Y);
        }

        private Matrix GetShipWorldMatrix()
        {
            return Matrix.CreateScale(1f, 1f, 1f) *
                   Matrix.CreateRotationZ(0) *
                   Matrix.CreateTranslation(new Vector3(shipPos, 0f));
        }

        public void Update(float x, float y, float elapsed)
        {
            position.X += x;
            position.Y += y;
            shipPos.X += x;
            shipPos.Y += y;
            rec = new Rectangle((int)position.X, (int)position.Y, conTexture.Width, conTexture.Height);
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {

            }

            if (Math.Sqrt(Math.Pow(Math.Abs(mousePosition.X - position.X), 2) + Math.Pow(Math.Abs(mousePosition.Y - position.Y), 2)) > 200f)
            {
                rotation = Tool.TurnToFace(position, mousePosition, rotation, 0.05f);
                turretMatrix = Matrix.CreateRotationZ(rotation) * GetShipWorldMatrix();
                position = Vector2.Transform(localVector, turretMatrix);
            } 
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
            //spritebatch.Draw(conTexture, rec, Color.AntiqueWhite);
        }
    }
}
