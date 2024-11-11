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

using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.UserMessages;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes.Fixes;

public class TeamMessagesFix: BaseFix
{
    public TeamMessagesFix()
    {
        Name = "TeamMessagesFix";
        ConfigurationProperty = "DisableTeamMessages";
		Events = new Dictionary<string, CSSharpFixes.GameEventHandler>
		{
			{ "OnRoundStart", OnRoundStart },
		};
	}

	public HookResult OnRoundStart(GameEvent @event, GameEventInfo info, ILogger<CSSharpFixes> logger)
	{
		Server.ExecuteCommand("sv_ignoregrenaderadio 1");
		logger.LogInformation("[CSSharpFixes][Fix][TeamMessagesFix][OnRoundStart()][sv_ignoregrenaderadio set to 1]");
		return HookResult.Continue;
	}

	public HookResult OnChatMessage(UserMessage um)
	{
		if (!Enabled) return HookResult.Continue;
		for (int i = 0; i < um.GetRepeatedFieldCount("param"); i++)
		{
			var message = um.ReadString("param", i);
			for (int j = 0; j < TeamWarningArray.Length; j++)
				if (message.Contains(TeamWarningArray[j])) return HookResult.Stop;

			for (int j = 0; j < MoneyMessageArray.Length; j++)
				if (message.Contains(MoneyMessageArray[j])) return HookResult.Stop;
			
			for (int j = 0; j < SavedbyArray.Length; j++)
			if (message.Contains(SavedbyArray[j])) return HookResult.Stop;

			if (message.Contains("Pet_Killed")) return HookResult.Stop;
		}
		return HookResult.Continue;
	}

	public HookResult Listener_RadioCommands(CCSPlayerController? player, CommandInfo info)
	{
		if (!Enabled) return HookResult.Continue;
		return HookResult.Handled;
	}
	public HookResult Listener_Chatwheel(CCSPlayerController? player, CommandInfo info)
	{
		if (!Enabled) return HookResult.Continue;
		return HookResult.Handled;
	}

	public static string[] RadioArray = new string[] {
	"coverme",
	"takepoint",
	"holdpos",
	"regroup",
	"followme",
	"takingfire",
	"go",
	"fallback",
	"sticktog",
	"getinpos",
	"stormfront",
	"report",
	"roger",
	"enemyspot",
	"needbackup",
	"sectorclear",
	"inposition",
	"reportingin",
	"getout",
	"negative",
	"enemydown",
	"sorry",
	"cheer",
	"compliment",
	"thanks",
	"go_a",
	"go_b",
	"needrop",
	"deathcry"
	};
	public static string[] MoneyMessageArray = new string[] {
	"Player_Cash_Award_Kill_Teammate",
	"Player_Cash_Award_Killed_VIP",
	"Player_Cash_Award_Killed_Enemy_Generic",
	"Player_Cash_Award_Killed_Enemy",
	"Player_Cash_Award_Bomb_Planted",
	"Player_Cash_Award_Bomb_Defused",
	"Player_Cash_Award_Rescued_Hostage",
	"Player_Cash_Award_Interact_Hostage",
	"Player_Cash_Award_Respawn",
	"Player_Cash_Award_Get_Killed",
	"Player_Cash_Award_Damage_Hostage",
	"Player_Cash_Award_Kill_Hostage",
	"Player_Point_Award_Killed_Enemy",
	"Player_Point_Award_Killed_Enemy_Plural",
	"Player_Point_Award_Killed_Enemy_NoWeapon",
	"Player_Point_Award_Killed_Enemy_NoWeapon_Plural",
	"Player_Point_Award_Assist_Enemy",
	"Player_Point_Award_Assist_Enemy_Plural",
	"Player_Point_Award_Picked_Up_Dogtag",
	"Player_Point_Award_Picked_Up_Dogtag_Plural",
	"Player_Team_Award_Killed_Enemy",
	"Player_Team_Award_Killed_Enemy_Plural",
	"Player_Team_Award_Bonus_Weapon",
	"Player_Team_Award_Bonus_Weapon_Plural",
	"Player_Team_Award_Picked_Up_Dogtag",
	"Player_Team_Award_Picked_Up_Dogtag_Plural",
	"Player_Team_Award_Picked_Up_Dogtag_Friendly",
	"Player_Cash_Award_ExplainSuicide_YouGotCash",
	"Player_Cash_Award_ExplainSuicide_TeammateGotCash",
	"Player_Cash_Award_ExplainSuicide_EnemyGotCash",
	"Player_Cash_Award_ExplainSuicide_Spectators",
	"Team_Cash_Award_T_Win_Bomb",
	"Team_Cash_Award_Elim_Hostage",
	"Team_Cash_Award_Elim_Bomb",
	"Team_Cash_Award_Win_Time",
	"Team_Cash_Award_Win_Defuse_Bomb",
	"Team_Cash_Award_Win_Hostages_Rescue",
	"Team_Cash_Award_Win_Hostage_Rescue",
	"Team_Cash_Award_Loser_Bonus",
	"Team_Cash_Award_Bonus_Shorthanded",
	"Notice_Bonus_Enemy_Team",
	"Notice_Bonus_Shorthanded_Eligibility",
	"Notice_Bonus_Shorthanded_Eligibility_Single",
	"Team_Cash_Award_Loser_Bonus_Neg",
	"Team_Cash_Award_Loser_Zero",
	"Team_Cash_Award_Rescued_Hostage",
	"Team_Cash_Award_Hostage_Interaction",
	"Team_Cash_Award_Hostage_Alive",
	"Team_Cash_Award_Planted_Bomb_But_Defused",
	"Team_Cash_Award_Survive_GuardianMode_Wave",
	"Team_Cash_Award_CT_VIP_Escaped",
	"Team_Cash_Award_T_VIP_Killed",
	"Team_Cash_Award_no_income",
	"Team_Cash_Award_no_income_suicide",
	"Team_Cash_Award_Generic",
	"Team_Cash_Award_Custom"
	};
	public static string[] SavedbyArray = new string[] {
	"Chat_SavePlayer_Savior",
	"Chat_SavePlayer_Spectator",
	"Chat_SavePlayer_Saved"
	};
	public static string[] TeamWarningArray = new string[] {
	"Cstrike_TitlesTXT_Game_teammate_attack",
	"Cstrike_TitlesTXT_Game_teammate_kills",
	"Cstrike_TitlesTXT_Hint_careful_around_teammates",
	"Cstrike_TitlesTXT_Hint_try_not_to_injure_teammates",
	"Cstrike_TitlesTXT_Killed_Teammate",
	"SFUI_Notice_Game_teammate_kills",
	"SFUI_Notice_Hint_careful_around_teammates",
	"SFUI_Notice_Killed_Teammate"
	};
}