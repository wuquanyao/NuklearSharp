using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace NuklearSharp
{
	public unsafe static partial class Nuklear
	{
		[StructLayout(LayoutKind.Sequential)]
		public unsafe partial struct Colorf
		{
			public float r;
			public float g;
			public float b;
			public float a;
		}

		[StructLayout(LayoutKind.Sequential)]
		public unsafe partial struct RectangleFi
		{
			public short x;
			public short y;
			public short w;
			public short h;
		}

		public unsafe partial class nk_image
		{
			public nk_handle handle = new nk_handle();
			public ushort w;
			public ushort h;
			public ushort[] region = new ushort[4];
		}

		public unsafe partial class nk_cursor
		{
			public nk_image img = new nk_image();
			public Vector2 size = new Vector2();
			public Vector2 offset = new Vector2();
		}

		public unsafe partial class nk_list_view
		{
			public int begin;
			public int end;
			public int count;
			public int total_height;
			public nk_context ctx;
			public uint* scroll_pointer;
			public uint scroll_value;
		}

		public unsafe partial class nk_chart_slot
		{
			public int type;
			public Color color = new Color();
			public Color highlight = new Color();
			public float min;
			public float max;
			public float range;
			public int count;
			public Vector2 last = new Vector2();
			public int index;
		}

		[StructLayout(LayoutKind.Sequential)]
		public unsafe partial struct nk_text
		{
			public Vector2 padding;
			public Color background;
			public Color text;
		}

		public static RectangleF RectangleFa(Vector2 pos, Vector2 size)
		{
			return (RectangleF) (RectangleF_((float) (pos.X), (float) (pos.Y), (float) (size.X), (float) (size.Y)));
		}

		public static Vector2 RectangleF_pos(RectangleF r)
		{
			Vector2 ret = new Vector2();
			ret.X = (float) (r.X);
			ret.Y = (float) (r.Y);
			return (Vector2) (ret);
		}

		public static Vector2 RectangleF_size(RectangleF r)
		{
			Vector2 ret = new Vector2();
			ret.X = (float) (r.Width);
			ret.Y = (float) (r.Height);
			return (Vector2) (ret);
		}

		public static RectangleF nk_shriRectangleF_(RectangleF r, float amount)
		{
			RectangleF res = new RectangleF();
			r.Width = (float) ((r.Width) < (2*amount) ? (2*amount) : (r.Width));
			r.Height = (float) ((r.Height) < (2*amount) ? (2*amount) : (r.Height));
			res.X = (float) (r.X + amount);
			res.Y = (float) (r.Y + amount);
			res.Width = (float) (r.Width - 2*amount);
			res.Height = (float) (r.Height - 2*amount);
			return (RectangleF) (res);
		}

		public static RectangleF nk_pad_rect(RectangleF r, Vector2 pad)
		{
			r.Width = (float) ((r.Width) < (2*pad.X) ? (2*pad.X) : (r.Width));
			r.Height = (float) ((r.Height) < (2*pad.Y) ? (2*pad.Y) : (r.Height));
			r.X += (float) (pad.X);
			r.Y += (float) (pad.Y);
			r.Width -= (float) (2*pad.X);
			r.Height -= (float) (2*pad.Y);
			return (RectangleF) (r);
		}

		public static Color nk_rgba_cf(Colorf c)
		{
			return (Color) (nk_rgba_f((float) (c.r), (float) (c.g), (float) (c.b), (float) (c.a)));
		}

		public static Color nk_rgb_cf(Colorf c)
		{
			return (Color) (nk_rgb_f((float) (c.r), (float) (c.g), (float) (c.b)));
		}

		public static uint Color_u32(Color _in_)
		{
			uint _out_ = (uint) (_in_.r);
			_out_ |= (uint) ((uint) (_in_.g) << 8);
			_out_ |= (uint) ((uint) (_in_.b) << 16);
			_out_ |= (uint) ((uint) (_in_.a) << 24);
			return (uint) (_out_);
		}

		public static Colorf Color_cf(Color _in_)
		{
			Colorf o = new Colorf();
			Color_f(&o.r, &o.g, &o.b, &o.a, (Color) (_in_));
			return (Colorf) (o);
		}

		public static nk_image nk_subimage_handle(nk_handle handle, ushort w, ushort h, RectangleF r)
		{
			nk_image s = new nk_image();

			s.handle = (nk_handle) (handle);
			s.Width = (ushort) (w);
			s.Height = (ushort) (h);
			s.region[0] = ((ushort) (r.X));
			s.region[1] = ((ushort) (r.Y));
			s.region[2] = ((ushort) (r.Width));
			s.region[3] = ((ushort) (r.Height));
			return (nk_image) (s);
		}

		public static nk_image nk_image_handle(nk_handle handle)
		{
			nk_image s = new nk_image();

			s.handle = (nk_handle) (handle);
			s.Width = (ushort) (0);
			s.Height = (ushort) (0);
			s.region[0] = (ushort) (0);
			s.region[1] = (ushort) (0);
			s.region[2] = (ushort) (0);
			s.region[3] = (ushort) (0);
			return (nk_image) (s);
		}

		public static int nk_image_is_subimage(nk_image img)
		{
			return (int) ((((img.Width) == (0)) && ((img.Height) == (0))) ? 1 : 0);
		}

		public static void nk_unify(ref RectangleF clip, ref RectangleF a, float x0, float y0, float x1, float y1)
		{
			clip.X = (float) ((a.X) < (x0) ? (x0) : (a.X));
			clip.Y = (float) ((a.Y) < (y0) ? (y0) : (a.Y));
			clip.Width = (float) (((a.X + a.Width) < (x1) ? (a.X + a.Width) : (x1)) - clip.X);
			clip.Height = (float) (((a.Y + a.Height) < (y1) ? (a.Y + a.Height) : (y1)) - clip.Y);
			clip.Width = (float) ((0) < (clip.Width) ? (clip.Width) : (0));
			clip.Height = (float) ((0) < (clip.Height) ? (clip.Height) : (0));
		}

		public static void nk_triangle_from_direction(Vector2* result, RectangleF r, float pad_x, float pad_y, int direction)
		{
			float w_half;
			float h_half;
			r.Width = (float) ((2*pad_x) < (r.Width) ? (r.Width) : (2*pad_x));
			r.Height = (float) ((2*pad_y) < (r.Height) ? (r.Height) : (2*pad_y));
			r.Width = (float) (r.Width - 2*pad_x);
			r.Height = (float) (r.Height - 2*pad_y);
			r.X = (float) (r.X + pad_x);
			r.Y = (float) (r.Y + pad_y);
			w_half = (float) (r.Width/2.0f);
			h_half = (float) (r.Height/2.0f);
			if ((direction) == (NK_UP))
			{
				result[0] = (Vector2) (new Vector2((float) (r.X + w_half), (float) (r.Y)));
				result[1] = (Vector2) (new Vector2((float) (r.X + r.Width), (float) (r.Y + r.Height)));
				result[2] = (Vector2) (new Vector2((float) (r.X), (float) (r.Y + r.Height)));
			}
			else if ((direction) == (NK_RIGHT))
			{
				result[0] = (Vector2) (new Vector2((float) (r.X), (float) (r.Y)));
				result[1] = (Vector2) (new Vector2((float) (r.X + r.Width), (float) (r.Y + h_half)));
				result[2] = (Vector2) (new Vector2((float) (r.X), (float) (r.Y + r.Height)));
			}
			else if ((direction) == (NK_DOWN))
			{
				result[0] = (Vector2) (new Vector2((float) (r.X), (float) (r.Y)));
				result[1] = (Vector2) (new Vector2((float) (r.X + r.Width), (float) (r.Y)));
				result[2] = (Vector2) (new Vector2((float) (r.X + w_half), (float) (r.Y + r.Height)));
			}
			else
			{
				result[0] = (Vector2) (new Vector2((float) (r.X), (float) (r.Y + h_half)));
				result[1] = (Vector2) (new Vector2((float) (r.X + r.Width), (float) (r.Y)));
				result[2] = (Vector2) (new Vector2((float) (r.X + r.Width), (float) (r.Y + r.Height)));
			}

		}

		public static void* nk_malloc(nk_handle unused, void* old, ulong size)
		{
			return CRuntime.malloc((ulong) (size));
		}

		public static void nk_mfree(nk_handle unused, void* ptr)
		{
			CRuntime.free(ptr);
		}

		public static float nk_font_text_width(nk_font font, float height, char* text, int len)
		{
			char unicode;
			int text_len = (int) (0);
			float text_width = (float) (0);
			int glyph_len = (int) (0);
			float scale = (float) (0);

			if (((font == null) || (text == null)) || (len == 0)) return (float) (0);
			scale = (float) (height/font.info.Height);
			glyph_len = (int) (text_len = (int) (nk_utf_decode(text, &unicode, (int) (len))));
			if (glyph_len == 0) return (float) (0);
			while ((text_len <= len) && ((glyph_len) != 0))
			{
				nk_font_glyph* g;
				if ((unicode) == (0xFFFD)) break;
				g = nk_font_find_glyph(font, unicode);
				text_width += (float) (g->xadvance*scale);
				glyph_len = (int) (nk_utf_decode(text + text_len, &unicode, (int) (len - text_len)));
				text_len += (int) (glyph_len);
			}
			return (float) (text_width);
		}

		public static void nk_font_query_font_glyph(nk_font font, float height, nk_user_font_glyph* glyph, char codepoint,
			char next_codepoint)
		{
			float scale;
			nk_font_glyph* g;


			if ((font == null) || (glyph == null)) return;
			scale = (float) (height/font.info.Height);
			g = nk_font_find_glyph(font, codepoint);
			glyph->width = (float) ((g->x1 - g->x0)*scale);
			glyph->height = (float) ((g->y1 - g->y0)*scale);
			glyph->offset = (Vector2) (new Vector2((float) (g->x0*scale), (float) (g->y0*scale)));
			glyph->xadvance = (float) (g->xadvance*scale);
			glyph->uv_x[0] = g->u0;
			glyph->uv_y[0] = g->v0;
			glyph->uv_x[1] = g->u1;
			glyph->uv_y[1] = g->v1;
		}

		public static nk_style_item nk_style_item_image(nk_image img)
		{
			nk_style_item i = new nk_style_item();
			i.type = (int) (NK_STYLE_ITEM_IMAGE);
			i.data.image = (nk_image) (img);
			return (nk_style_item) (i);
		}

		public static nk_style_item nk_style_item_color(Color col)
		{
			nk_style_item i = new nk_style_item();
			i.type = (int) (NK_STYLE_ITEM_COLOR);
			i.data.color = (Color) (col);
			return (nk_style_item) (i);
		}

		public static void nk_layout_widget_space(RectangleF* bounds, nk_context ctx, nk_window win, int modify)
		{
			nk_panel layout;
			nk_style style;
			Vector2 spacing = new Vector2();
			Vector2 padding = new Vector2();
			float item_offset = (float) (0);
			float item_width = (float) (0);
			float item_spacing = (float) (0);
			float panel_space = (float) (0);
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			style = ctx.style;
			spacing = (Vector2) (style.Widthindow.spacing);
			padding = (Vector2) (nk_panel_get_padding(style, (int) (layout.type)));
			panel_space =
				(float)
					(nk_layout_row_calculate_usable_space(ctx.style, (int) (layout.type), (float) (layout.bounds.Width),
						(int) (layout.row.columns)));
			switch (layout.row.type)
			{
				case NK_LAYOUT_DYNAMIC_FIXED:
				{
					item_width = (float) (((1.0f) < (panel_space - 1.0f) ? (panel_space - 1.0f) : (1.0f))/(float) (layout.row.columns));
					item_offset = (float) ((float) (layout.row.index)*item_width);
					item_spacing = (float) ((float) (layout.row.index)*spacing.X);
				}
					break;
				case NK_LAYOUT_DYNAMIC_ROW:
				{
					item_width = (float) (layout.row.item_width*panel_space);
					item_offset = (float) (layout.row.item_offset);
					item_spacing = (float) (0);
					if ((modify) != 0)
					{
						layout.row.item_offset += (float) (item_width + spacing.X);
						layout.row.filled += (float) (layout.row.item_width);
						layout.row.index = (int) (0);
					}
				}
					break;
				case NK_LAYOUT_DYNAMIC_FREE:
				{
					bounds->x = (float) (layout.at_x + (layout.bounds.Width*layout.row.item.X));
					bounds->x -= ((float) (layout.offset.X));
					bounds->y = (float) (layout.at_y + (layout.row.Height*layout.row.item.Y));
					bounds->y -= ((float) (layout.offset.Y));
					bounds->w = (float) (layout.bounds.Width*layout.row.item.Width);
					bounds->h = (float) (layout.row.Height*layout.row.item.Height);
					return;
				}
				case NK_LAYOUT_DYNAMIC:
				{
					float ratio;
					ratio =
						(float)
							(((layout.row.ratio[layout.row.index]) < (0)) ? layout.row.item_width : layout.row.ratio[layout.row.index]);
					item_spacing = (float) ((float) (layout.row.index)*spacing.X);
					item_width = (float) (ratio*panel_space);
					item_offset = (float) (layout.row.item_offset);
					if ((modify) != 0)
					{
						layout.row.item_offset += (float) (item_width);
						layout.row.filled += (float) (ratio);
					}
				}
					break;
				case NK_LAYOUT_STATIC_FIXED:
				{
					item_width = (float) (layout.row.item_width);
					item_offset = (float) ((float) (layout.row.index)*item_width);
					item_spacing = (float) ((float) (layout.row.index)*spacing.X);
				}
					break;
				case NK_LAYOUT_STATIC_ROW:
				{
					item_width = (float) (layout.row.item_width);
					item_offset = (float) (layout.row.item_offset);
					item_spacing = (float) ((float) (layout.row.index)*spacing.X);
					if ((modify) != 0) layout.row.item_offset += (float) (item_width);
				}
					break;
				case NK_LAYOUT_STATIC_FREE:
				{
					bounds->x = (float) (layout.at_x + layout.row.item.X);
					bounds->w = (float) (layout.row.item.Width);
					if (((bounds->x + bounds->w) > (layout.max_x)) && ((modify) != 0)) layout.max_x = (float) (bounds->x + bounds->w);
					bounds->x -= ((float) (layout.offset.X));
					bounds->y = (float) (layout.at_y + layout.row.item.Y);
					bounds->y -= ((float) (layout.offset.Y));
					bounds->h = (float) (layout.row.item.Height);
					return;
				}
				case NK_LAYOUT_STATIC:
				{
					item_spacing = (float) ((float) (layout.row.index)*spacing.X);
					item_width = (float) (layout.row.ratio[layout.row.index]);
					item_offset = (float) (layout.row.item_offset);
					if ((modify) != 0) layout.row.item_offset += (float) (item_width);
				}
					break;
				case NK_LAYOUT_TEMPLATE:
				{
					item_width = (float) (layout.row.templates[layout.row.index]);
					item_offset = (float) (layout.row.item_offset);
					item_spacing = (float) ((float) (layout.row.index)*spacing.X);
					if ((modify) != 0) layout.row.item_offset += (float) (item_width);
				}
					break;
				default:
					;
					break;
			}

			bounds->w = (float) (item_width);
			bounds->h = (float) (layout.row.Height - spacing.Y);
			bounds->y = (float) (layout.at_y - (float) (layout.offset.Y));
			bounds->x = (float) (layout.at_x + item_offset + item_spacing + padding.X);
			if (((bounds->x + bounds->w) > (layout.max_x)) && ((modify) != 0)) layout.max_x = (float) (bounds->x + bounds->w);
			bounds->x -= ((float) (layout.offset.X));
		}

		public static void nk_panel_alloc_space(RectangleF* bounds, nk_context ctx)
		{
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			if ((layout.row.index) >= (layout.row.columns)) nk_panel_alloc_row(ctx, win);
			nk_layout_widget_space(bounds, ctx, win, (int) (nk_true));
			layout.row.index++;
		}

		public static void nk_layout_peek(RectangleF* bounds, nk_context ctx)
		{
			float y;
			int index;
			nk_window win;
			nk_panel layout;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return;
			win = ctx.current;
			layout = win.layout;
			y = (float) (layout.at_y);
			index = (int) (layout.row.index);
			if ((layout.row.index) >= (layout.row.columns))
			{
				layout.at_y += (float) (layout.row.Height);
				layout.row.index = (int) (0);
			}

			nk_layout_widget_space(bounds, ctx, win, (int) (nk_false));
			if (layout.row.index == 0)
			{
				bounds->x -= (float) (layout.row.item_offset);
			}

			layout.at_y = (float) (y);
			layout.row.index = (int) (index);
		}

		public static int nk_widget(RectangleF* bounds, nk_context ctx)
		{
			RectangleF c = new RectangleF();
			RectangleF v = new RectangleF();
			nk_window win;
			nk_panel layout;
			nk_input _in_;
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (NK_WIDGET_INVALID);
			nk_panel_alloc_space(bounds, ctx);
			win = ctx.current;
			layout = win.layout;
			_in_ = ctx.input;
			c = (RectangleF) (layout.clip);
			bounds->x = ((float) ((int) (bounds->x)));
			bounds->y = ((float) ((int) (bounds->y)));
			bounds->w = ((float) ((int) (bounds->w)));
			bounds->h = ((float) ((int) (bounds->h)));
			c.X = ((float) ((int) (c.X)));
			c.Y = ((float) ((int) (c.Y)));
			c.Width = ((float) ((int) (c.Width)));
			c.Height = ((float) ((int) (c.Height)));
			nk_unify(ref v, ref c, (float) (bounds->x), (float) (bounds->y), (float) (bounds->x + bounds->w),
				(float) (bounds->y + bounds->h));
			if (
				!(!(((((bounds->x) > (c.X + c.Width)) || ((bounds->x + bounds->w) < (c.X))) || ((bounds->y) > (c.Y + c.Height))) ||
				    ((bounds->y + bounds->h) < (c.Y))))) return (int) (NK_WIDGET_INVALID);
			if (
				!((((v.X) <= (_in_.mouse.pos.X)) && ((_in_.mouse.pos.X) < (v.X + v.Width))) &&
				  (((v.Y) <= (_in_.mouse.pos.Y)) && ((_in_.mouse.pos.Y) < (v.Y + v.Height))))) return (int) (NK_WIDGET_ROM);
			return (int) (NK_WIDGET_VALID);
		}

		public static int nk_widget_fitting(RectangleF* bounds, nk_context ctx, Vector2 item_padding)
		{
			nk_window win;
			nk_style style;
			nk_panel layout;
			int state;
			Vector2 panel_padding = new Vector2();
			if (((ctx == null) || (ctx.current == null)) || (ctx.current.layout == null)) return (int) (NK_WIDGET_INVALID);
			win = ctx.current;
			style = ctx.style;
			layout = win.layout;
			state = (int) (nk_widget(bounds, ctx));
			panel_padding = (Vector2) (nk_panel_get_padding(style, (int) (layout.type)));
			if ((layout.row.index) == (1))
			{
				bounds->w += (float) (panel_padding.X);
				bounds->x -= (float) (panel_padding.X);
			}
			else bounds->x -= (float) (item_padding.X);
			if ((layout.row.index) == (layout.row.columns)) bounds->w += (float) (panel_padding.X);
			else bounds->w += (float) (item_padding.X);
			return (int) (state);
		}

		public static void nk_list_view_end(nk_list_view view)
		{
			nk_context ctx;
			nk_window win;
			nk_panel layout;
			if ((view == null) || (view.ctx == null)) return;
			ctx = view.ctx;
			win = ctx.current;
			layout = win.layout;
			layout.at_y = (float) (layout.bounds.Y + (float) (view.total_height));
			*view.scroll_pointer = (uint) (*view.scroll_pointer + view.scroll_value);
			nk_group_end(view.ctx);
		}
	}
}