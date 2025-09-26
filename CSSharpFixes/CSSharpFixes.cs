/*
    =============================================================================
    CS#Fixes
    Copyright (C) 2023-2025 Charles Barone <CharlesBarone> / hypnos <hyps.dev>
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
using CounterStrikeSharp.API.Core.Attributes;
using CSSharpFixes.Config;
using CSSharpFixes.Managers;

namespace CSSharpFixes;

[MinimumApiVersion(330)]
public partial class CSSharpFixes(ModuleInformation moduleInformation, GameDataManager gameDataManager, DetourManager detourManager,
	PatchManager patchManager, EventManager eventManager, FixManager fixManager, Configuration configuration) : BasePlugin
{
    public override string ModuleName => ModuleInformation.ModuleName;
    public override string ModuleVersion => ModuleInformation.ModuleVersion;
    public override string ModuleAuthor => ModuleInformation.ModuleAuthor;
    public override string ModuleDescription => ModuleInformation.ModuleDescription;
    
    private readonly ModuleInformation _moduleInformation = moduleInformation;
    private readonly Configuration _configuration = configuration;
    
    private readonly GameDataManager _gameDataManager = gameDataManager;
    private readonly DetourManager _detourManager = detourManager;
    private readonly PatchManager _patchManager = patchManager;
    private readonly EventManager _eventManager = eventManager;
    private readonly FixManager _fixManager = fixManager;

	public override void Load(bool hotReload)
    {
        RegisterHooks();
        RegisterConVars();
        _gameDataManager.Start();
        _eventManager.Start();
        _detourManager.Start();
        _patchManager.Start();
        _fixManager.Start();
        _configuration.Start();
    }

    public override void Unload(bool hotReload)
    {
        UnregisterHooks();
        _fixManager.Stop();
        _patchManager.Stop();
        _detourManager.Stop();
        _eventManager.Stop();
        _gameDataManager.Stop();
    }
}