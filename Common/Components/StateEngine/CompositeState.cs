using System.Diagnostics;
using System.Collections.Generic;
using Godot;
using Godot.Extensions;
using System;

namespace Game.Components;

[Tool, GlobalClass, Icon("CompositeState.svg")]
public partial class CompositeState : State
{
    [Export] private State _initialSubState;
    [Export] private bool _debugMode;

    private State _currentSubState;
    private readonly HashSet<State> _subStates = [];

    public override void _Ready()
    {
        base._Ready();

        Debug.Assert(
			this.GetChildCount<State>() > 0,
			$"{typeof(CompositeState).Name} must have at least one substate child."
		);

        foreach (var subState in this.GetChildren<State>())
        {
            _subStates.Add(subState);
            subState.Transitioned += ChangeState;
        }

        _initialSubState ??= GetChild<State>(0);  // If no initial substate given, assign the 1st substate child.

        Debug.Assert(
            _subStates.Contains(_initialSubState),
            $"Selected initial substate {_initialSubState.Name} must be a substate of " + 
            $"{typeof(CompositeState)} {Name}."
        );
    }

    public override void OnEnter()
    {
        _initialSubState.OnEnter();
        _currentSubState = _initialSubState;
    }

    public override void OnInput(InputEvent @event)
    {
        _currentSubState.OnInput(@event);
    }

    public override void FrameUpdate(double delta)
    {
        _currentSubState.FrameUpdate(delta);
    }

    public override void FixedUpdate(double delta)
    {
        _currentSubState.FixedUpdate(delta);

        if (_debugMode)
        {
            GD.Print($"Current SubState: {_currentSubState.Name}");
        }
    }

    public override void OnExit()
    {
        _currentSubState.OnExit();
        _currentSubState = null;
    }

    public override void _ExitTree()
    {
        foreach (var substate in _subStates)
        {
            substate.Transitioned -= ChangeState;
        }
    }

    private void ChangeState(State fromSubState, State toSubState)
    {
        Debug.Assert(
            fromSubState == _currentSubState,
            $"Invalid substate transition in {Owner.Name}, trying from {fromSubState.Name} state but " + 
            $"currently in {_currentSubState.Name} state."
        );

        Debug.Assert(
            _subStates.Contains(toSubState),
            $"Target substate {toSubState.Name} in {Owner.Name} does not exist."
        );

        if (!toSubState.CanEnter()) return;

        _currentSubState.OnExit();
        toSubState.OnEnter();
        _currentSubState = toSubState;
    }
}