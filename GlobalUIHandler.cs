using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ItemBorder
{
    public class GlobalUIHandler : ModSystem
    {
        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);

            //DetectHoveredElement();
        }

        private void DetectHoveredElement()
        {
            // Get all the UI elements in the game
            List<UIElement> allElements = GetAllUIElements();

            foreach (UIElement element in allElements)
            {
                if (element.ContainsPoint(new Vector2(Main.mouseX, Main.mouseY)))
                {
                    // Print to chat the type of element hovered over
                    //Main.NewText($"Hovering over: {element.GetType().Name}", 255, 255, 0);

                    // Optional: Break after finding the first hovered element to avoid multiple messages
                    break;
                }
            }
        }

        private List<UIElement> GetAllUIElements()
        {
            List<UIElement> allElements = new List<UIElement>();

            // Add logic to gather all UI elements.
            // For example, you could iterate through the `Main.IngameUI` or similar collections.
            // You may also need to access the UI elements from your mod's UI.

            // Example: Add elements from IngameUI
            if (Main.InGameUI?.CurrentState != null)
            {
                allElements.AddRange(Main.InGameUI.CurrentState.Children);
            }

            // If you have your own UI elements, add them to the list.
            // Example:
            // allElements.AddRange(myCustomUIElementList);

            return allElements;
        }
    }
}
