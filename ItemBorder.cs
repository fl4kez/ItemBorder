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
using ReLogic.Content;
using Terraria.ModLoader.Config;

namespace ItemBorder
{
    public class ItemBorder : Mod
    {
        internal static TableRowConfig mouseItem => CustomTableUI.rows["mouseItem"];
        internal static TableRowConfig potion => CustomTableUI.rows["potion"];
        internal static TableRowConfig tile => CustomTableUI.rows["tile"];
        internal static TableRowConfig wall => CustomTableUI.rows["wall"];
        internal static TableRowConfig material => CustomTableUI.rows["material"];

        internal static TableRowConfig hotbar => CustomTableUI.rows["hotbar"];
        internal static TableRowConfig chest => CustomTableUI.rows["chest"];
        internal static TableRowConfig inventory => CustomTableUI.rows["inventory"];
        internal static TableRowConfig ammo => CustomTableUI.rows["ammo"];
        internal static TableRowConfig coin => CustomTableUI.rows["coin"];
        internal static TableRowConfig trash => CustomTableUI.rows["trash"];
        internal static TableRowConfig armor => CustomTableUI.rows["armor"];
        internal static TableRowConfig vanityArmor => CustomTableUI.rows["vanityArmor"];
        internal static TableRowConfig accessory => CustomTableUI.rows["accessory"];
        internal static TableRowConfig vanityAccessory => CustomTableUI.rows["vanityAccessory"];
        internal static TableRowConfig dye => CustomTableUI.rows["dye"];
        internal static TableRowConfig shop => CustomTableUI.rows["shop"];
        internal static TableRowConfig grapple => CustomTableUI.rows["grapple"];
        internal static TableRowConfig mount => CustomTableUI.rows["mount"];
        internal static TableRowConfig minecart => CustomTableUI.rows["minecart"];
        internal static TableRowConfig pet => CustomTableUI.rows["pet"];
        internal static TableRowConfig lightPet => CustomTableUI.rows["lightPet"];
        internal static TableRowConfig bank => CustomTableUI.rows["bank"];
        internal static TableRowConfig hatRack => CustomTableUI.rows["hatRack"];
        internal static TableRowConfig hatRackDye => CustomTableUI.rows["hatRackDye"];
        internal static TableRowConfig mannequinArmor => CustomTableUI.rows["mannequinArmor"];
        internal static TableRowConfig mannequinAccessory => CustomTableUI.rows["mannequinAccessory"];
        internal static TableRowConfig mannequinDye => CustomTableUI.rows["mannequinDye"];

        public static bool usingMagicalStorage;
        //ItemBorderConfig config => ModContent.GetInstance<ItemBorderConfig>();
        Mod magicalStorage;

        string customAssetsPath = "\\customAssets";

        public static Effect whiteEffect;
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

            if(Main.netMode != NetmodeID.Server)
            {
                whiteEffect = ModContent.Request<Effect>("ItemBorder/effects/whiteShader", AssetRequestMode.ImmediateLoad).Value;
            }

            Terraria.UI.On_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += DrawHandle;
            Terraria.UI.On_ItemSlot.DrawItemIcon += CorrectlyDrawOutline;

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

            config = ModContent.GetInstance<ItemBorderConfig>();
            config.ConfigTable = new CustomTableUI();
            config.ConfigTable.AddCustomizationRowToList("mouseItem", "Use for item in cursor", Column(false, false), Column(true, true), Column(false, false));

            config.ConfigTable.AddCustomizationRowToList("potion", "Use for potions", Column(true,true), Column(true, true), Column(true, true));
            config.ConfigTable.AddCustomizationRowToList("tile", "Use for tiles", Column(true,true), Column(true, true), Column(true, true));
            config.ConfigTable.AddCustomizationRowToList("wall", "Use for walls", Column(true,true), Column(true, true), Column(true, true));
            config.ConfigTable.AddCustomizationRowToList("material", "Use for materials", Column(true,true), Column(true, true), Column(true, true));

            config.ConfigTable.AddCustomizationRowToList("hotbar", "Use for hotbar slots", Column(true,true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("chest", "Use for chest slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("inventory", "Use for inventory slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("ammo", "Use for ammo slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("coin", "Use for coin slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("trash", "Use for trash slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("armor", "Use for armor slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("vanityArmor", "Use for vanity armor slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("accessory", "Use for accessory slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("vanityAccessory", "Use for vanity accessory slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("dye", "Use for dye slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("shop", "Use for shop slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("grapple", "Use for grapple slot", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("mount", "Use for mount slot", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("minecart", "Use for minecart slot", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("pet", "Use for pet slot", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("lightPet", "Use for light pet slot", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("bank", "Use for bank slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("hatRack", "Use for hat rack slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("hatRackDye", "Use for hat rack dye slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("mannequinArmor", "Use for mannequin armor slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("mannequinAccessory", "Use for mannequin accessory slots", Column(true, true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("mannequinDye", "Use for mannequin dye slots", Column(true, true), Column(true, true), Column(false, false));

        }
        public static ItemBorderConfig config;

        public TableRowConfig.BoolColumn Column(bool use, bool defaultVal)
        {
            return new TableRowConfig.BoolColumn(use, defaultVal);
        }

        private float CorrectlyDrawOutline(On_ItemSlot.orig_DrawItemIcon orig, Item item, int context, SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, float scale, float sizeLimit, Color environmentColor)
        {
            switch (context)
            {
                case ItemSlot.Context.MouseItem:
                    if (mouseItem.OutlineValue() == false)
                    {
                        orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
                        return scale;
                    }
                    break;
                case 13:
                    if (hotbar.OutlineValue() == false)
                    {
                        orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
                        return scale;
                    }
                    break;
                case 20:
                    if (lightPet.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 19:
                    if (pet.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 18:
                    if (minecart.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 17:
                    if (mount.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 16:
                    if (grapple.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 15:
                    if (shop.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 12:
                    if (dye.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 11:
                    if (vanityAccessory.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 10:
                    if (accessory.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 9:
                    if (vanityArmor.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 8:
                    if (armor.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 6:
                    if (trash.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 1:
                    if (coin.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 2:
                    if (ammo.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 0:
                    if (inventory.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 3:
                    if (chest.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;

                case 4:
                    if (bank.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 26:
                    if (hatRack.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 27:
                    if (hatRackDye.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 23:
                    if (mannequinArmor.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 24:
                    if (mannequinAccessory.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                case 25:
                    if (mannequinDye.OutlineValue() == false){orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);return scale;}
                    break;
                default:
                    break;
            }
            //AMMO
            if (ItemBorder.ammo.OutlineValue() == false)
            {
                if (IsAmmoSlot(item))
                {
                    orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
                    return scale;
                }
            }

            //POTION
            if (ItemBorder.potion.OutlineValue() == false)
            {
                if (IsPotion(item))
                {
                    orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
                    return scale;
                }
            }

            //TILE
            if (ItemBorder.tile.OutlineValue() == false)
            {
                if (IsTile(item))
                {
                    orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
                    return scale;
                }
            }

            //WALL
            if (ItemBorder.wall.OutlineValue() == false)
            {
                if (IsWall(item))
                {
                    orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
                    return scale;
                };
            }

            //MATERIAL
            if (ItemBorder.material.OutlineValue() == false)
            {
                if (IsMaterial(item))
                {
                    orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
                    return scale;
                }
            }
            Texture2D sprite = TextureAssets.Item[item.type].Value;
            Rectangle frame = new Rectangle(0, 0, sprite.Width, sprite.Height);
           
            Vector2 origin = new Vector2(frame.Width / 2, frame.Height / 2);
            if (CustomTableUI.rows.Count > 0)
            {
                //Main.NewText($"{CustomTableUI.rows.Count} {CustomTableUI.rows[0].Outline.Selected}");
                //if (CustomTableUI.rows["hotbar"].OutlineValue() == true)
                //{

                    // Save the current state of the spriteBatch
                    //SpriteSortMode originalSortMode = Main.spriteBatch.GraphicsDevice.BlendState == BlendState.AlphaBlend
                    //? SpriteSortMode.Deferred
                    //: SpriteSortMode.Immediate; // Example of how Terraria might be using it (you can customize this based on your needs)
                    var originalBlendState = Main.spriteBatch.GraphicsDevice.BlendState;
                    var originalSamplerState = Main.spriteBatch.GraphicsDevice.SamplerStates[0];
                    var originalDepthStencilState = Main.spriteBatch.GraphicsDevice.DepthStencilState;
                    var originalRasterizerState = Main.spriteBatch.GraphicsDevice.RasterizerState;

                    //Texture2D sprite = TextureAssets.Item[item.type].Value;
                    Texture2D spriteCopy = TextureAssets.Item[item.type].Value;

                    Rectangle rect = new Rectangle(0, 0, sprite.Width, sprite.Height);

                    float outlineWidth = ItemBorder.config.outlineWidth;

                    bool normalRarity = true;
                    Color abnormalColor = new Color(0, 0, 0);
                    ItemDefinition definedAsSpecial = IsSpecial(item);
                    if (ItemBorder.config.specialPickup && definedAsSpecial != null && item.GetGlobalItem<GlobalItemBorderItem>().pickedUpBefore == GlobalItemBorderItem.PickupState.PickedUpFirstTime)
                    {

                        normalRarity = false;
                        abnormalColor = ItemBorder.InvertMeAColour(Main.DiscoColor);//new Color(Main.DiscoG, Main.DiscoR, Main.masterColor);

                    }
                    #region CoinCheck
                    else if (item.IsACoin)
                    {
                        switch (item.type)
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
                    else if (item.expert == true || (ItemBorder.config.outlineBaseRarity ? item.OriginalRarity == -12 : item.rare == -12))
                    {
                        normalRarity = false;
                        abnormalColor = Main.DiscoColor;
                    }
                    else if (item.master || (ItemBorder.config.outlineBaseRarity ? item.OriginalRarity == -13 : item.rare == -13))
                    {
                        //Main.NewText($"{item.Name} {item.rare} {item.OriginalRarity} {ItemRarity.GetColor(item.rare)} ");
                        normalRarity = false;
                        abnormalColor = new Color(255, Main.masterColor, 0);//ItemRarity.GetColor(-13);
                    }
                    else if ((ItemBorder.config.outlineBaseRarity ? item.OriginalRarity : item.rare) >= ItemRarityID.Count)
                    {
                        ModRarity rarity = RarityLoader.GetRarity(ItemBorder.config.outlineBaseRarity ? item.OriginalRarity : item.rare);
                        normalRarity = false;
                        abnormalColor = rarity.RarityColor;
                        //Main.NewText($"{item.Name} {rarity.RarityColor}");
                    }

                    Color trueSetColor = (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(ItemBorder.config.outlineBaseRarity ? item.OriginalRarity : item.rare);
                    trueSetColor *= ItemBorder.config.outlineOpacity / 100;

                    Vector2[] offsets = new Vector2[]
                    {
                new Vector2(-outlineWidth,0),//LEFT
                new Vector2(outlineWidth,0),//RIGHT
                new Vector2(0,-outlineWidth),//UP
                new Vector2(0,outlineWidth),//DOWN
                                            //new Vector2(-outlineWidth,-outlineWidth),//TOPLEFT
                                            //new Vector2(outlineWidth,outlineWidth),//BOTTOMRIGHT
                                            //new Vector2(outlineWidth,-outlineWidth),//TOPRIGHT
                                            //new Vector2(-outlineWidth,outlineWidth),//BOTTOMLEFT
                    };
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, originalSamplerState, originalDepthStencilState, originalRasterizerState, ItemBorder.whiteEffect, Main.UIScaleMatrix);


                    ItemBorder.whiteEffect.Parameters["CustomColor"].SetValue(trueSetColor.ToVector4());
                    ItemBorder.whiteEffect.CurrentTechnique.Passes[0].Apply();

                    float scale2 = 1f;
                    float num = 1f;
                    if ((float)frame.Width > sizeLimit || (float)frame.Height > sizeLimit)
                    {
                        num = ((frame.Width <= frame.Height) ? (sizeLimit / (float)frame.Height) : (sizeLimit / (float)frame.Width));
                    }
                    float finalDrawScale = scale * num * scale2;
                    foreach (Vector2 offset in offsets)
                    {
                        spriteBatch.Draw(spriteCopy,
                                    position: screenPositionForItemCenter + offset,
                                    sourceRectangle: frame,
                                    color: trueSetColor,
                                    rotation: 0f,
                                    origin: origin,
                                    scale: finalDrawScale,
                                    SpriteEffects.None,
                                    layerDepth: 0f);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, originalBlendState, originalSamplerState, originalDepthStencilState, originalRasterizerState, null, Main.UIScaleMatrix);
                //}
            }
            //Sets Draw color ALPHA to 255 (full) to not overlay with outline color
            environmentColor.A = 255;
            orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
            return scale;
        }

        public override void Unload()
        {
            whiteEffect = null;
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
            Item item = inv[slot];
            ItemDefinition definedAsSpecial = IsSpecial(item);
            if (definedAsSpecial != null && Main.mouseItem.type != item.type)
            {
                
                if (item.GetGlobalItem<GlobalItemBorderItem>().pickedUpBefore == GlobalItemBorderItem.PickupState.PickedUpFirstTime)
                {
                    //Color color = new Color(Main.DiscoG, Main.DiscoR, Main.masterColor);

                    spriteBatch.Draw(TextureAssets.InventoryBack16.Value,
                            position: position,
                            sourceRectangle: new Rectangle(0, 0, 52, 52),
                            color: /*(specialColor.R == 0 && specialColor.G == 0 && specialColor.B == 0)?*/ItemBorder.InvertMeAColour(Main.DiscoColor)/*:specialColor*/,
                            rotation: 0f,
                            origin: Vector2.Zero,
                            scale: 1 * Main.inventoryScale,
                            SpriteEffects.None,
                            layerDepth: 0f);
                }
            }

            orig(spriteBatch, inv, context, slot, position, lightColor);
            if (config.useBorder == true)
                ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color(orig, spriteBatch, inv, context, slot, position, lightColor);
            //if (useOutline == true)
            //    DrawOutline(item,spriteBatch,position,lightColor,lightColor,1);
        }

        const int RGBMAX = 255;
        public static Color InvertMeAColour(Color ColourToInvert)
        {
            return new Color(RGBMAX - ColourToInvert.R,
              RGBMAX - ColourToInvert.G, RGBMAX - ColourToInvert.B);
        }

        public static bool IsWall(Item item)
        {
            return (item.createWall != -1);
        }
        public static bool IsTile(Item item)
        {
            return (item.createTile != -1);
        }
        public static bool IsMaterial(Item item)
        {
            return (item.material);
        }
        public static bool IsAmmoSlot(int slot)
        {
            return (slot > 53 && slot < 58);
        }
        public static bool IsAmmoSlot(Item item)
        {
            for (int i = 54; i < 58; i++) //AMMO SLOT
            {
                if (Main.LocalPlayer.inventory[i].type == item.type)
                {
                   return true;
                }
            }
            return false;
        }
        public static ItemDefinition IsSpecial(Item item)
        {
            return config.specialItems.FirstOrDefault(x => x.Type == item.type, null);
        }

        private void ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color(Terraria.UI.On_ItemSlot.orig_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color orig, SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor)
        {
            //orig(spriteBatch, inv, context, slot, position, lightColor);
            //CHECK FLAGS
            //bool[] flags = new[] {useForHotbar,useForLightPet,useForPet,useForMinecart,useForMount,useForGrapple,useForShop,useForDye,
            //useForVanityAccessory,useForAccessory,useForVanityArmor,useForArmor,useForTrash,useForCoin,useForAmmo,useForInventory,useForChests};
            switch (context)
            {
                case 13:
                    if (hotbar.BorderValue() == false)
                        return;
                    break;
                case 20:
                    if (lightPet.BorderValue() == false)
                        return;
                    break;
                case 19:
                    if (pet.BorderValue() == false)
                        return;
                    break;
                case 18:
                    if (minecart.BorderValue() == false)
                        return;
                    break;
                case 17:
                    if (mount.BorderValue() == false)
                        return;
                    break;
                case 16:
                    if (grapple.BorderValue() == false)
                        return;
                    break;
                case 15:
                    if (shop.BorderValue() == false)
                        return;
                    break;
                case 12:
                    if (dye.BorderValue() == false)
                        return;
                    break;
                case 11:
                    if (vanityAccessory.BorderValue() == false)
                        return;
                    break;
                case 10:
                    if (accessory.BorderValue() == false)
                        return;
                    break;
                case 9:
                    if (vanityArmor.BorderValue() == false)
                        return;
                    break;
                case 8:
                    if (armor.BorderValue() == false)
                        return;
                    break;
                case 6:
                    if (trash.BorderValue() == false)
                        return;
                    break;
                case 1:
                    if (coin.BorderValue() == false)
                        return;
                    break;
                case 2:
                    if (ammo.BorderValue() == false)
                        return;
                    break;
                case 0:
                    if (inventory.BorderValue() == false)
                        return;
                    break;
                case 3:
                    if (chest.BorderValue() == false)
                        return;
                    break;

                case 4:
                    if (bank.BorderValue() == false)
                        return;
                    break;
                case 26:
                    if (hatRack.BorderValue() == false)
                        return;
                    break;
                case 27:
                    if (hatRackDye.BorderValue() == false)
                        return;
                    break;
                case 23:
                    if (mannequinArmor.BorderValue() == false)
                        return;
                    break;
                case 24:
                    if (mannequinAccessory.BorderValue() == false)
                        return;
                    break;
                case 25:
                    if (mannequinDye.BorderValue() == false)
                        return;
                    break;
                default:
                    break;
            }

            Item item = inv[slot];
            
            //AMMO
            if (ItemBorder.ammo.BorderValue() == false)
            {
                if (IsAmmoSlot(slot))
                    return;
            }

            //POTION
            if (ItemBorder.potion.BorderValue() == false)
            {
                if (IsPotion(item))
                    return;
            }

            //TILE
            if (ItemBorder.tile.BorderValue() == false)
            {
                if (IsTile(item))
                    return;
            }

            //WALL
            if (ItemBorder.wall.BorderValue() == false)
            {
                if (IsWall(item))
                    return;
            }

            //MATERIAL
            if (ItemBorder.material.BorderValue() == false)
            {
                if (IsMaterial(item))
                    return;
            }
                
            if (item.Name != "" && Main.mouseItem != item && context != ItemSlot.Context.ChatItem)
            {
                bool normalRarity = true;
                Color abnormalColor = new Color(0,0,0);
                ItemDefinition definedAsSpecial = IsSpecial(item);
                if (config.specialPickup && definedAsSpecial != null && item.GetGlobalItem<GlobalItemBorderItem>().pickedUpBefore == GlobalItemBorderItem.PickupState.PickedUpFirstTime)
                {
                    normalRarity = false;
                    abnormalColor = ItemBorder.InvertMeAColour(Main.DiscoColor);//new Color(Main.DiscoG, Main.DiscoR, Main.masterColor);
                }
                #region CoinCheck
                else if (item.IsACoin)
                {
                    switch (item.type)
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
                else if (item.expert == true || (config.borderBaseRarity?item.OriginalRarity == -12:item.rare == -12))
                {
                    normalRarity = false;
                    abnormalColor = Main.DiscoColor;
                }
                else if (item.master || (config.borderBaseRarity ? item.OriginalRarity == -13 : item.rare == -13))
                {
                    //Main.NewText($"{item.Name} {item.rare} {item.OriginalRarity} {ItemRarity.GetColor(item.rare)} ");
                    normalRarity = false;
                    abnormalColor = new Color(255, Main.masterColor, 0);//ItemRarity.GetColor(-13);
                }
                else if ((config.borderBaseRarity ? item.OriginalRarity:item.rare) >= ItemRarityID.Count)
                {
                    ModRarity rarity = RarityLoader.GetRarity(config.borderBaseRarity ? item.OriginalRarity:item.rare);
                    normalRarity = false;
                    abnormalColor = rarity.RarityColor;
                    //Main.NewText($"{item.Name} {rarity.RarityColor}");
                }


                int actualBorderType = config.borderType;

                    

                Color trueSetColor = (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(config.borderBaseRarity ? item.OriginalRarity:item.rare);
                trueSetColor *= config.borderOpacity/100;


                float correctScale = 1 * Main.inventoryScale;

                Texture2D drawTexture = ModContent.Request<Texture2D>($"ItemBorder/assets/border_new{actualBorderType}").Value;
                if(customBorders.Count > 0)
                {
                    if(config.customBorderType >= 0 && config.customBorderType < customBorders.Count)
                    {
                        drawTexture = customBorders[config.customBorderType];
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


        public static bool IsPotion(Item item)
        {
            bool mightBeAPotion = item.healLife > 0 || item.healMana > 0 || item.buffType > 0 || item.potion;

            if (!mightBeAPotion)
                return false;

            return (item.consumable && item.UseSound is SoundStyle style && (style.IsTheSameAs(SoundID.Item2) || style.IsTheSameAs(SoundID.Item3)));
        }

        //public void DrawOutline(Item item, SpriteBatch spriteBatch, Vector2 position, Color drawColor, Color itemColor, float scale)
        //{
        //    Texture2D sprite = TextureAssets.Item[item.type].Value;
        //    Rectangle frame = new Rectangle(0,0,sprite.Width,sprite.Height);
        //    float sizeLimit = 32;

        //    float scale2 = 1f;
        //    float num = 1f;
        //    if ((float)frame.Width > sizeLimit || (float)frame.Height > sizeLimit)
        //    {
        //        num = ((frame.Width <= frame.Height) ? (sizeLimit / (float)frame.Height) : (sizeLimit / (float)frame.Width));
        //    }
        //    float finalDrawScale = 1f * num * scale2;
        //    if (item.Name == "Bee Keeper" || item.Name == "Mushroom")
        //    {
        //        //Main.NewText($"O:{item.Name} ({sprite.Width},{sprite.Height}) {position} {position+new Vector2(22,22)} scale:{finalDrawScale} {num} ");
        //    }
        //    Vector2 origin = new Vector2(frame.Width/2,frame.Height/2);
        //    if (CustomTableUI.rows.Count > 0)
        //    {
        //        //Main.NewText($"{CustomTableUI.rows.Count} {CustomTableUI.rows[0].Outline.Selected}");
        //        if (CustomTableUI.rows["hotbar"].Outline.Selected == true)
        //        {

        //            // Save the current state of the spriteBatch
        //            //SpriteSortMode originalSortMode = Main.spriteBatch.GraphicsDevice.BlendState == BlendState.AlphaBlend
        //            //? SpriteSortMode.Deferred
        //            //: SpriteSortMode.Immediate; // Example of how Terraria might be using it (you can customize this based on your needs)
        //            var originalBlendState = Main.spriteBatch.GraphicsDevice.BlendState;
        //            var originalSamplerState = Main.spriteBatch.GraphicsDevice.SamplerStates[0];
        //            var originalDepthStencilState = Main.spriteBatch.GraphicsDevice.DepthStencilState;
        //            var originalRasterizerState = Main.spriteBatch.GraphicsDevice.RasterizerState;

        //            //Texture2D sprite = TextureAssets.Item[item.type].Value;
        //            Texture2D spriteCopy = TextureAssets.Item[item.type].Value;

        //            Rectangle rect = new Rectangle(0, 0, sprite.Width, sprite.Height);

        //            float outlineWidth = ItemBorder.outlineWidth;

        //            bool normalRarity = true;
        //            Color abnormalColor = new Color(0, 0, 0);
        //            int definedAsSpecial = ItemBorder.itemDefinitions.FirstOrDefault(x => x == item.netID, 0);
        //            if (ItemBorder.specialPickup && definedAsSpecial != 0 && item.GetGlobalItem<GlobalItemBorderItem>().pickedUpBefore == GlobalItemBorderItem.PickupState.PickedUpFirstTime)
        //            {

        //                normalRarity = false;
        //                abnormalColor = ItemBorder.InvertMeAColour(Main.DiscoColor);//new Color(Main.DiscoG, Main.DiscoR, Main.masterColor);

        //            }
        //            #region CoinCheck
        //            else if (item.IsACoin)
        //            {
        //                switch (item.netID)
        //                {
        //                    case ItemID.CopperCoin:
        //                        normalRarity = false;
        //                        abnormalColor = new Color(183, 88, 25);
        //                        break;
        //                    case ItemID.SilverCoin:
        //                        normalRarity = false;
        //                        abnormalColor = new Color(124, 141, 142);
        //                        break;
        //                    case ItemID.GoldCoin:
        //                        normalRarity = false;
        //                        abnormalColor = new Color(148, 126, 24);
        //                        break;
        //                    case ItemID.PlatinumCoin:
        //                        normalRarity = false;
        //                        abnormalColor = new Color(136, 164, 176);
        //                        break;

        //                    default: break;
        //                }
        //            }
        //            #endregion
        //            else if (item.expert == true || (ItemBorder.useBaseRarity ? item.OriginalRarity == -12 : item.rare == -12))
        //            {
        //                normalRarity = false;
        //                abnormalColor = Main.DiscoColor;
        //            }
        //            else if (item.master || (ItemBorder.useBaseRarity ? item.OriginalRarity == -13 : item.rare == -13))
        //            {
        //                //Main.NewText($"{item.Name} {item.rare} {item.OriginalRarity} {ItemRarity.GetColor(item.rare)} ");
        //                normalRarity = false;
        //                abnormalColor = new Color(255, Main.masterColor, 0);//ItemRarity.GetColor(-13);
        //            }
        //            else if ((ItemBorder.useBaseRarity ? item.OriginalRarity : item.rare) >= ItemRarityID.Count)
        //            {
        //                ModRarity rarity = RarityLoader.GetRarity(ItemBorder.useBaseRarity ? item.OriginalRarity : item.rare);
        //                normalRarity = false;
        //                abnormalColor = rarity.RarityColor;
        //                //Main.NewText($"{item.Name} {rarity.RarityColor}");
        //            }

        //            Color trueSetColor = (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(ItemBorder.useBaseRarity ? item.OriginalRarity : item.rare);
        //            trueSetColor *= ItemBorder.borderOpacity;

        //            Vector2[] offsets = new Vector2[]
        //            {
        //        new Vector2(-outlineWidth,0),//LEFT
        //        new Vector2(outlineWidth,0),//RIGHT
        //        new Vector2(0,-outlineWidth),//UP
        //        new Vector2(0,outlineWidth),//DOWN
        //                                    //new Vector2(-outlineWidth,-outlineWidth),//TOPLEFT
        //                                    //new Vector2(outlineWidth,outlineWidth),//BOTTOMRIGHT
        //                                    //new Vector2(outlineWidth,-outlineWidth),//TOPRIGHT
        //                                    //new Vector2(-outlineWidth,outlineWidth),//BOTTOMLEFT
        //            };
        //            Main.spriteBatch.End();
        //            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, originalSamplerState, originalDepthStencilState, originalRasterizerState, ItemBorder.whiteEffect, Main.UIScaleMatrix);
                    
                    
        //            ItemBorder.whiteEffect.Parameters["CustomColor"].SetValue(trueSetColor.ToVector4());
        //            ItemBorder.whiteEffect.CurrentTechnique.Passes[0].Apply();

        //            foreach (Vector2 offset in offsets)
        //            {
        //                spriteBatch.Draw(spriteCopy,
        //                            position: position + offset + (new Vector2(52,52)*Main.inventoryScale/2),
        //                            sourceRectangle: frame,
        //                            color: trueSetColor,
        //                            rotation: 0f,
        //                            origin: origin,
        //                            scale: Main.inventoryScale,
        //                            SpriteEffects.None,
        //                            layerDepth: 0f);
        //            }
        //            Main.spriteBatch.End();
        //            Main.spriteBatch.Begin(SpriteSortMode.Immediate, originalBlendState, originalSamplerState, originalDepthStencilState, originalRasterizerState, null, Main.UIScaleMatrix);


        //            //foreach (Vector2 offset in offsets)
        //            //{
        //            //spriteBatch.Draw(spriteCopy,
        //            //            position: position,
        //            //            sourceRectangle: rect,
        //            //            color: Color.Black,
        //            //            rotation: 0f,
        //            //            origin: origin,
        //            //            scale: scale,
        //            //            SpriteEffects.None,
        //            //            layerDepth: 0f);
        //            //}
        //        }
        //    }
            
        //}
    }   
}