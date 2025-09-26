using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace CSSharpFixes.Fixes;

public class HammerIDFix: BaseFix
{
	bool bHammerIDFixOnce = true;
	readonly byte[] byteHammerIDFixPatch = [0xB0, 0x01];
	public HammerIDFix()
    {
        Name = "HammerIDFix";
        ConfigurationProperty = "EnableHammerIDFix";
	}
	unsafe public void Listener_OnEntityCreated(CEntityInstance entity, ILogger<CSSharpFixes> logger)
	{
        if (bHammerIDFixOnce && Enabled)
        {
			void** vtable = *(void***)entity.Handle;
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) Memory.UnixMemoryUtils.PatchBytesAtAddress((IntPtr)vtable[GameData.GetOffset("GetHammerUniqueId")], byteHammerIDFixPatch, 2);
			else Memory.WinMemoryUtils.PatchBytesAtAddress((IntPtr)vtable[GameData.GetOffset("GetHammerUniqueId")], byteHammerIDFixPatch, 2);
			bHammerIDFixOnce = false;
			logger.LogInformation("[CSSharpFixes][Fix][HammerIDFix][OnEntityCreated()][GetHammerUniqueId once patch]");
		}
	}
}