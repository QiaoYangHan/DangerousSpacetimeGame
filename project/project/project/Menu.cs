using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project
{
    class Menu
    {
        private List<string> MenuItems;
        private int iterator;
        public string InfoText { get; set; }
        public string Title { get; set; }
        public int Iterator
        {
            get
            {
                return iterator;
            }
            set
            {
                iterator = value;
                if (iterator > MenuItems.Count - 1) iterator = MenuItems.Count - 1;
                if (iterator < 0) iterator = 0;
            }
        }

        public Menu()
        {
            Title = "Jump Shot";
            MenuItems = new List<string>();
            MenuItems.Add("Play Game");
            MenuItems.Add("Exit Game");
            Iterator = 0;
            InfoText = string.Empty;
        }

        public int GetNumberOfOptions()
        {
            return MenuItems.Count;
        }

        public string GetItem(int index)
        {
            return MenuItems[index];
        }

        public void DrawMenu(SpriteBatch batch, int screenWidth, SpriteFont courierNewBig, SpriteFont courierNew)
        {
            batch.DrawString(courierNewBig, Title, new Vector2(screenWidth / 2 - courierNewBig.MeasureString(Title).X / 2, 20), Color.White);
            int yPos = 100;
            for (int i = 0; i < GetNumberOfOptions(); i++)
            {
                Color colour = Color.Gray;
                if (i == Iterator)
                {
                    colour = Color.White;
                }
                batch.DrawString(courierNew, GetItem(i), new Vector2(screenWidth / 2 - courierNew.MeasureString(GetItem(i)).X / 2, yPos), colour);
                yPos += 50;
            }
            
        }

        public void DrawEndScreen(SpriteBatch batch, int screenWidth, SpriteFont courierNew)
        {
            batch.DrawString(courierNew, InfoText, new Vector2(screenWidth / 2 - courierNew.MeasureString(InfoText).X / 2, 300), Color.White);
            string prompt = "Press Enter to Continue";
            batch.DrawString(courierNew, prompt, new Vector2(screenWidth / 2 - courierNew.MeasureString(prompt).X / 2, 400), Color.White);
        }
    }
}
