using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace project
{
    class EnemyArbitrary
    {
        private Texture2D texture;
        public Vector2 Position;
        public float Rotation;
        private Rectangle size;
        public Vector2 velocity;
        Vector2 origin;
        public int Gen;


        Random random = new Random();

        public EnemyArbitrary(ContentManager content, Vector2 screenSize, int Generation)
        {
            texture = content.Load<Texture2D>(@"Sprites/enemyArbitrary");               
            Position.X = screenSize.X / 2 - texture.Width / 2;
            Position.Y = screenSize.Y / 2 - texture.Height / 2;
            Rotation = (float)random.NextDouble() * .008f;
            size = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2, texture.Height/2);
            Gen = Generation;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public Vector2 GetOrigin()
        {
            return origin;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, Position, null, Color.White, Rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
