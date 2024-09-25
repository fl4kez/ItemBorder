using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Config;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Emit;
using Terraria.ModLoader.UI;

namespace YourModNamespace
{
    [CustomModConfigItem(typeof(CustomTableUI))]
    public class CustomTableUI : ConfigElement
    {
        private UIPanel panel;
        private UIText label;
        private UICheckbox borderCheckbox;
        private UICheckbox outlineCheckbox;

        private UIText labelHeader;
        private UIText borderHeader;
        private UIText outlineHeader;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void OnInitialize()
        {
            //// Create the main panel
            //panel = new UIPanel();
            //panel.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) => { Main.NewText($"I clicked TablePanel"); };
            //panel.Width.Set(0, 1f);  // Full width
            //panel.Height.Set(300f, 0f);  // Set a fixed height for testing
            //panel.BackgroundColor = new Color(63, 82, 151) * 0.7f;  // Color to make sure it's visible
            //Append(panel);
            this.Height.Set(300f, 0f);
            //this.label.SetText("");
            //this.labelHeader.SetText("");
            //this.AddOrRemoveChild



            labelHeader = new UIText("Name", 0.9f);
            labelHeader.Left.Set(100f, 0f);
            Append(labelHeader);

            borderHeader = new UIText("Border", 0.9f);
            borderHeader.Left.Set(400f, 0f);
            Append(borderHeader);    

            outlineHeader = new UIText("Outline", 0.9f);
            outlineHeader.Left.Set(500f, 0f);
            Append(outlineHeader);

            // Add a simple label to the panel
            label = new UIText("Test Label",0.8f);
            //label.Height = this.Height;
            label.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) => { Main.NewText($"I clicked label"); };
            label.Left.Set(20f, 0f);  // Position it from the left
            label.Top.Set(10f, 0f);
            Append(label);

            // Add a checkbox for "Outline"
            outlineCheckbox = new UICheckbox("Outline", false);  // False for initial value
            //outlineCheckbox.Height = this.Height;
            outlineCheckbox.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) => { Main.NewText($"I clicked outlineCheckbox"); };
            outlineCheckbox.Left.Set(150f, 0f);  // Positioned to the right of the label
            outlineCheckbox.Top.Set(10f, 0f);
            Append(outlineCheckbox);

            // Add a checkbox for "Border"
            borderCheckbox = new UICheckbox("Border", false);  // False for initial value
            //borderCheckbox.Height = this.Height;
            borderCheckbox.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) => { Main.NewText($"I clicked borderCheckbox"); };
            borderCheckbox.Left.Set(250f, 0f);  // Positioned to the right of the Outline checkbox
            borderCheckbox.Top.Set(10f, 0f);
            Append(borderCheckbox);

            Main.NewText("Panel initialized");
            Main.NewText("Row added: " + label);
            
        }
    }
}
