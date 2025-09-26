using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.UserMessages;
using CSSharpFixes.Fixes;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes.Managers;

public class FixManager(PatchManager patchManager, DetourManager detourManager, EventManager eventManager,
    ILogger<CSSharpFixes> logger)
{
    private readonly List<BaseFix> _fixes = [];

    public void OnConfigChanged(string propertyName, object? newValue)
    {
        int index = _fixes.FindIndex(fix => fix.ConfigurationProperty == propertyName);
        if (newValue == null)
        {
            StopFix(index);
            return;
        }

        if(newValue is not bool value) return;
        
        if(value) StartFix(index);
        else StopFix(index);
    }

    private void StartFix(int index)
    {
        if(index < 0 || index >= _fixes.Count) return;

        foreach(string patchName in _fixes[index].PatchNames) patchManager.PerformPatch(patchName);
        foreach(string detourHandlerName in _fixes[index].DetourHandlerNames) detourManager.StartHandler(detourHandlerName);
        foreach(var eventPair in _fixes[index].Events) eventManager.RegisterEvent(eventPair.Key, eventPair.Value);
        
        _fixes[index].Enabled = true;
    }

    private void StopFix(int index)
    {
        if(index < 0 || index >= _fixes.Count) return;
        
        foreach(string patchName in _fixes[index].PatchNames) patchManager.UndoPatch(patchName);
        foreach(string detourHandlerName in _fixes[index].DetourHandlerNames) detourManager.StopHandler(detourHandlerName);
        foreach(var eventPair in _fixes[index].Events) eventManager.UnregisterEvent(eventPair.Key, eventPair.Value);
        
        _fixes[index].Enabled = false;
    }

	public HookResult OnChatMessage(UserMessage um)
    {
		foreach (BaseFix fix in _fixes)
		{
			if (fix is TeamMessagesFix teammessagefix) return teammessagefix.OnChatMessage(um);
		}
		return HookResult.Continue;
	}

	public void Listener_OnEntityCreated(CEntityInstance entity)
	{
		foreach (BaseFix fix in _fixes)
		{
            if (fix is HammerIDFix hammeridfix) hammeridfix.Listener_OnEntityCreated(entity, logger);
		}
	}

	public HookResult Listener_RadioCommands(CCSPlayerController? player, CommandInfo info)
	{
		foreach (BaseFix fix in _fixes)
		{
			if (fix is TeamMessagesFix teammessagefix) return teammessagefix.Listener_RadioCommands(player, info);
		}
		return HookResult.Continue;
	}

	public HookResult Listener_Chatwheel(CCSPlayerController? player, CommandInfo info)
	{
		foreach (BaseFix fix in _fixes)
		{
			if (fix is TeamMessagesFix teammessagefix) return teammessagefix.Listener_Chatwheel(player, info);
		}
		return HookResult.Continue;
	}

	public void Start()
    {
        logger.LogInformation("[CSSharpFixes][FixManager][Start()]");
        
        _fixes.Add(new WaterFix());
        _fixes.Add(new TriggerPushFix());
        _fixes.Add(new CPhysBoxUseFix());
        _fixes.Add(new NavmeshLookupLagFix());
        _fixes.Add(new NoBlockFix());
        _fixes.Add(new TeamMessagesFix());
        _fixes.Add(new SubTickMovementFix());
        _fixes.Add(new MovementUnlockerFix());
        _fixes.Add(new FullAllTalkFix());
		_fixes.Add(new HammerIDFix());
		_fixes.Add(new EmitSoundVolumeFix());
	}
    
    public void Stop()
    {
        logger.LogInformation("[CSSharpFixes][FixManager][Stop()]");
        
        foreach (BaseFix fix in _fixes)
        {
            foreach(string patchName in fix.PatchNames) patchManager.UndoPatch(patchName);
            foreach(string detourHandlerName in fix.DetourHandlerNames) detourManager.StopHandler(detourHandlerName);
        }
        
        _fixes.Clear();
    }
}