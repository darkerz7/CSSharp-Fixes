using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Events;
using CSSharpFixes.Extensions;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes.Fixes;

public class NoBlockFix: BaseFix
{
    public NoBlockFix()
    {
        Name = "NoBlockFix";
        ConfigurationProperty = "EnableNoBlock";
        Events = new Dictionary<string, CSSharpFixes.GameEventHandler>
         {
             { "OnPlayerSpawn", OnPlayerSpawn },
         };
    }

	private static void ApplyNoBlock(CCSPlayerController? player)
    {
        if(!player.IsCompletelyValid(out var playerPawn)) return;
        CollisionGroup collisionGroup = (CollisionGroup)playerPawn.Collision.CollisionAttribute.CollisionGroup;
        if(collisionGroup == CollisionGroup.COLLISION_GROUP_DEBRIS) return;
        playerPawn.SetCollisionGroup(CollisionGroup.COLLISION_GROUP_DEBRIS);
    }

    public HookResult OnPlayerSpawn(GameEvent @event, GameEventInfo info, ILogger<CSSharpFixes> logger)
    {
        if (!Enabled) return HookResult.Continue;

        if (@event is not EventPlayerSpawn playerSpawnEvent || playerSpawnEvent.Userid == null) return HookResult.Continue;
        // logger.LogInformation("[CSSharpFixes][Fix][NoBlockFix][OnPlayerSpawn()]");
        CCSPlayerController player = new(playerSpawnEvent.Userid.Handle);

        Server.NextFrame(() =>
        {
            ApplyNoBlock(player);
        });
        return HookResult.Continue;
    }
}