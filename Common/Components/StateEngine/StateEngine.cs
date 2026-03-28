using Godot;

namespace Game.Components;

[Tool, GlobalClass, Icon("./StateEngine.svg")]
public partial class StateEngine : Node
{
    private State rootState;
}
