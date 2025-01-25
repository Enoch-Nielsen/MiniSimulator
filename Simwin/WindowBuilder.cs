using OpenTK.Windowing.Desktop;

namespace Simwin;

public static class WindowBuilder
{
    /// <summary>
    /// Creates and Returns a new Window with the specified Width, Height, and Title.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static Window CreateWindow(int width, int height, string title)
    {
        return new Window(GameWindowSettings.Default, new NativeWindowSettings()
        {
            ClientSize = (width, height),
            Title = title
        });
    }
}