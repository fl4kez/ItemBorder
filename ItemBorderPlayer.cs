using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ItemBorder
{
    public class ItemBorderPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ItemBorder.toggleItemBorderKey.JustPressed)
            {
                TryToggleItemBorder();
            }
        }

        private void TryToggleItemBorder()
        {
            Item hoveredItem = GetHoveredItem();
            if (hoveredItem != null && !hoveredItem.IsAir)
            {
                ToggleItemBorder(hoveredItem);
            }
        }

        private Item GetHoveredItem()
        {
            if (Main.HoverItem != null && !Main.HoverItem.IsAir)
            {
                return Main.HoverItem;
            }

            return null;
        }

        private void ToggleItemBorder(Item item)
        {
            var config = ModContent.GetInstance<ItemBorderConfig>();
            if (config == null) return;

            ItemDefinition itemDef = new ItemDefinition(item.type);
            
            var existingExclusion = config.excludedItems.FirstOrDefault(x => x.Type == item.type);
            
            if (existingExclusion != null)
            {
                config.excludedItems.Remove(existingExclusion);
            //    Main.NewText($"Borders restored for {item.Name}", 0, 255, 0);
            }
            else
            {
                config.excludedItems.Add(itemDef);
            //    Main.NewText($"Borders disabled for {item.Name}", 255, 100, 100);
            }
            
            try
            {
                MethodInfo saveMethod = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.Public);
                saveMethod?.Invoke(null, new object[] { config });
            }
            catch
            {
            }
        }
    }
}
