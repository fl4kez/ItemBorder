using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace ItemBorder
{
    public class GlobalItemBorderItem : GlobalItem
    {
        public enum PickupState
        {
            NeverSeen = 0,
            PickedUpFirstTime = 1,
            NotFirstTime = 2
        }
        public PickupState pickedUpBefore = PickupState.NeverSeen;

        public override bool InstancePerEntity => true;
        public override bool OnPickup(Item item, Player player)
        {
            if (pickedUpBefore == PickupState.NeverSeen)
                pickedUpBefore = PickupState.PickedUpFirstTime;
            else
                pickedUpBefore = PickupState.NotFirstTime;
            return true;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("pickup", (int)pickedUpBefore);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if(tag.ContainsKey("pickup"))
            {
                pickedUpBefore = (PickupState)tag.GetInt("pickup");
            }
        }

        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (ItemBorder.useOutline == true)
            {
                Texture2D sprite = TextureAssets.Item[item.type].Value;
                Texture2D copy = null;
                ItemBorder.RunOnMainThread(() =>
                {
                    copy = new Texture2D(Main.graphics.GraphicsDevice, sprite.Width, sprite.Height);

                    Color[] data = new Color[sprite.Width * sprite.Height];
                    sprite.GetData(data);
                    // cor nova
                    Color novaCor = Color.White;

                    for (int i = 0; i < data.Length; i++)
                    {
                        // include your RGB color
                        if (data[i].A != 0)
                        {
                            data[i] = novaCor;
                        }
                    }
                    copy.SetData<Color>(data);
                });
                Rectangle rect = new Rectangle(0, 0, sprite.Width, sprite.Height);

                float outlineWidth = ItemBorder.outlineWidth;

                bool normalRarity = true;
                Color abnormalColor = new Color(0, 0, 0);
                if (ItemBorder.specialPickup)
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
                else if ((ItemBorder.useBaseRarity?item.OriginalRarity:item.rare) >= ItemRarityID.Count)
                {
                    ModRarity rarity = RarityLoader.GetRarity(ItemBorder.useBaseRarity?item.OriginalRarity:item.rare);
                    normalRarity = false;
                    abnormalColor = rarity.RarityColor;
                    //Main.NewText($"{item.Name} {rarity.RarityColor}");
                }

                Color trueSetColor = (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(ItemBorder.useBaseRarity?item.OriginalRarity:item.rare);
                trueSetColor *= ItemBorder.borderOpacity;

                Vector2[] offsets = new Vector2[]
                {
                new Vector2(-outlineWidth,0),//LEFT
                new Vector2(outlineWidth,0),//RIGHT
                new Vector2(0,-outlineWidth),//UP
                new Vector2(0,outlineWidth),//DOWN
                new Vector2(-outlineWidth,-outlineWidth),//TOPLEFT
                new Vector2(outlineWidth,outlineWidth),//BOTTOMRIGHT
                new Vector2(outlineWidth,-outlineWidth),//TOPRIGHT
                new Vector2(-outlineWidth,outlineWidth),//BOTTOMLEFT
                };
                foreach (Vector2 offset in offsets)
                {
                    spriteBatch.Draw(copy,
                                position: position + offset,
                                sourceRectangle: rect,
                                color: trueSetColor,
                                rotation: 0f,
                                origin: origin,
                                scale: scale,
                                SpriteEffects.None,
                                layerDepth: 0f);
                }
                //foreach (Vector2 offset in offsets)
                //{
                    //spriteBatch.Draw(spriteCopy,
                    //            position: position,
                    //            sourceRectangle: rect,
                    //            color: Color.Black,
                    //            rotation: 0f,
                    //            origin: origin,
                    //            scale: scale,
                    //            SpriteEffects.None,
                    //            layerDepth: 0f);
                //}
            }
            return true;
        }

        /// <summary>
        /// IL SPY TEST
        /// </summary>
        /// <param name="item"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="frame"></param>
        /// <param name="drawColor"></param>
        /// <param name="itemColor"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        //public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        //{
        //    //Item texture
        //    Texture2D value6 = TextureAssets.Item[item.type].Value;
        //    if (item.Name == "Solar Flare Drill")
        //    {
        //        Main.NewText($"position2:{position} rectangle2:{frame} num8*scale3:{scale} invScale:{Main.inventoryScale}");
        //    }


        //    bool normalRarity = true;
        //    Color abnormalColor = new Color(0, 0, 0);

        //    #region CoinCheck
        //    if (item.IsACoin)
        //    {
        //        switch (item.netID)
        //        {
        //            case ItemID.CopperCoin:
        //                normalRarity = false;
        //                abnormalColor = new Color(183, 88, 25);
        //                break;
        //            case ItemID.SilverCoin:
        //                normalRarity = false;
        //                abnormalColor = new Color(124, 141, 142);
        //                break;
        //            case ItemID.GoldCoin:
        //                normalRarity = false;
        //                abnormalColor = new Color(148, 126, 24);
        //                break;
        //            case ItemID.PlatinumCoin:
        //                normalRarity = false;
        //                abnormalColor = new Color(136, 164, 176);
        //                break;

        //            default: break;
        //        }
        //    }
        //    #endregion
        //    else if (item.expert == true || item.rare == -12)
        //    {
        //        normalRarity = false;
        //        abnormalColor = Main.DiscoColor;
        //    }
        //    else if (item.master || item.rare == -13)
        //    {
        //        //Main.NewText($"{item.Name} {item.rare} {item.OriginalRarity} {ItemRarity.GetColor(item.rare)} ");
        //        normalRarity = false;
        //        abnormalColor = new Color(255, Main.masterColor, 0);//ItemRarity.GetColor(-13);
        //    }
        //    else if (item.rare >= ItemRarityID.Count)
        //    {
        //        ModRarity rarity = RarityLoader.GetRarity(item.rare);
        //        normalRarity = false;
        //        abnormalColor = rarity.RarityColor;
        //        //Main.NewText($"{item.Name} {rarity.RarityColor}");
        //    }

        //    float num10 = scale / Main.inventoryScale;
        //    if (frame.Width > 32 || frame.Height > 32)
        //    {
        //        num10 = ((frame.Width <= frame.Height) ? (32f / (float)frame.Height) : (32f / (float)frame.Width));

        //    }
        //    //Main.NewText($"{item.Name} {Main.inventoryScale} {scale}");
        //    Texture2D value = TextureAssets.InventoryBack9.Value;
        //    Vector2 vector = value.Size() * Main.inventoryScale;
        //    Vector2 positionReal = position - (vector / 2f - frame.Size() * num10 / 2f);

        //    Color trueSetColor = (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(item.rare);
        //    trueSetColor *= ItemBorder.borderOpacity;

        //    spriteBatch.Draw(ModContent.Request<Texture2D>($"ItemBorder/assets/border_new11").Value,
        //                position: positionReal,
        //                sourceRectangle: new Rectangle(0, 0, 52, 52),
        //                color: trueSetColor,
        //                rotation: 0f,
        //                origin: Vector2.Zero,
        //                scale: scale,
        //                //((context == ItemSlot.Context.CraftingMaterial) ? ((Main.LocalPlayer.HeldItem == item)?0f:0.15f) : 0),
        //                SpriteEffects.None,
        //                layerDepth: 0f);
        //    return true;
        //}





        /*public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            bool normalRarity = true;
            Color abnormalColor = new Color(0, 0, 0);
            if (item.expert == true)
            {
                normalRarity = false;
                abnormalColor = Main.DiscoColor;
            }
            else if (item.master)
            {
                //Main.NewText($"{item.Name} {item.rare} {item.OriginalRarity} {ItemRarity.GetColor(item.rare)} ");
                normalRarity = false;
                abnormalColor = ItemRarity.GetColor(-13);
            }
            float proportionX = 32 / item.Size.X;
            float proportionY = 32 / item.Size.Y;

            
            //Main.NewText($"{Main.Camera.ScaledPosition} {Main.screenPosition}");
            //item.
            Vector2 actualPos = item.position - Main.screenPosition;

            
            //Main.NewText($"{item.Name} {item.position} {actualPos} ||| {item.Size} {reduced} {item.scale} {posOffset} {test}");
            if(item.Name != "Fallen Star")
            Main.NewText($"I:{item.Name} Pos{item.position} Center{item.Center} Diff{item.Center - item.position} Size:{item.Size} {item.Hitbox}");



            spriteBatch.Draw(
                ModContent.Request<Texture2D>($"ItemBorder/assets/itemBorderWhite{ItemBorder.borderType}").Value,
                        position: actualPos - (new Vector2(Math.Abs(item.Center.X - item.position.X), Math.Abs(item.Center.Y - item.position.Y))/2),//-
                        //((item.Size.X > 16 || item.Size.Y > 16)?new Vector2(item.Size.X/2,item.Size.Y/2): new Vector2(item.Size.X/1.25f, item.Size.Y/1.25f))
                        //-((item.Size.X > 16 && item.Size.Y > 16)?new Vector2(Math.Abs(32-item.Size.X),Math.Abs(32 -item.Size.Y)):Vector2.Zero),//+(lel/(item.Size.X - item.Size.Y)/16),//+new Vector2(item.Size.X-32,item.Size.Y-32),//-posOffset,
                        sourceRectangle: new Rectangle(0, 0, 32, 32),
                        color: (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(item.rare),
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: 1.35f / ((proportionX + proportionY) / 2),// * (1+((proportionX + proportionY)/2)), //* minusScale, //+ (item.scale - 1) - minusScale,
                        //((context == ItemSlot.Context.CraftingMaterial) ? ((Main.LocalPlayer.HeldItem == item)?0f:0.15f) : 0),
                        SpriteEffects.None,
                        layerDepth: 0f
                );
            //Utils.DrawRect(spriteBatch, item.Hitbox, (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(item.rare));
        }*/


        //public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        //{
        //    //base.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        //    //int slot = -1;
        //    //for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
        //    //{
        //    //    if (Main.LocalPlayer.inventory[i] == item)
        //    //    {
        //    //        slot = i;
        //    //    }
        //    //}


        //    Vector2 bonusTest = new Vector2(frame.Size().X-16, frame.Size().Y-16)/2;

        //    Vector2 evenMore = new Vector2(frame.Size().X - 32, frame.Size().Y - 32);

        //    if (frame.Size().X > 32)
        //        evenMore.X /= 2;
        //    if (frame.Size().Y > 32)
        //        evenMore.Y /= 2;

        //    float realScale = 1.55f;

        //    bool isAmmo = false;
        //    for (int i = 54; i < 58; i++) //AMMO SLOT
        //    {
        //        if (Main.LocalPlayer.inventory[i].Name == item.Name)
        //        {
        //            isAmmo = true;
        //            break;
        //        }
        //    }


        //    Main.NewText($"{item.Name} {item.Size} {frame.Size()} {Main.inventoryScale} {new Vector2(frame.Size().X - 16, frame.Size().Y - 16) + bonusTest + evenMore}");

        //    spriteBatch.Draw(ModContent.Request<Texture2D>($"ItemBorder/assets/itemBorderWhite4").Value,
        //            position: position-new Vector2(frame.Size().X-16,frame.Size().Y-16)+bonusTest+evenMore,
        //            sourceRectangle: new Rectangle(0, 0, 32, 32),
        //            color: (item.rare == -12) ? Main.DiscoColor : ItemRarity.GetColor(item.rare),
        //            rotation: 0f,
        //            origin: Vector2.Zero,
        //            scale: (realScale)*Main.inventoryScale,
        //            SpriteEffects.None,
        //            layerDepth: 0f);
        //    //Vector2 siz = frame.Size();

        //    //int modX = ((int)siz.X - 16) % 16;
        //    //int modY = ((int)siz.Y - 16) % 16;

        //    //Vector2 offset = new Vector2((siz.X / 16) + modX, (siz.Y / 16) + modY);

        //    //float x = 32 - (siz.X - (siz.X / 2));
        //    //float y = 32 - (siz.Y - (siz.Y / 2));
        //    //Vector2 allTest = new Vector2(x, y);


        //    //int moduloX = (int)siz.X % 16;
        //    ////if (moduloX == siz.X)
        //    ////    moduloX = 16 - (int)siz.X;

        //    //int moduloY = (int)siz.Y % 16;
        //    ////if (moduloY == siz.Y)
        //    ////    moduloY = 16 - (int)siz.Y;
        //    //allTest.X += moduloX;
        //    //allTest.Y += moduloY;

        //    ////allTest /= 2;
        //    //Main.NewText($"{item.Name} {slot} {allTest} {frame}");








        //    ////ITEM-SOLO-TESTS
        //    //Vector2 itm = Vector2.Zero;

        //    ///*if(item.Name == "Solar Flare Hamaxe")
        //    //{
        //    //    itm.X = 8;
        //    //    itm.Y = 6;
        //    //}
        //    //else if (item.Name == "Solar Flare Drill")
        //    //{
        //    //    itm.X = 6;
        //    //    itm.Y = 14;
        //    //}
        //    //else if (item.Name == "Ironskin Potion")
        //    //{
        //    //    itm.X = 12;
        //    //    itm.Y = 14;
        //    //}
        //    //else if (item.Name == "Demon Bow")
        //    //{
        //    //    itm.X = 2;
        //    //    itm.Y = 8;
        //    //}*/
        //    //itm.X = (int)siz.X % 16;
        //    ////if (siz.X <= ((int)(siz.X / 16)*16) && itm.X != 0)
        //    ////    itm.X = ((int)(siz.X / 16) * 16);

        //    //itm.Y = (int)siz.Y % 16;
        //    ////if (siz.Y <= ((int)(siz.Y / 16) * 16) && itm.Y != 0)
        //    ////    itm.Y = ((int)(siz.Y / 16) * 16);

        //    //if (itm.X == 0)
        //    //{ 
        //    //    itm.X += (siz.X / 16)/siz.X;
        //    //}
        //    //if (itm.Y == 0)
        //    //{
        //    //    itm.Y += (siz.Y / 16)/siz.Y;
        //    //}


        //    //spriteBatch.Draw(ModContent.Request<Texture2D>($"ItemBorder/assets/itemBorderWhite{borderType}").Value,
        //    //    position: position-itm,
        //    //    sourceRectangle: new Rectangle(0, 0, 32, 32),
        //    //    color: (item.rare == -12) ? Main.DiscoColor : ItemRarity.GetColor(item.rare),
        //    //    rotation: 0f,
        //    //    origin: Vector2.Zero,
        //    //    scale: 0.9f / Main.inventoryScale,
        //    //    SpriteEffects.None,
        //    //    layerDepth: 0f);
        //}
        //REVERSING 1.4 CODE

        //public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        //{
        //    //base.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        //    Vector2 vector = item.Size * Main.inventoryScale;
        //    if(item.type > 0 && item.stack > 0)
        //    {
        //        Rectangle rect = 
        //    }
        //}

        /*public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (item.Name == "Iron Shortsword" || item.Name == "Stardust Pickaxe" || item.Name == "Magic Mirror")
            {
                Main.NewText($"I:{item.Name} | {position} {frame.Size()} | Size:{item.Size} Center:{item.Center} Scale:{scale} {position - new Vector2(8, 8)} {Main.MouseScreen}");
            }
            if (!item.IsACoin && !item.Name.ToLower().Contains("potion") && !item.potion && item.ammo == 0 && item.createTile == -1 && !item.consumable)
                if (item.Name == "Worm Scarf")
                    Main.NewText($"{item.rare} {item.OriginalRarity}");



            //-----------------TESTING DETOUR


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
            //    isPotion = !IsPotion(item);
            }
            //TILE
        bool isTile = true;
            if (!useTiles)
            {
                isTile = (item.createTile == -1);
            }
            Main.NewText(Main.LocalPlayer.HeldItem.Name);

            if (!item.IsACoin && isPotion && isTile && !isAmmo && Main.LocalPlayer.HeldItem != item)
            {
                float proportionX = 32 / frame.Size().X;
                float proportionY = 32 / frame.Size().Y;

                spriteBatch.Draw(ModContent.Request<Texture2D>($"ItemBorder/assets/itemBorderWhite{borderType}").Value,
                    position: position,
                    sourceRectangle: new Rectangle(0, 0, 32, 32),
                    color: (item.rare == -12) ? Main.DiscoColor : ItemRarity.GetColor(item.rare),
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1f,
                    SpriteEffects.None,
                    layerDepth: 0f);

                if (Main.LocalPlayer.HeldItem.Name == item.Name)
                {
                    Main.NewText($"{scale} {frame.Size()} {item.ammo} {item.FitsAmmoSlot()} {item.createTile}");
                    Main.NewText($"{item.healLife} {item.healMana} {item.buffType} {item.potion}");
                }
            }
            return true;
        }
            //return true;

            //--------------END TESTING DETOUR
            //}
            /*public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
            {
                //Utils.DrawLine(spriteBatch, new Point((int)position.X, (int)position.Y), new Point((int)position.X + 24, (int)position.Y + 24), Color.Red);
                spriteBatch.Draw(ModContent.Request<Texture2D>("ItemBorder/assets/itemBorderWhite").Value,
                    position, Color.Red);
            }*/
        /*public bool IsPotion(Item item)
        {
            bool mightBeAPotion = item.healLife > 0 || item.healMana > 0 || item.buffType > 0 || item.potion;

            if (!mightBeAPotion)
                return false;

            return item.consumable && item.UseSound is SoundStyle style && (style.IsTheSameAs(SoundID.Item2) || style.IsTheSameAs(SoundID.Item3));
        }*/
    }
    }
