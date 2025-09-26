using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Events;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes.Fixes;

public class FullAllTalkFix: BaseFix
{
    public FullAllTalkFix()
    {
        Name = "FullAllTalkFix";
        ConfigurationProperty = "EnforceFullAlltalk";
        Events = new Dictionary<string, CSSharpFixes.GameEventHandler>
        {
            { "OnRoundStart", OnRoundStart },
        };
    }
    
    public HookResult OnRoundStart(GameEvent @event, GameEventInfo info, ILogger<CSSharpFixes> logger)
    {
        Server.ExecuteCommand("sv_full_alltalk 1");
        logger.LogInformation("[CSSharpFixes][Fix][FullAllTalkFix][OnRoundStart()][sv_full_alltalk set to 1]");
        return HookResult.Continue;
    }
}