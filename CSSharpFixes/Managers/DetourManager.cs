using CSSharpFixes.Detours;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes.Managers;

public class DetourManager(ILogger<CSSharpFixes> logger)
{
    private readonly ILogger<CSSharpFixes> _logger = logger;
    
    private readonly Dictionary<string, BaseHandler> _handlers = [];

	public void Start()
    {
        _handlers.Add("ProcessUserCmdsHandler", BaseHandler.Build<ProcessUserCmdsHandler>(_logger));
        _handlers.Add("TriggerPushTouchHandler", BaseHandler.Build<TriggerPushTouchHandler>(_logger));
    }
    
    public void Stop()
    {
        StopAllHandlers();
        _handlers.Clear();
    }

    private void StartAllHandlers() { foreach(BaseHandler handler in _handlers.Values) handler.Start(); }
    private void StopAllHandlers() { foreach(BaseHandler handler in _handlers.Values) handler.Stop(); }
    
    public void StartHandler(string name)
    {
        if(_handlers.TryGetValue(name, out BaseHandler? handler)) handler.Start();
    }
    
    public void StopHandler(string name)
    {
        if(_handlers.TryGetValue(name, out BaseHandler? handler)) handler.Stop();
    }
    
    
}