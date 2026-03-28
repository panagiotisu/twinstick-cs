using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using Godot.Extensions;

namespace Game.Components;

[Tool, GlobalClass, Icon("StateEngine.svg")]
public partial class StateEngine : Node
{
    private State _rootState;

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;

        Debug.Assert(
            this.GetChildCount<State>() == 1,
            $"{typeof(StateEngine).Name} should have exactly one child of type {typeof(State).Name}"
        );

        _rootState = GetChild<State>(0);
        _rootState.OnEnter();
    }

    public override string[] _GetConfigurationWarnings()
	{
		List<string> warnings = [];

		if (this.GetChildCount<State>() != 1)
		{
			warnings.Add(
				$"{typeof(StateEngine).Name} should have exactly one child of type {typeof(State).Name}"
			);
		}

		return warnings.ToArray();
	}

    public override void _Input(InputEvent @event)
    {
        _rootState.OnInput(@event);
    }

    public override void _Process(double delta)
    {
        _rootState.FrameUpdate(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _rootState.FixedUpdate(delta);
    }
}
