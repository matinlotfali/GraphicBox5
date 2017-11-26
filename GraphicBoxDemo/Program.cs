using System;
using GraphicBox;

namespace GraphicBoxDemo
{
    class MainClass: Graph
    {
        public static void Main(string[] args)
        {
            Graph.main("MainClass", args);
        }

        int i = 0;

        protected override void draw()
        {
            clear();

            stroke(255,255,255,255);
            fill(0, 0, 0, 255);
            line(50, 50, 150, i);
            rect(i+50, 50, 100, 100, 
                 25);

            i++;
            if (i > 200)
                i = 0;

            Console.WriteLine("FPS: " + FPS);
        }
    }
}
