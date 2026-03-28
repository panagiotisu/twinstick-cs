using System.Diagnostics;
using Godot;

namespace Game.Actors;

[GlobalClass, Icon("Player2D.svg")]
public partial class Player2D : CharacterBody2D
{
    public static Player2D Instance { get; private set; }

    public override void _Ready()
    {
        Debug.Assert(
            Instance == null || Instance == this,
            $"There should be at most one {typeof(Player2D).Name} " +
            "object in the game at all times."
        );

        Instance = this;
    }
}