using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsPhoneGame1
{
    class Braile
    {
        public struct braileDot
        {
            public bool state;
            public bool touched;
            public Rectangle rect;

            public braileDot(int x, int y, int width, int height)
            {
                touched = false;
                state = true;
                rect = new Rectangle(x, y, width, height);
            }
        };

        static Dictionary<char, int[]> alphabet = new Dictionary<char, int[]>()
            {
                {'a', new int[] {0}}, {'b', new int[] {0, 1}},
                {'c', new int[] {0, 3}}, {'d', new int[] {0, 3, 4}},
                {'e', new int[] {0, 4}}, {'f', new int[] {0, 1, 3}},
                {'g', new int[] {0, 1, 3, 4}}, {'h', new int[] {0, 1, 4}},
                {'i', new int[] {1, 3}}, {'j', new int[] {1, 3, 4}},
                {'k', new int[] {0, 1, 2}}, {'l', new int[] {0, 1, 2}},
                {'m', new int[] {0, 2, 3}}, {'n', new int[] {0, 2, 3, 4}},
                {'o', new int[] {0, 2, 4}}, {'p', new int[] {0, 1, 2, 5}},
                {'q', new int[] {0, 1, 2, 3, 4}}, {'r', new int[] {0, 1, 2, 4}},
                {'s', new int[] {1, 2, 3}}, {'t', new int[] {1, 2, 3, 4}},
                {'u', new int[] {0, 2, 5}}, {'v', new int[] {0, 1, 2, 5}},
                {'w', new int[] {1, 3, 4, 5}}, {'x', new int[] {0, 2, 3, 5}},
                {'y', new int[] {0, 2, 3, 4, 5}}, {'z', new int[] {0, 2, 4, 5}},

                {'.', new int[] {1, 4, 5}}, {',', new int[] {1}},
                {'?', new int[] {1, 5}}, {';', new int[] {1, 2}},
                {'!', new int[] {1, 2, 4}}, {'-', new int[] {2, 5}},

                {'#', new int[] {1, 3, 5, 7}}
            };

        public const int cBraileDotsNum = 15;
        int dispHeight, dispWidth;
        int braileDotWidth, braileDotHeight;

        public braileDot[] braileLetter1;

        public Braile(GraphicsDeviceManager graphics)
        {
            braileLetter1 = new braileDot[cBraileDotsNum];

            dispHeight = graphics.PreferredBackBufferHeight;
            dispWidth = graphics.PreferredBackBufferWidth;

            braileDotWidth = dispWidth / 3; // We have 3 dots from left to right
            braileDotHeight = braileDotWidth; // Square braile dot

            // Create first braile letter
            int num = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    braileLetter1[num] = new braileDot(j * braileDotWidth, i * braileDotHeight,
                        braileDotWidth, braileDotHeight);
                    num++;
                }
            }
        }

        static public void setBraileState(Braile.braileDot[] braileLetter, char letter)
        {
            for (int i = 0; i < Braile.cBraileDotsNum; i++)
            {
                braileLetter[i].state = false;
            }

            foreach (int n in alphabet[letter])
            {
                // We suppose that braile will display in order:
                // 345
                // 012
                int trLetter = n;

                /*switch (n)
                {
                    case 0:
                        trLetter = 3;
                        break;

                    case 1:
                        trLetter = 4;
                        break;

                    case 2:
                        trLetter = 5;
                        break;

                    case 3:
                        trLetter = 0;
                        break;

                    case 4:
                        trLetter = 1;
                        break;

                    case 5:
                        trLetter = 2;
                        break;
                }*/

                braileLetter[trLetter].state = true;
            }
        }

        public static string[] prepareText(string text)
        {
            text = text.ToLower();
            StringBuilder sb = new StringBuilder();

            foreach (char c in text)
            {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || 
                    c == ' ' || c == '.' || c == '!' || c == '?' || c == ';' ||
                    c == ',' ||  c == '-')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Split();
        }
    }
}
