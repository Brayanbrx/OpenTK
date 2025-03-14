using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK
{
    public class Game : GameWindow
    {
        private int VertexArrayObject;
        private int VertexBufferObject;
        private Shader shader;

        private float[] vertices =
        {
            -0.5f, -0.5f, 0.0f,  // Vértice inferior izquierdo
             0.5f, -0.5f, 0.0f,  // Vértice inferior derecho
             0.0f,  0.5f, 0.0f   // Vértice superior
        };
        
        public Game(int width, int height, string title) : base
        (GameWindowSettings.Default, new NativeWindowSettings()
        { ClientSize = new Vector2i(width, height), Title = title }){ }



        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f); // Color de fondo

            // Crear VAO
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // Crear VBO y pasar los datos del triángulo
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Configurar el formato de los datos en el VAO
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Cargar y compilar los shaders
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //base.OnRenderFrame(e);
            //GL.Clear(ClearBufferMask.ColorBufferBit); //Limpia la pantalla usando el color definido en OnLoad, 1ra funcion al renderizar

            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GL.DeleteVertexArray(VertexArrayObject);
            GL.DeleteBuffer(VertexBufferObject);
            shader.Dispose();
        }





    }
}
