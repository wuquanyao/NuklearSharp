using Microsoft.Xna.Framework.Graphics;

namespace NuklearSharp
{
	internal static class SpriteFontExtensions
	{
		public static float width(this SpriteFont font, string text)
		{
			return font.MeasureString(text).X;
		}

		public static float height(this SpriteFont font)
		{
			return font.LineSpacing;
		}
	}
}
