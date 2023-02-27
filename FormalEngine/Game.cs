// Game.cs
// This script communicates and overrides with openTK
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
        // This handles the vertex buffer
        private int vertexBufferHandle;
        private int shaderProgramHandle;
        private int vertexArrayHandle;

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

        // When resizing the window
        protected override void OnResize(ResizeEventArgs e)
        {
            // Resize the openGL viewport to the new window dimensions
            GL.Viewport
            (
                0, // X position
                0, // Y position
                e.Width, // Window width
                e.Height // Window height
            );
            base.OnResize(e);
        }

        // When window is opened
        protected override void OnLoad()
        {
            // Set the color buffer
            GL.ClearColor
                (Color4.DarkGreen);

            // Triangle vertices
            float[] vertices = new float[]
            {
                0.0f, 0.5f, 0.0f, // vertex0
                0.5f, -0.5f, 0.0f, // vertex1
                -0.5f, -0.5f, 0.0f, // vertex2
            };

            this.vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(this.vertexArrayHandle);

            GL.BindBuffer
                (BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.VertexAttribPointer
                (0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            string vertexShaderCode =
                @"
                #version 330 core

                layout (location = 0) in vec3 aPosition;

                void main()
                {
                    gl_Position = vec4(aPosition, 1.0);
                }
                ";

            string fragmentShaderCode = 
                @"
                    #version 330 core

                    out vec4 pixelColor;

                    void main()
                    {
                        pixelColor = vec4(0.8f, 0.8f, 0.1f, 1);
                    }
                ";

            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
            GL.CompileShader(vertexShaderHandle);

            int fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderCode);
            GL.CompileShader(fragmentShaderHandle);

            this.shaderProgramHandle = GL.CreateProgram();

            GL.AttachShader(this.shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(this.shaderProgramHandle, fragmentShaderHandle);

            GL.LinkProgram(this.shaderProgramHandle);

            GL.DetachShader(this.shaderProgramHandle, vertexShaderHandle);
            GL.DetachShader(this.shaderProgramHandle, fragmentShaderHandle);

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);

            base.OnLoad();
        }

        // When window is about to close
        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(this.vertexBufferHandle);

            GL.UseProgram(0);
            GL.DeleteProgram(this.shaderProgramHandle);

            base.OnUnload();
        }

        // Every frame
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        // This handles the rendering
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Clear buffer at every frame render
            GL.Clear
                (ClearBufferMask.ColorBufferBit);

            GL.UseProgram
                (this.shaderProgramHandle);
            GL.BindVertexArray(this.vertexBufferHandle);

            GL.DrawArrays
                (PrimitiveType.Triangles, 0, 3);

            // Display the buffer
            this.Context.SwapBuffers();

            base.OnRenderFrame(args);
        }
    }
}
