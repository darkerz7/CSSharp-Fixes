/*
    =============================================================================
    CS#Fixes
    Copyright (C) 2023-2024 Charles Barone <CharlesBarone> / hypnos <hyps.dev>
    =============================================================================

    This program is free software; you can redistribute it and/or modify it under
    the terms of the GNU General Public License, version 3.0, as published by the
    Free Software Foundation.

    This program is distributed in the hope that it will be useful, but WITHOUT
    ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
    FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
    details.

    You should have received a copy of the GNU General Public License along with
    this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace CSSharpFixes.Fixes;

public class HammerIDFix: BaseFix
{
	bool bHammerIDFixOnce = true;
	byte[] byteHammerIDFixPatch = { 0xB0, 0x01 };
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