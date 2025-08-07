using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace ItemBorder
{
    public class TableRowConfig
    {
        public UIText Label;
        //public string KEY;
        public BoolColumn Border;
        public BoolColumn Outline;
        public BoolColumn World;

        public TableRowConfig(UIText label, BoolColumn border, BoolColumn outline, BoolColumn world)
        {
            //KEY = key;
            Label = label;
            Border = border;
            Outline = outline;
            World = world;
        }

        public bool BorderValue()
        {
            if(Border.Value != null)
                return this.Border.Value.Selected;
            return false;
        }
        public bool OutlineValue()
        {
            if (Outline.Value != null)
                return this.Outline.Value.Selected;
            return false;
        }
        public bool WorldValue()
        {
            if (World.Value != null)
                return this.World.Value.Selected;
            return false;
        }
        public class BoolColumn
        {
            public bool Use;
            public bool DefaultValue;
            public UICheckbox Value;
            public BoolColumn(bool use,bool defaultValue)
            {
                Use = use;
                DefaultValue = defaultValue;
            }
        }
    }
    public class ItemBorderConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ClientSide;

        #region BORDER
        [Header("Border")]
        [DefaultValue(true)]
        public bool useBorder;

        [DefaultValue(false)]
        public bool borderBaseRarity;

        [DefaultValue(5)] //PREVIOUSLY 11
        [Range(0, 11)]
        public int borderType;

        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public int borderOpacity;

        [Increment(1)]
        [DefaultValue(-1)]
        [Range(-1, 1000)]
        public int customBorderType;
        #endregion

        #region OUTLINE
        [Header("Outline")]
        [DefaultValue(true)]
        public bool useOutline;

        [DefaultValue(false)]
        public bool outlineBaseRarity;

        [Range(1, 5)]
        [DefaultValue(2)]
        public int outlineWidth;

        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public int outlineOpacity;
        #endregion

        #region WORLD
        [Header("World")]
        [DefaultValue(true)]
        public bool useWorld;

        [DefaultValue(false)]
        public bool worldBaseRarity;

        [Range(1, 5)]
        [DefaultValue(2)]
        public int worldWidth;

        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public int worldOpacity;
        #endregion

     
        [Header("Customization")]
        ////[LabelKey("$Customization Table")]
        ////[CustomModConfigItem(typeof(CustomTableUI))]
        //// List of all table entries with labels and border/outline booleans
        public CustomTableUI ConfigTable;

        //ITEM TOGGLE
        [Header("ItemToggle")]
        public List<ItemDefinition> excludedItems = new List<ItemDefinition>();

        //EXTRA
        [Header("Extra")]
        [DefaultValue(false)]
        public bool specialPickup;

        public List<ItemDefinition> specialItems = new List<ItemDefinition>();
    }
}
