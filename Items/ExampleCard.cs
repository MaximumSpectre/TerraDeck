using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using IL.Terraria.GameContent.UI.Elements;

namespace TerraDeck.Items
{
	internal class ExampleCard : ModItem
	{
		private const string CardTexturePath = "TerraDeck/Items/ExampleCardBig"; // the large art for the card used for the tooltip
		public string nameDisplay = "Example Card"; // sets the items display name
		public string name = "ExampleCard"; // sets the items internal name
		public int damage = 10;// sets the cards damage
		public string faction = "Example";
		public string type = "Spell";
		public int cost = 1;
		public string description = "This is an Example\nSpell. It does a thing";

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("This line wont show up :^)");
		}
		public override bool CloneNewInstances => true;

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 3;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.consumable = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 20;
			item.useAnimation = 20;
			item.noUseGraphic = true;
			item.noMelee = true;
		}

		public override bool UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer) // run on client
			{
				if (player.GetModPlayer<DeckPlayer>().deckBox)
				{
					// adds a new card to the player instance of the card list
					player.GetModPlayer<DeckPlayer>().StaticDeck.Add(new Card(){
					name = name,
					nameDisplay = nameDisplay,
					damage = damage,
					faction = faction,
					type = type,
					cost = cost,
					description = description
					});;
				}
				return true;
			}
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			// only useable if the player has a deck box equipped and hasnt already filled it
			if (player.GetModPlayer<DeckPlayer>().deckBox && player.GetModPlayer<DeckPlayer>().StaticDeck.Count < 12)
			{
				return true;
			}
			return false;
		}

		private Vector2 boxSize = new Vector2(224, 320);
		private Vector2 TTpos = new Vector2(0, 0);

		public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
		{
			Player player = Main.player[item.owner];
			// draws the card where the tooltip is
			if (player.whoAmI == Main.myPlayer)
			{
				Texture2D cardTexture = ModContent.GetTexture(CardTexturePath);

				Vector2 drawPos = new Vector2(x, y);
				Main.spriteBatch.Draw(cardTexture, drawPos, Color.White);
				TTpos.X = x;
				TTpos.Y = y;
				return true;
			}
			return false;
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			Player player = Main.player[item.owner];
			if (player.whoAmI == Main.myPlayer)
			{
				if (line.mod == "TerraDeck")// only lets tooltip lines from this mod to be displayed to stop interference
				{
					float boxOffset = boxSize.X / 2;// gets the middle of the card
					// sets the positioning for the name of the card
					if (line.Name == "CardName")
					{
						line.X = (int)TTpos.X + (int)boxOffset - (int)line.font.MeasureString(line.text).X / 2;
						line.Y = (int)TTpos.Y + 8;
					}
					if (line.Name == "Cost")
					{
						line.X = (int)TTpos.X + 16;
						line.Y = (int)TTpos.Y + 8;
					}
					if (line.Name == "Damage")
					{
						line.X = (int)TTpos.X + 16;
						line.Y = (int)TTpos.Y + 126;
					}
					if (line.Name == "Description")
					{
						line.X = (int)TTpos.X + 20;
						line.Y = (int)TTpos.Y + 158;
					}
					return true;
				}
			}
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.player[item.owner];
			// create the tooltip lines
			if (player.whoAmI == Main.myPlayer)
			{
				var line = new TooltipLine(mod, "CardName", nameDisplay);
				tooltips.Add(line);
				var line2 = new TooltipLine(mod, "Cost", cost.ToString());
				tooltips.Add(line2);
				line2.overrideColor = Color.MediumPurple;
				var line3 = new TooltipLine(mod, "Damage", damage.ToString());
				line3.overrideColor = Color.Red;
				tooltips.Add(line3);
				var line4 = new TooltipLine(mod, "Description", description);
				tooltips.Add(line4);
			}
		}
	}
}
