using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsPhoneGame1
{
    class Controls
    {
        int dispHeight, dispWidth;
        
        public Rectangle letterControl;
        public float letterControlDeltaX;

        public Controls(GraphicsDeviceManager graphics)
        {
            dispHeight = graphics.PreferredBackBufferHeight;
            dispWidth = graphics.PreferredBackBufferWidth;

            letterControlDeltaX = 0;
            int letterControlWidth = dispWidth;
            int letterControlHeight = dispHeight / 5;
            letterControl = new Rectangle(0, dispHeight - letterControlHeight, letterControlWidth, letterControlHeight);
        }
    }
}
