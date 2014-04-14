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
    class ShipKernel
    {
        // the blueprint of the kernel
        public static Sprite[,] layout;

        private Sprite console;

        private Texture2D tile;

        private static int width;

        private static int height;

        public Vector2 spritePosition;

        public Vector2 shipPosition;

        private Stream layoutStream;

        public ShipKernel(Stream layoutFile,Texture2D tileSprite, Texture2D consoleSprite, Vector2 shipPos)
        {
            tile = tileSprite;
            width = tileSprite.Width;
            height = tileSprite.Height;
            spritePosition.X = shipPos.X - 85 * (float)Math.Sqrt(2);
            spritePosition.Y = shipPos.Y - 85 * (float)Math.Sqrt(2);
            shipPosition.X = shipPos.X - 85 * (float)Math.Sqrt(2);
            shipPosition.Y = shipPos.Y - 85 * (float)Math.Sqrt(2);
            LoadStream(layoutFile);
            layoutStream = layoutFile;
        }

        private void LoadStream(Stream fileStream)
        {
            List<string> lines = new List<string>();
            width = 0;
            height = 0;
            // scan the layout of the ship kernel
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (width < line.Length)
                        width = line.Length;
                    lines.Add(line);
                    ++height;
                    line = reader.ReadLine();
                }
            }
            // the code above retrieves information about width and height

            layout = new Sprite[height, width];
            //load sprite according to the layout
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (lines[i][j] == '1')
                    {
                        layout[i, j] = new Sprite(tile, spritePosition);
                    }
                    else
                    {
                        layout[i, j] = null;
                    }
                    spritePosition.X += 6;
                }
                spritePosition.X = shipPosition.X;
                spritePosition.Y += 6;
            }
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            shipPosition.X = newPosition.X - 85 * (float)Math.Sqrt(2);
            shipPosition.Y = newPosition.X - 85 * (float)Math.Sqrt(2);
            spritePosition.X = newPosition.X - 85 * (float)Math.Sqrt(2);
            spritePosition.Y = newPosition.Y - 85 * (float)Math.Sqrt(2);
            for(int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    if (layout[i, j] != null)
                    {
                        layout[i, j].setPosition(spritePosition);
                        layout[i, j].spriteRec = new Rectangle((int)layout[i, j].getPosition().X, (int)layout[i, j].getPosition().Y, 6, 6);
                    }
                    spritePosition.X += 6;
                }
                spritePosition.X = shipPosition.X;
                spritePosition.Y += 6;
            }
        }

        public void DrawKernel(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (layout[i, j] != null)
                    {
                        layout[i, j].Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
