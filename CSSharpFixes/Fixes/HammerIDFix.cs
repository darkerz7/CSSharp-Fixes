namespace CSSharpFixes.Fixes;

public class HammerIDFix: BaseFix
{
	public HammerIDFix()
    {
        Name = "HammerIDFix";
        ConfigurationProperty = "EnableHammerIDFix";
        PatchNames =
        [
            "SetHammerId"
        ];
    }
}