using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using Godot.Extensions;

namespace Game.Components;

[Tool, GlobalClass, Icon("AtomicState.svg")]
public partial class AtomicState : State
{
    public override void _Ready()
    {
        base._Ready();

		Debug.Assert(
			this.GetChildCount<State>() == 0,
			$"{typeof(AtomicState).Name} cannot have children substates."
		);
    }

	public override string[] _GetConfigurationWarnings()
	{
		List<string> warnings = [];
		warnings.AddRange(base._GetConfigurationWarnings());

		if (this.GetChildCount<State>() == 0)
		{
			warnings.Add(
				$"{typeof(AtomicState).Name} cannot have children substates."
			);
		}

		return warnings.ToArray();
	}
}
