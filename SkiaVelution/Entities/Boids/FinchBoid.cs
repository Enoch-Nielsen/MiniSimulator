using Silk.NET.Maths;
using SkiaSharp;
using SkiaVelution.Lib;
using SkiaVelution.Objects;
using SkiaVelution.Settings;

namespace SkiaVelution.Entities.Boids;

public class FinchBoid : VisualEntity
{
    private SKColor _color;
    protected Vector2D<float> Velocity;
    public int ID { get; private set; }
    public int Group { get; private set; }
    private Vector2D<int> tile = new();
    
    private static readonly Vector2D<int>[] vectors = 
    [
        new Vector2D<int>(0, 0),
        new Vector2D<int>(0, 1),
        new Vector2D<int>(0, -1),
        new Vector2D<int>(-1, 0),
        new Vector2D<int>(-1, 1),
        new Vector2D<int>(-1, -1),
        new Vector2D<int>(1, -1),
        new Vector2D<int>(1, 0),
        new Vector2D<int>(1, 1),
    ];
    
    public FinchBoid(int group, int id, Transform transform, SKColor color) : base(transform)
    {
        tile = FinchManager.ConvertToTilePosition(transform.Position);
        Group = group;   
        ID = id;
        _color = color;
        Velocity = Vector2D<float>.Zero;
    }

    public override void Update(double deltaTime)
    {
        if (BoidSettings.Instance == null) return;
        
        // Fix Position BS
        if (Transform.Position.X < 0 || Transform.Position.X > WindowManager.WIDTH || Transform.Position.Y < 0 || Transform.Position.Y > WindowManager.HEIGHT)
        {
            Transform.Position.X = WindowManager.WIDTH / 2;
            Transform.Position.Y = WindowManager.HEIGHT / 2;
        }
        
        // Separation
        Vector2D<float> close = Vector2D<float>.Zero;
        Vector2D<float> groupClose = Vector2D<float>.Zero;

        // Alignment
        Vector2D<float> velocityAverage = Vector2D<float>.Zero;
        
        // Cohesion
        Vector2D<float> positionAverage = Vector2D<float>.Zero;

        int closeBoidCount = 0;
        
        Vector2D<int> newTile = FinchManager.ConvertToTilePosition(Transform.Position);
        
        if (newTile != tile)
        {
            FinchManager.Instance.MoveBoidToTile(newTile, tile, ID);
            tile = newTile;
        }

        
        foreach (Vector2D<int> aTile in GetTiles())
            foreach (Finch other in FinchManager.FinchDict[aTile].Values)
            {
                FinchBoid otherBoid = other.Boid;
                
                if (otherBoid.ID == ID) continue;
                
                // Calculate Separation.
                if (otherBoid.Group == Group)
                {
                    if (Dist(this, otherBoid) >= BoidSettings.GroupVisionRange) continue;
                    
                    groupClose.X += Transform.Position.X - otherBoid.Transform.Position.X;
                    groupClose.Y += Transform.Position.Y - otherBoid.Transform.Position.Y;
                    
                    // Calculate alignment.
                    velocityAverage.X += otherBoid.Velocity.X;
                    velocityAverage.Y += otherBoid.Velocity.Y;
                
                    // Calculate Cohesion.
                    positionAverage.X += otherBoid.Transform.Position.X;
                    positionAverage.Y += otherBoid.Transform.Position.Y;

                    closeBoidCount++;
                }
                else
                {
                    if (Dist(this, otherBoid) >= BoidSettings.VisionRange) continue;

                    close.X += Transform.Position.X - otherBoid.Transform.Position.X;
                    close.Y += Transform.Position.Y - otherBoid.Transform.Position.Y;
                }
            }

        // Average out Alignment and Cohesion.
        if (closeBoidCount > 0)
        {
            velocityAverage /= closeBoidCount;
            positionAverage /= closeBoidCount;
            
            // Adjust Velocity for alignment.
            Velocity.X += (velocityAverage.X - Velocity.X) * BoidSettings.MatchingFactor;
            Velocity.Y += (velocityAverage.Y - Velocity.Y) * BoidSettings.MatchingFactor;
            
            // Adjust Velocity for Cohesion
            Velocity.X += (positionAverage.X - Transform.Position.X) * BoidSettings.CenteringFactor;
            Velocity.Y += (positionAverage.Y - Transform.Position.Y) * BoidSettings.CenteringFactor;
        }
        
        // Adjust Velocity for Separation.
        Velocity.X += close.X * BoidSettings.AvoidanceFactor;
        Velocity.Y += close.Y * BoidSettings.AvoidanceFactor;
        
        Velocity.X += groupClose.X * BoidSettings.GroupAvoidanceFactor;
        Velocity.Y += groupClose.Y * BoidSettings.GroupAvoidanceFactor;
        
        // Handle Screen Edges using turn factor.
        
        if (Transform.Position.X < BoidSettings.XMargin)
        {
            Velocity.X += BoidSettings.TurnFactor;
        }
        else if (Transform.Position.X > WindowManager.WIDTH - BoidSettings.XMargin)
        {
            Velocity.X -= BoidSettings.TurnFactor;
        }
        else if (Transform.Position.Y < BoidSettings.YMargin)
        {
            Velocity.Y += BoidSettings.TurnFactor;
        }
        else if (Transform.Position.Y > WindowManager.HEIGHT - BoidSettings.XMargin)
        {
            Velocity.Y -= BoidSettings.TurnFactor;
        }
        
        
        // Tweaks and final positioning.
        float velocityMagnitude = Velocity.Length;

        if (velocityMagnitude != 0)
        {
            if (velocityMagnitude > BoidSettings.MaximumSpeed)
            {
                Velocity.X = (Velocity.X / velocityMagnitude) * BoidSettings.MaximumSpeed;
                Velocity.Y = (Velocity.Y / velocityMagnitude) * BoidSettings.MaximumSpeed;
            }
        
            if (velocityMagnitude < BoidSettings.MinimumSpeed)
            {
                Velocity.X = (Velocity.X / velocityMagnitude) * BoidSettings.MinimumSpeed;
                Velocity.Y = (Velocity.Y / velocityMagnitude) * BoidSettings.MinimumSpeed;
            }
        }
        
        Transform.Position.X += (float)(Velocity.X * (deltaTime * BoidSettings.FinchSpeed));
        Transform.Position.Y += (float)(Velocity.Y * (deltaTime * BoidSettings.FinchSpeed));
    }

    private Vector2D<int>[] GetTiles()
    {
        return vectors.Select(v => v + tile).Where(FinchManager.CheckTileValid).ToArray();
    }

    protected override void Draw(SKCanvas canvas, SKPaint skPaint)
    {
        canvas.Save();
        
        skPaint.Color = _color;
        skPaint.Style = SKPaintStyle.StrokeAndFill;

        SKPath triPath = new() { FillType = SKPathFillType.EvenOdd };
        triPath.MoveTo(-Transform.Scale + Transform.Position.X, 0f + Transform.Position.Y);
        triPath.LineTo(0 + Transform.Position.X, (2f*Transform.Scale) + Transform.Position.Y);
        triPath.LineTo(Transform.Scale + Transform.Position.X, 0f + Transform.Position.Y);
        triPath.LineTo(-Transform.Scale + Transform.Position.X, 0f + Transform.Position.Y);
        
        Transform.Rotation = MathF.Atan2(-Velocity.Y, -Velocity.X) + (float.Pi / 2f);

        if (Transform.Rotation < 0f)
            Transform.Rotation += (float.Pi * 2f);

        SKMatrix rotate = SKMatrix.CreateRotation(Transform.Rotation, Transform.Position.X, Transform.Position.Y);
        triPath.Transform(rotate);
        
        // canvas.RotateRadians(Rotation, Position.X, Position.Y);
        canvas.DrawPath(triPath, skPaint);
        // canvas.Restore();
    }
}