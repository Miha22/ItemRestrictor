using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ItemRestrictor
{
    public class Group
    {
        public string GroupID { get; set; }
        [XmlArrayItem(ElementName = "ID")]
        public List<ushort> BlackListItems;
        [XmlArrayItem(ElementName = "ID")]
        public List<ushort> BlackListVehicles;
        public Group()
        {

        }
    }
}
