using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.UserMessages;
using CounterStrikeSharp.API.Modules.Utils;
using CSSharpFixes.Fixes;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes;

public partial class CSSharpFixes
{
    public delegate HookResult GameEventHandler(GameEvent gameEvent, GameEventInfo gameEventInfo, ILogger<CSSharpFixes> logger);
    
    private void RegisterHooks()
    {
        RegisterListener<Listeners.OnServerPrecacheResources>(OnServerPrecacheResources);
		RegisterListener<Listeners.OnEntityCreated>(OnEntityCreated);

		HookUserMessage(124, OnChatMessage, HookMode.Pre);
		for (int i = 0; i < TeamMessagesFix.RadioArray.Length; i++)
		{
			AddCommandListener(TeamMessagesFix.RadioArray[i], Listener_RadioCommands, HookMode.Pre);
		}
		AddCommandListener("playerchatwheel", Listener_Chatwheel, HookMode.Pre);
	}

    private void UnregisterHooks()
    {
        RemoveListener<Listeners.OnServerPrecacheResources>(OnServerPrecacheResources);
		RemoveListener<Listeners.OnEntityCreated>(OnEntityCreated);

		UnhookUserMessage(124, OnChatMessage, HookMode.Pre);
		for (int i = 0; i < TeamMessagesFix.RadioArray.Length; i++)
		{
			RemoveCommandListener(TeamMessagesFix.RadioArray[i], Listener_RadioCommands, HookMode.Pre);
		}
		RemoveCommandListener("playerchatwheel", Listener_Chatwheel, HookMode.Pre);
	}

    private void OnServerPrecacheResources(ResourceManifest manifest)
    {
        //manifest.AddResource("models/food/pizza/pizza_1.vmdl");
    }

    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info) => 
        _eventManager.OnRoundStart(@event, info);
    
    [GameEventHandler]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info) => 
        _eventManager.OnPlayerSpawn(@event, info);
    
    [GameEventHandler]
    public HookResult OnPlayerTeam(EventPlayerTeam @event, GameEventInfo info) => 
        _eventManager.OnPlayerTeam(@event, info);
    
	private void OnEntityCreated(CEntityInstance entity) => _fixManager.Listener_OnEntityCreated(entity);

	private HookResult OnChatMessage(UserMessage um) => _fixManager.OnChatMessage(um);

	private HookResult Listener_RadioCommands(CCSPlayerController? player, CommandInfo info) => _fixManager.Listener_RadioCommands(player, info);

	private HookResult Listener_Chatwheel(CCSPlayerController? player, CommandInfo info) => _fixManager.Listener_Chatwheel(player, info);
}