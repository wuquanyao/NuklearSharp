﻿namespace NuklearSharp
{
	unsafe partial class TextEdit
	{
		public void Text(string text, int total_len)
		{
			fixed (char* p = text)
			{
				Text(p, total_len);
			}
		}
	}
}
