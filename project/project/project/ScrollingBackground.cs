using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//  Holly shit
//  That was annoying
namespace project
{
    class ScrollingBackground
    {
        private Vector2 screenpos, origin, texturesize, texturesizew;
        private Texture2D mytexture11, mytexture12, mytexture13, mytexture21, mytexture22, mytexture23, mytexture31, mytexture32, mytexture33;
        private Vector2 screen1, screen2, screen3, screen4, screen5, screen6, screen7, screen8, screen9, origin2, origin3, origin4, origin5;
        private int screenheight, screenwidth;
        public void Load(GraphicsDevice device, Texture2D bT11, Texture2D bT12,Texture2D bT13,Texture2D bT21,Texture2D bT22,Texture2D bT23,Texture2D bT31,Texture2D bT32,Texture2D bT33)        
        {
            mytexture11 = bT11;
            mytexture12 = bT12;
            mytexture13 = bT13;
            mytexture21 = bT21;
            mytexture22 = bT22;
            mytexture23 = bT23;
            mytexture31 = bT31;
            mytexture32 = bT32;
            mytexture33 = bT33;
            screenheight = device.Viewport.Height;
            screenwidth = device.Viewport.Width;
            // Set the origin so that we're drawing from the 
            // center of the top edge.
            origin = new Vector2(0, 0);
            // Set the screen position to the center of the screen.
            screenpos = new Vector2(screenwidth / 2, screenheight / 2);
            // Offset to draw the second texture, when necessary.
            texturesize = new Vector2(0, mytexture11.Height);
            texturesizew = new Vector2(mytexture11.Width, 0);
            origin2 = new Vector2(950, -475);
            origin3 = new Vector2(-950, 475);
            origin4 = new Vector2(950, 1425);
            origin5 = new Vector2(2850, 475);

            screen1 = new Vector2(0, 0);
            screen2 = new Vector2(0, 950);//
            screen3 = new Vector2(1900, 0);//
            screen4 = new Vector2(1900, 950);
            screen5 = new Vector2(-1900, 0);//
            screen6 = new Vector2(0, -950);//
            screen7 = new Vector2(-1900, -950);
            screen8 = new Vector2(1900, -950);
            screen9 = new Vector2(-1900, 950);
        }

        public void Update(int i)
        {
            if (i == 1)
            {
                screen1.Y += 6f;
                screen2.Y += 6f;
                screen3.Y += 6f;
                screen4.Y += 6f;
                screen5.Y += 6f;
                screen6.Y += 6f;
                screen7.Y += 6f;
                screen8.Y += 6f;
                screen9.Y += 6f;
            }
            if (i == 2)
            {
                screen1.Y -= 6f;
                screen2.Y -= 6f;
                screen3.Y -= 6f;
                screen4.Y -= 6f;
                screen5.Y -= 6f;
                screen6.Y -= 6f;
                screen7.Y -= 6f;
                screen8.Y -= 6f;
                screen9.Y -= 6f;
            }
            if (i == 3)
            {
                screen1.X += 6f;
                screen2.X += 6f;
                screen3.X += 6f;
                screen4.X += 6f;
                screen5.X += 6f;
                screen6.X += 6f;
                screen7.X += 6f;
                screen8.X += 6f;
                screen9.X += 6f;
            }
            if (i == 4)
            {
                screen1.X -= 6f;
                screen2.X -= 6f;
                screen3.X -= 6f;
                screen4.X -= 6f;
                screen5.X -= 6f;
                screen6.X -= 6f;
                screen7.X -= 6f;
                screen8.X -= 6f;
                screen9.X -= 6f;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            
            
            batch.Draw(mytexture22, screen1, null,
            Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(mytexture23, screen2, null,
            Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(mytexture32, screen3, null,
            Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(mytexture33, screen4, null,
            Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(mytexture12, screen5, null,
            Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(mytexture21, screen6, null,
            Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(mytexture11, screen7, null,
            Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(mytexture31, screen8, null,
            Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(mytexture13, screen9, null,
            Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}
