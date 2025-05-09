using Silk.NET.Maths;
using SkiaSharp;
using SkiaVelution.Lib;
using SkiaVelution.Settings;

namespace SkiaVelution.Entities.Boids;

public class FinchManager
{
    
    // Model Config and Values
    private const int FINCH_COUNT = 512;
    private const float TILE_SIZE = 256;
    private const int GROUP_COUNT = 1;
    private const int FRAME_COUNT = 60;
    public static FinchBehaviour FinchBehaviour { get; private set; } = new();

    // Values.
    public static double Runtime { get; private set; } = 0f;

    private List<double> _frames = new();
    public static int FPS { get; private set; }
    
    public static FinchManager Instance { get; private set; }

    // Boids
    public static Dictionary<Vector2D<int>, Dictionary<int, Finch>> FinchDict { get; } = new();

    public FinchManager() => Initialize();

    public void Initialize()
    {
        Instance = this;
        
        // FinchBoidBehaviour = new FinchBoidBehaviour()
        // {
        //     VisionRange = 20f,
        //     
        //     GroupVisionRange = 128f,
        //     GroupAvoidanceFactor = 0.02f,
        //     
        //     AvoidanceFactor = 0.03f,
        //     MatchingFactor = 0.25f,
        //     CenteringFactor = 0f,
        //     
        //     TurnFactor = 4f,
        //     MinimumSpeed = 3f,
        //     MaximumSpeed = 7.5f,
        //     XMargin =  100f,
        //     YMargin = 100f,
        // };
        
        // Initialize Finch Dictionary
        for (int x = -1; x < (WindowManager.WIDTH/TILE_SIZE)+1; x++)
            for (int y = -1; y < (WindowManager.HEIGHT/TILE_SIZE)+1; y++)
                FinchDict.Add(new Vector2D<int>(x, y), new Dictionary<int, Finch>());
        
        Random random = new();

        for (int g = 0; g < GROUP_COUNT; g++)
        {
            byte red = (byte)RandomRange(random, 0, 255);
            byte green = (byte)RandomRange(random, 0, 255);
            byte blue = (byte)RandomRange(random, 0, 255);
            
            SKColor color = new SKColor(red, green, blue);


            float group_x = RandomRange(random, BoidSettings.XMargin, WindowManager.WIDTH - BoidSettings.XMargin);
            float group_y = RandomRange(random, BoidSettings.YMargin, WindowManager.HEIGHT - BoidSettings.YMargin);
        
            for (int i = 0; i < FINCH_COUNT / GROUP_COUNT; i++)
            {
                float x = RandomRange(random, -35, 35);
                float y = RandomRange(random, -35, 35);
                Vector2D<float> Position = new(group_x + x, group_y + y);
            
                float scale = RandomRange(random, 2f, 8f);

                int id = i + (g * (FINCH_COUNT / GROUP_COUNT));
                Finch finchInstance = new(g, id, new Transform(position: Position, scale: scale, 0f), color);
                FinchDict[ConvertToTilePosition(Position)].Add(id, finchInstance);
            }
        }
    }

    public static bool CheckTileValid(Vector2D<int> tile)
    {
        bool output = (tile.X >= 0 && tile.X <= (WindowManager.WIDTH / TILE_SIZE)+1) && 
                      (tile.Y >= 0 && tile.Y <= (WindowManager.HEIGHT / TILE_SIZE)+1);
        
        return output;
    }

    public void MoveBoidToTile(Vector2D<int> newTile, Vector2D<int> oldTile, int id)
    {
        FinchDict[newTile].Add(id, FinchDict[oldTile][id]);
        FinchDict[oldTile].Remove(id);
    }

    public static Vector2D<int> ConvertToTilePosition(Vector2D<float> position)
    {
        int x = (int)(position.X / TILE_SIZE);
        int y = (int)(position.Y / TILE_SIZE);

        return new Vector2D<int>(x, y);
    }

    private float RandomRange(Random random, float min, float max) => (float)((random.NextDouble() * (max - min)) + min);
}