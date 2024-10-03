using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace ItemBorder
{
    public class TableRowConfig
    {
        public UIText Label;
        //public string KEY;
        public UICheckbox Border;
        public UICheckbox Outline;

        public TableRowConfig(UIText label, UICheckbox border, UICheckbox outline)
        {
            //KEY = key;
            Label = label;
            Border = border;
            Outline = outline;
        }

        bool BorderValue()
        {
            return this.Border.Selected;
        }
        bool OutlineValue()
        {
            return this.Outline.Selected;
        }
    }
    public class ItemBorderConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Border")]
        //[LabelKey("Border type")]
        [DefaultValue(11)]
        [Range(0, 11)]
        public int borderType;

        //[LabelKey("Use border")]
        //[LabelKey("Use border")]
        //[TooltipKey("Use border")]
        [DefaultValue(true)]
        public bool useBorder;

        //[LabelKey("Use base rarity")]
        //[TooltipKey("Uses the base rarity of an item instead of the rarity that comes with reforging")]
        [DefaultValue(false)]
        public bool useBaseRarity;

        //[LabelKey("Border opacity %")]
        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public int borderOpacity;

        //TYPES
        [Header("Types")]
        //[LabelKey("Use for potions")]
        [DefaultValue(false)]
        public bool showPotionBorder;

        //[LabelKey("Use for tiles")]
        [DefaultValue(true)]
        public bool showTileBorder;

        //[LabelKey("Use for walls")]
        [DefaultValue(false)]
        public bool showWallBorder;

        //[LabelKey("Use for materials")]
        [DefaultValue(true)]
        public bool showMaterialBorder;

        //EXTRA
        [Header("Extra")]
        //[TooltipKey("Use to mark items in a special way")]
        //[LabelKey("Special items")]
        public List<ItemDefinition> specialItems = new List<ItemDefinition>();

        //[LabelKey("Unique border on new item pickup")]
        //[TooltipKey("Enables/disables the special border on a first time pickup")]
        [DefaultValue(false)]
        public bool specialPickup;

        //[LabelKey("Custom border")]
        [Increment(1)]
        [DefaultValue(-1)]
        [Range(-1,1000)]
        public int customBorderType;

        //[LabelKey("Use outline")]
        //[TooltipKey("Use item outline")]
        [DefaultValue(false)]
        public bool useOutline;

        //[LabelKey("Outline width")]
        [Range(1,3)]
        [DefaultValue(2)]
        public int outlineWidth;

        /*//[LabelKey("Special item background color")]
        //[TooltipKey("Set to 0 0 0 for rainbow")]
        public Color specialColor;*/

        //[CustomModConfigItem(typeof(ConfigUISlot))]
        //public ConfigUISlot item;

        //SLOT
        [Header("Slot-options")]
        //[LabelKey("Use for hotbar slots")]
        [DefaultValue(true)]
        public bool useForHotbar;

        //[LabelKey("Use for chest slots")]
        [DefaultValue(true)]
        public bool useForChests;

        //[LabelKey("Use for inventory slots")]
        [DefaultValue(true)]
        public bool useForInventory;

        //[LabelKey("Use for ammo slots")]
        [DefaultValue(false)]
        public bool useForAmmo;

        //[LabelKey("Use for coin slots")]
        [DefaultValue(false)]
        public bool useForCoin;

        //[LabelKey("Use for trash slot")]
        [DefaultValue(true)]
        public bool useForTrash;

        //[LabelKey("Use for armor slots")]
        [DefaultValue(true)]
        public bool useForArmor;

        //[LabelKey("Use for vanity armor slots")]
        [DefaultValue(true)]
        public bool useForVanityArmor;

        //[LabelKey("Use for accessory slots")]
        [DefaultValue(true)]
        public bool useForAccessory;

        //[LabelKey("Use for vanity accessory slots")]
        [DefaultValue(true)]
        public bool useForVanityAccessory;

        //[LabelKey("Use for dye slots")]
        [DefaultValue(true)]
        public bool useForDye;

        //[LabelKey("Use for shop slots")]
        [DefaultValue(true)]
        public bool useForShop;

        //[LabelKey("Use for grapple slot")]
        [DefaultValue(true)]
        public bool useForGrapple;

        //[LabelKey("Use for mount slot")]
        [DefaultValue(true)]
        public bool useForMount;

        //[LabelKey("Use for minecart slot")]
        [DefaultValue(true)]
        public bool useForMinecart;

        //[LabelKey("Use for pet slot")]
        [DefaultValue(true)]
        public bool useForPet;

        //[LabelKey("Use for light pet slot")]
        [DefaultValue(true)]
        public bool useForLightPet;

        //NEW CONFIGS - 09.11 - 19:14
        //[LabelKey("Use for bank slots")]
        [DefaultValue(true)]
        public bool useForBank;

        //[LabelKey("Use for hat rack hat slots")]
        [DefaultValue(true)]
        public bool useForHatRack;

        //[LabelKey("Use for hat rack dye slots")]
        [DefaultValue(true)]
        public bool useForHatRackDye;

        //[LabelKey("Use for mannequin armor slots")]
        [DefaultValue(true)]
        public bool useForMannequinArmor;

        //[LabelKey("Use for mannequin accessory slots")]
        [DefaultValue(true)]
        public bool useForMannequinAccessory;

        //[LabelKey("Use for mannequin dye slots")]
        [DefaultValue(true)]
        public bool useForMannequinDye;

        //public Texture2D border;
        

        public override void OnChanged()
        {
            ItemBorder.usePotions = showPotionBorder;
            ItemBorder.useTiles = showTileBorder;
            ItemBorder.useWalls = showWallBorder;
            ItemBorder.useMaterials = showMaterialBorder;
            ItemBorder.borderType = borderType;
            //border = ModContent.Request<Texture2D>($"ItemBorder/assets/itemBorderWhite{borderType}").Value;

            ItemBorder.useForHotbar = useForHotbar;
            ItemBorder.useForChests = useForChests;
            ItemBorder.useForInventory = useForInventory;
            ItemBorder.useForAmmo = useForAmmo;
            ItemBorder.useForCoin = useForCoin;
            ItemBorder.useForTrash = useForTrash;
            ItemBorder.useForArmor = useForArmor;
            ItemBorder.useForVanityArmor = useForVanityArmor;
            ItemBorder.useForAccessory = useForAccessory;
            ItemBorder.useForVanityAccessory = useForVanityAccessory;
            ItemBorder.useForDye = useForDye;
            ItemBorder.useForShop = useForShop;
            ItemBorder.useForGrapple = useForGrapple;
            ItemBorder.useForMount = useForMount;
            ItemBorder.useForMinecart = useForMinecart;
            ItemBorder.useForPet = useForPet;
            ItemBorder.useForLightPet = useForLightPet;

            ItemBorder.useForBank = useForBank;
            ItemBorder.useForHatRack = useForHatRack;
            ItemBorder.useForHatRackDye = useForHatRackDye;
            ItemBorder.useForMannequinArmor = useForMannequinArmor;
            ItemBorder.useForMannequinAccessory = useForMannequinAccessory;
            ItemBorder.useForMannequinDye = useForMannequinDye;

            ItemBorder.borderOpacity = (float)borderOpacity / 100;


            //ItemBorder.itemDefinitions = specialItems;
            ItemBorder.itemDefinitions = specialItems.Select(x => x.Type).ToList();
            //ItemBorder.specialColor = specialColor;
            ItemBorder.customBorder = customBorderType;

            ItemBorder.specialPickup = specialPickup;

            ItemBorder.useOutline = useOutline;
            ItemBorder.useBorder = useBorder;
            ItemBorder.useBaseRarity = useBaseRarity;

            ItemBorder.outlineWidth = outlineWidth;
        }
        public override void OnLoaded()
        {
            ItemBorder.usePotions = showPotionBorder;
            ItemBorder.useTiles = showTileBorder;
            ItemBorder.useWalls = showWallBorder;
            ItemBorder.useMaterials = showMaterialBorder;
            ItemBorder.borderType = borderType;
            

            ItemBorder.useForHotbar = useForHotbar;
            ItemBorder.useForChests = useForChests;
            ItemBorder.useForInventory = useForInventory;
            ItemBorder.useForAmmo = useForAmmo;
            ItemBorder.useForCoin = useForCoin;
            ItemBorder.useForTrash = useForTrash;
            ItemBorder.useForArmor = useForArmor;
            ItemBorder.useForVanityArmor = useForVanityArmor;
            ItemBorder.useForAccessory = useForAccessory;
            ItemBorder.useForVanityAccessory = useForVanityAccessory;
            ItemBorder.useForDye = useForDye;
            ItemBorder.useForShop = useForShop;
            ItemBorder.useForGrapple = useForGrapple;
            ItemBorder.useForMount = useForMount;
            ItemBorder.useForMinecart = useForMinecart;
            ItemBorder.useForPet = useForPet;
            ItemBorder.useForLightPet = useForLightPet;

            ItemBorder.useForBank = useForBank;
            ItemBorder.useForHatRack = useForHatRack;
            ItemBorder.useForHatRackDye = useForHatRackDye;
            ItemBorder.useForMannequinArmor = useForMannequinArmor;
            ItemBorder.useForMannequinAccessory = useForMannequinAccessory;
            ItemBorder.useForMannequinDye = useForMannequinDye;
            ItemBorder.borderOpacity = (float)borderOpacity / 100;




            //ItemBorder.itemDefinitions = specialItems;
            ItemBorder.itemDefinitions = specialItems.Select(x => x.Type).ToList();
            //ItemBorder.specialColor = specialColor;
            ItemBorder.customBorder = customBorderType;

            ItemBorder.specialPickup = specialPickup;

            ItemBorder.useOutline = useOutline;
            ItemBorder.useBorder = useBorder;
            ItemBorder.useBaseRarity = useBaseRarity;

            ItemBorder.outlineWidth = outlineWidth;
        }

        [Header("Customization")]
        ////[LabelKey("$Customization Table")]
        ////[CustomModConfigItem(typeof(CustomTableUI))]
        //// List of all table entries with labels and border/outline booleans
        public CustomTableUI ConfigTable;


    }
}
