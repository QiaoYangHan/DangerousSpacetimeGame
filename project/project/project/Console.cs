using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace project
{
    class Console
    {
        private Vector2 position;
        private Texture2D texture;
        public Rectangle consoleRec;
        int xShift;
        int yShift;

        public Console(ContentManager content, Ship aShip, int anXShift, int aYShift, Texture2D img)
        {
            xShift = anXShift;
            yShift = aYShift;
            position.X = aShip.GetPosition().X + (float)xShift;
            position.Y = aShip.GetPosition().Y + (float)yShift;
            texture = img;
            consoleRec = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(float x, float y)
        {
            position.X += x;
            position.Y += y;
            consoleRec = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, Color.White);
        }
    }
}
