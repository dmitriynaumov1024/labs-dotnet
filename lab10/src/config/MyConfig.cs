namespace Lab10;
using System;
using System.Collections.Generic;
using System.Linq;

public class MyConfig
{
    public WindowConfig Window { get; set; } = new WindowConfig();
    public List<GlyphSourceConfig> GlyphSources { get; set; } = new List<GlyphSourceConfig>();
    public SceneConfig Scene { get; set; } = new SceneConfig();
    public CameraConfig Camera { get; set; } = new CameraConfig();
    public List<TextSpanConfig> Text { get; set; } = new List<TextSpanConfig>();

    public class WindowConfig
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Title { get; set; }
    }

    public class GlyphSourceConfig
    {
        public string Format { get; set; }
        public string Source { get; set; }
        public List<String> Chars { get; set; }
        public int GlyphHeight { get; set; }
    }

    public class SceneConfig
    {
        public float[] BackgroundColor { get; set; } = new float[3];
        public float[] AmbientLightPosition { get; set; } = new float[3];
        public float AmbientLightStrength { get; set; }
    }

    public class CameraConfig
    {
        public float[] Position { get; set; } = new float[3];
        public float Fov { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float MoveSpeed { get; set; }
        public float RotateSpeed { get; set; }
        public float MouseSensitivity { get; set; }
    }

    public class TextSpanConfig
    {
        public string Text { get; set; }
        public float[] Color { get; set; } = new float[3];
        public int[] Position { get; set; } = new int[3];
    }
}
