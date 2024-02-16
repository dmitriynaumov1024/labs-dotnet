namespace Lab10;
using System.IO;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

public class MainWindow : GameWindow
{
    // These are the handles to OpenGL objects.
    private int VBO;
    private int shapeVAO, lightVAO;

    private float[] allVerts;

    private Vector3 lightPos = new Vector3(1.2f, 1.0f, 2.0f);
    private Vector3 cameraPos = new Vector3(12f, 11f, 16f);
    private float cameraYaw = -124, cameraPitch = -28; // in degrees

    private Shader shapeShader, lightShader;

    private Observer camera, observer;

    const float cameraSpeed = 1.7f;
    const float sensitivity = 0.043f;

    private Vector2? lastPos = null;

    public MainWindow (
        GameWindowSettings gameWindowSettings, 
        NativeWindowSettings nativeWindowSettings
    ): base(gameWindowSettings, nativeWindowSettings) { }

    // Now, we start initializing OpenGL.
    protected override void OnLoad()
    {
        base.OnLoad();

        observer = new Observer(cameraPos, Size.X / (float)Size.Y) { Yaw = cameraYaw };
        camera = new Observer(cameraPos, Size.X / (float)Size.Y) { Pitch = cameraPitch, Yaw = cameraYaw };

        GL.ClearColor(0.94f, 0.93f, 0.93f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        Voxel[] shapes = new[] {
            new Voxel(0, 1, 0, new float[]{ 0.12f, 0.56f, 0.78f }),
            new Voxel(1, 1, 0, new float[]{ 0.13f, 0.66f, 0.80f }),
            new Voxel(1, 1, 1, new float[]{ 0.14f, 0.68f, 0.85f }),
            new Voxel(2, -1, 1, new float[]{ 0.93f, 0.5f, 0.55f })
        };

        allVerts = new float[Voxel.VertSize*1024];
        int allVertsIdx = 0;
        foreach (var shape in shapes) {
            var verts = shape.ToVerts();
            verts.CopyTo(allVerts, allVertsIdx);
            allVertsIdx += verts.Length;
        }

        this.VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBO);

        GL.BufferData(BufferTarget.ArrayBuffer, allVerts.Length * sizeof(float), allVerts, BufferUsageHint.StaticDraw);

        shapeShader = new Shader (
            File.ReadAllText("./src-shader/shader.vert"), 
            File.ReadAllText("./src-shader/shader.frag")
        );

        lightShader = new Shader (
            File.ReadAllText("./src-shader/shader.vert"), 
            File.ReadAllText("./src-shader/lighting.frag")
        );

        shapeVAO = GL.GenVertexArray();
        GL.BindVertexArray(shapeVAO);

        var aPos = lightShader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(aPos);
        GL.VertexAttribPointer(aPos, 3, VertexAttribPointerType.Float, false, 9*sizeof(float), 0);

        var aNormal = lightShader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(aNormal);
        GL.VertexAttribPointer(aNormal, 3, VertexAttribPointerType.Float, false, 9*sizeof(float), 3*sizeof(float));

        var aColor = lightShader.GetAttribLocation("aColor");
        GL.EnableVertexAttribArray(aColor);
        GL.VertexAttribPointer(aColor, 3, VertexAttribPointerType.Float, false, 9*sizeof(float), 6*sizeof(float));

        CursorState = CursorState.Grabbed;

        // Setup is now complete! Now we move to the OnRenderFrame function to finally draw the triangle.
    }

    // Now that initialization is done, let's create our render loop.
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        System.Threading.Thread.Sleep(10);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.BindVertexArray(shapeVAO);

        lightShader.Use();
        lightShader.SetMatrix4("model", Matrix4.Identity);
        lightShader.SetMatrix4("view", camera.GetViewMatrix());
        lightShader.SetMatrix4("projection", camera.GetProjectionMatrix());
        lightShader.SetFloat("ambientLightStrength", 0.7f);
        // lightShader.SetVector3("objectColor", new Vector3(0.4f, 0.5f, 0.99f));
        lightShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
        lightShader.SetVector3("lightPos", lightPos);
        lightShader.SetVector3("viewPos", camera.Position);

        GL.DrawArrays(PrimitiveType.Triangles, 0, allVerts.Length / Voxel.VertSize);

        SwapBuffers();

        // And that's all you have to do for rendering! You should now see a yellow triangle on a black screen.
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (!IsFocused) {
            return;
        }

        var input = this.KeyboardState;

        if (input.IsKeyDown(Keys.Escape) && this.CursorState == CursorState.Grabbed) {
            this.CursorState = CursorState.Normal;
        }
        if (input.IsKeyDown(Keys.Space) && this.CursorState == CursorState.Normal) {
            this.CursorState = CursorState.Grabbed;
        }

        if (input.IsKeyDown(Keys.W)) {
            observer.Position += observer.Front * cameraSpeed * (float)e.Time; // Forward
        }
        if (input.IsKeyDown(Keys.S)) {
            observer.Position -= observer.Front * cameraSpeed * (float)e.Time; // Forward
        }
        if (input.IsKeyDown(Keys.A)) {
            observer.Position -= observer.Right * cameraSpeed * (float)e.Time; // Left
        }
        if (input.IsKeyDown(Keys.D)) {
            observer.Position += observer.Right * cameraSpeed * (float)e.Time; // Right
        }
        if (input.IsKeyDown(Keys.Q)) {
            observer.Yaw -= 17 * cameraSpeed * (float)e.Time;
        }
        if (input.IsKeyDown(Keys.E)) {
            observer.Yaw += 17 * cameraSpeed * (float)e.Time;
        }
        if (input.IsKeyDown(Keys.LeftShift)) {
            observer.Position += observer.Up * cameraSpeed * (float)e.Time; // Up
        }
        if (input.IsKeyDown(Keys.LeftControl)) {
            observer.Position -= observer.Up * cameraSpeed * (float)e.Time; // Down
        }

        camera.Position = observer.Position;
        camera.Yaw = observer.Yaw;
        
        if (this.CursorState == CursorState.Grabbed) {
            var mouse = this.MouseState;

            if (lastPos == null) {
                lastPos = new Vector2(mouse.X, mouse.Y);
            }

            var deltaX = mouse.X - lastPos.Value.X;
            var deltaY = mouse.Y - lastPos.Value.Y;

            // observer pitch is fixed
            observer.Yaw += deltaX * sensitivity;
            camera.Yaw = observer.Yaw;
            camera.Pitch -= deltaY * sensitivity;

            lastPos = new Vector2(mouse.X, mouse.Y);
        }
        else if (this.lastPos != null) {
            this.lastPos = null;
        }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        camera.Fov -= e.OffsetY;
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        camera.AspectRatio = Size.X / (float)Size.Y;
    }

    protected override void OnUnload()
    {
        // Unbind all the resources by binding the targets to 0/null.
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        // Delete all the resources.
        GL.DeleteBuffer(this.VBO);
        GL.DeleteVertexArray(shapeVAO);
        GL.DeleteVertexArray(lightVAO);

        GL.DeleteProgram(lightShader.Handle);
        GL.DeleteProgram(shapeShader.Handle);

        base.OnUnload();
    }
}
