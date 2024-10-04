using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Config;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Linq;
using System.Collections.Generic;
using ItemBorder;

namespace ItemBorder
{
    [CustomModConfigItem(typeof(CustomTableUI))]
    public class CustomTableUI : ConfigElement
    {
        private UIText labelHeader;
        private UIText borderHeader;
        private UIText outlineHeader;
        private UIText worldHeader;

        //private Asset<Texture2D> checkedTexture;
        //private Asset<Texture2D> uncheckedTexture;

        float baseHeight;

        static bool initializedRows = false;

        public override void OnBind()
        {
            base.OnBind();
        }

        
        //protected override void DrawSelf(SpriteBatch spriteBatch)
        //{
        //    base.DrawSelf(spriteBatch);
        //    Main.NewText($"{rows["hotbar"].Outline.GetDimensions().X} {rows["hotbar"].Outline.GetDimensions().Y}");
        //    Utils.DrawLine(spriteBatch, new Point(600, 400), new Point(1200, 400), Color.White);
        //    Utils.DrawLine(spriteBatch, new Point(600, 500), new Point(1200, 500), Color.Yellow);
        //    Utils.DrawLine(spriteBatch, new Point(600, 600), new Point(1200, 600), Color.White);
        //    Utils.DrawLine(spriteBatch, new Point(600, 700), new Point(1200, 700), Color.Yellow);
        //    Utils.DrawLine(spriteBatch, new Point(600, 800), new Point(1200, 800), Color.White);

        //}
        public override void OnInitialize()
        {
            baseHeight = Height.Pixels;


            //checkedTexture = ModContent.Request<Texture2D>("ItemBorder/assets/SettingsToggleOn", ReLogic.Content.AssetRequestMode.ImmediateLoad);
            //uncheckedTexture = ModContent.Request<Texture2D>("ItemBorder/assets/SettingsToggleOff", ReLogic.Content.AssetRequestMode.ImmediateLoad);

            // Initialize headers
            labelHeader = new UIText("Name", 0.8f);
            labelHeader.Left.Set(100f, 0f);
            Append(labelHeader);

            borderHeader = new UIText("Border", 0.8f);
            borderHeader.Left.Set(300f, 0f);
            Append(borderHeader);

            outlineHeader = new UIText("Outline", 0.8f);
            outlineHeader.Left.Set(400f, 0f);
            Append(outlineHeader);

            worldHeader = new UIText("World", 0.8f);
            worldHeader.Left.Set(500f, 0f);
            Append(worldHeader);


            bool skip = true;
            foreach (var row in rows)
            {
                if (skip)
                { 
                    AddCustomizationRow(row.Value, true);
                    skip = false;
                    continue;
                }

                AddCustomizationRow(row.Value,false);
            }
            //AddCustomizationRow("Test Label 2", false, false);
        }

        float offsetValue = 20f;
        int rowIndex = 0;

        public void AddCustomizationRow(TableRowConfig tableRow,bool skipSeperator)
        {
            //int rowIndex = (Children.Count() - 3) / 3;  // Calculate row index based on current number of rows


            if (this.Parent != null)
            {
                this.Parent.Height.Set(baseHeight + (offsetValue * (rowIndex)), 0);
                this.Parent.Recalculate();
                this.Height.Set(baseHeight + (offsetValue * (rowIndex)), 0);
                //this.Recalculate();
                //this.Parent.Recalculate();
                this.Parent.RecalculateChildren();
            }



            //Main.NewText($"RowIndex: {rowIndex}");
            //Main.NewText($"{this.Parent.GetDimensions().Y}");
            //Main.NewText($"{this.GetDimensions().Y}");

            // Label for the row
            //tableRow.Label.Left.Set(100f, 0f);
            

            tableRow.Label.Top.Set(15f + offsetValue * rowIndex, 0f);
            Append(tableRow.Label);

            // Checkbox for "Border"
            if (tableRow.Border.Use)
            {
                tableRow.Border.Value.Top.Set(15f + offsetValue * rowIndex, 0f);
                Append(tableRow.Border.Value);
            }

            // Checkbox for "Outline"
            if (tableRow.Outline.Use)
            {
                tableRow.Outline.Value.Top.Set(15f + offsetValue * rowIndex, 0f);
                Append(tableRow.Outline.Value);
            }

            //Checkbox for "World"
            if (tableRow.World.Use)
            {
                tableRow.World.Value.Top.Set(15f + offsetValue * rowIndex, 0f);
                Append(tableRow.World.Value);
            }

            if (skipSeperator)
            {
                //UIHorizontalSeparator line = new UIHorizontalSeparator(600, false);
                //line.Top.Set(15f + offsetValue * (rowIndex), 0f);
                //line.MinHeight.Set(0, 0);
                //line.Height.Set(0, 0);
                ////line.MinWidth.Set(0, 0);
                ////line.Width.Set(0, 0);

                //line.IgnoresMouseInteraction = true;
                ////line.Left.Set(525f, 0f);

                //Append(line);
            }

            //TableRowConfig row = new TableRowConfig(KEY,label, borderCheckbox, outlineCheckbox);
            //rows.Add(row);
            rowIndex++;
        }
        public void AddCustomizationRowToList(string KEY, string labelText, 
            TableRowConfig.BoolColumn border, TableRowConfig.BoolColumn outline, TableRowConfig.BoolColumn world)
        {
            //int rowIndex = (Children.Count() - 3) / 3;  // Calculate row index based on current number of rows





            //Main.NewText($"RowIndex: {rowIndex}");
            //Main.NewText($"{this.Parent.GetDimensions().Y}");
            //Main.NewText($"{this.GetDimensions().Y}");

            // Label for the row
            UIText label = new UIText(labelText, 0.7f);
            label.Left.Set(100f, 0f);
            label.Top.Set(15f + offsetValue * rowIndex, 0f);  // Adjust the position based on the number of rows
            //Append(label);

            // Checkbox for "Border"
            UICheckbox borderCheckbox = new UICheckbox(border.DefaultValue);
            borderCheckbox.Left.Set(325f, 0f);
            borderCheckbox.Top.Set(15f + offsetValue * rowIndex, 0f);
            if (border.Use)
            {
                border.Value = borderCheckbox;
            }

            // Checkbox for "Outline"
            UICheckbox outlineCheckbox = new UICheckbox(outline.DefaultValue);
            outlineCheckbox.Left.Set(425f, 0f);
            outlineCheckbox.Top.Set(15f + offsetValue * rowIndex, 0f);
            if (outline.Use)
            {
                outline.Value = outlineCheckbox;
            }

            UICheckbox worldCheckbox = new UICheckbox(world.DefaultValue);
            worldCheckbox.Left.Set(525f, 0f);
            worldCheckbox.Top.Set(15f + offsetValue * rowIndex, 0f);
            if (world.Use)
            {
                world.Value = worldCheckbox;
            }


            TableRowConfig row = new TableRowConfig(label, border, outline, world);
            rows.Add(KEY,row);
        }
        public static Dictionary<string,TableRowConfig> rows = new Dictionary<string, TableRowConfig>();
    }
    
}
