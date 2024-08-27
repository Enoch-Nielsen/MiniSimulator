using System.Drawing;
using ImGuiNET;
using Silk.NET.Input;
using SkiaSharp;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using Silk.NET.OpenGL.Extensions.ImGui;
using System.Numerics;
using SkiaVelution.Objects;
using SkiaVelution.UI;

namespace SkiaVelution;

public class Graphics
{
    public const int WIDTH = 1600;
    public const int HEIGHT = 900;
    
    public event Action<double>? OnUpdate;

    private List<VisualEntity> _drawables = new();

    public static IWindow ActiveWindow { get; private set; }
    
    private static IWindow _window;
    private static ImGuiController _imguiController;

    private GL _gl;
    
    public void StartWindow()
    {
        try
        {
            WindowOptions windowOptions = WindowOptions.Default;
            windowOptions.Size = new Vector2D<int>(WIDTH, HEIGHT);
            windowOptions.Title = "Skiavelution";
            windowOptions.PreferredStencilBufferBits = 8;
            windowOptions.PreferredBitDepth = new Vector4D<int>(8, 8, 8, 8);
            
            GlfwWindowing.Use();
        
            IWindow window = Window.Create(windowOptions);
            window.Initialize();
            
            using GRGlInterface grGlInterface = GRGlInterface.Create((name => window.GLContext!.TryGetProcAddress(name, out var addr) ? addr : (IntPtr) 0));
            grGlInterface.Validate();
            
            using GRContext? grContext = GRContext.CreateGl(grGlInterface);
            
            GRBackendRenderTarget renderTarget = new GRBackendRenderTarget(WIDTH, HEIGHT, 
                0, 8, new GRGlFramebufferInfo(0, 0x8058)); // 0x8058 = GL_RGBA8`
            
            using SKSurface surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            using SKCanvas canvas = surface.Canvas;
            
            IInputContext input = window.CreateInput();
            
            foreach (IKeyboard key in input.Keyboards)
                key.KeyDown += Input.KeyDown;
            
            _imguiController = new ImGuiController(
                _gl = window.CreateOpenGL(), // load OpenGL
                window, // pass in our window
                input // create an input context
            );

            BoidImGUI boidImGui = new();
            
            window.Update += Update;
            
            window.Render += delta =>
            {
                Render(delta, grContext, canvas);
                boidImGui.RenderGUI(delta, _imguiController);
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
    
    private void Update(double deltaTime) => OnUpdate?.Invoke(deltaTime);
    

    private void Render(double deltaTime, GRContext grContext, SKCanvas canvas)
    {
        grContext.ResetContext();
        canvas.Clear(SKColors.CadetBlue);
        
        SKPaint skPaint = new();
        skPaint.Style = SKPaintStyle.StrokeAndFill;
        
        foreach (VisualEntity drawable in _drawables)
        {
            drawable.Draw(canvas, skPaint);
        }
                
        canvas.Flush();
    }

    public void AddDrawable(VisualEntity visualEntity)
    {
        _drawables.Add(visualEntity);
    }

    public void RemoveDrawable(VisualEntity visualEntity)
    {
        _drawables.Remove(visualEntity);
    }
}