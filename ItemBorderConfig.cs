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
        public BoolColumn Border;
        public BoolColumn Outline;
        public BoolColumn World;

        public TableRowConfig(UIText label, BoolColumn border, BoolColumn outline, BoolColumn world)
        {
            //KEY = key;
            Label = label;
            Border = border;
            Outline = outline;
            World = world;
        }

        public bool BorderValue()
        {
            if(Border.Value != null)
                return this.Border.Value.Selected;
            return false;
        }
        public bool OutlineValue()
        {
            if (Outline.Value != null)
                return this.Outline.Value.Selected;
            return false;
        }
        public bool WorldValue()
        {
            if (World.Value != null)
                return this.World.Value.Selected;
            return false;
        }
        public class BoolColumn
        {
            public bool Use;
            public bool DefaultValue;
            public UICheckbox Value;
            public BoolColumn(bool use,bool defaultValue)
            {
                Use = use;
                DefaultValue = defaultValue;
            }
        }
    }
    public class ItemBorderConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ClientSide;

        #region BORDER
        [Header("Border")]
        [DefaultValue(true)]
        public bool useBorder;

        [DefaultValue(false)]
        public bool borderBaseRarity;

        [DefaultValue(11)]
        [Range(0, 11)]
        public int borderType;

        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public int borderOpacity;

        [Increment(1)]
        [DefaultValue(-1)]
        [Range(-1, 1000)]
        public int customBorderType;
        #endregion

        #region OUTLINE
        [Header("Outline")]
        [DefaultValue(true)]
        public bool useOutline;

        [DefaultValue(false)]
        public bool outlineBaseRarity;

        [Range(1, 5)]
        [DefaultValue(2)]
        public int outlineWidth;

        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public float outlineOpacity;
        #endregion

        #region WORLD
        [Header("World")]
        [DefaultValue(true)]
        public bool useWorld;

        [DefaultValue(false)]
        public bool worldBaseRarity;

        [Range(1, 5)]
        [DefaultValue(2)]
        public int worldWidth;

        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public float worldOpacity;
        #endregion

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

        

        //SLOT
        [Header("Slot-options")]
        [DefaultValue(true)]
        public bool useForHotbar;

        [DefaultValue(true)]
        public bool useForChests;

        [DefaultValue(true)]
        public bool useForInventory;

        [DefaultValue(false)]
        public bool useForAmmo;

        [DefaultValue(false)]
        public bool useForCoin;

        [DefaultValue(true)]
        public bool useForTrash;

        [DefaultValue(true)]
        public bool useForArmor;

        [DefaultValue(true)]
        public bool useForVanityArmor;

        [DefaultValue(true)]
        public bool useForAccessory;

        [DefaultValue(true)]
        public bool useForVanityAccessory;

        [DefaultValue(true)]
        public bool useForDye;

        [DefaultValue(true)]
        public bool useForShop;

        [DefaultValue(true)]
        public bool useForGrapple;

        [DefaultValue(true)]
        public bool useForMount;

        [DefaultValue(true)]
        public bool useForMinecart;

        [DefaultValue(true)]
        public bool useForPet;

        [DefaultValue(true)]
        public bool useForLightPet;

        [DefaultValue(true)]
        public bool useForBank;

        [DefaultValue(true)]
        public bool useForHatRack;

        [DefaultValue(true)]
        public bool useForHatRackDye;

        [DefaultValue(true)]
        public bool useForMannequinArmor;

        [DefaultValue(true)]
        public bool useForMannequinAccessory;

        [DefaultValue(true)]
        public bool useForMannequinDye;

        

        [Header("Customization")]
        ////[LabelKey("$Customization Table")]
        ////[CustomModConfigItem(typeof(CustomTableUI))]
        //// List of all table entries with labels and border/outline booleans
        public CustomTableUI ConfigTable;

        //EXTRA
        [Header("Extra")]
        [DefaultValue(false)]
        public bool specialPickup;

        public List<ItemDefinition> specialItems = new List<ItemDefinition>();

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
            ItemBorder.useBaseRarity = borderBaseRarity;

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
            ItemBorder.useBaseRarity = borderBaseRarity;

            ItemBorder.outlineWidth = outlineWidth;
        }
    }
}
