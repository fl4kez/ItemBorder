using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;

namespace ItemBorder
{
    public class GlobalItemBorderTile : GlobalTile
    {
        public bool FrameHovered = false;
        public override void MouseOver(int i, int j, int type)
        {
            if(type == ItemID.ItemFrame)
            {
                FrameHovered = true;
            }
            base.MouseOver(i, j, type);
        }

        
    }
}
