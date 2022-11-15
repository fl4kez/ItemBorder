using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItemBorder
{
    public class ItemSlotContext
    {
        
        public string name { get; private set; }
        private int contextID;
        public bool enabled;

        public ItemSlotContext(string name, int contextID, bool enabled)
        {
            this.name = name;
            this.contextID = contextID;
            this.enabled = enabled;
        }

    }
}
