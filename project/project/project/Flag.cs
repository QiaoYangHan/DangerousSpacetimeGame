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
    class Flag
    {
        public Vector2 position;
        public Texture2D img;
        public Flag(Texture2D texture, Vector2 pos)
        {
            img = texture;
            position = pos;
        }

        public void ShiftPos(int i)
        {
            if (i == 1)
            {
                position.Y += 10;
            }
            if (i == 2)
            {
                position.Y -= 10;
            }
            if (i == 3)
            {
                position.X += 10;
            }
            if (i == 4)
            {
                position.X -= 10;
            }
        }
    }
}
