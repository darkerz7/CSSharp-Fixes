using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Events;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes.Managers;

public class EventManager(ILogger<CSSharpFixes> logger)
{
    private readonly ILogger<CSSharpFixes> _logger = logger;
    
    private readonly Dictionary<string, List<CSSharpFixes.GameEventHandler>> _events = [];

	public void Start()
    {
        _events.Add("OnRoundStart", []);
        _events.Add("OnPlayerSpawn", []);
        _events.Add("OnPlayerTeam", []);
    }
    
    public void Stop()
    {
        foreach (var eventPair in _events)
        {
            eventPair.Value.Clear();
        }
        _events.Clear();
    }
    
    public void RegisterEvent(string name, CSSharpFixes.GameEventHandler gameEventHandler)
    {
        if(_events.TryGetValue(name, out List<CSSharpFixes.GameEventHandler>? handlers))
        {
            handlers.Add(gameEventHandler);
        }
    }
    
    public void UnregisterEvent(string name, CSSharpFixes.GameEventHandler gameEventHandler)
    {
        if(_events.TryGetValue(name, out List<CSSharpFixes.GameEventHandler>? handlers))
        {
            handlers.Remove(gameEventHandler);
        }
    }

    public HookResult ProcessEvent(GameEvent @event, GameEventInfo info, string eventName)
    {
        HookResult returnValue = HookResult.Continue;

        foreach(CSSharpFixes.GameEventHandler handler in _events[eventName])
        {
            switch(handler(@event, info, _logger))
            {
                case HookResult.Continue:
                    continue;
                case HookResult.Changed:
                    returnValue = HookResult.Changed;
                    break;
                case HookResult.Handled:
                    returnValue = HookResult.Handled;
                    break;
                case HookResult.Stop:
                    return HookResult.Stop;
            }
        }

        return returnValue;
    }
    
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info) =>
        ProcessEvent(@event, info, "OnRoundStart");
    
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info) =>
        ProcessEvent(@event, info, "OnPlayerSpawn");
    
    public HookResult OnPlayerTeam(EventPlayerTeam @event, GameEventInfo info) =>
        ProcessEvent(@event, info, "OnPlayerTeam");
}