using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TerraDeck.Items.Discs;
using TerraDeck.UI;

namespace TerraDeck
{
	public class TerraDeck : Mod
	{
        private UserInterface _sourceUserInterface;

        internal Source Source;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                AddEquipTexture(null, EquipType.HandsOn, "ExampleDisc", "TerraDeck/Items/Discs/ExampleDisc_Hand");
                Source = new Source();
                _sourceUserInterface = new UserInterface();
                _sourceUserInterface.SetState(Source);
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _sourceUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "TerraDeck: Source",
                    delegate {
                        _sourceUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

    }


    public class Card : TagSerializable
    {
        public static readonly Func<TagCompound, Card> DESERIALIZER = Load;

        public string name;
        public string nameDisplay;
        public int damage;
        public string faction;
        public string type;
        public int cost;
        public string description;

        public TagCompound SerializeData()
        {
            return new TagCompound
            {
                ["name"] = name,
                ["nameDisplay"] = nameDisplay,
                ["damage"] = damage,
                ["faction"] = faction,
                ["type"] = type,
                ["cost"] = cost,
                ["description"] = description,
            };
        }
        public static Card Load(TagCompound tag)
        {
            var Card = new Card();
            Card.name = tag.GetString("name");
            Card.nameDisplay = tag.GetString("nameDisplay");
            Card.damage = tag.GetInt("damage");
            Card.faction = tag.GetString("faction");
            Card.type = tag.GetString("type");
            Card.cost = tag.GetInt("cost");
            Card.description = tag.GetString("description");
            return Card;
        }
    }
}