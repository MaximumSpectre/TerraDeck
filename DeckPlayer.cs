using TerraDeck.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace TerraDeck
{
	// ModPlayer classes provide a way to attach data to Players and act on that data. ExamplePlayer has a lot of functionality related to 
	// several effects and items in ExampleMod. See SimpleModPlayer for a very simple example of how ModPlayer classes work.
	public class DeckPlayer : ModPlayer
	{
		public bool exampleDeck;
		public bool deckBox;
		public List<Card> deck;
		public List<Card> StaticDeck;
		public bool discEquipped;
		public bool discExample;

		public int sourceCurrent;
		public const int DefaultSourceMax = 10;
		public int sourceMax;
		public int sourceMax2;
		public static readonly Color HealChargeResource = new Color(187, 91, 201); // We can use this for CombatText, if you create an item that replenishes exampleResourceCurrent.

		public int sourceBarCurrent;
		public const int DefaultSourceBarMax = 100;
		public int sourceBarMax;
		public int sourceBarMax2;
		public float sourceBarRegenRate;
		internal int sourceBarRegenTimer = 0;
		public static DeckPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<DeckPlayer>();
		}
		public override void ResetEffects()
		{
			ResetVariables();
		}

		public override void Initialize()
		{
			sourceMax = DefaultSourceMax;
			sourceBarMax = DefaultSourceBarMax;
		}

		public override void UpdateDead()
		{
			ResetVariables();
		}

		private void ResetVariables()
		{
			exampleDeck = false;
			deckBox = false;
			discEquipped = false;
			discExample = false;
			sourceBarRegenRate = 1f;
			sourceBarMax2 = sourceBarMax;
			sourceMax2 = sourceMax;
		}
		public override void PostUpdateMiscEffects()
		{
			UpdateResource();
		}
		private void UpdateResource()
		{
			sourceBarRegenTimer++;
			if (sourceBarRegenTimer > 1 * sourceBarRegenRate && sourceCurrent < sourceMax2)
			{
				sourceBarCurrent += 1;
				sourceBarRegenTimer = 0;
			}
			if(sourceBarCurrent == sourceBarMax2)
			{
				sourceBarCurrent = 0;
				sourceCurrent++;
			}
			// Limit ResourceCurrent from going over the limit imposed by ResourceMax.
			sourceBarCurrent = Utils.Clamp(sourceBarCurrent, 0, sourceBarMax2);
			sourceCurrent = Utils.Clamp(sourceCurrent, 0, sourceMax2);
		}
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
		}

		public override void PreUpdate()
		{
			if (!deckBox)
			{
				StaticDeck = new List<Card>();
			}
		}

		public override void FrameEffects()
		{
			if(discEquipped && discExample)
			{
				player.handon = mod.GetAccessorySlot("ExampleDisc", EquipType.HandsOn);
			}
		}
	}
}
