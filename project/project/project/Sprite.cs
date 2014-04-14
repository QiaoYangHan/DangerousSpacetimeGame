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
    class Sprite
    {
        public Texture2D getSpriteImage()
        {
            return spriteImage; 
        }private Texture2D spriteImage;

        public Vector2 getPosition()
        {
            return position;
        }
        private Vector2 position;

        public void setPosition(Vector2 newPos)
        {
            position.X = newPos.X;
            position.Y = newPos.Y;
        }

        public int getWidth()
        {
            return width;
        }
        private int width;

        public int getHeight()
        {
            return height;
        }
        private int height;

        public Rectangle getSpriteRec()
        {
            return spriteRec;
        }
        public Rectangle spriteRec;

        //constructor
        public Sprite(Texture2D image, Vector2 pos)
        {
            spriteImage = image;
            position = pos;
            width = image.Width;
            height = image.Height;
            spriteRec = new Rectangle((int)position.X, (int)position.Y, image.Width, image.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteImage, position, Color.White);    
        }

    }
}
