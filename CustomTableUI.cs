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

        //private Asset<Texture2D> checkedTexture;
        //private Asset<Texture2D> uncheckedTexture;

        float baseHeight;

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
            borderHeader.Left.Set(400f, 0f);
            Append(borderHeader);

            outlineHeader = new UIText("Outline", 0.8f);
            outlineHeader.Left.Set(500f, 0f);
            Append(outlineHeader);

            // Example: Add a default row
            AddCustomizationRow("hotbar","Use for hotbar", true, true);
            //AddCustomizationRow("Test Label 2", false, false);
        }

        float offsetValue = 15f;
        int rowIndex => (Children.Count() - 3) / 3;

        public void AddCustomizationRow(string KEY,string labelText, bool initialBorderState, bool initialOutlineState)
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
            UIText label = new UIText(labelText, 0.7f);
            label.Left.Set(100f, 0f);
            label.Top.Set(15f + offsetValue * rowIndex, 0f);  // Adjust the position based on the number of rows
            Append(label);

            // Checkbox for "Border"
            UICheckbox borderCheckbox = new UICheckbox(initialBorderState);
            borderCheckbox.Left.Set(425f, 0f);
            borderCheckbox.Top.Set(15f + offsetValue * rowIndex, 0f);
            Append(borderCheckbox);

            // Checkbox for "Outline"
            UICheckbox outlineCheckbox = new UICheckbox(initialOutlineState);
            outlineCheckbox.Left.Set(525f, 0f);
            outlineCheckbox.Top.Set(15f + offsetValue * rowIndex, 0f);
            Append(outlineCheckbox);

            TableRowConfig row = new TableRowConfig(KEY,label, borderCheckbox, outlineCheckbox);
            rows.Add(row);
        }
        public List<TableRowConfig> rows = new List<TableRowConfig>();

        public CustomTableUI()
        {
        }
    }
    
}
