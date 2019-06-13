using Rocket.API.Collections;
using Rocket.Unturned.Player;
using Rocket.Core.Plugins;
using System;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.API;
using Rocket.Core;
using System.Linq;

namespace ItemRestrictor
{
    class ItemRestrictor : RocketPlugin<PluginConfiguration>
    {
        public static ItemRestrictor _instance;

        public ItemRestrictor()
        {

        }

        protected override void Load()
        {
            if (Configuration.Instance.Enabled)
            {
                _instance = this;
                Logger.Log("ItemRestrictor loaded!", ConsoleColor.Cyan);
            }
            else
            {
                Logger.Log("Configuration.Instance.Enabled == false");
                Logger.Log("ItemRestrictor Unloaded!", ConsoleColor.Cyan);
                this.Unload();
            }
        }

        public static bool IsPlayersGroup(IRocketPlayer caller, Group group)
        {
            string[] groups = R.Permissions.GetGroups(caller, true).Select(g => g.Id).ToArray();
            for (ushort i = 0; i < groups.Length; i++)
            {
                if (group.GroupID.ToLower() == groups[i].ToLower())
                    return true;
            }

            return false;
        }
    }
}
