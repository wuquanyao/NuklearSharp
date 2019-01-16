using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace NuklearSharp
{
	public unsafe static partial class Nuklear
	{
		[StructLayout(LayoutKind.Sequential)]
		public unsafe partial struct nk_mouse_button
		{
			public int down;
			public uint clicked;
			public Vector2 clicked_pos;
		}

		public unsafe partial class nk_input
		{
			public nk_keyboard keyboard = new nk_keyboard();
			public nk_mouse mouse = new nk_mouse();
		}

		public static int nk_input_has_mouse_click(nk_input i, int id)
		{
			nk_mouse_button* btn;
			if (i == null) return (int) (nk_false);
			btn = (nk_mouse_button*) i.mouse.buttons + id;
			return (int) ((((btn->clicked) != 0) && ((btn->down) == (nk_false))) ? nk_true : nk_false);
		}

		public static int nk_input_has_mouse_click_in_rect(nk_input i, int id, RectangleF b)
		{
			nk_mouse_button* btn;
			if (i == null) return (int) (nk_false);
			btn = (nk_mouse_button*) i.mouse.buttons + id;
			if (
				!((((b.X) <= (btn->clicked_pos.X)) && ((btn->clicked_pos.X) < (b.X + b.Width))) &&
				  (((b.Y) <= (btn->clicked_pos.Y)) && ((btn->clicked_pos.Y) < (b.Y + b.Height))))) return (int) (nk_false);
			return (int) (nk_true);
		}

		public static int nk_input_has_mouse_click_down_in_rect(nk_input i, int id, RectangleF b, int down)
		{
			nk_mouse_button* btn;
			if (i == null) return (int) (nk_false);
			btn = (nk_mouse_button*) i.mouse.buttons + id;
			return
				(int) (((nk_input_has_mouse_click_in_rect(i, (int) (id), (RectangleF) (b))) != 0) && ((btn->down) == (down)) ? 1 : 0);
		}

		public static int nk_input_is_mouse_click_in_rect(nk_input i, int id, RectangleF b)
		{
			nk_mouse_button* btn;
			if (i == null) return (int) (nk_false);
			btn = (nk_mouse_button*) i.mouse.buttons + id;
			return
				(int)
					((((nk_input_has_mouse_click_down_in_rect(i, (int) (id), (RectangleF) (b), (int) (nk_false))) != 0) &&
					  ((btn->clicked) != 0))
						? nk_true
						: nk_false);
		}

		public static int nk_input_is_mouse_click_down_in_rect(nk_input i, int id, RectangleF b, int down)
		{
			nk_mouse_button* btn;
			if (i == null) return (int) (nk_false);
			btn = (nk_mouse_button*) i.mouse.buttons + id;
			return
				(int)
					((((nk_input_has_mouse_click_down_in_rect(i, (int) (id), (RectangleF) (b), (int) (down))) != 0) &&
					  ((btn->clicked) != 0))
						? nk_true
						: nk_false);
		}

		public static int nk_input_any_mouse_click_in_rect(nk_input _in_, RectangleF b)
		{
			int i;
			int down = (int) (0);
			for (i = (int) (0); (i) < (NK_BUTTON_MAX); ++i)
			{
				down = (int) (((down) != 0) || ((nk_input_is_mouse_click_in_rect(_in_, (int) (i), (RectangleF) (b))) != 0) ? 1 : 0);
			}
			return (int) (down);
		}

		public static int nk_input_is_mouse_hovering_rect(nk_input i, RectangleF rect)
		{
			if (i == null) return (int) (nk_false);
			return (((rect.X) <= (i.mouse.pos.X)) && ((i.mouse.pos.X) < (rect.X + rect.Width))) &&
			       (((rect.Y) <= (i.mouse.pos.Y)) && ((i.mouse.pos.Y) < (rect.Y + rect.Height)))
				? 1
				: 0;
		}

		public static int nk_input_is_mouse_prev_hovering_rect(nk_input i, RectangleF rect)
		{
			if (i == null) return (int) (nk_false);
			return (((rect.X) <= (i.mouse.prev.X)) && ((i.mouse.prev.X) < (rect.X + rect.Width))) &&
			       (((rect.Y) <= (i.mouse.prev.Y)) && ((i.mouse.prev.Y) < (rect.Y + rect.Height)))
				? 1
				: 0;
		}

		public static int nk_input_mouse_clicked(nk_input i, int id, RectangleF rect)
		{
			if (i == null) return (int) (nk_false);
			if (nk_input_is_mouse_hovering_rect(i, (RectangleF) (rect)) == 0) return (int) (nk_false);
			return (int) (nk_input_is_mouse_click_in_rect(i, (int) (id), (RectangleF) (rect)));
		}

		public static int nk_input_is_mouse_down(nk_input i, int id)
		{
			if (i == null) return (int) (nk_false);
			return (int) (i.mouse.buttons[id].down);
		}

		public static int nk_input_is_mouse_pressed(nk_input i, int id)
		{
			nk_mouse_button* b;
			if (i == null) return (int) (nk_false);
			b = (nk_mouse_button*) i.mouse.buttons + id;
			if (((b->down) != 0) && ((b->clicked) != 0)) return (int) (nk_true);
			return (int) (nk_false);
		}

		public static int nk_input_is_mouse_released(nk_input i, int id)
		{
			if (i == null) return (int) (nk_false);
			return ((i.mouse.buttons[id].down == 0) && ((i.mouse.buttons[id].clicked) != 0)) ? 1 : 0;
		}

		public static int nk_input_is_key_pressed(nk_input i, int key)
		{
			nk_key* k;
			if (i == null) return (int) (nk_false);
			k = (nk_key*) i.keyboard.keys + key;
			if ((((k->down) != 0) && ((k->clicked) != 0)) || ((k->down == 0) && ((k->clicked) >= (2)))) return (int) (nk_true);
			return (int) (nk_false);
		}

		public static int nk_input_is_key_released(nk_input i, int key)
		{
			nk_key* k;
			if (i == null) return (int) (nk_false);
			k = (nk_key*) i.keyboard.keys + key;
			if (((k->down == 0) && ((k->clicked) != 0)) || (((k->down) != 0) && ((k->clicked) >= (2)))) return (int) (nk_true);
			return (int) (nk_false);
		}

		public static int nk_input_is_key_down(nk_input i, int key)
		{
			nk_key* k;
			if (i == null) return (int) (nk_false);
			k = (nk_key*) i.keyboard.keys + key;
			if ((k->down) != 0) return (int) (nk_true);
			return (int) (nk_false);
		}

		public static int nk_toggle_behavior(nk_input _in_, RectangleF select, ref uint state, int active)
		{
			if (((state) & NK_WIDGET_STATE_MODIFIED) != 0)
				(state) = (uint) (NK_WIDGET_STATE_INACTIVE | NK_WIDGET_STATE_MODIFIED);
			else (state) = (uint) (NK_WIDGET_STATE_INACTIVE);
			if ((nk_button_behavior(ref state, (RectangleF) (select), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
			{
				state = (uint) (NK_WIDGET_STATE_ACTIVE);
				active = active != 0 ? 0 : 1;
			}

			if (((state & NK_WIDGET_STATE_HOVER) != 0) && (nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (select)) == 0))
				state |= (uint) (NK_WIDGET_STATE_ENTERED);
			else if ((nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (select))) != 0) state |= (uint) (NK_WIDGET_STATE_LEFT);
			return (int) (active);
		}
	}
}