using OpenTK.Mathematics;
using Simwin;

namespace SimwinTesting;

public static class Program
{
    public static void Main()
    {
        Window window = WindowBuilder.CreateWindow(1280, 720, "Test Window");
        Canvas canvas = new();

        Paint black = new();
        black.SetColor(Color4.Black);
        
        canvas.Clear(black);
        canvas.DrawTri(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0.5f));
        
        window.Run();
    }
}