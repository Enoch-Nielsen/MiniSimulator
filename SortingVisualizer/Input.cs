using Silk.NET.Input;
using SkiaTemplate.Xml;

namespace SkiaTemplate;

public class Input
{
    public static event Action? OnClose;

    public static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        switch (key)
        {
            case Key.Escape:
                OnClose?.Invoke();
                break;
        }
    }
}