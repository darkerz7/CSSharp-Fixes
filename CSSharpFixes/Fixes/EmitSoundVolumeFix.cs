namespace CSSharpFixes.Fixes;

public class EmitSoundVolumeFix: BaseFix
{
    public EmitSoundVolumeFix()
    {
        Name = "EmitSoundVolumeFix";
        ConfigurationProperty = "EnableEmitSoundVolumeFix";
        PatchNames =
		[
			"EmitSndVolumeFix"
		];
    }
}