using System.Collections.Generic;
using TerraDeck;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace TerraDeck.Items
{
	public class ExampleDeckBox : ModItem
	{
		public bool justHeld;
		private List<Card> Deck = new List<Card>();
		public override bool CloneNewInstances => true; // allows for the tooltips to be updated during gameplay
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Example of a Deck Box.");
		}

		public override void SetDefaults() {
			item.width = 28;
			item.height = 28;
			item.accessory = true;
			item.value = Item.sellPrice(silver: 30);
			item.rare = ItemRarityID.Blue;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 20;
			item.useAnimation = 20;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			// Only false for the first frame of the player equipping the item
			if (!justHeld)
			{
				justHeld = true;
				// when the player initial equips this, the player instance of this list gets updated
				player.GetModPlayer<DeckPlayer>().StaticDeck = new List<Card>(Deck);
			}
			else
			{
				// after that, this list changes when the player instance changes to acount for added cards
				Deck = new List<Card>(player.GetModPlayer<DeckPlayer>().StaticDeck);
			}
			// tells the player vital info
			player.GetModPlayer<DeckPlayer>().exampleDeck = true;
			player.GetModPlayer<DeckPlayer>().deckBox = true;
		}
		
		public override bool UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer) // only run on the client
			{
				int deckTempCount = Deck.Count; // having a temp deck count means that chaning deck counts in the for loop dont effect the result
				// goes through each card from last to first
				for (int i = 0; i < deckTempCount; i++)
				{
					player.QuickSpawnItem(mod.ItemType(Deck[deckTempCount - i-1].name)); // give the player the item version of the card
					Deck.RemoveAt(deckTempCount - i-1); // remove the card from the list
				}
			}
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			// can only use if you have cards in your deck
			if(Deck.Count > 0)
			{
				return true;
			}
			return false;
		}
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			// tells the code that the player has stopped equipping the item
			justHeld = false;
		}

		public override void UpdateInventory(Player player)
		{
			// tells the code that the player has stopped equipping the item
			justHeld = false;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			// draws a tooltip line for each card in the deck and gives it a displayed number
			Player player = Main.player[item.owner];
			if (Deck.Count > 0 && player.whoAmI == Main.myPlayer)
			{
				for (int i = 0; i < Deck.Count; i++)
				{
					var line = new TooltipLine(mod, "Cards" + i, (i+1)+": "+Deck[i].name);
					tooltips.Add(line);

				}
			}

		}

		public override TagCompound Save()
		{
			// saves what cards are in the deck
			return new TagCompound
			{
				[nameof(Deck)] = Deck
			};
		}
		
		public override void Load(TagCompound tag)
		{
			// loads the cards that are saved
			Deck = tag.Get<List<Card>>(nameof(Deck));
		}
		

	}
}
