using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace ItemBorder
{
    public class ItemBorderConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Border")]
        [Label("Border type")]
        [DefaultValue(11)]
        [Range(0, 11)]
        public int borderType;

        [Label("Border opacity %")]
        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public int borderOpacity;

        //TYPES
        [Header("Types")]
        [Label("Use for potions")]
        [DefaultValue(false)]
        public bool showPotionBorder;

        [Label("Use for tiles")]
        [DefaultValue(true)]
        public bool showTileBorder;

        [Label("Use for walls")]
        [DefaultValue(false)]
        public bool showWallBorder;

        [Label("Use for materials")]
        [DefaultValue(true)]
        public bool showMaterialBorder;

        //EXTRA
        [Header("Extra")]
        [Tooltip("Use to mark items in a special way")]
        [Label("Special items")]
        public List<ItemDefinition> specialItems = new List<ItemDefinition>();

        [Label("Custom border")]
        [Increment(1)]
        [DefaultValue(-1)]
        public int customBorderType;

        /*[Label("Special item background color")]
        [Tooltip("Set to 0 0 0 for rainbow")]
        public Color specialColor;*/

        //[CustomModConfigItem(typeof(ConfigUISlot))]
        //public ConfigUISlot item;

        //SLOT
        [Header("Slot options")]
        [Label("Use for hotbar slots")]
        [DefaultValue(true)]
        public bool useForHotbar;

        [Label("Use for chest slots")]
        [DefaultValue(true)]
        public bool useForChests;

        [Label("Use for inventory slots")]
        [DefaultValue(true)]
        public bool useForInventory;

        [Label("Use for ammo slots")]
        [DefaultValue(false)]
        public bool useForAmmo;

        [Label("Use for coin slots")]
        [DefaultValue(false)]
        public bool useForCoin;

        [Label("Use for trash slot")]
        [DefaultValue(true)]
        public bool useForTrash;

        [Label("Use for armor slots")]
        [DefaultValue(true)]
        public bool useForArmor;

        [Label("Use for vanity armor slots")]
        [DefaultValue(true)]
        public bool useForVanityArmor;

        [Label("Use for accessory slots")]
        [DefaultValue(true)]
        public bool useForAccessory;

        [Label("Use for vanity accessory slots")]
        [DefaultValue(true)]
        public bool useForVanityAccessory;

        [Label("Use for dye slots")]
        [DefaultValue(true)]
        public bool useForDye;

        [Label("Use for shop slots")]
        [DefaultValue(true)]
        public bool useForShop;

        [Label("Use for grapple slot")]
        [DefaultValue(true)]
        public bool useForGrapple;

        [Label("Use for mount slot")]
        [DefaultValue(true)]
        public bool useForMount;

        [Label("Use for minecart slot")]
        [DefaultValue(true)]
        public bool useForMinecart;

        [Label("Use for pet slot")]
        [DefaultValue(true)]
        public bool useForPet;

        [Label("Use for light pet slot")]
        [DefaultValue(true)]
        public bool useForLightPet;

        //NEW CONFIGS - 09.11 - 19:14
        [Label("Use for bank slots")]
        [DefaultValue(true)]
        public bool useForBank;

        [Label("Use for hat rack hat slots")]
        [DefaultValue(true)]
        public bool useForHatRack;

        [Label("Use for hat rack dye slots")]
        [DefaultValue(true)]
        public bool useForHatRackDye;

        [Label("Use for mannequin armor slots")]
        [DefaultValue(true)]
        public bool useForMannequinArmor;

        [Label("Use for mannequin accessory slots")]
        [DefaultValue(true)]
        public bool useForMannequinAccessory;

        [Label("Use for mannequin dye slots")]
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
        }
        

    }
}
