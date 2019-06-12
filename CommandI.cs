using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Rocket.API.Serialisation;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Core;

namespace ItemRestrictor
{
    public class CommandI : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "i";
        public string Help => "Gives yourself an item";
        public string Syntax => "<id> [amount]";
        public List<string> Aliases => new List<string>() { "item" };
        public List<string> Permissions => new List<string>() {"rocket.item","rocket.i"};

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            //UnturnedChat.Say("Your groups: " + string.Join(", ", R.Permissions.GetGroups(caller, true).Select(g => g.Id).ToArray()));
            if (command.Length == 0 || command.Length > 2)
            {
                UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_generic_invalid_parameter"));
                throw new WrongUsageOfCommandException(caller, (IRocketCommand)this);
            }
            //if(ItemRestrictor._instance.Configuration.Instance.Groups.Contains())
            ushort result1 = 0;
            byte result2 = 1;
            string itemString = command[0].ToString();
            if (!ushort.TryParse(itemString, out result1))
            {
                ItemAsset itemAsset = new List<ItemAsset>(Assets.find(EAssetType.ITEM).Cast<ItemAsset>()).Where<ItemAsset>((Func<ItemAsset, bool>)(i => i.itemName != null)).OrderBy<ItemAsset, int>((Func<ItemAsset, int>)(i => i.itemName.Length)).Where<ItemAsset>((Func<ItemAsset, bool>)(i => i.itemName.ToLower().Contains(itemString.ToLower()))).FirstOrDefault<ItemAsset>();
                if (itemAsset != null)
                    result1 = itemAsset.id;
                if (string.IsNullOrEmpty(itemString.Trim()) || result1 == (ushort)0)
                {
                    UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_generic_invalid_parameter"));
                    throw new WrongUsageOfCommandException(caller, (IRocketCommand)this);
                }
            }

            Asset asset = Assets.find(EAssetType.ITEM, result1);
            if (command.Length == 2 && !byte.TryParse(command[1].ToString(), out result2) || asset == null)
            {
                UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_generic_invalid_parameter"));
                throw new WrongUsageOfCommandException(caller, (IRocketCommand)this);
            }
            string itemName = ((ItemAsset)asset).itemName;
            //if (U.Settings.Instance.EnableItemBlacklist && !player.HasPermission("itemblacklist.bypass") && player.HasPermission("item." + (object)result1))
            //    UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_i_blacklisted"));

            //foreach (var group in ItemRestrictor._instance.Configuration.Instance.Groups)// checking id for blacklist    OLD SCHOOL BY M22
            //{
            //    if (ItemRestrictor.IsPlayersGroup(caller, group) && group.BlackListItems.Contains(result1))
            //    {
            //        UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_i_blacklisted"));
            //        return;
            //    }
            //}

            if (ItemRestrictor._instance.Configuration.Instance.Groups.Any(g => ItemRestrictor.IsPlayersGroup(caller, g) // NEW SCHOOL EXPLAINED BY DAEMONN
                                                                    && g.BlackListItems.Contains(result1)))
            {
                UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_i_blacklisted"));
                return;
            }

            if (U.Settings.Instance.EnableItemSpawnLimit && !player.HasPermission("itemspawnlimit.bypass") && (int)result2 > U.Settings.Instance.MaxSpawnAmount)
                UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_i_too_much", (object)U.Settings.Instance.MaxSpawnAmount));
            else if (player.GiveItem(result1, result2))
            {
                Logger.Log(U.Translate("command_i_giving_console", (object)player.DisplayName, (object)result1, (object)result2), ConsoleColor.White);
                UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_i_giving_private", (object)result2, (object)itemName, (object)result1));
            }
            else
                UnturnedChat.Say((IRocketPlayer)player, U.Translate("command_i_giving_failed_private", (object)result2, (object)itemName, (object)result1));
        }
    }
}
