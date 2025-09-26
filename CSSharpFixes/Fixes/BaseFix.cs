namespace CSSharpFixes.Fixes;

public abstract class BaseFix
{
    public string Name = String.Empty;
    public string ConfigurationProperty = String.Empty;
    public List<string> PatchNames = [];
    public List<string> DetourHandlerNames = [];
    public Dictionary<string, CSSharpFixes.GameEventHandler> Events = [];
    public Boolean Enabled = false;
}