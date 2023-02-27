using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace FormalEngine
{
    public class Game : GameWindow
    {
        public Game()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            // Center the window to the screen
            this.CenterWindow(
                new Vector2i(1280, 720)
            );

            // This sets the window title
            this.Title = "FormalEngine window";
        }

        // Every frame
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        // This handles the rendering
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Set the color buffer
            GL.ClearColor
                (Color4.DarkGreen);
            GL.Clear
                (ClearBufferMask.ColorBufferBit);

            // Display the buffer
            this.Context.SwapBuffers();

            base.OnRenderFrame(args);
        }
    }
}
