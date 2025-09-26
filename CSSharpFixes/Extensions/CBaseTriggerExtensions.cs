using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;

namespace CSSharpFixes.Extensions;

public static class CBaseTriggerExtensions
{
    public static bool PassesTriggerFilters(this CBaseTrigger? trigger, IntPtr pOther)
    {
        if (trigger is null || !trigger.IsValid) return false;
        return VirtualFunction.Create<IntPtr, IntPtr, bool>(trigger.Handle, GameData.GetOffset("PassesTriggerFilters"))(
            trigger.Handle, pOther);
    }
}