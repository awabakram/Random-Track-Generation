using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Random_Track_Generation
{
    class Button
    {
        int height;
        int width;
        string buttonText;
        Vector2 buttonPosition;
        Vector2 stringPos;
        SpriteFont font;

        Color buttonColor; //the original color of the button, this wont change after its been set

        Color currentButtonColor;
        bool isHovering;

        MouseState currentMState;
        MouseState previousMState;
        
        

        public Button(int newHeight, int newWidth, string text, Vector2 position, SpriteFont newfont, MouseState mstate, Color newColor)
        {
            height = newHeight;
            width = newWidth;
            buttonText = text;
            buttonColor = newColor;
            buttonPosition = position;
            font = newfont;

            currentButtonColor = buttonColor;
            stringPos = new Vector2(buttonPosition.X + (width / 10), buttonPosition.Y + (height / 10));

            

            isHovering = false;
            previousMState = mstate;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(buttonPosition, new Size2(width, height), currentButtonColor, height/2);
            spriteBatch.DrawString(font, buttonText, stringPos , Color.Black);
            
        }


        public void Update(GameTime gameTime, MouseState mState)
        {
            //update the mouse state every frame
            currentMState = mState;



            //check if the mouse is hovering over the button in this frame
            isHovering = checkHovering(currentMState);

            //if the mouse is hovering over the button, make it blue, if it isn't then set it to its normal color
            if (isHovering)
            {
                currentButtonColor = Color.Blue;
            }
            else
            {
                currentButtonColor = buttonColor;
            }

            //if the button is clicked, run the code that is meant to be run when clicked
            if (checkClicked())
            {
                clicked();
            }



            //update the previous mouse state right at the end of the update method
            previousMState = currentMState;
        }

        bool checkHovering(MouseState mstate)
        {
            if ((mstate.X > buttonPosition.X) && (mstate.X <= (buttonPosition.X + width)) && (mstate.Y > buttonPosition.Y) && (mstate.Y <= (buttonPosition.Y + height))) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool checkClicked()
        {
            if (isHovering  && previousMState.LeftButton == ButtonState.Released && currentMState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void loadTextAsTexture2D()
        {
            
        }

        protected virtual void clicked()
        {

        }

    }
}
