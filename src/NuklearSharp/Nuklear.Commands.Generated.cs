using System;
using System.Runtime.InteropServices;

namespace NuklearSharp
{
	public unsafe static partial class Nuklear
	{
		public unsafe partial class nk_scroll
		{
			public uint x;
			public uint y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public unsafe partial struct nk_command
		{
			public int type;
			public ulong next;
		}

		public unsafe partial class nk_row_layout
		{
			public int type;
			public int index;
			public float height;
			public float min_height;
			public int columns;
			public float* ratio;
			public float item_width;
			public float item_height;
			public float item_offset;
			public float filled;
			public Rectangle item = new Rectangle();
			public int tree_depth;
			public float[] templates = new float[16];
		}

		public unsafe partial class nk_menu_state
		{
			public float x;
			public float y;
			public float w;
			public float h;
			public nk_scroll offset = new nk_scroll();
		}

		public unsafe partial class nk_popup_state
		{
			public nk_window win;
			public int type;
			public nk_popup_buffer buf = new nk_popup_buffer();
			public uint name;
			public int active;
			public uint combo_count;
			public uint con_count;
			public uint con_old;
			public uint active_con;
			public Rectangle header = new Rectangle();
		}

		public unsafe partial class nk_edit_state
		{
			public uint name;
			public uint seq;
			public uint old;
			public int active;
			public int prev;
			public int cursor;
			public int sel_start;
			public int sel_end;
			public nk_scroll scrollbar = new nk_scroll();
			public byte mode;
			public byte single_line;
		}

		public unsafe partial class nk_property_state
		{
			public int active;
			public int prev;
			public string buffer;
			public int cursor;
			public int select_start;
			public int select_end;
			public uint name;
			public uint seq;
			public uint old;
			public int state;
		}

		public unsafe partial class nk_table
		{
			public uint seq;
			public uint size;
			public uint[] keys = new uint[51];
			public uint[] values = new uint[51];
			public nk_table next;
			public nk_table prev;
		}



		public static void nk_widget_text(nk_command_buffer o, Rectangle b, char* _string_, int len, nk_text* t, uint a,
			nk_user_font f)
		{
			Rectangle label = new Rectangle();
			float text_width;
			if ((o == null) || (t == null)) return;
			b.h = (float) ((b.h) < (2*t->padding.y) ? (2*t->padding.y) : (b.h));
			label.x = (float) (0);
			label.w = (float) (0);
			label.y = (float) (b.y + t->padding.y);
			label.h = (float) ((f.height) < (b.h - 2*t->padding.y) ? (f.height) : (b.h - 2*t->padding.y));
			text_width = (float) (f.width((nk_handle) (f.userdata), (float) (f.height), _string_, (int) (len)));
			text_width += (float) (2.0f*t->padding.x);
			if ((a & NK_TEXT_ALIGN_LEFT) != 0)
			{
				label.x = (float) (b.x + t->padding.x);
				label.w = (float) ((0) < (b.w - 2*t->padding.x) ? (b.w - 2*t->padding.x) : (0));
			}
			else if ((a & NK_TEXT_ALIGN_CENTERED) != 0)
			{
				label.w = (float) ((1) < (2*t->padding.x + text_width) ? (2*t->padding.x + text_width) : (1));
				label.x = (float) (b.x + t->padding.x + ((b.w - 2*t->padding.x) - label.w)/2);
				label.x = (float) ((b.x + t->padding.x) < (label.x) ? (label.x) : (b.x + t->padding.x));
				label.w = (float) ((b.x + b.w) < (label.x + label.w) ? (b.x + b.w) : (label.x + label.w));
				if ((label.w) >= (label.x)) label.w -= (float) (label.x);
			}
			else if ((a & NK_TEXT_ALIGN_RIGHT) != 0)
			{
				label.x =
					(float)
						((b.x + t->padding.x) < ((b.x + b.w) - (2*t->padding.x + text_width))
							? ((b.x + b.w) - (2*t->padding.x + text_width))
							: (b.x + t->padding.x));
				label.w = (float) (text_width + 2*t->padding.x);
			}
			else return;
			if ((a & NK_TEXT_ALIGN_MIDDLE) != 0)
			{
				label.y = (float) (b.y + b.h/2.0f - f.height/2.0f);
				label.h =
					(float)
						((b.h/2.0f) < (b.h - (b.h/2.0f + f.height/2.0f))
							? (b.h - (b.h/2.0f + f.height/2.0f))
							: (b.h/2.0f));
			}
			else if ((a & NK_TEXT_ALIGN_BOTTOM) != 0)
			{
				label.y = (float) (b.y + b.h - f.height);
				label.h = (float) (f.height);
			}

			nk_draw_text(o, (Rectangle) (label), _string_, (int) (len), f, (Color) (t->background),
				(Color) (t->text));
		}

		public static void nk_widget_text_wrap(nk_command_buffer o, Rectangle b, char* _string_, int len, nk_text* t,
			nk_user_font f)
		{
			float width;
			int glyphs = (int) (0);
			int fitting = (int) (0);
			int done = (int) (0);
			Rectangle line = new Rectangle();
			nk_text text = new nk_text();
			uint* seperator = stackalloc uint[1];
			seperator[0] = (uint) (' ');

			if ((o == null) || (t == null)) return;
			text.padding = (Vector2) (Vector2_((float) (0), (float) (0)));
			text.background = (Color) (t->background);
			text.text = (Color) (t->text);
			b.w = (float) ((b.w) < (2*t->padding.x) ? (2*t->padding.x) : (b.w));
			b.h = (float) ((b.h) < (2*t->padding.y) ? (2*t->padding.y) : (b.h));
			b.h = (float) (b.h - 2*t->padding.y);
			line.x = (float) (b.x + t->padding.x);
			line.y = (float) (b.y + t->padding.y);
			line.w = (float) (b.w - 2*t->padding.x);
			line.h = (float) (2*t->padding.y + f.height);
			fitting = (int) (nk_text_clamp(f, _string_, (int) (len), (float) (line.w), &glyphs, &width, seperator, 1));
			while ((done) < (len))
			{
				if ((fitting == 0) || ((line.y + line.h) >= (b.y + b.h))) break;
				nk_widget_text(o, (Rectangle) (line), &_string_[done], (int) (fitting), &text, (uint) (NK_TEXT_LEFT), f);
				done += (int) (fitting);
				line.y += (float) (f.height + 2*t->padding.y);
				fitting =
					(int)
						(nk_text_clamp(f, &_string_[done], (int) (len - done), (float) (line.w), &glyphs, &width,
							seperator, 1));
			}
		}

		public static void nk_draw_symbol(nk_command_buffer _out_, int type, Rectangle content, Color background,
			Color foreground, float border_width, nk_user_font font)
		{
			switch (type)
			{
				case NK_SYMBOL_X:
				case NK_SYMBOL_UNDERSCORE:
				case NK_SYMBOL_PLUS:
				case NK_SYMBOL_MINUS:
				{
					char X = ((type) == (NK_SYMBOL_X))
						? 'x'
						: ((type) == (NK_SYMBOL_UNDERSCORE)) ? '_' : ((type) == (NK_SYMBOL_PLUS)) ? '+' : '-';
					nk_text text = new nk_text();
					text.padding = (Vector2) (Vector2_((float) (0), (float) (0)));
					text.background = (Color) (background);
					text.text = (Color) (foreground);
					nk_widget_text(_out_, (Rectangle) (content), &X, (int) (1), &text, (uint) (NK_TEXT_CENTERED), font);
				}
					break;
				case NK_SYMBOL_CIRCLE_SOLID:
				case NK_SYMBOL_CIRCLE_OUTLINE:
				case NK_SYMBOL_RECT_SOLID:
				case NK_SYMBOL_RECT_OUTLINE:
				{
					if (((type) == (NK_SYMBOL_RECT_SOLID)) || ((type) == (NK_SYMBOL_RECT_OUTLINE)))
					{
						nk_fill_rect(_out_, (Rectangle) (content), (float) (0), (Color) (foreground));
						if ((type) == (NK_SYMBOL_RECT_OUTLINE))
							nk_fill_rect(_out_, (Rectangle) (nk_shriRectangle_((Rectangle) (content), (float) (border_width))),
								(float) (0), (Color) (background));
					}
					else
					{
						nk_fill_circle(_out_, (Rectangle) (content), (Color) (foreground));
						if ((type) == (NK_SYMBOL_CIRCLE_OUTLINE))
							nk_fill_circle(_out_, (Rectangle) (nk_shriRectangle_((Rectangle) (content), (float) (1))),
								(Color) (background));
					}
				}
					break;
				case NK_SYMBOL_TRIANGLE_UP:
				case NK_SYMBOL_TRIANGLE_DOWN:
				case NK_SYMBOL_TRIANGLE_LEFT:
				case NK_SYMBOL_TRIANGLE_RIGHT:
				{
					int heading;
					Vector2* points = stackalloc Vector2[3];
					heading =
						(int)
							(((type) == (NK_SYMBOL_TRIANGLE_RIGHT))
								? NK_RIGHT
								: ((type) == (NK_SYMBOL_TRIANGLE_LEFT))
									? NK_LEFT
									: ((type) == (NK_SYMBOL_TRIANGLE_UP)) ? NK_UP : NK_DOWN);
					nk_triangle_from_direction(points, (Rectangle) (content), (float) (0), (float) (0), (int) (heading));
					nk_fill_triangle(_out_, (float) (points[0].x), (float) (points[0].y), (float) (points[1].x),
						(float) (points[1].y), (float) (points[2].x), (float) (points[2].y), (Color) (foreground));
				}
					break;
				default:
				case NK_SYMBOL_NONE:
				case NK_SYMBOL_MAX:
					break;
			}

		}

		public static nk_style_item nk_draw_button(nk_command_buffer _out_, Rectangle* bounds, uint state,
			nk_style_button style)
		{
			nk_style_item background;
			if ((state & NK_WIDGET_STATE_HOVER) != 0) background = style.hover;
			else if ((state & NK_WIDGET_STATE_ACTIVED) != 0) background = style.active;
			else background = style.normal;
			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				nk_draw_image(_out_, (Rectangle) (*bounds), background.data.image, (Color) (nk_white));
			}
			else
			{
				nk_fill_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (Color) (background.data.color));
				nk_stroke_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (float) (style.border),
					(Color) (style.border_color));
			}

			return background;
		}

		public static void nk_draw_button_text(nk_command_buffer _out_, Rectangle* bounds, Rectangle* content, uint state,
			nk_style_button style, char* txt, int len, uint text_alignment, nk_user_font font)
		{
			nk_text text = new nk_text();
			nk_style_item background;
			background = nk_draw_button(_out_, bounds, (uint) (state), style);
			if ((background.type) == (NK_STYLE_ITEM_COLOR)) text.background = (Color) (background.data.color);
			else text.background = (Color) (style.text_background);
			if ((state & NK_WIDGET_STATE_HOVER) != 0) text.text = (Color) (style.text_hover);
			else if ((state & NK_WIDGET_STATE_ACTIVED) != 0) text.text = (Color) (style.text_active);
			else text.text = (Color) (style.text_normal);
			text.padding = (Vector2) (Vector2_((float) (0), (float) (0)));
			nk_widget_text(_out_, (Rectangle) (*content), txt, (int) (len), &text, (uint) (text_alignment), font);
		}

		public static void nk_draw_button_symbol(nk_command_buffer _out_, Rectangle* bounds, Rectangle* content, uint state,
			nk_style_button style, int type, nk_user_font font)
		{
			Color sym = new Color();
			Color bg = new Color();
			nk_style_item background;
			background = nk_draw_button(_out_, bounds, (uint) (state), style);
			if ((background.type) == (NK_STYLE_ITEM_COLOR)) bg = (Color) (background.data.color);
			else bg = (Color) (style.text_background);
			if ((state & NK_WIDGET_STATE_HOVER) != 0) sym = (Color) (style.text_hover);
			else if ((state & NK_WIDGET_STATE_ACTIVED) != 0) sym = (Color) (style.text_active);
			else sym = (Color) (style.text_normal);
			nk_draw_symbol(_out_, (int) (type), (Rectangle) (*content), (Color) (bg), (Color) (sym), (float) (1),
				font);
		}

		public static void nk_draw_button_image(nk_command_buffer _out_, Rectangle* bounds, Rectangle* content, uint state,
			nk_style_button style, nk_image img)
		{
			nk_draw_button(_out_, bounds, (uint) (state), style);
			nk_draw_image(_out_, (Rectangle) (*content), img, (Color) (nk_white));
		}

		public static void nk_draw_button_text_symbol(nk_command_buffer _out_, Rectangle* bounds, Rectangle* label,
			Rectangle* symbol, uint state, nk_style_button style, char* str, int len, int type, nk_user_font font)
		{
			Color sym = new Color();
			nk_text text = new nk_text();
			nk_style_item background;
			background = nk_draw_button(_out_, bounds, (uint) (state), style);
			if ((background.type) == (NK_STYLE_ITEM_COLOR)) text.background = (Color) (background.data.color);
			else text.background = (Color) (style.text_background);
			if ((state & NK_WIDGET_STATE_HOVER) != 0)
			{
				sym = (Color) (style.text_hover);
				text.text = (Color) (style.text_hover);
			}
			else if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				sym = (Color) (style.text_active);
				text.text = (Color) (style.text_active);
			}
			else
			{
				sym = (Color) (style.text_normal);
				text.text = (Color) (style.text_normal);
			}

			text.padding = (Vector2) (Vector2_((float) (0), (float) (0)));
			nk_draw_symbol(_out_, (int) (type), (Rectangle) (*symbol), (Color) (style.text_background),
				(Color) (sym), (float) (0), font);
			nk_widget_text(_out_, (Rectangle) (*label), str, (int) (len), &text, (uint) (NK_TEXT_CENTERED), font);
		}

		public static void nk_draw_button_text_image(nk_command_buffer _out_, Rectangle* bounds, Rectangle* label,
			Rectangle* image, uint state, nk_style_button style, char* str, int len, nk_user_font font, nk_image img)
		{
			nk_text text = new nk_text();
			nk_style_item background;
			background = nk_draw_button(_out_, bounds, (uint) (state), style);
			if ((background.type) == (NK_STYLE_ITEM_COLOR)) text.background = (Color) (background.data.color);
			else text.background = (Color) (style.text_background);
			if ((state & NK_WIDGET_STATE_HOVER) != 0) text.text = (Color) (style.text_hover);
			else if ((state & NK_WIDGET_STATE_ACTIVED) != 0) text.text = (Color) (style.text_active);
			else text.text = (Color) (style.text_normal);
			text.padding = (Vector2) (Vector2_((float) (0), (float) (0)));
			nk_widget_text(_out_, (Rectangle) (*label), str, (int) (len), &text, (uint) (NK_TEXT_CENTERED), font);
			nk_draw_image(_out_, (Rectangle) (*image), img, (Color) (nk_white));
		}

		public static void nk_draw_checkbox(nk_command_buffer _out_, uint state, nk_style_toggle style, int active,
			Rectangle* label, Rectangle* selector, Rectangle* cursors, char* _string_, int len, nk_user_font font)
		{
			nk_style_item background;
			nk_style_item cursor;
			nk_text text = new nk_text();
			if ((state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.hover;
				cursor = style.cursor_hover;
				text.text = (Color) (style.text_hover);
			}
			else if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.hover;
				cursor = style.cursor_hover;
				text.text = (Color) (style.text_active);
			}
			else
			{
				background = style.normal;
				cursor = style.cursor_normal;
				text.text = (Color) (style.text_normal);
			}

			if ((background.type) == (NK_STYLE_ITEM_COLOR))
			{
				nk_fill_rect(_out_, (Rectangle) (*selector), (float) (0), (Color) (style.border_color));
				nk_fill_rect(_out_, (Rectangle) (nk_shriRectangle_((Rectangle) (*selector), (float) (style.border))),
					(float) (0), (Color) (background.data.color));
			}
			else nk_draw_image(_out_, (Rectangle) (*selector), background.data.image, (Color) (nk_white));
			if ((active) != 0)
			{
				if ((cursor.type) == (NK_STYLE_ITEM_IMAGE))
					nk_draw_image(_out_, (Rectangle) (*cursors), cursor.data.image, (Color) (nk_white));
				else nk_fill_rect(_out_, (Rectangle) (*cursors), (float) (0), (Color) (cursor.data.color));
			}

			text.padding.x = (float) (0);
			text.padding.y = (float) (0);
			text.background = (Color) (style.text_background);
			nk_widget_text(_out_, (Rectangle) (*label), _string_, (int) (len), &text, (uint) (NK_TEXT_LEFT), font);
		}

		public static void nk_draw_option(nk_command_buffer _out_, uint state, nk_style_toggle style, int active,
			Rectangle* label, Rectangle* selector, Rectangle* cursors, char* _string_, int len, nk_user_font font)
		{
			nk_style_item background;
			nk_style_item cursor;
			nk_text text = new nk_text();
			if ((state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.hover;
				cursor = style.cursor_hover;
				text.text = (Color) (style.text_hover);
			}
			else if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.hover;
				cursor = style.cursor_hover;
				text.text = (Color) (style.text_active);
			}
			else
			{
				background = style.normal;
				cursor = style.cursor_normal;
				text.text = (Color) (style.text_normal);
			}

			if ((background.type) == (NK_STYLE_ITEM_COLOR))
			{
				nk_fill_circle(_out_, (Rectangle) (*selector), (Color) (style.border_color));
				nk_fill_circle(_out_, (Rectangle) (nk_shriRectangle_((Rectangle) (*selector), (float) (style.border))),
					(Color) (background.data.color));
			}
			else nk_draw_image(_out_, (Rectangle) (*selector), background.data.image, (Color) (nk_white));
			if ((active) != 0)
			{
				if ((cursor.type) == (NK_STYLE_ITEM_IMAGE))
					nk_draw_image(_out_, (Rectangle) (*cursors), cursor.data.image, (Color) (nk_white));
				else nk_fill_circle(_out_, (Rectangle) (*cursors), (Color) (cursor.data.color));
			}

			text.padding.x = (float) (0);
			text.padding.y = (float) (0);
			text.background = (Color) (style.text_background);
			nk_widget_text(_out_, (Rectangle) (*label), _string_, (int) (len), &text, (uint) (NK_TEXT_LEFT), font);
		}

		public static void nk_draw_selectable(nk_command_buffer _out_, uint state, nk_style_selectable style, int active,
			Rectangle* bounds, Rectangle* icon, nk_image img, char* _string_, int len, uint align, nk_user_font font)
		{
			nk_style_item background;
			nk_text text = new nk_text();
			text.padding = (Vector2) (style.padding);
			if (active == 0)
			{
				if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
				{
					background = style.pressed;
					text.text = (Color) (style.text_pressed);
				}
				else if ((state & NK_WIDGET_STATE_HOVER) != 0)
				{
					background = style.hover;
					text.text = (Color) (style.text_hover);
				}
				else
				{
					background = style.normal;
					text.text = (Color) (style.text_normal);
				}
			}
			else
			{
				if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
				{
					background = style.pressed_active;
					text.text = (Color) (style.text_pressed_active);
				}
				else if ((state & NK_WIDGET_STATE_HOVER) != 0)
				{
					background = style.hover_active;
					text.text = (Color) (style.text_hover_active);
				}
				else
				{
					background = style.normal_active;
					text.text = (Color) (style.text_normal_active);
				}
			}

			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				nk_draw_image(_out_, (Rectangle) (*bounds), background.data.image, (Color) (nk_white));
				text.background = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			}
			else
			{
				nk_fill_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (Color) (background.data.color));
				text.background = (Color) (background.data.color);
			}

			if (((img) != null) && ((icon) != null))
				nk_draw_image(_out_, (Rectangle) (*icon), img, (Color) (nk_white));
			nk_widget_text(_out_, (Rectangle) (*bounds), _string_, (int) (len), &text, (uint) (align), font);
		}

		public static void nk_draw_slider(nk_command_buffer _out_, uint state, nk_style_slider style, Rectangle* bounds,
			Rectangle* visual_cursor, float min, float value, float max)
		{
			Rectangle fill = new Rectangle();
			Rectangle bar = new Rectangle();
			nk_style_item background;
			Color bar_color = new Color();
			nk_style_item cursor;
			if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.active;
				bar_color = (Color) (style.bar_active);
				cursor = style.cursor_active;
			}
			else if ((state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.hover;
				bar_color = (Color) (style.bar_hover);
				cursor = style.cursor_hover;
			}
			else
			{
				background = style.normal;
				bar_color = (Color) (style.bar_normal);
				cursor = style.cursor_normal;
			}

			bar.x = (float) (bounds->x);
			bar.y = (float) ((visual_cursor->y + visual_cursor->h/2) - bounds->h/12);
			bar.w = (float) (bounds->w);
			bar.h = (float) (bounds->h/6);
			fill.w = (float) ((visual_cursor->x + (visual_cursor->w/2.0f)) - bar.x);
			fill.x = (float) (bar.x);
			fill.y = (float) (bar.y);
			fill.h = (float) (bar.h);
			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				nk_draw_image(_out_, (Rectangle) (*bounds), background.data.image, (Color) (nk_white));
			}
			else
			{
				nk_fill_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (Color) (background.data.color));
				nk_stroke_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (float) (style.border),
					(Color) (style.border_color));
			}

			nk_fill_rect(_out_, (Rectangle) (bar), (float) (style.rounding), (Color) (bar_color));
			nk_fill_rect(_out_, (Rectangle) (fill), (float) (style.rounding), (Color) (style.bar_filled));
			if ((cursor.type) == (NK_STYLE_ITEM_IMAGE))
				nk_draw_image(_out_, (Rectangle) (*visual_cursor), cursor.data.image, (Color) (nk_white));
			else nk_fill_circle(_out_, (Rectangle) (*visual_cursor), (Color) (cursor.data.color));
		}

		public static void nk_draw_progress(nk_command_buffer _out_, uint state, nk_style_progress style,
			Rectangle* bounds, Rectangle* scursor, ulong value, ulong max)
		{
			nk_style_item background;
			nk_style_item cursor;
			if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.active;
				cursor = style.cursor_active;
			}
			else if ((state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.hover;
				cursor = style.cursor_hover;
			}
			else
			{
				background = style.normal;
				cursor = style.cursor_normal;
			}

			if ((background.type) == (NK_STYLE_ITEM_COLOR))
			{
				nk_fill_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (Color) (background.data.color));
				nk_stroke_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (float) (style.border),
					(Color) (style.border_color));
			}
			else nk_draw_image(_out_, (Rectangle) (*bounds), background.data.image, (Color) (nk_white));
			if ((cursor.type) == (NK_STYLE_ITEM_COLOR))
			{
				nk_fill_rect(_out_, (Rectangle) (*scursor), (float) (style.rounding), (Color) (cursor.data.color));
				nk_stroke_rect(_out_, (Rectangle) (*scursor), (float) (style.rounding), (float) (style.border),
					(Color) (style.border_color));
			}
			else nk_draw_image(_out_, (Rectangle) (*scursor), cursor.data.image, (Color) (nk_white));
		}

		public static void nk_draw_scrollbar(nk_command_buffer _out_, uint state, nk_style_scrollbar style,
			Rectangle* bounds, Rectangle* scroll)
		{
			nk_style_item background;
			nk_style_item cursor;
			if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.active;
				cursor = style.cursor_active;
			}
			else if ((state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.hover;
				cursor = style.cursor_hover;
			}
			else
			{
				background = style.normal;
				cursor = style.cursor_normal;
			}

			if ((background.type) == (NK_STYLE_ITEM_COLOR))
			{
				nk_fill_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (Color) (background.data.color));
				nk_stroke_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (float) (style.border),
					(Color) (style.border_color));
			}
			else
			{
				nk_draw_image(_out_, (Rectangle) (*bounds), background.data.image, (Color) (nk_white));
			}

			if ((background.type) == (NK_STYLE_ITEM_COLOR))
			{
				nk_fill_rect(_out_, (Rectangle) (*scroll), (float) (style.rounding_cursor), (Color) (cursor.data.color));
				nk_stroke_rect(_out_, (Rectangle) (*scroll), (float) (style.rounding_cursor),
					(float) (style.border_cursor), (Color) (style.cursor_border_color));
			}
			else nk_draw_image(_out_, (Rectangle) (*scroll), cursor.data.image, (Color) (nk_white));
		}

		public static void nk_edit_draw_text(nk_command_buffer _out_, nk_style_edit style, float pos_x, float pos_y,
			float x_offset, char* text, int byte_len, float row_height, nk_user_font font, Color background,
			Color foreground, int is_selected)
		{
			if ((((text == null) || (byte_len == 0)) || (_out_ == null)) || (style == null)) return;
			{
				int glyph_len = (int) (0);
				char unicode = (char) 0;
				int text_len = (int) (0);
				float line_width = (float) (0);
				float glyph_width;
				char* line = text;
				float line_offset = (float) (0);
				int line_count = (int) (0);
				nk_text txt = new nk_text();
				txt.padding = (Vector2) (Vector2_((float) (0), (float) (0)));
				txt.background = (Color) (background);
				txt.text = (Color) (foreground);
				glyph_len = (int) (nk_utf_decode(text + text_len, &unicode, (int) (byte_len - text_len)));
				if (glyph_len == 0) return;
				while (((text_len) < (byte_len)) && ((glyph_len) != 0))
				{
					if ((unicode) == ('\n'))
					{
						Rectangle label = new Rectangle();
						label.y = (float) (pos_y + line_offset);
						label.h = (float) (row_height);
						label.w = (float) (line_width);
						label.x = (float) (pos_x);
						if (line_count == 0) label.x += (float) (x_offset);
						if ((is_selected) != 0)
							nk_fill_rect(_out_, (Rectangle) (label), (float) (0), (Color) (background));
						nk_widget_text(_out_, (Rectangle) (label), line, (int) ((text + text_len) - line), &txt,
							(uint) (NK_TEXT_CENTERED), font);
						text_len++;
						line_count++;
						line_width = (float) (0);
						line = text + text_len;
						line_offset += (float) (row_height);
						glyph_len = (int) (nk_utf_decode(text + text_len, &unicode, (int) (byte_len - text_len)));
						continue;
					}
					if ((unicode) == ('\r'))
					{
						text_len++;
						glyph_len = (int) (nk_utf_decode(text + text_len, &unicode, (int) (byte_len - text_len)));
						continue;
					}
					glyph_width =
						(float)
							(font.width((nk_handle) (font.userdata), (float) (font.height), text + text_len,
								(int) (glyph_len)));
					line_width += (float) (glyph_width);
					text_len += (int) (glyph_len);
					glyph_len = (int) (nk_utf_decode(text + text_len, &unicode, (int) (byte_len - text_len)));
					continue;
				}
				if ((line_width) > (0))
				{
					Rectangle label = new Rectangle();
					label.y = (float) (pos_y + line_offset);
					label.h = (float) (row_height);
					label.w = (float) (line_width);
					label.x = (float) (pos_x);
					if (line_count == 0) label.x += (float) (x_offset);
					if ((is_selected) != 0)
						nk_fill_rect(_out_, (Rectangle) (label), (float) (0), (Color) (background));
					nk_widget_text(_out_, (Rectangle) (label), line, (int) ((text + text_len) - line), &txt,
						(uint) (NK_TEXT_LEFT), font);
				}
			}

		}

		public static void nk_draw_property(nk_command_buffer _out_, nk_style_property style, Rectangle* bounds,
			Rectangle* label, uint state, char* name, int len, nk_user_font font)
		{
			nk_text text = new nk_text();
			nk_style_item background;
			if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.active;
				text.text = (Color) (style.label_active);
			}
			else if ((state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.hover;
				text.text = (Color) (style.label_hover);
			}
			else
			{
				background = style.normal;
				text.text = (Color) (style.label_normal);
			}

			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				nk_draw_image(_out_, (Rectangle) (*bounds), background.data.image, (Color) (nk_white));
				text.background = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			}
			else
			{
				text.background = (Color) (background.data.color);
				nk_fill_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (Color) (background.data.color));
				nk_stroke_rect(_out_, (Rectangle) (*bounds), (float) (style.rounding), (float) (style.border),
					(Color) (background.data.color));
			}

			text.padding = (Vector2) (Vector2_((float) (0), (float) (0)));
			nk_widget_text(_out_, (Rectangle) (*label), name, (int) (len), &text, (uint) (NK_TEXT_CENTERED), font);
		}

		public static void nk_draw_color_picker(nk_command_buffer o, Rectangle* matrix, Rectangle* hue_bar,
			Rectangle* alpha_bar, Colorf col)
		{
			Color black = (Color) (nk_black);
			Color white = (Color) (nk_white);
			Color black_trans = new Color();
			float crosshair_size = (float) (7.0f);
			Color temp = new Color();
			float* hsva = stackalloc float[4];
			float line_y;
			int i;
			Colorf_hsva_fv(hsva, (Colorf) (col));
			for (i = (int) (0); (i) < (6); ++i)
			{
				nk_fill_rect_multi_color(o,
					(Rectangle)
						(Rectangle_((float) (hue_bar->x), (float) (hue_bar->y + (float) (i)*(hue_bar->h/6.0f) + 0.5f),
							(float) (hue_bar->w), (float) ((hue_bar->h/6.0f) + 0.5f))), (Color) (hue_colors[i]),
					(Color) (hue_colors[i]), (Color) (hue_colors[i + 1]), (Color) (hue_colors[i + 1]));
			}
			line_y = ((float) ((int) (hue_bar->y + hsva[0]*matrix->h + 0.5f)));
			nk_stroke_line(o, (float) (hue_bar->x - 1), (float) (line_y), (float) (hue_bar->x + hue_bar->w + 2),
				(float) (line_y), (float) (1), (Color) (nk_rgb((int) (255), (int) (255), (int) (255))));
			if ((alpha_bar) != null)
			{
				float alpha =
					(float) ((0) < ((1.0f) < (col.a) ? (1.0f) : (col.a)) ? ((1.0f) < (col.a) ? (1.0f) : (col.a)) : (0));
				line_y = ((float) ((int) (alpha_bar->y + (1.0f - alpha)*matrix->h + 0.5f)));
				nk_fill_rect_multi_color(o, (Rectangle) (*alpha_bar), (Color) (white), (Color) (white),
					(Color) (black), (Color) (black));
				nk_stroke_line(o, (float) (alpha_bar->x - 1), (float) (line_y),
					(float) (alpha_bar->x + alpha_bar->w + 2), (float) (line_y), (float) (1),
					(Color) (nk_rgb((int) (255), (int) (255), (int) (255))));
			}

			temp = (Color) (nk_hsv_f((float) (hsva[0]), (float) (1.0f), (float) (1.0f)));
			nk_fill_rect_multi_color(o, (Rectangle) (*matrix), (Color) (white), (Color) (temp), (Color) (temp),
				(Color) (white));
			nk_fill_rect_multi_color(o, (Rectangle) (*matrix), (Color) (black_trans), (Color) (black_trans),
				(Color) (black), (Color) (black));
			{
				Vector2 p = new Vector2();
				float S = (float) (hsva[1]);
				float V = (float) (hsva[2]);
				p.x = ((float) ((int) (matrix->x + S*matrix->w)));
				p.y = ((float) ((int) (matrix->y + (1.0f - V)*matrix->h)));
				nk_stroke_line(o, (float) (p.x - crosshair_size), (float) (p.y), (float) (p.x - 2), (float) (p.y),
					(float) (1.0f), (Color) (white));
				nk_stroke_line(o, (float) (p.x + crosshair_size + 1), (float) (p.y), (float) (p.x + 3), (float) (p.y),
					(float) (1.0f), (Color) (white));
				nk_stroke_line(o, (float) (p.x), (float) (p.y + crosshair_size + 1), (float) (p.x), (float) (p.y + 3),
					(float) (1.0f), (Color) (white));
				nk_stroke_line(o, (float) (p.x), (float) (p.y - crosshair_size), (float) (p.x), (float) (p.y - 2),
					(float) (1.0f), (Color) (white));
			}

		}

		public static void nk_push_table(nk_window win, nk_table tbl)
		{
			if (win.tables == null)
			{
				win.tables = tbl;
				tbl.next = null;
				tbl.prev = null;
				tbl.size = (uint) (0);
				win.table_count = (uint) (1);
				return;
			}

			win.tables.prev = tbl;
			tbl.next = win.tables;
			tbl.prev = null;
			tbl.size = (uint) (0);
			win.tables = tbl;
			win.table_count++;
		}

		public static void nk_remove_table(nk_window win, nk_table tbl)
		{
			if ((win.tables) == (tbl)) win.tables = tbl.next;
			if ((tbl.next) != null) tbl.next.prev = tbl.prev;
			if ((tbl.prev) != null) tbl.prev.next = tbl.next;
			tbl.next = null;
			tbl.prev = null;
		}

		public static uint* nk_find_value(nk_window win, uint name)
		{
			nk_table iter = win.tables;
			while ((iter) != null)
			{
				uint i = (uint) (0);
				uint size = (uint) (iter.size);
				for (i = (uint) (0); (i) < (size); ++i)
				{
					if ((iter.keys[i]) == (name))
					{
						iter.seq = (uint) (win.seq);
						return (uint*) iter.values + i;
					}
				}
				size = (uint) (51);
				iter = iter.next;
			}
			return null;
		}

	}
}