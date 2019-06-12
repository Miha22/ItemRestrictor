#region Configuration
using Rocket.API;
using System.Collections.Generic;

namespace ItemRestrictor
{
    public class PluginConfiguration : IRocketPluginConfiguration
    {
        public bool Enabled;
        public List<Group> Groups;
        public void LoadDefaults()
        {
            Groups = new List<Group>
            {
                new Group()
                {
                    GroupID = "vip",
                    BlackListItems = new List<ushort>() { 1394, 519, 1241 }, 
                    BlackListVehicles = new List<ushort>() { 120, 121, 137 }
                },
                new Group()
                {
                    GroupID = "moderator",
                    BlackListItems = new List<ushort>() { 519, 1241, 519 },
                    BlackListVehicles = new List<ushort>() { 137, 120, 137 }
                }
            };
            Enabled = true;
        }
    }
}
#endregion Configuration
