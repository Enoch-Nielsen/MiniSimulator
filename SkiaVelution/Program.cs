// See https://aka.ms/new-console-template for more information

namespace SkiaVelution;

public class SkiaVelution
{
    static void Main()
    {
        Graphics graphics = new();
        Model model = new(graphics);
        
        model.Initialize();

        graphics.OnUpdate += model.Update;
        
        graphics.StartWindow();
    }
}