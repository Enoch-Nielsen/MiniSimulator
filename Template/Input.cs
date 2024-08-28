using Silk.NET.Input;
using SkiaTemplate.Xml;

namespace SkiaTemplate;

public class Input
{
    public static event Action? OnClose;

    public static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        OnClose?.Invoke();

        switch (key)
        {
            case Key.Escape:
                WindowManager.ActiveWindow?.Close();
                break;
        }
    }
}