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
using Terraria.ModLoader.Config;
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
            if(ItemBorder.IsSpecial(item) == null)
            {
                return true;
            }
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
        //public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        //{
        //    //Main.NewText($"{item.Name} is in{item.playerIndexTheItemIsReservedFor} {item.type} {item.netID}");
        //    Texture2D sprite = TextureAssets.Item[item.type].Value;
        //    if(item.Name == "Bee Keeper" || item.Name == "Mushroom")
        //    {
        //        //Main.NewText($"I:{item.Name} ({sprite.Width},{sprite.Height}) {position} scale:{scale}");
        //        //float scale2 = 1f;
        //        //float num = 1f;
        //        //if ((float)frame.Width > sizeLimit || (float)frame.Height > sizeLimit)
        //        //{
        //        //    num = ((frame.Width <= frame.Height) ? (sizeLimit / (float)frame.Height) : (sizeLimit / (float)frame.Width));
        //        //}
        //        //float finalDrawScale = scale * num * scale2;
        //        //sizeLimit = x
        //        //0.48 = x / 50
        //    }
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
        //                            position: position + offset,
        //                            sourceRectangle: frame,
        //                            color: trueSetColor,
        //                            rotation: 0f,
        //                            origin: origin,
        //                            scale: scale,
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
        //    //DRAW ITEM YOURSELF SO OPACITY IS 100% AND NOT 75%
        //    drawColor = new Color(drawColor.R, drawColor.G, drawColor.B, 255);
        //    spriteBatch.Draw(sprite,
        //                    position: position,
        //                    sourceRectangle: frame,
        //                    color: drawColor,
        //                    rotation: 0f,
        //                    origin: origin,
        //                    scale: scale,
        //                    SpriteEffects.None,
        //                    layerDepth: 0f);
        //    return false;
        //}
        
        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            //if(item.Name == "Stone Wall")
            //{
            //    Main.NewText($"{CustomTableUI.rows["wall"].WorldValue()} {ItemBorder.IsWall(item)}");
            //}

            if(ItemBorder.config.useWorld == false)
            {
                return true;
            }

            //POTION
            if (ItemBorder.potion.WorldValue() == false)
            {
                if (ItemBorder.IsPotion(item))
                    return true;
            }

            //TILE
            if (ItemBorder.tile.WorldValue() == false)
            {
                if (ItemBorder.IsTile(item))
                    return true;
            }

            //WALL
            if (ItemBorder.wall.WorldValue() == false)
            {
                if (ItemBorder.IsWall(item))
                    return true;
            }

            //MATERIAL
            if (ItemBorder.material.WorldValue() == false)
            {
                if (ItemBorder.IsMaterial(item))
                    return true;
            }
            var originalBlendState = Main.spriteBatch.GraphicsDevice.BlendState;
            var originalSamplerState = Main.spriteBatch.GraphicsDevice.SamplerStates[0];
            var originalDepthStencilState = Main.spriteBatch.GraphicsDevice.DepthStencilState;
            var originalRasterizerState = Main.spriteBatch.GraphicsDevice.RasterizerState;

            bool normalRarity = true;
            Color abnormalColor = new Color(0, 0, 0);
            ItemDefinition definedAsSpecial = ItemBorder.IsSpecial(item);
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
            else if (item.expert == true || (ItemBorder.config.worldBaseRarity ? item.OriginalRarity == -12 : item.rare == -12))
            {
                normalRarity = false;
                abnormalColor = Main.DiscoColor;
            }
            else if (item.master || (ItemBorder.config.worldBaseRarity ? item.OriginalRarity == -13 : item.rare == -13))
            {
                //Main.NewText($"{item.Name} {item.rare} {item.OriginalRarity} {ItemRarity.GetColor(item.rare)} ");
                normalRarity = false;
                abnormalColor = new Color(255, Main.masterColor, 0);//ItemRarity.GetColor(-13);
            }
            else if ((ItemBorder.config.worldBaseRarity ? item.OriginalRarity : item.rare) >= ItemRarityID.Count)
            {
                ModRarity rarity = RarityLoader.GetRarity(ItemBorder.config.worldBaseRarity ? item.OriginalRarity : item.rare);
                normalRarity = false;
                abnormalColor = rarity.RarityColor;
                //Main.NewText($"{item.Name} {rarity.RarityColor}");
            }

            Color trueSetColor = (normalRarity != true) ? abnormalColor : ItemRarity.GetColor(ItemBorder.config.worldBaseRarity ? item.OriginalRarity : item.rare);
            trueSetColor *= (float)ItemBorder.config.worldOpacity/100f;

            // Get the rarity color based on the item's rarity
            //Color outlineColor = GetRarityColor(item.rare);

            // Get the item's texture
            Texture2D texture = TextureAssets.Item[item.type].Value;
            Rectangle sourceRect = texture.Frame();
            bool itemHaveAnim = Main.itemAnimations[item.type] != null;
            if (itemHaveAnim)
                sourceRect = Main.itemAnimations[item.type].GetFrame(texture);
            if (item.Name == "Bee Keeper" || item.Name == "Copper Shortsword")
            {
                Main.NewText($"{sourceRect.Size()} {sourceRect.X} {sourceRect.Y} {texture.Size()} {trueSetColor} {alphaColor} {lightColor} {trueSetColor.MultiplyRGB(lightColor)}");
                //foreach(var type in Main.itemAnimationsRegistered)
                //{
                //    Main.NewText($"{type} {Main.item[type].Name}");
                //}
                
            }
            // Calculate the position to draw the outline
            Vector2 position = item.position - Main.screenPosition + new Vector2(item.width / 2, item.height - sourceRect.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, originalSamplerState, originalDepthStencilState, originalRasterizerState, ItemBorder.whiteEffect, Main.GameViewMatrix.TransformationMatrix);
            ItemBorder.whiteEffect.Parameters["CustomColor"].SetValue(trueSetColor.ToVector4());
            ItemBorder.whiteEffect.CurrentTechnique.Passes[0].Apply();

            Color clr = Lighting.GetColor(item.Center.ToTileCoordinates());
            
            // Draw the outline around the item
            DrawOutline(spriteBatch, texture, position, trueSetColor.MultiplyRGBA(clr), rotation, scale,sourceRect);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, originalBlendState, originalSamplerState, originalDepthStencilState, originalRasterizerState, null, Main.GameViewMatrix.TransformationMatrix);


            //DRAW ITEM YOURSELF SO OPACITY IS 100% AND NOT 75%
            Color drawColor = new Color(Color.White.R, Color.White.G, Color.White.B, 255);
            Vector2 origin = new Vector2((sourceRect.Width / 2), (sourceRect.Height / 2));
            //Vector2 positionReal = item.position - Main.screenPosition + new Vector2(item.width / 2, item.height - sourceRect.Height / 2);
            //spriteBatch.Draw(texture,
            //                position: position,
            //                sourceRectangle: sourceRect,
            //                color: lightColor,
            //                rotation: rotation,
            //                origin: origin,
            //                scale: scale,
            //                SpriteEffects.None,
            //                layerDepth: 0f);

            return true;
        }

        private void DrawOutline(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color outlineColor, float rotation, float scale, Rectangle sourceRect)
        {
            Vector2 origin = new Vector2((sourceRect.Width / 2), (sourceRect.Height / 2));

            // Define the offset vectors for the outline
            Vector2[] offsets = new Vector2[]
            {
            new Vector2(-ItemBorder.config.worldWidth, 0),  // Left
            new Vector2(ItemBorder.config.worldWidth, 0),   // Right
            new Vector2(0, -ItemBorder.config.worldWidth),  // Up
            new Vector2(0, ItemBorder.config.worldWidth),   // Down
            };
            
            // Draw the outline by drawing the sprite slightly offset in all directions
            foreach (Vector2 offset in offsets)
            {
                spriteBatch.Draw(texture, position + offset, sourceRect, outlineColor, rotation, origin, scale, SpriteEffects.None, 0f);
            }

            // Draw the actual item sprite
            //spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
