using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NuklearSharp
{
    internal class Batch
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly NkBuffer<VertexPositionColorTexture> _vertices = new NkBuffer<VertexPositionColorTexture>();
        private readonly NkBuffer<ushort> _indices = new NkBuffer<ushort>();
        private readonly Stack<RectangleF> _scissors = new Stack<RectangleF>();
        private readonly List<Vector2> _points = new List<Vector2>();
        private readonly List<Vector2> normals = new List<Vector2>();
        private readonly Nuklear.nk_convert_config config;

        public Batch(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException();
            }

            _graphicsDevice = graphicsDevice;
        }
        
		public void nk_push_scissor(RectangleF scissor)
		{
			if (_scissors.Count == 0)
			{
				if (scissor.Widthidth < 1 || scissor.Height < 1) return;
// 				GL20.EnableScissors(true);
			}
			else
			{
				// merge scissors
				var parent = _scissors.Peek();
				scissor = RectangleF.Intersect(parent, scissor);
				if (scissor.Widthidth == 0 || scissor.Height == 0)
				{
					return;
				}
			}
			_scissors.Push(scissor);

			_graphicsDevice.ScissorRectangleF = scissor;
		}

		public void nk_draw_list_stroke_poly_line(Color color, int closed, float thickness, bool aliasing)
		{
			int count;
			int thick_line;

			var points_count = _points.Count;
			Nuklear.Colorf col = new Nuklear.Colorf();
			Nuklear.Colorf col_trans = new Nuklear.Colorf();
			if (points_count < 2) return;
			
			color.A = ((byte) ((float) (color.A)*config.global_alpha));
			count = (int) (points_count);
			if (closed == 0) count = (int) (points_count - 1);
			thick_line = (int) ((thickness) > (1.0f) ? 1 : 0);
			color.A = ((byte) ((float) (color.A)*config.global_alpha));
			Utility.color_fv(ref col, (Color) (color));
			col_trans = (Nuklear.Colorf) (col);
			col_trans.a = (float) (0);
			if (aliasing)
			{
				float AA_SIZE = (float) (1.0f);
				int i1 = (int) (0);
				int idx_count = (int) ((thick_line) != 0 ? (count*18) : (count*12));
				int vtx_count = (int) ((thick_line) != 0 ? (points_count*4) : (points_count*3));


				Vector2 temp;
				int points_total = (int) (((thick_line) != 0 ? 5 : 3)*(int) points_count);

				for (i1 = (int) (0); (i1) < (count); ++i1)
				{
					int i2 = (((i1 + 1) == points_count) ? 0 : (i1 + 1));
					Vector2 diff =
						(Vector2)
							(new Vector2((float) ((_points[i2]).X - (_points[i1]).X),
								(float) ((_points[i2]).Y - (_points[i1]).Y)));
					float len;
					len = (float) ((diff).X*(diff).X + (diff).Y*(diff).Y);
					if (len != 0.0f) len = (float) (Utility.nk_inv_sqrt((float) (len)));
					else len = (float) (1.0f);
					diff = (Vector2) (new Vector2((float) ((diff).X*(len)), (float) ((diff).Y*(len))));
					normals[i1] = new Vector2(diff.Y, -diff.X);
				}
				if (closed == 0) normals[points_count - 1] = (Vector2) (normals[points_count - 2]);
				if (thick_line == 0)
				{
					int idx1;
					int i;
					if (closed == 0)
					{
						Vector2 d = new Vector2();
						temp[0] =
							(Vector2)
								(new Vector2(
									(float)
										((_points[0]).X + (new Vector2((float) ((normals[0]).X*(AA_SIZE)), (float) ((normals[0]).Y*(AA_SIZE)))).X),
									(float)
										((_points[0]).Y + (new Vector2((float) ((normals[0]).X*(AA_SIZE)), (float) ((normals[0]).Y*(AA_SIZE)))).Y)));
						temp[1] =
							(Vector2)
								(new Vector2(
									(float)
										((_points[0]).X - (new Vector2((float) ((normals[0]).X*(AA_SIZE)), (float) ((normals[0]).Y*(AA_SIZE)))).X),
									(float)
										((_points[0]).Y - (new Vector2((float) ((normals[0]).X*(AA_SIZE)), (float) ((normals[0]).Y*(AA_SIZE)))).Y)));
						d =
							(Vector2)
								(new Vector2((float) ((normals[points_count - 1]).X*(AA_SIZE)),
									(float) ((normals[points_count - 1]).Y*(AA_SIZE))));
						temp[(points_count - 1)*2 + 0] =
							(Vector2)
								(new Vector2((float) ((_points[points_count - 1]).X + (d).X),
									(float) ((_points[points_count - 1]).Y + (d).Y)));
						temp[(points_count - 1)*2 + 1] =
							(Vector2)
								(new Vector2((float) ((_points[points_count - 1]).X - (d).X),
									(float) ((_points[points_count - 1]).Y - (d).Y)));
					}
					idx1 = (int) (index);
					for (i1 = (int) (0); (i1) < (count); i1++)
					{
						Vector2 dm = new Vector2();
						float dmr2;
						int i2 = (int) (((i1 + 1) == (points_count)) ? 0 : (i1 + 1));
						int idx2 = (int) (((i1 + 1) == (points_count)) ? index : (idx1 + 3));
						dm =
							(Vector2)
								(new Vector2(
									(float)
										((new Vector2((float) ((normals[i1]).X + (normals[i2]).X), (float) ((normals[i1]).Y + (normals[i2]).Y))).X*
										 (0.5f)),
									(float)
										((new Vector2((float) ((normals[i1]).X + (normals[i2]).X), (float) ((normals[i1]).Y + (normals[i2]).Y))).Y*
										 (0.5f))));
						dmr2 = (float) (dm.X*dm.X + dm.Y*dm.Y);
						if ((dmr2) > (0.000001f))
						{
							float scale = (float) (1.0f/dmr2);
							scale = (float) ((100.0f) < (scale) ? (100.0f) : (scale));
							dm = (Vector2) (new Vector2((float) ((dm).X*(scale)), (float) ((dm).Y*(scale))));
						}
						dm = (Vector2) (new Vector2((float) ((dm).X*(AA_SIZE)), (float) ((dm).Y*(AA_SIZE))));
						temp[i2*2 + 0] =
							(Vector2) (new Vector2((float) ((_points[i2]).X + (dm).X), (float) ((_points[i2]).Y + (dm).Y)));
						temp[i2*2 + 1] =
							(Vector2) (new Vector2((float) ((_points[i2]).X - (dm).X), (float) ((_points[i2]).Y - (dm).Y)));
						ids[0] = ((ushort) (idx2 + 0));
						ids[1] = ((ushort) (idx1 + 0));
						ids[2] = ((ushort) (idx1 + 2));
						ids[3] = ((ushort) (idx1 + 2));
						ids[4] = ((ushort) (idx2 + 2));
						ids[5] = ((ushort) (idx2 + 0));
						ids[6] = ((ushort) (idx2 + 1));
						ids[7] = ((ushort) (idx1 + 1));
						ids[8] = ((ushort) (idx1 + 0));
						ids[9] = ((ushort) (idx1 + 0));
						ids[10] = ((ushort) (idx2 + 0));
						ids[11] = ((ushort) (idx2 + 1));
						ids += 12;
						idx1 = (int) (idx2);
					}
					for (i = (int) (0); (i) < (points_count); ++i)
					{
						Vector2 uv = (Vector2) (config._null_.uv);
						vtx = nk_draw_vertex(vtx, config, (Vector2) (_points[i]), (Vector2) (uv), (Nuklear.Colorf) (col));
						vtx = nk_draw_vertex(vtx, config, (Vector2) (temp[i*2 + 0]), (Vector2) (uv), (Nuklear.Colorf) (col_trans));
						vtx = nk_draw_vertex(vtx, config, (Vector2) (temp[i*2 + 1]), (Vector2) (uv), (Nuklear.Colorf) (col_trans));
					}
				}
				else
				{
					int idx1;
					int i;
					float half_inner_thickness = (float) ((thickness - AA_SIZE)*0.5f);
					if (closed == 0)
					{
						Vector2 d1 =
							(Vector2)
								(new Vector2((float) ((normals[0]).X*(half_inner_thickness + AA_SIZE)),
									(float) ((normals[0]).Y*(half_inner_thickness + AA_SIZE))));
						Vector2 d2 =
							(Vector2)
								(new Vector2((float) ((normals[0]).X*(half_inner_thickness)), (float) ((normals[0]).Y*(half_inner_thickness))));
						temp[0] = (Vector2) (new Vector2((float) ((_points[0]).X + (d1).X), (float) ((_points[0]).Y + (d1).Y)));
						temp[1] = (Vector2) (new Vector2((float) ((_points[0]).X + (d2).X), (float) ((_points[0]).Y + (d2).Y)));
						temp[2] = (Vector2) (new Vector2((float) ((_points[0]).X - (d2).X), (float) ((_points[0]).Y - (d2).Y)));
						temp[3] = (Vector2) (new Vector2((float) ((_points[0]).X - (d1).X), (float) ((_points[0]).Y - (d1).Y)));
						d1 =
							(Vector2)
								(new Vector2((float) ((normals[points_count - 1]).X*(half_inner_thickness + AA_SIZE)),
									(float) ((normals[points_count - 1]).Y*(half_inner_thickness + AA_SIZE))));
						d2 =
							(Vector2)
								(new Vector2((float) ((normals[points_count - 1]).X*(half_inner_thickness)),
									(float) ((normals[points_count - 1]).Y*(half_inner_thickness))));
						temp[(points_count - 1)*4 + 0] =
							(Vector2)
								(new Vector2((float) ((_points[points_count - 1]).X + (d1).X),
									(float) ((_points[points_count - 1]).Y + (d1).Y)));
						temp[(points_count - 1)*4 + 1] =
							(Vector2)
								(new Vector2((float) ((_points[points_count - 1]).X + (d2).X),
									(float) ((_points[points_count - 1]).Y + (d2).Y)));
						temp[(points_count - 1)*4 + 2] =
							(Vector2)
								(new Vector2((float) ((_points[points_count - 1]).X - (d2).X),
									(float) ((_points[points_count - 1]).Y - (d2).Y)));
						temp[(points_count - 1)*4 + 3] =
							(Vector2)
								(new Vector2((float) ((_points[points_count - 1]).X - (d1).X),
									(float) ((_points[points_count - 1]).Y - (d1).Y)));
					}
					idx1 = (int) (index);
					for (i1 = (int) (0); (i1) < (count); ++i1)
					{
						Vector2 dm_out = new Vector2();
						Vector2 dm_in = new Vector2();
						int i2 = (int) (((i1 + 1) == (points_count)) ? 0 : (i1 + 1));
						int idx2 = (int) (((i1 + 1) == (points_count)) ? index : (idx1 + 4));
						Vector2 dm =
							(Vector2)
								(new Vector2(
									(float)
										((new Vector2((float) ((normals[i1]).X + (normals[i2]).X), (float) ((normals[i1]).Y + (normals[i2]).Y))).X*
										 (0.5f)),
									(float)
										((new Vector2((float) ((normals[i1]).X + (normals[i2]).X), (float) ((normals[i1]).Y + (normals[i2]).Y))).Y*
										 (0.5f))));
						float dmr2 = (float) (dm.X*dm.X + dm.Y*dm.Y);
						if ((dmr2) > (0.000001f))
						{
							float scale = (float) (1.0f/dmr2);
							scale = (float) ((100.0f) < (scale) ? (100.0f) : (scale));
							dm = (Vector2) (new Vector2((float) ((dm).X*(scale)), (float) ((dm).Y*(scale))));
						}
						dm_out =
							(Vector2)
								(new Vector2((float) ((dm).X*((half_inner_thickness) + AA_SIZE)),
									(float) ((dm).Y*((half_inner_thickness) + AA_SIZE))));
						dm_in = (Vector2) (new Vector2((float) ((dm).X*(half_inner_thickness)), (float) ((dm).Y*(half_inner_thickness))));
						temp[i2*4 + 0] =
							(Vector2) (new Vector2((float) ((_points[i2]).X + (dm_out).X), (float) ((_points[i2]).Y + (dm_out).Y)));
						temp[i2*4 + 1] =
							(Vector2) (new Vector2((float) ((_points[i2]).X + (dm_in).X), (float) ((_points[i2]).Y + (dm_in).Y)));
						temp[i2*4 + 2] =
							(Vector2) (new Vector2((float) ((_points[i2]).X - (dm_in).X), (float) ((_points[i2]).Y - (dm_in).Y)));
						temp[i2*4 + 3] =
							(Vector2) (new Vector2((float) ((_points[i2]).X - (dm_out).X), (float) ((_points[i2]).Y - (dm_out).Y)));
						ids[0] = ((ushort) (idx2 + 1));
						ids[1] = ((ushort) (idx1 + 1));
						ids[2] = ((ushort) (idx1 + 2));
						ids[3] = ((ushort) (idx1 + 2));
						ids[4] = ((ushort) (idx2 + 2));
						ids[5] = ((ushort) (idx2 + 1));
						ids[6] = ((ushort) (idx2 + 1));
						ids[7] = ((ushort) (idx1 + 1));
						ids[8] = ((ushort) (idx1 + 0));
						ids[9] = ((ushort) (idx1 + 0));
						ids[10] = ((ushort) (idx2 + 0));
						ids[11] = ((ushort) (idx2 + 1));
						ids[12] = ((ushort) (idx2 + 2));
						ids[13] = ((ushort) (idx1 + 2));
						ids[14] = ((ushort) (idx1 + 3));
						ids[15] = ((ushort) (idx1 + 3));
						ids[16] = ((ushort) (idx2 + 3));
						ids[17] = ((ushort) (idx2 + 2));
						ids += 18;
						idx1 = (int) (idx2);
					}
					for (i = (int) (0); (i) < (points_count); ++i)
					{
						Vector2 uv = (Vector2) (config._null_.uv);
						vtx = nk_draw_vertex(vtx, config, (Vector2) (temp[i*4 + 0]), (Vector2) (uv), (Nuklear.Colorf) (col_trans));
						vtx = nk_draw_vertex(vtx, config, (Vector2) (temp[i*4 + 1]), (Vector2) (uv), (Nuklear.Colorf) (col));
						vtx = nk_draw_vertex(vtx, config, (Vector2) (temp[i*4 + 2]), (Vector2) (uv), (Nuklear.Colorf) (col));
						vtx = nk_draw_vertex(vtx, config, (Vector2) (temp[i*4 + 3]), (Vector2) (uv), (Nuklear.Colorf) (col_trans));
					}
				}

				list.normals.reset();
			}
			else
			{
				int i1 = (int) (0);
				int idx = (int) (list.vertex_offset);
				int idx_count = (int) (count*6);
				int vtx_count = (int) (count*4);

				int vtxStart = list.vertices.Count;
				list.vertices.addToEnd((int) (vtx_count*config.vertex_size));
				int idxStart = list.addElements((int)idx_count);

				fixed (byte* vtx2 = list.vertices.Data)
				{
					void* vtx = (void*) (vtx2 + vtxStart);
					fixed (ushort* ids2 = list.elements.Data)
					{
						ushort* ids = ids2 + idxStart;

						for (i1 = (int) (0); (i1) < (count); ++i1)
						{
							float dx;
							float dy;
							Vector2 uv = (Vector2) (config._null_.uv);
							int i2 = (int) (((i1 + 1) == (points_count)) ? 0 : i1 + 1);
							Vector2 p1 = (Vector2) (_points[i1]);
							Vector2 p2 = (Vector2) (_points[i2]);
							Vector2 diff = (Vector2) (new Vector2((float) ((p2).X - (p1).X), (float) ((p2).Y - (p1).Y)));
							float len;
							len = (float) ((diff).X*(diff).X + (diff).Y*(diff).Y);
							if (len != 0.0f) len = (float) (Utility.nk_inv_sqrt((float) (len)));
							else len = (float) (1.0f);
							diff = (Vector2) (new Vector2((float) ((diff).X*(len)), (float) ((diff).Y*(len))));
							dx = (float) (diff.X*(thickness*0.5f));
							dy = (float) (diff.Y*(thickness*0.5f));
							vtx = nk_draw_vertex(vtx, config, (Vector2) (new Vector2((float) (p1.X + dy), (float) (p1.Y - dx))),
								(Vector2) (uv), (Nuklear.Colorf) (col));
							vtx = nk_draw_vertex(vtx, config, (Vector2) (new Vector2((float) (p2.X + dy), (float) (p2.Y - dx))),
								(Vector2) (uv), (Nuklear.Colorf) (col));
							vtx = nk_draw_vertex(vtx, config, (Vector2) (new Vector2((float) (p2.X - dy), (float) (p2.Y + dx))),
								(Vector2) (uv), (Nuklear.Colorf) (col));
							vtx = nk_draw_vertex(vtx, config, (Vector2) (new Vector2((float) (p1.X - dy), (float) (p1.Y + dx))),
								(Vector2) (uv), (Nuklear.Colorf) (col));
							ids[0] = ((ushort) (idx + 0));
							ids[1] = ((ushort) (idx + 1));
							ids[2] = ((ushort) (idx + 2));
							ids[3] = ((ushort) (idx + 0));
							ids[4] = ((ushort) (idx + 2));
							ids[5] = ((ushort) (idx + 3));
							ids += 6;
							idx += (int) (4);
						}
					}
				}
			}
		}

		public static void nk_draw_list_fill_poly_convex(Color color, int aliasing)
		{
			Nuklear.Colorf col = new Nuklear.Colorf();
			Nuklear.Colorf col_trans = new Nuklear.Colorf();

			var points_count = (int) _points.Count;
			if ((list == null) || ((_points.Count) < (3))) return;
			color.A = ((byte) ((float) (color.A)*config.global_alpha));
			Utility.color_fv(&col.r, (Color) (color));
			col_trans = (Nuklear.Colorf) (col);
			col_trans.a = (float) (0);
			if ((aliasing) == (NK_ANTI_ALIASING_ON))
			{
				int i = (int) (0);
				int i0 = (int) (0);
				int i1 = (int) (0);
				float AA_SIZE = (float) (1.0f);
				int index = (int) (list.vertex_offset);
				int idx_count = (int) ((points_count - 2)*3 + points_count*6);
				int vtx_count = (int) (points_count*2);

				int vtxStart = list.vertices.Count;
				list.vertices.addToEnd((int) (vtx_count*config.vertex_size));
				int idxStart = list.addElements((int)idx_count);

				fixed (byte* vtx2 = list.vertices.Data)
				{
					void* vtx = (void*) (vtx2 + vtxStart);
					fixed (ushort* ids2 = list.elements.Data)
					{
						ushort* ids = ids2 + idxStart;
						uint vtx_inner_idx = (uint) (index + 0);
						uint vtx_outer_idx = (uint) (index + 1);
						if ((vtx == null) || (ids == null)) return;

						int normalsStart = list.normals.Count;
						list.normals.addToEnd((int) points_count);

						fixed (Vector2* normals2 = list.normals.Data)
						{
							Vector2* normals = normals2 + normalsStart;

							for (i = (int) (2); (i) < (points_count); i++)
							{
								ids[0] = ((ushort) (vtx_inner_idx));
								ids[1] = ((ushort) (vtx_inner_idx + ((i - 1) << 1)));
								ids[2] = ((ushort) (vtx_inner_idx + (i << 1)));
								ids += 3;
							}
							for (i0 = (int) (points_count - 1) , i1 = (int) (0); (i1) < (points_count); i0 = (int) (i1++))
							{
								Vector2 p0 = (Vector2) (_points[i0]);
								Vector2 p1 = (Vector2) (_points[i1]);
								Vector2 diff = (Vector2) (new Vector2((float) ((p1).X - (p0).X), (float) ((p1).Y - (p0).Y)));
								float len = (float) ((diff).X*(diff).X + (diff).Y*(diff).Y);
								if (len != 0.0f) len = (float) (Utility.nk_inv_sqrt((float) (len)));
								else len = (float) (1.0f);
								diff = (Vector2) (new Vector2((float) ((diff).X*(len)), (float) ((diff).Y*(len))));
								normals[i0].X = (float) (diff.Y);
								normals[i0].Y = (float) (-diff.X);
							}
							for (i0 = (int) (points_count - 1) , i1 = (int) (0); (i1) < (points_count); i0 = (int) (i1++))
							{
								Vector2 uv = (Vector2) (config._null_.uv);
								Vector2 n0 = (Vector2) (normals[i0]);
								Vector2 n1 = (Vector2) (normals[i1]);
								Vector2 dm =
									(Vector2)
										(new Vector2((float) ((new Vector2((float) ((n0).X + (n1).X), (float) ((n0).Y + (n1).Y))).X*(0.5f)),
											(float) ((new Vector2((float) ((n0).X + (n1).X), (float) ((n0).Y + (n1).Y))).Y*(0.5f))));
								float dmr2 = (float) (dm.X*dm.X + dm.Y*dm.Y);
								if ((dmr2) > (0.000001f))
								{
									float scale = (float) (1.0f/dmr2);
									scale = (float) ((scale) < (100.0f) ? (scale) : (100.0f));
									dm = (Vector2) (new Vector2((float) ((dm).X*(scale)), (float) ((dm).Y*(scale))));
								}
								dm = (Vector2) (new Vector2((float) ((dm).X*(AA_SIZE*0.5f)), (float) ((dm).Y*(AA_SIZE*0.5f))));
								vtx = nk_draw_vertex(vtx, config,
									(Vector2) (new Vector2((float) ((_points[i1]).X - (dm).X), (float) ((_points[i1]).Y - (dm).Y))),
									(Vector2) (uv),
									(Nuklear.Colorf) (col));
								vtx = nk_draw_vertex(vtx, config,
									(Vector2) (new Vector2((float) ((_points[i1]).X + (dm).X), (float) ((_points[i1]).Y + (dm).Y))),
									(Vector2) (uv),
									(Nuklear.Colorf) (col_trans));
								ids[0] = ((ushort) (vtx_inner_idx + (i1 << 1)));
								ids[1] = ((ushort) (vtx_inner_idx + (i0 << 1)));
								ids[2] = ((ushort) (vtx_outer_idx + (i0 << 1)));
								ids[3] = ((ushort) (vtx_outer_idx + (i0 << 1)));
								ids[4] = ((ushort) (vtx_outer_idx + (i1 << 1)));
								ids[5] = ((ushort) (vtx_inner_idx + (i1 << 1)));
								ids += 6;
							}
						}
					}
				}
				list.normals.reset();
			}
			else
			{
				int i = (int) (0);
				int index = (int) (list.vertex_offset);
				int idx_count = (int) ((points_count - 2)*3);
				int vtx_count = (int) (points_count);
				int vtxStart = list.vertices.Count;
				list.vertices.addToEnd((int) (vtx_count*config.vertex_size));
				int idxStart = list.addElements((int)idx_count);

				fixed (byte* vtx2 = list.vertices.Data)
				{
					void* vtx = (void*) (vtx2 + vtxStart);
					fixed (ushort* ids2 = list.elements.Data)
					{
						ushort* ids = ids2 + idxStart;
						if ((vtx == null) || (ids == null)) return;
						for (i = (int) (0); (i) < (vtx_count); ++i)
						{
							vtx = nk_draw_vertex(vtx, config, (Vector2) (_points[i]), (Vector2) (config._null_.uv),
								(Nuklear.Colorf) (col));
						}
						for (i = (int) (2); (i) < (points_count); ++i)
						{
							ids[0] = ((ushort) (index));
							ids[1] = ((ushort) (index + i - 1));
							ids[2] = ((ushort) (index + i));
							ids += 3;
						}
					}
				}
			}
		}

		public static void nk_draw_list_path_clear(nk_draw_list list)
		{
			if (list == null) return;
			_points.reset();
		}

		public static void nk_draw_list_path_line_to(Vector2 pos)
		{
			if (list == null) return;
			if (list.buffer.Count == 0) nk_draw_list_add_clip(list, (RectangleF) (nk_null_rect));

			if ((list.buffer[list.buffer.Count - 1].texture.ptr != config._null_.texture.ptr))
				nk_draw_list_push_image(list, (Nuklear.nk_handle) (config._null_.texture));
			int i = _points.Count;
			_points.addToEnd(1);
			_points[i] = pos;
		}

		public static void nk_draw_list_path_arc_to_fast(Vector2 center, float radius, int a_min, int a_max)
		{
			int a = (int) (0);
			if (list == null) return;
			if (a_min <= a_max)
			{
				for (a = (int) (a_min); a <= a_max; a++)
				{
					Vector2 c = (Vector2) (list.circle_vtx[(int) (a)%(int) list.circle_vtx.Length]);
					float x = (float) (center.X + c.X*radius);
					float y = (float) (center.Y + c.Y*radius);
					nk_draw_list_path_line_to(list, (Vector2) (new Vector2((float) (x), (float) (y))));
				}
			}

		}

		public static void nk_draw_list_path_arc_to(Vector2 center, float radius, float a_min, float a_max,
			uint segments)
		{
			uint i = (uint) (0);
			if (list == null) return;
			if ((radius) == (0.0f)) return;
			{
				float d_angle = (float) ((a_max - a_min)/(float) (segments));
				float sin_d = (float) (nk_sin((float) (d_angle)));
				float cos_d = (float) (nk_cos((float) (d_angle)));
				float cx = (float) (nk_cos((float) (a_min))*radius);
				float cy = (float) (nk_sin((float) (a_min))*radius);
				for (i = (uint) (0); i <= segments; ++i)
				{
					float new_cx;
					float new_cy;
					float x = (float) (center.X + cx);
					float y = (float) (center.Y + cy);
					nk_draw_list_path_line_to(list, (Vector2) (new Vector2((float) (x), (float) (y))));
					new_cx = (float) (cx*cos_d - cy*sin_d);
					new_cy = (float) (cy*cos_d + cx*sin_d);
					cx = (float) (new_cx);
					cy = (float) (new_cy);
				}
			}
		}

		public static void nk_draw_list_path_rect_to(Vector2 a, Vector2 b, float rounding)
		{
			float r;
			if (list == null) return;
			r = (float) (rounding);
			r =
				(float)
					((r) < (((b.X - a.X) < (0)) ? -(b.X - a.X) : (b.X - a.X))
						? (r)
						: (((b.X - a.X) < (0)) ? -(b.X - a.X) : (b.X - a.X)));
			r =
				(float)
					((r) < (((b.Y - a.Y) < (0)) ? -(b.Y - a.Y) : (b.Y - a.Y))
						? (r)
						: (((b.Y - a.Y) < (0)) ? -(b.Y - a.Y) : (b.Y - a.Y)));
			if ((r) == (0.0f))
			{
				nk_draw_list_path_line_to(list, (Vector2) (a));
				nk_draw_list_path_line_to(list, (Vector2) (new Vector2((float) (b.X), (float) (a.Y))));
				nk_draw_list_path_line_to(list, (Vector2) (b));
				nk_draw_list_path_line_to(list, (Vector2) (new Vector2((float) (a.X), (float) (b.Y))));
			}
			else
			{
				nk_draw_list_path_arc_to_fast(list, (Vector2) (new Vector2((float) (a.X + r), (float) (a.Y + r))), (float) (r),
					(int) (6), (int) (9));
				nk_draw_list_path_arc_to_fast(list, (Vector2) (new Vector2((float) (b.X - r), (float) (a.Y + r))), (float) (r),
					(int) (9), (int) (12));
				nk_draw_list_path_arc_to_fast(list, (Vector2) (new Vector2((float) (b.X - r), (float) (b.Y - r))), (float) (r),
					(int) (0), (int) (3));
				nk_draw_list_path_arc_to_fast(list, (Vector2) (new Vector2((float) (a.X + r), (float) (b.Y - r))), (float) (r),
					(int) (3), (int) (6));
			}
		}

		public static void nk_draw_list_path_fill(Color color)
		{
			if (list == null) return;
			nk_draw_list_fill_poly_convex(list, (Color) (color),
				(int) (config.shape_AA));
			nk_draw_list_path_clear(list);
		}

		public static void nk_draw_list_path_stroke(Color color, int closed, float thickness)
		{
			if (list == null) return;
			nk_draw_list_stroke_poly_line(list, (Color) (color), (int) (closed),
				(float) (thickness), (int) (config.line_AA));
			nk_draw_list_path_clear(list);
		}

		public static void nk_draw_list_fill_rect(RectangleF rect, Color col, float rounding)
		{
			if ((list == null) || (col.a == 0)) return;
			if ((list.line_AA) == (NK_ANTI_ALIASING_ON))
			{
				nk_draw_list_path_rect_to(list, (Vector2) (new Vector2((float) (rect.X), (float) (rect.Y))),
					(Vector2) (new Vector2((float) (rect.X + rect.Width), (float) (rect.Y + rect.Height))), (float) (rounding));
			}
			else
			{
				nk_draw_list_path_rect_to(list, (Vector2) (new Vector2((float) (rect.X - 0.5f), (float) (rect.Y - 0.5f))),
					(Vector2) (new Vector2((float) (rect.X + rect.Width), (float) (rect.Y + rect.Height))), (float) (rounding));
			}

			nk_draw_list_path_fill(list, (Color) (col));
		}

		public static void nk_draw_list_stroke_rect(RectangleF rect, Color col, float rounding,
			float thickness)
		{
			if ((list == null) || (col.a == 0)) return;
			if ((list.line_AA) == (NK_ANTI_ALIASING_ON))
			{
				nk_draw_list_path_rect_to(list, (Vector2) (new Vector2((float) (rect.X), (float) (rect.Y))),
					(Vector2) (new Vector2((float) (rect.X + rect.Width), (float) (rect.Y + rect.Height))), (float) (rounding));
			}
			else
			{
				nk_draw_list_path_rect_to(list, (Vector2) (new Vector2((float) (rect.X - 0.5f), (float) (rect.Y - 0.5f))),
					(Vector2) (new Vector2((float) (rect.X + rect.Width), (float) (rect.Y + rect.Height))), (float) (rounding));
			}

			nk_draw_list_path_stroke(list, (Color) (col), (int) (NK_STROKE_CLOSED), (float) (thickness));
		}

		public static void nk_draw_list_fill_rect_multi_color(RectangleF rect, Color left, Color top,
			Color right, Color bottom)
		{
			Nuklear.Colorf col_left = new Nuklear.Colorf();
			Nuklear.Colorf col_top = new Nuklear.Colorf();
			Nuklear.Colorf col_right = new Nuklear.Colorf();
			Nuklear.Colorf col_bottom = new Nuklear.Colorf();
			ushort index;
			Utility.color_fv(&col_left.r, (Color) (left));
			Utility.color_fv(&col_right.r, (Color) (right));
			Utility.color_fv(&col_top.r, (Color) (top));
			Utility.color_fv(&col_bottom.r, (Color) (bottom));
			if (list == null) return;
			nk_draw_list_push_image(list, (Nuklear.nk_handle) (config._null_.texture));
			index = ((ushort) (list.vertex_offset));
			int vtxStart = list.vertices.Count;
			list.vertices.addToEnd((int) (4*config.vertex_size));
			int idxStart = list.addElements(6);

			fixed (byte* vtx2 = list.vertices.Data)
			{
				void* vtx = (void*) (vtx2 + vtxStart);
				fixed (ushort* ids2 = list.elements.Data)
				{
					ushort* idx = ids2 + idxStart;
					if ((vtx == null) || (idx == null)) return;
					idx[0] = ((ushort) (index + 0));
					idx[1] = ((ushort) (index + 1));
					idx[2] = ((ushort) (index + 2));
					idx[3] = ((ushort) (index + 0));
					idx[4] = ((ushort) (index + 2));
					idx[5] = ((ushort) (index + 3));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (new Vector2((float) (rect.X), (float) (rect.Y))),
						(Vector2) (config._null_.uv), (Nuklear.Colorf) (col_left));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (new Vector2((float) (rect.X + rect.Width), (float) (rect.Y))),
						(Vector2) (config._null_.uv), (Nuklear.Colorf) (col_top));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (new Vector2((float) (rect.X + rect.Width), (float) (rect.Y + rect.Height))),
						(Vector2) (config._null_.uv), (Nuklear.Colorf) (col_right));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (new Vector2((float) (rect.X), (float) (rect.Y + rect.Height))),
						(Vector2) (config._null_.uv), (Nuklear.Colorf) (col_bottom));
				}
			}
		}

		public static void nk_draw_list_fill_triangle(Vector2 a, Vector2 b, Vector2 c, Color col)
		{
			if ((list == null) || (col.a == 0)) return;
			nk_draw_list_path_line_to(list, (Vector2) (a));
			nk_draw_list_path_line_to(list, (Vector2) (b));
			nk_draw_list_path_line_to(list, (Vector2) (c));
			nk_draw_list_path_fill(list, (Color) (col));
		}

		public static void nk_draw_list_fill_circle(Vector2 center, float radius, Color col, uint segs)
		{
			float a_max;
			if ((list == null) || (col.a == 0)) return;
			a_max = (float) (3.141592654f*2.0f*((float) (segs) - 1.0f)/(float) (segs));
			nk_draw_list_path_arc_to(list, (Vector2) (center), (float) (radius), (float) (0.0f), (float) (a_max), (uint) (segs));
			nk_draw_list_path_fill(list, (Color) (col));
		}

		public static void nk_draw_list_push_rect_uv(Vector2 a, Vector2 c, Vector2 uva, Vector2 uvc,
			Color color)
		{
			Vector2 uvb = new Vector2();
			Vector2 uvd = new Vector2();
			Vector2 b = new Vector2();
			Vector2 d = new Vector2();
			Nuklear.Colorf col = new Nuklear.Colorf();
			ushort index;
			if (list == null) return;
			Utility.color_fv(&col.r, (Color) (color));
			uvb = (Vector2) (new Vector2((float) (uvc.X), (float) (uva.Y)));
			uvd = (Vector2) (new Vector2((float) (uva.X), (float) (uvc.Y)));
			b = (Vector2) (new Vector2((float) (c.X), (float) (a.Y)));
			d = (Vector2) (new Vector2((float) (a.X), (float) (c.Y)));
			index = ((ushort) (list.vertex_offset));
			int vtxStart = list.vertices.Count;
			list.vertices.addToEnd((int) (4*config.vertex_size));
			int idxStart = list.addElements(6);

			fixed (byte* vtx2 = list.vertices.Data)
			{
				void* vtx = (void*) (vtx2 + vtxStart);
				fixed (ushort* ids2 = list.elements.Data)
				{
					ushort* idx = ids2 + idxStart;
					if ((vtx == null) || (idx == null)) return;
					idx[0] = ((ushort) (index + 0));
					idx[1] = ((ushort) (index + 1));
					idx[2] = ((ushort) (index + 2));
					idx[3] = ((ushort) (index + 0));
					idx[4] = ((ushort) (index + 2));
					idx[5] = ((ushort) (index + 3));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (a), (Vector2) (uva), (Nuklear.Colorf) (col));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (b), (Vector2) (uvb), (Nuklear.Colorf) (col));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (c), (Vector2) (uvc), (Nuklear.Colorf) (col));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (d), (Vector2) (uvd), (Nuklear.Colorf) (col));
				}
			}
		}

		public static void nk_draw_list_add_image(Nuklear.nk_image texture, RectangleF rect, Color color)
		{
			if (list == null) return;
			nk_draw_list_push_image(list, (Nuklear.nk_handle) (texture.handle));
			if ((nk_image_is_subimage(texture)) != 0)
			{
				Vector2* uv = stackalloc Vector2[2];
				uv[0].X = (float) ((float) (texture.region[0])/(float) (texture.Width));
				uv[0].Y = (float) ((float) (texture.region[1])/(float) (texture.Height));
				uv[1].X = (float) ((float) (texture.region[0] + texture.region[2])/(float) (texture.Width));
				uv[1].Y = (float) ((float) (texture.region[1] + texture.region[3])/(float) (texture.Height));
				nk_draw_list_push_rect_uv(list, (Vector2) (new Vector2((float) (rect.X), (float) (rect.Y))),
					(Vector2) (new Vector2((float) (rect.X + rect.Width), (float) (rect.Y + rect.Height))), (Vector2) (uv[0]), (Vector2) (uv[1]),
					(Color) (color));
			}
			else
				nk_draw_list_push_rect_uv(list, (Vector2) (new Vector2((float) (rect.X), (float) (rect.Y))),
					(Vector2) (new Vector2((float) (rect.X + rect.Width), (float) (rect.Y + rect.Height))),
					(Vector2) (new Vector2((float) (0.0f), (float) (0.0f))), (Vector2) (new Vector2((float) (1.0f), (float) (1.0f))),
					(Color) (color));
		}

		public static void nk_draw_list_add_text(Nuklear.nk_user_font font, RectangleF rect, char* text, int len,
			float font_height, Color fg)
		{
			float x = (float) (0);
			int text_len = (int) (0);
			char unicode = (char) 0;
			char next = (char) (0);
			int glyph_len = (int) (0);
			int next_glyph_len = (int) (0);
			Nuklear.nk_user_font_glyph g = new Nuklear.nk_user_font_glyph();
			if (((list == null) || (len == 0)) || (text == null)) return;
			if (
				!(!(((((list.clip_rect.X) > (rect.X + rect.Width)) || ((list.clip_rect.X + list.clip_rect.Width) < (rect.X))) ||
				     ((list.clip_rect.Y) > (rect.Y + rect.Height))) || ((list.clip_rect.Y + list.clip_rect.Height) < (rect.Y))))) return;
			nk_draw_list_push_image(list, (Nuklear.nk_handle) (font.texture));
			x = (float) (rect.X);
			glyph_len = (int) (nk_utf_decode(text, &unicode, (int) (len)));
			if (glyph_len == 0) return;
			fg.a = ((byte) ((float) (fg.a)*config.global_alpha));
			while (((text_len) < (len)) && ((glyph_len) != 0))
			{
				float gx;
				float gy;
				float gh;
				float gw;
				float char_width = (float) (0);
				if ((unicode) == (0xFFFD)) break;
				next_glyph_len = (int) (nk_utf_decode(text + text_len + glyph_len, &next, (int) (len - text_len)));
				font.query((Nuklear.nk_handle) (font.userdata), (float) (font_height), &g, unicode, (next == 0xFFFD) ? '\0' : next);
				gx = (float) (x + g.offset.X);
				gy = (float) (rect.Y + g.offset.Y);
				gw = (float) (g.Widthidth);
				gh = (float) (g.Height);
				char_width = (float) (g.Xadvance);
				nk_draw_list_push_rect_uv(list, (Vector2) (new Vector2((float) (gx), (float) (gy))),
					(Vector2) (new Vector2((float) (gx + gw), (float) (gy + gh))), new Vector2(g.uv_x[0], g.uv_y[0]),
					new Vector2(g.uv_x[1], g.uv_y[1]), (Color) (fg));
				text_len += (int) (glyph_len);
				x += (float) (char_width);
				glyph_len = (int) (next_glyph_len);
				unicode = (char) (next);
			}
		}		

		public void nk_stroke_line(float x0, float y0, float x1, float y1, float line_thickness, Color c)
		{
			if ((b == null) || (line_thickness <= 0)) return;
			if ((list == null) || (col.a == 0)) return;
			if ((list.line_AA) == (NK_ANTI_ALIASING_ON))
			{
				nk_draw_list_path_line_to(list, (Vector2) (a));
				nk_draw_list_path_line_to(list, (Vector2) (b));
			}
			else
			{
				nk_draw_list_path_line_to(list,
					(Vector2)
					(new Vector2((float) ((a).X - (new Vector2((float) (0.5f), (float) (0.5f))).X),
						(float) ((a).Y - (new Vector2((float) (0.5f), (float) (0.5f))).Y))));
				nk_draw_list_path_line_to(list,
					(Vector2)
					(new Vector2((float) ((b).X - (new Vector2((float) (0.5f), (float) (0.5f))).X),
						(float) ((b).Y - (new Vector2((float) (0.5f), (float) (0.5f))).Y))));
			}

			nk_draw_list_path_stroke(list, (Color) (col), (int) (NK_STROKE_OPEN), (float) (thickness));
		}

		public void nk_stroke_rect(Nuklear.nk_command_buffer b, RectangleF rect, float rounding, float line_thickness,
			Color c)
		{
			nk_command_rect cmd;
			if (((((b == null) || ((c.a) == (0))) || ((rect.Width) == (0))) || ((rect.Height) == (0))) || (line_thickness <= 0))
				return;
			if ((b.use_clipping) != 0)
			{
				if (
					!(!(((((b.clip.X) > (rect.X + rect.Width)) || ((b.clip.X + b.clip.Width) < (rect.X))) ||
					     ((b.clip.Y) > (rect.Y + rect.Height))) || ((b.clip.Y + b.clip.Height) < (rect.Y))))) return;
			}

			cmd = (nk_command_rect) (nk_command_buffer_push(b, (int) (NK_COMMAND_RECT)));
			if (cmd == null) return;
			cmd.rounding = ((ushort) (rounding));
			cmd.line_thickness = ((ushort) (line_thickness));
			cmd.X = ((short) (rect.X));
			cmd.Y = ((short) (rect.Y));
			cmd.Width = ((ushort) ((0) < (rect.Width) ? (rect.Width) : (0)));
			cmd.Height = ((ushort) ((0) < (rect.Height) ? (rect.Height) : (0)));
			cmd.color = (Color) (c);
		}

		public void nk_fill_rect(Nuklear.nk_command_buffer b, RectangleF rect, float rounding, Color c)
		{
			nk_command_rect_filled cmd;
			if ((((b == null) || ((c.a) == (0))) || ((rect.Width) == (0))) || ((rect.Height) == (0))) return;
			if ((b.use_clipping) != 0)
			{
				if (
					!(!(((((b.clip.X) > (rect.X + rect.Width)) || ((b.clip.X + b.clip.Width) < (rect.X))) ||
					     ((b.clip.Y) > (rect.Y + rect.Height))) || ((b.clip.Y + b.clip.Height) < (rect.Y))))) return;
			}

			cmd = (nk_command_rect_filled) (nk_command_buffer_push(b, (int) (NK_COMMAND_RECT_FILLED)));
			if (cmd == null) return;
			cmd.rounding = ((ushort) (rounding));
			cmd.X = ((short) (rect.X));
			cmd.Y = ((short) (rect.Y));
			cmd.Width = ((ushort) ((0) < (rect.Width) ? (rect.Width) : (0)));
			cmd.Height = ((ushort) ((0) < (rect.Height) ? (rect.Height) : (0)));
			cmd.color = (Color) (c);
		}

		public void nk_fill_rect_multi_color(Nuklear.nk_command_buffer b, RectangleF rect, Color left, Color top,
			Color right, Color bottom)
		{
			nk_command_rect_multi_color cmd;
			if (((b == null) || ((rect.Width) == (0))) || ((rect.Height) == (0))) return;
			if ((b.use_clipping) != 0)
			{
				if (
					!(!(((((b.clip.X) > (rect.X + rect.Width)) || ((b.clip.X + b.clip.Width) < (rect.X))) ||
					     ((b.clip.Y) > (rect.Y + rect.Height))) || ((b.clip.Y + b.clip.Height) < (rect.Y))))) return;
			}

			cmd = (nk_command_rect_multi_color) (nk_command_buffer_push(b, (int) (NK_COMMAND_RECT_MULTI_COLOR)));
			if (cmd == null) return;
			cmd.X = ((short) (rect.X));
			cmd.Y = ((short) (rect.Y));
			cmd.Width = ((ushort) ((0) < (rect.Width) ? (rect.Width) : (0)));
			cmd.Height = ((ushort) ((0) < (rect.Height) ? (rect.Height) : (0)));
			cmd.left = (Color) (left);
			cmd.top = (Color) (top);
			cmd.right = (Color) (right);
			cmd.bottom = (Color) (bottom);
		}

		public void nk_fill_circle(Nuklear.nk_command_buffer b, RectangleF r, Color c)
		{
			nk_command_circle_filled cmd;
			if ((((b == null) || ((c.a) == (0))) || ((r.Width) == (0))) || ((r.Height) == (0))) return;
			if ((b.use_clipping) != 0)
			{
				if (
					!(!(((((b.clip.X) > (r.X + r.Width)) || ((b.clip.X + b.clip.Width) < (r.X))) || ((b.clip.Y) > (r.Y + r.Height))) ||
					    ((b.clip.Y + b.clip.Height) < (r.Y))))) return;
			}

			cmd = (nk_command_circle_filled) (nk_command_buffer_push(b, (int) (NK_COMMAND_CIRCLE_FILLED)));
			if (cmd == null) return;
			cmd.X = ((short) (r.X));
			cmd.Y = ((short) (r.Y));
			cmd.Width = ((ushort) ((r.Width) < (0) ? (0) : (r.Width)));
			cmd.Height = ((ushort) ((r.Height) < (0) ? (0) : (r.Height)));
			cmd.color = (Color) (c);
		}

		public void nk_fill_triangle(Nuklear.nk_command_buffer b, float x0, float y0, float x1, float y1, float x2,
			float y2, Color c)
		{
			nk_command_triangle_filled cmd;
			if ((b == null) || ((c.a) == (0))) return;
			if (b == null) return;
			if ((b.use_clipping) != 0)
			{
				if (
					((!((((b.clip.X) <= (x0)) && ((x0) < (b.clip.X + b.clip.Width))) &&
					    (((b.clip.Y) <= (y0)) && ((y0) < (b.clip.Y + b.clip.Height))))) &&
					 (!((((b.clip.X) <= (x1)) && ((x1) < (b.clip.X + b.clip.Width))) &&
					    (((b.clip.Y) <= (y1)) && ((y1) < (b.clip.Y + b.clip.Height)))))) &&
					(!((((b.clip.X) <= (x2)) && ((x2) < (b.clip.X + b.clip.Width))) &&
					   (((b.clip.Y) <= (y2)) && ((y2) < (b.clip.Y + b.clip.Height)))))) return;
			}

			cmd = (nk_command_triangle_filled) (nk_command_buffer_push(b, (int) (NK_COMMAND_TRIANGLE_FILLED)));
			if (cmd == null) return;
			cmd.a.X = ((short) (x0));
			cmd.a.Y = ((short) (y0));
			cmd.b.X = ((short) (x1));
			cmd.b.Y = ((short) (y1));
			cmd.c.X = ((short) (x2));
			cmd.c.Y = ((short) (y2));
			cmd.color = (Color) (c);
		}

		public void nk_draw_image(Nuklear.nk_command_buffer b, RectangleF r, Nuklear.nk_image img, Color col)
		{
			nk_command_image cmd;
			if (b == null) return;
			if ((b.use_clipping) != 0)
			{
				if ((((b.clip.Width) == (0)) || ((b.clip.Height) == (0))) ||
				    (!(!(((((b.clip.X) > (r.X + r.Width)) || ((b.clip.X + b.clip.Width) < (r.X))) || ((b.clip.Y) > (r.Y + r.Height))) ||
				         ((b.clip.Y + b.clip.Height) < (r.Y)))))) return;
			}

			cmd = (nk_command_image) (nk_command_buffer_push(b, (int) (NK_COMMAND_IMAGE)));
			if (cmd == null) return;
			cmd.X = ((short) (r.X));
			cmd.Y = ((short) (r.Y));
			cmd.Width = ((ushort) ((0) < (r.Width) ? (r.Width) : (0)));
			cmd.Height = ((ushort) ((0) < (r.Height) ? (r.Height) : (0)));
			cmd.img = (Nuklear.nk_image) (img);
			cmd.col = (Color) (col);
		}

		public void nk_draw_text(Nuklear.nk_command_buffer b, RectangleF r, char* _string_, int length, Nuklear.nk_user_font font,
			Color bg, Color fg)
		{
			float text_width = (float) (0);
			nk_command_text cmd;
			if ((((b == null) || (_string_ == null)) || (length == 0)) || (((bg.a) == (0)) && ((fg.a) == (0)))) return;
			if ((b.use_clipping) != 0)
			{
				if ((((b.clip.Width) == (0)) || ((b.clip.Height) == (0))) ||
				    (!(!(((((b.clip.X) > (r.X + r.Width)) || ((b.clip.X + b.clip.Width) < (r.X))) || ((b.clip.Y) > (r.Y + r.Height))) ||
				         ((b.clip.Y + b.clip.Height) < (r.Y)))))) return;
			}

			text_width =
				(float) (font.Widthidth((Nuklear.nk_handle) (font.userdata), (float) (font.Height), _string_, (int) (length)));
			if ((text_width) > (r.Width))
			{
				int glyphs = (int) (0);
				float txt_width = (float) (text_width);
				length =
					(int)
						(nk_text_clamp(font, _string_, (int) (length), (float) (r.Width), &glyphs, &txt_width, null,
							(int) (0)));
			}

			if (length == 0) return;
			cmd = (nk_command_text) (nk_command_buffer_push(b, (int) (NK_COMMAND_TEXT)));
			if (cmd == null) return;
			cmd.X = ((short) (r.X));
			cmd.Y = ((short) (r.Y));
			cmd.Width = ((ushort) (r.Width));
			cmd.Height = ((ushort) (r.Height));
			cmd.background = (Color) (bg);
			cmd.foreground = (Color) (fg);
			cmd.font = font;
			cmd.length = (int) (length);
			cmd.Height = (float) (font.Height);
			cmd._string_ = (char*)CRuntime.malloc(length);
			CRuntime.memcpy((void*) cmd._string_, _string_, length*sizeof (char));
			cmd._string_[length] = ('\0');
		}        
    }
}