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
    class Shield
    {
        //Ship Referencing
        private Vector2 shipPos;
        public Vector2 position;
        public Vector2 direction;

        //In Ship console stuff
        private Vector2 consolePosition;
        private Texture2D consoleTexture;
        public Rectangle consoleRec;
        int xShift;
        int yShift;

        //The Shield Stuff
        private Texture2D shieldTexture;
        public Rectangle shieldRec;
        public float rotation;
        private Vector2 origin;
        public Rectangle rec;
        private Vector2 localVector;
        private Matrix shieldMatrix;
        private Matrix localPosition;
        public bool shouldUpdate;
        public bool clockWise;
        private float angle = MathHelper.PiOver4;
        const float ShipRotateSpd = .05f;
        

        public Shield(ContentManager content, Ship aShip, int anXShift, int aYShift)
        {
            xShift = anXShift;
            yShift = aYShift;
            consolePosition.X = aShip.GetPosition().X + (float)xShift;
            consolePosition.Y = aShip.GetPosition().Y + (float)yShift;
            shipPos.X = aShip.GetPosition().X;
            shipPos.Y = aShip.GetPosition().Y;
            
            consoleTexture = content.Load<Texture2D>(@"Sprites/shieldConsole");
            consoleRec = new Rectangle((int)consolePosition.X, (int)consolePosition.Y, consoleTexture.Width, consoleTexture.Height);

            position.X = aShip.GetPosition().X + 0f;
            position.Y = aShip.GetPosition().Y - 165f;
            shieldTexture = content.Load<Texture2D>(@"Sprites/shield");
            rotation = 0f;
            origin = new Vector2(shieldTexture.Width / 2, shieldTexture.Height / 2);
            shouldUpdate = false;
            clockWise = false;
            localVector = new Vector2(0, position.Y - shipPos.Y );
        }

        private Matrix GetShipWorldMatrix()
        {
            return Matrix.CreateScale(1f, 1f, 1f) *
                   Matrix.CreateRotationZ(0) *
                   Matrix.CreateTranslation(new Vector3(shipPos, 0f));
        }

        public void Update(float x, float y)
        {
            consolePosition.X += x;
            consolePosition.Y += y;
            consoleRec = new Rectangle((int)consolePosition.X, (int)consolePosition.Y, consoleTexture.Width, consoleTexture.Height);

            position.X += x;
            position.Y += y;
            
            shipPos.X += x;
            shipPos.Y += y;
        }

        public void UpdateShield()
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Q))
            {
                rotation += 0.02f;
                shieldMatrix = Matrix.CreateRotationZ(rotation) * GetShipWorldMatrix();
                position = Vector2.Transform(localVector, shieldMatrix);
            }
            if (keyboard.IsKeyDown(Keys.E))
            {
                rotation -= 0.02f;
                shieldMatrix = Matrix.CreateRotationZ(rotation) * GetShipWorldMatrix();
                position = Vector2.Transform(localVector, shieldMatrix);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(consoleTexture, consoleRec, Color.White);
            batch.Draw(shieldTexture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
