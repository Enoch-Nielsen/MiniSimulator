using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Simwin;

public class Canvas
{
    public static Canvas Instance { get; private set; }
    
    
    public Canvas()
    {
        Instance ??= this;
    }

    public void DrawTri(Vector2 a, Vector2 b, Vector2 c)
    {
        float[] vertices =
        [
            a.X, a.Y, 0.0f, //Bottom-left vertex
            b.X, b.Y, 0.0f, //Bottom-right vertex
            c.X,  c.Y, 0.0f  //Top vertex
        ]; 
        
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
    }
    
    /// <summary>
    /// Clears the Canvas.
    /// </summary>
    /// <param name="paint"></param>
    public void Clear(Paint paint)
    {
        GL.ClearColor(paint.Color.R, paint.Color.G, paint.Color.B, paint.Color.A);
        GL.Clear(ClearBufferMask.ColorBufferBit);
    }
}