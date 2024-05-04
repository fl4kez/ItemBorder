using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using System.Linq;
using System.Diagnostics;
using Terraria.GameContent;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace ItemBorder
{
    public class ItemBorder : Mod
    {
        public static List<int> itemDefinitions;
        public static bool usePotions, useTiles;
        public static int borderType;
        internal static bool useForHotbar;
        internal static bool useForLightPet;
        internal static bool useForPet;
        internal static bool useForMinecart;
        internal static bool useForMount;
        internal static bool useForGrapple;
        internal static bool useForShop;
        internal static bool useForDye;
        internal static bool useForVanityAccessory;
        internal static bool useForAccessory;
        internal static bool useForVanityArmor;
        internal static bool useForArmor;
        internal static bool useForTrash;
        internal static bool useForCoin;
        internal static bool useForAmmo;
        internal static bool useForInventory;
        internal static bool useForChests;

        public static bool usingMagicalStorage;
        public static float borderOpacity;
        //ItemBorderConfig config => ModContent.GetInstance<ItemBorderConfig>();
        Mod magicalStorage;

        string customAssetsPath = "\\customAssets";

        public override void Load()
        {

            //Mod calamity = ModLoader.GetMod("Calamity");
            //Mod magicalStorage;
            if (ModLoader.TryGetMod("MagicStorage",out magicalStorage))
            {
                usingMagicalStorage = true;
            }
            else
            {
                usingMagicalStorage = false;
            }

            //Texture2D lel = TextureAssets.Extra[54].Value;
            //lel.SaveAsJpeg(File.Create("lel_texture.jpg"), lel.Width, lel.Height);


            Terraria.UI.On_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += DrawHandle;

            //CUSTOM BORDERS
            if(!Directory.Exists($"{Main.SavePath}{customAssetsPath}"))
            {
                Directory.CreateDirectory($"{Main.SavePath}{customAssetsPath}");
            }

            customBorders = new List<Texture2D>();

            DirectoryInfo di = new DirectoryInfo($"{Main.SavePath}{customAssetsPath}");
            FileInfo[] finfos = di.GetFiles("*.png", SearchOption.TopDirectoryOnly);
            foreach (FileInfo fi in finfos)
            {
                using (var stream = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    RunOnMainThread(() =>
                    {
                        Texture2D texture = Texture2D.FromStream(Main.graphics.GraphicsDevice, stream);
                        if(texture.Width == 52 && texture.Height == 52)
                            customBorders.Add(texture);
                    });
                    //customBorders.Add(texture);
                }
            }
        }
        public static void RunOnMainThread(Action action)
        {
            if (ReLogic.Content.AssetRepository.IsMainThread)
            {
                action();
                return;
            }
            ManualResetEventSlim evt = new(false);
            Main.QueueMainThreadAction(() =>
            {
                action();
                evt.Set();
            });
            evt.Wait();
        }
        public List<Texture2D> customBorders;
        private void DrawHandle(Terraria.UI.On_ItemSlot.orig_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color orig, SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor)
        {
            int definedAsSpecial = itemDefinitions.FirstOrDefault(x => x == inv[slot].netID,0);
            //Main.NewText($"{inv[slot].Name} {definedAsSpecial}");
            if (definedAsSpecial != 0)
            {
                spriteBatch.Draw(TextureAssets.InventoryBack16.Value,
                        position: position,
                        sourceRectangle: new Rectangle(0, 0, 52, 52),
                        color: /*(specialColor.R == 0 && specialColor.G == 0 && specialColor.B == 0)?*/Main.DiscoColor/*:specialColor*/,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: 1 * Main.inventoryScale,
                        SpriteEffects.None,
                        layerDepth: 0f);
            }
            orig(spriteBatch, inv, context, slot, position, lightColor);

            if (useBorder == true)
                ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color(orig, spriteBatch, inv, context, slot, position, lightColor);
        }

        private void ItemSlot_Draw_SpriteBatch_refItem_int_Vector2_Color(Terraria.UI.On_ItemSlot.orig_Draw_SpriteBatch_refItem_int_Vector2_Color orig, SpriteBatch spriteBatch, ref Item inv, int context, Vector2 position, Color lightColor)
        {
            //orig(spriteBatch, ref inv, context, position, lightColor);

            bool isAmmo = false;
            for (int i = 54; i < 58; i++) //AMMO SLOT
            {
                if (Main.LocalPlayer.inventory[i].Name == inv.Name)
                {
                    isAmmo = true;
                }
            }

            //POTION
            bool isPotion = true;
            if (!usePotions)
            {
                isPotion = !IsPotion(inv);
            }
            //TILE
            bool isTile = true;
            if (!useTiles)
            {
                isTile = (inv.createTile == -1);
            }
            //if (inv.Name == "Iron Shortsword")
            //    Main.NewText($"Pos:{position} inv:{inv} | {context} | {inv.rare}");

            if (!inv.IsACoin && isPotion && isTile && !isAmmo && context == ItemSlot.Context.InventoryItem)
            {
                //float proportionX = 32 / frame.Size().X;
                //float proportionY = 32 / frame.Size().Y;

                spriteBatch.Draw(ModContent.Request<Texture2D>($"ItemBorder/assets/itemBorderWhite{borderType}").Value,
                    position: position,
                    sourceRectangle: new Rectangle(0, 0, 32, 32),
                    color: (useBaseRarity==true)?(ItemRarity.GetColor(inv.OriginalRarity)):((inv.rare == -12) ? Main.DiscoColor : ItemRarity.GetColor(inv.rare)),
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1f / ((Main.LocalPlayer.HeldItem.Name == inv.Name) ? 0.75f : 1f),
                    SpriteEffects.None,
                    layerDepth: 0f);

                //if(Main.LocalPlayer.HeldItem.Name == item.Name)
                //{
                //Main.NewText($"{scale} {frame.Size()} {item.ammo} {item.FitsAmmoSlot()} {item.createTile}");
                //Main.NewText($"{item.healLife} {item.healMana} {item.buffType} {item.potion}");
                //}
            }
        }

        int[] contextIDs = new int[] { 0 };
        internal static bool useForBank;
        internal static bool useForHatRack;
        internal static bool useForHatRackDye;
        internal static bool useForMannequinArmor;
        internal static bool useForMannequinAccessory;
        internal static bool useForMannequinDye;
        internal static bool useWalls;
        internal static bool useMaterials;
        internal static int customBorder;
        internal static bool specialPickup;
        internal static bool useOutline;
        internal static bool useBorder;
        internal static bool useBaseRarity;
        internal static int outlineWidth;

        private void ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color(Terraria.UI.On_ItemSlot.orig_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color orig, SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor)
        {
            //orig(spriteBatch, inv, context, slot, position, lightColor);
            //CHECK FLAGS
            //bool[] flags = new[] {useForHotbar,useForLightPet,useForPet,useForMinecart,useForMount,useForGrapple,useForShop,useForDye,
            //useForVanityAccessory,useForAccessory,useForVanityArmor,useForArmor,useForTrash,useForCoin,useForAmmo,useForInventory,useForChests};
            switch (context)
            {
                case 13:
                    if (useForHotbar == false)
                        return;
                    break;
                case 20:
                    if (useForLightPet == false)
                        return;
                    break;
                case 19:
                    if (useForPet == false)
                        return;
                    break;
                case 18:
                    if (useForMinecart == false)
                        return;
                    break;
                case 17:
                    if (useForMount == false)
                        return;
                    break;
                case 16:
                    if (useForGrapple == false)
                        return;
                    break;
                case 15:
                    if (useForShop == false)
                        return;
                    break;
                case 12:
                    if (useForDye == false)
                        return;
                    break;
                case 11:
                    if (useForVanityAccessory == false)
                        return;
                    break;
                case 10:
                    if (useForAccessory == false)
                        return;
                    break;
                case 9:
                    if (useForVanityArmor == false)
                        return;
                    break;
                case 8:
                    if (useForArmor == false)
                        return;
                    break;
                case 6:
                    if (useForTrash == false)
                        return;
                    break;
                case 1:
                    if (useForCoin == false)
                        return;
                    break;
                case 2:
                    if (useForAmmo == false)
                        return;
                    break;
                case 0:
                    if (useForInventory == false)
                        return;
                    break;
                case 3:
                    if (useForChests == false)
                        return;
                    break;

                case 4:
                    if (useForBank == false)
                        return;
                    break;
                case 26:
                    if (useForHatRack == false)
                        return;
                    break;
                case 27:
                    if (useForHatRackDye == false)
                        return;
                    break;
                case 23:
                    if (useForMannequinArmor == false)
                        return;
                    break;
                case 24:
                    if (useForMannequinAccessory == false)
                        return;
                    break;
                case 25:
                    if (useForMannequinDye == false)
                        return;
                    break;
                default:
                    break;
            }

            Item item = inv[slot];
            {
                
                bool isAmmo = false;
                for (int i = 54; i < 58; i++) //AMMO SLOT
                {
                    if (Main.LocalPlayer.inventory[i].Name == item.Name)
                    {
                        isAmmo = true;
                    }
                }

                //POTION
                bool isPotion = true;
                if (!usePotions)
                {
                    isPotion = !IsPotion(item);
                }
                //TILE
                bool isTile = true;
                if (!useTiles)
                {
                    isTile = (item.createTile == -1);
                }

                //WALL
                bool isWall = true;
                if(!useWalls)
                {
                    isWall = (item.createWall == -1);
                }

                //MATERIAL
                bool isMaterial = true;
                if (!useMaterials)
                {
                    isMaterial = !(item.material);
                }
                
                if (isPotion && isTile && isWall && isMaterial && item.Name != "" && Main.mouseItem != item && context != ItemSlot.Context.ChatItem)
                {
                    bool isMagicStorageSlot = false;
                    if(usingMagicalStorage)
                    {
                        isMagicStorageSlot = new StackTrace().GetFrames().Any(f => f.GetMethod()?.DeclaringType == magicalStorage.Code.GetType("MagicStorage.UI.MagicStorageItemSlot"));
                    }


                    //float proportionX = 32 / frame.Size().X;
                    //float proportionY = 32 / frame.Size().Y;

                    bool normalRarity = true;
                    Color abnormalColor = new Color(0,0,0);
                    if (specialPickup)
                    {
                        if (item.GetGlobalItem<GlobalItemBorderItem>().pickedUpBefore == GlobalItemBorderItem.PickupState.PickedUpFirstTime)
                        {
                            normalRarity = false;
                            abnormalColor = new Color(Main.DiscoG, Main.DiscoR, Main.masterColor);
                        }
                    }
                    #region CoinCheck
                    else if (item.IsACoin)
                    {
                        switch (item.netID)
                        {
                            case ItemID.CopperCoin:
                                normalRarity = false;
                                abnormalColor = new Color(183, 88, 25);
                                break;
                            case ItemID.SilverCoin:
                                normalRarity = false;
                                abnormalColor = new Color(124, 141, 142);
                                break;
                            case ItemID.GoldCoin:
                                normalRarity = false;
                                abnormalColor = new Color(148, 126, 24);
                                break;
                            case ItemID.PlatinumCoin:
                                normalRarity = false;
                                abnormalColor = new Color(136, 164, 176);
                                break;

                            default: break;
                        }
                    }
                    #endregion
                    else if (item.expert == true || (useBaseRarity?item.OriginalRarity == -12:item.rare == -12))
                    {
                        normalRarity = false;
                        abnormalColor = Main.DiscoColor;
                    }
                    else if (item.master || (useBaseRarity ? item.OriginalRarity == -13 : item.rare == -13))
                    {
                        //Main.NewText($"{item.Name} {item.rare} {item.OriginalRarity} {ItemRarity.GetColor(item.rare)} ");
                        normalRarity = false;
                        abnormalColor = new Color(255, Main.masterColor, 0);//ItemRarity.GetColor(-13);
                    }
                    else if ((useBaseRarity?item.OriginalRarity:item.rare) >= ItemRarityID.Count)
                    {
                        ModRarity rarity = RarityLoader.GetRarity(useBaseRarity?item.OriginalRarity:item.rare);
                        normalRarity = false;
                        abnormalColor = rarity.RarityColor;
                        //Main.NewText($"{item.Name} {rarity.RarityColor}");
                    }


                    int actualBorderType = borderType;

                    

                    Color trueSetColor = (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(useBaseRarity?item.OriginalRarity:item.rare);
                    trueSetColor *= borderOpacity;


                    float correctScale = 1 * Main.inventoryScale;

                    Texture2D drawTexture = ModContent.Request<Texture2D>($"ItemBorder/assets/border_new{actualBorderType}").Value;
                    if(customBorders.Count > 0)
                    {
                        if(customBorder >= 0 && customBorder < customBorders.Count)
                        {
                            drawTexture = customBorders[customBorder];
                        }
                    }

                    spriteBatch.Draw(drawTexture,
                        position: position,
                        sourceRectangle: new Rectangle(0, 0, 52, 52),
                        color: trueSetColor,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: correctScale,
                        SpriteEffects.None,
                        layerDepth: 0f);


                }
                
                //Main.NewText($"{Main.libPath} ||| {Main.SavePath}");
            }
        }


        public bool IsPotion(Item item)
        {
            bool mightBeAPotion = item.healLife > 0 || item.healMana > 0 || item.buffType > 0 || item.potion;

            if (!mightBeAPotion)
                return false;

            return item.consumable && item.UseSound is SoundStyle style && (style.IsTheSameAs(SoundID.Item2) || style.IsTheSameAs(SoundID.Item3));
        }
    }   
}