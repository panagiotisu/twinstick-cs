using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace Game.Components;

[Tool, GlobalClass]
public abstract partial class State : Node
{
    public event Action<State, State> Transitioned;

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;

		Debug.Assert(
			IsParentValid(),
			$"{typeof(State).Name} must have a parent of type " +
			$"{typeof(State).Name} or {typeof(StateEngine).Name}."
		);
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];
        
        if (!IsParentValid())
        {
            warnings.Add(
                $"{typeof(State).Name} must have a parent of type " +
                $"{typeof(State).Name} or {typeof(StateEngine).Name}."
            );
        }
        return warnings.ToArray();
    }

    public virtual bool CanEnter() { return true; }

    public virtual void OnEnter() {}

    public virtual void OnInput(InputEvent @event) {}

    public virtual void FrameUpdate(double delta) {}

    public virtual void FixedUpdate(double delta) {}
    
    public virtual void OnExit() {}

    protected void TriggerTransitionTo(State toState)
    {
        Transitioned?.Invoke(this, toState);
    }

	private bool IsParentValid()
	{
		Node parent = GetParent();

		if (parent == null) return false;

		return parent is State || parent is StateEngine;
	}
}