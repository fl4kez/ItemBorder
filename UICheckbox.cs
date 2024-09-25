using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using ReLogic.Content;
using Terraria.ModLoader;

namespace YourModNamespace
{
    public class UICheckbox : UIElement
    {
        public UIImageButton checkbox;
        private bool isChecked;
        private Asset<Texture2D> checkedTexture;
        private Asset<Texture2D> uncheckedTexture;
        private UIText labelText;

        public bool Selected => isChecked; // Property to get the checkbox state

        public override void LeftClick(UIMouseEvent evt)
        {
            SetState(!isChecked);
        }

        public UICheckbox(string label, bool defaultState = false)
        {
            isChecked = defaultState;

            checkedTexture = ModContent.Request<Texture2D>("ItemBorder/assets/SettingsToggleOn", ReLogic.Content.AssetRequestMode.ImmediateLoad);
            uncheckedTexture = ModContent.Request<Texture2D>("ItemBorder/assets/SettingsToggleOff", ReLogic.Content.AssetRequestMode.ImmediateLoad);

            checkbox = new UIImageButton(isChecked ? checkedTexture : uncheckedTexture);
            checkbox.OnLeftClick += ToggleCheck;
            Append(checkbox);

            //labelText = new UIText(label);
            //labelText.Left.Set(25f, 0f); // Position the label to the right of the checkbox
            //Append(labelText);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void ToggleCheck(UIMouseEvent evt, UIElement listeningElement)
        {
            isChecked = !isChecked;
            checkbox.SetImage(isChecked ? checkedTexture : uncheckedTexture);
        }

        public void SetState(bool state)
        {
            isChecked = state;
            checkbox.SetImage(isChecked ? checkedTexture : uncheckedTexture);
        }
    }
}
