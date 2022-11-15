using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace ItemBorder
{
    public class ConfigUISlot : ConfigElement
    {
        Texture2D texture;
        Item item;
        public override void OnBind()
        {
            base.OnBind();
            texture = TextureAssets.InventoryBack9.Value;
            item = Main.item[ItemID.RoyalGel];

            //ItemSlot.Draw
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            CalculatedStyle dimensions = GetDimensions();
            Rectangle rect = new Rectangle(0, 0, (texture.Width - 2) / 2, texture.Height);
            spriteBatch.Draw(texture, rect, Color.LightGreen);
            //ItemSlot.Draw(spriteBatch, ref item, 0, )
        }
    }
}
