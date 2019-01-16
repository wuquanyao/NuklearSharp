using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace NuklearSharp
{
	public unsafe static partial class Nuklear
	{
		public unsafe partial class nk_style_text
		{
			public Color color = new Color();
			public Vector2 padding = new Vector2();
		}

		public unsafe partial class nk_style_button
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item active = new nk_style_item();
			public Color border_color = new Color();
			public Color text_background = new Color();
			public Color text_normal = new Color();
			public Color text_hover = new Color();
			public Color text_active = new Color();
			public uint text_alignment;
			public float border;
			public float rounding;
			public Vector2 padding = new Vector2();
			public Vector2 image_padding = new Vector2();
			public Vector2 touch_padding = new Vector2();
			public nk_handle userdata = new nk_handle();
			public NkDrawNotify draw_begin;
			public NkDrawNotify draw_end;
		}

		public unsafe partial class nk_style_toggle
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item active = new nk_style_item();
			public Color border_color = new Color();
			public nk_style_item cursor_normal = new nk_style_item();
			public nk_style_item cursor_hover = new nk_style_item();
			public Color text_normal = new Color();
			public Color text_hover = new Color();
			public Color text_active = new Color();
			public Color text_background = new Color();
			public uint text_alignment;
			public Vector2 padding = new Vector2();
			public Vector2 touch_padding = new Vector2();
			public float spacing;
			public float border;
			public nk_handle userdata = new nk_handle();
			public NkDrawNotify draw_begin;
			public NkDrawNotify draw_end;
		}

		public unsafe partial class nk_style_selectable
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item pressed = new nk_style_item();
			public nk_style_item normal_active = new nk_style_item();
			public nk_style_item hover_active = new nk_style_item();
			public nk_style_item pressed_active = new nk_style_item();
			public Color text_normal = new Color();
			public Color text_hover = new Color();
			public Color text_pressed = new Color();
			public Color text_normal_active = new Color();
			public Color text_hover_active = new Color();
			public Color text_pressed_active = new Color();
			public Color text_background = new Color();
			public uint text_alignment;
			public float rounding;
			public Vector2 padding = new Vector2();
			public Vector2 touch_padding = new Vector2();
			public Vector2 image_padding = new Vector2();
			public nk_handle userdata = new nk_handle();
			public NkDrawNotify draw_begin;
			public NkDrawNotify draw_end;
		}

		public unsafe partial class nk_style_slider
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item active = new nk_style_item();
			public Color border_color = new Color();
			public Color bar_normal = new Color();
			public Color bar_hover = new Color();
			public Color bar_active = new Color();
			public Color bar_filled = new Color();
			public nk_style_item cursor_normal = new nk_style_item();
			public nk_style_item cursor_hover = new nk_style_item();
			public nk_style_item cursor_active = new nk_style_item();
			public float border;
			public float rounding;
			public float bar_height;
			public Vector2 padding = new Vector2();
			public Vector2 spacing = new Vector2();
			public Vector2 cursor_size = new Vector2();
			public int show_buttons;
			public nk_style_button inc_button = new nk_style_button();
			public nk_style_button dec_button = new nk_style_button();
			public int inc_symbol;
			public int dec_symbol;
			public nk_handle userdata = new nk_handle();
			public NkDrawNotify draw_begin;
			public NkDrawNotify draw_end;
		}

		public unsafe partial class nk_style_progress
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item active = new nk_style_item();
			public Color border_color = new Color();
			public nk_style_item cursor_normal = new nk_style_item();
			public nk_style_item cursor_hover = new nk_style_item();
			public nk_style_item cursor_active = new nk_style_item();
			public Color cursor_border_color = new Color();
			public float rounding;
			public float border;
			public float cursor_border;
			public float cursor_rounding;
			public Vector2 padding = new Vector2();
			public nk_handle userdata = new nk_handle();
			public NkDrawNotify draw_begin;
			public NkDrawNotify draw_end;
		}

		public unsafe partial class nk_style_scrollbar
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item active = new nk_style_item();
			public Color border_color = new Color();
			public nk_style_item cursor_normal = new nk_style_item();
			public nk_style_item cursor_hover = new nk_style_item();
			public nk_style_item cursor_active = new nk_style_item();
			public Color cursor_border_color = new Color();
			public float border;
			public float rounding;
			public float border_cursor;
			public float rounding_cursor;
			public Vector2 padding = new Vector2();
			public int show_buttons;
			public nk_style_button inc_button = new nk_style_button();
			public nk_style_button dec_button = new nk_style_button();
			public int inc_symbol;
			public int dec_symbol;
			public nk_handle userdata = new nk_handle();
			public NkDrawNotify draw_begin;
			public NkDrawNotify draw_end;
		}

		public unsafe partial class nk_style_edit
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item active = new nk_style_item();
			public Color border_color = new Color();
			public nk_style_scrollbar scrollbar = new nk_style_scrollbar();
			public Color cursor_normal = new Color();
			public Color cursor_hover = new Color();
			public Color cursor_text_normal = new Color();
			public Color cursor_text_hover = new Color();
			public Color text_normal = new Color();
			public Color text_hover = new Color();
			public Color text_active = new Color();
			public Color selected_normal = new Color();
			public Color selected_hover = new Color();
			public Color selected_text_normal = new Color();
			public Color selected_text_hover = new Color();
			public float border;
			public float rounding;
			public float cursor_size;
			public Vector2 scrollbar_size = new Vector2();
			public Vector2 padding = new Vector2();
			public float row_padding;
		}

		public unsafe partial class nk_style_property
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item active = new nk_style_item();
			public Color border_color = new Color();
			public Color label_normal = new Color();
			public Color label_hover = new Color();
			public Color label_active = new Color();
			public int sym_left;
			public int sym_right;
			public float border;
			public float rounding;
			public Vector2 padding = new Vector2();
			public nk_style_edit edit = new nk_style_edit();
			public nk_style_button inc_button = new nk_style_button();
			public nk_style_button dec_button = new nk_style_button();
			public nk_handle userdata = new nk_handle();
			public NkDrawNotify draw_begin;
			public NkDrawNotify draw_end;
		}

		public unsafe partial class nk_style_chart
		{
			public nk_style_item background = new nk_style_item();
			public Color border_color = new Color();
			public Color selected_color = new Color();
			public Color color = new Color();
			public float border;
			public float rounding;
			public Vector2 padding = new Vector2();
		}

		public unsafe partial class nk_style_combo
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item active = new nk_style_item();
			public Color border_color = new Color();
			public Color label_normal = new Color();
			public Color label_hover = new Color();
			public Color label_active = new Color();
			public Color symbol_normal = new Color();
			public Color symbol_hover = new Color();
			public Color symbol_active = new Color();
			public nk_style_button button = new nk_style_button();
			public int sym_normal;
			public int sym_hover;
			public int sym_active;
			public float border;
			public float rounding;
			public Vector2 content_padding = new Vector2();
			public Vector2 button_padding = new Vector2();
			public Vector2 spacing = new Vector2();
		}

		public unsafe partial class nk_style_tab
		{
			public nk_style_item background = new nk_style_item();
			public Color border_color = new Color();
			public Color text = new Color();
			public nk_style_button tab_maximize_button = new nk_style_button();
			public nk_style_button tab_minimize_button = new nk_style_button();
			public nk_style_button node_maximize_button = new nk_style_button();
			public nk_style_button node_minimize_button = new nk_style_button();
			public int sym_minimize;
			public int sym_maximize;
			public float border;
			public float rounding;
			public float indent;
			public Vector2 padding = new Vector2();
			public Vector2 spacing = new Vector2();
		}

		public unsafe partial class nk_style_window_header
		{
			public nk_style_item normal = new nk_style_item();
			public nk_style_item hover = new nk_style_item();
			public nk_style_item active = new nk_style_item();
			public nk_style_button close_button = new nk_style_button();
			public nk_style_button minimize_button = new nk_style_button();
			public int close_symbol;
			public int minimize_symbol;
			public int maximize_symbol;
			public Color label_normal = new Color();
			public Color label_hover = new Color();
			public Color label_active = new Color();
			public int align;
			public Vector2 padding = new Vector2();
			public Vector2 label_padding = new Vector2();
			public Vector2 spacing = new Vector2();
		}

		public unsafe partial class nk_style_window
		{
			public nk_style_window_header header = new nk_style_window_header();
			public nk_style_item fixed_background = new nk_style_item();
			public Color background = new Color();
			public Color border_color = new Color();
			public Color popup_border_color = new Color();
			public Color combo_border_color = new Color();
			public Color contextual_border_color = new Color();
			public Color menu_border_color = new Color();
			public Color group_border_color = new Color();
			public Color tooltip_border_color = new Color();
			public nk_style_item scaler = new nk_style_item();
			public float border;
			public float combo_border;
			public float contextual_border;
			public float menu_border;
			public float group_border;
			public float tooltip_border;
			public float popup_border;
			public float min_row_height_padding;
			public float rounding;
			public Vector2 spacing = new Vector2();
			public Vector2 scrollbar_size = new Vector2();
			public Vector2 min_size = new Vector2();
			public Vector2 padding = new Vector2();
			public Vector2 group_padding = new Vector2();
			public Vector2 popup_padding = new Vector2();
			public Vector2 combo_padding = new Vector2();
			public Vector2 contextual_padding = new Vector2();
			public Vector2 menu_padding = new Vector2();
			public Vector2 tooltip_padding = new Vector2();
		}

		public static Vector2 nk_panel_get_padding(nk_style style, int type)
		{
			switch (type)
			{
				default:
				case NK_PANEL_WINDOW:
					return (Vector2) (style.Widthindow.padding);
				case NK_PANEL_GROUP:
					return (Vector2) (style.Widthindow.group_padding);
				case NK_PANEL_POPUP:
					return (Vector2) (style.Widthindow.popup_padding);
				case NK_PANEL_CONTEXTUAL:
					return (Vector2) (style.Widthindow.contextual_padding);
				case NK_PANEL_COMBO:
					return (Vector2) (style.Widthindow.combo_padding);
				case NK_PANEL_MENU:
					return (Vector2) (style.Widthindow.menu_padding);
				case NK_PANEL_TOOLTIP:
					return (Vector2) (style.Widthindow.menu_padding);
			}

		}

		public static float nk_panel_get_border(nk_style style, uint flags, int type)
		{
			if ((flags & NK_WINDOW_BORDER) != 0)
			{
				switch (type)
				{
					default:
					case NK_PANEL_WINDOW:
						return (float) (style.Widthindow.border);
					case NK_PANEL_GROUP:
						return (float) (style.Widthindow.group_border);
					case NK_PANEL_POPUP:
						return (float) (style.Widthindow.popup_border);
					case NK_PANEL_CONTEXTUAL:
						return (float) (style.Widthindow.contextual_border);
					case NK_PANEL_COMBO:
						return (float) (style.Widthindow.combo_border);
					case NK_PANEL_MENU:
						return (float) (style.Widthindow.menu_border);
					case NK_PANEL_TOOLTIP:
						return (float) (style.Widthindow.menu_border);
				}
			}
			else return (float) (0);
		}

		public static Color nk_panel_get_border_color(nk_style style, int type)
		{
			switch (type)
			{
				default:
				case NK_PANEL_WINDOW:
					return (Color) (style.Widthindow.border_color);
				case NK_PANEL_GROUP:
					return (Color) (style.Widthindow.group_border_color);
				case NK_PANEL_POPUP:
					return (Color) (style.Widthindow.popup_border_color);
				case NK_PANEL_CONTEXTUAL:
					return (Color) (style.Widthindow.contextual_border_color);
				case NK_PANEL_COMBO:
					return (Color) (style.Widthindow.combo_border_color);
				case NK_PANEL_MENU:
					return (Color) (style.Widthindow.menu_border_color);
				case NK_PANEL_TOOLTIP:
					return (Color) (style.Widthindow.menu_border_color);
			}

		}

		public static float nk_layout_row_calculate_usable_space(nk_style style, int type, float total_space, int columns)
		{
			float panel_padding;
			float panel_spacing;
			float panel_space;
			Vector2 spacing = new Vector2();
			Vector2 padding = new Vector2();
			spacing = (Vector2) (style.Widthindow.spacing);
			padding = (Vector2) (nk_panel_get_padding(style, (int) (type)));
			panel_padding = (float) (2*padding.X);
			panel_spacing = (float) ((float) ((columns - 1) < (0) ? (0) : (columns - 1))*spacing.X);
			panel_space = (float) (total_space - panel_padding - panel_spacing);
			return (float) (panel_space);
		}
	}
}