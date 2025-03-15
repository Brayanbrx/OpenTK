using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenTK;


public class Game : GameWindow
{
    private Shader shader;
    private Cube cube;
    private List<Matrix4> cubeTransforms;

    public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings()
    { ClientSize = new Vector2i(width, height), Title = title })
    { }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        shader = new Shader("shader.vert", "shader.frag");
        cube = new Cube(shader);

        Matrix4 view = Matrix4.LookAt(new Vector3(6.0f, 6.0f, 6.0f), Vector3.Zero, Vector3.UnitY);
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);

        shader.Use();
        shader.SetMatrix4("view", view);
        shader.SetMatrix4("projection", projection);

        cubeTransforms = new List<Matrix4>();

        // 6 cubos de alto
        for (int i = 0; i < 6; i++)
            cubeTransforms.Add(Matrix4.CreateTranslation(-2.0f, i, 0.0f));

        //  6 cubos de alto
        for (int i = 0; i < 6; i++)
            cubeTransforms.Add(Matrix4.CreateTranslation(2.0f, i, 0.0f));

        //  4 cubos de ancho
        for (int i = -1; i <= 2; i++)
            cubeTransforms.Add(Matrix4.CreateTranslation(i, 0.0f, 0.0f));
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        foreach (var transform in cubeTransforms)
        {
            cube.Draw(transform, Matrix4.LookAt(new Vector3(6.0f, 6.0f, 6.0f), Vector3.Zero, Vector3.UnitY),
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f));
        }

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        cube.Dispose();
        shader.Dispose();
    }
}
