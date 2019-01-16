using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace NuklearSharp
{
	public unsafe partial class Nuklear
	{
		public static uint nk_convert(nk_context ctx, NkBuffer<nk_draw_command> cmds, NkBuffer<byte> vertices,
			NkBuffer<ushort> elements, nk_convert_config config)
		{
			uint res = (uint) (NK_CONVERT_SUCCESS);

			if ((((((ctx == null) || (cmds == null)) || (vertices == null)) || (elements == null)) || (config == null)) ||
			    (config.vertex_layout == null)) return (uint) (NK_CONVERT_INVALID_PARAM);
			nk_draw_list_setup(ctx.draw_list, config, cmds, vertices, elements, (int) (config.line_AA), (int) (config.shape_AA));
			var top_window = nk__begin(ctx);

			int cnt = 0;
			for (var cmd = top_window.buffer.first; cmd != null; cmd = cmd.next)
			{
				switch (cmd.Heighteader.type)
				{
					case NK_COMMAND_NOP:
						break;
					case NK_COMMAND_SCISSOR:
					{
						nk_command_scissor s = (nk_command_scissor) (cmd);
						nk_draw_list_add_clip(ctx.draw_list,
							(RectangleF) (RectangleF_((float) (s.X), (float) (s.Y), (float) (s.Width), (float) (s.Height))));
					}
						break;
					case NK_COMMAND_LINE:
					{
						nk_command_line l = (nk_command_line) (cmd);
						nk_draw_list_stroke_line(ctx.draw_list, (Vector2) (new Vector2((float) (l.begin.X), (float) (l.begin.Y))),
							(Vector2) (new Vector2((float) (l.end.X), (float) (l.end.Y))), (Color) (l.color), (float) (l.line_thickness));
					}
						break;

					case NK_COMMAND_RECT:
					{
						nk_command_rect r = (nk_command_rect) (cmd);
						nk_draw_list_stroke_rect(ctx.draw_list,
							(RectangleF) (RectangleF_((float) (r.X), (float) (r.Y), (float) (r.Width), (float) (r.Height))), (Color) (r.color),
							(float) (r.rounding), (float) (r.line_thickness));
					}
						break;
					case NK_COMMAND_RECT_FILLED:
					{
						nk_command_rect_filled r = (nk_command_rect_filled) (cmd);
						nk_draw_list_fill_rect(ctx.draw_list,
							(RectangleF) (RectangleF_((float) (r.X), (float) (r.Y), (float) (r.Width), (float) (r.Height))), (Color) (r.color),
							(float) (r.rounding));
					}
						break;
					case NK_COMMAND_RECT_MULTI_COLOR:
					{
						nk_command_rect_multi_color r = (nk_command_rect_multi_color) (cmd);
						nk_draw_list_fill_rect_multi_color(ctx.draw_list,
							(RectangleF) (RectangleF_((float) (r.X), (float) (r.Y), (float) (r.Width), (float) (r.Height))), (Color) (r.left),
							(Color) (r.top), (Color) (r.right), (Color) (r.bottom));
					}
						break;

					case NK_COMMAND_CIRCLE_FILLED:
					{
						nk_command_circle_filled c = (nk_command_circle_filled) (cmd);
						nk_draw_list_fill_circle(ctx.draw_list,
							(Vector2) (new Vector2((float) ((float) (c.X) + (float) (c.Width)/2), (float) ((float) (c.Y) + (float) (c.Height)/2))),
							(float) ((float) (c.Width)/2), (Color) (c.color), (uint) (config.circle_segment_count));
					}
						break;

					case NK_COMMAND_TRIANGLE_FILLED:
					{
						nk_command_triangle_filled t = (nk_command_triangle_filled) (cmd);
						nk_draw_list_fill_triangle(ctx.draw_list, (Vector2) (new Vector2((float) (t.a.X), (float) (t.a.Y))),
							(Vector2) (new Vector2((float) (t.b.X), (float) (t.b.Y))), (Vector2) (new Vector2((float) (t.c.X), (float) (t.c.Y))),
							(Color) (t.color));
					}
						break;
					case NK_COMMAND_POLYGON:
					{
						int i;
						nk_command_polygon p = (nk_command_polygon) (cmd);
						for (i = (int) (0); (i) < (p.point_count); ++i)
						{
							Vector2 pnt = (Vector2) (new Vector2((float) (p.points[i].X), (float) (p.points[i].Y)));
							nk_draw_list_path_line_to(ctx.draw_list, (Vector2) (pnt));
						}
						nk_draw_list_path_stroke(ctx.draw_list, (Color) (p.color), (int) (NK_STROKE_CLOSED), (float) (p.line_thickness));
					}
						break;
					case NK_COMMAND_POLYGON_FILLED:
					{
						int i;
						nk_command_polygon_filled p = (nk_command_polygon_filled) (cmd);
						for (i = (int) (0); (i) < (p.point_count); ++i)
						{
							Vector2 pnt = (Vector2) (new Vector2((float) (p.points[i].X), (float) (p.points[i].Y)));
							nk_draw_list_path_line_to(ctx.draw_list, (Vector2) (pnt));
						}
						nk_draw_list_path_fill(ctx.draw_list, (Color) (p.color));
					}
						break;
					case NK_COMMAND_POLYLINE:
					{
						int i;
						nk_command_polyline p = (nk_command_polyline) (cmd);
						for (i = (int) (0); (i) < (p.point_count); ++i)
						{
							Vector2 pnt = (Vector2) (new Vector2((float) (p.points[i].X), (float) (p.points[i].Y)));
							nk_draw_list_path_line_to(ctx.draw_list, (Vector2) (pnt));
						}
						nk_draw_list_path_stroke(ctx.draw_list, (Color) (p.color), (int) (NK_STROKE_OPEN), (float) (p.line_thickness));
					}
						break;
					case NK_COMMAND_TEXT:
					{
						nk_command_text t = (nk_command_text) (cmd);
						nk_draw_list_add_text(ctx.draw_list, t.font,
							(RectangleF) (RectangleF_((float) (t.X), (float) (t.Y), (float) (t.Width), (float) (t.Height))), t._string_, (int) (t.length),
							(float) (t.Height), (Color) (t.foreground));
					}
						break;
					case NK_COMMAND_IMAGE:
					{
						nk_command_image i = (nk_command_image) (cmd);
						nk_draw_list_add_image(ctx.draw_list, (nk_image) (i.img),
							(RectangleF) (RectangleF_((float) (i.X), (float) (i.Y), (float) (i.Width), (float) (i.Height))), (Color) (i.col));
					}
						break;

					default:
						break;
				}
				++cnt;
			}

			return res;
		}

		public static void nk_input_begin(nk_context ctx)
		{
			int i;
			nk_input _in_;
			if (ctx == null) return;
			_in_ = ctx.input;
			for (i = (int) (0); (i) < (NK_BUTTON_MAX); ++i)
			{
				_in_.mouse.buttons[i].clicked = (uint) (0);
			}
			_in_.keyboard.text_len = (int) (0);
			_in_.mouse.scroll_delta = (Vector2) (new Vector2((float) (0), (float) (0)));
			_in_.mouse.prev.X = (float) (_in_.mouse.pos.X);
			_in_.mouse.prev.Y = (float) (_in_.mouse.pos.Y);
			_in_.mouse.delta.X = (float) (0);
			_in_.mouse.delta.Y = (float) (0);
			for (i = (int) (0); (i) < (NK_KEY_MAX); i++)
			{
				_in_.keyboard.keys[i].clicked = (uint) (0);
			}
		}

		public static void nk_input_end(nk_context ctx)
		{
			nk_input _in_;
			if (ctx == null) return;
			_in_ = ctx.input;
			if ((_in_.mouse.grab) != 0) _in_.mouse.grab = (byte) (0);
			if ((_in_.mouse.ungrab) != 0)
			{
				_in_.mouse.grabbed = (byte) (0);
				_in_.mouse.ungrab = (byte) (0);
				_in_.mouse.grab = (byte) (0);
			}

		}

		public static void nk_input_motion(nk_context ctx, int x, int y)
		{
			nk_input _in_;
			if (ctx == null) return;
			_in_ = ctx.input;
			_in_.mouse.pos.X = ((float) (x));
			_in_.mouse.pos.Y = ((float) (y));
			_in_.mouse.delta.X = (float) (_in_.mouse.pos.X - _in_.mouse.prev.X);
			_in_.mouse.delta.Y = (float) (_in_.mouse.pos.Y - _in_.mouse.prev.Y);
		}

		public static void nk_input_key(nk_context ctx, int key, int down)
		{
			nk_input _in_;
			if (ctx == null) return;
			_in_ = ctx.input;
			if (_in_.keyboard.keys[key].down != down) _in_.keyboard.keys[key].clicked++;
			_in_.keyboard.keys[key].down = (int) (down);
		}

		public static void nk_input_button(nk_context ctx, int id, int x, int y, int down)
		{
			nk_input _in_;
			if (ctx == null) return;
			_in_ = ctx.input;
			if ((_in_.mouse.buttons[id].down) == (down)) return;
			fixed (nk_mouse_button* btn = &_in_.mouse.buttons[id])
			{
				btn->clicked_pos.X = ((float)(x));
				btn->clicked_pos.Y = ((float)(y));
				btn->down = (int)(down);
				btn->clicked++;
			}
		}

		public static void nk_input_scroll(nk_context ctx, Vector2 val)
		{
			if (ctx == null) return;
			ctx.input.mouse.scroll_delta.X += (float) (val.X);
			ctx.input.mouse.scroll_delta.Y += (float) (val.Y);
		}

		public static void nk_input_glyph(nk_context ctx, char* glyph)
		{
			int len = (int) (0);
			char unicode;
			nk_input _in_;
			if (ctx == null) return;
			_in_ = ctx.input;
			len = (int) (nk_utf_decode(glyph, &unicode, (int) (4)));
			if (((len) != 0) && ((_in_.keyboard.text_len + len) < (16)))
			{
				nk_utf_encode(unicode, (char*) _in_.keyboard.text + _in_.keyboard.text_len, (int) (16 - _in_.keyboard.text_len));
				_in_.keyboard.text_len += (int) (len);
			}

		}

		public static void nk_input_char(nk_context ctx, char c)
		{
			char* glyph = stackalloc char[4];
			if (ctx == null) return;
			glyph[0] = c;
			nk_input_glyph(ctx, glyph);
		}

		public static void nk_input_unicode(nk_context ctx, char unicode)
		{
			char* rune = stackalloc char[4];
			if (ctx == null) return;
			nk_utf_encode(unicode, rune, (int) (4));
			nk_input_glyph(ctx, rune);
		}

		public static void nk_style_default(nk_context ctx)
		{
			nk_style_from_table(ctx, null);
		}

		public static void nk_style_from_table(nk_context ctx, Color[] table)
		{
			nk_style style;
			nk_style_text text;
			nk_style_button button;
			nk_style_toggle toggle;
			nk_style_selectable select;
			nk_style_slider slider;
			nk_style_progress prog;
			nk_style_scrollbar scroll;
			nk_style_edit edit;
			nk_style_property property;
			nk_style_combo combo;
			nk_style_chart chart;
			nk_style_tab tab;
			nk_style_window win;
			if (ctx == null) return;
			style = ctx.style;
			table = (table == null) ? nk_default_color_style : table;
			text = style.text;
			text.color = (Color) (table[Color_TEXT]);
			text.padding = (Vector2) (new Vector2((float) (0), (float) (0)));
			button = style.button;

			button.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_BUTTON])));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_BUTTON_HOVER])));
			button.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_BUTTON_ACTIVE])));
			button.border_color = (Color) (table[Color_BORDER]);
			button.text_background = (Color) (table[Color_BUTTON]);
			button.text_normal = (Color) (table[Color_TEXT]);
			button.text_hover = (Color) (table[Color_TEXT]);
			button.text_active = (Color) (table[Color_TEXT]);
			button.padding = (Vector2) (new Vector2((float) (2.0f), (float) (2.0f)));
			button.image_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (1.0f);
			button.rounding = (float) (4.0f);
			button.draw_begin = null;
			button.draw_end = null;
			button = style.contextual_button;

			button.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_WINDOW])));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_BUTTON_HOVER])));
			button.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_BUTTON_ACTIVE])));
			button.border_color = (Color) (table[Color_WINDOW]);
			button.text_background = (Color) (table[Color_WINDOW]);
			button.text_normal = (Color) (table[Color_TEXT]);
			button.text_hover = (Color) (table[Color_TEXT]);
			button.text_active = (Color) (table[Color_TEXT]);
			button.padding = (Vector2) (new Vector2((float) (2.0f), (float) (2.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (0.0f);
			button.rounding = (float) (0.0f);
			button.draw_begin = null;
			button.draw_end = null;
			button = style.menu_button;

			button.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_WINDOW])));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_WINDOW])));
			button.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_WINDOW])));
			button.border_color = (Color) (table[Color_WINDOW]);
			button.text_background = (Color) (table[Color_WINDOW]);
			button.text_normal = (Color) (table[Color_TEXT]);
			button.text_hover = (Color) (table[Color_TEXT]);
			button.text_active = (Color) (table[Color_TEXT]);
			button.padding = (Vector2) (new Vector2((float) (2.0f), (float) (2.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (0.0f);
			button.rounding = (float) (1.0f);
			button.draw_begin = null;
			button.draw_end = null;
			toggle = style.checkbox;

			toggle.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE])));
			toggle.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE_HOVER])));
			toggle.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE_HOVER])));
			toggle.cursor_normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE_CURSOR])));
			toggle.cursor_hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE_CURSOR])));
			toggle.userdata = (nk_handle) (nk_handle_ptr(null));
			toggle.text_background = (Color) (table[Color_WINDOW]);
			toggle.text_normal = (Color) (table[Color_TEXT]);
			toggle.text_hover = (Color) (table[Color_TEXT]);
			toggle.text_active = (Color) (table[Color_TEXT]);
			toggle.padding = (Vector2) (new Vector2((float) (2.0f), (float) (2.0f)));
			toggle.touch_padding = (Vector2) (new Vector2((float) (0), (float) (0)));
			toggle.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			toggle.border = (float) (0.0f);
			toggle.spacing = (float) (4);
			toggle = style.option;

			toggle.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE])));
			toggle.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE_HOVER])));
			toggle.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE_HOVER])));
			toggle.cursor_normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE_CURSOR])));
			toggle.cursor_hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_TOGGLE_CURSOR])));
			toggle.userdata = (nk_handle) (nk_handle_ptr(null));
			toggle.text_background = (Color) (table[Color_WINDOW]);
			toggle.text_normal = (Color) (table[Color_TEXT]);
			toggle.text_hover = (Color) (table[Color_TEXT]);
			toggle.text_active = (Color) (table[Color_TEXT]);
			toggle.padding = (Vector2) (new Vector2((float) (3.0f), (float) (3.0f)));
			toggle.touch_padding = (Vector2) (new Vector2((float) (0), (float) (0)));
			toggle.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			toggle.border = (float) (0.0f);
			toggle.spacing = (float) (4);
			select = style.selectable;

			select.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_SELECT])));
			select.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_SELECT])));
			select.pressed = (nk_style_item) (nk_style_item_color((Color) (table[Color_SELECT])));
			select.normal_active = (nk_style_item) (nk_style_item_color((Color) (table[Color_SELECT_ACTIVE])));
			select.hover_active = (nk_style_item) (nk_style_item_color((Color) (table[Color_SELECT_ACTIVE])));
			select.pressed_active = (nk_style_item) (nk_style_item_color((Color) (table[Color_SELECT_ACTIVE])));
			select.text_normal = (Color) (table[Color_TEXT]);
			select.text_hover = (Color) (table[Color_TEXT]);
			select.text_pressed = (Color) (table[Color_TEXT]);
			select.text_normal_active = (Color) (table[Color_TEXT]);
			select.text_hover_active = (Color) (table[Color_TEXT]);
			select.text_pressed_active = (Color) (table[Color_TEXT]);
			select.padding = (Vector2) (new Vector2((float) (2.0f), (float) (2.0f)));
			select.touch_padding = (Vector2) (new Vector2((float) (0), (float) (0)));
			select.userdata = (nk_handle) (nk_handle_ptr(null));
			select.rounding = (float) (0.0f);
			select.draw_begin = null;
			select.draw_end = null;
			slider = style.slider;

			slider.normal = (nk_style_item) (nk_style_item_hide());
			slider.hover = (nk_style_item) (nk_style_item_hide());
			slider.active = (nk_style_item) (nk_style_item_hide());
			slider.bar_normal = (Color) (table[Color_SLIDER]);
			slider.bar_hover = (Color) (table[Color_SLIDER]);
			slider.bar_active = (Color) (table[Color_SLIDER]);
			slider.bar_filled = (Color) (table[Color_SLIDER_CURSOR]);
			slider.cursor_normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_SLIDER_CURSOR])));
			slider.cursor_hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_SLIDER_CURSOR_HOVER])));
			slider.cursor_active = (nk_style_item) (nk_style_item_color((Color) (table[Color_SLIDER_CURSOR_ACTIVE])));
			slider.inc_symbol = (int) (NK_SYMBOL_TRIANGLE_RIGHT);
			slider.dec_symbol = (int) (NK_SYMBOL_TRIANGLE_LEFT);
			slider.cursor_size = (Vector2) (new Vector2((float) (16), (float) (16)));
			slider.padding = (Vector2) (new Vector2((float) (2), (float) (2)));
			slider.spacing = (Vector2) (new Vector2((float) (2), (float) (2)));
			slider.userdata = (nk_handle) (nk_handle_ptr(null));
			slider.show_buttons = (int) (nk_false);
			slider.bar_height = (float) (8);
			slider.rounding = (float) (0);
			slider.draw_begin = null;
			slider.draw_end = null;
			button = style.slider.inc_button;
			button.normal = (nk_style_item) (nk_style_item_color((Color) (nk_rgb((int) (40), (int) (40), (int) (40)))));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (nk_rgb((int) (42), (int) (42), (int) (42)))));
			button.active = (nk_style_item) (nk_style_item_color((Color) (nk_rgb((int) (44), (int) (44), (int) (44)))));
			button.border_color = (Color) (nk_rgb((int) (65), (int) (65), (int) (65)));
			button.text_background = (Color) (nk_rgb((int) (40), (int) (40), (int) (40)));
			button.text_normal = (Color) (nk_rgb((int) (175), (int) (175), (int) (175)));
			button.text_hover = (Color) (nk_rgb((int) (175), (int) (175), (int) (175)));
			button.text_active = (Color) (nk_rgb((int) (175), (int) (175), (int) (175)));
			button.padding = (Vector2) (new Vector2((float) (8.0f), (float) (8.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (1.0f);
			button.rounding = (float) (0.0f);
			button.draw_begin = null;
			button.draw_end = null;
			style.slider.dec_button = (nk_style_button) (style.slider.inc_button);
			prog = style.progress;

			prog.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_SLIDER])));
			prog.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_SLIDER])));
			prog.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_SLIDER])));
			prog.cursor_normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_SLIDER_CURSOR])));
			prog.cursor_hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_SLIDER_CURSOR_HOVER])));
			prog.cursor_active = (nk_style_item) (nk_style_item_color((Color) (table[Color_SLIDER_CURSOR_ACTIVE])));
			prog.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			prog.cursor_border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			prog.userdata = (nk_handle) (nk_handle_ptr(null));
			prog.padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			prog.rounding = (float) (0);
			prog.border = (float) (0);
			prog.cursor_rounding = (float) (0);
			prog.cursor_border = (float) (0);
			prog.draw_begin = null;
			prog.draw_end = null;
			scroll = style.scrollh;

			scroll.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_SCROLLBAR])));
			scroll.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_SCROLLBAR])));
			scroll.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_SCROLLBAR])));
			scroll.cursor_normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_SCROLLBAR_CURSOR])));
			scroll.cursor_hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_SCROLLBAR_CURSOR_HOVER])));
			scroll.cursor_active = (nk_style_item) (nk_style_item_color((Color) (table[Color_SCROLLBAR_CURSOR_ACTIVE])));
			scroll.dec_symbol = (int) (NK_SYMBOL_CIRCLE_SOLID);
			scroll.inc_symbol = (int) (NK_SYMBOL_CIRCLE_SOLID);
			scroll.userdata = (nk_handle) (nk_handle_ptr(null));
			scroll.border_color = (Color) (table[Color_SCROLLBAR]);
			scroll.cursor_border_color = (Color) (table[Color_SCROLLBAR]);
			scroll.padding = (Vector2) (new Vector2((float) (0), (float) (0)));
			scroll.show_buttons = (int) (nk_false);
			scroll.border = (float) (0);
			scroll.rounding = (float) (0);
			scroll.border_cursor = (float) (0);
			scroll.rounding_cursor = (float) (0);
			scroll.draw_begin = null;
			scroll.draw_end = null;
			style.scrollv = (nk_style_scrollbar) (style.scrollh);
			button = style.scrollh.inc_button;
			button.normal = (nk_style_item) (nk_style_item_color((Color) (nk_rgb((int) (40), (int) (40), (int) (40)))));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (nk_rgb((int) (42), (int) (42), (int) (42)))));
			button.active = (nk_style_item) (nk_style_item_color((Color) (nk_rgb((int) (44), (int) (44), (int) (44)))));
			button.border_color = (Color) (nk_rgb((int) (65), (int) (65), (int) (65)));
			button.text_background = (Color) (nk_rgb((int) (40), (int) (40), (int) (40)));
			button.text_normal = (Color) (nk_rgb((int) (175), (int) (175), (int) (175)));
			button.text_hover = (Color) (nk_rgb((int) (175), (int) (175), (int) (175)));
			button.text_active = (Color) (nk_rgb((int) (175), (int) (175), (int) (175)));
			button.padding = (Vector2) (new Vector2((float) (4.0f), (float) (4.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (1.0f);
			button.rounding = (float) (0.0f);
			button.draw_begin = null;
			button.draw_end = null;
			style.scrollh.dec_button = (nk_style_button) (style.scrollh.inc_button);
			style.scrollv.inc_button = (nk_style_button) (style.scrollh.inc_button);
			style.scrollv.dec_button = (nk_style_button) (style.scrollh.inc_button);
			edit = style.edit;

			edit.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_EDIT])));
			edit.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_EDIT])));
			edit.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_EDIT])));
			edit.cursor_normal = (Color) (table[Color_TEXT]);
			edit.cursor_hover = (Color) (table[Color_TEXT]);
			edit.cursor_text_normal = (Color) (table[Color_EDIT]);
			edit.cursor_text_hover = (Color) (table[Color_EDIT]);
			edit.border_color = (Color) (table[Color_BORDER]);
			edit.text_normal = (Color) (table[Color_TEXT]);
			edit.text_hover = (Color) (table[Color_TEXT]);
			edit.text_active = (Color) (table[Color_TEXT]);
			edit.selected_normal = (Color) (table[Color_TEXT]);
			edit.selected_hover = (Color) (table[Color_TEXT]);
			edit.selected_text_normal = (Color) (table[Color_EDIT]);
			edit.selected_text_hover = (Color) (table[Color_EDIT]);
			edit.scrollbar_size = (Vector2) (new Vector2((float) (10), (float) (10)));
			edit.scrollbar = (nk_style_scrollbar) (style.scrollv);
			edit.padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			edit.row_padding = (float) (2);
			edit.cursor_size = (float) (4);
			edit.border = (float) (1);
			edit.rounding = (float) (0);
			property = style.property;

			property.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_PROPERTY])));
			property.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_PROPERTY])));
			property.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_PROPERTY])));
			property.border_color = (Color) (table[Color_BORDER]);
			property.label_normal = (Color) (table[Color_TEXT]);
			property.label_hover = (Color) (table[Color_TEXT]);
			property.label_active = (Color) (table[Color_TEXT]);
			property.sym_left = (int) (NK_SYMBOL_TRIANGLE_LEFT);
			property.sym_right = (int) (NK_SYMBOL_TRIANGLE_RIGHT);
			property.userdata = (nk_handle) (nk_handle_ptr(null));
			property.padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			property.border = (float) (1);
			property.rounding = (float) (10);
			property.draw_begin = null;
			property.draw_end = null;
			button = style.property.dec_button;

			button.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_PROPERTY])));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_PROPERTY])));
			button.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_PROPERTY])));
			button.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			button.text_background = (Color) (table[Color_PROPERTY]);
			button.text_normal = (Color) (table[Color_TEXT]);
			button.text_hover = (Color) (table[Color_TEXT]);
			button.text_active = (Color) (table[Color_TEXT]);
			button.padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (0.0f);
			button.rounding = (float) (0.0f);
			button.draw_begin = null;
			button.draw_end = null;
			style.property.inc_button = (nk_style_button) (style.property.dec_button);
			edit = style.property.edit;

			edit.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_PROPERTY])));
			edit.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_PROPERTY])));
			edit.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_PROPERTY])));
			edit.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			edit.cursor_normal = (Color) (table[Color_TEXT]);
			edit.cursor_hover = (Color) (table[Color_TEXT]);
			edit.cursor_text_normal = (Color) (table[Color_EDIT]);
			edit.cursor_text_hover = (Color) (table[Color_EDIT]);
			edit.text_normal = (Color) (table[Color_TEXT]);
			edit.text_hover = (Color) (table[Color_TEXT]);
			edit.text_active = (Color) (table[Color_TEXT]);
			edit.selected_normal = (Color) (table[Color_TEXT]);
			edit.selected_hover = (Color) (table[Color_TEXT]);
			edit.selected_text_normal = (Color) (table[Color_EDIT]);
			edit.selected_text_hover = (Color) (table[Color_EDIT]);
			edit.padding = (Vector2) (new Vector2((float) (0), (float) (0)));
			edit.cursor_size = (float) (8);
			edit.border = (float) (0);
			edit.rounding = (float) (0);
			chart = style.chart;

			chart.background = (nk_style_item) (nk_style_item_color((Color) (table[Color_CHART])));
			chart.border_color = (Color) (table[Color_BORDER]);
			chart.selected_color = (Color) (table[Color_CHART_COLOR_HIGHLIGHT]);
			chart.color = (Color) (table[Color_CHART_COLOR]);
			chart.padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			chart.border = (float) (0);
			chart.rounding = (float) (0);
			combo = style.combo;
			combo.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_COMBO])));
			combo.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_COMBO])));
			combo.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_COMBO])));
			combo.border_color = (Color) (table[Color_BORDER]);
			combo.label_normal = (Color) (table[Color_TEXT]);
			combo.label_hover = (Color) (table[Color_TEXT]);
			combo.label_active = (Color) (table[Color_TEXT]);
			combo.sym_normal = (int) (NK_SYMBOL_TRIANGLE_DOWN);
			combo.sym_hover = (int) (NK_SYMBOL_TRIANGLE_DOWN);
			combo.sym_active = (int) (NK_SYMBOL_TRIANGLE_DOWN);
			combo.content_padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			combo.button_padding = (Vector2) (new Vector2((float) (0), (float) (4)));
			combo.spacing = (Vector2) (new Vector2((float) (4), (float) (0)));
			combo.border = (float) (1);
			combo.rounding = (float) (0);
			button = style.combo.button;

			button.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_COMBO])));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_COMBO])));
			button.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_COMBO])));
			button.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			button.text_background = (Color) (table[Color_COMBO]);
			button.text_normal = (Color) (table[Color_TEXT]);
			button.text_hover = (Color) (table[Color_TEXT]);
			button.text_active = (Color) (table[Color_TEXT]);
			button.padding = (Vector2) (new Vector2((float) (2.0f), (float) (2.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (0.0f);
			button.rounding = (float) (0.0f);
			button.draw_begin = null;
			button.draw_end = null;
			tab = style.tab;
			tab.background = (nk_style_item) (nk_style_item_color((Color) (table[Color_TAB_HEADER])));
			tab.border_color = (Color) (table[Color_BORDER]);
			tab.text = (Color) (table[Color_TEXT]);
			tab.sym_minimize = (int) (NK_SYMBOL_TRIANGLE_RIGHT);
			tab.sym_maximize = (int) (NK_SYMBOL_TRIANGLE_DOWN);
			tab.padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			tab.spacing = (Vector2) (new Vector2((float) (4), (float) (4)));
			tab.indent = (float) (10.0f);
			tab.border = (float) (1);
			tab.rounding = (float) (0);
			button = style.tab.tab_minimize_button;

			button.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_TAB_HEADER])));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_TAB_HEADER])));
			button.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_TAB_HEADER])));
			button.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			button.text_background = (Color) (table[Color_TAB_HEADER]);
			button.text_normal = (Color) (table[Color_TEXT]);
			button.text_hover = (Color) (table[Color_TEXT]);
			button.text_active = (Color) (table[Color_TEXT]);
			button.padding = (Vector2) (new Vector2((float) (2.0f), (float) (2.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (0.0f);
			button.rounding = (float) (0.0f);
			button.draw_begin = null;
			button.draw_end = null;
			style.tab.tab_maximize_button = (nk_style_button) (button);
			button = style.tab.node_minimize_button;

			button.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_WINDOW])));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_WINDOW])));
			button.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_WINDOW])));
			button.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			button.text_background = (Color) (table[Color_TAB_HEADER]);
			button.text_normal = (Color) (table[Color_TEXT]);
			button.text_hover = (Color) (table[Color_TEXT]);
			button.text_active = (Color) (table[Color_TEXT]);
			button.padding = (Vector2) (new Vector2((float) (2.0f), (float) (2.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (0.0f);
			button.rounding = (float) (0.0f);
			button.draw_begin = null;
			button.draw_end = null;
			style.tab.node_maximize_button = (nk_style_button) (button);
			win = style.Widthindow;
			win.Heighteader.align = (int) (NK_HEADER_RIGHT);
			win.Heighteader.close_symbol = (int) (NK_SYMBOL_X);
			win.Heighteader.minimize_symbol = (int) (NK_SYMBOL_MINUS);
			win.Heighteader.maximize_symbol = (int) (NK_SYMBOL_PLUS);
			win.Heighteader.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_HEADER])));
			win.Heighteader.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_HEADER])));
			win.Heighteader.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_HEADER])));
			win.Heighteader.label_normal = (Color) (table[Color_TEXT]);
			win.Heighteader.label_hover = (Color) (table[Color_TEXT]);
			win.Heighteader.label_active = (Color) (table[Color_TEXT]);
			win.Heighteader.label_padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			win.Heighteader.padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			win.Heighteader.spacing = (Vector2) (new Vector2((float) (0), (float) (0)));
			button = style.Widthindow.Heighteader.close_button;

			button.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_HEADER])));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_HEADER])));
			button.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_HEADER])));
			button.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			button.text_background = (Color) (table[Color_HEADER]);
			button.text_normal = (Color) (table[Color_TEXT]);
			button.text_hover = (Color) (table[Color_TEXT]);
			button.text_active = (Color) (table[Color_TEXT]);
			button.padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (0.0f);
			button.rounding = (float) (0.0f);
			button.draw_begin = null;
			button.draw_end = null;
			button = style.Widthindow.Heighteader.minimize_button;

			button.normal = (nk_style_item) (nk_style_item_color((Color) (table[Color_HEADER])));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (table[Color_HEADER])));
			button.active = (nk_style_item) (nk_style_item_color((Color) (table[Color_HEADER])));
			button.border_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			button.text_background = (Color) (table[Color_HEADER]);
			button.text_normal = (Color) (table[Color_TEXT]);
			button.text_hover = (Color) (table[Color_TEXT]);
			button.text_active = (Color) (table[Color_TEXT]);
			button.padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.touch_padding = (Vector2) (new Vector2((float) (0.0f), (float) (0.0f)));
			button.userdata = (nk_handle) (nk_handle_ptr(null));
			button.text_alignment = (uint) (NK_TEXT_CENTERED);
			button.border = (float) (0.0f);
			button.rounding = (float) (0.0f);
			button.draw_begin = null;
			button.draw_end = null;
			win.background = (Color) (table[Color_WINDOW]);
			win.fixed_background = (nk_style_item) (nk_style_item_color((Color) (table[Color_WINDOW])));
			win.border_color = (Color) (table[Color_BORDER]);
			win.popup_border_color = (Color) (table[Color_BORDER]);
			win.combo_border_color = (Color) (table[Color_BORDER]);
			win.contextual_border_color = (Color) (table[Color_BORDER]);
			win.menu_border_color = (Color) (table[Color_BORDER]);
			win.group_border_color = (Color) (table[Color_BORDER]);
			win.tooltip_border_color = (Color) (table[Color_BORDER]);
			win.scaler = (nk_style_item) (nk_style_item_color((Color) (table[Color_TEXT])));
			win.rounding = (float) (0.0f);
			win.spacing = (Vector2) (new Vector2((float) (4), (float) (4)));
			win.scrollbar_size = (Vector2) (new Vector2((float) (10), (float) (10)));
			win.min_size = (Vector2) (new Vector2((float) (64), (float) (64)));
			win.combo_border = (float) (1.0f);
			win.contextual_border = (float) (1.0f);
			win.menu_border = (float) (1.0f);
			win.group_border = (float) (1.0f);
			win.tooltip_border = (float) (1.0f);
			win.popup_border = (float) (1.0f);
			win.border = (float) (2.0f);
			win.min_row_height_padding = (float) (8);
			win.padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			win.group_padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			win.popup_padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			win.combo_padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			win.contextual_padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			win.menu_padding = (Vector2) (new Vector2((float) (4), (float) (4)));
			win.tooltip_padding = (Vector2) (new Vector2((float) (4), (float) (4)));
		}

		public static void nk_style_set_font(nk_context ctx, nk_user_font font)
		{
			nk_style style;
			if (ctx == null) return;
			style = ctx.style;
			style.font = font;
			ctx.stacks.fonts.Heightead = (int) (0);
			if ((ctx.current) != null) nk_layout_reset_min_row_height(ctx);
		}

		public static int nk_style_push_font(nk_context ctx, nk_user_font font)
		{
			nk_config_stack_user_font font_stack;
			nk_config_stack_user_font_element element;
			if (ctx == null) return (int) (0);
			font_stack = ctx.stacks.fonts;
			if ((font_stack.Heightead) >= (int) font_stack.elements.Length) return (int) (0);
			element = font_stack.elements[font_stack.Heightead++];
			element.address = ctx.style.font;
			element.old_value = ctx.style.font;
			ctx.style.font = font;
			return (int) (1);
		}

		public static int nk_style_pop_font(nk_context ctx)
		{
			nk_config_stack_user_font font_stack;
			nk_config_stack_user_font_element element;
			if (ctx == null) return (int) (0);
			font_stack = ctx.stacks.fonts;
			if ((font_stack.Heightead) < (1)) return (int) (0);
			element = font_stack.elements[--font_stack.Heightead];
			element.address = element.old_value;
			return (int) (1);
		}

		public static int nk_style_push_style_item(nk_context ctx, nk_style_item address, nk_style_item value)
		{
			nk_config_stack_style_item type_stack;
			nk_config_stack_style_item_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.style_items;
			if ((type_stack.Heightead) >= (int) type_stack.elements.Length) return (int) (0);
			element = type_stack.elements[(type_stack.Heightead++)];
			element.address = address;
			element.old_value = (nk_style_item) (address);
			address = (nk_style_item) (value);
			return (int) (1);
		}

		public static int nk_style_push_float(nk_context ctx, float* address, float value)
		{
			nk_config_stack_float type_stack;
			nk_config_stack_float_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.floats;
			if ((type_stack.Heightead) >= (int) type_stack.elements.Length) return (int) (0);
			element = type_stack.elements[(type_stack.Heightead++)];
			element.address = address;
			element.old_value = (float) (*address);
			*address = (float) (value);
			return (int) (1);
		}

		public static int nk_style_push_vec2(nk_context ctx, Vector2* address, Vector2 value)
		{
			nk_config_stack_vec2 type_stack;
			nk_config_stack_vec2_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.vectors;
			if ((type_stack.Heightead) >= (int) type_stack.elements.Length) return (int) (0);
			element = type_stack.elements[(type_stack.Heightead++)];
			element.address = address;
			element.old_value = (Vector2) (*address);
			*address = (Vector2) (value);
			return (int) (1);
		}

		public static int nk_style_push_flags(nk_context ctx, uint* address, uint value)
		{
			nk_config_stack_flags type_stack;
			nk_config_stack_flags_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.flags;
			if ((type_stack.Heightead) >= (int) type_stack.elements.Length) return (int) (0);
			element = type_stack.elements[(type_stack.Heightead++)];
			element.address = address;
			element.old_value = (uint) (*address);
			*address = (uint) (value);
			return (int) (1);
		}

		public static int nk_style_push_color(nk_context ctx, Color* address, Color value)
		{
			nk_config_stack_color type_stack;
			nk_config_stack_color_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.colors;
			if ((type_stack.Heightead) >= (int) type_stack.elements.Length) return (int) (0);
			element = type_stack.elements[(type_stack.Heightead++)];
			element.address = address;
			element.old_value = (Color) (*address);
			*address = (Color) (value);
			return (int) (1);
		}

		public static int nk_style_pop_style_item(nk_context ctx)
		{
			nk_config_stack_style_item type_stack;
			nk_config_stack_style_item_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.style_items;
			if ((type_stack.Heightead) < (1)) return (int) (0);
			element = type_stack.elements[(--type_stack.Heightead)];
			element.address = (nk_style_item) (element.old_value);
			return (int) (1);
		}

		public static int nk_style_pop_float(nk_context ctx)
		{
			nk_config_stack_float type_stack;
			nk_config_stack_float_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.floats;
			if ((type_stack.Heightead) < (1)) return (int) (0);
			element = type_stack.elements[(--type_stack.Heightead)];
			*element.address = (float) (element.old_value);
			return (int) (1);
		}

		public static int nk_style_pop_vec2(nk_context ctx)
		{
			nk_config_stack_vec2 type_stack;
			nk_config_stack_vec2_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.vectors;
			if ((type_stack.Heightead) < (1)) return (int) (0);
			element = type_stack.elements[(--type_stack.Heightead)];
			*element.address = (Vector2) (element.old_value);
			return (int) (1);
		}

		public static int nk_style_pop_flags(nk_context ctx)
		{
			nk_config_stack_flags type_stack;
			nk_config_stack_flags_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.flags;
			if ((type_stack.Heightead) < (1)) return (int) (0);
			element = type_stack.elements[(--type_stack.Heightead)];
			*element.address = (uint) (element.old_value);
			return (int) (1);
		}

		public static int nk_style_pop_color(nk_context ctx)
		{
			nk_config_stack_color type_stack;
			nk_config_stack_color_element element;
			if (ctx == null) return (int) (0);
			type_stack = ctx.stacks.colors;
			if ((type_stack.Heightead) < (1)) return (int) (0);
			element = type_stack.elements[(--type_stack.Heightead)];
			*element.address = (Color) (element.old_value);
			return (int) (1);
		}

		public static int nk_style_set_cursor(nk_context ctx, int c)
		{
			nk_style style;
			if (ctx == null) return (int) (0);
			style = ctx.style;
			if ((style.cursors[c]) != null)
			{
				style.cursor_active = style.cursors[c];
				return (int) (1);
			}

			return (int) (0);
		}

		public static void nk_style_show_cursor(nk_context ctx)
		{
			ctx.style.cursor_visible = (int) (nk_true);
		}

		public static void nk_style_hide_cursor(nk_context ctx)
		{
			ctx.style.cursor_visible = (int) (nk_false);
		}

		public static void nk_style_load_cursor(nk_context ctx, int cursor, nk_cursor c)
		{
			nk_style style;
			if (ctx == null) return;
			style = ctx.style;
			style.cursors[cursor] = c;
		}

		public static void nk_style_load_all_cursors(nk_context ctx, nk_cursor[] cursors)
		{
			int i = (int) (0);
			nk_style style;
			if (ctx == null) return;
			style = ctx.style;
			for (i = (int) (0); (i) < (NK_CURSOR_COUNT); ++i)
			{
				style.cursors[i] = cursors[i];
			}
			style.cursor_visible = (int) (nk_true);
		}

		public static void nk_setup(nk_context ctx, nk_user_font font)
		{
			if (ctx == null) return;

			nk_style_default(ctx);
			ctx.seq = (uint) (1);
			if ((font) != null) ctx.style.font = font;
			nk_draw_list_init(ctx.draw_list);
		}

		public static void nk_clear(nk_context ctx)
		{
			nk_window iter;
			nk_window next;
			if (ctx == null) return;
			ctx.build = (int) (0);
			ctx.last_widget_state = (uint) (0);
			ctx.style.cursor_active = ctx.style.cursors[NK_CURSOR_ARROW];

			nk_draw_list_clear(ctx.draw_list);
			iter = ctx.begin;
			while ((iter) != null)
			{
				if ((((iter.flags & NK_WINDOW_MINIMIZED) != 0) && ((iter.flags & NK_WINDOW_CLOSED) == 0)) &&
				    ((iter.seq) == (ctx.seq)))
				{
					iter = iter.next;
					continue;
				}
				if ((((iter.flags & NK_WINDOW_HIDDEN) != 0) || ((iter.flags & NK_WINDOW_CLOSED) != 0)) && ((iter) == (ctx.active)))
				{
					ctx.active = iter.prev;
					ctx.end = iter.prev;
					if ((ctx.active) != null) ctx.active.flags &= (uint) (~(uint) (NK_WINDOW_ROM));
				}
				if (((iter.popup.Widthin) != null) && (iter.popup.Widthin.seq != ctx.seq))
				{
					nk_free_window(ctx, iter.popup.Widthin);
					iter.popup.Widthin = null;
				}
				{
					nk_table n;
					nk_table it = iter.tables;
					while ((it) != null)
					{
						n = it.next;
						if (it.seq != ctx.seq)
						{
							nk_remove_table(iter, it);
							if ((it) == (iter.tables)) iter.tables = n;
						}
						it = n;
					}
				}
				if ((iter.seq != ctx.seq) || ((iter.flags & NK_WINDOW_CLOSED) != 0))
				{
					next = iter.next;
					nk_remove_window(ctx, iter);
					nk_free_window(ctx, iter);
					iter = next;
				}
				else iter = iter.next;
			}
			ctx.seq++;
		}

		public static void nk_start_buffer(nk_context ctx, nk_command_buffer buffer)
		{
			if ((ctx == null) || (buffer == null)) return;

			buffer.first = buffer.last = null;
			buffer.count = 0;
			buffer.clip = (RectangleF) (nk_null_rect);
		}

		public static void nk_start(nk_context ctx, nk_window win)
		{
			nk_start_buffer(ctx, win.buffer);
		}

		public static void nk_start_popup(nk_context ctx, nk_window win)
		{
			if ((ctx == null) || (win == null)) return;

			var buf = win.popup.buf.buffer;

			buf.first = buf.last = null;
			buf.count = 0;

			win.popup.buf.old_buffer = win.buffer;
			win.buffer = buf;
		}

		public static void nk_finish_popup(nk_context ctx, nk_window win)
		{
			if ((ctx == null) || (win == null)) return;

			win.buffer = win.popup.buf.old_buffer;
		}

		public static void nk_finish(nk_context ctx, nk_window win)
		{
			if ((ctx == null) || (win == null) || win.popup.active == 0) return;
		}

		public static int nk_panel_begin(nk_context ctx, char* title, int panel_type)
		{
			nk_input _in_;
			nk_window win;
			nk_panel layout;
			nk_command_buffer _out_;
			nk_style style;
			nk_user_font font;
			Vector2 scrollbar_size = new Vector2();
			Vector2 panel_padding = new Vector2();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);

			if (((ctx.current.flags & NK_WINDOW_HIDDEN) != 0) || ((ctx.current.flags & NK_WINDOW_CLOSED) != 0))
			{
				ctx.current.layout.type = (int) (panel_type);
				return (int) (0);
			}

			style = ctx.style;
			font = style.font;
			win = ctx.current;
			layout = win.layout;
			_out_ = win.buffer;
			_in_ = (win.flags & NK_WINDOW_NO_INPUT) != 0 ? null : ctx.input;
			scrollbar_size = (Vector2) (style.Widthindow.scrollbar_size);
			panel_padding = (Vector2) (nk_panel_get_padding(style, (int) (panel_type)));
			if (((win.flags & NK_WINDOW_MOVABLE) != 0) && ((win.flags & NK_WINDOW_ROM) == 0))
			{
				int left_mouse_down;
				int left_mouse_click_in_cursor;
				RectangleF header = new RectangleF();
				header.X = (float) (win.bounds.X);
				header.Y = (float) (win.bounds.Y);
				header.Width = (float) (win.bounds.Width);
				if ((nk_panel_has_header((uint) (win.flags), title)) != 0)
				{
					header.Height = (float) (font.Height + 2.0f*style.Widthindow.Heighteader.padding.Y);
					header.Height += (float) (2.0f*style.Widthindow.Heighteader.label_padding.Y);
				}
				else header.Height = (float) (panel_padding.Y);
				left_mouse_down = (int) (_in_.mouse.buttons[NK_BUTTON_LEFT].down);
				left_mouse_click_in_cursor =
					(int) (nk_input_has_mouse_click_down_in_rect(_in_, (int) (NK_BUTTON_LEFT), (RectangleF) (header), (int) (nk_true)));
				if (((left_mouse_down) != 0) && ((left_mouse_click_in_cursor) != 0))
				{
					win.bounds.X = (float) (win.bounds.X + _in_.mouse.delta.X);
					win.bounds.Y = (float) (win.bounds.Y + _in_.mouse.delta.Y);
					_in_.mouse.buttons[NK_BUTTON_LEFT].clicked_pos.X += (float) (_in_.mouse.delta.X);
					_in_.mouse.buttons[NK_BUTTON_LEFT].clicked_pos.Y += (float) (_in_.mouse.delta.Y);
					ctx.style.cursor_active = ctx.style.cursors[NK_CURSOR_MOVE];
				}
			}

			layout.type = (int) (panel_type);
			layout.flags = (uint) (win.flags);
			layout.bounds = (RectangleF) (win.bounds);
			layout.bounds.X += (float) (panel_padding.X);
			layout.bounds.Width -= (float) (2*panel_padding.X);
			if ((win.flags & NK_WINDOW_BORDER) != 0)
			{
				layout.border = (float) (nk_panel_get_border(style, (uint) (win.flags), (int) (panel_type)));
				layout.bounds = (RectangleF) (nk_shriRectangleF_((RectangleF) (layout.bounds), (float) (layout.border)));
			}
			else layout.border = (float) (0);
			layout.at_y = (float) (layout.bounds.Y);
			layout.at_x = (float) (layout.bounds.X);
			layout.max_x = (float) (0);
			layout.Heighteader_height = (float) (0);
			layout.footer_height = (float) (0);
			nk_layout_reset_min_row_height(ctx);
			layout.row.index = (int) (0);
			layout.row.columns = (int) (0);
			layout.row.ratio = null;
			layout.row.item_width = (float) (0);
			layout.row.tree_depth = (int) (0);
			layout.row.Height = (float) (panel_padding.Y);
			layout.Heightas_scrolling = (uint) (nk_true);
			if ((win.flags & NK_WINDOW_NO_SCROLLBAR) == 0) layout.bounds.Width -= (float) (scrollbar_size.X);
			if (nk_panel_is_nonblock((int) (panel_type)) == 0)
			{
				layout.footer_height = (float) (0);
				if (((win.flags & NK_WINDOW_NO_SCROLLBAR) == 0) || ((win.flags & NK_WINDOW_SCALABLE) != 0))
					layout.footer_height = (float) (scrollbar_size.Y);
				layout.bounds.Height -= (float) (layout.footer_height);
			}

			if ((nk_panel_has_header((uint) (win.flags), title)) != 0)
			{
				nk_text text = new nk_text();
				RectangleF header = new RectangleF();
				nk_style_item background = null;
				header.X = (float) (win.bounds.X);
				header.Y = (float) (win.bounds.Y);
				header.Width = (float) (win.bounds.Width);
				header.Height = (float) (font.Height + 2.0f*style.Widthindow.Heighteader.padding.Y);
				header.Height += (float) (2.0f*style.Widthindow.Heighteader.label_padding.Y);
				layout.Heighteader_height = (float) (header.Height);
				layout.bounds.Y += (float) (header.Height);
				layout.bounds.Height -= (float) (header.Height);
				layout.at_y += (float) (header.Height);
				if ((ctx.active) == (win))
				{
					background = style.Widthindow.Heighteader.active;
					text.text = (Color) (style.Widthindow.Heighteader.label_active);
				}
				else if ((nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (header))) != 0)
				{
					background = style.Widthindow.Heighteader.hover;
					text.text = (Color) (style.Widthindow.Heighteader.label_hover);
				}
				else
				{
					background = style.Widthindow.Heighteader.normal;
					text.text = (Color) (style.Widthindow.Heighteader.label_normal);
				}
				header.Height += (float) (1.0f);
				if ((background.type) == (NK_STYLE_ITEM_IMAGE))
				{
					text.background = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
					nk_draw_image(win.buffer, (RectangleF) (header), background.data.image, (Color) (nk_white));
				}
				else
				{
					text.background = (Color) (background.data.color);
					nk_fill_rect(_out_, (RectangleF) (header), (float) (0), (Color) (background.data.color));
				}
				{
					RectangleF button = new RectangleF();
					button.Y = (float) (header.Y + style.Widthindow.Heighteader.padding.Y);
					button.Height = (float) (header.Height - 2*style.Widthindow.Heighteader.padding.Y);
					button.Width = (float) (button.Height);
					if ((win.flags & NK_WINDOW_CLOSABLE) != 0)
					{
						uint ws = (uint) (0);
						if ((style.Widthindow.Heighteader.align) == (NK_HEADER_RIGHT))
						{
							button.X = (float) ((header.Width + header.X) - (button.Width + style.Widthindow.Heighteader.padding.X));
							header.Width -= (float) (button.Width + style.Widthindow.Heighteader.spacing.X + style.Widthindow.Heighteader.padding.X);
						}
						else
						{
							button.X = (float) (header.X + style.Widthindow.Heighteader.padding.X);
							header.X += (float) (button.Width + style.Widthindow.Heighteader.spacing.X + style.Widthindow.Heighteader.padding.X);
						}
						if (
							((nk_do_button_symbol(ref ws, win.buffer, (RectangleF) (button), (int) (style.Widthindow.Heighteader.close_symbol),
								(int) (NK_BUTTON_DEFAULT), style.Widthindow.Heighteader.close_button, _in_, style.font)) != 0) &&
							((win.flags & NK_WINDOW_ROM) == 0))
						{
							layout.flags |= (uint) (NK_WINDOW_HIDDEN);
							layout.flags &= ((uint) (~(uint) NK_WINDOW_MINIMIZED));
						}
					}
					if ((win.flags & NK_WINDOW_MINIMIZABLE) != 0)
					{
						uint ws = (uint) (0);
						if ((style.Widthindow.Heighteader.align) == (NK_HEADER_RIGHT))
						{
							button.X = (float) ((header.Width + header.X) - button.Width);
							if ((win.flags & NK_WINDOW_CLOSABLE) == 0)
							{
								button.X -= (float) (style.Widthindow.Heighteader.padding.X);
								header.Width -= (float) (style.Widthindow.Heighteader.padding.X);
							}
							header.Width -= (float) (button.Width + style.Widthindow.Heighteader.spacing.X);
						}
						else
						{
							button.X = (float) (header.X);
							header.X += (float) (button.Width + style.Widthindow.Heighteader.spacing.X + style.Widthindow.Heighteader.padding.X);
						}
						if (
							((nk_do_button_symbol(ref ws, win.buffer, (RectangleF) (button),
								(int)
									((layout.flags & NK_WINDOW_MINIMIZED) != 0
										? style.Widthindow.Heighteader.maximize_symbol
										: style.Widthindow.Heighteader.minimize_symbol), (int) (NK_BUTTON_DEFAULT), style.Widthindow.Heighteader.minimize_button, _in_,
								style.font)) != 0) && ((win.flags & NK_WINDOW_ROM) == 0))
							layout.flags =
								(uint)
									((layout.flags & NK_WINDOW_MINIMIZED) != 0
										? layout.flags & (uint) (~(uint) NK_WINDOW_MINIMIZED)
										: layout.flags | NK_WINDOW_MINIMIZED);
					}
				}
				{
					int text_len = (int) (nk_strlen(title));
					RectangleF label = new RectangleF();
					float t = (float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), title, (int) (text_len)));
					text.padding = (Vector2) (new Vector2((float) (0), (float) (0)));
					label.X = (float) (header.X + style.Widthindow.Heighteader.padding.X);
					label.X += (float) (style.Widthindow.Heighteader.label_padding.X);
					label.Y = (float) (header.Y + style.Widthindow.Heighteader.label_padding.Y);
					label.Height = (float) (font.Height + 2*style.Widthindow.Heighteader.label_padding.Y);
					label.Width = (float) (t + 2*style.Widthindow.Heighteader.spacing.X);
					label.Width =
						(float)
							(((label.Width) < (header.X + header.Width - label.X) ? (label.Width) : (header.X + header.Width - label.X)) < (0)
								? (0)
								: ((label.Width) < (header.X + header.Width - label.X) ? (label.Width) : (header.X + header.Width - label.X)));
					nk_widget_text(_out_, (RectangleF) (label), title, (int) (text_len), &text, (uint) (NK_TEXT_LEFT), font);
				}
			}

			if (((layout.flags & NK_WINDOW_MINIMIZED) == 0) && ((layout.flags & NK_WINDOW_DYNAMIC) == 0))
			{
				RectangleF body = new RectangleF();
				body.X = (float) (win.bounds.X);
				body.Width = (float) (win.bounds.Width);
				body.Y = (float) (win.bounds.Y + layout.Heighteader_height);
				body.Height = (float) (win.bounds.Height - layout.Heighteader_height);
				if ((style.Widthindow.fixed_background.type) == (NK_STYLE_ITEM_IMAGE))
					nk_draw_image(_out_, (RectangleF) (body), style.Widthindow.fixed_background.data.image, (Color) (nk_white));
				else nk_fill_rect(_out_, (RectangleF) (body), (float) (0), (Color) (style.Widthindow.fixed_background.data.color));
			}

			{
				RectangleF clip = new RectangleF();
				layout.clip = (RectangleF) (layout.bounds);
				nk_unify(ref clip, ref win.buffer.clip, (float) (layout.clip.X), (float) (layout.clip.Y),
					(float) (layout.clip.X + layout.clip.Width), (float) (layout.clip.Y + layout.clip.Height));
				nk_push_scissor(_out_, (RectangleF) (clip));
				layout.clip = (RectangleF) (clip);
			}

			return (int) (((layout.flags & NK_WINDOW_HIDDEN) == 0) && ((layout.flags & NK_WINDOW_MINIMIZED) == 0) ? 1 : 0);
		}

		public static void nk_panel_end(nk_context ctx)
		{
			nk_input _in_;
			nk_window window;
			nk_panel layout;
			nk_style style;
			nk_command_buffer _out_;
			Vector2 scrollbar_size = new Vector2();
			Vector2 panel_padding = new Vector2();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			window = ctx.current;
			layout = window.layout;
			style = ctx.style;
			_out_ = window.buffer;
			_in_ = (((layout.flags & NK_WINDOW_ROM) != 0) || ((layout.flags & NK_WINDOW_NO_INPUT) != 0)) ? null : ctx.input;
			if (nk_panel_is_sub((int) (layout.type)) == 0) nk_push_scissor(_out_, (RectangleF) (nk_null_rect));
			scrollbar_size = (Vector2) (style.Widthindow.scrollbar_size);
			panel_padding = (Vector2) (nk_panel_get_padding(style, (int) (layout.type)));
			layout.at_y += (float) (layout.row.Height);
			if (((layout.flags & NK_WINDOW_DYNAMIC) != 0) && ((layout.flags & NK_WINDOW_MINIMIZED) == 0))
			{
				RectangleF empty_space = new RectangleF();
				if ((layout.at_y) < (layout.bounds.Y + layout.bounds.Height)) layout.bounds.Height = (float) (layout.at_y - layout.bounds.Y);
				empty_space.X = (float) (window.bounds.X);
				empty_space.Y = (float) (layout.bounds.Y);
				empty_space.Height = (float) (panel_padding.Y);
				empty_space.Width = (float) (window.bounds.Width);
				nk_fill_rect(_out_, (RectangleF) (empty_space), (float) (0), (Color) (style.Widthindow.background));
				empty_space.X = (float) (window.bounds.X);
				empty_space.Y = (float) (layout.bounds.Y);
				empty_space.Width = (float) (panel_padding.X + layout.border);
				empty_space.Height = (float) (layout.bounds.Height);
				nk_fill_rect(_out_, (RectangleF) (empty_space), (float) (0), (Color) (style.Widthindow.background));
				empty_space.X = (float) (layout.bounds.X + layout.bounds.Width - layout.border);
				empty_space.Y = (float) (layout.bounds.Y);
				empty_space.Width = (float) (panel_padding.X + layout.border);
				empty_space.Height = (float) (layout.bounds.Height);
				if (((layout.offset.Y) == (0)) && ((layout.flags & NK_WINDOW_NO_SCROLLBAR) == 0))
					empty_space.Width += (float) (scrollbar_size.X);
				nk_fill_rect(_out_, (RectangleF) (empty_space), (float) (0), (Color) (style.Widthindow.background));
				if ((layout.offset.X != 0) && ((layout.flags & NK_WINDOW_NO_SCROLLBAR) == 0))
				{
					empty_space.X = (float) (window.bounds.X);
					empty_space.Y = (float) (layout.bounds.Y + layout.bounds.Height);
					empty_space.Width = (float) (window.bounds.Width);
					empty_space.Height = (float) (scrollbar_size.Y);
					nk_fill_rect(_out_, (RectangleF) (empty_space), (float) (0), (Color) (style.Widthindow.background));
				}
			}

			if ((((layout.flags & NK_WINDOW_NO_SCROLLBAR) == 0) && ((layout.flags & NK_WINDOW_MINIMIZED) == 0)) &&
			    ((window.scrollbar_hiding_timer) < (4.0f)))
			{
				RectangleF scroll = new RectangleF();
				int scroll_has_scrolling;
				float scroll_target;
				float scroll_offset;
				float scroll_step;
				float scroll_inc;
				if ((nk_panel_is_sub((int) (layout.type))) != 0)
				{
					nk_window root_window = window;
					nk_panel root_panel = window.layout;
					while ((root_panel.parent) != null)
					{
						root_panel = root_panel.parent;
					}
					while ((root_window.parent) != null)
					{
						root_window = root_window.parent;
					}
					scroll_has_scrolling = (int) (0);
					if (((root_window) == (ctx.active)) && ((layout.Heightas_scrolling) != 0))
					{
						if (((nk_input_is_mouse_hovering_rect(_in_, (RectangleF) (layout.bounds))) != 0) &&
						    (!(((((root_panel.clip.X) > (layout.bounds.X + layout.bounds.Width)) ||
						         ((root_panel.clip.X + root_panel.clip.Width) < (layout.bounds.X))) ||
						        ((root_panel.clip.Y) > (layout.bounds.Y + layout.bounds.Height))) ||
						       ((root_panel.clip.Y + root_panel.clip.Height) < (layout.bounds.Y)))))
						{
							root_panel = window.layout;
							while ((root_panel.parent) != null)
							{
								root_panel.Heightas_scrolling = (uint) (nk_false);
								root_panel = root_panel.parent;
							}
							root_panel.Heightas_scrolling = (uint) (nk_false);
							scroll_has_scrolling = (int) (nk_true);
						}
					}
				}
				else if (nk_panel_is_sub((int) (layout.type)) == 0)
				{
					scroll_has_scrolling = (int) (((window) == (ctx.active)) && ((layout.Heightas_scrolling) != 0) ? 1 : 0);
					if ((((_in_) != null) && (((_in_.mouse.scroll_delta.Y) > (0)) || ((_in_.mouse.scroll_delta.X) > (0)))) &&
					    ((scroll_has_scrolling) != 0)) window.scrolled = (uint) (nk_true);
					else window.scrolled = (uint) (nk_false);
				}
				else scroll_has_scrolling = (int) (nk_false);
				{
					uint state = (uint) (0);
					scroll.X = (float) (layout.bounds.X + layout.bounds.Width + panel_padding.X);
					scroll.Y = (float) (layout.bounds.Y);
					scroll.Width = (float) (scrollbar_size.X);
					scroll.Height = (float) (layout.bounds.Height);
					scroll_offset = ((float) (layout.offset.Y));
					scroll_step = (float) (scroll.Height*0.10f);
					scroll_inc = (float) (scroll.Height*0.01f);
					scroll_target = ((float) ((int) (layout.at_y - scroll.Y)));
					scroll_offset =
						(float)
							(nk_do_scrollbarv(ref state, _out_, (RectangleF) (scroll), (int) (scroll_has_scrolling), (float) (scroll_offset),
								(float) (scroll_target), (float) (scroll_step), (float) (scroll_inc), ctx.style.scrollv, _in_, style.font));
					layout.offset.Y = ((uint) (scroll_offset));
					if (((_in_) != null) && ((scroll_has_scrolling) != 0)) _in_.mouse.scroll_delta.Y = (float) (0);
				}
				{
					uint state = (uint) (0);
					scroll.X = (float) (layout.bounds.X);
					scroll.Y = (float) (layout.bounds.Y + layout.bounds.Height);
					scroll.Width = (float) (layout.bounds.Width);
					scroll.Height = (float) (scrollbar_size.Y);
					scroll_offset = ((float) (layout.offset.X));
					scroll_target = ((float) ((int) (layout.max_x - scroll.X)));
					scroll_step = (float) (layout.max_x*0.05f);
					scroll_inc = (float) (layout.max_x*0.005f);
					scroll_offset =
						(float)
							(nk_do_scrollbarh(ref state, _out_, (RectangleF) (scroll), (int) (scroll_has_scrolling), (float) (scroll_offset),
								(float) (scroll_target), (float) (scroll_step), (float) (scroll_inc), ctx.style.scrollh, _in_, style.font));
					layout.offset.X = ((uint) (scroll_offset));
				}
			}

			if ((window.flags & NK_WINDOW_SCROLL_AUTO_HIDE) != 0)
			{
				int has_input =
					(int)
						(((ctx.input.mouse.delta.X != 0) || (ctx.input.mouse.delta.Y != 0)) || (ctx.input.mouse.scroll_delta.Y != 0)
							? 1
							: 0);
				int is_window_hovered = (int) (nk_window_is_hovered(ctx));
				int any_item_active = (int) (ctx.last_widget_state & NK_WIDGET_STATE_MODIFIED);
				if (((has_input == 0) && ((is_window_hovered) != 0)) || ((is_window_hovered == 0) && (any_item_active == 0)))
					window.scrollbar_hiding_timer += (float) (ctx.delta_time_seconds);
				else window.scrollbar_hiding_timer = (float) (0);
			}
			else window.scrollbar_hiding_timer = (float) (0);
			if ((layout.flags & NK_WINDOW_BORDER) != 0)
			{
				Color border_color = (Color) (nk_panel_get_border_color(style, (int) (layout.type)));
				float padding_y =
					(float)
						((layout.flags & NK_WINDOW_MINIMIZED) != 0
							? style.Widthindow.border + window.bounds.Y + layout.Heighteader_height
							: (layout.flags & NK_WINDOW_DYNAMIC) != 0
								? layout.bounds.Y + layout.bounds.Height + layout.footer_height
								: window.bounds.Y + window.bounds.Height);
				RectangleF b = window.bounds;
				b.Height = padding_y - window.bounds.Y;
				nk_stroke_rect(_out_, b, 0, layout.border, border_color);
			}

			if ((((layout.flags & NK_WINDOW_SCALABLE) != 0) && ((_in_) != null)) && ((layout.flags & NK_WINDOW_MINIMIZED) == 0))
			{
				RectangleF scaler = new RectangleF();
				scaler.Width = (float) (scrollbar_size.X);
				scaler.Height = (float) (scrollbar_size.Y);
				scaler.Y = (float) (layout.bounds.Y + layout.bounds.Height);
				if ((layout.flags & NK_WINDOW_SCALE_LEFT) != 0) scaler.X = (float) (layout.bounds.X - panel_padding.X*0.5f);
				else scaler.X = (float) (layout.bounds.X + layout.bounds.Width + panel_padding.X);
				if ((layout.flags & NK_WINDOW_NO_SCROLLBAR) != 0) scaler.X -= (float) (scaler.Width);
				{
					nk_style_item item = style.Widthindow.scaler;
					if ((item.type) == (NK_STYLE_ITEM_IMAGE))
						nk_draw_image(_out_, (RectangleF) (scaler), item.data.image, (Color) (nk_white));
					else
					{
						if ((layout.flags & NK_WINDOW_SCALE_LEFT) != 0)
						{
							nk_fill_triangle(_out_, (float) (scaler.X), (float) (scaler.Y), (float) (scaler.X), (float) (scaler.Y + scaler.Height),
								(float) (scaler.X + scaler.Width), (float) (scaler.Y + scaler.Height), (Color) (item.data.color));
						}
						else
						{
							nk_fill_triangle(_out_, (float) (scaler.X + scaler.Width), (float) (scaler.Y), (float) (scaler.X + scaler.Width),
								(float) (scaler.Y + scaler.Height), (float) (scaler.X), (float) (scaler.Y + scaler.Height), (Color) (item.data.color));
						}
					}
				}
				if ((window.flags & NK_WINDOW_ROM) == 0)
				{
					Vector2 window_size = (Vector2) (style.Widthindow.min_size);
					int left_mouse_down = (int) (_in_.mouse.buttons[NK_BUTTON_LEFT].down);
					int left_mouse_click_in_scaler =
						(int) (nk_input_has_mouse_click_down_in_rect(_in_, (int) (NK_BUTTON_LEFT), (RectangleF) (scaler), (int) (nk_true)));
					if (((left_mouse_down) != 0) && ((left_mouse_click_in_scaler) != 0))
					{
						float delta_x = (float) (_in_.mouse.delta.X);
						if ((layout.flags & NK_WINDOW_SCALE_LEFT) != 0)
						{
							delta_x = (float) (-delta_x);
							window.bounds.X += (float) (_in_.mouse.delta.X);
						}
						if ((window.bounds.Width + delta_x) >= (window_size.X))
						{
							if (((delta_x) < (0)) || (((delta_x) > (0)) && ((_in_.mouse.pos.X) >= (scaler.X))))
							{
								window.bounds.Width = (float) (window.bounds.Width + delta_x);
								scaler.X += (float) (_in_.mouse.delta.X);
							}
						}
						if ((layout.flags & NK_WINDOW_DYNAMIC) == 0)
						{
							if ((window_size.Y) < (window.bounds.Height + _in_.mouse.delta.Y))
							{
								if (((_in_.mouse.delta.Y) < (0)) || (((_in_.mouse.delta.Y) > (0)) && ((_in_.mouse.pos.Y) >= (scaler.Y))))
								{
									window.bounds.Height = (float) (window.bounds.Height + _in_.mouse.delta.Y);
									scaler.Y += (float) (_in_.mouse.delta.Y);
								}
							}
						}
						ctx.style.cursor_active = ctx.style.cursors[NK_CURSOR_RESIZE_TOP_RIGHT_DOWN_LEFT];
						_in_.mouse.buttons[NK_BUTTON_LEFT].clicked_pos.X = (float) (scaler.X + scaler.Width/2.0f);
						_in_.mouse.buttons[NK_BUTTON_LEFT].clicked_pos.Y = (float) (scaler.Y + scaler.Height/2.0f);
					}
				}
			}

			if (nk_panel_is_sub((int) (layout.type)) == 0)
			{
				if ((layout.flags & NK_WINDOW_HIDDEN) != 0) nk_command_buffer_reset(window.buffer);
				else nk_finish(ctx, window);
			}

			if ((layout.flags & NK_WINDOW_REMOVE_ROM) != 0)
			{
				layout.flags &= (uint) (~(uint) (NK_WINDOW_ROM));
				layout.flags &= (uint) (~(uint) (NK_WINDOW_REMOVE_ROM));
			}

			window.flags = (uint) (layout.flags);
			if ((((window.property.active) != 0) && (window.property.old != window.property.seq)) &&
			    ((window.property.active) == (window.property.prev)))
			{
			}
			else
			{
				window.property.old = (uint) (window.property.seq);
				window.property.prev = (int) (window.property.active);
				window.property.seq = (uint) (0);
			}

			if ((((window.edit.active) != 0) && (window.edit.old != window.edit.seq)) &&
			    ((window.edit.active) == (window.edit.prev)))
			{
			}
			else
			{
				window.edit.old = (uint) (window.edit.seq);
				window.edit.prev = (int) (window.edit.active);
				window.edit.seq = (uint) (0);
			}

			if (((window.popup.active_con) != 0) && (window.popup.con_old != window.popup.con_count))
			{
				window.popup.con_count = (uint) (0);
				window.popup.con_old = (uint) (0);
				window.popup.active_con = (uint) (0);
			}
			else
			{
				window.popup.con_old = (uint) (window.popup.con_count);
				window.popup.con_count = (uint) (0);
			}

			window.popup.combo_count = (uint) (0);
		}

		public static uint* nk_add_value(nk_context ctx, nk_window win, uint name, uint value)
		{
			if ((win == null) || (ctx == null)) return null;
			if ((win.tables == null) || ((win.tables.size) >= (51)))
			{
				nk_table tbl = nk_create_table(ctx);
				if (tbl == null) return null;
				nk_push_table(win, tbl);
			}

			win.tables.seq = (uint) (win.seq);
			win.tables.keys[win.tables.size] = (uint) (name);
			win.tables.values[win.tables.size] = (uint) (value);
			return (uint*) win.tables.values + (win.tables.size++);
		}

		public static nk_window nk_find_window(nk_context ctx, uint hash, char* name)
		{
			nk_window iter;
			iter = ctx.begin;
			while ((iter) != null)
			{
				if ((iter.name) == (hash))
				{
					int max_len = (int) (nk_strlen(iter.name_string));
					if (nk_stricmpn(iter.name_string, name, (int) (max_len)) == 0) return iter;
				}
				iter = iter.next;
			}
			return null;
		}

		public static void nk_insert_window(nk_context ctx, nk_window win, int loc)
		{
			nk_window iter;
			if ((win == null) || (ctx == null)) return;
			iter = ctx.begin;
			while ((iter) != null)
			{
				if ((iter) == (win)) return;
				iter = iter.next;
			}
			if (ctx.begin == null)
			{
				win.next = null;
				win.prev = null;
				ctx.begin = win;
				ctx.end = win;
				ctx.count = (uint) (1);
				return;
			}

			if ((loc) == (NK_INSERT_BACK))
			{
				nk_window end;
				end = ctx.end;
				end.flags |= (uint) (NK_WINDOW_ROM);
				end.next = win;
				win.prev = ctx.end;
				win.next = null;
				ctx.end = win;
				ctx.active = ctx.end;
				ctx.end.flags &= (uint) (~(uint) (NK_WINDOW_ROM));
			}
			else
			{
				ctx.begin.prev = win;
				win.next = ctx.begin;
				win.prev = null;
				ctx.begin = win;
				ctx.begin.flags &= (uint) (~(uint) (NK_WINDOW_ROM));
			}

			ctx.count++;
		}

		public static void nk_remove_window(nk_context ctx, nk_window win)
		{
			if (((win) == (ctx.begin)) || ((win) == (ctx.end)))
			{
				if ((win) == (ctx.begin))
				{
					ctx.begin = win.next;
					if ((win.next) != null) win.next.prev = null;
				}
				if ((win) == (ctx.end))
				{
					ctx.end = win.prev;
					if ((win.prev) != null) win.prev.next = null;
				}
			}
			else
			{
				if ((win.next) != null) win.next.prev = win.prev;
				if ((win.prev) != null) win.prev.next = win.next;
			}

			if (((win) == (ctx.active)) || (ctx.active == null))
			{
				ctx.active = ctx.end;
				if ((ctx.end) != null) ctx.end.flags &= (uint) (~(uint) (NK_WINDOW_ROM));
			}

			win.next = null;
			win.prev = null;
			ctx.count--;
		}

		public static int nk_begin(nk_context ctx, char* title, RectangleF bounds, uint flags)
		{
			return (int) (nk_begin_titled(ctx, title, title, (RectangleF) (bounds), (uint) (flags)));
		}

		public static int nk_begin_titled(nk_context ctx, char* name, char* title, RectangleF bounds, uint flags)
		{
			nk_window win;
			nk_style style;
			uint title_hash;
			int title_len;
			int ret = (int) (0);
			if ((((ctx == null) || ((ctx.current) != null)) || (title == null)) || (name == null)) return (int) (0);
			style = ctx.style;
			title_len = (int) (nk_strlen(name));
			title_hash = (uint) (nk_murmur_hash(name, (int) (title_len), (uint) (NK_WINDOW_TITLE)));
			win = nk_find_window(ctx, (uint) (title_hash), name);
			if (win == null)
			{
				ulong name_length = (ulong) (nk_strlen(name));
				win = (nk_window) (nk_create_window(ctx));
				if (win == null) return (int) (0);
				if ((flags & NK_WINDOW_BACKGROUND) != 0) nk_insert_window(ctx, win, (int) (NK_INSERT_FRONT));
				else nk_insert_window(ctx, win, (int) (NK_INSERT_BACK));
				nk_command_buffer_init(win.buffer, (int) (NK_CLIPPING_ON));
				win.flags = (uint) (flags);
				win.bounds = (RectangleF) (bounds);
				win.name = (uint) (title_hash);
				name_length = (ulong) ((name_length) < (64 - 1) ? (name_length) : (64 - 1));
				win.name_string = name;
				win.popup.Widthin = null;
				if (ctx.active == null) ctx.active = win;
			}
			else
			{
				win.flags &= (uint) (~(uint) (NK_WINDOW_PRIVATE - 1));
				win.flags |= (uint) (flags);
				if ((win.flags & (NK_WINDOW_MOVABLE | NK_WINDOW_SCALABLE)) == 0) win.bounds = (RectangleF) (bounds);
				win.seq = (uint) (ctx.seq);
				if ((ctx.active == null) && ((win.flags & NK_WINDOW_HIDDEN) == 0))
				{
					ctx.active = win;
					ctx.end = win;
				}
			}

			if ((win.flags & NK_WINDOW_HIDDEN) != 0)
			{
				ctx.current = win;
				win.layout = null;
				return (int) (0);
			}
			else nk_start(ctx, win);
			if (((win.flags & NK_WINDOW_HIDDEN) == 0) && ((win.flags & NK_WINDOW_NO_INPUT) == 0))
			{
				int inpanel;
				int ishovered;
				nk_window iter = win;
				float h =
					(float) (ctx.style.font.Height + 2.0f*style.Widthindow.Heighteader.padding.Y + (2.0f*style.Widthindow.Heighteader.label_padding.Y));
				RectangleF win_bounds =
					(RectangleF)
						(((win.flags & NK_WINDOW_MINIMIZED) == 0)
							? win.bounds
							: RectangleF_((float) (win.bounds.X), (float) (win.bounds.Y), (float) (win.bounds.Width), (float) (h)));
				inpanel =
					(int)
						(nk_input_has_mouse_click_down_in_rect(ctx.input, (int) (NK_BUTTON_LEFT), (RectangleF) (win_bounds), (int) (nk_true)));
				inpanel = (int) (((inpanel) != 0) && ((ctx.input.mouse.buttons[NK_BUTTON_LEFT].clicked) != 0) ? 1 : 0);
				ishovered = (int) (nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (win_bounds)));
				if (((win != ctx.active) && ((ishovered) != 0)) && (ctx.input.mouse.buttons[NK_BUTTON_LEFT].down == 0))
				{
					iter = win.next;
					while ((iter) != null)
					{
						RectangleF iter_bounds =
							(RectangleF)
								(((iter.flags & NK_WINDOW_MINIMIZED) == 0)
									? iter.bounds
									: RectangleF_((float) (iter.bounds.X), (float) (iter.bounds.Y), (float) (iter.bounds.Width), (float) (h)));
						if (
							(!(((((iter_bounds.X) > (win_bounds.X + win_bounds.Width)) || ((iter_bounds.X + iter_bounds.Width) < (win_bounds.X))) ||
							    ((iter_bounds.Y) > (win_bounds.Y + win_bounds.Height))) || ((iter_bounds.Y + iter_bounds.Height) < (win_bounds.Y)))) &&
							((iter.flags & NK_WINDOW_HIDDEN) == 0)) break;
						if (((((iter.popup.Widthin) != null) && ((iter.popup.active) != 0)) && ((iter.flags & NK_WINDOW_HIDDEN) == 0)) &&
						    (!(((((iter.popup.Widthin.bounds.X) > (win.bounds.X + win_bounds.Width)) ||
						         ((iter.popup.Widthin.bounds.X + iter.popup.Widthin.bounds.Width) < (win.bounds.X))) ||
						        ((iter.popup.Widthin.bounds.Y) > (win_bounds.Y + win_bounds.Height))) ||
						       ((iter.popup.Widthin.bounds.Y + iter.popup.Widthin.bounds.Height) < (win_bounds.Y))))) break;
						iter = iter.next;
					}
				}
				if ((((iter) != null) && ((inpanel) != 0)) && (win != ctx.end))
				{
					iter = win.next;
					while ((iter) != null)
					{
						RectangleF iter_bounds =
							(RectangleF)
								(((iter.flags & NK_WINDOW_MINIMIZED) == 0)
									? iter.bounds
									: RectangleF_((float) (iter.bounds.X), (float) (iter.bounds.Y), (float) (iter.bounds.Width), (float) (h)));
						if (((((iter_bounds.X) <= (ctx.input.mouse.pos.X)) && ((ctx.input.mouse.pos.X) < (iter_bounds.X + iter_bounds.Width))) &&
						     (((iter_bounds.Y) <= (ctx.input.mouse.pos.Y)) && ((ctx.input.mouse.pos.Y) < (iter_bounds.Y + iter_bounds.Height)))) &&
						    ((iter.flags & NK_WINDOW_HIDDEN) == 0)) break;
						if (((((iter.popup.Widthin) != null) && ((iter.popup.active) != 0)) && ((iter.flags & NK_WINDOW_HIDDEN) == 0)) &&
						    (!(((((iter.popup.Widthin.bounds.X) > (win_bounds.X + win_bounds.Width)) ||
						         ((iter.popup.Widthin.bounds.X + iter.popup.Widthin.bounds.Width) < (win_bounds.X))) ||
						        ((iter.popup.Widthin.bounds.Y) > (win_bounds.Y + win_bounds.Height))) ||
						       ((iter.popup.Widthin.bounds.Y + iter.popup.Widthin.bounds.Height) < (win_bounds.Y))))) break;
						iter = iter.next;
					}
				}
				if ((((iter) != null) && ((win.flags & NK_WINDOW_ROM) == 0)) && ((win.flags & NK_WINDOW_BACKGROUND) != 0))
				{
					win.flags |= ((uint) (NK_WINDOW_ROM));
					iter.flags &= (uint) (~(uint) (NK_WINDOW_ROM));
					ctx.active = iter;
					if ((iter.flags & NK_WINDOW_BACKGROUND) == 0)
					{
						nk_remove_window(ctx, iter);
						nk_insert_window(ctx, iter, (int) (NK_INSERT_BACK));
					}
				}
				else
				{
					if ((iter == null) && (ctx.end != win))
					{
						if ((win.flags & NK_WINDOW_BACKGROUND) == 0)
						{
							nk_remove_window(ctx, win);
							nk_insert_window(ctx, win, (int) (NK_INSERT_BACK));
						}
						win.flags &= (uint) (~(uint) (NK_WINDOW_ROM));
						ctx.active = win;
					}
					if ((ctx.end != win) && ((win.flags & NK_WINDOW_BACKGROUND) == 0)) win.flags |= (uint) (NK_WINDOW_ROM);
				}
			}

			win.layout = (nk_panel) (nk_create_panel(ctx));
			ctx.current = win;
			ret = (int) (nk_panel_begin(ctx, title, (int) (NK_PANEL_WINDOW)));
			win.layout.offset = win.scrollbar;

			return (int) (ret);
		}

		public static void nk_end(nk_context ctx)
		{
			nk_panel layout;
			if ((ctx == null) || (ctx.current == null)) return;
			layout = ctx.current.layout;
			if ((layout == null) || (((layout.type) == (NK_PANEL_WINDOW)) && ((ctx.current.flags & NK_WINDOW_HIDDEN) != 0)))
			{
				ctx.current = null;
				return;
			}

			nk_panel_end(ctx);

			ctx.current = null;
		}

		public static RectangleF nk_window_get_bounds(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null))
				return (RectangleF) (RectangleF_((float) (0), (float) (0), (float) (0), (float) (0)));
			return (RectangleF) (ctx.current.bounds);
		}

		public static Vector2 nk_window_get_position(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return (Vector2) (new Vector2((float) (0), (float) (0)));
			return (Vector2) (new Vector2((float) (ctx.current.bounds.X), (float) (ctx.current.bounds.Y)));
		}

		public static Vector2 nk_window_get_size(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return (Vector2) (new Vector2((float) (0), (float) (0)));
			return (Vector2) (new Vector2((float) (ctx.current.bounds.Width), (float) (ctx.current.bounds.Height)));
		}

		public static float nk_window_get_width(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return (float) (0);
			return (float) (ctx.current.bounds.Width);
		}

		public static float nk_window_get_height(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return (float) (0);
			return (float) (ctx.current.bounds.Height);
		}

		public static RectangleF nk_window_get_content_region(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null))
				return (RectangleF) (RectangleF_((float) (0), (float) (0), (float) (0), (float) (0)));
			return (RectangleF) (ctx.current.layout.clip);
		}

		public static Vector2 nk_window_get_content_region_min(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return (Vector2) (new Vector2((float) (0), (float) (0)));
			return (Vector2) (new Vector2((float) (ctx.current.layout.clip.X), (float) (ctx.current.layout.clip.Y)));
		}

		public static Vector2 nk_window_get_content_region_max(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return (Vector2) (new Vector2((float) (0), (float) (0)));
			return
				(Vector2)
					(new Vector2((float) (ctx.current.layout.clip.X + ctx.current.layout.clip.Width),
						(float) (ctx.current.layout.clip.Y + ctx.current.layout.clip.Height)));
		}

		public static Vector2 nk_window_get_content_region_size(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return (Vector2) (new Vector2((float) (0), (float) (0)));
			return (Vector2) (new Vector2((float) (ctx.current.layout.clip.Width), (float) (ctx.current.layout.clip.Height)));
		}

		public static nk_command_buffer nk_window_get_canvas(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return null;
			return ctx.current.buffer;
		}

		public static nk_panel nk_window_get_panel(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return null;
			return ctx.current.layout;
		}

		public static int nk_window_has_focus(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return (int) (0);
			return (int) ((ctx.current) == (ctx.active) ? 1 : 0);
		}

		public static int nk_window_is_hovered(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return (int) (0);
			if ((ctx.current.flags & NK_WINDOW_HIDDEN) != 0) return (int) (0);
			return (int) (nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (ctx.current.bounds)));
		}

		public static int nk_window_is_any_hovered(nk_context ctx)
		{
			nk_window iter;
			if (ctx == null) return (int) (0);
			iter = ctx.begin;
			while ((iter) != null)
			{
				if ((iter.flags & NK_WINDOW_HIDDEN) == 0)
				{
					if ((((iter.popup.active) != 0) && ((iter.popup.Widthin) != null)) &&
					    ((nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (iter.popup.Widthin.bounds))) != 0)) return (int) (1);
					if ((iter.flags & NK_WINDOW_MINIMIZED) != 0)
					{
						RectangleF header = (RectangleF) (iter.bounds);
						header.Height = (float) (ctx.style.font.Height + 2*ctx.style.Widthindow.Heighteader.padding.Y);
						if ((nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (header))) != 0) return (int) (1);
					}
					else if ((nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (iter.bounds))) != 0)
					{
						return (int) (1);
					}
				}
				iter = iter.next;
			}
			return (int) (0);
		}

		public static int nk_item_is_any_active(nk_context ctx)
		{
			int any_hovered = (int) (nk_window_is_any_hovered(ctx));
			int any_active = (int) (ctx.last_widget_state & NK_WIDGET_STATE_MODIFIED);
			return (int) (((any_hovered) != 0) || ((any_active) != 0) ? 1 : 0);
		}

		public static int nk_window_is_collapsed(nk_context ctx, char* name)
		{
			int title_len;
			uint title_hash;
			nk_window win;
			if (ctx == null) return (int) (0);
			title_len = (int) (nk_strlen(name));
			title_hash = (uint) (nk_murmur_hash(name, (int) (title_len), (uint) (NK_WINDOW_TITLE)));
			win = nk_find_window(ctx, (uint) (title_hash), name);
			if (win == null) return (int) (0);
			return (int) (win.flags & NK_WINDOW_MINIMIZED);
		}

		public static int nk_window_is_closed(nk_context ctx, char* name)
		{
			int title_len;
			uint title_hash;
			nk_window win;
			if (ctx == null) return (int) (1);
			title_len = (int) (nk_strlen(name));
			title_hash = (uint) (nk_murmur_hash(name, (int) (title_len), (uint) (NK_WINDOW_TITLE)));
			win = nk_find_window(ctx, (uint) (title_hash), name);
			if (win == null) return (int) (1);
			return (int) (win.flags & NK_WINDOW_CLOSED);
		}

		public static int nk_window_is_hidden(nk_context ctx, char* name)
		{
			int title_len;
			uint title_hash;
			nk_window win;
			if (ctx == null) return (int) (1);
			title_len = (int) (nk_strlen(name));
			title_hash = (uint) (nk_murmur_hash(name, (int) (title_len), (uint) (NK_WINDOW_TITLE)));
			win = nk_find_window(ctx, (uint) (title_hash), name);
			if (win == null) return (int) (1);
			return (int) (win.flags & NK_WINDOW_HIDDEN);
		}

		public static int nk_window_is_active(nk_context ctx, char* name)
		{
			int title_len;
			uint title_hash;
			nk_window win;
			if (ctx == null) return (int) (0);
			title_len = (int) (nk_strlen(name));
			title_hash = (uint) (nk_murmur_hash(name, (int) (title_len), (uint) (NK_WINDOW_TITLE)));
			win = nk_find_window(ctx, (uint) (title_hash), name);
			if (win == null) return (int) (0);
			return (int) ((win) == (ctx.active) ? 1 : 0);
		}

		public static nk_window nk_window_find(nk_context ctx, char* name)
		{
			int title_len;
			uint title_hash;
			title_len = (int) (nk_strlen(name));
			title_hash = (uint) (nk_murmur_hash(name, (int) (title_len), (uint) (NK_WINDOW_TITLE)));
			return nk_find_window(ctx, (uint) (title_hash), name);
		}

		public static void nk_window_close(nk_context ctx, char* name)
		{
			nk_window win;
			if (ctx == null) return;
			win = nk_window_find(ctx, name);
			if (win == null) return;
			if ((ctx.current) == (win)) return;
			win.flags |= (uint) (NK_WINDOW_HIDDEN);
			win.flags |= (uint) (NK_WINDOW_CLOSED);
		}

		public static void nk_window_set_bounds(nk_context ctx, char* name, RectangleF bounds)
		{
			nk_window win;
			if (ctx == null) return;
			win = nk_window_find(ctx, name);
			if (win == null) return;
			win.bounds = (RectangleF) (bounds);
		}

		public static void nk_window_set_position(nk_context ctx, char* name, Vector2 pos)
		{
			nk_window win = nk_window_find(ctx, name);
			if (win == null) return;
			win.bounds.X = (float) (pos.X);
			win.bounds.Y = (float) (pos.Y);
		}

		public static void nk_window_set_size(nk_context ctx, char* name, Vector2 size)
		{
			nk_window win = nk_window_find(ctx, name);
			if (win == null) return;
			win.bounds.Width = (float) (size.X);
			win.bounds.Height = (float) (size.Y);
		}

		public static void nk_window_collapse(nk_context ctx, char* name, int c)
		{
			int title_len;
			uint title_hash;
			nk_window win;
			if (ctx == null) return;
			title_len = (int) (nk_strlen(name));
			title_hash = (uint) (nk_murmur_hash(name, (int) (title_len), (uint) (NK_WINDOW_TITLE)));
			win = nk_find_window(ctx, (uint) (title_hash), name);
			if (win == null) return;
			if ((c) == (NK_MINIMIZED)) win.flags |= (uint) (NK_WINDOW_MINIMIZED);
			else win.flags &= (uint) (~(uint) (NK_WINDOW_MINIMIZED));
		}

		public static void nk_window_collapse_if(nk_context ctx, char* name, int c, int cond)
		{
			if ((ctx == null) || (cond == 0)) return;
			nk_window_collapse(ctx, name, (int) (c));
		}

		public static void nk_window_show(nk_context ctx, char* name, int s)
		{
			int title_len;
			uint title_hash;
			nk_window win;
			if (ctx == null) return;
			title_len = (int) (nk_strlen(name));
			title_hash = (uint) (nk_murmur_hash(name, (int) (title_len), (uint) (NK_WINDOW_TITLE)));
			win = nk_find_window(ctx, (uint) (title_hash), name);
			if (win == null) return;
			if ((s) == (NK_HIDDEN))
			{
				win.flags |= (uint) (NK_WINDOW_HIDDEN);
			}
			else win.flags &= (uint) (~(uint) (NK_WINDOW_HIDDEN));
		}

		public static void nk_window_show_if(nk_context ctx, char* name, int s, int cond)
		{
			if ((ctx == null) || (cond == 0)) return;
			nk_window_show(ctx, name, (int) (s));
		}

		public static void nk_window_set_focus(nk_context ctx, char* name)
		{
			int title_len;
			uint title_hash;
			nk_window win;
			if (ctx == null) return;
			title_len = (int) (nk_strlen(name));
			title_hash = (uint) (nk_murmur_hash(name, (int) (title_len), (uint) (NK_WINDOW_TITLE)));
			win = nk_find_window(ctx, (uint) (title_hash), name);
			if (((win) != null) && (ctx.end != win))
			{
				nk_remove_window(ctx, win);
				nk_insert_window(ctx, win, (int) (NK_INSERT_BACK));
			}

			ctx.active = win;
		}

		public static void nk_menubar_begin(nk_context ctx)
		{
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			layout = ctx.current.layout;
			if (((layout.flags & NK_WINDOW_HIDDEN) != 0) || ((layout.flags & NK_WINDOW_MINIMIZED) != 0)) return;
			layout.menu.X = (float) (layout.at_x);
			layout.menu.Y = (float) (layout.at_y + layout.row.Height);
			layout.menu.Width = (float) (layout.bounds.Width);
			layout.menu.offset = layout.offset;

			layout.offset.Y = (uint) (0);
		}

		public static void nk_menubar_end(nk_context ctx)
		{
			nk_window win;
			nk_panel layout;
			nk_command_buffer _out_;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			_out_ = win.buffer;
			layout = win.layout;
			if (((layout.flags & NK_WINDOW_HIDDEN) != 0) || ((layout.flags & NK_WINDOW_MINIMIZED) != 0)) return;
			layout.menu.Height = (float) (layout.at_y - layout.menu.Y);
			layout.bounds.Y += (float) (layout.menu.Height + ctx.style.Widthindow.spacing.Y + layout.row.Height);
			layout.bounds.Height -= (float) (layout.menu.Height + ctx.style.Widthindow.spacing.Y + layout.row.Height);
			layout.offset.X = (uint) (layout.menu.offset.X);
			layout.offset.Y = (uint) (layout.menu.offset.Y);
			layout.at_y = (float) (layout.bounds.Y - layout.row.Height);
			layout.clip.Y = (float) (layout.bounds.Y);
			layout.clip.Height = (float) (layout.bounds.Height);
			nk_push_scissor(_out_, (RectangleF) (layout.clip));
		}

		public static void nk_layout_set_min_row_height(nk_context ctx, float height)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			layout.row.min_height = (float) (height);
		}

		public static void nk_layout_reset_min_row_height(nk_context ctx)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			layout.row.min_height = (float) (ctx.style.font.Height);
			layout.row.min_height += (float) (ctx.style.text.padding.Y*2);
			layout.row.min_height += (float) (ctx.style.Widthindow.min_row_height_padding*2);
		}

		public static void nk_panel_layout(nk_context ctx, nk_window win, float height, int cols)
		{
			nk_panel layout;
			nk_style style;
			nk_command_buffer _out_;
			Vector2 item_spacing = new Vector2();
			Color color = new Color();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			layout = win.layout;
			style = ctx.style;
			_out_ = win.buffer;
			color = (Color) (style.Widthindow.background);
			item_spacing = (Vector2) (style.Widthindow.spacing);
			layout.row.index = (int) (0);
			layout.at_y += (float) (layout.row.Height);
			layout.row.columns = (int) (cols);
			if ((height) == (0.0f))
				layout.row.Height =
					(float) (((height) < (layout.row.min_height) ? (layout.row.min_height) : (height)) + item_spacing.Y);
			else layout.row.Height = (float) (height + item_spacing.Y);
			layout.row.item_offset = (float) (0);
			if ((layout.flags & NK_WINDOW_DYNAMIC) != 0)
			{
				RectangleF background = new RectangleF();
				background.X = (float) (win.bounds.X);
				background.Width = (float) (win.bounds.Width);
				background.Y = (float) (layout.at_y - 1.0f);
				background.Height = (float) (layout.row.Height + 1.0f);
				nk_fill_rect(_out_, (RectangleF) (background), (float) (0), (Color) (color));
			}

		}

		public static void nk_row_layout_(nk_context ctx, int fmt, float height, int cols, int width)
		{
			nk_window win;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			nk_panel_layout(ctx, win, (float) (height), (int) (cols));
			if ((fmt) == (NK_DYNAMIC)) win.layout.row.type = (int) (NK_LAYOUT_DYNAMIC_FIXED);
			else win.layout.row.type = (int) (NK_LAYOUT_STATIC_FIXED);
			win.layout.row.ratio = null;
			win.layout.row.filled = (float) (0);
			win.layout.row.item_offset = (float) (0);
			win.layout.row.item_width = ((float) (width));
		}

		public static float nk_layout_ratio_from_pixel(nk_context ctx, float pixel_width)
		{
			nk_window win;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (float) (0);
			win = ctx.current;
			return
				(float)
					(((pixel_width/win.bounds.X) < (1.0f) ? (pixel_width/win.bounds.X) : (1.0f)) < (0.0f)
						? (0.0f)
						: ((pixel_width/win.bounds.X) < (1.0f) ? (pixel_width/win.bounds.X) : (1.0f)));
		}

		public static void nk_layout_row_dynamic(nk_context ctx, float height, int cols)
		{
			nk_row_layout_(ctx, (int) (NK_DYNAMIC), (float) (height), (int) (cols), (int) (0));
		}

		public static void nk_layout_row_static(nk_context ctx, float height, int item_width, int cols)
		{
			nk_row_layout_(ctx, (int) (NK_STATIC), (float) (height), (int) (cols), (int) (item_width));
		}

		public static void nk_layout_row_begin(nk_context ctx, int fmt, float row_height, int cols)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			nk_panel_layout(ctx, win, (float) (row_height), (int) (cols));
			if ((fmt) == (NK_DYNAMIC)) layout.row.type = (int) (NK_LAYOUT_DYNAMIC_ROW);
			else layout.row.type = (int) (NK_LAYOUT_STATIC_ROW);
			layout.row.ratio = null;
			layout.row.filled = (float) (0);
			layout.row.item_width = (float) (0);
			layout.row.item_offset = (float) (0);
			layout.row.columns = (int) (cols);
		}

		public static void nk_layout_row_push(nk_context ctx, float ratio_or_width)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			if ((layout.row.type != NK_LAYOUT_STATIC_ROW) && (layout.row.type != NK_LAYOUT_DYNAMIC_ROW)) return;
			if ((layout.row.type) == (NK_LAYOUT_DYNAMIC_ROW))
			{
				float ratio = (float) (ratio_or_width);
				if ((ratio + layout.row.filled) > (1.0f)) return;
				if ((ratio) > (0.0f))
					layout.row.item_width =
						(float) ((0) < ((1.0f) < (ratio) ? (1.0f) : (ratio)) ? ((1.0f) < (ratio) ? (1.0f) : (ratio)) : (0));
				else layout.row.item_width = (float) (1.0f - layout.row.filled);
			}
			else layout.row.item_width = (float) (ratio_or_width);
		}

		public static void nk_layout_row_end(nk_context ctx)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			if ((layout.row.type != NK_LAYOUT_STATIC_ROW) && (layout.row.type != NK_LAYOUT_DYNAMIC_ROW)) return;
			layout.row.item_width = (float) (0);
			layout.row.item_offset = (float) (0);
		}

		public static void nk_layout_row(nk_context ctx, int fmt, float height, int cols, float* ratio)
		{
			int i;
			int n_undef = (int) (0);
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			nk_panel_layout(ctx, win, (float) (height), (int) (cols));
			if ((fmt) == (NK_DYNAMIC))
			{
				float r = (float) (0);
				layout.row.ratio = ratio;
				for (i = (int) (0); (i) < (cols); ++i)
				{
					if ((ratio[i]) < (0.0f)) n_undef++;
					else r += (float) (ratio[i]);
				}
				r = (float) ((0) < ((1.0f) < (1.0f - r) ? (1.0f) : (1.0f - r)) ? ((1.0f) < (1.0f - r) ? (1.0f) : (1.0f - r)) : (0));
				layout.row.type = (int) (NK_LAYOUT_DYNAMIC);
				layout.row.item_width = (float) ((((r) > (0)) && ((n_undef) > (0))) ? (r/(float) (n_undef)) : 0);
			}
			else
			{
				layout.row.ratio = ratio;
				layout.row.type = (int) (NK_LAYOUT_STATIC);
				layout.row.item_width = (float) (0);
				layout.row.item_offset = (float) (0);
			}

			layout.row.item_offset = (float) (0);
			layout.row.filled = (float) (0);
		}

		public static void nk_layout_row_template_begin(nk_context ctx, float height)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			nk_panel_layout(ctx, win, (float) (height), (int) (1));
			layout.row.type = (int) (NK_LAYOUT_TEMPLATE);
			layout.row.columns = (int) (0);
			layout.row.ratio = null;
			layout.row.item_width = (float) (0);
			layout.row.item_height = (float) (0);
			layout.row.item_offset = (float) (0);
			layout.row.filled = (float) (0);
			layout.row.item.X = (float) (0);
			layout.row.item.Y = (float) (0);
			layout.row.item.Width = (float) (0);
			layout.row.item.Height = (float) (0);
		}

		public static void nk_layout_row_template_push_dynamic(nk_context ctx)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			if (layout.row.type != NK_LAYOUT_TEMPLATE) return;
			if ((layout.row.columns) >= (16)) return;
			layout.row.templates[layout.row.columns++] = (float) (-1.0f);
		}

		public static void nk_layout_row_template_push_variable(nk_context ctx, float min_width)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			if (layout.row.type != NK_LAYOUT_TEMPLATE) return;
			if ((layout.row.columns) >= (16)) return;
			layout.row.templates[layout.row.columns++] = (float) (-min_width);
		}

		public static void nk_layout_row_template_push_static(nk_context ctx, float width)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			if (layout.row.type != NK_LAYOUT_TEMPLATE) return;
			if ((layout.row.columns) >= (16)) return;
			layout.row.templates[layout.row.columns++] = (float) (width);
		}

		public static void nk_layout_row_template_end(nk_context ctx)
		{
			nk_window win;
			nk_panel layout;
			int i = (int) (0);
			int variable_count = (int) (0);
			int min_variable_count = (int) (0);
			float min_fixed_width = (float) (0.0f);
			float total_fixed_width = (float) (0.0f);
			float max_variable_width = (float) (0.0f);
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			if (layout.row.type != NK_LAYOUT_TEMPLATE) return;
			for (i = (int) (0); (i) < (layout.row.columns); ++i)
			{
				float width = (float) (layout.row.templates[i]);
				if ((width) >= (0.0f))
				{
					total_fixed_width += (float) (width);
					min_fixed_width += (float) (width);
				}
				else if ((width) < (-1.0f))
				{
					width = (float) (-width);
					total_fixed_width += (float) (width);
					max_variable_width = (float) ((max_variable_width) < (width) ? (width) : (max_variable_width));
					variable_count++;
				}
				else
				{
					min_variable_count++;
					variable_count++;
				}
			}
			if ((variable_count) != 0)
			{
				float space =
					(float)
						(nk_layout_row_calculate_usable_space(ctx.style, (int) (layout.type), (float) (layout.bounds.Width),
							(int) (layout.row.columns)));
				float var_width =
					(float) (((space - min_fixed_width) < (0.0f) ? (0.0f) : (space - min_fixed_width))/(float) (variable_count));
				int enough_space = (int) ((var_width) >= (max_variable_width) ? 1 : 0);
				if (enough_space == 0)
					var_width =
						(float) (((space - total_fixed_width) < (0) ? (0) : (space - total_fixed_width))/(float) (min_variable_count));
				for (i = (int) (0); (i) < (layout.row.columns); ++i)
				{
					var width = layout.row.templates[i];
					layout.row.templates[i] = (float) (((width) >= (0.0f)) ? width : (((width) < (-1.0f)) && (enough_space == 0)) ? -(width) : var_width);
				}
			}

		}

		public static void nk_layout_space_begin(nk_context ctx, int fmt, float height, int widget_count)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			nk_panel_layout(ctx, win, (float) (height), (int) (widget_count));
			if ((fmt) == (NK_STATIC)) layout.row.type = (int) (NK_LAYOUT_STATIC_FREE);
			else layout.row.type = (int) (NK_LAYOUT_DYNAMIC_FREE);
			layout.row.ratio = null;
			layout.row.filled = (float) (0);
			layout.row.item_width = (float) (0);
			layout.row.item_offset = (float) (0);
		}

		public static void nk_layout_space_end(nk_context ctx)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			layout.row.item_width = (float) (0);
			layout.row.item_height = (float) (0);
			layout.row.item_offset = (float) (0);
			fixed (void* ptr = &layout.row.item)
			{
				nk_zero(ptr, (ulong) (sizeof (RectangleF)));
			}
		}

		public static void nk_layout_space_push(nk_context ctx, RectangleF rect)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			layout.row.item = (RectangleF) (rect);
		}

		public static RectangleF nk_layout_space_bounds(nk_context ctx)
		{
			RectangleF ret = new RectangleF();
			nk_window win;
			nk_panel layout;
			win = ctx.current;
			layout = win.layout;
			ret.X = (float) (layout.clip.X);
			ret.Y = (float) (layout.clip.Y);
			ret.Width = (float) (layout.clip.Width);
			ret.Height = (float) (layout.row.Height);
			return (RectangleF) (ret);
		}

		public static RectangleF nk_layout_widget_bounds(nk_context ctx)
		{
			RectangleF ret = new RectangleF();
			nk_window win;
			nk_panel layout;
			win = ctx.current;
			layout = win.layout;
			ret.X = (float) (layout.at_x);
			ret.Y = (float) (layout.at_y);
			ret.Width = (float) (layout.bounds.Width - ((layout.at_x - layout.bounds.X) < (0) ? (0) : (layout.at_x - layout.bounds.X)));
			ret.Height = (float) (layout.row.Height);
			return (RectangleF) (ret);
		}

		public static Vector2 nk_layout_space_to_screen(nk_context ctx, Vector2 ret)
		{
			nk_window win;
			nk_panel layout;
			win = ctx.current;
			layout = win.layout;
			ret.X += (float) (layout.at_x - (float) (layout.offset.X));
			ret.Y += (float) (layout.at_y - (float) (layout.offset.Y));
			return (Vector2) (ret);
		}

		public static Vector2 nk_layout_space_to_local(nk_context ctx, Vector2 ret)
		{
			nk_window win;
			nk_panel layout;
			win = ctx.current;
			layout = win.layout;
			ret.X += (float) (-layout.at_x + (float) (layout.offset.X));
			ret.Y += (float) (-layout.at_y + (float) (layout.offset.Y));
			return (Vector2) (ret);
		}

		public static RectangleF nk_layout_space_rect_to_screen(nk_context ctx, RectangleF ret)
		{
			nk_window win;
			nk_panel layout;
			win = ctx.current;
			layout = win.layout;
			ret.X += (float) (layout.at_x - (float) (layout.offset.X));
			ret.Y += (float) (layout.at_y - (float) (layout.offset.Y));
			return (RectangleF) (ret);
		}

		public static RectangleF nk_layout_space_rect_to_local(nk_context ctx, RectangleF ret)
		{
			nk_window win;
			nk_panel layout;
			win = ctx.current;
			layout = win.layout;
			ret.X += (float) (-layout.at_x + (float) (layout.offset.X));
			ret.Y += (float) (-layout.at_y + (float) (layout.offset.Y));
			return (RectangleF) (ret);
		}

		public static void nk_panel_alloc_row(nk_context ctx, nk_window win)
		{
			nk_panel layout = win.layout;
			Vector2 spacing = (Vector2) (ctx.style.Widthindow.spacing);
			float row_height = (float) (layout.row.Height - spacing.Y);
			nk_panel_layout(ctx, win, (float) (row_height), (int) (layout.row.columns));
		}

		public static int nk_tree_state_base(nk_context ctx, int type, nk_image img, char* title, ref int state)
		{
			nk_window win;
			nk_panel layout;
			nk_style style;
			nk_command_buffer _out_;
			nk_input _in_;
			nk_style_button button;
			int symbol;
			float row_height;
			Vector2 item_spacing = new Vector2();
			RectangleF header = new RectangleF();
			RectangleF sym = new RectangleF();
			nk_text text = new nk_text();
			uint ws = (uint) (0);
			int widget_state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			layout = win.layout;
			_out_ = win.buffer;
			style = ctx.style;
			item_spacing = (Vector2) (style.Widthindow.spacing);
			row_height = (float) (style.font.Height + 2*style.tab.padding.Y);
			nk_layout_set_min_row_height(ctx, (float) (row_height));
			nk_layout_row_dynamic(ctx, (float) (row_height), (int) (1));
			nk_layout_reset_min_row_height(ctx);
			widget_state = (int) (nk_widget(&header, ctx));
			if ((type) == (NK_TREE_TAB))
			{
				nk_style_item background = style.tab.background;
				if ((background.type) == (NK_STYLE_ITEM_IMAGE))
				{
					nk_draw_image(_out_, (RectangleF) (header), background.data.image, (Color) (nk_white));
					text.background = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
				}
				else
				{
					text.background = (Color) (background.data.color);
					nk_fill_rect(_out_, (RectangleF) (header), (float) (0), (Color) (style.tab.border_color));
					nk_fill_rect(_out_, (RectangleF) (nk_shriRectangleF_((RectangleF) (header), (float) (style.tab.border))),
						(float) (style.tab.rounding), (Color) (background.data.color));
				}
			}
			else text.background = (Color) (style.Widthindow.background);
			_in_ = ((layout.flags & NK_WINDOW_ROM) == 0) ? ctx.input : null;
			_in_ = (((_in_) != null) && ((widget_state) == (NK_WIDGET_VALID))) ? ctx.input : null;
			if ((nk_button_behavior(ref ws, (RectangleF) (header), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
				state = (int) (((state) == (NK_MAXIMIZED)) ? NK_MINIMIZED : NK_MAXIMIZED);
			if ((state) == (NK_MAXIMIZED))
			{
				symbol = (int) (style.tab.sym_maximize);
				if ((type) == (NK_TREE_TAB)) button = style.tab.tab_maximize_button;
				else button = style.tab.node_maximize_button;
			}
			else
			{
				symbol = (int) (style.tab.sym_minimize);
				if ((type) == (NK_TREE_TAB)) button = style.tab.tab_minimize_button;
				else button = style.tab.node_minimize_button;
			}

			{
				sym.Width = (float) (sym.Height = (float) (style.font.Height));
				sym.Y = (float) (header.Y + style.tab.padding.Y);
				sym.X = (float) (header.X + style.tab.padding.X);
				nk_do_button_symbol(ref ws, win.buffer, (RectangleF) (sym), (int) (symbol), (int) (NK_BUTTON_DEFAULT), button, null,
					style.font);
				if ((img) != null)
				{
					sym.X = (float) (sym.X + sym.Width + 4*item_spacing.X);
					nk_draw_image(win.buffer, (RectangleF) (sym), img, (Color) (nk_white));
					sym.Width = (float) (style.font.Height + style.tab.spacing.X);
				}
			}

			{
				RectangleF label = new RectangleF();
				header.Width = (float) ((header.Width) < (sym.Width + item_spacing.X) ? (sym.Width + item_spacing.X) : (header.Width));
				label.X = (float) (sym.X + sym.Width + item_spacing.X);
				label.Y = (float) (sym.Y);
				label.Width = (float) (header.Width - (sym.Width + item_spacing.Y + style.tab.indent));
				label.Height = (float) (style.font.Height);
				text.text = (Color) (style.tab.text);
				text.padding = (Vector2) (new Vector2((float) (0), (float) (0)));
				nk_widget_text(_out_, (RectangleF) (label), title, (int) (nk_strlen(title)), &text, (uint) (NK_TEXT_LEFT), style.font);
			}

			if ((state) == (NK_MAXIMIZED))
			{
				layout.at_x = (float) (header.X + (float) (layout.offset.X) + style.tab.indent);
				layout.bounds.Width = (float) ((layout.bounds.Width) < (style.tab.indent) ? (style.tab.indent) : (layout.bounds.Width));
				layout.bounds.Width -= (float) (style.tab.indent + style.Widthindow.padding.X);
				layout.row.tree_depth++;
				return (int) (nk_true);
			}
			else return (int) (nk_false);
		}

		public static int nk_tree_base(nk_context ctx, int type, nk_image img, char* title, int initial_state, char* hash,
			int len, int line)
		{
			nk_window win = ctx.current;
			int title_len = (int) (0);
			uint tree_hash = (uint) (0);
			uint* state = null;
			if (hash == null)
			{
				title_len = (int) (nk_strlen(title));
				tree_hash = (uint) (nk_murmur_hash(title, (int) (title_len), (uint) (line)));
			}
			else tree_hash = (uint) (nk_murmur_hash(hash, (int) (len), (uint) (line)));
			state = nk_find_value(win, (uint) (tree_hash));
			if (state == null)
			{
				state = nk_add_value(ctx, win, (uint) (tree_hash), (uint) (0));
				*state = (uint) (initial_state);
			}

			int kkk = (int) (*state);
			int result = (int) (nk_tree_state_base(ctx, (int) (type), img, title, ref kkk));
			*state = (uint) kkk;
			return result;
		}

		public static int nk_tree_state_push(nk_context ctx, int type, char* title, ref int state)
		{
			return (int) (nk_tree_state_base(ctx, (int) (type), null, title, ref state));
		}

		public static int nk_tree_state_image_push(nk_context ctx, int type, nk_image img, char* title, ref int state)
		{
			return (int) (nk_tree_state_base(ctx, (int) (type), img, title, ref state));
		}

		public static void nk_tree_state_pop(nk_context ctx)
		{
			nk_window win = null;
			nk_panel layout = null;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			layout.at_x -= (float) (ctx.style.tab.indent + ctx.style.Widthindow.padding.X);
			layout.bounds.Width += (float) (ctx.style.tab.indent + ctx.style.Widthindow.padding.X);
			layout.row.tree_depth--;
		}

		public static int nk_tree_push_hashed(nk_context ctx, int type, char* title, int initial_state, char* hash, int len,
			int line)
		{
			return (int) (nk_tree_base(ctx, (int) (type), null, title, (int) (initial_state), hash, (int) (len), (int) (line)));
		}

		public static int nk_tree_image_push_hashed(nk_context ctx, int type, nk_image img, char* title, int initial_state,
			char* hash, int len, int seed)
		{
			return (int) (nk_tree_base(ctx, (int) (type), img, title, (int) (initial_state), hash, (int) (len), (int) (seed)));
		}

		public static void nk_tree_pop(nk_context ctx)
		{
			nk_tree_state_pop(ctx);
		}

		public static RectangleF nk_widget_bounds(nk_context ctx)
		{
			RectangleF bounds = new RectangleF();
			if ((ctx == null) || (ctx.current == null))
				return (RectangleF) (RectangleF_((float) (0), (float) (0), (float) (0), (float) (0)));
			nk_layout_peek(&bounds, ctx);
			return (RectangleF) (bounds);
		}

		public static Vector2 nk_widget_position(nk_context ctx)
		{
			RectangleF bounds = new RectangleF();
			if ((ctx == null) || (ctx.current == null)) return (Vector2) (new Vector2((float) (0), (float) (0)));
			nk_layout_peek(&bounds, ctx);
			return (Vector2) (new Vector2((float) (bounds.X), (float) (bounds.Y)));
		}

		public static Vector2 nk_widget_size(nk_context ctx)
		{
			RectangleF bounds = new RectangleF();
			if ((ctx == null) || (ctx.current == null)) return (Vector2) (new Vector2((float) (0), (float) (0)));
			nk_layout_peek(&bounds, ctx);
			return (Vector2) (new Vector2((float) (bounds.Width), (float) (bounds.Height)));
		}

		public static float nk_widget_width(nk_context ctx)
		{
			RectangleF bounds = new RectangleF();
			if ((ctx == null) || (ctx.current == null)) return (float) (0);
			nk_layout_peek(&bounds, ctx);
			return (float) (bounds.Width);
		}

		public static float nk_widget_height(nk_context ctx)
		{
			RectangleF bounds = new RectangleF();
			if ((ctx == null) || (ctx.current == null)) return (float) (0);
			nk_layout_peek(&bounds, ctx);
			return (float) (bounds.Height);
		}

		public static int nk_widget_is_hovered(nk_context ctx)
		{
			RectangleF c = new RectangleF();
			RectangleF v = new RectangleF();
			RectangleF bounds = new RectangleF();
			if (((ctx == null) || (ctx.current == null)) || (ctx.active != ctx.current)) return (int) (0);
			c = (RectangleF) (ctx.current.layout.clip);
			c.X = ((float) ((int) (c.X)));
			c.Y = ((float) ((int) (c.Y)));
			c.Width = ((float) ((int) (c.Width)));
			c.Height = ((float) ((int) (c.Height)));
			nk_layout_peek(&bounds, ctx);
			nk_unify(ref v, ref c, (float) (bounds.X), (float) (bounds.Y), (float) (bounds.X + bounds.Width),
				(float) (bounds.Y + bounds.Height));
			if (
				!(!(((((bounds.X) > (c.X + c.Width)) || ((bounds.X + bounds.Width) < (c.X))) || ((bounds.Y) > (c.Y + c.Height))) ||
				    ((bounds.Y + bounds.Height) < (c.Y))))) return (int) (0);
			return (int) (nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (bounds)));
		}

		public static int nk_widget_is_mouse_clicked(nk_context ctx, int btn)
		{
			RectangleF c = new RectangleF();
			RectangleF v = new RectangleF();
			RectangleF bounds = new RectangleF();
			if (((ctx == null) || (ctx.current == null)) || (ctx.active != ctx.current)) return (int) (0);
			c = (RectangleF) (ctx.current.layout.clip);
			c.X = ((float) ((int) (c.X)));
			c.Y = ((float) ((int) (c.Y)));
			c.Width = ((float) ((int) (c.Width)));
			c.Height = ((float) ((int) (c.Height)));
			nk_layout_peek(&bounds, ctx);
			nk_unify(ref v, ref c, (float) (bounds.X), (float) (bounds.Y), (float) (bounds.X + bounds.Width),
				(float) (bounds.Y + bounds.Height));
			if (
				!(!(((((bounds.X) > (c.X + c.Width)) || ((bounds.X + bounds.Width) < (c.X))) || ((bounds.Y) > (c.Y + c.Height))) ||
				    ((bounds.Y + bounds.Height) < (c.Y))))) return (int) (0);
			return (int) (nk_input_mouse_clicked(ctx.input, (int) (btn), (RectangleF) (bounds)));
		}

		public static int nk_widget_has_mouse_click_down(nk_context ctx, int btn, int down)
		{
			RectangleF c = new RectangleF();
			RectangleF v = new RectangleF();
			RectangleF bounds = new RectangleF();
			if (((ctx == null) || (ctx.current == null)) || (ctx.active != ctx.current)) return (int) (0);
			c = (RectangleF) (ctx.current.layout.clip);
			c.X = ((float) ((int) (c.X)));
			c.Y = ((float) ((int) (c.Y)));
			c.Width = ((float) ((int) (c.Width)));
			c.Height = ((float) ((int) (c.Height)));
			nk_layout_peek(&bounds, ctx);
			nk_unify(ref v, ref c, (float) (bounds.X), (float) (bounds.Y), (float) (bounds.X + bounds.Width),
				(float) (bounds.Y + bounds.Height));
			if (
				!(!(((((bounds.X) > (c.X + c.Width)) || ((bounds.X + bounds.Width) < (c.X))) || ((bounds.Y) > (c.Y + c.Height))) ||
				    ((bounds.Y + bounds.Height) < (c.Y))))) return (int) (0);
			return (int) (nk_input_has_mouse_click_down_in_rect(ctx.input, (int) (btn), (RectangleF) (bounds), (int) (down)));
		}

		public static void nk_spacing(nk_context ctx, int cols)
		{
			nk_window win;
			nk_panel layout;
			RectangleF none = new RectangleF();
			int i;
			int index;
			int rows;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			index = (int) ((layout.row.index + cols)%layout.row.columns);
			rows = (int) ((layout.row.index + cols)/layout.row.columns);
			if ((rows) != 0)
			{
				for (i = (int) (0); (i) < (rows); ++i)
				{
					nk_panel_alloc_row(ctx, win);
				}
				cols = (int) (index);
			}

			if ((layout.row.type != NK_LAYOUT_DYNAMIC_FIXED) && (layout.row.type != NK_LAYOUT_STATIC_FIXED))
			{
				for (i = (int) (0); (i) < (cols); ++i)
				{
					nk_panel_alloc_space(&none, ctx);
				}
			}

			layout.row.index = (int) (index);
		}

		public static void nk_text_colored(nk_context ctx, char* str, int len, uint alignment, Color color)
		{
			nk_window win;
			nk_style style;
			Vector2 item_padding = new Vector2();
			RectangleF bounds = new RectangleF();
			nk_text text = new nk_text();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			style = ctx.style;
			nk_panel_alloc_space(&bounds, ctx);
			item_padding = (Vector2) (style.text.padding);
			text.padding.X = (float) (item_padding.X);
			text.padding.Y = (float) (item_padding.Y);
			text.background = (Color) (style.Widthindow.background);
			text.text = (Color) (color);
			nk_widget_text(win.buffer, (RectangleF) (bounds), str, (int) (len), &text, (uint) (alignment), style.font);
		}

		public static void nk_text_wrap_colored(nk_context ctx, char* str, int len, Color color)
		{
			nk_window win;
			nk_style style;
			Vector2 item_padding = new Vector2();
			RectangleF bounds = new RectangleF();
			nk_text text = new nk_text();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			style = ctx.style;
			nk_panel_alloc_space(&bounds, ctx);
			item_padding = (Vector2) (style.text.padding);
			text.padding.X = (float) (item_padding.X);
			text.padding.Y = (float) (item_padding.Y);
			text.background = (Color) (style.Widthindow.background);
			text.text = (Color) (color);
			nk_widget_text_wrap(win.buffer, (RectangleF) (bounds), str, (int) (len), &text, style.font);
		}

		public static void nk_text_(nk_context ctx, char* str, int len, uint alignment)
		{
			if (ctx == null) return;
			nk_text_colored(ctx, str, (int) (len), (uint) (alignment), (Color) (ctx.style.text.color));
		}

		public static void nk_text_wrap(nk_context ctx, char* str, int len)
		{
			if (ctx == null) return;
			nk_text_wrap_colored(ctx, str, (int) (len), (Color) (ctx.style.text.color));
		}

		public static void nk_label(nk_context ctx, char* str, uint alignment)
		{
			nk_text_(ctx, str, (int) (nk_strlen(str)), (uint) (alignment));
		}

		public static void nk_label_colored(nk_context ctx, char* str, uint align, Color color)
		{
			nk_text_colored(ctx, str, (int) (nk_strlen(str)), (uint) (align), (Color) (color));
		}

		public static void nk_label_wrap(nk_context ctx, char* str)
		{
			nk_text_wrap(ctx, str, (int) (nk_strlen(str)));
		}

		public static void nk_label_colored_wrap(nk_context ctx, char* str, Color color)
		{
			nk_text_wrap_colored(ctx, str, (int) (nk_strlen(str)), (Color) (color));
		}

		public static void nk_image_(nk_context ctx, nk_image img)
		{
			nk_window win;
			RectangleF bounds = new RectangleF();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			if (nk_widget(&bounds, ctx) == 0) return;
			nk_draw_image(win.buffer, (RectangleF) (bounds), img, (Color) (nk_white));
		}

		public static void nk_button_set_behavior(nk_context ctx, int behavior)
		{
			if (ctx == null) return;
			ctx.button_behavior = (int) (behavior);
		}

		public static int nk_button_push_behavior(nk_context ctx, int behavior)
		{
			nk_config_stack_button_behavior button_stack;
			nk_config_stack_button_behavior_element element;
			if (ctx == null) return (int) (0);
			button_stack = ctx.stacks.button_behaviors;
			if ((button_stack.Heightead) >= ((int) ((int) button_stack.elements.Length))) return (int) (0);
			element = button_stack.elements[button_stack.Heightead++];
			element.old_value = (int) (ctx.button_behavior);
			ctx.button_behavior = (int) (behavior);
			return (int) (1);
		}

		public static int nk_button_pop_behavior(nk_context ctx)
		{
			nk_config_stack_button_behavior button_stack;
			nk_config_stack_button_behavior_element element;
			if (ctx == null) return (int) (0);
			button_stack = ctx.stacks.button_behaviors;
			if ((button_stack.Heightead) < (1)) return (int) (0);
			element = button_stack.elements[--button_stack.Heightead];
			ctx.button_behavior = element.old_value;
			return (int) (1);
		}

		public static int nk_button_text_styled(nk_context ctx, nk_style_button style, char* title, int len)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			RectangleF bounds = new RectangleF();
			int state;
			if ((((style == null) || (ctx == null)) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			return
				(int)
					(nk_do_button_text(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), title, (int) (len),
						(uint) (style.text_alignment), (int) (ctx.button_behavior), style, _in_, ctx.style.font));
		}

		public static int nk_button_text(nk_context ctx, char* title, int len)
		{
			if (ctx == null) return (int) (0);
			return (int) (nk_button_text_styled(ctx, ctx.style.button, title, (int) (len)));
		}

		public static int nk_button_label_styled(nk_context ctx, nk_style_button style, char* title)
		{
			return (int) (nk_button_text_styled(ctx, style, title, (int) (nk_strlen(title))));
		}

		public static int nk_button_label(nk_context ctx, char* title)
		{
			return (int) (nk_button_text(ctx, title, (int) (nk_strlen(title))));
		}

		public static int nk_button_color(nk_context ctx, Color color)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			nk_style_button button = new nk_style_button();
			int ret = (int) (0);
			RectangleF bounds = new RectangleF();
			RectangleF content = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			button = (nk_style_button) (ctx.style.button);
			button.normal = (nk_style_item) (nk_style_item_color((Color) (color)));
			button.hover = (nk_style_item) (nk_style_item_color((Color) (color)));
			button.active = (nk_style_item) (nk_style_item_color((Color) (color)));
			ret =
				(int)
					(nk_do_button(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), button, _in_, (int) (ctx.button_behavior),
						&content));
			nk_draw_button(win.buffer, &bounds, (uint) (ctx.last_widget_state), button);
			return (int) (ret);
		}

		public static int nk_button_symbol_styled(nk_context ctx, nk_style_button style, int symbol)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			RectangleF bounds = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			return
				(int)
					(nk_do_button_symbol(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), (int) (symbol),
						(int) (ctx.button_behavior), style, _in_, ctx.style.font));
		}

		public static int nk_button_symbol(nk_context ctx, int symbol)
		{
			if (ctx == null) return (int) (0);
			return (int) (nk_button_symbol_styled(ctx, ctx.style.button, (int) (symbol)));
		}

		public static int nk_button_image_styled(nk_context ctx, nk_style_button style, nk_image img)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			RectangleF bounds = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			return
				(int)
					(nk_do_button_image(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), (nk_image) (img),
						(int) (ctx.button_behavior), style, _in_));
		}

		public static int nk_button_image(nk_context ctx, nk_image img)
		{
			if (ctx == null) return (int) (0);
			return (int) (nk_button_image_styled(ctx, ctx.style.button, (nk_image) (img)));
		}

		public static int nk_button_symbol_text_styled(nk_context ctx, nk_style_button style, int symbol, char* text, int len,
			uint align)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			RectangleF bounds = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			return
				(int)
					(nk_do_button_text_symbol(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), (int) (symbol), text,
						(int) (len), (uint) (align), (int) (ctx.button_behavior), style, ctx.style.font, _in_));
		}

		public static int nk_button_symbol_text(nk_context ctx, int symbol, char* text, int len, uint align)
		{
			if (ctx == null) return (int) (0);
			return (int) (nk_button_symbol_text_styled(ctx, ctx.style.button, (int) (symbol), text, (int) (len), (uint) (align)));
		}

		public static int nk_button_symbol_label(nk_context ctx, int symbol, char* label, uint align)
		{
			return (int) (nk_button_symbol_text(ctx, (int) (symbol), label, (int) (nk_strlen(label)), (uint) (align)));
		}

		public static int nk_button_symbol_label_styled(nk_context ctx, nk_style_button style, int symbol, char* title,
			uint align)
		{
			return
				(int) (nk_button_symbol_text_styled(ctx, style, (int) (symbol), title, (int) (nk_strlen(title)), (uint) (align)));
		}

		public static int nk_button_image_text_styled(nk_context ctx, nk_style_button style, nk_image img, char* text, int len,
			uint align)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			RectangleF bounds = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			return
				(int)
					(nk_do_button_text_image(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), (nk_image) (img), text,
						(int) (len), (uint) (align), (int) (ctx.button_behavior), style, ctx.style.font, _in_));
		}

		public static int nk_button_image_text(nk_context ctx, nk_image img, char* text, int len, uint align)
		{
			return
				(int) (nk_button_image_text_styled(ctx, ctx.style.button, (nk_image) (img), text, (int) (len), (uint) (align)));
		}

		public static int nk_button_image_label(nk_context ctx, nk_image img, char* label, uint align)
		{
			return (int) (nk_button_image_text(ctx, (nk_image) (img), label, (int) (nk_strlen(label)), (uint) (align)));
		}

		public static int nk_button_image_label_styled(nk_context ctx, nk_style_button style, nk_image img, char* label,
			uint text_alignment)
		{
			return
				(int)
					(nk_button_image_text_styled(ctx, style, (nk_image) (img), label, (int) (nk_strlen(label)), (uint) (text_alignment)));
		}

		public static int nk_selectable_text(nk_context ctx, char* str, int len, uint align, ref int value)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			nk_style style;
			int state;
			RectangleF bounds = new RectangleF();
			if ((((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null))) return (int) (0);
			win = ctx.current;
			layout = win.layout;
			style = ctx.style;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			return
				(int)
					(nk_do_selectable(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), str, (int) (len), (uint) (align),
						ref value, style.selectable, _in_, style.font));
		}

		public static int nk_selectable_image_text(nk_context ctx, nk_image img, char* str, int len, uint align, ref int value)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			nk_style style;
			int state;
			RectangleF bounds = new RectangleF();
			if ((((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null))) return (int) (0);
			win = ctx.current;
			layout = win.layout;
			style = ctx.style;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			return
				(int)
					(nk_do_selectable_image(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), str, (int) (len), (uint) (align),
						ref value, img, style.selectable, _in_, style.font));
		}

		public static int nk_select_text(nk_context ctx, char* str, int len, uint align, int value)
		{
			nk_selectable_text(ctx, str, (int) (len), (uint) (align), ref value);
			return (int) (value);
		}

		public static int nk_selectable_label(nk_context ctx, char* str, uint align, ref int value)
		{
			return (int) (nk_selectable_text(ctx, str, (int) (nk_strlen(str)), (uint) (align), ref value));
		}

		public static int nk_selectable_image_label(nk_context ctx, nk_image img, char* str, uint align, ref int value)
		{
			return
				(int) (nk_selectable_image_text(ctx, (nk_image) (img), str, (int) (nk_strlen(str)), (uint) (align), ref value));
		}

		public static int nk_select_label(nk_context ctx, char* str, uint align, int value)
		{
			nk_selectable_text(ctx, str, (int) (nk_strlen(str)), (uint) (align), ref value);
			return (int) (value);
		}

		public static int nk_select_image_label(nk_context ctx, nk_image img, char* str, uint align, int value)
		{
			nk_selectable_image_text(ctx, (nk_image) (img), str, (int) (nk_strlen(str)), (uint) (align), ref value);
			return (int) (value);
		}

		public static int nk_select_image_text(nk_context ctx, nk_image img, char* str, int len, uint align, int value)
		{
			nk_selectable_image_text(ctx, (nk_image) (img), str, (int) (len), (uint) (align), ref value);
			return (int) (value);
		}

		public static int nk_check_text(nk_context ctx, char* text, int len, int active)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			nk_style style;
			RectangleF bounds = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (active);
			win = ctx.current;
			style = ctx.style;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (active);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			nk_do_toggle(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), &active, text, (int) (len),
				(int) (NK_TOGGLE_CHECK), style.checkbox, _in_, style.font);
			return (int) (active);
		}

		public static uint nk_check_flags_text(nk_context ctx, char* text, int len, uint flags, uint value)
		{
			int old_active;
			if ((ctx == null) || (text == null)) return (uint) (flags);
			old_active = ((int) ((flags & value) & value));
			if ((nk_check_text(ctx, text, (int) (len), (int) (old_active))) != 0) flags |= (uint) (value);
			else flags &= (uint) (~value);
			return (uint) (flags);
		}

		public static int nk_checkbox_text(nk_context ctx, char* text, int len, int* active)
		{
			int old_val;
			if (((ctx == null) || (text == null)) || (active == null)) return (int) (0);
			old_val = (int) (*active);
			*active = (int) (nk_check_text(ctx, text, (int) (len), (int) (*active)));
			return (old_val != *active) ? 1 : 0;
		}

		public static int nk_checkbox_flags_text(nk_context ctx, char* text, int len, uint* flags, uint value)
		{
			int active;
			if (((ctx == null) || (text == null)) || (flags == null)) return (int) (0);
			active = ((int) ((*flags & value) & value));
			if ((nk_checkbox_text(ctx, text, (int) (len), &active)) != 0)
			{
				if ((active) != 0) *flags |= (uint) (value);
				else *flags &= (uint) (~value);
				return (int) (1);
			}

			return (int) (0);
		}

		public static int nk_check_label(nk_context ctx, char* label, int active)
		{
			return (int) (nk_check_text(ctx, label, (int) (nk_strlen(label)), (int) (active)));
		}

		public static uint nk_check_flags_label(nk_context ctx, char* label, uint flags, uint value)
		{
			return (uint) (nk_check_flags_text(ctx, label, (int) (nk_strlen(label)), (uint) (flags), (uint) (value)));
		}

		public static int nk_checkbox_label(nk_context ctx, char* label, int* active)
		{
			return (int) (nk_checkbox_text(ctx, label, (int) (nk_strlen(label)), active));
		}

		public static int nk_checkbox_flags_label(nk_context ctx, char* label, uint* flags, uint value)
		{
			return (int) (nk_checkbox_flags_text(ctx, label, (int) (nk_strlen(label)), flags, (uint) (value)));
		}

		public static int nk_option_text(nk_context ctx, char* text, int len, int is_active)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			nk_style style;
			RectangleF bounds = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (is_active);
			win = ctx.current;
			style = ctx.style;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (state);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			nk_do_toggle(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), &is_active, text, (int) (len),
				(int) (NK_TOGGLE_OPTION), style.option, _in_, style.font);
			return (int) (is_active);
		}

		public static int nk_radio_text(nk_context ctx, char* text, int len, int* active)
		{
			int old_value;
			if (((ctx == null) || (text == null)) || (active == null)) return (int) (0);
			old_value = (int) (*active);
			*active = (int) (nk_option_text(ctx, text, (int) (len), (int) (old_value)));
			return (old_value != *active) ? 1 : 0;
		}

		public static int nk_option_label(nk_context ctx, char* label, int active)
		{
			return (int) (nk_option_text(ctx, label, (int) (nk_strlen(label)), (int) (active)));
		}

		public static int nk_radio_label(nk_context ctx, char* label, int* active)
		{
			return (int) (nk_radio_text(ctx, label, (int) (nk_strlen(label)), active));
		}

		public static int nk_slider_float(nk_context ctx, float min_value, ref float value, float max_value, float value_step)
		{
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			nk_style style;
			int ret = (int) (0);
			float old_value;
			RectangleF bounds = new RectangleF();
			int state;
			if ((((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)))
				return (int) (ret);
			win = ctx.current;
			style = ctx.style;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (ret);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			old_value = (float) (value);
			value =
				(float)
					(nk_do_slider(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), (float) (min_value), (float) (old_value),
						(float) (max_value), (float) (value_step), style.slider, _in_, style.font));
			return (((old_value) > (value)) || ((old_value) < (value))) ? 1 : 0;
		}

		public static float nk_slide_float(nk_context ctx, float min, float val, float max, float step)
		{
			nk_slider_float(ctx, (float) (min), ref val, (float) (max), (float) (step));
			return (float) (val);
		}

		public static int nk_slide_int(nk_context ctx, int min, int val, int max, int step)
		{
			float value = (float) (val);
			nk_slider_float(ctx, (float) (min), ref value, (float) (max), (float) (step));
			return (int) (value);
		}

		public static int nk_slider_int(nk_context ctx, int min, ref int val, int max, int step)
		{
			int ret;
			float value = (float) (val);
			ret = (int) (nk_slider_float(ctx, (float) (min), ref value, (float) (max), (float) (step)));
			val = ((int) (value));
			return (int) (ret);
		}

		public static int nk_progress(nk_context ctx, ulong* cur, ulong max, int is_modifyable)
		{
			nk_window win;
			nk_panel layout;
			nk_style style;
			nk_input _in_;
			RectangleF bounds = new RectangleF();
			int state;
			ulong old_value;
			if ((((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) || (cur == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			old_value = (ulong) (*cur);
			*cur =
				(ulong)
					(nk_do_progress(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), (ulong) (*cur), (ulong) (max),
						(int) (is_modifyable), style.progress, _in_));
			return (*cur != old_value) ? 1 : 0;
		}

		public static ulong nk_prog(nk_context ctx, ulong cur, ulong max, int modifyable)
		{
			nk_progress(ctx, &cur, (ulong) (max), (int) (modifyable));
			return (ulong) (cur);
		}

		public static void nk_edit_focus(nk_context ctx, uint flags)
		{
			uint hash;
			nk_window win;
			if ((ctx == null) || (ctx.current == null)) return;
			win = ctx.current;
			hash = (uint) (win.edit.seq);
			win.edit.active = (int) (nk_true);
			win.edit.name = (uint) (hash);
			if ((flags & NK_EDIT_ALWAYS_INSERT_MODE) != 0) win.edit.mode = (byte) (NK_TEXT_EDIT_MODE_INSERT);
		}

		public static void nk_edit_unfocus(nk_context ctx)
		{
			nk_window win;
			if ((ctx == null) || (ctx.current == null)) return;
			win = ctx.current;
			win.edit.active = (int) (nk_false);
			win.edit.name = (uint) (0);
		}

		public static uint nk_edit_string(nk_context ctx, uint flags, NkStr str, int max,
			NkPluginFilter filter)
		{
			uint hash;
			uint state;
			nk_text_edit edit;
			nk_window win;
			if (((ctx == null))) return (uint) (0);
			filter = (filter == null) ? nk_filter_default : filter;
			win = ctx.current;
			hash = (uint) (win.edit.seq);
			edit = ctx.text_edit;
			nk_textedit_clear_state(ctx.text_edit,
				(int) ((flags & NK_EDIT_MULTILINE) != 0 ? NK_TEXT_EDIT_MULTI_LINE : NK_TEXT_EDIT_SINGLE_LINE), filter);
			if (((win.edit.active) != 0) && ((hash) == (win.edit.name)))
			{
				if ((flags & NK_EDIT_NO_CURSOR) != 0) edit.cursor = (int) (str.len);
				else edit.cursor = (int) (win.edit.cursor);
				if ((flags & NK_EDIT_SELECTABLE) == 0)
				{
					edit.select_start = (int) (win.edit.cursor);
					edit.select_end = (int) (win.edit.cursor);
				}
				else
				{
					edit.select_start = (int) (win.edit.sel_start);
					edit.select_end = (int) (win.edit.sel_end);
				}
				edit.mode = (byte) (win.edit.mode);
				edit.scrollbar.X = ((float) (win.edit.scrollbar.X));
				edit.scrollbar.Y = ((float) (win.edit.scrollbar.Y));
				edit.active = (byte) (nk_true);
			}
			else edit.active = (byte) (nk_false);
			max = (int) ((1) < (max) ? (max) : (1));

			if (str.len > max)
			{
				str.str = str.str.Substring(0, max);
			}

			edit._string_ = str;
			state = (uint) (nk_edit_buffer(ctx, (uint) (flags), edit, filter));
			if ((edit.active) != 0)
			{
				win.edit.cursor = (int) (edit.cursor);
				win.edit.sel_start = (int) (edit.select_start);
				win.edit.sel_end = (int) (edit.select_end);
				win.edit.mode = (byte) (edit.mode);
				win.edit.scrollbar.X = ((uint) (edit.scrollbar.X));
				win.edit.scrollbar.Y = ((uint) (edit.scrollbar.Y));
			}

			return (uint) (state);
		}

		public static uint nk_edit_buffer(nk_context ctx, uint flags, nk_text_edit edit, NkPluginFilter filter)
		{
			nk_window win;
			nk_style style;
			nk_input _in_;
			int state;
			RectangleF bounds = new RectangleF();
			uint ret_flags = (uint) (0);
			byte prev_state;
			uint hash;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (uint) (0);
			win = ctx.current;
			style = ctx.style;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (uint) (state);
			_in_ = (win.layout.flags & NK_WINDOW_ROM) != 0 ? null : ctx.input;
			hash = (uint) (win.edit.seq++);
			if (((win.edit.active) != 0) && ((hash) == (win.edit.name)))
			{
				if ((flags & NK_EDIT_NO_CURSOR) != 0) edit.cursor = (int) (edit._string_.len);
				if ((flags & NK_EDIT_SELECTABLE) == 0)
				{
					edit.select_start = (int) (edit.cursor);
					edit.select_end = (int) (edit.cursor);
				}
				if ((flags & NK_EDIT_CLIPBOARD) != 0) edit.clip = (nk_clipboard) (ctx.clip);
				edit.active = ((byte) (win.edit.active));
			}
			else edit.active = (byte) (nk_false);
			edit.mode = (byte) (win.edit.mode);
			filter = (filter == null) ? nk_filter_default : filter;
			prev_state = (byte) (edit.active);
			_in_ = (flags & NK_EDIT_READ_ONLY) != 0 ? null : _in_;
			ret_flags =
				(uint)
					(nk_do_edit(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), (uint) (flags), filter, edit, style.edit,
						_in_, style.font));
			if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0)
				ctx.style.cursor_active = ctx.style.cursors[NK_CURSOR_TEXT];
			if (((edit.active) != 0) && (prev_state != edit.active))
			{
				win.edit.active = (int) (nk_true);
				win.edit.name = (uint) (hash);
			}
			else if (((prev_state) != 0) && (edit.active == 0))
			{
				win.edit.active = (int) (nk_false);
			}

			return (uint) (ret_flags);
		}

		public static void nk_property_int(nk_context ctx, char* name, int min, ref int val, int max, int step,
			float inc_per_pixel)
		{
			nk_property_variant variant = new nk_property_variant();
			if ((((ctx == null) || (ctx.current == null)) || (name == null))) return;
			variant = (nk_property_variant) (nk_property_variant_int((int) (val), (int) (min), (int) (max), (int) (step)));
			nk_property_(ctx, name, &variant, (float) (inc_per_pixel), (int) (NK_FILTER_INT));
			val = (int) (variant.value.i);
		}

		public static void nk_property_float(nk_context ctx, char* name, float min, ref float val, float max, float step,
			float inc_per_pixel)
		{
			nk_property_variant variant = new nk_property_variant();
			if ((((ctx == null) || (ctx.current == null)) || (name == null))) return;
			variant =
				(nk_property_variant) (nk_property_variant_float((float) (val), (float) (min), (float) (max), (float) (step)));
			nk_property_(ctx, name, &variant, (float) (inc_per_pixel), (int) (NK_FILTER_FLOAT));
			val = (float) (variant.value.f);
		}

		public static void nk_property_double(nk_context ctx, char* name, double min, ref double val, double max, double step,
			float inc_per_pixel)
		{
			nk_property_variant variant = new nk_property_variant();
			if ((((ctx == null) || (ctx.current == null)) || (name == null))) return;
			variant =
				(nk_property_variant) (nk_property_variant_double((double) (val), (double) (min), (double) (max), (double) (step)));
			nk_property_(ctx, name, &variant, (float) (inc_per_pixel), (int) (NK_FILTER_FLOAT));
			val = (double) (variant.value.d);
		}

		public static int nk_propertyi(nk_context ctx, char* name, int min, int val, int max, int step, float inc_per_pixel)
		{
			nk_property_variant variant = new nk_property_variant();
			if (((ctx == null) || (ctx.current == null)) || (name == null)) return (int) (val);
			variant = (nk_property_variant) (nk_property_variant_int((int) (val), (int) (min), (int) (max), (int) (step)));
			nk_property_(ctx, name, &variant, (float) (inc_per_pixel), (int) (NK_FILTER_INT));
			val = (int) (variant.value.i);
			return (int) (val);
		}

		public static float nk_propertyf(nk_context ctx, char* name, float min, float val, float max, float step,
			float inc_per_pixel)
		{
			nk_property_variant variant = new nk_property_variant();
			if (((ctx == null) || (ctx.current == null)) || (name == null)) return (float) (val);
			variant =
				(nk_property_variant) (nk_property_variant_float((float) (val), (float) (min), (float) (max), (float) (step)));
			nk_property_(ctx, name, &variant, (float) (inc_per_pixel), (int) (NK_FILTER_FLOAT));
			val = (float) (variant.value.f);
			return (float) (val);
		}

		public static double nk_propertyd(nk_context ctx, char* name, double min, double val, double max, double step,
			float inc_per_pixel)
		{
			nk_property_variant variant = new nk_property_variant();
			if (((ctx == null) || (ctx.current == null)) || (name == null)) return (double) (val);
			variant =
				(nk_property_variant) (nk_property_variant_double((double) (val), (double) (min), (double) (max), (double) (step)));
			nk_property_(ctx, name, &variant, (float) (inc_per_pixel), (int) (NK_FILTER_FLOAT));
			val = (double) (variant.value.d);
			return (double) (val);
		}

		public static int Color_pick(nk_context ctx, Colorf* color, int fmt)
		{
			nk_window win;
			nk_panel layout;
			nk_style config;
			nk_input _in_;
			int state;
			RectangleF bounds = new RectangleF();
			if ((((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) || (color == null)) return (int) (0);
			win = ctx.current;
			config = ctx.style;
			layout = win.layout;
			state = (int) (nk_widget(&bounds, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			return
				(int)
					(nk_do_color_picker(ref ctx.last_widget_state, win.buffer, color, (int) (fmt), (RectangleF) (bounds),
						(Vector2) (new Vector2((float) (0), (float) (0))), _in_, config.font));
		}

		public static Colorf Color_picker(nk_context ctx, Colorf color, int fmt)
		{
			Color_pick(ctx, &color, (int) (fmt));
			return (Colorf) (color);
		}

		public static int nk_chart_begin_colored(nk_context ctx, int type, Color color, Color highlight, int count,
			float min_value, float max_value)
		{
			nk_window win;
			nk_chart chart;
			nk_style config;
			nk_style_chart style;
			nk_style_item background;
			RectangleF bounds = new RectangleF();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			if (nk_widget(&bounds, ctx) == 0)
			{
				chart = ctx.current.layout.chart;
				return (int) (0);
			}

			win = ctx.current;
			config = ctx.style;
			chart = win.layout.chart;
			style = config.chart;

			chart.X = (float) (bounds.X + style.padding.X);
			chart.Y = (float) (bounds.Y + style.padding.Y);
			chart.Width = (float) (bounds.Width - 2*style.padding.X);
			chart.Height = (float) (bounds.Height - 2*style.padding.Y);
			chart.Width = (float) ((chart.Width) < (2*style.padding.X) ? (2*style.padding.X) : (chart.Width));
			chart.Height = (float) ((chart.Height) < (2*style.padding.Y) ? (2*style.padding.Y) : (chart.Height));
			{
				nk_chart_slot slot = chart.slots[chart.slot++];
				slot.type = (int) (type);
				slot.count = (int) (count);
				slot.color = (Color) (color);
				slot.Heightighlight = (Color) (highlight);
				slot.min = (float) ((min_value) < (max_value) ? (min_value) : (max_value));
				slot.max = (float) ((min_value) < (max_value) ? (max_value) : (min_value));
				slot.range = (float) (slot.max - slot.min);
			}

			background = style.background;
			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				nk_draw_image(win.buffer, (RectangleF) (bounds), background.data.image, (Color) (nk_white));
			}
			else
			{
				nk_fill_rect(win.buffer, (RectangleF) (bounds), (float) (style.rounding), (Color) (style.border_color));
				nk_fill_rect(win.buffer, (RectangleF) (nk_shriRectangleF_((RectangleF) (bounds), (float) (style.border))),
					(float) (style.rounding), (Color) (style.background.data.color));
			}

			return (int) (1);
		}

		public static int nk_chart_begin(nk_context ctx, int type, int count, float min_value, float max_value)
		{
			return
				(int)
					(nk_chart_begin_colored(ctx, (int) (type), (Color) (ctx.style.chart.color),
						(Color) (ctx.style.chart.selected_color), (int) (count), (float) (min_value), (float) (max_value)));
		}

		public static void nk_chart_add_slot_colored(nk_context ctx, int type, Color color, Color highlight, int count,
			float min_value, float max_value)
		{
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			if ((ctx.current.layout.chart.slot) >= (4)) return;
			{
				nk_chart chart = ctx.current.layout.chart;
				nk_chart_slot slot = chart.slots[chart.slot++];
				slot.type = (int) (type);
				slot.count = (int) (count);
				slot.color = (Color) (color);
				slot.Heightighlight = (Color) (highlight);
				slot.min = (float) ((min_value) < (max_value) ? (min_value) : (max_value));
				slot.max = (float) ((min_value) < (max_value) ? (max_value) : (min_value));
				slot.range = (float) (slot.max - slot.min);
			}

		}

		public static void nk_chart_add_slot(nk_context ctx, int type, int count, float min_value, float max_value)
		{
			nk_chart_add_slot_colored(ctx, (int) (type), (Color) (ctx.style.chart.color),
				(Color) (ctx.style.chart.selected_color), (int) (count), (float) (min_value), (float) (max_value));
		}

		public static uint nk_chart_push_line(nk_context ctx, nk_window win, nk_chart g, float value, int slot)
		{
			nk_panel layout = win.layout;
			nk_input i = ctx.input;
			nk_command_buffer _out_ = win.buffer;
			uint ret = (uint) (0);
			Vector2 cur = new Vector2();
			RectangleF bounds = new RectangleF();
			Color color = new Color();
			float step;
			float range;
			float ratio;
			step = (float) (g.Width/(float) (g.slots[slot].count));
			range = (float) (g.slots[slot].max - g.slots[slot].min);
			ratio = (float) ((value - g.slots[slot].min)/range);
			if ((g.slots[slot].index) == (0))
			{
				g.slots[slot].last.X = (float) (g.X);
				g.slots[slot].last.Y = (float) ((g.Y + g.Height) - ratio*g.Height);
				bounds.X = (float) (g.slots[slot].last.X - 2);
				bounds.Y = (float) (g.slots[slot].last.Y - 2);
				bounds.Width = (float) (bounds.Height = (float) (4));
				color = (Color) (g.slots[slot].color);
				if (((layout.flags & NK_WINDOW_ROM) == 0) &&
				    ((((g.slots[slot].last.X - 3) <= (i.mouse.pos.X)) && ((i.mouse.pos.X) < (g.slots[slot].last.X - 3 + 6))) &&
				     (((g.slots[slot].last.Y - 3) <= (i.mouse.pos.Y)) && ((i.mouse.pos.Y) < (g.slots[slot].last.Y - 3 + 6)))))
				{
					ret = (uint) ((nk_input_is_mouse_hovering_rect(i, (RectangleF) (bounds))) != 0 ? NK_CHART_HOVERING : 0);
					ret |=
						(uint)
							((((i.mouse.buttons[NK_BUTTON_LEFT].down) != 0) && ((i.mouse.buttons[NK_BUTTON_LEFT].clicked) != 0))
								? NK_CHART_CLICKED
								: 0);
					color = (Color) (g.slots[slot].Heightighlight);
				}
				nk_fill_rect(_out_, (RectangleF) (bounds), (float) (0), (Color) (color));
				g.slots[slot].index += (int) (1);
				return (uint) (ret);
			}

			color = (Color) (g.slots[slot].color);
			cur.X = (float) (g.X + (step*(float) (g.slots[slot].index)));
			cur.Y = (float) ((g.Y + g.Height) - (ratio*g.Height));
			nk_stroke_line(_out_, (float) (g.slots[slot].last.X), (float) (g.slots[slot].last.Y), (float) (cur.X),
				(float) (cur.Y), (float) (1.0f), (Color) (color));
			bounds.X = (float) (cur.X - 3);
			bounds.Y = (float) (cur.Y - 3);
			bounds.Width = (float) (bounds.Height = (float) (6));
			if ((layout.flags & NK_WINDOW_ROM) == 0)
			{
				if ((nk_input_is_mouse_hovering_rect(i, (RectangleF) (bounds))) != 0)
				{
					ret = (uint) (NK_CHART_HOVERING);
					ret |=
						(uint)
							(((i.mouse.buttons[NK_BUTTON_LEFT].down == 0) && ((i.mouse.buttons[NK_BUTTON_LEFT].clicked) != 0))
								? NK_CHART_CLICKED
								: 0);
					color = (Color) (g.slots[slot].Heightighlight);
				}
			}

			nk_fill_rect(_out_, (RectangleF) (RectangleF_((float) (cur.X - 2), (float) (cur.Y - 2), (float) (4), (float) (4))),
				(float) (0), (Color) (color));
			g.slots[slot].last.X = (float) (cur.X);
			g.slots[slot].last.Y = (float) (cur.Y);
			g.slots[slot].index += (int) (1);
			return (uint) (ret);
		}

		public static uint nk_chart_push_column(nk_context ctx, nk_window win, nk_chart chart, float value, int slot)
		{
			nk_command_buffer _out_ = win.buffer;
			nk_input _in_ = ctx.input;
			nk_panel layout = win.layout;
			float ratio;
			uint ret = (uint) (0);
			Color color = new Color();
			RectangleF item = new RectangleF();
			if ((chart.slots[slot].index) >= (chart.slots[slot].count)) return (uint) (nk_false);
			if ((chart.slots[slot].count) != 0)
			{
				float padding = (float) (chart.slots[slot].count - 1);
				item.Width = (float) ((chart.Width - padding)/(float) (chart.slots[slot].count));
			}

			color = (Color) (chart.slots[slot].color);
			item.Height =
				(float)
					(chart.Height*
					 (((value/chart.slots[slot].range) < (0)) ? -(value/chart.slots[slot].range) : (value/chart.slots[slot].range)));
			if ((value) >= (0))
			{
				ratio =
					(float)
						((value + (((chart.slots[slot].min) < (0)) ? -(chart.slots[slot].min) : (chart.slots[slot].min)))/
						 (((chart.slots[slot].range) < (0)) ? -(chart.slots[slot].range) : (chart.slots[slot].range)));
				item.Y = (float) ((chart.Y + chart.Height) - chart.Height*ratio);
			}
			else
			{
				ratio = (float) ((value - chart.slots[slot].max)/chart.slots[slot].range);
				item.Y = (float) (chart.Y + (chart.Height*(((ratio) < (0)) ? -(ratio) : (ratio))) - item.Height);
			}

			item.X = (float) (chart.X + ((float) (chart.slots[slot].index)*item.Width));
			item.X = (float) (item.X + ((float) (chart.slots[slot].index)));
			if (((layout.flags & NK_WINDOW_ROM) == 0) &&
			    ((((item.X) <= (_in_.mouse.pos.X)) && ((_in_.mouse.pos.X) < (item.X + item.Width))) &&
			     (((item.Y) <= (_in_.mouse.pos.Y)) && ((_in_.mouse.pos.Y) < (item.Y + item.Height)))))
			{
				ret = (uint) (NK_CHART_HOVERING);
				ret |=
					(uint)
						(((_in_.mouse.buttons[NK_BUTTON_LEFT].down == 0) &&
						  ((_in_.mouse.buttons[NK_BUTTON_LEFT].clicked) != 0))
							? NK_CHART_CLICKED
							: 0);
				color = (Color) (chart.slots[slot].Heightighlight);
			}

			nk_fill_rect(_out_, (RectangleF) (item), (float) (0), (Color) (color));
			chart.slots[slot].index += (int) (1);
			return (uint) (ret);
		}

		public static uint nk_chart_push_slot(nk_context ctx, float value, int slot)
		{
			uint flags;
			nk_window win;
			if (((ctx == null) || (ctx.current == null)) || ((slot) >= (4))) return (uint) (nk_false);
			if ((slot) >= (ctx.current.layout.chart.slot)) return (uint) (nk_false);
			win = ctx.current;
			if ((win.layout.chart.slot) < (slot)) return (uint) (nk_false);
			switch (win.layout.chart.slots[slot].type)
			{
				case NK_CHART_LINES:
					flags = (uint) (nk_chart_push_line(ctx, win, win.layout.chart, (float) (value), (int) (slot)));
					break;
				case NK_CHART_COLUMN:
					flags = (uint) (nk_chart_push_column(ctx, win, win.layout.chart, (float) (value), (int) (slot)));
					break;
				default:
				case NK_CHART_MAX:
					flags = (uint) (0);
					break;
			}

			return (uint) (flags);
		}

		public static uint nk_chart_push(nk_context ctx, float value)
		{
			return (uint) (nk_chart_push_slot(ctx, (float) (value), (int) (0)));
		}

		public static void nk_chart_end(nk_context ctx)
		{
			nk_window win;
			nk_chart chart;
			if ((ctx == null) || (ctx.current == null)) return;
			win = ctx.current;
			chart = win.layout.chart;

			return;
		}

		public static void nk_plot(nk_context ctx, int type, float* values, int count, int offset)
		{
			int i = (int) (0);
			float min_value;
			float max_value;
			if (((ctx == null) || (values == null)) || (count == 0)) return;
			min_value = (float) (values[offset]);
			max_value = (float) (values[offset]);
			for (i = (int) (0); (i) < (count); ++i)
			{
				min_value = (float) ((values[i + offset]) < (min_value) ? (values[i + offset]) : (min_value));
				max_value = (float) ((values[i + offset]) < (max_value) ? (max_value) : (values[i + offset]));
			}
			if ((nk_chart_begin(ctx, (int) (type), (int) (count), (float) (min_value), (float) (max_value))) != 0)
			{
				for (i = (int) (0); (i) < (count); ++i)
				{
					nk_chart_push(ctx, (float) (values[i + offset]));
				}
				nk_chart_end(ctx);
			}

		}

		public static void nk_plot_function(nk_context ctx, int type, void* userdata, NkFloatValueGetter value_getter,
			int count, int offset)
		{
			int i = (int) (0);
			float min_value;
			float max_value;
			if (((ctx == null) || (value_getter == null)) || (count == 0)) return;
			max_value = (float) (min_value = (float) (value_getter(userdata, (int) (offset))));
			for (i = (int) (0); (i) < (count); ++i)
			{
				float value = (float) (value_getter(userdata, (int) (i + offset)));
				min_value = (float) ((value) < (min_value) ? (value) : (min_value));
				max_value = (float) ((value) < (max_value) ? (max_value) : (value));
			}
			if ((nk_chart_begin(ctx, (int) (type), (int) (count), (float) (min_value), (float) (max_value))) != 0)
			{
				for (i = (int) (0); (i) < (count); ++i)
				{
					nk_chart_push(ctx, (float) (value_getter(userdata, (int) (i + offset))));
				}
				nk_chart_end(ctx);
			}

		}

		public static int nk_group_scrolled_offset_begin(nk_context ctx, nk_scroll offset, char* title, uint flags)
		{
			RectangleF bounds = new RectangleF();
			nk_window panel = new nk_window();
			nk_window win;
			win = ctx.current;
			nk_panel_alloc_space(&bounds, ctx);
			{
				if (
					(!(!(((((bounds.X) > (win.layout.clip.X + win.layout.clip.Width)) || ((bounds.X + bounds.Width) < (win.layout.clip.X))) ||
					      ((bounds.Y) > (win.layout.clip.Y + win.layout.clip.Height))) || ((bounds.Y + bounds.Height) < (win.layout.clip.Y))))) &&
					((flags & NK_WINDOW_MOVABLE) == 0))
				{
					return (int) (0);
				}
			}

			if ((win.flags & NK_WINDOW_ROM) != 0) flags |= (uint) (NK_WINDOW_ROM);

			panel.bounds = (RectangleF) (bounds);
			panel.flags = (uint) (flags);
			panel.scrollbar.X = offset.X;
			panel.scrollbar.Y = offset.Y;
			panel.buffer = (nk_command_buffer) (win.buffer);
			panel.layout = (nk_panel) (nk_create_panel(ctx));
			ctx.current = panel;
			nk_panel_begin(ctx, (flags & NK_WINDOW_TITLE) != 0 ? title : null, (int) (NK_PANEL_GROUP));
			win.buffer = (nk_command_buffer) (panel.buffer);
			win.buffer.clip = (RectangleF) (panel.layout.clip);
			panel.layout.offset = offset;

			panel.layout.parent = win.layout;
			win.layout = panel.layout;
			ctx.current = win;
			if (((panel.layout.flags & NK_WINDOW_CLOSED) != 0) || ((panel.layout.flags & NK_WINDOW_MINIMIZED) != 0))
			{
				uint f = (uint) (panel.layout.flags);
				nk_group_scrolled_end(ctx);
				if ((f & NK_WINDOW_CLOSED) != 0) return (int) (NK_WINDOW_CLOSED);
				if ((f & NK_WINDOW_MINIMIZED) != 0) return (int) (NK_WINDOW_MINIMIZED);
			}

			return (int) (1);
		}

		public static void nk_group_scrolled_end(nk_context ctx)
		{
			nk_window win;
			nk_panel parent;
			nk_panel g;
			RectangleF clip = new RectangleF();
			nk_window pan = new nk_window();
			Vector2 panel_padding = new Vector2();
			if ((ctx == null) || (ctx.current == null)) return;
			win = ctx.current;
			g = win.layout;
			parent = g.parent;

			panel_padding = (Vector2) (nk_panel_get_padding(ctx.style, (int) (NK_PANEL_GROUP)));
			pan.bounds.Y = (float) (g.bounds.Y - (g.Heighteader_height + g.menu.Height));
			pan.bounds.X = (float) (g.bounds.X - panel_padding.X);
			pan.bounds.Width = (float) (g.bounds.Width + 2*panel_padding.X);
			pan.bounds.Height = (float) (g.bounds.Height + g.Heighteader_height + g.menu.Height);
			if ((g.flags & NK_WINDOW_BORDER) != 0)
			{
				pan.bounds.X -= (float) (g.border);
				pan.bounds.Y -= (float) (g.border);
				pan.bounds.Width += (float) (2*g.border);
				pan.bounds.Height += (float) (2*g.border);
			}

			if ((g.flags & NK_WINDOW_NO_SCROLLBAR) == 0)
			{
				pan.bounds.Width += (float) (ctx.style.Widthindow.scrollbar_size.X);
				pan.bounds.Height += (float) (ctx.style.Widthindow.scrollbar_size.Y);
			}

			pan.scrollbar.X = (uint) (g.offset.X);
			pan.scrollbar.Y = (uint) (g.offset.Y);
			pan.flags = (uint) (g.flags);
			pan.buffer = (nk_command_buffer) (win.buffer);
			pan.layout = g;
			pan.parent = win;
			ctx.current = pan;
			nk_unify(ref clip, ref parent.clip, (float) (pan.bounds.X), (float) (pan.bounds.Y),
				(float) (pan.bounds.X + pan.bounds.Width), (float) (pan.bounds.Y + pan.bounds.Height + panel_padding.X));
			nk_push_scissor(pan.buffer, (RectangleF) (clip));
			nk_end(ctx);
			win.buffer = (nk_command_buffer) (pan.buffer);
			nk_push_scissor(win.buffer, (RectangleF) (parent.clip));
			ctx.current = win;
			win.layout = parent;
			g.bounds = (RectangleF) (pan.bounds);
			return;
		}

		public static int nk_group_scrolled_begin(nk_context ctx, nk_scroll scroll, char* title, uint flags)
		{
			return (int) (nk_group_scrolled_offset_begin(ctx, scroll, title, (uint) (flags)));
		}

		public static int nk_group_begin_titled(nk_context ctx, char* id, char* title, uint flags)
		{
			int id_len;
			uint id_hash;
			nk_window win;
			uint* x_offset;
			uint* y_offset;
			if ((((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) || (id == null)) return (int) (0);
			win = ctx.current;
			id_len = (int) (nk_strlen(id));
			id_hash = (uint) (nk_murmur_hash(id, (int) (id_len), (uint) (NK_PANEL_GROUP)));
			x_offset = nk_find_value(win, (uint) (id_hash));
			if (x_offset == null)
			{
				x_offset = nk_add_value(ctx, win, (uint) (id_hash), (uint) (0));
				y_offset = nk_add_value(ctx, win, (uint) (id_hash + 1), (uint) (0));
				if ((x_offset == null) || (y_offset == null)) return (int) (0);
				*x_offset = (uint) (*y_offset = (uint) (0));
			}
			else y_offset = nk_find_value(win, (uint) (id_hash + 1));
			return
				(int) (nk_group_scrolled_offset_begin(ctx, new nk_scroll {x = *x_offset, y = *y_offset}, title, (uint) (flags)));
		}

		public static int nk_group_begin(nk_context ctx, char* title, uint flags)
		{
			return (int) (nk_group_begin_titled(ctx, title, title, (uint) (flags)));
		}

		public static void nk_group_end(nk_context ctx)
		{
			nk_group_scrolled_end(ctx);
		}

		public static int nk_list_view_begin(nk_context ctx, nk_list_view view, char* title, uint flags, int row_height,
			int row_count)
		{
			int title_len;
			uint title_hash;
			uint* x_offset;
			uint* y_offset;
			int result;
			nk_window win;
			nk_panel layout;
			nk_style style;
			Vector2 item_spacing = new Vector2();
			if (((ctx == null) || (view == null)) || (title == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			item_spacing = (Vector2) (style.Widthindow.spacing);
			row_height += (int) ((0) < ((int) (item_spacing.Y)) ? ((int) (item_spacing.Y)) : (0));
			title_len = (int) (nk_strlen(title));
			title_hash = (uint) (nk_murmur_hash(title, (int) (title_len), (uint) (NK_PANEL_GROUP)));
			x_offset = nk_find_value(win, (uint) (title_hash));
			if (x_offset == null)
			{
				x_offset = nk_add_value(ctx, win, (uint) (title_hash), (uint) (0));
				y_offset = nk_add_value(ctx, win, (uint) (title_hash + 1), (uint) (0));
				if ((x_offset == null) || (y_offset == null)) return (int) (0);
				*x_offset = (uint) (*y_offset = (uint) (0));
			}
			else y_offset = nk_find_value(win, (uint) (title_hash + 1));
			view.scroll_value = *y_offset;
			view.scroll_pointer = y_offset;
			*y_offset = (uint) (0);
			result =
				(int) (nk_group_scrolled_offset_begin(ctx, new nk_scroll {x = *x_offset, y = *y_offset}, title, (uint) (flags)));
			win = ctx.current;
			layout = win.layout;
			view.total_height = (int) (row_height*((row_count) < (1) ? (1) : (row_count)));
			view.begin =
				((int)
					(((float) (view.scroll_value)/(float) (row_height)) < (0.0f)
						? (0.0f)
						: ((float) (view.scroll_value)/(float) (row_height))));
			view.count =
				(int)
					((nk_iceilf((float) ((layout.clip.Height)/(float) (row_height)))) < (0)
						? (0)
						: (nk_iceilf((float) ((layout.clip.Height)/(float) (row_height)))));
			view.end = (int) (view.begin + view.count);
			view.ctx = ctx;
			return (int) (result);
		}

		public static int nk_popup_begin(nk_context ctx, int type, char* title, uint flags, RectangleF rect)
		{
			nk_window popup;
			nk_window win;
			nk_panel panel;
			int title_len;
			uint title_hash;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			panel = win.layout;
			title_len = (int) (nk_strlen(title));
			title_hash = (uint) (nk_murmur_hash(title, (int) (title_len), (uint) (NK_PANEL_POPUP)));
			popup = win.popup.Widthin;
			if (popup == null)
			{
				popup = (nk_window) (nk_create_window(ctx));
				popup.parent = win;
				win.popup.Widthin = popup;
				win.popup.active = (int) (0);
				win.popup.type = (int) (NK_PANEL_POPUP);
			}

			if (win.popup.name != title_hash)
			{
				if (win.popup.active == 0)
				{
					win.popup.name = (uint) (title_hash);
					win.popup.active = (int) (1);
					win.popup.type = (int) (NK_PANEL_POPUP);
				}
				else return (int) (0);
			}

			ctx.current = popup;
			rect.X += (float) (win.layout.clip.X);
			rect.Y += (float) (win.layout.clip.Y);
			popup.parent = win;
			popup.bounds = (RectangleF) (rect);
			popup.seq = (uint) (ctx.seq);
			popup.layout = (nk_panel) (nk_create_panel(ctx));
			popup.flags = (uint) (flags);
			popup.flags |= (uint) (NK_WINDOW_BORDER);
			if ((type) == (NK_POPUP_DYNAMIC)) popup.flags |= (uint) (NK_WINDOW_DYNAMIC);
			nk_start_popup(ctx, win);
			popup.buffer = (nk_command_buffer) (win.buffer);
			nk_push_scissor(popup.buffer, (RectangleF) (nk_null_rect));
			if ((nk_panel_begin(ctx, title, (int) (NK_PANEL_POPUP))) != 0)
			{
				nk_panel root;
				root = win.layout;
				while ((root) != null)
				{
					root.flags |= (uint) (NK_WINDOW_ROM);
					root.flags &= (uint) (~(uint) (NK_WINDOW_REMOVE_ROM));
					root = root.parent;
				}
				win.popup.active = (int) (1);
				popup.layout.offset = popup.scrollbar;
				popup.layout.parent = win.layout;
				return (int) (1);
			}
			else
			{
				nk_panel root;
				root = win.layout;
				while ((root) != null)
				{
					root.flags |= (uint) (NK_WINDOW_REMOVE_ROM);
					root = root.parent;
				}
				win.popup.active = (int) (0);
				ctx.current = win;
				nk_free_panel(ctx, popup.layout);
				popup.layout = null;
				return (int) (0);
			}

		}

		public static int nk_nonblock_begin(nk_context ctx, uint flags, RectangleF body, RectangleF header, int panel_type)
		{
			nk_window popup;
			nk_window win;
			nk_panel panel;
			int is_active = (int) (nk_true);
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			panel = win.layout;
			popup = win.popup.Widthin;
			if (popup == null)
			{
				popup = (nk_window) (nk_create_window(ctx));
				popup.parent = win;
				win.popup.Widthin = popup;
				win.popup.type = (int) (panel_type);
				nk_command_buffer_init(popup.buffer, (int) (NK_CLIPPING_ON));
			}
			else
			{
				int pressed;
				int in_body;
				int in_header;
				pressed = (int) (nk_input_is_mouse_pressed(ctx.input, (int) (NK_BUTTON_LEFT)));
				in_body = (int) (nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (body)));
				in_header = (int) (nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (header)));
				if (((pressed) != 0) && ((in_body == 0) || ((in_header) != 0))) is_active = (int) (nk_false);
			}

			win.popup.Heighteader = (RectangleF) (header);
			if (is_active == 0)
			{
				nk_panel root = win.layout;
				while ((root) != null)
				{
					root.flags |= (uint) (NK_WINDOW_REMOVE_ROM);
					root = root.parent;
				}
				return (int) (is_active);
			}

			popup.bounds = (RectangleF) (body);
			popup.parent = win;
			popup.layout = (nk_panel) (nk_create_panel(ctx));
			popup.flags = (uint) (flags);
			popup.flags |= (uint) (NK_WINDOW_BORDER);
			popup.flags |= (uint) (NK_WINDOW_DYNAMIC);
			popup.seq = (uint) (ctx.seq);
			win.popup.active = (int) (1);
			nk_start_popup(ctx, win);
			popup.buffer = (nk_command_buffer) (win.buffer);
			nk_push_scissor(popup.buffer, (RectangleF) (nk_null_rect));
			ctx.current = popup;
			nk_panel_begin(ctx, null, (int) (panel_type));
			win.buffer = (nk_command_buffer) (popup.buffer);
			popup.layout.parent = win.layout;
			popup.layout.offset = popup.scrollbar;

			{
				nk_panel root;
				root = win.layout;
				while ((root) != null)
				{
					root.flags |= (uint) (NK_WINDOW_ROM);
					root = root.parent;
				}
			}

			return (int) (is_active);
		}

		public static void nk_popup_close(nk_context ctx)
		{
			nk_window popup;
			if ((ctx == null) || (ctx.current == null)) return;
			popup = ctx.current;
			popup.flags |= (uint) (NK_WINDOW_HIDDEN);
		}

		public static void nk_popup_end(nk_context ctx)
		{
			nk_window win;
			nk_window popup;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			popup = ctx.current;
			if (popup.parent == null) return;
			win = popup.parent;
			if ((popup.flags & NK_WINDOW_HIDDEN) != 0)
			{
				nk_panel root;
				root = win.layout;
				while ((root) != null)
				{
					root.flags |= (uint) (NK_WINDOW_REMOVE_ROM);
					root = root.parent;
				}
				win.popup.active = (int) (0);
			}

			nk_push_scissor(popup.buffer, (RectangleF) (nk_null_rect));
			nk_end(ctx);
			win.buffer = (nk_command_buffer) (popup.buffer);
			nk_finish_popup(ctx, win);
			ctx.current = win;
			nk_push_scissor(win.buffer, (RectangleF) (win.layout.clip));
		}

		public static int nk_tooltip_begin(nk_context ctx, float width)
		{
			int x;
			int y;
			int w;
			int h;
			nk_window win;
			nk_input _in_;
			RectangleF bounds = new RectangleF();
			int ret;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			_in_ = ctx.input;
			if (((win.popup.Widthin) != null) && ((win.popup.type & NK_PANEL_SET_NONBLOCK) != 0)) return (int) (0);
			w = (int) (nk_iceilf((float) (width)));
			h = (int) (nk_iceilf((float) (nk_null_rect.Height)));
			x = (int) (nk_ifloorf((float) (_in_.mouse.pos.X + 1)) - (int) (win.layout.clip.X));
			y = (int) (nk_ifloorf((float) (_in_.mouse.pos.Y + 1)) - (int) (win.layout.clip.Y));
			bounds.X = ((float) (x));
			bounds.Y = ((float) (y));
			bounds.Width = ((float) (w));
			bounds.Height = ((float) (h));
			ret =
				(int)
					(nk_popup_begin(ctx, (int) (NK_POPUP_DYNAMIC), "__##Tooltip##__",
						(uint) (NK_WINDOW_NO_SCROLLBAR | NK_WINDOW_BORDER), (RectangleF) (bounds)));
			if ((ret) != 0) win.layout.flags &= (uint) (~(uint) (NK_WINDOW_ROM));
			win.popup.type = (int) (NK_PANEL_TOOLTIP);
			ctx.current.layout.type = (int) (NK_PANEL_TOOLTIP);
			return (int) (ret);
		}

		public static void nk_tooltip_end(nk_context ctx)
		{
			if ((ctx == null) || (ctx.current == null)) return;
			ctx.current.seq--;
			nk_popup_close(ctx);
			nk_popup_end(ctx);
		}

		public static void nk_tooltip(nk_context ctx, char* text)
		{
			nk_style style;
			Vector2 padding = new Vector2();
			int text_len;
			float text_width;
			float text_height;
			if ((((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) || (text == null)) return;
			style = ctx.style;
			padding = (Vector2) (style.Widthindow.padding);
			text_len = (int) (nk_strlen(text));
			text_width =
				(float) (style.font.Widthidth((nk_handle) (style.font.userdata), (float) (style.font.Height), text, (int) (text_len)));
			text_width += (float) (4*padding.X);
			text_height = (float) (style.font.Height + 2*padding.Y);
			if ((nk_tooltip_begin(ctx, (float) (text_width))) != 0)
			{
				nk_layout_row_dynamic(ctx, (float) (text_height), (int) (1));
				nk_text_(ctx, text, (int) (text_len), (uint) (NK_TEXT_LEFT));
				nk_tooltip_end(ctx);
			}

		}

		public static int nk_contextual_begin(nk_context ctx, uint flags, Vector2 size, RectangleF trigger_bounds)
		{
			nk_window win;
			nk_window popup;
			RectangleF body = new RectangleF();
			RectangleF null_rect = new RectangleF();
			int is_clicked = (int) (0);
			int is_active = (int) (0);
			int is_open = (int) (0);
			int ret = (int) (0);
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			++win.popup.con_count;
			popup = win.popup.Widthin;
			is_open = (int) (((popup) != null) && ((win.popup.type) == (NK_PANEL_CONTEXTUAL)) ? 1 : 0);
			is_clicked = (int) (nk_input_mouse_clicked(ctx.input, (int) (NK_BUTTON_RIGHT), (RectangleF) (trigger_bounds)));
			if (((win.popup.active_con) != 0) && (win.popup.con_count != win.popup.active_con)) return (int) (0);
			if (((((is_clicked) != 0) && ((is_open) != 0)) && (is_active == 0)) ||
			    (((is_open == 0) && (is_active == 0)) && (is_clicked == 0))) return (int) (0);
			win.popup.active_con = (uint) (win.popup.con_count);
			if ((is_clicked) != 0)
			{
				body.X = (float) (ctx.input.mouse.pos.X);
				body.Y = (float) (ctx.input.mouse.pos.Y);
			}
			else
			{
				body.X = (float) (popup.bounds.X);
				body.Y = (float) (popup.bounds.Y);
			}

			body.Width = (float) (size.X);
			body.Height = (float) (size.Y);
			ret =
				(int)
					(nk_nonblock_begin(ctx, (uint) (flags | NK_WINDOW_NO_SCROLLBAR), (RectangleF) (body), (RectangleF) (null_rect),
						(int) (NK_PANEL_CONTEXTUAL)));
			if ((ret) != 0) win.popup.type = (int) (NK_PANEL_CONTEXTUAL);
			else
			{
				win.popup.active_con = (uint) (0);
				if ((win.popup.Widthin) != null) win.popup.Widthin.flags = (uint) (0);
			}

			return (int) (ret);
		}

		public static int nk_contextual_item_text(nk_context ctx, char* text, int len, uint alignment)
		{
			nk_window win;
			nk_input _in_;
			nk_style style;
			RectangleF bounds = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			state = (int) (nk_widget_fitting(&bounds, ctx, (Vector2) (style.contextual_button.padding)));
			if (state == 0) return (int) (nk_false);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((win.layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			if (
				(nk_do_button_text(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), text, (int) (len), (uint) (alignment),
					(int) (NK_BUTTON_DEFAULT), style.contextual_button, _in_, style.font)) != 0)
			{
				nk_contextual_close(ctx);
				return (int) (nk_true);
			}

			return (int) (nk_false);
		}

		public static int nk_contextual_item_label(nk_context ctx, char* label, uint align)
		{
			return (int) (nk_contextual_item_text(ctx, label, (int) (nk_strlen(label)), (uint) (align)));
		}

		public static int nk_contextual_item_image_text(nk_context ctx, nk_image img, char* text, int len, uint align)
		{
			nk_window win;
			nk_input _in_;
			nk_style style;
			RectangleF bounds = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			state = (int) (nk_widget_fitting(&bounds, ctx, (Vector2) (style.contextual_button.padding)));
			if (state == 0) return (int) (nk_false);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((win.layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			if (
				(nk_do_button_text_image(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), (nk_image) (img), text,
					(int) (len), (uint) (align), (int) (NK_BUTTON_DEFAULT), style.contextual_button, style.font, _in_)) != 0)
			{
				nk_contextual_close(ctx);
				return (int) (nk_true);
			}

			return (int) (nk_false);
		}

		public static int nk_contextual_item_image_label(nk_context ctx, nk_image img, char* label, uint align)
		{
			return (int) (nk_contextual_item_image_text(ctx, (nk_image) (img), label, (int) (nk_strlen(label)), (uint) (align)));
		}

		public static int nk_contextual_item_symbol_text(nk_context ctx, int symbol, char* text, int len, uint align)
		{
			nk_window win;
			nk_input _in_;
			nk_style style;
			RectangleF bounds = new RectangleF();
			int state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			state = (int) (nk_widget_fitting(&bounds, ctx, (Vector2) (style.contextual_button.padding)));
			if (state == 0) return (int) (nk_false);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((win.layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			if (
				(nk_do_button_text_symbol(ref ctx.last_widget_state, win.buffer, (RectangleF) (bounds), (int) (symbol), text,
					(int) (len), (uint) (align), (int) (NK_BUTTON_DEFAULT), style.contextual_button, style.font, _in_)) != 0)
			{
				nk_contextual_close(ctx);
				return (int) (nk_true);
			}

			return (int) (nk_false);
		}

		public static int nk_contextual_item_symbol_label(nk_context ctx, int symbol, char* text, uint align)
		{
			return (int) (nk_contextual_item_symbol_text(ctx, (int) (symbol), text, (int) (nk_strlen(text)), (uint) (align)));
		}

		public static void nk_contextual_close(nk_context ctx)
		{
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			nk_popup_close(ctx);
		}

		public static void nk_contextual_end(nk_context ctx)
		{
			nk_window popup;
			nk_panel panel;
			if ((ctx == null) || (ctx.current == null)) return;
			popup = ctx.current;
			panel = popup.layout;
			if ((panel.flags & NK_WINDOW_DYNAMIC) != 0)
			{
				RectangleF body = new RectangleF();
				if ((panel.at_y) < (panel.bounds.Y + panel.bounds.Height))
				{
					Vector2 padding = (Vector2) (nk_panel_get_padding(ctx.style, (int) (panel.type)));
					body = (RectangleF) (panel.bounds);
					body.Y = (float) (panel.at_y + panel.footer_height + panel.border + padding.Y + panel.row.Height);
					body.Height = (float) ((panel.bounds.Y + panel.bounds.Height) - body.Y);
				}
				{
					int pressed = (int) (nk_input_is_mouse_pressed(ctx.input, (int) (NK_BUTTON_LEFT)));
					int in_body = (int) (nk_input_is_mouse_hovering_rect(ctx.input, (RectangleF) (body)));
					if (((pressed) != 0) && ((in_body) != 0)) popup.flags |= (uint) (NK_WINDOW_HIDDEN);
				}
			}

			if ((popup.flags & NK_WINDOW_HIDDEN) != 0) popup.seq = (uint) (0);
			nk_popup_end(ctx);
			return;
		}

		public static int nk_combo_begin(nk_context ctx, nk_window win, Vector2 size, int is_clicked, RectangleF header)
		{
			nk_window popup;
			int is_open = (int) (0);
			int is_active = (int) (0);
			RectangleF body = new RectangleF();
			uint hash;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			popup = win.popup.Widthin;
			body.X = (float) (header.X);
			body.Width = (float) (size.X);
			body.Y = (float) (header.Y + header.Height - ctx.style.Widthindow.combo_border);
			body.Height = (float) (size.Y);
			hash = (uint) (win.popup.combo_count++);
			is_open = (int) ((popup != null) ? nk_true : nk_false);
			is_active =
				(int) ((((popup) != null) && ((win.popup.name) == (hash))) && ((win.popup.type) == (NK_PANEL_COMBO)) ? 1 : 0);
			if ((((((is_clicked) != 0) && ((is_open) != 0)) && (is_active == 0)) || (((is_open) != 0) && (is_active == 0))) ||
			    (((is_open == 0) && (is_active == 0)) && (is_clicked == 0))) return (int) (0);
			if (
				nk_nonblock_begin(ctx, (uint) (0), (RectangleF) (body),
					(RectangleF)
						((((is_clicked) != 0) && ((is_open) != 0)) ? RectangleF_((float) (0), (float) (0), (float) (0), (float) (0)) : header),
					(int) (NK_PANEL_COMBO)) == 0) return (int) (0);
			win.popup.type = (int) (NK_PANEL_COMBO);
			win.popup.name = (uint) (hash);
			return (int) (1);
		}

		public static int nk_combo_begin_text(nk_context ctx, char* selected, int len, Vector2 size)
		{
			nk_input _in_;
			nk_window win;
			nk_style style;
			int s;
			int is_clicked = (int) (nk_false);
			RectangleF header = new RectangleF();
			nk_style_item background;
			nk_text text = new nk_text();
			if ((((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) || (selected == null))
				return (int) (0);
			win = ctx.current;
			style = ctx.style;
			s = (int) (nk_widget(&header, ctx));
			if ((s) == (NK_WIDGET_INVALID)) return (int) (0);
			_in_ = (((win.layout.flags & NK_WINDOW_ROM) != 0) || ((s) == (NK_WIDGET_ROM))) ? null : ctx.input;
			if ((nk_button_behavior(ref ctx.last_widget_state, (RectangleF) (header), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
				is_clicked = (int) (nk_true);
			if ((ctx.last_widget_state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.combo.active;
				text.text = (Color) (style.combo.label_active);
			}
			else if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.combo.hover;
				text.text = (Color) (style.combo.label_hover);
			}
			else
			{
				background = style.combo.normal;
				text.text = (Color) (style.combo.label_normal);
			}

			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				text.background = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
				nk_draw_image(win.buffer, (RectangleF) (header), background.data.image, (Color) (nk_white));
			}
			else
			{
				text.background = (Color) (background.data.color);
				nk_fill_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (Color) (background.data.color));
				nk_stroke_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (float) (style.combo.border),
					(Color) (style.combo.border_color));
			}

			{
				RectangleF label = new RectangleF();
				RectangleF button = new RectangleF();
				RectangleF content = new RectangleF();
				int sym;
				if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0) sym = (int) (style.combo.sym_hover);
				else if ((is_clicked) != 0) sym = (int) (style.combo.sym_active);
				else sym = (int) (style.combo.sym_normal);
				button.Width = (float) (header.Height - 2*style.combo.button_padding.Y);
				button.X = (float) ((header.X + header.Width - header.Height) - style.combo.button_padding.X);
				button.Y = (float) (header.Y + style.combo.button_padding.Y);
				button.Height = (float) (button.Width);
				content.X = (float) (button.X + style.combo.button.padding.X);
				content.Y = (float) (button.Y + style.combo.button.padding.Y);
				content.Width = (float) (button.Width - 2*style.combo.button.padding.X);
				content.Height = (float) (button.Height - 2*style.combo.button.padding.Y);
				text.padding = (Vector2) (new Vector2((float) (0), (float) (0)));
				label.X = (float) (header.X + style.combo.content_padding.X);
				label.Y = (float) (header.Y + style.combo.content_padding.Y);
				label.Width = (float) (button.X - (style.combo.content_padding.X + style.combo.spacing.X) - label.X);
				label.Height = (float) (header.Height - 2*style.combo.content_padding.Y);
				nk_widget_text(win.buffer, (RectangleF) (label), selected, (int) (len), &text, (uint) (NK_TEXT_LEFT), ctx.style.font);
				nk_draw_button_symbol(win.buffer, &button, &content, (uint) (ctx.last_widget_state), ctx.style.combo.button,
					(int) (sym), style.font);
			}

			return (int) (nk_combo_begin(ctx, win, (Vector2) (size), (int) (is_clicked), (RectangleF) (header)));
		}

		public static int nk_combo_begin_label(nk_context ctx, char* selected, Vector2 size)
		{
			return (int) (nk_combo_begin_text(ctx, selected, (int) (nk_strlen(selected)), (Vector2) (size)));
		}

		public static int nk_combo_begin_color(nk_context ctx, Color color, Vector2 size)
		{
			nk_window win;
			nk_style style;
			nk_input _in_;
			RectangleF header = new RectangleF();
			int is_clicked = (int) (nk_false);
			int s;
			nk_style_item background;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			s = (int) (nk_widget(&header, ctx));
			if ((s) == (NK_WIDGET_INVALID)) return (int) (0);
			_in_ = (((win.layout.flags & NK_WINDOW_ROM) != 0) || ((s) == (NK_WIDGET_ROM))) ? null : ctx.input;
			if ((nk_button_behavior(ref ctx.last_widget_state, (RectangleF) (header), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
				is_clicked = (int) (nk_true);
			if ((ctx.last_widget_state & NK_WIDGET_STATE_ACTIVED) != 0) background = style.combo.active;
			else if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0) background = style.combo.hover;
			else background = style.combo.normal;
			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				nk_draw_image(win.buffer, (RectangleF) (header), background.data.image, (Color) (nk_white));
			}
			else
			{
				nk_fill_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (Color) (background.data.color));
				nk_stroke_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (float) (style.combo.border),
					(Color) (style.combo.border_color));
			}

			{
				RectangleF content = new RectangleF();
				RectangleF button = new RectangleF();
				RectangleF bounds = new RectangleF();
				int sym;
				if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0) sym = (int) (style.combo.sym_hover);
				else if ((is_clicked) != 0) sym = (int) (style.combo.sym_active);
				else sym = (int) (style.combo.sym_normal);
				button.Width = (float) (header.Height - 2*style.combo.button_padding.Y);
				button.X = (float) ((header.X + header.Width - header.Height) - style.combo.button_padding.X);
				button.Y = (float) (header.Y + style.combo.button_padding.Y);
				button.Height = (float) (button.Width);
				content.X = (float) (button.X + style.combo.button.padding.X);
				content.Y = (float) (button.Y + style.combo.button.padding.Y);
				content.Width = (float) (button.Width - 2*style.combo.button.padding.X);
				content.Height = (float) (button.Height - 2*style.combo.button.padding.Y);
				bounds.Height = (float) (header.Height - 4*style.combo.content_padding.Y);
				bounds.Y = (float) (header.Y + 2*style.combo.content_padding.Y);
				bounds.X = (float) (header.X + 2*style.combo.content_padding.X);
				bounds.Width = (float) ((button.X - (style.combo.content_padding.X + style.combo.spacing.X)) - bounds.X);
				nk_fill_rect(win.buffer, (RectangleF) (bounds), (float) (0), (Color) (color));
				nk_draw_button_symbol(win.buffer, &button, &content, (uint) (ctx.last_widget_state), ctx.style.combo.button,
					(int) (sym), style.font);
			}

			return (int) (nk_combo_begin(ctx, win, (Vector2) (size), (int) (is_clicked), (RectangleF) (header)));
		}

		public static int nk_combo_begin_symbol(nk_context ctx, int symbol, Vector2 size)
		{
			nk_window win;
			nk_style style;
			nk_input _in_;
			RectangleF header = new RectangleF();
			int is_clicked = (int) (nk_false);
			int s;
			nk_style_item background;
			Color sym_background = new Color();
			Color symbol_color = new Color();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			s = (int) (nk_widget(&header, ctx));
			if ((s) == (NK_WIDGET_INVALID)) return (int) (0);
			_in_ = (((win.layout.flags & NK_WINDOW_ROM) != 0) || ((s) == (NK_WIDGET_ROM))) ? null : ctx.input;
			if ((nk_button_behavior(ref ctx.last_widget_state, (RectangleF) (header), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
				is_clicked = (int) (nk_true);
			if ((ctx.last_widget_state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.combo.active;
				symbol_color = (Color) (style.combo.symbol_active);
			}
			else if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.combo.hover;
				symbol_color = (Color) (style.combo.symbol_hover);
			}
			else
			{
				background = style.combo.normal;
				symbol_color = (Color) (style.combo.symbol_hover);
			}

			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				sym_background = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
				nk_draw_image(win.buffer, (RectangleF) (header), background.data.image, (Color) (nk_white));
			}
			else
			{
				sym_background = (Color) (background.data.color);
				nk_fill_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (Color) (background.data.color));
				nk_stroke_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (float) (style.combo.border),
					(Color) (style.combo.border_color));
			}

			{
				RectangleF bounds = new RectangleF();
				RectangleF content = new RectangleF();
				RectangleF button = new RectangleF();
				int sym;
				if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0) sym = (int) (style.combo.sym_hover);
				else if ((is_clicked) != 0) sym = (int) (style.combo.sym_active);
				else sym = (int) (style.combo.sym_normal);
				button.Width = (float) (header.Height - 2*style.combo.button_padding.Y);
				button.X = (float) ((header.X + header.Width - header.Height) - style.combo.button_padding.Y);
				button.Y = (float) (header.Y + style.combo.button_padding.Y);
				button.Height = (float) (button.Width);
				content.X = (float) (button.X + style.combo.button.padding.X);
				content.Y = (float) (button.Y + style.combo.button.padding.Y);
				content.Width = (float) (button.Width - 2*style.combo.button.padding.X);
				content.Height = (float) (button.Height - 2*style.combo.button.padding.Y);
				bounds.Height = (float) (header.Height - 2*style.combo.content_padding.Y);
				bounds.Y = (float) (header.Y + style.combo.content_padding.Y);
				bounds.X = (float) (header.X + style.combo.content_padding.X);
				bounds.Width = (float) ((button.X - style.combo.content_padding.Y) - bounds.X);
				nk_draw_symbol(win.buffer, (int) (symbol), (RectangleF) (bounds), (Color) (sym_background),
					(Color) (symbol_color), (float) (1.0f), style.font);
				nk_draw_button_symbol(win.buffer, &bounds, &content, (uint) (ctx.last_widget_state), ctx.style.combo.button,
					(int) (sym), style.font);
			}

			return (int) (nk_combo_begin(ctx, win, (Vector2) (size), (int) (is_clicked), (RectangleF) (header)));
		}

		public static int nk_combo_begin_symbol_text(nk_context ctx, char* selected, int len, int symbol, Vector2 size)
		{
			nk_window win;
			nk_style style;
			nk_input _in_;
			RectangleF header = new RectangleF();
			int is_clicked = (int) (nk_false);
			int s;
			nk_style_item background;
			Color symbol_color = new Color();
			nk_text text = new nk_text();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			s = (int) (nk_widget(&header, ctx));
			if (s == 0) return (int) (0);
			_in_ = (((win.layout.flags & NK_WINDOW_ROM) != 0) || ((s) == (NK_WIDGET_ROM))) ? null : ctx.input;
			if ((nk_button_behavior(ref ctx.last_widget_state, (RectangleF) (header), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
				is_clicked = (int) (nk_true);
			if ((ctx.last_widget_state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.combo.active;
				symbol_color = (Color) (style.combo.symbol_active);
				text.text = (Color) (style.combo.label_active);
			}
			else if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.combo.hover;
				symbol_color = (Color) (style.combo.symbol_hover);
				text.text = (Color) (style.combo.label_hover);
			}
			else
			{
				background = style.combo.normal;
				symbol_color = (Color) (style.combo.symbol_normal);
				text.text = (Color) (style.combo.label_normal);
			}

			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				text.background = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
				nk_draw_image(win.buffer, (RectangleF) (header), background.data.image, (Color) (nk_white));
			}
			else
			{
				text.background = (Color) (background.data.color);
				nk_fill_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (Color) (background.data.color));
				nk_stroke_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (float) (style.combo.border),
					(Color) (style.combo.border_color));
			}

			{
				RectangleF content = new RectangleF();
				RectangleF button = new RectangleF();
				RectangleF label = new RectangleF();
				RectangleF image = new RectangleF();
				int sym;
				if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0) sym = (int) (style.combo.sym_hover);
				else if ((is_clicked) != 0) sym = (int) (style.combo.sym_active);
				else sym = (int) (style.combo.sym_normal);
				button.Width = (float) (header.Height - 2*style.combo.button_padding.Y);
				button.X = (float) ((header.X + header.Width - header.Height) - style.combo.button_padding.X);
				button.Y = (float) (header.Y + style.combo.button_padding.Y);
				button.Height = (float) (button.Width);
				content.X = (float) (button.X + style.combo.button.padding.X);
				content.Y = (float) (button.Y + style.combo.button.padding.Y);
				content.Width = (float) (button.Width - 2*style.combo.button.padding.X);
				content.Height = (float) (button.Height - 2*style.combo.button.padding.Y);
				nk_draw_button_symbol(win.buffer, &button, &content, (uint) (ctx.last_widget_state), ctx.style.combo.button,
					(int) (sym), style.font);
				image.X = (float) (header.X + style.combo.content_padding.X);
				image.Y = (float) (header.Y + style.combo.content_padding.Y);
				image.Height = (float) (header.Height - 2*style.combo.content_padding.Y);
				image.Width = (float) (image.Height);
				nk_draw_symbol(win.buffer, (int) (symbol), (RectangleF) (image), (Color) (text.background),
					(Color) (symbol_color), (float) (1.0f), style.font);
				text.padding = (Vector2) (new Vector2((float) (0), (float) (0)));
				label.X = (float) (image.X + image.Width + style.combo.spacing.X + style.combo.content_padding.X);
				label.Y = (float) (header.Y + style.combo.content_padding.Y);
				label.Width = (float) ((button.X - style.combo.content_padding.X) - label.X);
				label.Height = (float) (header.Height - 2*style.combo.content_padding.Y);
				nk_widget_text(win.buffer, (RectangleF) (label), selected, (int) (len), &text, (uint) (NK_TEXT_LEFT), style.font);
			}

			return (int) (nk_combo_begin(ctx, win, (Vector2) (size), (int) (is_clicked), (RectangleF) (header)));
		}

		public static int nk_combo_begin_image(nk_context ctx, nk_image img, Vector2 size)
		{
			nk_window win;
			nk_style style;
			nk_input _in_;
			RectangleF header = new RectangleF();
			int is_clicked = (int) (nk_false);
			int s;
			nk_style_item background;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			s = (int) (nk_widget(&header, ctx));
			if ((s) == (NK_WIDGET_INVALID)) return (int) (0);
			_in_ = (((win.layout.flags & NK_WINDOW_ROM) != 0) || ((s) == (NK_WIDGET_ROM))) ? null : ctx.input;
			if ((nk_button_behavior(ref ctx.last_widget_state, (RectangleF) (header), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
				is_clicked = (int) (nk_true);
			if ((ctx.last_widget_state & NK_WIDGET_STATE_ACTIVED) != 0) background = style.combo.active;
			else if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0) background = style.combo.hover;
			else background = style.combo.normal;
			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				nk_draw_image(win.buffer, (RectangleF) (header), background.data.image, (Color) (nk_white));
			}
			else
			{
				nk_fill_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (Color) (background.data.color));
				nk_stroke_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (float) (style.combo.border),
					(Color) (style.combo.border_color));
			}

			{
				RectangleF bounds = new RectangleF();
				RectangleF content = new RectangleF();
				RectangleF button = new RectangleF();
				int sym;
				if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0) sym = (int) (style.combo.sym_hover);
				else if ((is_clicked) != 0) sym = (int) (style.combo.sym_active);
				else sym = (int) (style.combo.sym_normal);
				button.Width = (float) (header.Height - 2*style.combo.button_padding.Y);
				button.X = (float) ((header.X + header.Width - header.Height) - style.combo.button_padding.Y);
				button.Y = (float) (header.Y + style.combo.button_padding.Y);
				button.Height = (float) (button.Width);
				content.X = (float) (button.X + style.combo.button.padding.X);
				content.Y = (float) (button.Y + style.combo.button.padding.Y);
				content.Width = (float) (button.Width - 2*style.combo.button.padding.X);
				content.Height = (float) (button.Height - 2*style.combo.button.padding.Y);
				bounds.Height = (float) (header.Height - 2*style.combo.content_padding.Y);
				bounds.Y = (float) (header.Y + style.combo.content_padding.Y);
				bounds.X = (float) (header.X + style.combo.content_padding.X);
				bounds.Width = (float) ((button.X - style.combo.content_padding.Y) - bounds.X);
				nk_draw_image(win.buffer, (RectangleF) (bounds), img, (Color) (nk_white));
				nk_draw_button_symbol(win.buffer, &bounds, &content, (uint) (ctx.last_widget_state), ctx.style.combo.button,
					(int) (sym), style.font);
			}

			return (int) (nk_combo_begin(ctx, win, (Vector2) (size), (int) (is_clicked), (RectangleF) (header)));
		}

		public static int nk_combo_begin_image_text(nk_context ctx, char* selected, int len, nk_image img, Vector2 size)
		{
			nk_window win;
			nk_style style;
			nk_input _in_;
			RectangleF header = new RectangleF();
			int is_clicked = (int) (nk_false);
			int s;
			nk_style_item background;
			nk_text text = new nk_text();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			style = ctx.style;
			s = (int) (nk_widget(&header, ctx));
			if (s == 0) return (int) (0);
			_in_ = (((win.layout.flags & NK_WINDOW_ROM) != 0) || ((s) == (NK_WIDGET_ROM))) ? null : ctx.input;
			if ((nk_button_behavior(ref ctx.last_widget_state, (RectangleF) (header), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
				is_clicked = (int) (nk_true);
			if ((ctx.last_widget_state & NK_WIDGET_STATE_ACTIVED) != 0)
			{
				background = style.combo.active;
				text.text = (Color) (style.combo.label_active);
			}
			else if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0)
			{
				background = style.combo.hover;
				text.text = (Color) (style.combo.label_hover);
			}
			else
			{
				background = style.combo.normal;
				text.text = (Color) (style.combo.label_normal);
			}

			if ((background.type) == (NK_STYLE_ITEM_IMAGE))
			{
				text.background = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
				nk_draw_image(win.buffer, (RectangleF) (header), background.data.image, (Color) (nk_white));
			}
			else
			{
				text.background = (Color) (background.data.color);
				nk_fill_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (Color) (background.data.color));
				nk_stroke_rect(win.buffer, (RectangleF) (header), (float) (style.combo.rounding), (float) (style.combo.border),
					(Color) (style.combo.border_color));
			}

			{
				RectangleF content = new RectangleF();
				RectangleF button = new RectangleF();
				RectangleF label = new RectangleF();
				RectangleF image = new RectangleF();
				int sym;
				if ((ctx.last_widget_state & NK_WIDGET_STATE_HOVER) != 0) sym = (int) (style.combo.sym_hover);
				else if ((is_clicked) != 0) sym = (int) (style.combo.sym_active);
				else sym = (int) (style.combo.sym_normal);
				button.Width = (float) (header.Height - 2*style.combo.button_padding.Y);
				button.X = (float) ((header.X + header.Width - header.Height) - style.combo.button_padding.X);
				button.Y = (float) (header.Y + style.combo.button_padding.Y);
				button.Height = (float) (button.Width);
				content.X = (float) (button.X + style.combo.button.padding.X);
				content.Y = (float) (button.Y + style.combo.button.padding.Y);
				content.Width = (float) (button.Width - 2*style.combo.button.padding.X);
				content.Height = (float) (button.Height - 2*style.combo.button.padding.Y);
				nk_draw_button_symbol(win.buffer, &button, &content, (uint) (ctx.last_widget_state), ctx.style.combo.button,
					(int) (sym), style.font);
				image.X = (float) (header.X + style.combo.content_padding.X);
				image.Y = (float) (header.Y + style.combo.content_padding.Y);
				image.Height = (float) (header.Height - 2*style.combo.content_padding.Y);
				image.Width = (float) (image.Height);
				nk_draw_image(win.buffer, (RectangleF) (image), img, (Color) (nk_white));
				text.padding = (Vector2) (new Vector2((float) (0), (float) (0)));
				label.X = (float) (image.X + image.Width + style.combo.spacing.X + style.combo.content_padding.X);
				label.Y = (float) (header.Y + style.combo.content_padding.Y);
				label.Width = (float) ((button.X - style.combo.content_padding.X) - label.X);
				label.Height = (float) (header.Height - 2*style.combo.content_padding.Y);
				nk_widget_text(win.buffer, (RectangleF) (label), selected, (int) (len), &text, (uint) (NK_TEXT_LEFT), style.font);
			}

			return (int) (nk_combo_begin(ctx, win, (Vector2) (size), (int) (is_clicked), (RectangleF) (header)));
		}

		public static int nk_combo_begin_symbol_label(nk_context ctx, char* selected, int type, Vector2 size)
		{
			return (int) (nk_combo_begin_symbol_text(ctx, selected, (int) (nk_strlen(selected)), (int) (type), (Vector2) (size)));
		}

		public static int nk_combo_begin_image_label(nk_context ctx, char* selected, nk_image img, Vector2 size)
		{
			return
				(int) (nk_combo_begin_image_text(ctx, selected, (int) (nk_strlen(selected)), (nk_image) (img), (Vector2) (size)));
		}

		public static int nk_combo_item_text(nk_context ctx, char* text, int len, uint align)
		{
			return (int) (nk_contextual_item_text(ctx, text, (int) (len), (uint) (align)));
		}

		public static int nk_combo_item_label(nk_context ctx, char* label, uint align)
		{
			return (int) (nk_contextual_item_label(ctx, label, (uint) (align)));
		}

		public static int nk_combo_item_image_text(nk_context ctx, nk_image img, char* text, int len, uint alignment)
		{
			return (int) (nk_contextual_item_image_text(ctx, (nk_image) (img), text, (int) (len), (uint) (alignment)));
		}

		public static int nk_combo_item_image_label(nk_context ctx, nk_image img, char* text, uint alignment)
		{
			return (int) (nk_contextual_item_image_label(ctx, (nk_image) (img), text, (uint) (alignment)));
		}

		public static int nk_combo_item_symbol_text(nk_context ctx, int sym, char* text, int len, uint alignment)
		{
			return (int) (nk_contextual_item_symbol_text(ctx, (int) (sym), text, (int) (len), (uint) (alignment)));
		}

		public static int nk_combo_item_symbol_label(nk_context ctx, int sym, char* label, uint alignment)
		{
			return (int) (nk_contextual_item_symbol_label(ctx, (int) (sym), label, (uint) (alignment)));
		}

		public static void nk_combo_end(nk_context ctx)
		{
			nk_contextual_end(ctx);
		}

		public static void nk_combo_close(nk_context ctx)
		{
			nk_contextual_close(ctx);
		}

		public static int nk_combo(nk_context ctx, char** items, int count, int selected, int item_height, Vector2 size)
		{
			int i = (int) (0);
			int max_height;
			Vector2 item_spacing = new Vector2();
			Vector2 window_padding = new Vector2();
			if (((ctx == null) || (items == null)) || (count == 0)) return (int) (selected);
			item_spacing = (Vector2) (ctx.style.Widthindow.spacing);
			window_padding = (Vector2) (nk_panel_get_padding(ctx.style, (int) (ctx.current.layout.type)));
			max_height = (int) (count*item_height + count*(int) (item_spacing.Y));
			max_height += (int) ((int) (item_spacing.Y)*2 + (int) (window_padding.Y)*2);
			size.Y = (float) ((size.Y) < ((float) (max_height)) ? (size.Y) : ((float) (max_height)));
			if ((nk_combo_begin_label(ctx, items[selected], (Vector2) (size))) != 0)
			{
				nk_layout_row_dynamic(ctx, (float) (item_height), (int) (1));
				for (i = (int) (0); (i) < (count); ++i)
				{
					if ((nk_combo_item_label(ctx, items[i], (uint) (NK_TEXT_LEFT))) != 0) selected = (int) (i);
				}
				nk_combo_end(ctx);
			}

			return (int) (selected);
		}

		public static int nk_combo_separator(nk_context ctx, char* items_separated_by_separator, int separator, int selected,
			int count, int item_height, Vector2 size)
		{
			int i;
			int max_height;
			Vector2 item_spacing = new Vector2();
			Vector2 window_padding = new Vector2();
			char* current_item;
			char* iter;
			;
			int length = (int) (0);
			if ((ctx == null) || (items_separated_by_separator == null)) return (int) (selected);
			item_spacing = (Vector2) (ctx.style.Widthindow.spacing);
			window_padding = (Vector2) (nk_panel_get_padding(ctx.style, (int) (ctx.current.layout.type)));
			max_height = (int) (count*item_height + count*(int) (item_spacing.Y));
			max_height += (int) ((int) (item_spacing.Y)*2 + (int) (window_padding.Y)*2);
			size.Y = (float) ((size.Y) < ((float) (max_height)) ? (size.Y) : ((float) (max_height)));
			current_item = items_separated_by_separator;
			for (i = (int) (0); (i) < (count); ++i)
			{
				iter = current_item;
				while (((*iter) != 0) && (*iter != separator))
				{
					iter++;
				}
				length = ((int) (iter - current_item));
				if ((i) == (selected)) break;
				current_item = iter + 1;
			}
			if ((nk_combo_begin_text(ctx, current_item, (int) (length), (Vector2) (size))) != 0)
			{
				current_item = items_separated_by_separator;
				nk_layout_row_dynamic(ctx, (float) (item_height), (int) (1));
				for (i = (int) (0); (i) < (count); ++i)
				{
					iter = current_item;
					while (((*iter) != 0) && (*iter != separator))
					{
						iter++;
					}
					length = ((int) (iter - current_item));
					if ((nk_combo_item_text(ctx, current_item, (int) (length), (uint) (NK_TEXT_LEFT))) != 0) selected = (int) (i);
					current_item = current_item + length + 1;
				}
				nk_combo_end(ctx);
			}

			return (int) (selected);
		}

		public static int nk_combo_string(nk_context ctx, char* items_separated_by_zeros, int selected, int count,
			int item_height, Vector2 size)
		{
			return
				(int)
					(nk_combo_separator(ctx, items_separated_by_zeros, (int) ('\0'), (int) (selected), (int) (count),
						(int) (item_height), (Vector2) (size)));
		}

		public static int nk_combo_callback(nk_context ctx, NkComboCallback item_getter, void* userdata, int selected,
			int count, int item_height, Vector2 size)
		{
			int i;
			int max_height;
			Vector2 item_spacing = new Vector2();
			Vector2 window_padding = new Vector2();
			char* item;
			if ((ctx == null) || (item_getter == null)) return (int) (selected);
			item_spacing = (Vector2) (ctx.style.Widthindow.spacing);
			window_padding = (Vector2) (nk_panel_get_padding(ctx.style, (int) (ctx.current.layout.type)));
			max_height = (int) (count*item_height + count*(int) (item_spacing.Y));
			max_height += (int) ((int) (item_spacing.Y)*2 + (int) (window_padding.Y)*2);
			size.Y = (float) ((size.Y) < ((float) (max_height)) ? (size.Y) : ((float) (max_height)));
			item_getter(userdata, (int) (selected), &item);
			if ((nk_combo_begin_label(ctx, item, (Vector2) (size))) != 0)
			{
				nk_layout_row_dynamic(ctx, (float) (item_height), (int) (1));
				for (i = (int) (0); (i) < (count); ++i)
				{
					item_getter(userdata, (int) (i), &item);
					if ((nk_combo_item_label(ctx, item, (uint) (NK_TEXT_LEFT))) != 0) selected = (int) (i);
				}
				nk_combo_end(ctx);
			}

			return (int) (selected);
		}

		public static void nk_combobox(nk_context ctx, char** items, int count, int* selected, int item_height, Vector2 size)
		{
			*selected = (int) (nk_combo(ctx, items, (int) (count), (int) (*selected), (int) (item_height), (Vector2) (size)));
		}

		public static void nk_combobox_string(nk_context ctx, char* items_separated_by_zeros, int* selected, int count,
			int item_height, Vector2 size)
		{
			*selected =
				(int)
					(nk_combo_string(ctx, items_separated_by_zeros, (int) (*selected), (int) (count), (int) (item_height),
						(Vector2) (size)));
		}

		public static void nk_combobox_separator(nk_context ctx, char* items_separated_by_separator, int separator,
			int* selected, int count, int item_height, Vector2 size)
		{
			*selected =
				(int)
					(nk_combo_separator(ctx, items_separated_by_separator, (int) (separator), (int) (*selected), (int) (count),
						(int) (item_height), (Vector2) (size)));
		}

		public static void nk_combobox_callback(nk_context ctx, NkComboCallback item_getter, void* userdata, int* selected,
			int count, int item_height, Vector2 size)
		{
			*selected =
				(int)
					(nk_combo_callback(ctx, item_getter, userdata, (int) (*selected), (int) (count), (int) (item_height),
						(Vector2) (size)));
		}

		public static int nk_menu_begin(nk_context ctx, nk_window win, char* id, int is_clicked, RectangleF header, Vector2 size)
		{
			int is_open = (int) (0);
			int is_active = (int) (0);
			RectangleF body = new RectangleF();
			nk_window popup;
			uint hash = (uint) (nk_murmur_hash(id, (int) (nk_strlen(id)), (uint) (NK_PANEL_MENU)));
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			body.X = (float) (header.X);
			body.Width = (float) (size.X);
			body.Y = (float) (header.Y + header.Height);
			body.Height = (float) (size.Y);
			popup = win.popup.Widthin;
			is_open = (int) (popup != null ? nk_true : nk_false);
			is_active =
				(int) ((((popup) != null) && ((win.popup.name) == (hash))) && ((win.popup.type) == (NK_PANEL_MENU)) ? 1 : 0);
			if ((((((is_clicked) != 0) && ((is_open) != 0)) && (is_active == 0)) || (((is_open) != 0) && (is_active == 0))) ||
			    (((is_open == 0) && (is_active == 0)) && (is_clicked == 0))) return (int) (0);
			if (
				nk_nonblock_begin(ctx, (uint) (NK_WINDOW_NO_SCROLLBAR), (RectangleF) (body), (RectangleF) (header), (int) (NK_PANEL_MENU)) ==
				0) return (int) (0);
			win.popup.type = (int) (NK_PANEL_MENU);
			win.popup.name = (uint) (hash);
			return (int) (1);
		}

		public static int nk_menu_begin_text(nk_context ctx, char* title, int len, uint align, Vector2 size)
		{
			nk_window win;
			nk_input _in_;
			RectangleF header = new RectangleF();
			int is_clicked = (int) (nk_false);
			uint state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			state = (uint) (nk_widget(&header, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((win.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			if (
				(nk_do_button_text(ref ctx.last_widget_state, win.buffer, (RectangleF) (header), title, (int) (len), (uint) (align),
					(int) (NK_BUTTON_DEFAULT), ctx.style.menu_button, _in_, ctx.style.font)) != 0) is_clicked = (int) (nk_true);
			return (int) (nk_menu_begin(ctx, win, title, (int) (is_clicked), (RectangleF) (header), (Vector2) (size)));
		}

		public static int nk_menu_begin_label(nk_context ctx, char* text, uint align, Vector2 size)
		{
			return (int) (nk_menu_begin_text(ctx, text, (int) (nk_strlen(text)), (uint) (align), (Vector2) (size)));
		}

		public static int nk_menu_begin_image(nk_context ctx, char* id, nk_image img, Vector2 size)
		{
			nk_window win;
			RectangleF header = new RectangleF();
			nk_input _in_;
			int is_clicked = (int) (nk_false);
			uint state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			state = (uint) (nk_widget(&header, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((win.layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			if (
				(nk_do_button_image(ref ctx.last_widget_state, win.buffer, (RectangleF) (header), (nk_image) (img),
					(int) (NK_BUTTON_DEFAULT), ctx.style.menu_button, _in_)) != 0) is_clicked = (int) (nk_true);
			return (int) (nk_menu_begin(ctx, win, id, (int) (is_clicked), (RectangleF) (header), (Vector2) (size)));
		}

		public static int nk_menu_begin_symbol(nk_context ctx, char* id, int sym, Vector2 size)
		{
			nk_window win;
			nk_input _in_;
			RectangleF header = new RectangleF();
			int is_clicked = (int) (nk_false);
			uint state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			state = (uint) (nk_widget(&header, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((win.layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			if (
				(nk_do_button_symbol(ref ctx.last_widget_state, win.buffer, (RectangleF) (header), (int) (sym),
					(int) (NK_BUTTON_DEFAULT), ctx.style.menu_button, _in_, ctx.style.font)) != 0) is_clicked = (int) (nk_true);
			return (int) (nk_menu_begin(ctx, win, id, (int) (is_clicked), (RectangleF) (header), (Vector2) (size)));
		}

		public static int nk_menu_begin_image_text(nk_context ctx, char* title, int len, uint align, nk_image img,
			Vector2 size)
		{
			nk_window win;
			RectangleF header = new RectangleF();
			nk_input _in_;
			int is_clicked = (int) (nk_false);
			uint state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			state = (uint) (nk_widget(&header, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((win.layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			if (
				(nk_do_button_text_image(ref ctx.last_widget_state, win.buffer, (RectangleF) (header), (nk_image) (img), title,
					(int) (len), (uint) (align), (int) (NK_BUTTON_DEFAULT), ctx.style.menu_button, ctx.style.font, _in_)) != 0)
				is_clicked = (int) (nk_true);
			return (int) (nk_menu_begin(ctx, win, title, (int) (is_clicked), (RectangleF) (header), (Vector2) (size)));
		}

		public static int nk_menu_begin_image_label(nk_context ctx, char* title, uint align, nk_image img, Vector2 size)
		{
			return
				(int)
					(nk_menu_begin_image_text(ctx, title, (int) (nk_strlen(title)), (uint) (align), (nk_image) (img), (Vector2) (size)));
		}

		public static int nk_menu_begin_symbol_text(nk_context ctx, char* title, int len, uint align, int sym, Vector2 size)
		{
			nk_window win;
			RectangleF header = new RectangleF();
			nk_input _in_;
			int is_clicked = (int) (nk_false);
			uint state;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (0);
			win = ctx.current;
			state = (uint) (nk_widget(&header, ctx));
			if (state == 0) return (int) (0);
			_in_ = (((state) == (NK_WIDGET_ROM)) || ((win.layout.flags & NK_WINDOW_ROM) != 0)) ? null : ctx.input;
			if (
				(nk_do_button_text_symbol(ref ctx.last_widget_state, win.buffer, (RectangleF) (header), (int) (sym), title, (int) (len),
					(uint) (align), (int) (NK_BUTTON_DEFAULT), ctx.style.menu_button, ctx.style.font, _in_)) != 0)
				is_clicked = (int) (nk_true);
			return (int) (nk_menu_begin(ctx, win, title, (int) (is_clicked), (RectangleF) (header), (Vector2) (size)));
		}

		public static int nk_menu_begin_symbol_label(nk_context ctx, char* title, uint align, int sym, Vector2 size)
		{
			return
				(int)
					(nk_menu_begin_symbol_text(ctx, title, (int) (nk_strlen(title)), (uint) (align), (int) (sym), (Vector2) (size)));
		}

		public static int nk_menu_item_text(nk_context ctx, char* title, int len, uint align)
		{
			return (int) (nk_contextual_item_text(ctx, title, (int) (len), (uint) (align)));
		}

		public static int nk_menu_item_label(nk_context ctx, char* label, uint align)
		{
			return (int) (nk_contextual_item_label(ctx, label, (uint) (align)));
		}

		public static int nk_menu_item_image_label(nk_context ctx, nk_image img, char* label, uint align)
		{
			return (int) (nk_contextual_item_image_label(ctx, (nk_image) (img), label, (uint) (align)));
		}

		public static int nk_menu_item_image_text(nk_context ctx, nk_image img, char* text, int len, uint align)
		{
			return (int) (nk_contextual_item_image_text(ctx, (nk_image) (img), text, (int) (len), (uint) (align)));
		}

		public static int nk_menu_item_symbol_text(nk_context ctx, int sym, char* text, int len, uint align)
		{
			return (int) (nk_contextual_item_symbol_text(ctx, (int) (sym), text, (int) (len), (uint) (align)));
		}

		public static int nk_menu_item_symbol_label(nk_context ctx, int sym, char* label, uint align)
		{
			return (int) (nk_contextual_item_symbol_label(ctx, (int) (sym), label, (uint) (align)));
		}

		public static void nk_menu_close(nk_context ctx)
		{
			nk_contextual_close(ctx);
		}

		public static void nk_menu_end(nk_context ctx)
		{
			nk_contextual_end(ctx);
		}
	}
}