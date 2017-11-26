using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace GraphicBox
{
    public abstract partial class Graph
    {
        public void clear()
        {
            GL.ClearColor(255,0,0,255);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void stroke(int r, int g, int b, int a)
        {            
            model.stroke = new Color4(r, g, b, a);
        }

		public void fill(int r, int g, int b, int a)
		{
			model.fill = new Color4(r, g, b, a);
		}

        //public void beginShape(int kind)
        //{
        //    GL.Begin(PrimitiveType.Lines);
        //}

        //public void vertex(float x, float y)
        //{
        //    GL.Vertex2(x,y);
        //}

        //public void endShape()
        //{
        //    GL.End();
        //}

        public void line(float x1, float y1, float x2, float y2)
        {
            GL.Color4(model.stroke);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex2(x1, y1);
            GL.Vertex2(x2, y2);
            GL.End();
        }

        public void rect(float x, float y, float width, float height)
        {rect(x,y,width,height,0);}
		public void rect(float x, float y, float width, float height, float r)
        {rect(x, y, width, height, r, r, r, r);}
        public void rect(float x, float y, float width, float height, 
                         float tl, float tr, float br, float bl)
        {
            if (tl < 0)
                throw new ArgumentOutOfRangeException(nameof(tl), "The radious must not be negative.");
			if (tr < 0)
				throw new ArgumentOutOfRangeException(nameof(tr), "The radious must not be negative.");
			if (br < 0)
				throw new ArgumentOutOfRangeException(nameof(br), "The radious must not be negative.");
			if (bl < 0)
				throw new ArgumentOutOfRangeException(nameof(bl), "The radious must not be negative.");

            poly(() => 
            {
				GL.Vertex2(x+tl, y);

                float xc = x + width - tr;
                float yc = y + tr;
                GL.Vertex2(xc, y);
                if (tr > 0)
                {
                    double dt = Math.Atan2(1, tr);
                    for (double i = 0; i < Math.PI/2; i+= dt)
                    {
                        double xi = tr * Math.Sin(i) + xc;
                        double yi = tr * -Math.Cos(i) + yc;
                        GL.Vertex2(xi, yi);
                    }
                }

				xc = x + width - br;
                yc = y + height - br;
                GL.Vertex2(x + width, yc);
                if (br > 0)
                {
                    double dt = Math.Atan2(1, br);
                    for (double i = Math.PI/2; i < Math.PI; i += dt)
                    {
                        double xi = br * Math.Sin(i) + xc;
                        double yi = br * -Math.Cos(i) + yc;
                        GL.Vertex2(xi, yi);
                    }
                }

				xc = x + bl;
				yc = y + height - bl;
                GL.Vertex2(xc, y + height);
                if (bl > 0)
                {
                    double dt = Math.Atan2(1, bl);
                    for (double i = Math.PI; i <= Math.PI * 3 / 2; i+= dt)
                    {
                        double xi = bl * Math.Sin(i) + xc;
                        double yi = bl * -Math.Cos(i) + yc;
                        GL.Vertex2(xi, yi);
                    }
                }

                xc = x + tl;
				yc = y + tl;
				GL.Vertex2(x, y + tl);
                if (tl > 0)
                {
                    double dt = Math.Atan2(1, tl);
                    for (double i = Math.PI * 3 / 2; i <= 2 * Math.PI; i += dt)
                    {
                        double xi = tl * Math.Sin(i) + xc;
                        double yi = tl * -Math.Cos(i) + yc;
                        GL.Vertex2(xi, yi);
                    }
                }
            });			
        }

        private void poly(Action drawPoints)
        {
			GL.Color4(model.fill);
            GL.Begin(PrimitiveType.Polygon);
            drawPoints();
            GL.End();

			GL.Color4(model.stroke);
            GL.Begin(PrimitiveType.LineLoop);
            drawPoints();
            GL.End();
        }
    }
}
