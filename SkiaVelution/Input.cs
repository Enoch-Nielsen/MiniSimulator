using Silk.NET.Input;

namespace SkiaVelution;

public class Input
{
    public static event Action OnClose; 
    public static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        OnClose?.Invoke();
        
        if (key == Key.Escape)
            Graphics.ActiveWindow.Close();
    }
}