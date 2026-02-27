using Godot;

namespace Game.Globals;

public static class GlobalStrings
{
    /*
    StringNames (and strings) that can be used globally. Useful for avoiding heap allocations and triggering GC due
    to Godot's conversion of C# Strings to StringNames under the hood.
    */

    // Input StringNames
    public static class Input
    {
        public static readonly StringName MoveUp = new("move_up");
        public static readonly StringName MoveDown = new("move_down");
        public static readonly StringName MoveLeft = new("move_left");
        public static readonly StringName MoveRight = new("move_right");
        public static readonly StringName Attack = new("attack");
        public static readonly StringName Reload = new("reload");
        public static readonly StringName Dash = new("dash");
        public static readonly StringName GamePause = new("game_pause");
        public static readonly StringName SwitchToPreviousWeapon = new("switch_to_previous_weapon");
        public static readonly StringName SwitchToNextWeapon = new("switch_to_next_weapon");
    }
    
    // Export Group Strings
    public static class ExportGroupNames
    {
        public const string Components = "Components";
        public const string Properties = "Properties";
        public const string NeighboringStates = "Neighboring States";
    }
}