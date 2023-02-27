// Game.cs
// This script communicates and overrides with openTK
using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FormalEngine
{
    public class Game : GameWindow
    {
        // This handles the vertex buffer
        private int vertexBufferHandle;
        private int shaderProgramHandle;
        private int vertexArrayHandle;
        private int elementBufferObject;

        // Triangle vertices
        float[] vertices = new float[]
        {
            0.0f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, // vertex0
            0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, // vertex1
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, // vertex2
        };
        // Rectangle vertices
        /*float[] vertices = new float[]{
                 0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, // top right
                 0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, // bottom right
                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, // bottom left
                -0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, // top left
            };*/

        uint[] indices = new uint[]
        {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };
        float[] texCoords = new float[]
        {
            0.0f, 0.0f,  // lower-left corner  
            1.0f, 0.0f,  // lower-right corner
            0.5f, 1.0f   // top-center corner
        };

        public Game(int width = 1280, int height = 720, string title = "FormalEngine window")
            : base(
                  GameWindowSettings.Default,
                  new NativeWindowSettings()
                  {
                      Title = title,
                      Size = new Vector2i(width, height),
                      WindowBorder = WindowBorder.Fixed,
                      StartVisible = false,
                      StartFocused = true,
                      API = ContextAPI.OpenGL,
                      Profile = ContextProfile.Core,
                      APIVersion = new Version(3, 3),
                  }
                )
        {
            this.CenterWindow();
            Console.WriteLine("Window starting...");
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
            Console.WriteLine("Window resized!");
            base.OnResize(e);
        }

        // When window is opened
        protected override void OnLoad()
        {
            this.IsVisible = true;
            Console.WriteLine("Window loaded!");

            // Set the color buffer
            GL.ClearColor
                (Color4.DarkGreen);

            

            this.vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);

            this.vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(this.vertexArrayHandle);

            GL.BindBuffer
                (BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.VertexAttribPointer
                (0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
            GL.VertexAttribPointer
                (1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);


            string vertexShaderCode = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Shaders/shader.vert"));

            string fragmentShaderCode = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Shaders/shader.frag"));

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
            Console.WriteLine("Quitting...");
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(this.vertexArrayHandle);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(this.vertexBufferHandle);

            GL.UseProgram(0);
            GL.DeleteProgram(this.shaderProgramHandle);

            base.OnUnload();
        }

        // Every frame
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            Vector4 vec = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90.0f));
            Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
            Matrix4 trans = rotation * scale;
            vec *= trans;
            //Console.WriteLine("{0}, {1}, {2}", vec.X, vec.Y, vec.Z);


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
                (PrimitiveType.Triangles, 0, this.indices.Length);


            GL.DrawElements(PrimitiveType.Triangles, this.indices.Length, DrawElementsType.UnsignedInt, 0);

            // Display the buffer
            this.Context.SwapBuffers();

            base.OnRenderFrame(args);
        }
    }
}
