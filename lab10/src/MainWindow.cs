namespace Lab10;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

public class MainWindow : GameWindow
{
    private MyConfig myConfig;

    // These are the handles to OpenGL objects.
    private int VBO;
    private int shapeVAO;

    private float[] allVerts;

    private float ambientLightStrength = 0.6f;
    private Vector3 lightPos = new Vector3(1f, 4f, 40f);
    private Vector3 cameraPos = new Vector3(12f, 11f, 16f);
    private float cameraYaw = -124, cameraPitch = -28; // in degrees

    private Shader phongShader;

    private Observer camera, observer;

    const float cameraSpeed = 2.7f;
    const float sensitivity = 0.043f;

    private Vector2? lastPos = null;

    public MainWindow (
        GameWindowSettings gameWindowSettings, 
        NativeWindowSettings nativeWindowSettings,
        MyConfig config
    ): base(gameWindowSettings, nativeWindowSettings) 
    {
        myConfig = config;
    }

    // Now, we start initializing OpenGL.
    protected override void OnLoad()
    {
        base.OnLoad();

        observer = new Observer(cameraPos, Size.X / (float)Size.Y) { Yaw = cameraYaw };
        camera = new Observer(cameraPos, Size.X / (float)Size.Y) { Pitch = cameraPitch, Yaw = cameraYaw };

        GL.ClearColor(0.12f, 0.13f, 0.16f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        var digitRasters = PbmOneParser.File("./src-pbm/digits.pbm").Parse();
        var digitMapBuilder = new RasterGlyphBuilder() {
            Rasters = digitRasters, // the raster
            GlyphHeight = 12, // how many rows in one glyph
            Chars = new char[] { 
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' 
            } // the chars represented with that raster
        };

        var digitMap = digitMapBuilder.Build();

        Voxel[] shapes = (new Voxel[][] {
            digitMap['2'].Color(0.4f, 0.5f, 0.88f).ToVoxels(-20, 4, -3),
            digitMap['0'].Color(0.5f, 0.5f, 0.8f).ToVoxels(-12, 4, -3),
            digitMap['2'].Color(0.6f, 0.5f, 0.63f).ToVoxels(-4, 4, -3),
            digitMap['4'].Color(0.72f, 0.5f, 0.54f).ToVoxels(4, 4, -3),
        }).SelectMany(item => item).ToArray();

        allVerts = new float[Voxel.VertSize*shapes.Length*Voxel.VertCount]; // this fixed size buffer is gonna backfire.
        int allVertsIdx = 0;
        foreach (var shape in shapes) {
            var verts = shape.ToVerts();
            verts.CopyTo(allVerts, allVertsIdx);
            allVertsIdx += verts.Length;
        }

        this.VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBO);

        GL.BufferData(BufferTarget.ArrayBuffer, allVerts.Length * sizeof(float), allVerts, BufferUsageHint.StaticDraw);

        phongShader = new Shader (
            File.ReadAllText("./src-shader/shader.vert"), 
            File.ReadAllText("./src-shader/phong.frag")
        );

        shapeVAO = GL.GenVertexArray();
        GL.BindVertexArray(shapeVAO);

        var aPos = phongShader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(aPos);
        GL.VertexAttribPointer(aPos, 3, VertexAttribPointerType.Float, false, 9*sizeof(float), 0);

        var aNormal = phongShader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(aNormal);
        GL.VertexAttribPointer(aNormal, 3, VertexAttribPointerType.Float, false, 9*sizeof(float), 3*sizeof(float));

        var aColor = phongShader.GetAttribLocation("aColor");
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

        phongShader.Use();
        phongShader.SetMatrix4("model", Matrix4.Identity);
        phongShader.SetMatrix4("view", camera.GetViewMatrix());
        phongShader.SetMatrix4("projection", camera.GetProjectionMatrix());
        phongShader.SetFloat("ambientLightStrength", ambientLightStrength);
        phongShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
        phongShader.SetVector3("lightPos", lightPos);
        phongShader.SetVector3("viewPos", camera.Position);

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
            observer.Yaw -= 7 * cameraSpeed * (float)e.Time;
        }
        if (input.IsKeyDown(Keys.E)) {
            observer.Yaw += 7 * cameraSpeed * (float)e.Time;
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

        GL.DeleteProgram(phongShader.Handle);

        base.OnUnload();
    }
}
