using System;
using System.Diagnostics;
using System.Threading;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GraphicBox
{
    public abstract partial class Graph
    {
        private static Model model = new Model();
        private static NativeWindow glWindow;
        private static GraphicsContext glContext;
        private static Stopwatch frameDrawElapseTimer = new Stopwatch();

        protected virtual void draw() { }
        protected virtual void setup() { }
        protected virtual void settings() { }

        public void Start()
        {
            settings();
            initgraph(model.width, model.height);

            while (glWindow.Exists)
            {
                glWindow.ProcessEvents();                        					

                if (model.firstDraw)
                {
                    setup();
                    model.firstDraw = false;
                }
                else
                    draw();

                delay(1000 / model.frameRate);
            }                

            glContext.Dispose();
            glWindow.Dispose();
        }

        protected static void main(string gapplet, string[] args)
        {
            StackFrame s = new StackFrame(1, false);
            var type = s.GetMethod().DeclaringType;
            if (type.ToString().EndsWith(gapplet, StringComparison.Ordinal))
            {
                var obj = Activator.CreateInstance(type);
                var g = obj as Graph;
                g.Start();
            }
            else
                throw new ArgumentException("The caller is not a GApplet or the name is not valid.");
        }

        public static void initgraph(int width, int height)
        {
			ColorFormat colorFormat = new ColorFormat(8);
			GraphicsMode glGraphicsMode = new GraphicsMode(colorFormat);

			glWindow = new NativeWindow(
                width, height, "GraphicBox", GameWindowFlags.FixedWindow,
				glGraphicsMode, DisplayDevice.Default);

			glContext = new GraphicsContext(glGraphicsMode, glWindow.WindowInfo);

			glContext.MakeCurrent(glWindow.WindowInfo);
			glContext.LoadAll();

			glWindow.Visible = true;
            frameDrawElapseTimer.Start();

			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.LineSmooth);
			GL.Enable(EnableCap.PolygonSmooth);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
			GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
        }

        public static void delay(int miliseconds)
        {
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			GL.Ortho(0, glWindow.Width, glWindow.Height, 0, -1, 1);
			GL.Viewport(0, 0, glWindow.Width, glWindow.Height);

			GL.Flush();
			glContext.SwapBuffers();

            var toSleep = miliseconds - (int)frameDrawElapseTimer.ElapsedMilliseconds;
            if(toSleep > 0)
                Thread.Sleep(toSleep);

            model.frameDrawElapseMiliseconds = (int)frameDrawElapseTimer.ElapsedMilliseconds;
            frameDrawElapseTimer.Restart();
		}

        public static int FPS
        {
            get { return 1000 / model.frameDrawElapseMiliseconds; }
        }
    }
}
