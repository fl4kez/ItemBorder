using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;


namespace ItemBorder
{
    public static class PoTHelper
    {
        [JITWhenModsEnabled("PathOfTerraria")]
        public static Color GetRarityColor(PathOfTerraria.Common.Enums.ItemRarity rarity)
        {
            return rarity switch
            {
                PathOfTerraria.Common.Enums.ItemRarity.Normal => Color.White,
                PathOfTerraria.Common.Enums.ItemRarity.Magic => new Color(110, 160, 255),
                PathOfTerraria.Common.Enums.ItemRarity.Rare => new Color(255, 255, 50),
                PathOfTerraria.Common.Enums.ItemRarity.Unique => new Color(25, 255, 25),
                _ => Color.White,
            };
        }

        public static Color GetPoTColor(Color trueSetColor, Item item)
        {
            if (ModLoader.TryGetMod("PathOfTerraria", out var potMod))
            {
                var potItemType = potMod.Code.GetType("PathOfTerraria.Core.Items.PoTInstanceItemData");
                if (potItemType != null)
                {
                    var getGlobalItemMethod = typeof(Item).GetMethod("GetGlobalItem", 1, new[] { typeof(bool) });
                    var genericMethod = getGlobalItemMethod.MakeGenericMethod(potItemType);
                    var potItemData = genericMethod.Invoke(item, new object[] { false });

                    var rarityField = potItemType.GetProperty("Rarity");
                    var rarityValue = rarityField.GetValue(potItemData);

                    var rarityEnumType = potMod.Code.GetType("PathOfTerraria.Common.Enums.ItemRarity");
                    var invalidValue = Enum.Parse(rarityEnumType, "Invalid");

                    if (!rarityValue.Equals(invalidValue))
                    {
                        var helperType = potMod.Code.GetType("PathOfTerraria.Core.Helpers.PoTHelper");
                        var getColorMethod = helperType.GetMethod("GetRarityColor");
                        var rarityColor = (Color)getColorMethod.Invoke(null, new object[] { rarityValue });

                        trueSetColor = rarityColor * ((float)ItemBorder.config.outlineOpacity / 100f);
                    }
                }
            }

            return trueSetColor;
        }


        //[JITWhenModsEnabled("PathOfTerraria")]
        //public static Color GetPoTColor(Color trueSetColor, Item item)
        //{
        //    if (ModLoader.HasMod("PathOfTerraria"))
        //    {
        //        var potRarity = item.GetGlobalItem<PathOfTerraria.Core.Items.PoTInstanceItemData>().Rarity;
        //        if (potRarity != PathOfTerraria.Common.Enums.ItemRarity.Invalid)
        //        {
        //            trueSetColor = PoTHelper.GetRarityColor(potRarity);
        //            trueSetColor *= (float)ItemBorder.config.outlineOpacity / 100f;
        //        }
        //    }
        //    return trueSetColor;
        //}
    }
}
