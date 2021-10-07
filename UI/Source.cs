using TerraDeck.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using TerraDeck;

namespace TerraDeck.UI
{
	internal class Source : UIState
	{
		// For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
		// Once this is all set up make sure to go and do the required stuff for most UI's in the Mod class.
		private UIText text;
		private UIElement area;
		private UIImage barFrame;
		private Color gradientA;
		private Color gradientB;

		public override void OnInitialize() {
			// Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
			// UIElement is invisible and has no padding. You can use a UIPanel if you wish for a background.
			area = new UIElement(); 
			area.Left.Set(Main.screenWidth / 2 - (182/2), 0f); // Place the resource bar to the left of the hearts.
			area.Top.Set(40, 0f); // Placing it just a bit below the top of the screen.
			area.Width.Set(182, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
			area.Height.Set(60, 0f);

			barFrame = new UIImage(ModContent.GetTexture("TerraDeck/UI/ChargeFrame"));
			barFrame.Left.Set(22, 0f);
			barFrame.Top.Set(0, 0f);
			barFrame.Width.Set(138, 0f);
			barFrame.Height.Set(34, 0f);

			text = new UIText("0/0", 1f); // text to show stat
			text.Width.Set(138, 0f);
			text.Height.Set(34, 0f);
			text.Top.Set(-20, 0f);
			text.Left.Set(20, 0f);

			gradientA = new Color(1, 250, 151); // A dark purple
			gradientB = new Color(12, 195, 255); // A light purple

			area.Append(text);
			area.Append(barFrame);
			Append(area);
		}
		
		public override void Draw(SpriteBatch spriteBatch) {
			// This prevents drawing unless we are using an ExampleDamageItem
			var modPlayer = Main.LocalPlayer.GetModPlayer<DeckPlayer>();
			if (!(modPlayer.discEquipped))
				return;

			base.Draw(spriteBatch);
			
		}
		
		
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);

			var modPlayer = Main.LocalPlayer.GetModPlayer<DeckPlayer>();
			// Calculate quotient
			float quotient = (float)modPlayer.sourceBarCurrent / modPlayer.sourceBarMax2; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
			quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

			// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
			Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
			hitbox.X += 8;
			hitbox.Width -= 16;
			hitbox.Y += 4;
			hitbox.Height -= 8;

			// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);
			spriteBatch.Draw(Main.magicPixel, new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height), Color.DarkSlateGray);
			for (int i = 0; i < steps; i += 1) {
				//float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right - left);
				
				spriteBatch.Draw(Main.magicPixel, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
			}
			
		}
		public override void Update(GameTime gameTime) {
			var modPlayer = Main.LocalPlayer.GetModPlayer<DeckPlayer>();
			if (!(modPlayer.discEquipped))
				return;

			
			// Setting the text per tick to update and show our resource values.
			text.SetText($"Source: {modPlayer.sourceCurrent}");
			if(modPlayer.deckBox)
			//barFrame.SetImage(ModContent.GetTexture("TerraDeck/UI/ChargeFrame2"));
			base.Update(gameTime);
		}
	}
}
