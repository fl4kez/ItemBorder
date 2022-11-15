using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.Config;

namespace ItemBorder
{

    public class ItemBorderItemDefinition
    {
        [Label("Item")]
        public ItemDefinition defintion = new ItemDefinition();
        [Label("Color")]
        public Color specialColor;

        public override string ToString()
        {
            return defintion.Name;
        }
    }
}
