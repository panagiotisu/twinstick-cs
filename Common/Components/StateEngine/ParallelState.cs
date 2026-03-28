using System.Diagnostics;
using System.Collections.Generic;
using Godot;
using Godot.Extensions;

namespace Game.Components;

[Tool, GlobalClass, Icon("ParallelState.svg")]
public partial class ParallelState : State
{
    private readonly HashSet<State> _subStates = [];

    public override void _Ready()
    {
        base._Ready();

        Debug.Assert(
			this.GetChildCount<State>() > 0,
			$"{typeof(ParallelState).Name} must have at least one substate child."
		);

        foreach (var subState in this.GetChildren<State>())
        {
            _subStates.Add(subState);
        }
    }

    public override string[] _GetConfigurationWarnings()
	{
		List<string> warnings = [];
		warnings.AddRange(base._GetConfigurationWarnings());

		if (this.GetChildCount<State>() == 0)
		{
			warnings.Add(
				$"{typeof(ParallelState).Name} must have at least one substate child " +
                "in order to function."
			);
		}

        if (this.GetChildCount<State>() == 1)
		{
			warnings.Add(
				$"{typeof(ParallelState).Name} should have at least two substate children " +
                "to run in parallel."
			);
		}

		return warnings.ToArray();
	}

    public override void OnEnter()
    {
        foreach (var substate in _subStates)
        {
            substate.OnEnter();
        }
    }

    public override void OnInput(InputEvent @event)
    {
        foreach (var substate in _subStates)
        {
            substate.OnInput(@event);
        }
    }

    public override void FrameUpdate(double delta)
    {
        foreach (var substate in _subStates)
        {
            substate.FrameUpdate(delta);
        }
    }

    public override void FixedUpdate(double delta)
    {
        foreach (var substate in _subStates)
        {
            substate.FixedUpdate(delta);
        }
    }

    public override void OnExit()
    {
        foreach (var substate in _subStates)
        {
            substate.OnExit();
        }
    }
}