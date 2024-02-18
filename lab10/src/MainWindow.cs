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

    private RasterGlyphMap glyphMap;

    private Observer camera, observer;
    private Vector2? lastPos = null;

    private Voxel[] shapes;

    private float[] allVerts;

    // handles to OpenGL objects
    private int VBO;
    private int shapeVAO;
    private Shader phongShader;

    public MainWindow (
        GameWindowSettings gameWindowSettings, 
        NativeWindowSettings nativeWindowSettings,
        MyConfig config
    ): base(gameWindowSettings, nativeWindowSettings) 
    {
        myConfig = config;
    }

    public static MainWindow Create (MyConfig config) 
    {
        var nativeSettings = new NativeWindowSettings() {
            ClientSize = new Vector2i(config.Window.Width, config.Window.Height),
            Title = config.Window.Title,
            Flags = ContextFlags.ForwardCompatible
        };

        return new MainWindow(GameWindowSettings.Default, nativeSettings, config);
    }

    protected void InitGlyphSources() 
    {
        this.glyphMap = new RasterGlyphMap();

        foreach (var source in myConfig.GlyphSources) {
            var fileParser = ImageParser.BooleanFormatParser(source.Format);
            if (fileParser == null) {
                throw new Exception("Image format not supported");
            }
            var rasters = fileParser(source.Source).Parse();
            var mapBuilder = new RasterGlyphBuilder() {
                Rasters = rasters, // the raster
                GlyphHeight = source.GlyphHeight, // how many rows in one glyph
                Chars = source.Chars.Select(item => item[0]).ToList()
            };
            this.glyphMap = this.glyphMap + mapBuilder.Build();
        }
    }

    protected void InitScene()
    {
        var cameraConfig = myConfig.Camera;
        var cameraPos = ToVector3(cameraConfig.Position);

        observer = new Observer(cameraPos, Size.X / (float)Size.Y) { 
            Yaw = cameraConfig.Yaw 
        };
        camera = new Observer(cameraPos, Size.X / (float)Size.Y) { 
            Pitch = cameraConfig.Pitch, 
            Yaw = cameraConfig.Yaw, 
            Fov = cameraConfig.Fov,
            Near = cameraConfig.Near,
            Far = cameraConfig.Far
        };

        var backColor = myConfig.Scene.BackgroundColor;
        GL.ClearColor(backColor[0], backColor[1], backColor[2], 1.0f);

        var voxels = new List<Voxel>(1024);

        foreach (var textSpan in myConfig.Text) {
            int[] pos = textSpan.Position;
            float[] color = textSpan.Color;
            int x = pos[0], y = pos[1], z = pos[2];
            foreach (char c in textSpan.Text) {
                var glyph = this.glyphMap[c];
                voxels.AddRange(glyph.Color(color).ToVoxels(x, y, z));
                x += glyph.Width;
            }
        }

        this.shapes = voxels.ToArray();
    }

    protected void InitVBO()
    {
        allVerts = new float[Voxel.VertSize*shapes.Length*Voxel.VertCount];
        int allVertsIdx = 0;
        foreach (var shape in shapes) {
            var verts = shape.ToVerts();
            verts.CopyTo(allVerts, allVertsIdx);
            allVertsIdx += verts.Length;
        }

        this.VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, allVerts.Length * sizeof(float), allVerts, BufferUsageHint.StaticDraw);
    }

    protected void InitRendering() 
    {
        GL.Enable(EnableCap.DepthTest);

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
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        this.InitGlyphSources();
        this.InitScene();
        this.InitVBO();
        this.InitRendering();

        CursorState = CursorState.Grabbed;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        System.Threading.Thread.Sleep(10);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.BindVertexArray(shapeVAO);

        phongShader.Use();
        phongShader.SetMatrix4("model", Matrix4.Identity);
        phongShader.SetVector3("viewPos", camera.Position);
        phongShader.SetMatrix4("view", camera.GetViewMatrix());
        phongShader.SetMatrix4("projection", camera.GetProjectionMatrix());
        phongShader.SetVector3("lightPos", ToVector3(myConfig.Scene.AmbientLightPosition));
        phongShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
        phongShader.SetFloat("ambientLightStrength", myConfig.Scene.AmbientLightStrength);

        GL.DrawArrays(PrimitiveType.Triangles, 0, allVerts.Length / Voxel.VertSize);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (!IsFocused) {
            return;
        }

        var moveSpeed = myConfig.Camera.MoveSpeed;
        var rotateSpeed = myConfig.Camera.RotateSpeed;
        var sensitivity = myConfig.Camera.MouseSensitivity;

        var input = this.KeyboardState;

        if (input.IsKeyDown(Keys.Escape) && this.CursorState == CursorState.Grabbed) {
            this.CursorState = CursorState.Normal;
        }
        if (input.IsKeyDown(Keys.Space) && this.CursorState == CursorState.Normal) {
            this.CursorState = CursorState.Grabbed;
        }

        if (input.IsKeyDown(Keys.W)) {
            observer.Position += observer.Front * moveSpeed * (float)e.Time; // Forward
        }
        if (input.IsKeyDown(Keys.S)) {
            observer.Position -= observer.Front * moveSpeed * (float)e.Time; // Forward
        }
        if (input.IsKeyDown(Keys.A)) {
            observer.Position -= observer.Right * moveSpeed * (float)e.Time; // Left
        }
        if (input.IsKeyDown(Keys.D)) {
            observer.Position += observer.Right * moveSpeed * (float)e.Time; // Right
        }
        if (input.IsKeyDown(Keys.Q)) {
            observer.Yaw -= rotateSpeed * (float)e.Time;
        }
        if (input.IsKeyDown(Keys.E)) {
            observer.Yaw += rotateSpeed * (float)e.Time;
        }
        if (input.IsKeyDown(Keys.LeftShift)) {
            observer.Position += observer.Up * moveSpeed * (float)e.Time; // Up
        }
        if (input.IsKeyDown(Keys.LeftControl)) {
            observer.Position -= observer.Up * moveSpeed * (float)e.Time; // Down
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
        // Unbind all the resources by binding the targets to 0/null
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        // Delete all the resources
        GL.DeleteBuffer(this.VBO);
        GL.DeleteVertexArray(this.shapeVAO);

        GL.DeleteProgram(this.phongShader.Handle);

        base.OnUnload();
    }

    public static Vector3 ToVector3 (float[] source) 
    {
        return new Vector3(source[0], source[1], source[2]);
    }

}
