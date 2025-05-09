using OpenTK.Mathematics;

namespace Simwin;

public class Paint
{
    public Color4 Color { get; private set; }

    public void SetColor(Color4 color) => Color = color;
}