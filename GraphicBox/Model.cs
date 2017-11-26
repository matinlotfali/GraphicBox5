using OpenTK.Graphics;
using System;

namespace GraphicBox
{
    internal class Model
    {		
		public int width = 200;
		public int height = 200;
        public int frameRate = 100;
        public bool firstDraw = true;
        public Color4 stroke, fill;
        public int frameDrawElapseMiliseconds;
    }
}
