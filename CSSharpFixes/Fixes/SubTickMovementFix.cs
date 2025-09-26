namespace CSSharpFixes.Fixes;

/*
 * Description: Disables subtick movement for all players on the server.
 *
 * This fix is not "really" a fix, but rather it disables subtick movement.
 * This is a prerequisite for TriggerPushFix.
 */
public class SubTickMovementFix: BaseFix
{
    public SubTickMovementFix()
    {
        Name = "SubTickMovementFix";
        ConfigurationProperty = "DisableSubTickMovement";
        DetourHandlerNames =
		[
			"ProcessUserCmdsHandler"
        ];
    }
}