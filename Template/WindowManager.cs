using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using SkiaSharp;
using SkiaTemplate.Entities.UI;

namespace SkiaTemplate;

public class WindowManager
{
    public const int WIDTH = 1200;
    public const int HEIGHT = 900;

    private static ImGuiController? _imguiController;
    public static IWindow? ActiveWindow { get; private set; }

    public static event Action<double>? OnUpdate;
    public static event Action<SKCanvas, SKPaint>? OnDraw;
    
    public void StartWindow()
    {
        try
        {
            // Initialize Window
            WindowOptions windowOptions = WindowOptions.Default;
            windowOptions.Size = new Vector2D<int>(WIDTH, HEIGHT);
            windowOptions.Title = "Template";
            windowOptions.PreferredStencilBufferBits = 8;
            windowOptions.PreferredBitDepth = new Vector4D<int>(8, 8, 8, 8);

            GlfwWindowing.Use();

            IWindow? window = Window.Create(windowOptions);
            window.Initialize();

            using GRGlInterface grGlInterface =
                GRGlInterface.Create(name => window.GLContext!.TryGetProcAddress(name, out var addr) ? addr : 0);
            grGlInterface.Validate();

            // OpenGL Context Creation
            using GRContext? grContext = GRContext.CreateGl(grGlInterface);

            GRBackendRenderTarget renderTarget = new(WIDTH, HEIGHT,
                0, 8, new GRGlFramebufferInfo(0, 0x8058)); // 0x8058 = GL_RGBA8`

            // Initialize Skia.
            using SKSurface surface =
                SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            using SKCanvas canvas = surface.Canvas;

            // Initialize ImGUI and Input.
            IInputContext input = window.CreateInput();

            foreach (IKeyboard key in input.Keyboards)
                key.KeyDown += Input.KeyDown;

            _imguiController = new ImGuiController(
                window.CreateOpenGL(), // load OpenGL
                window, // pass in our window
                input // create an input context
            );

            ImGuiControlPanel imGuiControlPanel = new();

            // Run Window Functions.
            window.Update += deltaTime => OnUpdate?.Invoke(deltaTime > 0.5 ? 0.0 : deltaTime);

            window.Render += deltaTime =>
            {
                Render(deltaTime, grContext, canvas);
                imGuiControlPanel.RenderGUI(deltaTime, _imguiController);
            };

            ActiveWindow = window;
            ActiveWindow.Run();
            ActiveWindow.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void Render(double deltaTime, GRContext grContext, SKCanvas canvas)
    {
        using SKPaint skPaint = new();
        skPaint.IsAntialias = true;

        grContext.ResetContext();
        canvas.Clear(SKColors.Black);
        
        OnDraw?.Invoke(canvas, skPaint);
        
        canvas.Flush();
        grContext.Flush();
    }
}