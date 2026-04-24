using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.UserMessages;

namespace CSSharpFixes.Fixes;

public class ParticleManagerMsgFix : BaseFix
{
    public ParticleManagerMsgFix()
    {
        Name = "ParticleManagerMsgFix";
        ConfigurationProperty = "EnableParticleManagerMsgFix";
	}

	public HookResult OnParticleManagerMessage(UserMessage um)
	{
        um.Recipients.Clear();
        return HookResult.Changed;
    }
}