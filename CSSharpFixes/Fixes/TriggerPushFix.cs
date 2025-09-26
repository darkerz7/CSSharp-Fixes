namespace CSSharpFixes.Fixes;

public class TriggerPushFix: BaseFix
{
    public TriggerPushFix()
    {
        Name = "TriggerPushFix";
        ConfigurationProperty = "EnableTriggerPushFix";
        DetourHandlerNames =
		[
			"TriggerPushTouchHandler"
        ];
    }
}