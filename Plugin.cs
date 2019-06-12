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
                Logger.Log("/i by M22 loaded!", ConsoleColor.Yellow);
                Logger.Log("/i by M22 loaded!", ConsoleColor.Magenta);
                Logger.Log("/i by M22 loaded!", ConsoleColor.Green);
            }
            else
            {
                Logger.Log("Configuration.Instance.Enabled == false");
                this.Unload();
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    {"drop_message","Successfully dropped {0} inventory!" },
                    {"drop_message_id","Successfully dropped {0} from {1} inventory!"},
                    {"drop_message_public","Successfully dropped all players inventory!" },
                    {"drop_message_id_public","Successfully dropped {0} from all players inventory!"},
                };
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
