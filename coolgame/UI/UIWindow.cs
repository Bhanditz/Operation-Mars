﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coolgame
{
    public class UIWindow : UIElement
    {
        private List<Button> menuButtons;
        private List<UIElement> menuItems;
        private UIElement background;
        protected int spacing = 10;

        public bool ButtonPressed;

        public List<Button> GetButtons()
        {
            return menuButtons;
        }

        public void AddItem(UIElement item)
        {
            menuItems.Add(item);
            //ArrangeMenu();
        }

        public void AddItem(Button button)
        {
            menuButtons.Add(button);
            ArrangeMenu();
        }

        private void ArrangeMenu()
        {
            // calculate the space occupied by the buttons
            int totalHeight = spacing;
            int maxWidth = 0;
            foreach (Button b in menuButtons)
            {
                totalHeight += b.Height + spacing;
                if (b.Width > maxWidth)
                    maxWidth = b.Width;
            }
            maxWidth += spacing * 2;

            // reposition the window in the center of the screen and resize it to fit all the elements
            position.X = Game.GAME_WIDTH / 2 - maxWidth / 2;
            position.Y = Game.GAME_HEIGHT / 2 - totalHeight / 2;
            Width = maxWidth;
            Height = totalHeight;
            background.Position = position;
            background.Width = Width;
            background.Height = Height;

            // place all the buttons in their own spot
            menuButtons[0].Position = new Vector2(position.X + spacing, position.Y + spacing);
            for (int i = 1; i < menuButtons.Count; ++i)
            {
                menuButtons[i].Position = new Vector2(position.X + spacing, menuButtons[i - 1].Position.Y + menuButtons[i - 1].Height + spacing);
            }

        }

        public UIWindow (ContentManager Content, Vector2 position, int width, int height) : base(Content, position, width, height)
        {
            menuButtons = new List<Button>();
            menuItems = new List<UIElement>();
            background = new UIElement(Content, position, width, height);
            background.BackgroundColor = new Color(Color.SlateGray, 0.2f);
            ButtonPressed = false;
            text = "";
        }

        public void Update()
        {
            foreach(Button b in menuItems)
            {
                ButtonPressed = false;
                b.Update();
                if(b.Pressed)
                {
                    ButtonPressed = true;
                }
            }
        }

        new public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            foreach(Button b  in menuButtons)
            {
                b.Draw(spriteBatch);
            }
        }
    }
}
