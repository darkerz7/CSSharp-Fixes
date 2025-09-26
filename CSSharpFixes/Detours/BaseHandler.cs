using System.Reflection;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes.Detours;

public abstract class BaseHandler
{
    public string Name { get; set; }

    public abstract Enums.Detours.Mode Mode { get; }
    public abstract Models.Detour PreDetour { get; }
    public abstract Models.Detour PostDetour { get; }

    protected readonly ILogger<CSSharpFixes> _logger;
    
    public abstract void Start();
    public abstract void Stop();
    protected abstract void UnhookAllDetours();
    
    protected BaseHandler(string name, ILogger<CSSharpFixes> logger)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));;
    }
    
    public static T Build<T>(ILogger<CSSharpFixes> logger) where T : BaseHandler
    {
        MethodInfo? buildMethod = typeof(T).GetMethod("Build", [typeof(ILogger<CSSharpFixes>)]);
        if (buildMethod == null || !buildMethod.IsStatic)
            throw new InvalidOperationException($"Class {typeof(T).Name} must define a static Build method.");
        
        return buildMethod.Invoke(null, [logger]) as T ?? throw new InvalidOperationException();
    }
}

public abstract class PreHandler(string name, Models.Detour preDetour, ILogger<CSSharpFixes> logger) : BaseHandler(name, logger)
{
    public override Enums.Detours.Mode Mode => Enums.Detours.Mode.Pre;

	public override Models.Detour PreDetour { get; } = preDetour ?? throw new ArgumentNullException(nameof(preDetour));
	public override Models.Detour PostDetour 
        => throw new NotSupportedException("PostDetour is only available if Mode is set to Post or Both.");
    
    protected override void UnhookAllDetours()
    {
        if(PreDetour.IsHooked()) PreDetour.Unhook();
    }
}

public abstract class PostHandler(string name, Models.Detour postDetour, ILogger<CSSharpFixes> logger) : BaseHandler(name, logger)
{
    public override Enums.Detours.Mode Mode => Enums.Detours.Mode.Post;
    
    public override Models.Detour PreDetour 
        => throw new NotSupportedException("PreDetour is only available if Mode is set to Pre or Both.");
	public override Models.Detour PostDetour { get; } = postDetour ?? throw new ArgumentNullException(nameof(postDetour));

	protected override void UnhookAllDetours()
    {
        if(PostDetour.IsHooked()) PostDetour.Unhook();
    }
}

public abstract class PrePostHandler(string name, Models.Detour preDetour, Models.Detour postDetour, ILogger<CSSharpFixes> logger) : BaseHandler(name, logger)
{
    public override Enums.Detours.Mode Mode => Enums.Detours.Mode.Both;

	public override Models.Detour PreDetour { get; } = preDetour ?? throw new ArgumentNullException(nameof(preDetour));
	public override Models.Detour PostDetour { get; } = postDetour ?? throw new ArgumentNullException(nameof(postDetour));

	protected override void UnhookAllDetours()
    {
        if(PreDetour.IsHooked()) PreDetour.Unhook();
        if(PostDetour.IsHooked()) PostDetour.Unhook();
    }
}