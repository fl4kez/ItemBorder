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

namespace ItemBorder
{
    public class ItemBorder : Mod
    {
        public static List<int> itemDefinitions;
        public static bool usePotions, useTiles;
        public static int borderType;
        internal static bool useForHotbar;
        internal static TableRowConfig hotbar => CustomTableUI.rows["hotbar"];
        internal static bool useForLightPet;
        internal static TableRowConfig lightPet => CustomTableUI.rows["lightPet"];
        internal static bool useForPet;
        internal static TableRowConfig pet => CustomTableUI.rows["pet"];
        internal static bool useForMinecart;
        internal static TableRowConfig minecart => CustomTableUI.rows["minecart"];
        internal static bool useForMount;
        internal static TableRowConfig mount => CustomTableUI.rows["mount"];
        internal static bool useForGrapple;
        internal static TableRowConfig grapple => CustomTableUI.rows["grapple"];
        internal static bool useForShop;
        internal static TableRowConfig shop => CustomTableUI.rows["shop"];
        internal static bool useForDye;
        internal static TableRowConfig dye => CustomTableUI.rows["dye"];
        internal static bool useForVanityAccessory;
        internal static TableRowConfig vanityAccessory => CustomTableUI.rows["svanityAccessoryhop"];
        internal static bool useForAccessory;
        internal static TableRowConfig accessory => CustomTableUI.rows["accessory"];
        internal static bool useForVanityArmor;
        internal static TableRowConfig vanityArmor => CustomTableUI.rows["vanityArmor"];
        internal static bool useForArmor;
        internal static TableRowConfig armor => CustomTableUI.rows["armor"];
        internal static bool useForTrash;
        internal static TableRowConfig trash => CustomTableUI.rows["trash"];
        internal static bool useForCoin;
        internal static TableRowConfig coin => CustomTableUI.rows["coin"];
        internal static bool useForAmmo;
        internal static TableRowConfig ammo => CustomTableUI.rows["ammo"];
        internal static bool useForInventory;
        internal static TableRowConfig inventory => CustomTableUI.rows["inventory"];
        internal static bool useForChests;
        internal static TableRowConfig chests => CustomTableUI.rows["chests"];

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

        public static bool usingMagicalStorage;
        public static float borderOpacity;
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
            //Texture2D lel = TextureAssets.Extra[54].Value;
            //lel.SaveAsJpeg(File.Create("lel_texture.jpg"), lel.Width, lel.Height);


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
            //config.ConfigTable.Add(new TableRowConfig("My First Label"));
            //config.ConfigTable.Add(new TableRowConfig("My Second Label"));
            //config.ConfigTable.Add(new TableRowConfig("My Third Label"));
            config.ConfigTable.AddCustomizationRowToList("hotbar", "Use for hotbar", Column(true,true), Column(true, true), Column(false, false));
            config.ConfigTable.AddCustomizationRowToList("shop", "Use for shop", Column(true, true), Column(true, true), Column(false, false));
        }

        public TableRowConfig.BoolColumn Column(bool use, bool defaultVal)
        {
            return new TableRowConfig.BoolColumn(use, defaultVal);
        }

        private float CorrectlyDrawOutline(On_ItemSlot.orig_DrawItemIcon orig, Item item, int context, SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, float scale, float sizeLimit, Color environmentColor)
        {
            Texture2D sprite = TextureAssets.Item[item.type].Value;
            Rectangle frame = new Rectangle(0, 0, sprite.Width, sprite.Height);
           
            Vector2 origin = new Vector2(frame.Width / 2, frame.Height / 2);
            if (CustomTableUI.rows.Count > 0)
            {
                //Main.NewText($"{CustomTableUI.rows.Count} {CustomTableUI.rows[0].Outline.Selected}");
                if (CustomTableUI.rows["hotbar"].OutlineValue() == true)
                {

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

                    float outlineWidth = ItemBorder.outlineWidth;

                    bool normalRarity = true;
                    Color abnormalColor = new Color(0, 0, 0);
                    int definedAsSpecial = ItemBorder.itemDefinitions.FirstOrDefault(x => x == item.netID, 0);
                    if (ItemBorder.specialPickup && definedAsSpecial != 0 && item.GetGlobalItem<GlobalItemBorderItem>().pickedUpBefore == GlobalItemBorderItem.PickupState.PickedUpFirstTime)
                    {

                        normalRarity = false;
                        abnormalColor = ItemBorder.InvertMeAColour(Main.DiscoColor);//new Color(Main.DiscoG, Main.DiscoR, Main.masterColor);

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
                    else if (item.expert == true || (ItemBorder.useBaseRarity ? item.OriginalRarity == -12 : item.rare == -12))
                    {
                        normalRarity = false;
                        abnormalColor = Main.DiscoColor;
                    }
                    else if (item.master || (ItemBorder.useBaseRarity ? item.OriginalRarity == -13 : item.rare == -13))
                    {
                        //Main.NewText($"{item.Name} {item.rare} {item.OriginalRarity} {ItemRarity.GetColor(item.rare)} ");
                        normalRarity = false;
                        abnormalColor = new Color(255, Main.masterColor, 0);//ItemRarity.GetColor(-13);
                    }
                    else if ((ItemBorder.useBaseRarity ? item.OriginalRarity : item.rare) >= ItemRarityID.Count)
                    {
                        ModRarity rarity = RarityLoader.GetRarity(ItemBorder.useBaseRarity ? item.OriginalRarity : item.rare);
                        normalRarity = false;
                        abnormalColor = rarity.RarityColor;
                        //Main.NewText($"{item.Name} {rarity.RarityColor}");
                    }

                    Color trueSetColor = (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(ItemBorder.useBaseRarity ? item.OriginalRarity : item.rare);
                    trueSetColor *= ItemBorder.borderOpacity;

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
                }
            }
            orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
            return scale;
        }

        ItemBorderConfig config;

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
            int definedAsSpecial = itemDefinitions.FirstOrDefault(x => x == inv[slot].netID,0);
            Item item = inv[slot];
            if (definedAsSpecial != 0 && Main.mouseItem.netID != inv[slot].netID)
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
            if (useBorder == true)
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
                    //bool isMagicStorageSlot = false;
                    //if(usingMagicalStorage)
                    //{
                    //    isMagicStorageSlot = new StackTrace().GetFrames().Any(f => f.GetMethod()?.DeclaringType == magicalStorage.Code.GetType("MagicStorage.UI.MagicStorageItemSlot"));
                    //}


                    //float proportionX = 32 / frame.Size().X;
                    //float proportionY = 32 / frame.Size().Y;

                    bool normalRarity = true;
                    Color abnormalColor = new Color(0,0,0);
                    int definedAsSpecial = itemDefinitions.FirstOrDefault(x => x == inv[slot].netID, 0);
                    if (specialPickup && definedAsSpecial != 0 && item.GetGlobalItem<GlobalItemBorderItem>().pickedUpBefore == GlobalItemBorderItem.PickupState.PickedUpFirstTime)
                    {
                       
                        //Main.NewText($"{inv[slot].Name} {definedAsSpecial}");
                        
                            
                        normalRarity = false;
                        //Main.NewText($"Drawing special item border {item.Name}");
                        abnormalColor = ItemBorder.InvertMeAColour(Main.DiscoColor);//new Color(Main.DiscoG, Main.DiscoR, Main.masterColor);
                            
                        
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