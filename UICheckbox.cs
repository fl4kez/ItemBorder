using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using ReLogic.Content;
using Terraria.ModLoader;

namespace ItemBorder
{
    public class UICheckbox : UIElement
    {
        public UIImageButton checkbox;
        private bool isChecked;
        private Asset<Texture2D> checkedTexture;
        private Asset<Texture2D> uncheckedTexture;
        private UIText labelText;

        public bool Selected => isChecked; // Property to get the checkbox state

        public UICheckbox(bool defaultState = false)
        {
            isChecked = defaultState;

            checkedTexture = ModContent.Request<Texture2D>("ItemBorder/assets/SettingsToggleOn", ReLogic.Content.AssetRequestMode.ImmediateLoad);
            uncheckedTexture = ModContent.Request<Texture2D>("ItemBorder/assets/SettingsToggleOff", ReLogic.Content.AssetRequestMode.ImmediateLoad);

            labelText = new UIText("", 0.7f);
            SetOnOffText();
            labelText.Left.Set(-25f, 0f); // Position the label to the right of the checkbox
            Append(labelText);

            checkbox = new UIImageButton(isChecked ? checkedTexture : uncheckedTexture);
            checkbox.SetVisibility(1, 1);
            checkbox.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) => { ToggleCheck(); };
            Width.Set(20f, 0f);
            Height.Set(20f, 0f);
            //checkbox.Width.Set(20f, 0f);
            //checkbox.Height.Set(20f, 0f);
            Append(checkbox);
            

            
        }

        //// Debug method to check if the element is receiving clicks
        //public override void LeftClick(UIMouseEvent evt)
        //{
        //    //base.LeftClick(evt);
        //    Main.NewText("UICheckbox clicked");
        //}

        public void SetOnOffText()
        {
            this.labelText.SetText(isChecked ? "On" : "Off");
        }

        public void ToggleCheck()
        {
            isChecked = !isChecked;
            checkbox.SetImage(isChecked ? checkedTexture : uncheckedTexture);
            //Main.NewText($"Changed outline to {isChecked}");
            //Main.NewText($"{CustomTableUI.rows[0].Label.Text} {CustomTableUI.rows[0].Outline.Selected} {CustomTableUI.rows[0].Border.Selected}");
            //Main.NewText($"{ReferenceEquals(CustomTableUI.rows[0].Outline,this)}");
            SetOnOffText();
            
        }

        public void SetState(bool state)
        {
            isChecked = state;
            checkbox.SetImage(isChecked ? checkedTexture : uncheckedTexture);
        }
    }
}
