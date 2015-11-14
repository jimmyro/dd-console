using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDconsole
{
    static class Globals
    {
        public static int Dice(int sides)
        {
            var random = new Random(DateTime.Now.Millisecond);
            return (random.Next(1, sides));
        }
    }

    public struct Vector2
    {
        private int x, y;

        public Vector2(int myX, int myY)
        {
            x = myX;
            y = myY;
        }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return Y; } set { Y = value; } }

        public static Vector2 Zero()
        {
            return new Vector2(0, 0);
        }
    }
}
