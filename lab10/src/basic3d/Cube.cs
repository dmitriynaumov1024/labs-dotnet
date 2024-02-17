namespace Lab10;

public class Cube
{
    public static readonly float[] Verts = new float[] {
        -0.5f, -0.5f,  0.5f, // Bottom-left
         0.5f, -0.5f,  0.5f, // Bottom-right
         0.5f,  0.5f,  0.5f, // Top-right

        -0.5f, -0.5f,  0.5f, // Bottom-left
         0.5f,  0.5f,  0.5f, // Top-right
        -0.5f,  0.5f,  0.5f, // Top-left

        // Back face
        -0.5f, -0.5f, -0.5f, // Bottom-left
         0.5f, -0.5f, -0.5f, // Bottom-right
         0.5f,  0.5f, -0.5f, // Top-right

        -0.5f, -0.5f, -0.5f, // Bottom-left
         0.5f,  0.5f, -0.5f, // Top-right
        -0.5f,  0.5f, -0.5f, // Top-left

        // Left face
        -0.5f, -0.5f, -0.5f, // Bottom-back
        -0.5f, -0.5f,  0.5f, // Bottom-front
        -0.5f,  0.5f,  0.5f, // Top-front

        -0.5f, -0.5f, -0.5f, // Bottom-back
        -0.5f,  0.5f,  0.5f, // Top-front
        -0.5f,  0.5f, -0.5f, // Top-back

        // Right face
         0.5f, -0.5f, -0.5f, // Bottom-back
         0.5f, -0.5f,  0.5f, // Bottom-front
         0.5f,  0.5f,  0.5f, // Top-front

         0.5f, -0.5f, -0.5f, // Bottom-back
         0.5f,  0.5f,  0.5f, // Top-front
         0.5f,  0.5f, -0.5f, // Top-back

        // Bottom face
        -0.5f, -0.5f, -0.5f, // Back-left
         0.5f, -0.5f, -0.5f, // Back-right
         0.5f, -0.5f,  0.5f, // Front-right

        -0.5f, -0.5f, -0.5f, // Back-left
         0.5f, -0.5f,  0.5f, // Front-right
        -0.5f, -0.5f,  0.5f, // Front-left

        // Top face
        -0.5f,  0.5f, -0.5f, // Back-left
         0.5f,  0.5f, -0.5f, // Back-right
         0.5f,  0.5f,  0.5f, // Front-right

        -0.5f,  0.5f, -0.5f, // Back-left
         0.5f,  0.5f,  0.5f, // Front-right
        -0.5f,  0.5f,  0.5f  // Front-left
    };

    public static readonly float[] VertsNormals = new float[] {
    //   Position              Normal
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Front face
         0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Back face
         0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, // Left face
        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, // Right face
         0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Bottom face
         0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Top face
         0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
    };
}
