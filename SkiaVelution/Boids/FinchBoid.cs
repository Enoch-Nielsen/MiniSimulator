using Silk.NET.Maths;
using Silk.NET.SDL;
using SkiaSharp;
using SkiaVelution.Entities;
using SkiaVelution.Objects;

namespace SkiaVelution.Boids;

public class FinchBoid : VisualEntity
{
    private SKColor _color;
    protected Vector2D<float> Velocity;
    private FinchBoidBehaviour _finchBoidBehaviour;
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
    
    public FinchBoid(int group, int id, Vector2D<float> position, float scale, float rotation, SKColor color) : base(position, scale, rotation)
    {
        tile = Model.ConvertToTilePosition(position);
        Group = group;   
        ID = id;
        _color = color;
        Velocity = Vector2D<float>.Zero;
        
        _finchBoidBehaviour = Model.FinchBoidBehaviour;
    }

    public override void Update(double deltaTime)
    {
        _finchBoidBehaviour = Model.FinchBoidBehaviour;
        
        // Fix Position BS
        if (Position.X < 0 || Position.X > Graphics.WIDTH || Position.Y < 0 || Position.Y > Graphics.HEIGHT)
        {
            Position.X = Graphics.WIDTH / 2;
            Position.Y = Graphics.HEIGHT / 2;
        }
        
        // Separation
        Vector2D<float> close = Vector2D<float>.Zero;
        Vector2D<float> groupClose = Vector2D<float>.Zero;

        // Alignment
        Vector2D<float> velocityAverage = Vector2D<float>.Zero;
        
        // Cohesion
        Vector2D<float> positionAverage = Vector2D<float>.Zero;

        int closeBoidCount = 0;
        
        Vector2D<int> newTile = Model.ConvertToTilePosition(Position);
        
        if (newTile != tile)
        {
            Model.Instance.MoveBoidToTile(newTile, tile, ID);
            tile = newTile;
        }

        
        foreach (Vector2D<int> aTile in GetTiles())
            foreach (Finch other in Model.FinchDict[aTile].Values)
            {
                FinchBoid otherBoid = other.Boid;
                
                if (otherBoid.ID == ID) continue;
                
                // Calculate Separation.
                if (otherBoid.Group == Group)
                {
                    if (Dist(this, otherBoid) >= _finchBoidBehaviour.GroupVisionRange) continue;
                    
                    groupClose.X += Position.X - otherBoid.Position.X;
                    groupClose.Y += Position.Y - otherBoid.Position.Y;
                    
                    // Calculate alignment.
                    velocityAverage.X += otherBoid.Velocity.X;
                    velocityAverage.Y += otherBoid.Velocity.Y;
                
                    // Calculate Cohesion.
                    positionAverage.X += otherBoid.Position.X;
                    positionAverage.Y += otherBoid.Position.Y;

                    closeBoidCount++;
                }
                else
                {
                    if (Dist(this, otherBoid) >= _finchBoidBehaviour.VisionRange) continue;

                    close.X += Position.X - otherBoid.Position.X;
                    close.Y += Position.Y - otherBoid.Position.Y;
                }
            }

        // Average out Alignment and Cohesion.
        if (closeBoidCount > 0)
        {
            velocityAverage /= closeBoidCount;
            positionAverage /= closeBoidCount;
            
            // Adjust Velocity for alignment.
            Velocity.X += (velocityAverage.X - Velocity.X) * _finchBoidBehaviour.MatchingFactor;
            Velocity.Y += (velocityAverage.Y - Velocity.Y) * _finchBoidBehaviour.MatchingFactor;
            
            // Adjust Velocity for Cohesion
            Velocity.X += (positionAverage.X - Position.X) * _finchBoidBehaviour.CenteringFactor;
            Velocity.Y += (positionAverage.Y - Position.Y) * _finchBoidBehaviour.CenteringFactor;
        }
        
        // Adjust Velocity for Separation.
        Velocity.X += close.X * _finchBoidBehaviour.AvoidanceFactor;
        Velocity.Y += close.Y * _finchBoidBehaviour.AvoidanceFactor;
        
        Velocity.X += groupClose.X * _finchBoidBehaviour.GroupAvoidanceFactor;
        Velocity.Y += groupClose.Y * _finchBoidBehaviour.GroupAvoidanceFactor;
        
        // Handle Screen Edges using turn factor.
        
        if (Position.X < _finchBoidBehaviour.XMargin)
        {
            Velocity.X += _finchBoidBehaviour.TurnFactor;
        }
        else if (Position.X > Graphics.WIDTH - _finchBoidBehaviour.XMargin)
        {
            Velocity.X -= _finchBoidBehaviour.TurnFactor;
        }
        else if (Position.Y < _finchBoidBehaviour.YMargin)
        {
            Velocity.Y += _finchBoidBehaviour.TurnFactor;
        }
        else if (Position.Y > Graphics.HEIGHT - _finchBoidBehaviour.XMargin)
        {
            Velocity.Y -= _finchBoidBehaviour.TurnFactor;
        }
        
        
        // Tweaks and final positioning.
        float velocityMagnitude = Velocity.Length;

        if (velocityMagnitude != 0)
        {
            if (velocityMagnitude > _finchBoidBehaviour.MaximumSpeed)
            {
                Velocity.X = (Velocity.X / velocityMagnitude) * _finchBoidBehaviour.MaximumSpeed;
                Velocity.Y = (Velocity.Y / velocityMagnitude) * _finchBoidBehaviour.MaximumSpeed;
            }
        
            if (velocityMagnitude < _finchBoidBehaviour.MinimumSpeed)
            {
                Velocity.X = (Velocity.X / velocityMagnitude) * _finchBoidBehaviour.MinimumSpeed;
                Velocity.Y = (Velocity.Y / velocityMagnitude) * _finchBoidBehaviour.MinimumSpeed;
            }
        }
        
        Position.X += (float)(Velocity.X * (deltaTime * Model.RUN_SPEED));
        Position.Y += (float)(Velocity.Y * (deltaTime * Model.RUN_SPEED));
    }

    private Vector2D<int>[] GetTiles()
    {
        return vectors.Select(v => v + tile).Where(Model.CheckTileValid).ToArray();
    }

    public override void Draw(SKCanvas canvas, SKPaint skPaint)
    {
        canvas.Save();
        
        skPaint.Color = _color;
        skPaint.Style = SKPaintStyle.StrokeAndFill;

        SKPath triPath = new() { FillType = SKPathFillType.EvenOdd };
        triPath.MoveTo(-Scale + Position.X, 0f + Position.Y);
        triPath.LineTo(0 + Position.X, (2f*Scale) + Position.Y);
        triPath.LineTo(Scale + Position.X, 0f + Position.Y);
        triPath.LineTo(-Scale + Position.X, 0f + Position.Y);
        
        Rotation = MathF.Atan2(-Velocity.Y, -Velocity.X) + (float.Pi / 2f);

        if (Rotation < 0f)
            Rotation += (float.Pi * 2f);

        SKMatrix rotate = SKMatrix.CreateRotation(Rotation, Position.X, Position.Y);
        triPath.Transform(rotate);
        
        // canvas.RotateRadians(Rotation, Position.X, Position.Y);
        canvas.DrawPath(triPath, skPaint);
        // canvas.Restore();
    }
}