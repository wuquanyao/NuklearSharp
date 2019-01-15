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
        private readonly Stack<Rectangle> _scissors = new Stack<Rectangle>();
        private readonly List<Vector2> _points = new List<Vector2>();
        private readonly Nuklear.nk_convert_config config;

        public Batch(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException();
            }

            _graphicsDevice = graphicsDevice;
        }
        
		public void nk_push_scissor(Rectangle scissor)
		{
			if (_scissors.Count == 0)
			{
				if (scissor.Width < 1 || scissor.Height < 1) return;
// 				GL20.EnableScissors(true);
			}
			else
			{
				// merge scissors
				var parent = _scissors.Peek();
				scissor = Rectangle.Intersect(parent, scissor);
				if (scissor.Width == 0 || scissor.Height == 0)
				{
					return;
				}
			}
			_scissors.Push(scissor);

			_graphicsDevice.ScissorRectangle = scissor;
		}
		
		public static void nk_draw_list_push_image(Nuklear.nk_handle texture)
		{
			if (list == null) return;
			if (list.buffer.Count == 0)
			{
				nk_draw_list_push_command(list, (Rectangle) (nk_null_rect), (Nuklear.nk_handle) (texture));
			}
			else
			{
				fixed (nk_draw_command* prev2 = list.buffer.Data)
				{
					nk_draw_command* prev = prev2 + list.buffer.Count - 1;
					if ((prev->elem_count) == (0))
					{
						prev->texture = (Nuklear.nk_handle) (texture);
					}
					else if (prev->texture.id != texture.id)
						nk_draw_list_push_command(list, (Rectangle) (prev->clip_rect), (Nuklear.nk_handle) (texture));
				}
			}

		}

		public void nk_draw_list_stroke_poly_line(Color color, int closed, float thickness, int aliasing)
		{
			ulong count;
			int thick_line;

			var points_count = _points.Count;
			Nuklear.Colorf col = new Nuklear.Colorf();
			Nuklear.Colorf col_trans = new Nuklear.Colorf();
			if (points_count < 2) return;
			
			color.A = ((byte) ((float) (color.A)*config.global_alpha));
			count = (ulong) (points_count);
			if (closed == 0) count = (ulong) (points_count - 1);
			thick_line = (int) ((thickness) > (1.0f) ? 1 : 0);
			color.A = ((byte) ((float) (color.A)*config.global_alpha));
			Color_fv(&col.R, (Color) (color));
			col_trans = (Nuklear.Colorf) (col);
			col_trans.a = (float) (0);
			if ((aliasing) == (NK_ANTI_ALIASING_ON))
			{
				float AA_SIZE = (float) (1.0f);
				ulong i1 = (ulong) (0);
				ulong index = (ulong) (list.vertex_offset);
				ulong idx_count = (ulong) ((thick_line) != 0 ? (count*18) : (count*12));
				ulong vtx_count = (ulong) ((thick_line) != 0 ? (points_count*4) : (points_count*3));


				int vtxStart = list.vertices.Count;
				list.vertices.addToEnd((int)(vtx_count * config.vertex_size));
				int idxStart = list.addElements((int)idx_count);

				Vector2* temp;
				int points_total = (int) (((thick_line) != 0 ? 5 : 3)*(int) points_count);

				int normalsStart = list.normals.Count;
				list.normals.addToEnd(points_total);

				fixed (Vector2* normals2 = list.normals.Data)
				{
					Vector2* normals = normals2 + normalsStart;
					fixed (byte* vtx2 = list.vertices.Data)
					{
						void* vtx = (void*) (vtx2 + vtxStart);
						fixed (ushort* ids2 = list.elements.Data)
						{
							ushort* ids = ids2 + idxStart;
							if (normals == null) return;
							temp = normals + points_count;
							for (i1 = (ulong) (0); (i1) < (count); ++i1)
							{
								ulong i2 = (ulong) (((i1 + 1) == (ulong) (points_count)) ? 0 : (i1 + 1));
								Vector2 diff =
									(Vector2)
										(Vector2_((float) ((_points[i2]).x - (_points[i1]).x),
											(float) ((_points[i2]).y - (_points[i1]).y)));
								float len;
								len = (float) ((diff).x*(diff).x + (diff).y*(diff).y);
								if (len != 0.0f) len = (float) (nk_inv_sqrt((float) (len)));
								else len = (float) (1.0f);
								diff = (Vector2) (Vector2_((float) ((diff).x*(len)), (float) ((diff).y*(len))));
								normals[i1].x = (float) (diff.y);
								normals[i1].y = (float) (-diff.x);
							}
							if (closed == 0) normals[points_count - 1] = (Vector2) (normals[points_count - 2]);
							if (thick_line == 0)
							{
								ulong idx1;
								ulong i;
								if (closed == 0)
								{
									Vector2 d = new Vector2();
									temp[0] =
										(Vector2)
											(Vector2_(
												(float)
													((_points[0]).x + (Vector2_((float) ((normals[0]).x*(AA_SIZE)), (float) ((normals[0]).y*(AA_SIZE)))).x),
												(float)
													((_points[0]).y + (Vector2_((float) ((normals[0]).x*(AA_SIZE)), (float) ((normals[0]).y*(AA_SIZE)))).y)));
									temp[1] =
										(Vector2)
											(Vector2_(
												(float)
													((_points[0]).x - (Vector2_((float) ((normals[0]).x*(AA_SIZE)), (float) ((normals[0]).y*(AA_SIZE)))).x),
												(float)
													((_points[0]).y - (Vector2_((float) ((normals[0]).x*(AA_SIZE)), (float) ((normals[0]).y*(AA_SIZE)))).y)));
									d =
										(Vector2)
											(Vector2_((float) ((normals[points_count - 1]).x*(AA_SIZE)),
												(float) ((normals[points_count - 1]).y*(AA_SIZE))));
									temp[(points_count - 1)*2 + 0] =
										(Vector2)
											(Vector2_((float) ((_points[points_count - 1]).x + (d).x),
												(float) ((_points[points_count - 1]).y + (d).y)));
									temp[(points_count - 1)*2 + 1] =
										(Vector2)
											(Vector2_((float) ((_points[points_count - 1]).x - (d).x),
												(float) ((_points[points_count - 1]).y - (d).y)));
								}
								idx1 = (ulong) (index);
								for (i1 = (ulong) (0); (i1) < (count); i1++)
								{
									Vector2 dm = new Vector2();
									float dmr2;
									ulong i2 = (ulong) (((i1 + 1) == (points_count)) ? 0 : (i1 + 1));
									ulong idx2 = (ulong) (((i1 + 1) == (points_count)) ? index : (idx1 + 3));
									dm =
										(Vector2)
											(Vector2_(
												(float)
													((Vector2_((float) ((normals[i1]).x + (normals[i2]).x), (float) ((normals[i1]).y + (normals[i2]).y))).x*
													 (0.5f)),
												(float)
													((Vector2_((float) ((normals[i1]).x + (normals[i2]).x), (float) ((normals[i1]).y + (normals[i2]).y))).y*
													 (0.5f))));
									dmr2 = (float) (dm.x*dm.x + dm.y*dm.y);
									if ((dmr2) > (0.000001f))
									{
										float scale = (float) (1.0f/dmr2);
										scale = (float) ((100.0f) < (scale) ? (100.0f) : (scale));
										dm = (Vector2) (Vector2_((float) ((dm).x*(scale)), (float) ((dm).y*(scale))));
									}
									dm = (Vector2) (Vector2_((float) ((dm).x*(AA_SIZE)), (float) ((dm).y*(AA_SIZE))));
									temp[i2*2 + 0] =
										(Vector2) (Vector2_((float) ((_points[i2]).x + (dm).x), (float) ((_points[i2]).y + (dm).y)));
									temp[i2*2 + 1] =
										(Vector2) (Vector2_((float) ((_points[i2]).x - (dm).x), (float) ((_points[i2]).y - (dm).y)));
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
									idx1 = (ulong) (idx2);
								}
								for (i = (ulong) (0); (i) < (points_count); ++i)
								{
									Vector2 uv = (Vector2) (config._null_.uv);
									vtx = nk_draw_vertex(vtx, config, (Vector2) (_points[i]), (Vector2) (uv), (Nuklear.Colorf) (col));
									vtx = nk_draw_vertex(vtx, config, (Vector2) (temp[i*2 + 0]), (Vector2) (uv), (Nuklear.Colorf) (col_trans));
									vtx = nk_draw_vertex(vtx, config, (Vector2) (temp[i*2 + 1]), (Vector2) (uv), (Nuklear.Colorf) (col_trans));
								}
							}
							else
							{
								ulong idx1;
								ulong i;
								float half_inner_thickness = (float) ((thickness - AA_SIZE)*0.5f);
								if (closed == 0)
								{
									Vector2 d1 =
										(Vector2)
											(Vector2_((float) ((normals[0]).x*(half_inner_thickness + AA_SIZE)),
												(float) ((normals[0]).y*(half_inner_thickness + AA_SIZE))));
									Vector2 d2 =
										(Vector2)
											(Vector2_((float) ((normals[0]).x*(half_inner_thickness)), (float) ((normals[0]).y*(half_inner_thickness))));
									temp[0] = (Vector2) (Vector2_((float) ((_points[0]).x + (d1).x), (float) ((_points[0]).y + (d1).y)));
									temp[1] = (Vector2) (Vector2_((float) ((_points[0]).x + (d2).x), (float) ((_points[0]).y + (d2).y)));
									temp[2] = (Vector2) (Vector2_((float) ((_points[0]).x - (d2).x), (float) ((_points[0]).y - (d2).y)));
									temp[3] = (Vector2) (Vector2_((float) ((_points[0]).x - (d1).x), (float) ((_points[0]).y - (d1).y)));
									d1 =
										(Vector2)
											(Vector2_((float) ((normals[points_count - 1]).x*(half_inner_thickness + AA_SIZE)),
												(float) ((normals[points_count - 1]).y*(half_inner_thickness + AA_SIZE))));
									d2 =
										(Vector2)
											(Vector2_((float) ((normals[points_count - 1]).x*(half_inner_thickness)),
												(float) ((normals[points_count - 1]).y*(half_inner_thickness))));
									temp[(points_count - 1)*4 + 0] =
										(Vector2)
											(Vector2_((float) ((_points[points_count - 1]).x + (d1).x),
												(float) ((_points[points_count - 1]).y + (d1).y)));
									temp[(points_count - 1)*4 + 1] =
										(Vector2)
											(Vector2_((float) ((_points[points_count - 1]).x + (d2).x),
												(float) ((_points[points_count - 1]).y + (d2).y)));
									temp[(points_count - 1)*4 + 2] =
										(Vector2)
											(Vector2_((float) ((_points[points_count - 1]).x - (d2).x),
												(float) ((_points[points_count - 1]).y - (d2).y)));
									temp[(points_count - 1)*4 + 3] =
										(Vector2)
											(Vector2_((float) ((_points[points_count - 1]).x - (d1).x),
												(float) ((_points[points_count - 1]).y - (d1).y)));
								}
								idx1 = (ulong) (index);
								for (i1 = (ulong) (0); (i1) < (count); ++i1)
								{
									Vector2 dm_out = new Vector2();
									Vector2 dm_in = new Vector2();
									ulong i2 = (ulong) (((i1 + 1) == (points_count)) ? 0 : (i1 + 1));
									ulong idx2 = (ulong) (((i1 + 1) == (points_count)) ? index : (idx1 + 4));
									Vector2 dm =
										(Vector2)
											(Vector2_(
												(float)
													((Vector2_((float) ((normals[i1]).x + (normals[i2]).x), (float) ((normals[i1]).y + (normals[i2]).y))).x*
													 (0.5f)),
												(float)
													((Vector2_((float) ((normals[i1]).x + (normals[i2]).x), (float) ((normals[i1]).y + (normals[i2]).y))).y*
													 (0.5f))));
									float dmr2 = (float) (dm.x*dm.x + dm.y*dm.y);
									if ((dmr2) > (0.000001f))
									{
										float scale = (float) (1.0f/dmr2);
										scale = (float) ((100.0f) < (scale) ? (100.0f) : (scale));
										dm = (Vector2) (Vector2_((float) ((dm).x*(scale)), (float) ((dm).y*(scale))));
									}
									dm_out =
										(Vector2)
											(Vector2_((float) ((dm).x*((half_inner_thickness) + AA_SIZE)),
												(float) ((dm).y*((half_inner_thickness) + AA_SIZE))));
									dm_in = (Vector2) (Vector2_((float) ((dm).x*(half_inner_thickness)), (float) ((dm).y*(half_inner_thickness))));
									temp[i2*4 + 0] =
										(Vector2) (Vector2_((float) ((_points[i2]).x + (dm_out).x), (float) ((_points[i2]).y + (dm_out).y)));
									temp[i2*4 + 1] =
										(Vector2) (Vector2_((float) ((_points[i2]).x + (dm_in).x), (float) ((_points[i2]).y + (dm_in).y)));
									temp[i2*4 + 2] =
										(Vector2) (Vector2_((float) ((_points[i2]).x - (dm_in).x), (float) ((_points[i2]).y - (dm_in).y)));
									temp[i2*4 + 3] =
										(Vector2) (Vector2_((float) ((_points[i2]).x - (dm_out).x), (float) ((_points[i2]).y - (dm_out).y)));
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
									idx1 = (ulong) (idx2);
								}
								for (i = (ulong) (0); (i) < (points_count); ++i)
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
					}
				}
			}
			else
			{
				ulong i1 = (ulong) (0);
				ulong idx = (ulong) (list.vertex_offset);
				ulong idx_count = (ulong) (count*6);
				ulong vtx_count = (ulong) (count*4);

				int vtxStart = list.vertices.Count;
				list.vertices.addToEnd((int) (vtx_count*config.vertex_size));
				int idxStart = list.addElements((int)idx_count);

				fixed (byte* vtx2 = list.vertices.Data)
				{
					void* vtx = (void*) (vtx2 + vtxStart);
					fixed (ushort* ids2 = list.elements.Data)
					{
						ushort* ids = ids2 + idxStart;

						for (i1 = (ulong) (0); (i1) < (count); ++i1)
						{
							float dx;
							float dy;
							Vector2 uv = (Vector2) (config._null_.uv);
							ulong i2 = (ulong) (((i1 + 1) == (points_count)) ? 0 : i1 + 1);
							Vector2 p1 = (Vector2) (_points[i1]);
							Vector2 p2 = (Vector2) (_points[i2]);
							Vector2 diff = (Vector2) (Vector2_((float) ((p2).x - (p1).x), (float) ((p2).y - (p1).y)));
							float len;
							len = (float) ((diff).x*(diff).x + (diff).y*(diff).y);
							if (len != 0.0f) len = (float) (nk_inv_sqrt((float) (len)));
							else len = (float) (1.0f);
							diff = (Vector2) (Vector2_((float) ((diff).x*(len)), (float) ((diff).y*(len))));
							dx = (float) (diff.x*(thickness*0.5f));
							dy = (float) (diff.y*(thickness*0.5f));
							vtx = nk_draw_vertex(vtx, config, (Vector2) (Vector2_((float) (p1.x + dy), (float) (p1.y - dx))),
								(Vector2) (uv), (Nuklear.Colorf) (col));
							vtx = nk_draw_vertex(vtx, config, (Vector2) (Vector2_((float) (p2.x + dy), (float) (p2.y - dx))),
								(Vector2) (uv), (Nuklear.Colorf) (col));
							vtx = nk_draw_vertex(vtx, config, (Vector2) (Vector2_((float) (p2.x - dy), (float) (p2.y + dx))),
								(Vector2) (uv), (Nuklear.Colorf) (col));
							vtx = nk_draw_vertex(vtx, config, (Vector2) (Vector2_((float) (p1.x - dy), (float) (p1.y + dx))),
								(Vector2) (uv), (Nuklear.Colorf) (col));
							ids[0] = ((ushort) (idx + 0));
							ids[1] = ((ushort) (idx + 1));
							ids[2] = ((ushort) (idx + 2));
							ids[3] = ((ushort) (idx + 0));
							ids[4] = ((ushort) (idx + 2));
							ids[5] = ((ushort) (idx + 3));
							ids += 6;
							idx += (ulong) (4);
						}
					}
				}
			}
		}

		public static void nk_draw_list_fill_poly_convex(Color color, int aliasing)
		{
			Nuklear.Colorf col = new Nuklear.Colorf();
			Nuklear.Colorf col_trans = new Nuklear.Colorf();

			var points_count = (ulong) _points.Count;
			if ((list == null) || ((_points.Count) < (3))) return;
			color.A = ((byte) ((float) (color.A)*config.global_alpha));
			Color_fv(&col.r, (Color) (color));
			col_trans = (Nuklear.Colorf) (col);
			col_trans.a = (float) (0);
			if ((aliasing) == (NK_ANTI_ALIASING_ON))
			{
				ulong i = (ulong) (0);
				ulong i0 = (ulong) (0);
				ulong i1 = (ulong) (0);
				float AA_SIZE = (float) (1.0f);
				ulong index = (ulong) (list.vertex_offset);
				ulong idx_count = (ulong) ((points_count - 2)*3 + points_count*6);
				ulong vtx_count = (ulong) (points_count*2);

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

							for (i = (ulong) (2); (i) < (points_count); i++)
							{
								ids[0] = ((ushort) (vtx_inner_idx));
								ids[1] = ((ushort) (vtx_inner_idx + ((i - 1) << 1)));
								ids[2] = ((ushort) (vtx_inner_idx + (i << 1)));
								ids += 3;
							}
							for (i0 = (ulong) (points_count - 1) , i1 = (ulong) (0); (i1) < (points_count); i0 = (ulong) (i1++))
							{
								Vector2 p0 = (Vector2) (_points[i0]);
								Vector2 p1 = (Vector2) (_points[i1]);
								Vector2 diff = (Vector2) (Vector2_((float) ((p1).x - (p0).x), (float) ((p1).y - (p0).y)));
								float len = (float) ((diff).x*(diff).x + (diff).y*(diff).y);
								if (len != 0.0f) len = (float) (nk_inv_sqrt((float) (len)));
								else len = (float) (1.0f);
								diff = (Vector2) (Vector2_((float) ((diff).x*(len)), (float) ((diff).y*(len))));
								normals[i0].x = (float) (diff.y);
								normals[i0].y = (float) (-diff.x);
							}
							for (i0 = (ulong) (points_count - 1) , i1 = (ulong) (0); (i1) < (points_count); i0 = (ulong) (i1++))
							{
								Vector2 uv = (Vector2) (config._null_.uv);
								Vector2 n0 = (Vector2) (normals[i0]);
								Vector2 n1 = (Vector2) (normals[i1]);
								Vector2 dm =
									(Vector2)
										(Vector2_((float) ((Vector2_((float) ((n0).x + (n1).x), (float) ((n0).y + (n1).y))).x*(0.5f)),
											(float) ((Vector2_((float) ((n0).x + (n1).x), (float) ((n0).y + (n1).y))).y*(0.5f))));
								float dmr2 = (float) (dm.x*dm.x + dm.y*dm.y);
								if ((dmr2) > (0.000001f))
								{
									float scale = (float) (1.0f/dmr2);
									scale = (float) ((scale) < (100.0f) ? (scale) : (100.0f));
									dm = (Vector2) (Vector2_((float) ((dm).x*(scale)), (float) ((dm).y*(scale))));
								}
								dm = (Vector2) (Vector2_((float) ((dm).x*(AA_SIZE*0.5f)), (float) ((dm).y*(AA_SIZE*0.5f))));
								vtx = nk_draw_vertex(vtx, config,
									(Vector2) (Vector2_((float) ((_points[i1]).x - (dm).x), (float) ((_points[i1]).y - (dm).y))),
									(Vector2) (uv),
									(Nuklear.Colorf) (col));
								vtx = nk_draw_vertex(vtx, config,
									(Vector2) (Vector2_((float) ((_points[i1]).x + (dm).x), (float) ((_points[i1]).y + (dm).y))),
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
				ulong i = (ulong) (0);
				ulong index = (ulong) (list.vertex_offset);
				ulong idx_count = (ulong) ((points_count - 2)*3);
				ulong vtx_count = (ulong) (points_count);
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
						for (i = (ulong) (0); (i) < (vtx_count); ++i)
						{
							vtx = nk_draw_vertex(vtx, config, (Vector2) (_points[i]), (Vector2) (config._null_.uv),
								(Nuklear.Colorf) (col));
						}
						for (i = (ulong) (2); (i) < (points_count); ++i)
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
			if (list.buffer.Count == 0) nk_draw_list_add_clip(list, (Rectangle) (nk_null_rect));

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
					Vector2 c = (Vector2) (list.circle_vtx[(ulong) (a)%(ulong) list.circle_vtx.Length]);
					float x = (float) (center.x + c.x*radius);
					float y = (float) (center.y + c.y*radius);
					nk_draw_list_path_line_to(list, (Vector2) (Vector2_((float) (x), (float) (y))));
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
					float x = (float) (center.x + cx);
					float y = (float) (center.y + cy);
					nk_draw_list_path_line_to(list, (Vector2) (Vector2_((float) (x), (float) (y))));
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
					((r) < (((b.x - a.x) < (0)) ? -(b.x - a.x) : (b.x - a.x))
						? (r)
						: (((b.x - a.x) < (0)) ? -(b.x - a.x) : (b.x - a.x)));
			r =
				(float)
					((r) < (((b.y - a.y) < (0)) ? -(b.y - a.y) : (b.y - a.y))
						? (r)
						: (((b.y - a.y) < (0)) ? -(b.y - a.y) : (b.y - a.y)));
			if ((r) == (0.0f))
			{
				nk_draw_list_path_line_to(list, (Vector2) (a));
				nk_draw_list_path_line_to(list, (Vector2) (Vector2_((float) (b.x), (float) (a.y))));
				nk_draw_list_path_line_to(list, (Vector2) (b));
				nk_draw_list_path_line_to(list, (Vector2) (Vector2_((float) (a.x), (float) (b.y))));
			}
			else
			{
				nk_draw_list_path_arc_to_fast(list, (Vector2) (Vector2_((float) (a.x + r), (float) (a.y + r))), (float) (r),
					(int) (6), (int) (9));
				nk_draw_list_path_arc_to_fast(list, (Vector2) (Vector2_((float) (b.x - r), (float) (a.y + r))), (float) (r),
					(int) (9), (int) (12));
				nk_draw_list_path_arc_to_fast(list, (Vector2) (Vector2_((float) (b.x - r), (float) (b.y - r))), (float) (r),
					(int) (0), (int) (3));
				nk_draw_list_path_arc_to_fast(list, (Vector2) (Vector2_((float) (a.x + r), (float) (b.y - r))), (float) (r),
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

		public static void nk_draw_list_fill_rect(Rectangle rect, Color col, float rounding)
		{
			if ((list == null) || (col.a == 0)) return;
			if ((list.line_AA) == (NK_ANTI_ALIASING_ON))
			{
				nk_draw_list_path_rect_to(list, (Vector2) (Vector2_((float) (rect.x), (float) (rect.y))),
					(Vector2) (Vector2_((float) (rect.x + rect.w), (float) (rect.y + rect.h))), (float) (rounding));
			}
			else
			{
				nk_draw_list_path_rect_to(list, (Vector2) (Vector2_((float) (rect.x - 0.5f), (float) (rect.y - 0.5f))),
					(Vector2) (Vector2_((float) (rect.x + rect.w), (float) (rect.y + rect.h))), (float) (rounding));
			}

			nk_draw_list_path_fill(list, (Color) (col));
		}

		public static void nk_draw_list_stroke_rect(Rectangle rect, Color col, float rounding,
			float thickness)
		{
			if ((list == null) || (col.a == 0)) return;
			if ((list.line_AA) == (NK_ANTI_ALIASING_ON))
			{
				nk_draw_list_path_rect_to(list, (Vector2) (Vector2_((float) (rect.x), (float) (rect.y))),
					(Vector2) (Vector2_((float) (rect.x + rect.w), (float) (rect.y + rect.h))), (float) (rounding));
			}
			else
			{
				nk_draw_list_path_rect_to(list, (Vector2) (Vector2_((float) (rect.x - 0.5f), (float) (rect.y - 0.5f))),
					(Vector2) (Vector2_((float) (rect.x + rect.w), (float) (rect.y + rect.h))), (float) (rounding));
			}

			nk_draw_list_path_stroke(list, (Color) (col), (int) (NK_STROKE_CLOSED), (float) (thickness));
		}

		public static void nk_draw_list_fill_rect_multi_color(Rectangle rect, Color left, Color top,
			Color right, Color bottom)
		{
			Nuklear.Colorf col_left = new Nuklear.Colorf();
			Nuklear.Colorf col_top = new Nuklear.Colorf();
			Nuklear.Colorf col_right = new Nuklear.Colorf();
			Nuklear.Colorf col_bottom = new Nuklear.Colorf();
			ushort index;
			Color_fv(&col_left.r, (Color) (left));
			Color_fv(&col_right.r, (Color) (right));
			Color_fv(&col_top.r, (Color) (top));
			Color_fv(&col_bottom.r, (Color) (bottom));
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
					vtx = nk_draw_vertex(vtx, config, (Vector2) (Vector2_((float) (rect.x), (float) (rect.y))),
						(Vector2) (config._null_.uv), (Nuklear.Colorf) (col_left));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (Vector2_((float) (rect.x + rect.w), (float) (rect.y))),
						(Vector2) (config._null_.uv), (Nuklear.Colorf) (col_top));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (Vector2_((float) (rect.x + rect.w), (float) (rect.y + rect.h))),
						(Vector2) (config._null_.uv), (Nuklear.Colorf) (col_right));
					vtx = nk_draw_vertex(vtx, config, (Vector2) (Vector2_((float) (rect.x), (float) (rect.y + rect.h))),
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
			Color_fv(&col.r, (Color) (color));
			uvb = (Vector2) (Vector2_((float) (uvc.x), (float) (uva.y)));
			uvd = (Vector2) (Vector2_((float) (uva.x), (float) (uvc.y)));
			b = (Vector2) (Vector2_((float) (c.x), (float) (a.y)));
			d = (Vector2) (Vector2_((float) (a.x), (float) (c.y)));
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

		public static void nk_draw_list_add_image(Nuklear.nk_image texture, Rectangle rect, Color color)
		{
			if (list == null) return;
			nk_draw_list_push_image(list, (Nuklear.nk_handle) (texture.handle));
			if ((nk_image_is_subimage(texture)) != 0)
			{
				Vector2* uv = stackalloc Vector2[2];
				uv[0].x = (float) ((float) (texture.region[0])/(float) (texture.w));
				uv[0].y = (float) ((float) (texture.region[1])/(float) (texture.h));
				uv[1].x = (float) ((float) (texture.region[0] + texture.region[2])/(float) (texture.w));
				uv[1].y = (float) ((float) (texture.region[1] + texture.region[3])/(float) (texture.h));
				nk_draw_list_push_rect_uv(list, (Vector2) (Vector2_((float) (rect.x), (float) (rect.y))),
					(Vector2) (Vector2_((float) (rect.x + rect.w), (float) (rect.y + rect.h))), (Vector2) (uv[0]), (Vector2) (uv[1]),
					(Color) (color));
			}
			else
				nk_draw_list_push_rect_uv(list, (Vector2) (Vector2_((float) (rect.x), (float) (rect.y))),
					(Vector2) (Vector2_((float) (rect.x + rect.w), (float) (rect.y + rect.h))),
					(Vector2) (Vector2_((float) (0.0f), (float) (0.0f))), (Vector2) (Vector2_((float) (1.0f), (float) (1.0f))),
					(Color) (color));
		}

		public static void nk_draw_list_add_text(Nuklear.nk_user_font font, Rectangle rect, char* text, int len,
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
				!(!(((((list.clip_rect.x) > (rect.x + rect.w)) || ((list.clip_rect.x + list.clip_rect.w) < (rect.x))) ||
				     ((list.clip_rect.y) > (rect.y + rect.h))) || ((list.clip_rect.y + list.clip_rect.h) < (rect.y))))) return;
			nk_draw_list_push_image(list, (Nuklear.nk_handle) (font.texture));
			x = (float) (rect.x);
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
				gx = (float) (x + g.offset.x);
				gy = (float) (rect.y + g.offset.y);
				gw = (float) (g.width);
				gh = (float) (g.height);
				char_width = (float) (g.xadvance);
				nk_draw_list_push_rect_uv(list, (Vector2) (Vector2_((float) (gx), (float) (gy))),
					(Vector2) (Vector2_((float) (gx + gw), (float) (gy + gh))), Vector2_(g.uv_x[0], g.uv_y[0]),
					Vector2_(g.uv_x[1], g.uv_y[1]), (Color) (fg));
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
					(Vector2_((float) ((a).x - (Vector2_((float) (0.5f), (float) (0.5f))).x),
						(float) ((a).y - (Vector2_((float) (0.5f), (float) (0.5f))).y))));
				nk_draw_list_path_line_to(list,
					(Vector2)
					(Vector2_((float) ((b).x - (Vector2_((float) (0.5f), (float) (0.5f))).x),
						(float) ((b).y - (Vector2_((float) (0.5f), (float) (0.5f))).y))));
			}

			nk_draw_list_path_stroke(list, (Color) (col), (int) (NK_STROKE_OPEN), (float) (thickness));
		}

		public void nk_stroke_rect(Nuklear.nk_command_buffer b, Rectangle rect, float rounding, float line_thickness,
			Color c)
		{
			nk_command_rect cmd;
			if (((((b == null) || ((c.a) == (0))) || ((rect.w) == (0))) || ((rect.h) == (0))) || (line_thickness <= 0))
				return;
			if ((b.use_clipping) != 0)
			{
				if (
					!(!(((((b.clip.x) > (rect.x + rect.w)) || ((b.clip.x + b.clip.w) < (rect.x))) ||
					     ((b.clip.y) > (rect.y + rect.h))) || ((b.clip.y + b.clip.h) < (rect.y))))) return;
			}

			cmd = (nk_command_rect) (nk_command_buffer_push(b, (int) (NK_COMMAND_RECT)));
			if (cmd == null) return;
			cmd.rounding = ((ushort) (rounding));
			cmd.line_thickness = ((ushort) (line_thickness));
			cmd.x = ((short) (rect.x));
			cmd.y = ((short) (rect.y));
			cmd.w = ((ushort) ((0) < (rect.w) ? (rect.w) : (0)));
			cmd.h = ((ushort) ((0) < (rect.h) ? (rect.h) : (0)));
			cmd.color = (Color) (c);
		}

		public void nk_fill_rect(Nuklear.nk_command_buffer b, Rectangle rect, float rounding, Color c)
		{
			nk_command_rect_filled cmd;
			if ((((b == null) || ((c.a) == (0))) || ((rect.w) == (0))) || ((rect.h) == (0))) return;
			if ((b.use_clipping) != 0)
			{
				if (
					!(!(((((b.clip.x) > (rect.x + rect.w)) || ((b.clip.x + b.clip.w) < (rect.x))) ||
					     ((b.clip.y) > (rect.y + rect.h))) || ((b.clip.y + b.clip.h) < (rect.y))))) return;
			}

			cmd = (nk_command_rect_filled) (nk_command_buffer_push(b, (int) (NK_COMMAND_RECT_FILLED)));
			if (cmd == null) return;
			cmd.rounding = ((ushort) (rounding));
			cmd.x = ((short) (rect.x));
			cmd.y = ((short) (rect.y));
			cmd.w = ((ushort) ((0) < (rect.w) ? (rect.w) : (0)));
			cmd.h = ((ushort) ((0) < (rect.h) ? (rect.h) : (0)));
			cmd.color = (Color) (c);
		}

		public void nk_fill_rect_multi_color(Nuklear.nk_command_buffer b, Rectangle rect, Color left, Color top,
			Color right, Color bottom)
		{
			nk_command_rect_multi_color cmd;
			if (((b == null) || ((rect.w) == (0))) || ((rect.h) == (0))) return;
			if ((b.use_clipping) != 0)
			{
				if (
					!(!(((((b.clip.x) > (rect.x + rect.w)) || ((b.clip.x + b.clip.w) < (rect.x))) ||
					     ((b.clip.y) > (rect.y + rect.h))) || ((b.clip.y + b.clip.h) < (rect.y))))) return;
			}

			cmd = (nk_command_rect_multi_color) (nk_command_buffer_push(b, (int) (NK_COMMAND_RECT_MULTI_COLOR)));
			if (cmd == null) return;
			cmd.x = ((short) (rect.x));
			cmd.y = ((short) (rect.y));
			cmd.w = ((ushort) ((0) < (rect.w) ? (rect.w) : (0)));
			cmd.h = ((ushort) ((0) < (rect.h) ? (rect.h) : (0)));
			cmd.left = (Color) (left);
			cmd.top = (Color) (top);
			cmd.right = (Color) (right);
			cmd.bottom = (Color) (bottom);
		}

		public void nk_fill_circle(Nuklear.nk_command_buffer b, Rectangle r, Color c)
		{
			nk_command_circle_filled cmd;
			if ((((b == null) || ((c.a) == (0))) || ((r.w) == (0))) || ((r.h) == (0))) return;
			if ((b.use_clipping) != 0)
			{
				if (
					!(!(((((b.clip.x) > (r.x + r.w)) || ((b.clip.x + b.clip.w) < (r.x))) || ((b.clip.y) > (r.y + r.h))) ||
					    ((b.clip.y + b.clip.h) < (r.y))))) return;
			}

			cmd = (nk_command_circle_filled) (nk_command_buffer_push(b, (int) (NK_COMMAND_CIRCLE_FILLED)));
			if (cmd == null) return;
			cmd.x = ((short) (r.x));
			cmd.y = ((short) (r.y));
			cmd.w = ((ushort) ((r.w) < (0) ? (0) : (r.w)));
			cmd.h = ((ushort) ((r.h) < (0) ? (0) : (r.h)));
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
					((!((((b.clip.x) <= (x0)) && ((x0) < (b.clip.x + b.clip.w))) &&
					    (((b.clip.y) <= (y0)) && ((y0) < (b.clip.y + b.clip.h))))) &&
					 (!((((b.clip.x) <= (x1)) && ((x1) < (b.clip.x + b.clip.w))) &&
					    (((b.clip.y) <= (y1)) && ((y1) < (b.clip.y + b.clip.h)))))) &&
					(!((((b.clip.x) <= (x2)) && ((x2) < (b.clip.x + b.clip.w))) &&
					   (((b.clip.y) <= (y2)) && ((y2) < (b.clip.y + b.clip.h)))))) return;
			}

			cmd = (nk_command_triangle_filled) (nk_command_buffer_push(b, (int) (NK_COMMAND_TRIANGLE_FILLED)));
			if (cmd == null) return;
			cmd.a.x = ((short) (x0));
			cmd.a.y = ((short) (y0));
			cmd.b.x = ((short) (x1));
			cmd.b.y = ((short) (y1));
			cmd.c.x = ((short) (x2));
			cmd.c.y = ((short) (y2));
			cmd.color = (Color) (c);
		}

		public void nk_draw_image(Nuklear.nk_command_buffer b, Rectangle r, Nuklear.nk_image img, Color col)
		{
			nk_command_image cmd;
			if (b == null) return;
			if ((b.use_clipping) != 0)
			{
				if ((((b.clip.w) == (0)) || ((b.clip.h) == (0))) ||
				    (!(!(((((b.clip.x) > (r.x + r.w)) || ((b.clip.x + b.clip.w) < (r.x))) || ((b.clip.y) > (r.y + r.h))) ||
				         ((b.clip.y + b.clip.h) < (r.y)))))) return;
			}

			cmd = (nk_command_image) (nk_command_buffer_push(b, (int) (NK_COMMAND_IMAGE)));
			if (cmd == null) return;
			cmd.x = ((short) (r.x));
			cmd.y = ((short) (r.y));
			cmd.w = ((ushort) ((0) < (r.w) ? (r.w) : (0)));
			cmd.h = ((ushort) ((0) < (r.h) ? (r.h) : (0)));
			cmd.img = (Nuklear.nk_image) (img);
			cmd.col = (Color) (col);
		}

		public void nk_draw_text(Nuklear.nk_command_buffer b, Rectangle r, char* _string_, int length, Nuklear.nk_user_font font,
			Color bg, Color fg)
		{
			float text_width = (float) (0);
			nk_command_text cmd;
			if ((((b == null) || (_string_ == null)) || (length == 0)) || (((bg.a) == (0)) && ((fg.a) == (0)))) return;
			if ((b.use_clipping) != 0)
			{
				if ((((b.clip.w) == (0)) || ((b.clip.h) == (0))) ||
				    (!(!(((((b.clip.x) > (r.x + r.w)) || ((b.clip.x + b.clip.w) < (r.x))) || ((b.clip.y) > (r.y + r.h))) ||
				         ((b.clip.y + b.clip.h) < (r.y)))))) return;
			}

			text_width =
				(float) (font.width((Nuklear.nk_handle) (font.userdata), (float) (font.height), _string_, (int) (length)));
			if ((text_width) > (r.w))
			{
				int glyphs = (int) (0);
				float txt_width = (float) (text_width);
				length =
					(int)
						(nk_text_clamp(font, _string_, (int) (length), (float) (r.w), &glyphs, &txt_width, null,
							(int) (0)));
			}

			if (length == 0) return;
			cmd = (nk_command_text) (nk_command_buffer_push(b, (int) (NK_COMMAND_TEXT)));
			if (cmd == null) return;
			cmd.x = ((short) (r.x));
			cmd.y = ((short) (r.y));
			cmd.w = ((ushort) (r.w));
			cmd.h = ((ushort) (r.h));
			cmd.background = (Color) (bg);
			cmd.foreground = (Color) (fg);
			cmd.font = font;
			cmd.length = (int) (length);
			cmd.height = (float) (font.height);
			cmd._string_ = (char*)CRuntime.malloc(length);
			CRuntime.memcpy((void*) cmd._string_, _string_, length*sizeof (char));
			cmd._string_[length] = ('\0');
		}        
    }
}