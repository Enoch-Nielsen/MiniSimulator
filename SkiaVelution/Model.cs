using System.Diagnostics;
using Silk.NET.Maths;
using SkiaSharp;
using System.Collections;
using SkiaVelution.Boids;
using SkiaVelution.Entities;
using SkiaVelution.Objects;
using SkiaVelution.UI;

namespace SkiaVelution;

public class Model
{
    private Graphics _graphics;

    public static Model Instance { get; private set; }

    // Model Config and Values
    public const double RUN_SPEED = 40.0;
    private const int FINCH_COUNT = 512;
    private const float TILE_SIZE = 256;
    private const int GROUP_COUNT = 8;
    private const int FRAME_COUNT = 60;

    public static FinchBoidBehaviour FinchBoidBehaviour { get; private set; } = new();
    public static FinchBehaviour FinchBehaviour { get; private set; } = new();

    // Values.
    public static double Runtime { get; private set; } = 0f;

    private List<double> _frames = new();
    public static int FPS { get; private set; }

    // Boids
    public static Dictionary<Vector2D<int>, Dictionary<int, Finch>> FinchDict { get; } = new();
        
    public Model(Graphics graphics)
    {
        _graphics = graphics;
    }

    public void Initialize()
    {
        Instance = this;
        
        FinchBoidBehaviour = new FinchBoidBehaviour()
        {
            VisionRange = 20f,
            
            GroupVisionRange = 128f,
            GroupAvoidanceFactor = 0.02f,
            
            AvoidanceFactor = 0.03f,
            MatchingFactor = 0.25f,
            CenteringFactor = 0f,
            
            TurnFactor = 4f,
            MinimumSpeed = 3f,
            MaximumSpeed = 7.5f,
            XMargin =  100f,
            YMargin = 100f,
        };

        FinchBehaviour = new FinchBehaviour()
        {
            
        };
        
        // Initialize Finch Dictionary
        for (int x = -1; x < (Graphics.WIDTH/TILE_SIZE)+1; x++)
            for (int y = -1; y < (Graphics.HEIGHT/TILE_SIZE)+1; y++)
                FinchDict.Add(new Vector2D<int>(x, y), new Dictionary<int, Finch>());
        
        Random random = new();

        for (int g = 0; g < GROUP_COUNT; g++)
        {
            byte red = (byte)RandomRange(random, 0, 255);
            byte green = (byte)RandomRange(random, 0, 255);
            byte blue = (byte)RandomRange(random, 0, 255);
            
            SKColor color = new SKColor(red, green, blue);
            
            float group_x = RandomRange(random, FinchBoidBehaviour.XMargin, Graphics.WIDTH - FinchBoidBehaviour.XMargin);
            float group_y = RandomRange(random, FinchBoidBehaviour.YMargin, Graphics.HEIGHT - FinchBoidBehaviour.YMargin);
            
            for (int i = 0; i < FINCH_COUNT / GROUP_COUNT; i++)
            {
                float x = RandomRange(random, -35, 35);
                float y = RandomRange(random, -35, 35);
                Vector2D<float> Position = new(group_x + x, group_y + y);
                
                float scale = RandomRange(random, 2f, 8f);

                int id = i + (g * (FINCH_COUNT / GROUP_COUNT));
                Finch finchInstance = new(g, id, Position, scale, color);
                FinchDict[ConvertToTilePosition(Position)].Add(id, finchInstance);
                _graphics.AddDrawable(finchInstance.Boid);
            }
        }
        
        _graphics.AddDrawable(new FPSCounter(new Vector2D<float>(50f, 50f), 1f, 0f));
    }

    public static bool CheckTileValid(Vector2D<int> tile)
    {
        bool output = (tile.X >= 0 && tile.X <= (Graphics.WIDTH / TILE_SIZE)+1) && 
                      (tile.Y >= 0 && tile.Y <= (Graphics.HEIGHT / TILE_SIZE)+1);
        
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

    private void TryAddFrame(double deltaTime)
    {
        if (_frames.Count == FRAME_COUNT)
            _frames.RemoveAt(0);
        
        _frames.Add(deltaTime);
    }

    /// <summary>
    /// Updates the boids behaviour from IMGui.
    /// </summary>
    /// <param name="boidBehaviour"></param>
    public static void UpdateBoidBehaviourFromUI(FinchBoidBehaviour boidBehaviour)
    {
        FinchBoidBehaviour = boidBehaviour;
    }

    public void Update(double _deltaTime)
    {
        Runtime += _deltaTime;

        TryAddFrame(_deltaTime);

        if (_frames.Count > 0)
            FPS = (int)(1.0/_frames.Average());

        foreach (Finch finch in FinchDict.Values.SelectMany(idPair => idPair.Values))
        {
            finch.Update(_deltaTime);
        }
    }
}