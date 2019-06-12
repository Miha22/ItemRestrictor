using Rocket.API;
using Rocket.API.Extensions;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemRestrictor
{
    public class CommandV : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "v";
        public string Help => "Gives yourself a vehicle";
        public string Syntax => "<id> [amount]";
        public List<string> Aliases => new List<string>() { "vehicle" };
        public List<string> Permissions => new List<string>() { "rocket.vehicle", "rocket.v" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length != 1)
            {
                UnturnedChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                throw new WrongUsageOfCommandException(caller, (IRocketCommand)this);
            }
            ushort? nullable = command.GetUInt16Parameter(0);
            if (!nullable.HasValue)
            {
                string stringParameter = command.GetStringParameter(0);
                if (stringParameter == null)
                {
                    UnturnedChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                    throw new WrongUsageOfCommandException(caller, (IRocketCommand)this);
                }
                foreach (VehicleAsset vehicleAsset in Assets.find(EAssetType.VEHICLE))
                {
                    if (vehicleAsset != null && vehicleAsset.vehicleName != null && vehicleAsset.vehicleName.ToLower().Contains(stringParameter.ToLower()))
                    {
                        nullable = new ushort?(vehicleAsset.id);
                        break;
                    }
                }
                if (!nullable.HasValue)
                {
                    UnturnedChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                    throw new WrongUsageOfCommandException(caller, (IRocketCommand)this);
                }
            }
            string vehicleName = ((VehicleAsset)Assets.find(EAssetType.VEHICLE, nullable.Value)).vehicleName;
            //if (U.Settings.Instance.EnableVehicleBlacklist && !player.HasPermission("vehicleblacklist.bypass") && player.HasPermission("vehicle." + (object)nullable))
            //    UnturnedChat.Say(caller, U.Translate("command_v_blacklisted"));

            //foreach (var group in ItemRestrictor._instance.Configuration.Instance.Groups)// checking id for blacklist    OLD SCHOOL BY M22
            //{
            //    if (ItemRestrictor.IsPlayersGroup(caller, group) && group.BlackListVehicles.Contains((ushort)nullable))
            //    {
            //        UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_v_blacklisted"));
            //        return;
            //    }
            //}

            if (ItemRestrictor._instance.Configuration.Instance.Groups.Any(g => ItemRestrictor.IsPlayersGroup(caller, g) // NEW SCHOOL EXPLAINED BY DAEMONN
                                                                    && g.BlackListVehicles.Contains((ushort)nullable)))
            {
                UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_v_blacklisted"));
                return;
            }

            if (VehicleTool.giveVehicle(player.Player, nullable.Value))
            {
                Logger.Log(U.Translate("command_v_giving_console", (object)player.CharacterName, (object)nullable), ConsoleColor.White);
                UnturnedChat.Say(caller, U.Translate("command_v_giving_private", (object)vehicleName, (object)nullable));
            }
            else
                UnturnedChat.Say(caller, U.Translate("command_v_giving_failed_private", (object)vehicleName, (object)nullable));
        }
    }
}
