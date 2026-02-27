using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using Game.Globals;

namespace Game.Components;

[GlobalClass, Icon("animation_director.svg")]
public partial class AnimationDirector : Node
{
    private enum Cardinal
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        // Rest are mirrored.
    }

    private enum FacingPattern
    {
        Linear,
        Square,
        Rhombus,
        Hexagon,
        Octagon,
    }

    [ExportGroup(GlobalStrings.ExportGroupNames.Properties)]
    [Export] private FacingPattern _facingPattern = FacingPattern.Linear;

    [ExportGroup(GlobalStrings.ExportGroupNames.Components)]
    [Export] private AnimationPlayer _animationPlayer;

    [Export] private Node2D _visuals;

    private const string Delimiter = ".";

    private Dictionary<(StringName, Cardinal), StringName> _directionalAnimationMap = [];
    private readonly Dictionary<StringName, Cardinal> _stringToCardinal = new()
    {
        { new StringName("up"),        Cardinal.Up        },
        { new StringName("upright"),   Cardinal.UpRight   },
        { new StringName("right"),     Cardinal.Right     },
        { new StringName("downright"), Cardinal.DownRight },
        { new StringName("down"),      Cardinal.Down      }
    };

    public float SpeedScale
    {
        get => _animationPlayer.SpeedScale;
        set => _animationPlayer.SpeedScale = value;
    }

    public void Play(StringName animation)
    {
        Debug.Assert(
            !animation.ToString().Contains(Delimiter),
            "Directionless animation names should never contain the special delimiter '" + Delimiter + "'."
        );

        _animationPlayer.Play(animation);
    }

    public void Play(StringName animationState, Vector2 faceDirection)
    {
        HandleFlipping(faceDirection);
    }

    private void PopulateDirectionalAnimationMap()
    {
        foreach (var rawAnimationName in _animationPlayer.GetAnimationList())
        {
            string[] parts = rawAnimationName.Split(Delimiter);
            
            Debug.Assert(
                parts.Length == 2,
                $"There should be exactly one {Delimiter} delimiter in " + 
                $"animation name {rawAnimationName} of {Owner.Name}."
            );
            
            StringName animationStateName = new(parts[0]);
            StringName cardinalSuffix = new(parts[1]);
            
            Debug.Assert(
                _stringToCardinal.ContainsKey(cardinalSuffix),
                $"Unknown cardinal direction suffix {cardinalSuffix} in " +
                $"animation name {rawAnimationName} of {Owner.Name}."
            );

            Cardinal cardinal = _stringToCardinal[cardinalSuffix];

            _directionalAnimationMap[(animationStateName, cardinal)] = new StringName(rawAnimationName);
        }
    }

    private void HandleFlipping(Vector2 faceDirection)
    {
        _visuals.Scale = new Vector2(Mathf.Sign(faceDirection.X), _visuals.Scale.Y);
    }

    private Cardinal CardinalFromVector(Vector2 faceDirection)
    {
        float angleDegrees = Mathf.RadToDeg(faceDirection.Angle());
        float angleDegreesWrapped = (angleDegrees + 360f) % 360f;
        
        switch (_facingPattern)
        {
            default:
            case FacingPattern.Linear: 
                return Cardinal.Right;
            case FacingPattern.Square:
                return CardinalFromSquarePattern(angleDegreesWrapped);
            case FacingPattern.Rhombus:
                return CardinalFromRhombusPattern(angleDegreesWrapped);
            case FacingPattern.Hexagon:
                return CardinalFromHexagonPattern(angleDegreesWrapped);
            case FacingPattern.Octagon:
                return CardinalFromOctagonPattern(angleDegreesWrapped);
        }
    }

    private static Cardinal CardinalFromSquarePattern(float angleDegreesWrapped)
    {
        return angleDegreesWrapped switch
        {
            >= 0f   and < 180f => Cardinal.DownRight,
            >= 180f and < 360f => Cardinal.UpRight,
            _ => Cardinal.DownRight
        };
    }
    
    private static Cardinal CardinalFromRhombusPattern(float angleDegreesWrapped)
    {
        return angleDegreesWrapped switch
        {
            >= 315f or  < 45f  => Cardinal.Right,
            >= 45f  and < 135f => Cardinal.Down,
            >= 135f and < 225f => Cardinal.Right,
            >= 225f and < 315f => Cardinal.Up,
            _ => Cardinal.Right,
        };
    }
    
    private static Cardinal CardinalFromHexagonPattern(float angleDegreesWrapped)
    {
        return angleDegreesWrapped switch
        {
            >= 0f   and < 60f  => Cardinal.DownRight,
            >= 60f  and < 120f => Cardinal.Down,
            >= 120f and < 180f => Cardinal.DownRight,
            >= 180f and < 240f => Cardinal.UpRight,
            >= 240f and < 300f => Cardinal.Up,
            >= 300f and < 360f => Cardinal.UpRight,
            _ => Cardinal.DownRight
        };
    }
    
    private static Cardinal CardinalFromOctagonPattern(float angleDegreesWrapped)
    {
        return angleDegreesWrapped switch
        {
            >= 337.5f or  < 22.5f   => Cardinal.Right,
            >= 22.5f  and < 67.5f   => Cardinal.DownRight,
            >= 67.5f  and < 112.5f  => Cardinal.Down,
            >= 112.5f and < 157.5f  => Cardinal.DownRight,
            >= 157.5f and < 202.5f  => Cardinal.Right,
            >= 202.5f and < 247.5f  => Cardinal.UpRight,
            >= 247.5f and < 292.5f  => Cardinal.Up,
            >= 292.5f and < 337.5f  => Cardinal.UpRight,
            _ => Cardinal.Right
        };
    }
}