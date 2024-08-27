using MiniTween;

namespace SkiaTemplate;

public static class Application
{
    public static readonly string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
    
    public static void Main()
    {
        WindowManager windowManager = new();
        Model model = new();
        Tween.Setup(action => WindowManager.OnUpdate += action);
        windowManager.StartWindow();
    }
}