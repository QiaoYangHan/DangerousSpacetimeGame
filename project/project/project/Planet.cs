using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace project
{
    class Planet
    {
        public Vector2 position;
        public Texture2D Texture;
        Vector2 TextureCenter;
        public Rectangle rect;
        int theInt;
        int fakeTime = 0;
        ContentManager cm;
        public bool Captured;

        

        public Planet(ContentManager content)
        {
            Captured = false;
            cm = content;
            theInt = 1;
            position = new Vector2(-1900, 1800);
            Texture = content.Load<Texture2D>(@"Sprites/bluePlanetSmall");
            TextureCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
            rect = new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);            
        }

        public Planet(ContentManager content, int x)
        {
            Captured = false;
            cm = content;
            theInt = x;
            if (x == 1)
                position = new Vector2(950, -1500);
            else if (x == 3)
                position = new Vector2(-1800, -800);
            else
                position = new Vector2(3690, 1770);
            if (x == 3)
                Texture = content.Load<Texture2D>(@"Sprites/marblePlanet");
            else
            Texture = content.Load<Texture2D>(@"Sprites/gasGiant");            
            TextureCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
            rect = new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
        }

        //public void Update(Ship aship, int score)
        //{
        //    if(fakeTime == 500)
        //    {
        //        EnemyChaser ec = new EnemyChaser(cm, new Vector2(position.X, position.Y));
        //        ecs.Add(ec);
        //        fakeTime = 0;
        //    }
        //    fakeTime++;
        //    Rectangle shipRectangle = new Rectangle((int)aship.GetPosition().X - 190, (int)aship.GetPosition().Y - 190, aship.shipRing.Width, aship.shipRing.Height);
            
                         
        //    for (int i = 0; i < ecs.Count; i++)
        //    {
        //        ecs[i].Update(aship);
        //        Rectangle eRect = new Rectangle((int)ecs[i].Position.X, (int)ecs[i].Position.Y, ecs[i].Texture.Width, ecs[i].Texture.Height);
        //        if (shipRectangle.Intersects(eRect))
        //        {
        //            ecs.RemoveAt(i);
        //            aship.Hurt();
        //            score++;
        //        }
        //    }
        //}

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

        public void Draw(SpriteBatch batch)
        {
            
            if (theInt == 1 || theInt == 3)
                batch.Draw(Texture, position, Color.White);
            else 
                batch.Draw(Texture, position, null, Color.Aqua, 0f, new Vector2(0,0), .77f, SpriteEffects.None, 0);

        }
    }
}