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

using System.Runtime.InteropServices;
using CounterStrikeSharp.API.Modules.Memory;
using CSSharpFixes.Models;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes.Managers;

public class PatchManager
{
    private List<Patch> _patches = new();
    
    private readonly GameDataManager _gameDataManager;
    private readonly ILogger<CSSharpFixes> _logger;
    
    public PatchManager(GameDataManager gameDataManager, ILogger<CSSharpFixes> logger)
    {
        _gameDataManager = gameDataManager;
        _logger = logger;
    }

    public void Start()
    {
        LoadCommonPatches();
    }
    
    public void Stop()
    {
        foreach (Patch patch in _patches)
        {
            patch.UndoPatch();
        }
        
        _patches.Clear();
    }

    // https://github.com/Source2ZE/CS2Fixes/blob/main/gamedata/cs2fixes.games.txt
    private void LoadCommonPatches()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Water Fix
            AddServerPatch("FixWaterFloorJump", "CheckJumpButtonWater", "11 43");
            
            // CPhysBox_Use Fix
            // Make func_physbox pass itself as the caller in OnPlayerUse
            // pCaller = inputdata->pCaller ->  pCaller = this
            // Linux: 	 mov rdx, [r12+8] 	->  mov rdx, rbx
            AddServerPatch("CPhysBox_Use", "CPhysBox_Use", "48 89 DA 90 90");
            
            // Server Movement Unlocker
            AddServerPatch("ServerMovementUnlock", "ServerMovementUnlock", "90 90 90 90 90 90");

			// BotNavIgnore Fix
			// Linux BotNavIgnore patch is now very similar to Windows
			AddServerPatch("BotNavIgnore", "BotNavIgnore", "E9 25 00 00 00 90");

			// Emit Sound Volume Fix
			AddServerPatch("EmitSndVolumeFix", "EmitSoundVolumeFix", "B8 E9 54 60 BD");
		}
        else
        {
            // Water Fix
            AddServerPatch("FixWaterFloorJump", "CheckJumpButtonWater", "11 43");
            
            // CPhysBox_Use Fix
            // Make func_physbox pass itself as the caller in OnPlayerUse
            // pCaller = inputdata->pCaller ->  pCaller = this
            // Windows:  mov r8, [rdi+8]  	->  mov r8, rbx
            AddServerPatch("CPhysBox_Use", "CPhysBox_Use", "49 89 F8 90");
            
            // Server Movement Unlocker
            AddServerPatch("ServerMovementUnlock", "ServerMovementUnlock", "E9 B8 00 00 00 90");
            
            // BotNavIgnore Fix
            AddServerPatch("BotNavIgnore", "BotNavIgnore", "E9 2C 00 00 00 90");

			// Emit Sound Volume Fix
			AddServerPatch("EmitSndVolumeFix", "EmitSoundVolumeFix", "41 B9 E9 54 60 BD");
		}
    }
    
    private void AddServerPatch(string name, string signature, string bytesHex)
    {
        _patches.Add(new Patch(name, Addresses.ServerPath, signature, bytesHex, _gameDataManager, _logger));
    }
    

    public void PerformPatch(string name)
    {
        // Find Patch by name
        int patch = _patches.FindIndex(patch => patch.GetPatchName() == name);
        if (patch == -1)
        {
            _logger.LogError(
                "[CSSharpFixes][PatchManager][PerformPatch()][Patch={patchName}] Error: Patch not found.",
                patch);
            return;
        }
        
        _patches[patch].PerformPatch();
	}
    
    public void UndoPatch(string name)
    {
        // Find Patch by name
        int patch = _patches.FindIndex(patch => patch.GetPatchName() == name);
        if (patch == -1)
        {
            _logger.LogError(
                "[CSSharpFixes][PatchManager][UndoPatch()][Patch={patchName}] Error: Patch not found.",
                patch);
            return;
        }

		_patches[patch].UndoPatch();
    }
}