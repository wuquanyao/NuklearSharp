using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace NuklearSharp
{
	public struct RectangleF
	{
		public float X, Y, Width, Height;
	}
	
	public unsafe static partial class Utility
	{
		public unsafe partial class nk_baked_font
		{
			public float height;
			public float ascent;
			public float descent;
			public uint glyph_offset;
			public uint glyph_count;
			public uint* ranges;
		}

		[StructLayout(LayoutKind.Sequential)]
		public unsafe partial struct nk_key
		{
			public int down;
			public uint clicked;
		}

		[StructLayout(LayoutKind.Sequential)]
		public unsafe partial struct conv
		{
			public uint i;
			public float f;
		}
		
		public static float nk_inv_sqrt(float number)
		{
			var threehalfs = 1.5f;
			var conv = new nk_inv_sqrt_union
			{
				i = 0,
				f = number,
			};
			var x2 = number*0.5f;
			conv.i = 0x5f375A84 - (conv.i >> 1);
			conv.f = conv.f*(threehalfs - (x2*conv.f*conv.f));

			return conv.f;
		}

		public static float nk_sqrt(float x)
		{
			return (float) (x*nk_inv_sqrt((float) (x)));
		}

		public static float nk_sin(float x)
		{
			float a0 = (float) (+1.91059300966915117e-31f);
			float a1 = (float) (+1.00086760103908896f);
			float a2 = (float) (-1.21276126894734565e-2f);
			float a3 = (float) (-1.38078780785773762e-1f);
			float a4 = (float) (-2.67353392911981221e-2f);
			float a5 = (float) (+2.08026600266304389e-2f);
			float a6 = (float) (-3.03996055049204407e-3f);
			float a7 = (float) (+1.38235642404333740e-4f);
			return (float) (a0 + x*(a1 + x*(a2 + x*(a3 + x*(a4 + x*(a5 + x*(a6 + x*a7)))))));
		}

		public static float nk_cos(float x)
		{
			float a0 = (float) (+1.00238601909309722f);
			float a1 = (float) (-3.81919947353040024e-2f);
			float a2 = (float) (-3.94382342128062756e-1f);
			float a3 = (float) (-1.18134036025221444e-1f);
			float a4 = (float) (+1.07123798512170878e-1f);
			float a5 = (float) (-1.86637164165180873e-2f);
			float a6 = (float) (+9.90140908664079833e-4f);
			float a7 = (float) (-5.23022132118824778e-14f);
			return (float) (a0 + x*(a1 + x*(a2 + x*(a3 + x*(a4 + x*(a5 + x*(a6 + x*a7)))))));
		}

		public static uint nk_round_up_pow2(uint v)
		{
			v--;
			v |= (uint) (v >> 1);
			v |= (uint) (v >> 2);
			v |= (uint) (v >> 4);
			v |= (uint) (v >> 8);
			v |= (uint) (v >> 16);
			v++;
			return (uint) (v);
		}

		public static RectangleF RectangleF_(float x, float y, float w, float h)
		{
			RectangleF r = new RectangleF();
			r.X = (float) (x);
			r.Y = (float) (y);
			r.Width = (float) (w);
			r.Height = (float) (h);
			return (RectangleF) (r);
		}

		public static Vector2 Vector2(float x, float y)
		{
			Vector2 ret = new Vector2();
			ret.X = (float) (x);
			ret.Y = (float) (y);
			return (Vector2) (ret);
		}

		public static Vector2 Point_(int x, int y)
		{
			Vector2 ret = new Vector2();
			ret.X = ((float) (x));
			ret.Y = ((float) (y));
			return (Vector2) (ret);
		}

		public static Vector2 Vector2v(float* v)
		{
			return (Vector2) (new Vector2((float) (v[0]), (float) (v[1])));
		}

		public static Vector2 Pointv(int* v)
		{
			return (Vector2) (Point_((int) (v[0]), (int) (v[1])));
		}

		public static int nk_is_lower(int c)
		{
			return (int) ((((c) >= ('a')) && (c <= 'z')) || (((c) >= (0xE0)) && (c <= 0xFF)) ? 1 : 0);
		}

		public static int nk_is_upper(int c)
		{
			return (int) ((((c) >= ('A')) && (c <= 'Z')) || (((c) >= (0xC0)) && (c <= 0xDF)) ? 1 : 0);
		}

		public static int nk_to_upper(int c)
		{
			return (int) ((((c) >= ('a')) && (c <= 'z')) ? (c - ('a' - 'A')) : c);
		}

		public static int nk_to_lower(int c)
		{
			return (int) ((((c) >= ('A')) && (c <= 'Z')) ? (c - ('a' + 'A')) : c);
		}

		public static void* nk_memcopy(void* dst0, void* src0, ulong length)
		{
			ulong t;
			sbyte* dst = (sbyte*) (dst0);
			sbyte* src = (sbyte*) (src0);
			if (((length) == (0)) || ((dst) == (src))) goto done;
			if ((dst) < (src))
			{
				t = ((ulong) (src));
				if (((t | (ulong) (dst)) & (sizeof (int) - 1)) != 0)
				{
					if ((((t ^ (ulong) (dst)) & (sizeof (int) - 1)) != 0) || ((length) < (sizeof (int)))) t = (ulong) (length);
					else t = (ulong) (sizeof (int) - (t & (sizeof (int) - 1)));
					length -= (ulong) (t);
					do
					{
						*dst++ = (sbyte) (*src++);
					} while ((--t) != 0);
				}
				t = (ulong) (length/sizeof (int));
				if ((t) != 0)
					do
					{
						*(int*) ((void*) (dst)) = (int) (*(int*) ((void*) (src)));
						src += sizeof (int);
						dst += sizeof (int);
					} while ((--t) != 0);
				t = (ulong) (length & (sizeof (int) - 1));
				if ((t) != 0)
					do
					{
						*dst++ = (sbyte) (*src++);
					} while ((--t) != 0);
			}
			else
			{
				src += length;
				dst += length;
				t = ((ulong) (src));
				if (((t | (ulong) (dst)) & (sizeof (int) - 1)) != 0)
				{
					if ((((t ^ (ulong) (dst)) & (sizeof (int) - 1)) != 0) || (length <= sizeof (int))) t = (ulong) (length);
					else t &= (ulong) (sizeof (int) - 1);
					length -= (ulong) (t);
					do
					{
						*--dst = (sbyte) (*--src);
					} while ((--t) != 0);
				}
				t = (ulong) (length/sizeof (int));
				if ((t) != 0)
					do
					{
						src -= sizeof (int);
						dst -= sizeof (int);
						*(int*) ((void*) (dst)) = (int) (*(int*) ((void*) (src)));
					} while ((--t) != 0);
				t = (ulong) (length & (sizeof (int) - 1));
				if ((t) != 0)
					do
					{
						*--dst = (sbyte) (*--src);
					} while ((--t) != 0);
			}

			done:
			;
			return (dst0);
		}

		public static void nk_memset(void* ptr, int c0, ulong size)
		{
			byte* dst = (byte*) (ptr);
			uint c = (uint) (0);
			ulong t = (ulong) (0);
			if ((c = (uint) ((byte) (c0))) != 0)
			{
				c = (uint) ((c << 8) | c);
				if (sizeof (uint) > 2) c = (uint) ((c << 16) | c);
			}

			dst = (byte*) (ptr);
			if ((size) < (3*sizeof (uint)))
			{
				while ((size--) != 0)
				{
					*dst++ = ((byte) (c0));
				}
				return;
			}

			if ((t = (ulong) (((ulong) ((long) (dst))) & (sizeof (uint) - 1))) != 0)
			{
				t = (ulong) (sizeof (uint) - t);
				size -= (ulong) (t);
				do
				{
					*dst++ = ((byte) (c0));
				} while (--t != 0);
			}

			t = (ulong) (size/sizeof (uint));
			do
			{
				*(uint*) ((void*) (dst)) = (uint) (c);
				dst += sizeof (uint);
			} while (--t != 0);
			t = (ulong) (size & (sizeof (uint) - 1));
			if (t != 0)
			{
				do
				{
					*dst++ = ((byte) (c0));
				} while (--t != 0);
			}

		}

		public static void nk_zero(void* ptr, ulong size)
		{
			nk_memset(ptr, (int) (0), (ulong) (size));
		}

		public static int nk_strlen(char* str)
		{
			int siz = (int) (0);
			while (((str) != null) && (*str++ != '\0'))
			{
				siz++;
			}
			return (int) (siz);
		}

		public static int nk_strtoi(char* str, char** endptr)
		{
			int neg = (int) (1);
			char* p = str;
			int value = (int) (0);
			if (str == null) return (int) (0);
			while ((*p) == (' '))
			{
				p++;
			}
			if ((*p) == ('-'))
			{
				neg = (int) (-1);
				p++;
			}

			while ((((*p) != 0) && ((*p) >= ('0'))) && (*p <= '9'))
			{
				value = (int) (value*10 + (*p - '0'));
				p++;
			}
			if ((endptr) != null) *endptr = p;
			return (int) (neg*value);
		}

		public static double nk_strtod(char* str, char** endptr)
		{
			double m;
			double neg = (double) (1.0);
			char* p = str;
			double value = (double) (0);
			double number = (double) (0);
			if (str == null) return (double) (0);
			while ((*p) == (' '))
			{
				p++;
			}
			if ((*p) == ('-'))
			{
				neg = (double) (-1.0);
				p++;
			}

			while ((((*p) != 0) && (*p != '.')) && (*p != 'e'))
			{
				value = (double) (value*10.0 + (double) (*p - '0'));
				p++;
			}
			if ((*p) == ('.'))
			{
				p++;
				for (m = (double) (0.1); ((*p) != 0) && (*p != 'e'); p++)
				{
					value = (double) (value + (double) (*p - '0')*m);
					m *= (double) (0.1);
				}
			}

			if ((*p) == ('e'))
			{
				int i;
				int pow;
				int div;
				p++;
				if ((*p) == ('-'))
				{
					div = (int) (nk_true);
					p++;
				}
				else if ((*p) == ('+'))
				{
					div = (int) (nk_false);
					p++;
				}
				else div = (int) (nk_false);
				for (pow = (int) (0); *p != 0; p++)
				{
					pow = (int) (pow*10 + (*p - '0'));
				}
				for (m = (double) (1.0) , i = (int) (0); (i) < (pow); i++)
				{
					m *= (double) (10.0);
				}
				if ((div) != 0) value /= (double) (m);
				else value *= (double) (m);
			}

			number = (double) (value*neg);
			if ((endptr) != null) *endptr = p;
			return (double) (number);
		}

		public static float nk_strtof(char* str, char** endptr)
		{
			float float_value;
			double double_value;
			double_value = (double) (nk_strtod(str, endptr));
			float_value = ((float) (double_value));
			return (float) (float_value);
		}

		public static int nk_stricmpn(char* s1, char* s2, int n)
		{
			int c1;
			int c2;
			int d;
			do
			{
				c1 = (int) (*s1++);
				c2 = (int) (*s2++);
				if (n-- == 0) return (int) (0);
				d = (int) (c1 - c2);
				while ((d) != 0)
				{
					if ((c1 <= 'Z') && ((c1) >= ('A')))
					{
						d += (int) ('a' - 'A');
						if (d == 0) break;
					}
					if ((c2 <= 'Z') && ((c2) >= ('A')))
					{
						d -= (int) ('a' - 'A');
						if (d == 0) break;
					}
					return (int) ((((d) >= (0) ? 1 : 0) << 1) - 1);
				}
			} while ((c1) != 0);
			return (int) (0);
		}

		public static int nk_str_match_here(sbyte* regexp, char* text)
		{
			if ((regexp[0]) == ('\0')) return (int) (1);
			if ((regexp[1]) == ('*')) return (int) (nk_str_match_star((int) (regexp[0]), regexp + 2, text));
			if (((regexp[0]) == ('$')) && ((regexp[1]) == ('\0'))) return (int) ((*text) == ('\0') ? 1 : 0);
			if ((*text != '\0') && (((regexp[0]) == ('.')) || ((regexp[0]) == (*text))))
				return (int) (nk_str_match_here(regexp + 1, text + 1));
			return (int) (0);
		}

		public static int nk_str_match_star(int c, sbyte* regexp, char* text)
		{
			do
			{
				if ((nk_str_match_here(regexp, text)) != 0) return (int) (1);
			} while ((*text != '\0') && (((*text++) == (c)) || ((c) == ('.'))));
			return (int) (0);
		}

		public static int nk_string_float_limit(char* _string_, int prec)
		{
			int dot = (int) (0);
			char* c = _string_;
			while ((*c) != 0)
			{
				if ((*c) == ('.'))
				{
					dot = (int) (1);
					c++;
					continue;
				}
				if ((dot) == (prec + 1))
				{
					*c = (char) 0;
					break;
				}
				if ((dot) > (0)) dot++;
				c++;
			}
			return (int) (c - _string_);
		}

		public static double nk_pow(double x, int n)
		{
			double r = (double) (1);
			int plus = (int) ((n) >= (0) ? 1 : 0);
			n = (int) ((plus) != 0 ? n : -n);
			while ((n) > (0))
			{
				if ((n & 1) == (1)) r *= (double) (x);
				n /= (int) (2);
				x *= (double) (x);
			}
			return (double) ((plus) != 0 ? r : 1.0/r);
		}

		public static int nk_ifloord(double x)
		{
			x = ((double) ((int) (x) - (((x) < (0.0)) ? 1 : 0)));
			return (int) (x);
		}

		public static int nk_ifloorf(float x)
		{
			x = ((float) ((int) (x) - (((x) < (0.0f)) ? 1 : 0)));
			return (int) (x);
		}

		public static int nk_iceilf(float x)
		{
			if ((x) >= (0))
			{
				int i = (int) (x);
				return (int) (((x) > (i)) ? i + 1 : i);
			}
			else
			{
				int t = (int) (x);
				float r = (float) (x - (float) (t));
				return (int) (((r) > (0.0f)) ? t + 1 : t);
			}

		}

		public static int nk_log10(double n)
		{
			int neg;
			int ret;
			int exp = (int) (0);
			neg = (int) (((n) < (0)) ? 1 : 0);
			ret = (int) ((neg) != 0 ? (int) (-n) : (int) (n));
			while ((ret/10) > (0))
			{
				ret /= (int) (10);
				exp++;
			}
			if ((neg) != 0) exp = (int) (-exp);
			return (int) (exp);
		}

		public static void nk_strrev_ascii(char* s)
		{
			int len = (int) (nk_strlen(s));
			int end = (int) (len/2);
			int i = (int) (0);
			char t;
			for (; (i) < (end); ++i)
			{
				t = (s[i]);
				s[i] = (s[len - 1 - i]);
				s[len - 1 - i] = t;
			}
		}

		public static char* nk_itoa(char* s, int n)
		{
			int i = (int) (0);
			if ((n) == (0))
			{
				s[i++] = ('0');
				s[i] = (char) (0);
				return s;
			}

			if ((n) < (0))
			{
				s[i++] = ('-');
				n = (int) (-n);
			}

			while ((n) > (0))
			{
				s[i++] = (char) (('0' + (char) (n%10)));
				n /= (int) (10);
			}
			s[i] = (char) (0);
			if ((s[0]) == ('-')) ++s;
			nk_strrev_ascii(s);
			return s;
		}

		public static char* nk_dtoa(char* s, double n)
		{
			int useExp = (int) (0);
			int digit = (int) (0);
			int m = (int) (0);
			int m1 = (int) (0);
			char* c = s;
			int neg = (int) (0);
			if (s == null) return null;
			if ((n) == (0.0))
			{
				s[0] = ('0');
				s[1] = ('\0');
				return s;
			}

			neg = (int) ((n) < (0) ? 1 : 0);
			if ((neg) != 0) n = (double) (-n);
			m = (int) (nk_log10((double) (n)));
			useExp = (int) ((((m) >= (14)) || (((neg) != 0) && ((m) >= (9)))) || (m <= -9) ? 1 : 0);
			if ((neg) != 0) *(c++) = ('-');
			if ((useExp) != 0)
			{
				if ((m) < (0)) m -= (int) (1);
				n = (double) (n/nk_pow((double) (10.0), (int) (m)));
				m1 = (int) (m);
				m = (int) (0);
			}

			if ((m) < (1.0))
			{
				m = (int) (0);
			}

			while (((n) > (0.00000000000001)) || ((m) >= (0)))
			{
				double weight = (double) (nk_pow((double) (10.0), (int) (m)));
				if ((weight) > (0))
				{
					double t = (double) (n/weight);
					digit = (int) (nk_ifloord((double) (t)));
					n -= (double) ((double) (digit)*weight);
					*(c++) = (char) (('0' + (char) (digit)));
				}
				if (((m) == (0)) && ((n) > (0))) *(c++) = ('.');
				m--;
			}
			if ((useExp) != 0)
			{
				int i;
				int j;
				*(c++) = ('e');
				if ((m1) > (0))
				{
					*(c++) = ('+');
				}
				else
				{
					*(c++) = ('-');
					m1 = (int) (-m1);
				}
				m = (int) (0);
				while ((m1) > (0))
				{
					*(c++) = (char) (('0' + (char) (m1%10)));
					m1 /= (int) (10);
					m++;
				}
				c -= m;
				for (i = (int) (0) , j = (int) (m - 1); (i) < (j); i++ , j--)
				{
					c[i] ^= (c[j]);
					c[j] ^= (c[i]);
					c[i] ^= (c[j]);
				}
				c += m;
			}

			*(c) = ('\0');
			return s;
		}

		public static uint nk_murmur_hash(void* key, int len, uint seed)
		{
			nk_murmur_hash_union conv = new nk_murmur_hash_union(null);
			byte* data = (byte*) (key);
			int nblocks = (int) (len/4);
			uint h1 = (uint) (seed);
			uint c1 = (uint) (0xcc9e2d51);
			uint c2 = (uint) (0x1b873593);
			byte* tail;
			uint* blocks;
			uint k1;
			int i;
			if (key == null) return (uint) (0);
			conv.b = (data + nblocks*4);
			blocks = conv.i;
			for (i = (int) (-nblocks); i != 0; ++i)
			{
				k1 = (uint) (blocks[i]);
				k1 *= (uint) (c1);
				k1 = (uint) ((k1) << (15) | ((k1) >> (32 - 15)));
				k1 *= (uint) (c2);
				h1 ^= (uint) (k1);
				h1 = (uint) ((h1) << (13) | ((h1) >> (32 - 13)));
				h1 = (uint) (h1*5 + 0xe6546b64);
			}
			tail = (data + nblocks*4);
			k1 = (uint) (0);
			int l = (int) (len & 3);
			switch (l)
			{
				case 1:
				case 2:
				case 3:
					if ((l) == (2))
					{
						k1 ^= ((uint) (tail[1] << 8));
					}
					else if ((l) == (3))
					{
						k1 ^= ((uint) (tail[2] << 16));
					}
					k1 ^= (uint) (tail[0]);
					k1 *= (uint) (c1);
					k1 = (uint) ((k1) << (15) | ((k1) >> (32 - 15)));
					k1 *= (uint) (c2);
					h1 ^= (uint) (k1);
					break;
				default:
					break;
			}

			h1 ^= ((uint) (len));
			h1 ^= (uint) (h1 >> 16);
			h1 *= (uint) (0x85ebca6b);
			h1 ^= (uint) (h1 >> 13);
			h1 *= (uint) (0xc2b2ae35);
			h1 ^= (uint) (h1 >> 16);
			return (uint) (h1);
		}

		public static int nk_parse_hex(sbyte* p, int length)
		{
			int i = (int) (0);
			int len = (int) (0);
			while ((len) < (length))
			{
				i <<= 4;
				if (((p[len]) >= ('a')) && (p[len] <= 'f')) i += (int) ((p[len] - 'a') + 10);
				else if (((p[len]) >= ('A')) && (p[len] <= 'F')) i += (int) ((p[len] - 'A') + 10);
				else i += (int) (p[len] - '0');
				len++;
			}
			return (int) (i);
		}

		public static Color nk_rgba(int r, int g, int b, int a)
		{
			Color ret = new Color();
			ret.r = ((byte) (((r) < (255) ? (r) : (255)) < (0) ? (0) : ((r) < (255) ? (r) : (255))));
			ret.g = ((byte) (((g) < (255) ? (g) : (255)) < (0) ? (0) : ((g) < (255) ? (g) : (255))));
			ret.b = ((byte) (((b) < (255) ? (b) : (255)) < (0) ? (0) : ((b) < (255) ? (b) : (255))));
			ret.a = ((byte) (((a) < (255) ? (a) : (255)) < (0) ? (0) : ((a) < (255) ? (a) : (255))));
			return (Color) (ret);
		}

		public static Color nk_rgb_hex(sbyte* rgb)
		{
			Color col = new Color();
			sbyte* c = rgb;
			if ((*c) == ('#')) c++;
			col.r = ((byte) (nk_parse_hex(c, (int) (2))));
			col.g = ((byte) (nk_parse_hex(c + 2, (int) (2))));
			col.b = ((byte) (nk_parse_hex(c + 4, (int) (2))));
			col.a = (byte) (255);
			return (Color) (col);
		}

		public static Color nk_rgba_hex(sbyte* rgb)
		{
			Color col = new Color();
			sbyte* c = rgb;
			if ((*c) == ('#')) c++;
			col.r = ((byte) (nk_parse_hex(c, (int) (2))));
			col.g = ((byte) (nk_parse_hex(c + 2, (int) (2))));
			col.b = ((byte) (nk_parse_hex(c + 4, (int) (2))));
			col.a = ((byte) (nk_parse_hex(c + 6, (int) (2))));
			return (Color) (col);
		}

		public static void Color_hex_rgba(char* output, Color col)
		{
			output[0] = ((char) (((col.r & 0xF0) >> 4) <= 9 ? '0' + ((col.r & 0xF0) >> 4) : 'A' - 10 + ((col.r & 0xF0) >> 4)));
			output[1] = ((char) ((col.r & 0x0F) <= 9 ? '0' + (col.r & 0x0F) : 'A' - 10 + (col.r & 0x0F)));
			output[2] = ((char) (((col.g & 0xF0) >> 4) <= 9 ? '0' + ((col.g & 0xF0) >> 4) : 'A' - 10 + ((col.g & 0xF0) >> 4)));
			output[3] = ((char) ((col.g & 0x0F) <= 9 ? '0' + (col.g & 0x0F) : 'A' - 10 + (col.g & 0x0F)));
			output[4] = ((char) (((col.b & 0xF0) >> 4) <= 9 ? '0' + ((col.b & 0xF0) >> 4) : 'A' - 10 + ((col.b & 0xF0) >> 4)));
			output[5] = ((char) ((col.b & 0x0F) <= 9 ? '0' + (col.b & 0x0F) : 'A' - 10 + (col.b & 0x0F)));
			output[6] = ((char) (((col.a & 0xF0) >> 4) <= 9 ? '0' + ((col.a & 0xF0) >> 4) : 'A' - 10 + ((col.a & 0xF0) >> 4)));
			output[7] = ((char) ((col.a & 0x0F) <= 9 ? '0' + (col.a & 0x0F) : 'A' - 10 + (col.a & 0x0F)));
			output[8] = ('\0');
		}

		public static void Color_hex_rgb(char* output, Color col)
		{
			output[0] = ((char) (((col.r & 0xF0) >> 4) <= 9 ? '0' + ((col.r & 0xF0) >> 4) : 'A' - 10 + ((col.r & 0xF0) >> 4)));
			output[1] = ((char) ((col.r & 0x0F) <= 9 ? '0' + (col.r & 0x0F) : 'A' - 10 + (col.r & 0x0F)));
			output[2] = ((char) (((col.g & 0xF0) >> 4) <= 9 ? '0' + ((col.g & 0xF0) >> 4) : 'A' - 10 + ((col.g & 0xF0) >> 4)));
			output[3] = ((char) ((col.g & 0x0F) <= 9 ? '0' + (col.g & 0x0F) : 'A' - 10 + (col.g & 0x0F)));
			output[4] = ((char) (((col.b & 0xF0) >> 4) <= 9 ? '0' + ((col.b & 0xF0) >> 4) : 'A' - 10 + ((col.b & 0xF0) >> 4)));
			output[5] = ((char) ((col.b & 0x0F) <= 9 ? '0' + (col.b & 0x0F) : 'A' - 10 + (col.b & 0x0F)));
			output[6] = ('\0');
		}

		public static Color nk_rgba_iv(int* c)
		{
			return (Color) (nk_rgba((int) (c[0]), (int) (c[1]), (int) (c[2]), (int) (c[3])));
		}

		public static Color nk_rgba_bv(byte* c)
		{
			return (Color) (nk_rgba((int) (c[0]), (int) (c[1]), (int) (c[2]), (int) (c[3])));
		}

		public static Color nk_rgb(int r, int g, int b)
		{
			Color ret = new Color();
			ret.r = ((byte) (((r) < (255) ? (r) : (255)) < (0) ? (0) : ((r) < (255) ? (r) : (255))));
			ret.g = ((byte) (((g) < (255) ? (g) : (255)) < (0) ? (0) : ((g) < (255) ? (g) : (255))));
			ret.b = ((byte) (((b) < (255) ? (b) : (255)) < (0) ? (0) : ((b) < (255) ? (b) : (255))));
			ret.a = ((byte) (255));
			return (Color) (ret);
		}

		public static Color nk_rgb_iv(int* c)
		{
			return (Color) (nk_rgb((int) (c[0]), (int) (c[1]), (int) (c[2])));
		}

		public static Color nk_rgb_bv(byte* c)
		{
			return (Color) (nk_rgb((int) (c[0]), (int) (c[1]), (int) (c[2])));
		}

		public static Color nk_rgba_u32(uint _in_)
		{
			Color ret = new Color();
			ret.r = (byte) (_in_ & 0xFF);
			ret.g = (byte) ((_in_ >> 8) & 0xFF);
			ret.b = (byte) ((_in_ >> 16) & 0xFF);
			ret.a = ((byte) ((_in_ >> 24) & 0xFF));
			return (Color) (ret);
		}

		public static Color nk_rgba_f(float r, float g, float b, float a)
		{
			Color ret = new Color();
			ret.r = ((byte) (((0) < ((1.0f) < (r) ? (1.0f) : (r)) ? ((1.0f) < (r) ? (1.0f) : (r)) : (0))*255.0f));
			ret.g = ((byte) (((0) < ((1.0f) < (g) ? (1.0f) : (g)) ? ((1.0f) < (g) ? (1.0f) : (g)) : (0))*255.0f));
			ret.b = ((byte) (((0) < ((1.0f) < (b) ? (1.0f) : (b)) ? ((1.0f) < (b) ? (1.0f) : (b)) : (0))*255.0f));
			ret.a = ((byte) (((0) < ((1.0f) < (a) ? (1.0f) : (a)) ? ((1.0f) < (a) ? (1.0f) : (a)) : (0))*255.0f));
			return (Color) (ret);
		}

		public static Color nk_rgba_fv(float* c)
		{
			return (Color) (nk_rgba_f((float) (c[0]), (float) (c[1]), (float) (c[2]), (float) (c[3])));
		}

		public static Color nk_rgb_f(float r, float g, float b)
		{
			Color ret = new Color();
			ret.r = ((byte) (((0) < ((1.0f) < (r) ? (1.0f) : (r)) ? ((1.0f) < (r) ? (1.0f) : (r)) : (0))*255.0f));
			ret.g = ((byte) (((0) < ((1.0f) < (g) ? (1.0f) : (g)) ? ((1.0f) < (g) ? (1.0f) : (g)) : (0))*255.0f));
			ret.b = ((byte) (((0) < ((1.0f) < (b) ? (1.0f) : (b)) ? ((1.0f) < (b) ? (1.0f) : (b)) : (0))*255.0f));
			ret.a = (byte) (255);
			return (Color) (ret);
		}

		public static Color nk_rgb_fv(float* c)
		{
			return (Color) (nk_rgb_f((float) (c[0]), (float) (c[1]), (float) (c[2])));
		}

		public static Color nk_hsv(int h, int s, int v)
		{
			return (Color) (nk_hsva((int) (h), (int) (s), (int) (v), (int) (255)));
		}

		public static Color nk_hsv_iv(int* c)
		{
			return (Color) (nk_hsv((int) (c[0]), (int) (c[1]), (int) (c[2])));
		}

		public static Color nk_hsv_bv(byte* c)
		{
			return (Color) (nk_hsv((int) (c[0]), (int) (c[1]), (int) (c[2])));
		}

		public static Color nk_hsv_f(float h, float s, float v)
		{
			return (Color) (nk_hsva_f((float) (h), (float) (s), (float) (v), (float) (1.0f)));
		}

		public static Color nk_hsv_fv(float* c)
		{
			return (Color) (nk_hsv_f((float) (c[0]), (float) (c[1]), (float) (c[2])));
		}

		public static Color nk_hsva(int h, int s, int v, int a)
		{
			float hf = (float) (((float) (((h) < (255) ? (h) : (255)) < (0) ? (0) : ((h) < (255) ? (h) : (255))))/255.0f);
			float sf = (float) (((float) (((s) < (255) ? (s) : (255)) < (0) ? (0) : ((s) < (255) ? (s) : (255))))/255.0f);
			float vf = (float) (((float) (((v) < (255) ? (v) : (255)) < (0) ? (0) : ((v) < (255) ? (v) : (255))))/255.0f);
			float af = (float) (((float) (((a) < (255) ? (a) : (255)) < (0) ? (0) : ((a) < (255) ? (a) : (255))))/255.0f);
			return (Color) (nk_hsva_f((float) (hf), (float) (sf), (float) (vf), (float) (af)));
		}

		public static Color nk_hsva_iv(int* c)
		{
			return (Color) (nk_hsva((int) (c[0]), (int) (c[1]), (int) (c[2]), (int) (c[3])));
		}

		public static Color nk_hsva_bv(byte* c)
		{
			return (Color) (nk_hsva((int) (c[0]), (int) (c[1]), (int) (c[2]), (int) (c[3])));
		}

		public static Colorf nk_hsva_colorf(float h, float s, float v, float a)
		{
			int i;
			float p;
			float q;
			float t;
			float f;
			Colorf _out_ = new Colorf();
			if (s <= 0.0f)
			{
				_out_.r = (float) (v);
				_out_.g = (float) (v);
				_out_.b = (float) (v);
				_out_.a = (float) (a);
				return (Colorf) (_out_);
			}

			h = (float) (h/(60.0f/360.0f));
			i = ((int) (h));
			f = (float) (h - (float) (i));
			p = (float) (v*(1.0f - s));
			q = (float) (v*(1.0f - (s*f)));
			t = (float) (v*(1.0f - s*(1.0f - f)));
			switch (i)
			{
				case 0:
				default:
					_out_.r = (float) (v);
					_out_.g = (float) (t);
					_out_.b = (float) (p);
					break;
				case 1:
					_out_.r = (float) (q);
					_out_.g = (float) (v);
					_out_.b = (float) (p);
					break;
				case 2:
					_out_.r = (float) (p);
					_out_.g = (float) (v);
					_out_.b = (float) (t);
					break;
				case 3:
					_out_.r = (float) (p);
					_out_.g = (float) (q);
					_out_.b = (float) (v);
					break;
				case 4:
					_out_.r = (float) (t);
					_out_.g = (float) (p);
					_out_.b = (float) (v);
					break;
				case 5:
					_out_.r = (float) (v);
					_out_.g = (float) (p);
					_out_.b = (float) (q);
					break;
			}

			_out_.a = (float) (a);
			return (Colorf) (_out_);
		}

		public static Colorf nk_hsva_colorfv(float* c)
		{
			return (Colorf) (nk_hsva_colorf((float) (c[0]), (float) (c[1]), (float) (c[2]), (float) (c[3])));
		}

		public static Color nk_hsva_f(float h, float s, float v, float a)
		{
			Colorf c = (Colorf) (nk_hsva_colorf((float) (h), (float) (s), (float) (v), (float) (a)));
			return (Color) (nk_rgba_f((float) (c.r), (float) (c.g), (float) (c.b), (float) (c.a)));
		}

		public static Color nk_hsva_fv(float* c)
		{
			return (Color) (nk_hsva_f((float) (c[0]), (float) (c[1]), (float) (c[2]), (float) (c[3])));
		}

		public static void color_fv(ref Nuklear.Colorf col, Color _in_)
		{
			col.r = (float)_in_.R / 255.0f;
			col.g = (float)_in_.G / 255.0f;
			col.b = (float)_in_.B / 255.0f;
			col.a = (float)_in_.A / 255.0f;
		}

		public static void Color_hsv_f(float* out_h, float* out_s, float* out_v, Color _in_)
		{
			float a;
			Color_hsva_f(out_h, out_s, out_v, &a, (Color) (_in_));
		}

		public static void Color_hsv_fv(float* _out_, Color _in_)
		{
			float a;
			Color_hsva_f(&_out_[0], &_out_[1], &_out_[2], &a, (Color) (_in_));
		}

		public static void Colorf_hsva_f(float* out_h, float* out_s, float* out_v, float* out_a, Colorf _in_)
		{
			float chroma;
			float K = (float) (0.0f);
			if ((_in_.g) < (_in_.b))
			{
				float t = (float) (_in_.g);
				_in_.g = (float) (_in_.b);
				_in_.b = (float) (t);
				K = (float) (-1.0f);
			}

			if ((_in_.r) < (_in_.g))
			{
				float t = (float) (_in_.r);
				_in_.r = (float) (_in_.g);
				_in_.g = (float) (t);
				K = (float) (-2.0f/6.0f - K);
			}

			chroma = (float) (_in_.r - (((_in_.g) < (_in_.b)) ? _in_.g : _in_.b));
			*out_h =
				(float)
					(((K + (_in_.g - _in_.b)/(6.0f*chroma + 1e-20f)) < (0))
						? -(K + (_in_.g - _in_.b)/(6.0f*chroma + 1e-20f))
						: (K + (_in_.g - _in_.b)/(6.0f*chroma + 1e-20f)));
			*out_s = (float) (chroma/(_in_.r + 1e-20f));
			*out_v = (float) (_in_.r);
			*out_a = (float) (_in_.a);
		}

		public static void Colorf_hsva_fv(float* hsva, Colorf _in_)
		{
			Colorf_hsva_f(&hsva[0], &hsva[1], &hsva[2], &hsva[3], (Colorf) (_in_));
		}

		public static void Color_hsva_f(float* out_h, float* out_s, float* out_v, float* out_a, Color _in_)
		{
			Colorf col = new Colorf();
			Color_f(&col.r, &col.g, &col.b, &col.a, (Color) (_in_));
			Colorf_hsva_f(out_h, out_s, out_v, out_a, (Colorf) (col));
		}

		public static void Color_hsva_fv(float* _out_, Color _in_)
		{
			Color_hsva_f(&_out_[0], &_out_[1], &_out_[2], &_out_[3], (Color) (_in_));
		}

		public static void Color_hsva_i(int* out_h, int* out_s, int* out_v, int* out_a, Color _in_)
		{
			float h;
			float s;
			float v;
			float a;
			Color_hsva_f(&h, &s, &v, &a, (Color) (_in_));
			*out_h = (int) ((byte) (h*255.0f));
			*out_s = (int) ((byte) (s*255.0f));
			*out_v = (int) ((byte) (v*255.0f));
			*out_a = (int) ((byte) (a*255.0f));
		}

		public static void Color_hsva_iv(int* _out_, Color _in_)
		{
			Color_hsva_i(&_out_[0], &_out_[1], &_out_[2], &_out_[3], (Color) (_in_));
		}

		public static void Color_hsva_bv(byte* _out_, Color _in_)
		{
			int* tmp = stackalloc int[4];
			Color_hsva_i(&tmp[0], &tmp[1], &tmp[2], &tmp[3], (Color) (_in_));
			_out_[0] = ((byte) (tmp[0]));
			_out_[1] = ((byte) (tmp[1]));
			_out_[2] = ((byte) (tmp[2]));
			_out_[3] = ((byte) (tmp[3]));
		}

		public static void Color_hsva_b(byte* h, byte* s, byte* v, byte* a, Color _in_)
		{
			int* tmp = stackalloc int[4];
			Color_hsva_i(&tmp[0], &tmp[1], &tmp[2], &tmp[3], (Color) (_in_));
			*h = ((byte) (tmp[0]));
			*s = ((byte) (tmp[1]));
			*v = ((byte) (tmp[2]));
			*a = ((byte) (tmp[3]));
		}

		public static void Color_hsv_i(int* out_h, int* out_s, int* out_v, Color _in_)
		{
			int a;
			Color_hsva_i(out_h, out_s, out_v, &a, (Color) (_in_));
		}

		public static void Color_hsv_b(byte* out_h, byte* out_s, byte* out_v, Color _in_)
		{
			int* tmp = stackalloc int[4];
			Color_hsva_i(&tmp[0], &tmp[1], &tmp[2], &tmp[3], (Color) (_in_));
			*out_h = ((byte) (tmp[0]));
			*out_s = ((byte) (tmp[1]));
			*out_v = ((byte) (tmp[2]));
		}

		public static void Color_hsv_iv(int* _out_, Color _in_)
		{
			Color_hsv_i(&_out_[0], &_out_[1], &_out_[2], (Color) (_in_));
		}

		public static void Color_hsv_bv(byte* _out_, Color _in_)
		{
			int* tmp = stackalloc int[4];
			Color_hsv_i(&tmp[0], &tmp[1], &tmp[2], (Color) (_in_));
			_out_[0] = ((byte) (tmp[0]));
			_out_[1] = ((byte) (tmp[1]));
			_out_[2] = ((byte) (tmp[2]));
		}

		public static nk_handle nk_handle_ptr(void* ptr)
		{
			nk_handle handle = new nk_handle();
			handle.ptr = ptr;
			return (nk_handle) (handle);
		}

		public static nk_handle nk_handle_id(int id)
		{
			nk_handle handle = new nk_handle();
			nk_zero(&handle, (ulong) (sizeof (nk_handle)));
			handle.id = (int) (id);
			return (nk_handle) (handle);
		}

		public static nk_image nk_subimage_ptr(void* ptr, ushort w, ushort h, RectangleF r)
		{
			nk_image s = new nk_image();

			s.handle.ptr = ptr;
			s.Width = (ushort) (w);
			s.Height = (ushort) (h);
			s.region[0] = ((ushort) (r.X));
			s.region[1] = ((ushort) (r.Y));
			s.region[2] = ((ushort) (r.Width));
			s.region[3] = ((ushort) (r.Height));
			return (nk_image) (s);
		}

		public static nk_image nk_subimage_id(int id, ushort w, ushort h, RectangleF r)
		{
			nk_image s = new nk_image();

			s.handle.id = (int) (id);
			s.Width = (ushort) (w);
			s.Height = (ushort) (h);
			s.region[0] = ((ushort) (r.X));
			s.region[1] = ((ushort) (r.Y));
			s.region[2] = ((ushort) (r.Width));
			s.region[3] = ((ushort) (r.Height));
			return (nk_image) (s);
		}

		public static nk_image nk_image_ptr(void* ptr)
		{
			nk_image s = new nk_image();

			s.handle.ptr = ptr;
			s.Width = (ushort) (0);
			s.Height = (ushort) (0);
			s.region[0] = (ushort) (0);
			s.region[1] = (ushort) (0);
			s.region[2] = (ushort) (0);
			s.region[3] = (ushort) (0);
			return (nk_image) (s);
		}

		public static nk_image nk_image_id(int id)
		{
			nk_image s = new nk_image();

			s.handle.id = (int) (id);
			s.Width = (ushort) (0);
			s.Height = (ushort) (0);
			s.region[0] = (ushort) (0);
			s.region[1] = (ushort) (0);
			s.region[2] = (ushort) (0);
			s.region[3] = (ushort) (0);
			return (nk_image) (s);
		}

		public static int nk_text_clamp(nk_user_font font, char* text, int text_len, float space, int* glyphs,
			float* text_width, uint* sep_list, int sep_count)
		{
			int i = (int) (0);
			int glyph_len = (int) (0);
			float last_width = (float) (0);
			char unicode = (char) 0;
			float width = (float) (0);
			int len = (int) (0);
			int g = (int) (0);
			float s;
			int sep_len = (int) (0);
			int sep_g = (int) (0);
			float sep_width = (float) (0);
			sep_count = (int) ((sep_count) < (0) ? (0) : (sep_count));
			glyph_len = (int) (nk_utf_decode(text, &unicode, (int) (text_len)));
			while ((((glyph_len) != 0) && ((width) < (space))) && ((len) < (text_len)))
			{
				len += (int) (glyph_len);
				s = (float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), text, (int) (len)));
				for (i = (int) (0); (i) < (sep_count); ++i)
				{
					if (unicode != sep_list[i]) continue;
					sep_width = (float) (last_width = (float) (width));
					sep_g = (int) (g + 1);
					sep_len = (int) (len);
					break;
				}
				if ((i) == (sep_count))
				{
					last_width = (float) (sep_width = (float) (width));
					sep_g = (int) (g + 1);
				}
				width = (float) (s);
				glyph_len = (int) (nk_utf_decode(text + len, &unicode, (int) (text_len - len)));
				g++;
			}
			if ((len) >= (text_len))
			{
				*glyphs = (int) (g);
				*text_width = (float) (last_width);
				return (int) (len);
			}
			else
			{
				*glyphs = (int) (sep_g);
				*text_width = (float) (sep_width);
				return (int) ((sep_len == 0) ? len : sep_len);
			}

		}

		public static Vector2 nk_text_calculate_text_bounds(nk_user_font font, char* begin, int byte_len, float row_height,
			char** remaining, Vector2* out_offset, int* glyphs, int op)
		{
			float line_height = (float) (row_height);
			Vector2 text_size = (Vector2) (new Vector2((float) (0), (float) (0)));
			float line_width = (float) (0.0f);
			float glyph_width;
			int glyph_len = (int) (0);
			char unicode = (char) 0;
			int text_len = (int) (0);
			if (((begin == null) || (byte_len <= 0)) || (font == null))
				return (Vector2) (new Vector2((float) (0), (float) (row_height)));
			glyph_len = (int) (nk_utf_decode(begin, &unicode, (int) (byte_len)));
			if (glyph_len == 0) return (Vector2) (text_size);
			glyph_width = (float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), begin, (int) (glyph_len)));
			*glyphs = (int) (0);
			while (((text_len) < (byte_len)) && ((glyph_len) != 0))
			{
				if ((unicode) == ('\n'))
				{
					text_size.X = (float) ((text_size.X) < (line_width) ? (line_width) : (text_size.X));
					text_size.Y += (float) (line_height);
					line_width = (float) (0);
					*glyphs += (int) (1);
					if ((op) == (NK_STOP_ON_NEW_LINE)) break;
					text_len++;
					glyph_len = (int) (nk_utf_decode(begin + text_len, &unicode, (int) (byte_len - text_len)));
					continue;
				}
				if ((unicode) == ('\r'))
				{
					text_len++;
					*glyphs += (int) (1);
					glyph_len = (int) (nk_utf_decode(begin + text_len, &unicode, (int) (byte_len - text_len)));
					continue;
				}
				*glyphs = (int) (*glyphs + 1);
				text_len += (int) (glyph_len);
				line_width += (float) (glyph_width);
				glyph_len = (int) (nk_utf_decode(begin + text_len, &unicode, (int) (byte_len - text_len)));
				glyph_width =
					(float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), begin + text_len, (int) (glyph_len)));
				continue;
			}
			if ((text_size.X) < (line_width)) text_size.X = (float) (line_width);
			if ((out_offset) != null)
				*out_offset = (Vector2) (new Vector2((float) (line_width), (float) (text_size.Y + line_height)));
			if (((line_width) > (0)) || ((text_size.Y) == (0.0f))) text_size.Y += (float) (line_height);
			if ((remaining) != null) *remaining = begin + text_len;
			return (Vector2) (text_size);
		}

		public static void* nk_buffer_align(void* unaligned, ulong align, ulong* alignment, int type)
		{
			void* memory = null;
			switch (type)
			{
				default:
				case NK_BUFFER_MAX:
				case NK_BUFFER_FRONT:
					if ((align) != 0)
					{
						memory = ((void*) ((long) (((ulong) ((long) ((byte*) (unaligned) + (align - 1)))) & ~(align - 1))));
						*alignment = ((ulong) ((byte*) (memory) - (byte*) (unaligned)));
					}
					else
					{
						memory = unaligned;
						*alignment = (ulong) (0);
					}
					break;
				case NK_BUFFER_BACK:
					if ((align) != 0)
					{
						memory = ((void*) ((long) (((ulong) ((long) ((byte*) (unaligned)))) & ~(align - 1))));
						*alignment = ((ulong) ((byte*) (unaligned) - (byte*) (memory)));
					}
					else
					{
						memory = unaligned;
						*alignment = (ulong) (0);
					}
					break;
			}

			return memory;
		}

		public static int RectangleF_height_compare(void* a, void* b)
		{
			nk_rp_rect* p = (nk_rp_rect*) (a);
			nk_rp_rect* q = (nk_rp_rect*) (b);
			if ((p->h) > (q->h)) return (int) (-1);
			if ((p->h) < (q->h)) return (int) (1);
			return (int) (((p->w) > (q->w)) ? -1 : ((p->w) < (q->w)) ? 1 : 0);
		}

		public static int RectangleF_original_order(void* a, void* b)
		{
			nk_rp_rect* p = (nk_rp_rect*) (a);
			nk_rp_rect* q = (nk_rp_rect*) (b);
			return (int) (((p->was_packed) < (q->was_packed)) ? -1 : ((p->was_packed) > (q->was_packed)) ? 1 : 0);
		}

		public static ushort nk_ttUSHORT(byte* p)
		{
			return (ushort) (p[0]*256 + p[1]);
		}

		public static short nk_ttSHORT(byte* p)
		{
			return (short) (p[0]*256 + p[1]);
		}

		public static uint nk_ttULONG(byte* p)
		{
			return (uint) ((p[0] << 24) + (p[1] << 16) + (p[2] << 8) + p[3]);
		}

		public static uint nk_tt__find_table(byte* data, uint fontstart, string tag)
		{
			int num_tables = (int) (nk_ttUSHORT(data + fontstart + 4));
			uint tabledir = (uint) (fontstart + 12);
			int i;
			for (i = (int) (0); (i) < (num_tables); ++i)
			{
				uint loc = (uint) (tabledir + (uint) (16*i));
				if (((((((data + loc + 0)[0]) == (tag[0])) && (((data + loc + 0)[1]) == (tag[1]))) &&
				      (((data + loc + 0)[2]) == (tag[2]))) && (((data + loc + 0)[3]) == (tag[3]))))
					return (uint) (nk_ttULONG(data + loc + 8));
			}
			return (uint) (0);
		}

		public static void nk_tt__handle_clipped_edge(float* scanline, int x, nk_tt__active_edge* e, float x0, float y0,
			float x1, float y1)
		{
			if ((y0) == (y1)) return;
			if ((y0) > (e->ey)) return;
			if ((y1) < (e->sy)) return;
			if ((y0) < (e->sy))
			{
				x0 += (float) ((x1 - x0)*(e->sy - y0)/(y1 - y0));
				y0 = (float) (e->sy);
			}

			if ((y1) > (e->ey))
			{
				x1 += (float) ((x1 - x0)*(e->ey - y1)/(y1 - y0));
				y1 = (float) (e->ey);
			}

			if ((x0 <= x) && (x1 <= x)) scanline[x] += (float) (e->direction*(y1 - y0));
			else if (((x0) >= (x + 1)) && ((x1) >= (x + 1)))
			{
			}
			else
			{
				scanline[x] += (float) (e->direction*(y1 - y0)*(1.0f - ((x0 - (float) (x)) + (x1 - (float) (x)))/2.0f));
			}

		}

		public static void nk_tt__fill_active_edges_new(float* scanline, float* scanline_fill, int len, nk_tt__active_edge* e,
			float y_top)
		{
			float y_bottom = (float) (y_top + 1);
			while ((e) != null)
			{
				if ((e->fdx) == (0))
				{
					float x0 = (float) (e->fx);
					if ((x0) < (len))
					{
						if ((x0) >= (0))
						{
							nk_tt__handle_clipped_edge(scanline, (int) (x0), e, (float) (x0), (float) (y_top), (float) (x0),
								(float) (y_bottom));
							nk_tt__handle_clipped_edge(scanline_fill - 1, (int) ((int) (x0) + 1), e, (float) (x0), (float) (y_top),
								(float) (x0), (float) (y_bottom));
						}
						else
						{
							nk_tt__handle_clipped_edge(scanline_fill - 1, (int) (0), e, (float) (x0), (float) (y_top), (float) (x0),
								(float) (y_bottom));
						}
					}
				}
				else
				{
					float x0 = (float) (e->fx);
					float dx = (float) (e->fdx);
					float xb = (float) (x0 + dx);
					float x_top;
					float x_bottom;
					float y0;
					float y1;
					float dy = (float) (e->fdy);
					if ((e->sy) > (y_top))
					{
						x_top = (float) (x0 + dx*(e->sy - y_top));
						y0 = (float) (e->sy);
					}
					else
					{
						x_top = (float) (x0);
						y0 = (float) (y_top);
					}
					if ((e->ey) < (y_bottom))
					{
						x_bottom = (float) (x0 + dx*(e->ey - y_top));
						y1 = (float) (e->ey);
					}
					else
					{
						x_bottom = (float) (xb);
						y1 = (float) (y_bottom);
					}
					if (((((x_top) >= (0)) && ((x_bottom) >= (0))) && ((x_top) < (len))) && ((x_bottom) < (len)))
					{
						if (((int) (x_top)) == ((int) (x_bottom)))
						{
							float height;
							int x = (int) (x_top);
							height = (float) (y1 - y0);
							scanline[x] += (float) (e->direction*(1.0f - ((x_top - (float) (x)) + (x_bottom - (float) (x)))/2.0f)*height);
							scanline_fill[x] += (float) (e->direction*height);
						}
						else
						{
							int x;
							int x1;
							int x2;
							float y_crossing;
							float step;
							float sign;
							float area;
							if ((x_top) > (x_bottom))
							{
								float t;
								y0 = (float) (y_bottom - (y0 - y_top));
								y1 = (float) (y_bottom - (y1 - y_top));
								t = (float) (y0);
								y0 = (float) (y1);
								y1 = (float) (t);
								t = (float) (x_bottom);
								x_bottom = (float) (x_top);
								x_top = (float) (t);
								dx = (float) (-dx);
								dy = (float) (-dy);
								t = (float) (x0);
								x0 = (float) (xb);
								xb = (float) (t);
							}
							x1 = ((int) (x_top));
							x2 = ((int) (x_bottom));
							y_crossing = (float) (((float) (x1) + 1 - x0)*dy + y_top);
							sign = (float) (e->direction);
							area = (float) (sign*(y_crossing - y0));
							scanline[x1] += (float) (area*(1.0f - ((x_top - (float) (x1)) + (float) (x1 + 1 - x1))/2.0f));
							step = (float) (sign*dy);
							for (x = (int) (x1 + 1); (x) < (x2); ++x)
							{
								scanline[x] += (float) (area + step/2);
								area += (float) (step);
							}
							y_crossing += (float) (dy*(float) (x2 - (x1 + 1)));
							scanline[x2] +=
								(float) (area + sign*(1.0f - ((float) (x2 - x2) + (x_bottom - (float) (x2)))/2.0f)*(y1 - y_crossing));
							scanline_fill[x2] += (float) (sign*(y1 - y0));
						}
					}
					else
					{
						int x;
						for (x = (int) (0); (x) < (len); ++x)
						{
							float ya = (float) (y_top);
							float x1 = (float) (x);
							float x2 = (float) (x + 1);
							float x3 = (float) (xb);
							float y3 = (float) (y_bottom);
							float yb;
							float y2;
							yb = (float) (((float) (x) - x0)/dx + y_top);
							y2 = (float) (((float) (x) + 1 - x0)/dx + y_top);
							if (((x0) < (x1)) && ((x3) > (x2)))
							{
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x0), (float) (ya), (float) (x1), (float) (yb));
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x1), (float) (yb), (float) (x2), (float) (y2));
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x2), (float) (y2), (float) (x3), (float) (y3));
							}
							else if (((x3) < (x1)) && ((x0) > (x2)))
							{
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x0), (float) (ya), (float) (x2), (float) (y2));
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x2), (float) (y2), (float) (x1), (float) (yb));
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x1), (float) (yb), (float) (x3), (float) (y3));
							}
							else if (((x0) < (x1)) && ((x3) > (x1)))
							{
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x0), (float) (ya), (float) (x1), (float) (yb));
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x1), (float) (yb), (float) (x3), (float) (y3));
							}
							else if (((x3) < (x1)) && ((x0) > (x1)))
							{
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x0), (float) (ya), (float) (x1), (float) (yb));
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x1), (float) (yb), (float) (x3), (float) (y3));
							}
							else if (((x0) < (x2)) && ((x3) > (x2)))
							{
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x0), (float) (ya), (float) (x2), (float) (y2));
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x2), (float) (y2), (float) (x3), (float) (y3));
							}
							else if (((x3) < (x2)) && ((x0) > (x2)))
							{
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x0), (float) (ya), (float) (x2), (float) (y2));
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x2), (float) (y2), (float) (x3), (float) (y3));
							}
							else
							{
								nk_tt__handle_clipped_edge(scanline, (int) (x), e, (float) (x0), (float) (ya), (float) (x3), (float) (y3));
							}
						}
					}
				}
				e = e->next;
			}
		}

		public static void nk_tt__h_prefilter(byte* pixels, int w, int h, int stride_in_bytes, int kernel_width)
		{
			byte* buffer = stackalloc byte[8];
			int safe_w = (int) (w - kernel_width);
			int j;
			for (j = (int) (0); (j) < (h); ++j)
			{
				int i;
				uint total;
				nk_memset(buffer, (int) (0), (ulong) (kernel_width));
				total = (uint) (0);
				switch (kernel_width)
				{
					case 2:
						for (i = (int) (0); i <= safe_w; ++i)
						{
							total += ((uint) (pixels[i] - buffer[i & (8 - 1)]));
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i]);
							pixels[i] = ((byte) (total/2));
						}
						break;
					case 3:
						for (i = (int) (0); i <= safe_w; ++i)
						{
							total += ((uint) (pixels[i] - buffer[i & (8 - 1)]));
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i]);
							pixels[i] = ((byte) (total/3));
						}
						break;
					case 4:
						for (i = (int) (0); i <= safe_w; ++i)
						{
							total += (uint) ((uint) (pixels[i]) - buffer[i & (8 - 1)]);
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i]);
							pixels[i] = ((byte) (total/4));
						}
						break;
					case 5:
						for (i = (int) (0); i <= safe_w; ++i)
						{
							total += ((uint) (pixels[i] - buffer[i & (8 - 1)]));
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i]);
							pixels[i] = ((byte) (total/5));
						}
						break;
					default:
						for (i = (int) (0); i <= safe_w; ++i)
						{
							total += ((uint) (pixels[i] - buffer[i & (8 - 1)]));
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i]);
							pixels[i] = ((byte) (total/(uint) (kernel_width)));
						}
						break;
				}
				for (; (i) < (w); ++i)
				{
					total -= ((uint) (buffer[i & (8 - 1)]));
					pixels[i] = ((byte) (total/(uint) (kernel_width)));
				}
				pixels += stride_in_bytes;
			}
		}

		public static void nk_tt__v_prefilter(byte* pixels, int w, int h, int stride_in_bytes, int kernel_width)
		{
			byte* buffer = stackalloc byte[8];
			int safe_h = (int) (h - kernel_width);
			int j;
			for (j = (int) (0); (j) < (w); ++j)
			{
				int i;
				uint total;
				nk_memset(buffer, (int) (0), (ulong) (kernel_width));
				total = (uint) (0);
				switch (kernel_width)
				{
					case 2:
						for (i = (int) (0); i <= safe_h; ++i)
						{
							total += ((uint) (pixels[i*stride_in_bytes] - buffer[i & (8 - 1)]));
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i*stride_in_bytes]);
							pixels[i*stride_in_bytes] = ((byte) (total/2));
						}
						break;
					case 3:
						for (i = (int) (0); i <= safe_h; ++i)
						{
							total += ((uint) (pixels[i*stride_in_bytes] - buffer[i & (8 - 1)]));
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i*stride_in_bytes]);
							pixels[i*stride_in_bytes] = ((byte) (total/3));
						}
						break;
					case 4:
						for (i = (int) (0); i <= safe_h; ++i)
						{
							total += ((uint) (pixels[i*stride_in_bytes] - buffer[i & (8 - 1)]));
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i*stride_in_bytes]);
							pixels[i*stride_in_bytes] = ((byte) (total/4));
						}
						break;
					case 5:
						for (i = (int) (0); i <= safe_h; ++i)
						{
							total += ((uint) (pixels[i*stride_in_bytes] - buffer[i & (8 - 1)]));
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i*stride_in_bytes]);
							pixels[i*stride_in_bytes] = ((byte) (total/5));
						}
						break;
					default:
						for (i = (int) (0); i <= safe_h; ++i)
						{
							total += ((uint) (pixels[i*stride_in_bytes] - buffer[i & (8 - 1)]));
							buffer[(i + kernel_width) & (8 - 1)] = (byte) (pixels[i*stride_in_bytes]);
							pixels[i*stride_in_bytes] = ((byte) (total/(uint) (kernel_width)));
						}
						break;
				}
				for (; (i) < (h); ++i)
				{
					total -= ((uint) (buffer[i & (8 - 1)]));
					pixels[i*stride_in_bytes] = ((byte) (total/(uint) (kernel_width)));
				}
				pixels += 1;
			}
		}

		public static float nk_tt__oversample_shift(int oversample)
		{
			if (oversample == 0) return (float) (0.0f);
			return (float) ((float) (-(oversample - 1))/(2.0f*(float) (oversample)));
		}

		public static int nk_range_count(uint* range)
		{
			uint* iter = range;
			if (range == null) return (int) (0);
			while (*(iter++) != 0)
			{
			}
			return (int) (((iter) == (range)) ? 0 : (int) ((iter - range)/2));
		}

		public static int nk_range_glyph_count(uint* range, int count)
		{
			int i = (int) (0);
			int total_glyphs = (int) (0);
			for (i = (int) (0); (i) < (count); ++i)
			{
				int diff;
				uint f = (uint) (range[(i*2) + 0]);
				uint t = (uint) (range[(i*2) + 1]);
				diff = ((int) ((t - f) + 1));
				total_glyphs += (int) (diff);
			}
			return (int) (total_glyphs);
		}

		public static void nk_font_baker_memory(ulong* temp, ref int glyph_count, nk_font_config config_list, int count)
		{
			int range_count = (int) (0);
			int total_range_count = (int) (0);
			nk_font_config iter;
			nk_font_config i;
			if (config_list == null)
			{
				*temp = (ulong) (0);
				glyph_count = (int) (0);
				return;
			}

			glyph_count = (int) (0);
			for (iter = config_list; iter != null; iter = iter.next)
			{
				i = iter;
				do
				{
					if (i.range == null) iter.range = nk_font_default_glyph_ranges();
					range_count = (int) (nk_range_count(i.range));
					total_range_count += (int) (range_count);
					glyph_count += (int) (nk_range_glyph_count(i.range, (int) (range_count)));
				} while ((i = i.n) != iter);
			}
			*temp = (ulong) ((ulong) (glyph_count)*(ulong) sizeof (nk_rp_rect));
			*temp += (ulong) ((ulong) (total_range_count)*(ulong) sizeof (nk_tt_pack_range));
			*temp += (ulong) ((ulong) (glyph_count)*(ulong) sizeof (nk_tt_packedchar));
			*temp += (ulong) ((ulong) (count)*(ulong) sizeof (nk_font_bake_data));
			*temp += (ulong) (sizeof (nk_font_baker));
			*temp += (ulong) (RectangleF_align + nk_range_align + nk_char_align);
			*temp += (ulong) (nk_build_align + nk_baker_align);
		}

		public static nk_font_baker* nk_font_baker_(void* memory, int glyph_count, int count)
		{
			nk_font_baker* baker;
			if (memory == null) return null;
			baker =
				(nk_font_baker*)
					((void*) ((long) (((ulong) ((long) ((byte*) (memory) + (nk_baker_align - 1)))) & ~(nk_baker_align - 1))));
			baker->build =
				(nk_font_bake_data*)
					((void*) ((long) (((ulong) ((long) ((byte*) (baker + 1) + (nk_build_align - 1)))) & ~(nk_build_align - 1))));
			baker->packed_chars =
				(nk_tt_packedchar*)
					((void*)
						((long) (((ulong) ((long) ((byte*) (baker->build + count) + (nk_char_align - 1)))) & ~(nk_char_align - 1))));
			baker->rects =
				(nk_rp_rect*)
					((void*)
						((long)
							(((ulong) ((long) ((byte*) (baker->packed_chars + glyph_count) + (RectangleF_align - 1)))) & ~(RectangleF_align - 1))));
			baker->ranges =
				(nk_tt_pack_range*)
					((void*)
						((long) (((ulong) ((long) ((byte*) (baker->rects + glyph_count) + (nk_range_align - 1)))) & ~(nk_range_align - 1))));

			return baker;
		}

		public static void nk_font_bake_custom_data(void* img_memory, int img_width, int img_height, RectangleFi img_dst,
			byte* texture_data_mask, int tex_width, int tex_height, char white, char black)
		{
			byte* pixels;
			int y = (int) (0);
			int x = (int) (0);
			int n = (int) (0);
			if ((((img_memory == null) || (img_width == 0)) || (img_height == 0)) || (texture_data_mask == null)) return;
			pixels = (byte*) (img_memory);
			for (y = (int) (0) , n = (int) (0); (y) < (tex_height); ++y)
			{
				for (x = (int) (0); (x) < (tex_width); ++x , ++n)
				{
					int off0 = (int) ((img_dst.X + x) + (img_dst.Y + y)*img_width);
					int off1 = (int) (off0 + 1 + tex_width);
					pixels[off0] = (byte) (((texture_data_mask[n]) == (white)) ? 0xFF : 0x00);
					pixels[off1] = (byte) (((texture_data_mask[n]) == (black)) ? 0xFF : 0x00);
				}
			}
		}

		public static void nk_font_bake_convert(void* out_memory, int img_width, int img_height, void* in_memory)
		{
			int n = (int) (0);
			uint* dst;
			byte* src;
			if ((((out_memory == null) || (in_memory == null)) || (img_height == 0)) || (img_width == 0)) return;
			dst = (uint*) (out_memory);
			src = (byte*) (in_memory);
			for (n = (int) (img_width*img_height); (n) > (0); n--)
			{
				*dst++ = (uint) (((uint) (*src++) << 24) | 0x00FFFFFF);
			}
		}

		public static uint nk_decompress_length(byte* input)
		{
			return (uint) ((input[8] << 24) + (input[9] << 16) + (input[10] << 8) + input[11]);
		}

		public static void nk__match(byte* data, uint length)
		{
			if ((nk__dout + length) > (nk__barrier))
			{
				nk__dout += length;
				return;
			}

			if ((data) < (nk__barrier4))
			{
				nk__dout = nk__barrier + 1;
				return;
			}

			while ((length--) != 0)
			{
				*nk__dout++ = (byte) (*data++);
			}
		}

		public static void nk__lit(byte* data, uint length)
		{
			if ((nk__dout + length) > (nk__barrier))
			{
				nk__dout += length;
				return;
			}

			if ((data) < (nk__barrier2))
			{
				nk__dout = nk__barrier + 1;
				return;
			}

			nk_memcopy(nk__dout, data, (ulong) (length));
			nk__dout += length;
		}

		public static byte* nk_decompress_token(byte* i)
		{
			if ((*i) >= (0x20))
			{
				if ((*i) >= (0x80))
				{
					nk__match(nk__dout - i[1] - 1, (uint) ((uint) (i[0]) - 0x80 + 1));
					i += 2;
				}
				else if ((*i) >= (0x40))
				{
					nk__match(nk__dout - (((i[0] << 8) + i[(0) + 1]) - 0x4000 + 1), (uint) ((uint) (i[2]) + 1));
					i += 3;
				}
				else
				{
					nk__lit(i + 1, (uint) ((uint) (i[0]) - 0x20 + 1));
					i += 1 + (i[0] - 0x20 + 1);
				}
			}
			else
			{
				if ((*i) >= (0x18))
				{
					nk__match(nk__dout - (uint) (((i[0] << 16) + ((i[(0) + 1] << 8) + i[((0) + 1) + 1])) - 0x180000 + 1),
						(uint) ((uint) (i[3]) + 1));
					i += 4;
				}
				else if ((*i) >= (0x10))
				{
					nk__match(nk__dout - (uint) (((i[0] << 16) + ((i[(0) + 1] << 8) + i[((0) + 1) + 1])) - 0x100000 + 1),
						(uint) ((uint) ((i[3] << 8) + i[(3) + 1]) + 1));
					i += 5;
				}
				else if ((*i) >= (0x08))
				{
					nk__lit(i + 2, (uint) ((uint) ((i[0] << 8) + i[(0) + 1]) - 0x0800 + 1));
					i += 2 + (((i[0] << 8) + i[(0) + 1]) - 0x0800 + 1);
				}
				else if ((*i) == (0x07))
				{
					nk__lit(i + 3, (uint) ((uint) ((i[1] << 8) + i[(1) + 1]) + 1));
					i += 3 + (((i[1] << 8) + i[(1) + 1]) + 1);
				}
				else if ((*i) == (0x06))
				{
					nk__match(nk__dout - (uint) (((i[1] << 16) + ((i[(1) + 1] << 8) + i[((1) + 1) + 1])) + 1), (uint) (i[4] + 1u));
					i += 5;
				}
				else if ((*i) == (0x04))
				{
					nk__match(nk__dout - (uint) (((i[1] << 16) + ((i[(1) + 1] << 8) + i[((1) + 1) + 1])) + 1),
						(uint) ((uint) ((i[4] << 8) + i[(4) + 1]) + 1u));
					i += 6;
				}
			}

			return i;
		}

		public static uint nk_adler32(uint adler32, byte* buffer, uint buflen)
		{
			int ADLER_MOD = (int) (65521);
			int s1 = (int) (adler32 & 0xffff);
			int s2 = (int) (adler32 >> 16);
			int blocklen;
			int i;
			blocklen = (int) (buflen%5552);
			while ((buflen) != 0)
			{
				for (i = (int) (0); (i + 7) < (blocklen); i += (int) (8))
				{
					s1 += (int) (buffer[0]);
					s2 += (int) (s1);
					s1 += (int) (buffer[1]);
					s2 += (int) (s1);
					s1 += (int) (buffer[2]);
					s2 += (int) (s1);
					s1 += (int) (buffer[3]);
					s2 += (int) (s1);
					s1 += (int) (buffer[4]);
					s2 += (int) (s1);
					s1 += (int) (buffer[5]);
					s2 += (int) (s1);
					s1 += (int) (buffer[6]);
					s2 += (int) (s1);
					s1 += (int) (buffer[7]);
					s2 += (int) (s1);
					buffer += 8;
				}
				for (; (i) < (blocklen); ++i)
				{
					s1 += (int) (*buffer++);
					s2 += (int) (s1);
				}
				s1 %= (int) (ADLER_MOD);
				s2 %= (int) (ADLER_MOD);
				buflen -= ((uint) (blocklen));
				blocklen = (int) (5552);
			}
			return (uint) ((uint) (s2 << 16) + (uint) (s1));
		}

		public static uint nk_decompress(byte* output, byte* i, uint length)
		{
			uint olen;
			if (((i[0] << 24) + ((i[(0) + 1] << 16) + ((i[((0) + 1) + 1] << 8) + i[(((0) + 1) + 1) + 1]))) != 0x57bC0000)
				return (uint) (0);
			if (((i[4] << 24) + ((i[(4) + 1] << 16) + ((i[((4) + 1) + 1] << 8) + i[(((4) + 1) + 1) + 1]))) != 0)
				return (uint) (0);
			olen = (uint) (nk_decompress_length(i));
			nk__barrier2 = i;
			nk__barrier3 = i + length;
			nk__barrier = output + olen;
			nk__barrier4 = output;
			i += 16;
			nk__dout = output;
			for (;;)
			{
				byte* old_i = i;
				i = nk_decompress_token(i);
				if ((i) == (old_i))
				{
					if (((*i) == (0x05)) && ((i[1]) == (0xfa)))
					{
						if (nk__dout != output + olen) return (uint) (0);
						if (nk_adler32((uint) (1), output, (uint) (olen)) !=
						    (uint) ((i[2] << 24) + ((i[(2) + 1] << 16) + ((i[((2) + 1) + 1] << 8) + i[(((2) + 1) + 1) + 1]))))
							return (uint) (0);
						return (uint) (olen);
					}
					else
					{
						return (uint) (0);
					}
				}
				if ((nk__dout) > (output + olen)) return (uint) (0);
			}
		}

		public static uint nk_decode_85_byte(sbyte c)
		{
			return (uint) (((c) >= ('\\')) ? c - 36 : c - 35);
		}

		public static void nk_decode_85(byte* dst, byte* src)
		{
			while ((*src) != 0)
			{
				uint tmp =
					(uint)
						(nk_decode_85_byte((sbyte) (src[0])) +
						 85*
						 (nk_decode_85_byte((sbyte) (src[1])) +
						  85*
						  (nk_decode_85_byte((sbyte) (src[2])) +
						   85*(nk_decode_85_byte((sbyte) (src[3])) + 85*nk_decode_85_byte((sbyte) (src[4]))))));
				dst[0] = ((byte) ((tmp >> 0) & 0xFF));
				dst[1] = ((byte) ((tmp >> 8) & 0xFF));
				dst[2] = ((byte) ((tmp >> 16) & 0xFF));
				dst[3] = ((byte) ((tmp >> 24) & 0xFF));
				src += 5;
				dst += 4;
			}
		}

		public static nk_font_config nk_font_config_(float pixel_height)
		{
			nk_font_config cfg = new nk_font_config();

			cfg.ttf_blob = null;
			cfg.ttf_size = (ulong) (0);
			cfg.ttf_data_owned_by_atlas = (byte) (0);
			cfg.size = (float) (pixel_height);
			cfg.oversample_h = (byte) (3);
			cfg.oversample_v = (byte) (1);
			cfg.pixel_snap = (byte) (0);
			cfg.coord_type = (int) (NK_COORD_UV);
			cfg.spacing = (Vector2) (new Vector2((float) (0), (float) (0)));
			cfg.range = nk_font_default_glyph_ranges();
			cfg.merge_mode = (byte) (0);
			cfg.fallback_glyph = '?';
			cfg.font = null;
			cfg.n = null;
			return (nk_font_config) (cfg);
		}

		public static int nk_button_behavior(ref uint state, RectangleF r, nk_input i, int behavior)
		{
			int ret = (int) (0);
			if (((state) & NK_WIDGET_STATE_MODIFIED) != 0)
				(state) = (uint) (NK_WIDGET_STATE_INACTIVE | NK_WIDGET_STATE_MODIFIED);
			else (state) = (uint) (NK_WIDGET_STATE_INACTIVE);
			if (i == null) return (int) (0);
			if ((nk_input_is_mouse_hovering_rect(i, (RectangleF) (r))) != 0)
			{
				state = (uint) (NK_WIDGET_STATE_HOVERED);
				if ((nk_input_is_mouse_down(i, (int) (NK_BUTTON_LEFT))) != 0) state = (uint) (NK_WIDGET_STATE_ACTIVE);
				if ((nk_input_has_mouse_click_in_rect(i, (int) (NK_BUTTON_LEFT), (RectangleF) (r))) != 0)
				{
					ret =
						(int)
							((behavior != NK_BUTTON_DEFAULT)
								? nk_input_is_mouse_down(i, (int) (NK_BUTTON_LEFT))
								: nk_input_is_mouse_pressed(i, (int) (NK_BUTTON_LEFT)));
				}
			}

			if (((state & NK_WIDGET_STATE_HOVER) != 0) && (nk_input_is_mouse_prev_hovering_rect(i, (RectangleF) (r)) == 0))
				state |= (uint) (NK_WIDGET_STATE_ENTERED);
			else if ((nk_input_is_mouse_prev_hovering_rect(i, (RectangleF) (r))) != 0) state |= (uint) (NK_WIDGET_STATE_LEFT);
			return (int) (ret);
		}

		public static int nk_do_button(ref uint state, nk_command_buffer _out_, RectangleF r, nk_style_button style,
			nk_input _in_, int behavior, RectangleF* content)
		{
			RectangleF bounds = new RectangleF();
			if ((_out_ == null) || (style == null)) return (int) (nk_false);
			content->x = (float) (r.X + style.padding.X + style.border + style.rounding);
			content->y = (float) (r.Y + style.padding.Y + style.border + style.rounding);
			content->w = (float) (r.Width - (2*style.padding.X + style.border + style.rounding*2));
			content->h = (float) (r.Height - (2*style.padding.Y + style.border + style.rounding*2));
			bounds.X = (float) (r.X - style.touch_padding.X);
			bounds.Y = (float) (r.Y - style.touch_padding.Y);
			bounds.Width = (float) (r.Width + 2*style.touch_padding.X);
			bounds.Height = (float) (r.Height + 2*style.touch_padding.Y);
			return (int) (nk_button_behavior(ref state, (RectangleF) (bounds), _in_, (int) (behavior)));
		}

		public static int nk_do_button_text(ref uint state, nk_command_buffer _out_, RectangleF bounds, char* _string_, int len,
			uint align, int behavior, nk_style_button style, nk_input _in_, nk_user_font font)
		{
			RectangleF content = new RectangleF();
			int ret = (int) (nk_false);
			if ((((_out_ == null) || (style == null)) || (font == null)) || (_string_ == null)) return (int) (nk_false);
			ret = (int) (nk_do_button(ref state, _out_, (RectangleF) (bounds), style, _in_, (int) (behavior), &content));
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_button_text(_out_, &bounds, &content, (uint) (state), style, _string_, (int) (len), (uint) (align), font);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return (int) (ret);
		}

		public static int nk_do_button_symbol(ref uint state, nk_command_buffer _out_, RectangleF bounds, int symbol,
			int behavior, nk_style_button style, nk_input _in_, nk_user_font font)
		{
			int ret;
			RectangleF content = new RectangleF();
			if ((((_out_ == null) || (style == null)) || (font == null))) return (int) (nk_false);
			ret = (int) (nk_do_button(ref state, _out_, (RectangleF) (bounds), style, _in_, (int) (behavior), &content));
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_button_symbol(_out_, &bounds, &content, (uint) (state), style, (int) (symbol), font);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return (int) (ret);
		}

		public static int nk_do_button_image(ref uint state, nk_command_buffer _out_, RectangleF bounds, nk_image img, int b,
			nk_style_button style, nk_input _in_)
		{
			int ret;
			RectangleF content = new RectangleF();
			if (((_out_ == null) || (style == null))) return (int) (nk_false);
			ret = (int) (nk_do_button(ref state, _out_, (RectangleF) (bounds), style, _in_, (int) (b), &content));
			content.X += (float) (style.image_padding.X);
			content.Y += (float) (style.image_padding.Y);
			content.Width -= (float) (2*style.image_padding.X);
			content.Height -= (float) (2*style.image_padding.Y);
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_button_image(_out_, &bounds, &content, (uint) (state), style, img);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return (int) (ret);
		}

		public static int nk_do_button_text_symbol(ref uint state, nk_command_buffer _out_, RectangleF bounds, int symbol,
			char* str, int len, uint align, int behavior, nk_style_button style, nk_user_font font, nk_input _in_)
		{
			int ret;
			RectangleF tri = new RectangleF();
			RectangleF content = new RectangleF();
			if (((_out_ == null) || (style == null)) || (font == null)) return (int) (nk_false);
			ret = (int) (nk_do_button(ref state, _out_, (RectangleF) (bounds), style, _in_, (int) (behavior), &content));
			tri.Y = (float) (content.Y + (content.Height/2) - font.Height/2);
			tri.Width = (float) (font.Height);
			tri.Height = (float) (font.Height);
			if ((align & NK_TEXT_ALIGN_LEFT) != 0)
			{
				tri.X = (float) ((content.X + content.Width) - (2*style.padding.X + tri.Width));
				tri.X = (float) ((tri.X) < (0) ? (0) : (tri.X));
			}
			else tri.X = (float) (content.X + 2*style.padding.X);

			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_button_text_symbol(_out_, &bounds, &content, &tri, (uint) (state), style, str, (int) (len), (int) (symbol),
				font);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return (int) (ret);
		}

		public static int nk_do_button_text_image(ref uint state, nk_command_buffer _out_, RectangleF bounds, nk_image img,
			char* str, int len, uint align, int behavior, nk_style_button style, nk_user_font font, nk_input _in_)
		{
			int ret;
			RectangleF icon = new RectangleF();
			RectangleF content = new RectangleF();
			if ((((_out_ == null) || (font == null)) || (style == null)) || (str == null)) return (int) (nk_false);
			ret = (int) (nk_do_button(ref state, _out_, (RectangleF) (bounds), style, _in_, (int) (behavior), &content));
			icon.Y = (float) (bounds.Y + style.padding.Y);
			icon.Width = (float) (icon.Height = (float) (bounds.Height - 2*style.padding.Y));
			if ((align & NK_TEXT_ALIGN_LEFT) != 0)
			{
				icon.X = (float) ((bounds.X + bounds.Width) - (2*style.padding.X + icon.Width));
				icon.X = (float) ((icon.X) < (0) ? (0) : (icon.X));
			}
			else icon.X = (float) (bounds.X + 2*style.padding.X);
			icon.X += (float) (style.image_padding.X);
			icon.Y += (float) (style.image_padding.Y);
			icon.Width -= (float) (2*style.image_padding.X);
			icon.Height -= (float) (2*style.image_padding.Y);
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_button_text_image(_out_, &bounds, &content, &icon, (uint) (state), style, str, (int) (len), font, img);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return (int) (ret);
		}

		public static int nk_do_toggle(ref uint state, nk_command_buffer _out_, RectangleF r, int* active, char* str, int len,
			int type, nk_style_toggle style, nk_input _in_, nk_user_font font)
		{
			int was_active;
			RectangleF bounds = new RectangleF();
			RectangleF select = new RectangleF();
			RectangleF cursor = new RectangleF();
			RectangleF label = new RectangleF();
			if ((((_out_ == null) || (style == null)) || (font == null)) || (active == null)) return (int) (0);
			r.Width = (float) ((r.Width) < (font.Height + 2*style.padding.X) ? (font.Height + 2*style.padding.X) : (r.Width));
			r.Height = (float) ((r.Height) < (font.Height + 2*style.padding.Y) ? (font.Height + 2*style.padding.Y) : (r.Height));
			bounds.X = (float) (r.X - style.touch_padding.X);
			bounds.Y = (float) (r.Y - style.touch_padding.Y);
			bounds.Width = (float) (r.Width + 2*style.touch_padding.X);
			bounds.Height = (float) (r.Height + 2*style.touch_padding.Y);
			select.Width = (float) (font.Height);
			select.Height = (float) (select.Width);
			select.Y = (float) (r.Y + r.Height/2.0f - select.Height/2.0f);
			select.X = (float) (r.X);
			cursor.X = (float) (select.X + style.padding.X + style.border);
			cursor.Y = (float) (select.Y + style.padding.Y + style.border);
			cursor.Width = (float) (select.Width - (2*style.padding.X + 2*style.border));
			cursor.Height = (float) (select.Height - (2*style.padding.Y + 2*style.border));
			label.X = (float) (select.X + select.Width + style.spacing);
			label.Y = (float) (select.Y);
			label.Width = (float) (((r.X + r.Width) < (label.X) ? (label.X) : (r.X + r.Width)) - label.X);
			label.Height = (float) (select.Width);
			was_active = (int) (*active);
			*active = (int) (nk_toggle_behavior(_in_, (RectangleF) (bounds), ref state, (int) (*active)));
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			if ((type) == (NK_TOGGLE_CHECK))
			{
				nk_draw_checkbox(_out_, (uint) (state), style, (int) (*active), &label, &select, &cursor, str, (int) (len), font);
			}
			else
			{
				nk_draw_option(_out_, (uint) (state), style, (int) (*active), &label, &select, &cursor, str, (int) (len), font);
			}

			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return was_active != *active ? 1 : 0;
		}

		public static int nk_do_selectable(ref uint state, nk_command_buffer _out_, RectangleF bounds, char* str, int len,
			uint align, ref int value, nk_style_selectable style, nk_input _in_, nk_user_font font)
		{
			int old_value;
			RectangleF touch = new RectangleF();
			if (((((((_out_ == null)) || (str == null)) || (len == 0))) || (style == null)) ||
			    (font == null)) return (int) (0);
			old_value = (int) (value);
			touch.X = (float) (bounds.X - style.touch_padding.X);
			touch.Y = (float) (bounds.Y - style.touch_padding.Y);
			touch.Width = (float) (bounds.Width + style.touch_padding.X*2);
			touch.Height = (float) (bounds.Height + style.touch_padding.Y*2);
			if ((nk_button_behavior(ref state, (RectangleF) (touch), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
				value = value != 0 ? 0 : 1;
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_selectable(_out_, (uint) (state), style, (int) (value), &bounds, null, null, str, (int) (len), (uint) (align),
				font);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return old_value != value ? 1 : 0;
		}

		public static int nk_do_selectable_image(ref uint state, nk_command_buffer _out_, RectangleF bounds, char* str, int len,
			uint align, ref int value, nk_image img, nk_style_selectable style, nk_input _in_, nk_user_font font)
		{
			int old_value;
			RectangleF touch = new RectangleF();
			RectangleF icon = new RectangleF();
			if (((((((_out_ == null)) || (str == null)) || (len == 0))) || (style == null)) ||
			    (font == null)) return (int) (0);
			old_value = (int) (value);
			touch.X = (float) (bounds.X - style.touch_padding.X);
			touch.Y = (float) (bounds.Y - style.touch_padding.Y);
			touch.Width = (float) (bounds.Width + style.touch_padding.X*2);
			touch.Height = (float) (bounds.Height + style.touch_padding.Y*2);
			if ((nk_button_behavior(ref state, (RectangleF) (touch), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
				value = value != 0 ? 0 : 1;
			icon.Y = (float) (bounds.Y + style.padding.Y);
			icon.Width = (float) (icon.Height = (float) (bounds.Height - 2*style.padding.Y));
			if ((align & NK_TEXT_ALIGN_LEFT) != 0)
			{
				icon.X = (float) ((bounds.X + bounds.Width) - (2*style.padding.X + icon.Width));
				icon.X = (float) ((icon.X) < (0) ? (0) : (icon.X));
			}
			else icon.X = (float) (bounds.X + 2*style.padding.X);
			icon.X += (float) (style.image_padding.X);
			icon.Y += (float) (style.image_padding.Y);
			icon.Width -= (float) (2*style.image_padding.X);
			icon.Height -= (float) (2*style.image_padding.Y);
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_selectable(_out_, (uint) (state), style, (int) (value), &bounds, &icon, img, str, (int) (len), (uint) (align),
				font);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return old_value != value ? 1 : 0;
		}

		public static float nk_slider_behavior(ref uint state, RectangleF* logical_cursor, RectangleF* visual_cursor, nk_input _in_,
			RectangleF bounds, float slider_min, float slider_max, float slider_value, float slider_step, float slider_steps)
		{
			int left_mouse_down;
			int left_mouse_click_in_cursor;
			if (((state) & NK_WIDGET_STATE_MODIFIED) != 0)
				(state) = (uint) (NK_WIDGET_STATE_INACTIVE | NK_WIDGET_STATE_MODIFIED);
			else (state) = (uint) (NK_WIDGET_STATE_INACTIVE);
			left_mouse_down =
				(int) (((_in_) != null) && ((_in_.mouse.buttons[NK_BUTTON_LEFT].down) != 0) ? 1 : 0);
			left_mouse_click_in_cursor =
				(int)
					(((_in_) != null) &&
					 ((nk_input_has_mouse_click_down_in_rect(_in_, (int) (NK_BUTTON_LEFT), (RectangleF) (*visual_cursor), (int) (nk_true))) !=
					  0)
						? 1
						: 0);
			if (((left_mouse_down) != 0) && ((left_mouse_click_in_cursor) != 0))
			{
				float ratio = (float) (0);
				float d = (float) (_in_.mouse.pos.X - (visual_cursor->x + visual_cursor->w*0.5f));
				float pxstep = (float) (bounds.Width/slider_steps);
				state = (uint) (NK_WIDGET_STATE_ACTIVE);
				if ((((d) < (0)) ? -(d) : (d)) >= (pxstep))
				{
					float steps = (float) ((int) ((((d) < (0)) ? -(d) : (d))/pxstep));
					slider_value += (float) (((d) > (0)) ? (slider_step*steps) : -(slider_step*steps));
					slider_value =
						(float)
							(((slider_value) < (slider_max) ? (slider_value) : (slider_max)) < (slider_min)
								? (slider_min)
								: ((slider_value) < (slider_max) ? (slider_value) : (slider_max)));
					ratio = (float) ((slider_value - slider_min)/slider_step);
					logical_cursor->x = (float) (bounds.X + (logical_cursor->w*ratio));
					_in_.mouse.buttons[NK_BUTTON_LEFT].clicked_pos.X = (float) (logical_cursor->x);
				}
			}

			if ((nk_input_is_mouse_hovering_rect(_in_, (RectangleF) (bounds))) != 0) state = (uint) (NK_WIDGET_STATE_HOVERED);
			if (((state & NK_WIDGET_STATE_HOVER) != 0) && (nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (bounds)) == 0))
				state |= (uint) (NK_WIDGET_STATE_ENTERED);
			else if ((nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (bounds))) != 0) state |= (uint) (NK_WIDGET_STATE_LEFT);
			return (float) (slider_value);
		}

		public static float nk_do_slider(ref uint state, nk_command_buffer _out_, RectangleF bounds, float min, float val,
			float max, float step, nk_style_slider style, nk_input _in_, nk_user_font font)
		{
			float slider_range;
			float slider_min;
			float slider_max;
			float slider_value;
			float slider_steps;
			float cursor_offset;
			RectangleF visual_cursor = new RectangleF();
			RectangleF logical_cursor = new RectangleF();
			if ((_out_ == null) || (style == null)) return (float) (0);
			bounds.X = (float) (bounds.X + style.padding.X);
			bounds.Y = (float) (bounds.Y + style.padding.Y);
			bounds.Height = (float) ((bounds.Height) < (2*style.padding.Y) ? (2*style.padding.Y) : (bounds.Height));
			bounds.Width =
				(float)
					((bounds.Width) < (2*style.padding.X + style.cursor_size.X) ? (2*style.padding.X + style.cursor_size.X) : (bounds.Width));
			bounds.Width -= (float) (2*style.padding.X);
			bounds.Height -= (float) (2*style.padding.Y);
			if ((style.show_buttons) != 0)
			{
				uint ws = 0;
				RectangleF button = new RectangleF();
				button.Y = (float) (bounds.Y);
				button.Width = (float) (bounds.Height);
				button.Height = (float) (bounds.Height);
				button.X = (float) (bounds.X);
				if (
					(nk_do_button_symbol(ref ws, _out_, (RectangleF) (button), (int) (style.dec_symbol), (int) (NK_BUTTON_DEFAULT),
						style.dec_button, _in_, font)) != 0) val -= (float) (step);
				button.X = (float) ((bounds.X + bounds.Width) - button.Width);
				if (
					(nk_do_button_symbol(ref ws, _out_, (RectangleF) (button), (int) (style.inc_symbol), (int) (NK_BUTTON_DEFAULT),
						style.inc_button, _in_, font)) != 0) val += (float) (step);
				bounds.X = (float) (bounds.X + button.Width + style.spacing.X);
				bounds.Width = (float) (bounds.Width - (2*button.Width + 2*style.spacing.X));
			}

			bounds.X += (float) (style.cursor_size.X*0.5f);
			bounds.Width -= (float) (style.cursor_size.X);
			slider_max = (float) ((min) < (max) ? (max) : (min));
			slider_min = (float) ((min) < (max) ? (min) : (max));
			slider_value =
				(float)
					(((val) < (slider_max) ? (val) : (slider_max)) < (slider_min)
						? (slider_min)
						: ((val) < (slider_max) ? (val) : (slider_max)));
			slider_range = (float) (slider_max - slider_min);
			slider_steps = (float) (slider_range/step);
			cursor_offset = (float) ((slider_value - slider_min)/step);
			logical_cursor.Height = (float) (bounds.Height);
			logical_cursor.Width = (float) (bounds.Width/slider_steps);
			logical_cursor.X = (float) (bounds.X + (logical_cursor.Width*cursor_offset));
			logical_cursor.Y = (float) (bounds.Y);
			visual_cursor.Height = (float) (style.cursor_size.Y);
			visual_cursor.Width = (float) (style.cursor_size.X);
			visual_cursor.Y = (float) ((bounds.Y + bounds.Height*0.5f) - visual_cursor.Height*0.5f);
			visual_cursor.X = (float) (logical_cursor.X - visual_cursor.Width*0.5f);
			slider_value =
				(float)
					(nk_slider_behavior(ref state, &logical_cursor, &visual_cursor, _in_, (RectangleF) (bounds), (float) (slider_min),
						(float) (slider_max), (float) (slider_value), (float) (step), (float) (slider_steps)));
			visual_cursor.X = (float) (logical_cursor.X - visual_cursor.Width*0.5f);
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_slider(_out_, (uint) (state), style, &bounds, &visual_cursor, (float) (slider_min), (float) (slider_value),
				(float) (slider_max));
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return (float) (slider_value);
		}

		public static ulong nk_progress_behavior(ref uint state, nk_input _in_, RectangleF r, RectangleF cursor, ulong max,
			ulong value, int modifiable)
		{
			int left_mouse_down = (int) (0);
			int left_mouse_click_in_cursor = (int) (0);
			if (((state) & NK_WIDGET_STATE_MODIFIED) != 0)
				(state) = (uint) (NK_WIDGET_STATE_INACTIVE | NK_WIDGET_STATE_MODIFIED);
			else (state) = (uint) (NK_WIDGET_STATE_INACTIVE);
			if ((_in_ == null) || (modifiable == 0)) return (ulong) (value);
			left_mouse_down =
				(int) (((_in_) != null) && ((_in_.mouse.buttons[NK_BUTTON_LEFT].down) != 0) ? 1 : 0);
			left_mouse_click_in_cursor =
				(int)
					(((_in_) != null) &&
					 ((nk_input_has_mouse_click_down_in_rect(_in_, (int) (NK_BUTTON_LEFT), (RectangleF) (cursor), (int) (nk_true))) != 0)
						? 1
						: 0);
			if ((nk_input_is_mouse_hovering_rect(_in_, (RectangleF) (r))) != 0) state = (uint) (NK_WIDGET_STATE_HOVERED);
			if ((((_in_) != null) && ((left_mouse_down) != 0)) && ((left_mouse_click_in_cursor) != 0))
			{
				if (((left_mouse_down) != 0) && ((left_mouse_click_in_cursor) != 0))
				{
					float ratio = (float) (((0) < (_in_.mouse.pos.X - cursor.X) ? (_in_.mouse.pos.X - cursor.X) : (0))/cursor.Width);
					value =
						((ulong)
							((((float) (max)*ratio) < ((float) (max)) ? ((float) (max)*ratio) : ((float) (max))) < (0)
								? (0)
								: (((float) (max)*ratio) < ((float) (max)) ? ((float) (max)*ratio) : ((float) (max)))));
					_in_.mouse.buttons[NK_BUTTON_LEFT].clicked_pos.X = (float) (cursor.X + cursor.Width/2.0f);
					state |= (uint) (NK_WIDGET_STATE_ACTIVE);
				}
			}

			if (((state & NK_WIDGET_STATE_HOVER) != 0) && (nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (r)) == 0))
				state |= (uint) (NK_WIDGET_STATE_ENTERED);
			else if ((nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (r))) != 0) state |= (uint) (NK_WIDGET_STATE_LEFT);
			return (ulong) (value);
		}

		public static ulong nk_do_progress(ref uint state, nk_command_buffer _out_, RectangleF bounds, ulong value, ulong max,
			int modifiable, nk_style_progress style, nk_input _in_)
		{
			float prog_scale;
			ulong prog_value;
			RectangleF cursor = new RectangleF();
			if ((_out_ == null) || (style == null)) return (ulong) (0);
			cursor.Width =
				(float) ((bounds.Width) < (2*style.padding.X + 2*style.border) ? (2*style.padding.X + 2*style.border) : (bounds.Width));
			cursor.Height =
				(float) ((bounds.Height) < (2*style.padding.Y + 2*style.border) ? (2*style.padding.Y + 2*style.border) : (bounds.Height));
			cursor =
				(RectangleF)
					(nk_pad_rect((RectangleF) (bounds),
						(Vector2) (new Vector2((float) (style.padding.X + style.border), (float) (style.padding.Y + style.border)))));
			prog_scale = (float) ((float) (value)/(float) (max));
			prog_value = (ulong) ((value) < (max) ? (value) : (max));
			prog_value =
				(ulong)
					(nk_progress_behavior(ref state, _in_, (RectangleF) (bounds), (RectangleF) (cursor), (ulong) (max), (ulong) (prog_value),
						(int) (modifiable)));
			cursor.Width = (float) (cursor.Width*prog_scale);
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_progress(_out_, (uint) (state), style, &bounds, &cursor, (ulong) (value), (ulong) (max));
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return (ulong) (prog_value);
		}

		public static float nk_scrollbar_behavior(ref uint state, nk_input _in_, int has_scrolling, RectangleF* scroll,
			ref RectangleF cursor, RectangleF* empty0, RectangleF* empty1, float scroll_offset, float target, float scroll_step, int o)
		{
			uint ws = (uint) (0);
			int left_mouse_down;
			int left_mouse_click_in_cursor;
			float scroll_delta;
			if (((state) & NK_WIDGET_STATE_MODIFIED) != 0)
				(state) = (uint) (NK_WIDGET_STATE_INACTIVE | NK_WIDGET_STATE_MODIFIED);
			else (state) = (uint) (NK_WIDGET_STATE_INACTIVE);
			if (_in_ == null) return (float) (scroll_offset);
			left_mouse_down = (int) (_in_.mouse.buttons[NK_BUTTON_LEFT].down);
			left_mouse_click_in_cursor =
				(int) (nk_input_has_mouse_click_down_in_rect(_in_, (int) (NK_BUTTON_LEFT), (RectangleF) (cursor), (int) (nk_true)));
			if ((nk_input_is_mouse_hovering_rect(_in_, (RectangleF) (*scroll))) != 0) state = (uint) (NK_WIDGET_STATE_HOVERED);
			scroll_delta = (float) (((o) == (NK_VERTICAL)) ? _in_.mouse.scroll_delta.Y : _in_.mouse.scroll_delta.X);
			if (((left_mouse_down) != 0) && ((left_mouse_click_in_cursor) != 0))
			{
				float pixel;
				float delta;
				state = (uint) (NK_WIDGET_STATE_ACTIVE);
				if ((o) == (NK_VERTICAL))
				{
					float cursor_y;
					pixel = (float) (_in_.mouse.delta.Y);
					delta = (float) ((pixel/scroll->h)*target);
					scroll_offset =
						(float)
							(((scroll_offset + delta) < (target - scroll->h) ? (scroll_offset + delta) : (target - scroll->h)) < (0)
								? (0)
								: ((scroll_offset + delta) < (target - scroll->h) ? (scroll_offset + delta) : (target - scroll->h)));
					cursor_y = (float) (scroll->y + ((scroll_offset/target)*scroll->h));
					_in_.mouse.buttons[NK_BUTTON_LEFT].clicked_pos.Y = (float) (cursor_y + cursor.Height/2.0f);
				}
				else
				{
					float cursor_x;
					pixel = (float) (_in_.mouse.delta.X);
					delta = (float) ((pixel/scroll->w)*target);
					scroll_offset =
						(float)
							(((scroll_offset + delta) < (target - scroll->w) ? (scroll_offset + delta) : (target - scroll->w)) < (0)
								? (0)
								: ((scroll_offset + delta) < (target - scroll->w) ? (scroll_offset + delta) : (target - scroll->w)));
					cursor_x = (float) (scroll->x + ((scroll_offset/target)*scroll->w));
					_in_.mouse.buttons[NK_BUTTON_LEFT].clicked_pos.X = (float) (cursor_x + cursor.Width/2.0f);
				}
			}
			else if (((((nk_input_is_key_pressed(_in_, (int) (NK_KEY_SCROLL_UP))) != 0) && ((o) == (NK_VERTICAL))) &&
			          ((has_scrolling) != 0)) ||
			         ((nk_button_behavior(ref ws, (RectangleF) (*empty0), _in_, (int) (NK_BUTTON_DEFAULT))) != 0))
			{
				if ((o) == (NK_VERTICAL))
					scroll_offset = (float) ((0) < (scroll_offset - scroll->h) ? (scroll_offset - scroll->h) : (0));
				else scroll_offset = (float) ((0) < (scroll_offset - scroll->w) ? (scroll_offset - scroll->w) : (0));
			}
			else if (((((nk_input_is_key_pressed(_in_, (int) (NK_KEY_SCROLL_DOWN))) != 0) && ((o) == (NK_VERTICAL))) &&
			          ((has_scrolling) != 0)) ||
			         ((nk_button_behavior(ref ws, (RectangleF) (*empty1), _in_, (int) (NK_BUTTON_DEFAULT))) != 0))
			{
				if ((o) == (NK_VERTICAL))
					scroll_offset =
						(float)
							((scroll_offset + scroll->h) < (target - scroll->h) ? (scroll_offset + scroll->h) : (target - scroll->h));
				else
					scroll_offset =
						(float)
							((scroll_offset + scroll->w) < (target - scroll->w) ? (scroll_offset + scroll->w) : (target - scroll->w));
			}
			else if ((has_scrolling) != 0)
			{
				if ((((scroll_delta) < (0)) || ((scroll_delta) > (0))))
				{
					scroll_offset = (float) (scroll_offset + scroll_step*(-scroll_delta));
					if ((o) == (NK_VERTICAL))
						scroll_offset =
							(float)
								(((scroll_offset) < (target - scroll->h) ? (scroll_offset) : (target - scroll->h)) < (0)
									? (0)
									: ((scroll_offset) < (target - scroll->h) ? (scroll_offset) : (target - scroll->h)));
					else
						scroll_offset =
							(float)
								(((scroll_offset) < (target - scroll->w) ? (scroll_offset) : (target - scroll->w)) < (0)
									? (0)
									: ((scroll_offset) < (target - scroll->w) ? (scroll_offset) : (target - scroll->w)));
				}
				else if ((nk_input_is_key_pressed(_in_, (int) (NK_KEY_SCROLL_START))) != 0)
				{
					if ((o) == (NK_VERTICAL)) scroll_offset = (float) (0);
				}
				else if ((nk_input_is_key_pressed(_in_, (int) (NK_KEY_SCROLL_END))) != 0)
				{
					if ((o) == (NK_VERTICAL)) scroll_offset = (float) (target - scroll->h);
				}
			}

			if (((state & NK_WIDGET_STATE_HOVER) != 0) && (nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (*scroll)) == 0))
				state |= (uint) (NK_WIDGET_STATE_ENTERED);
			else if ((nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (*scroll))) != 0) state |= (uint) (NK_WIDGET_STATE_LEFT);
			return (float) (scroll_offset);
		}

		public static float nk_do_scrollbarv(ref uint state, nk_command_buffer _out_, RectangleF scroll, int has_scrolling,
			float offset, float target, float step, float button_pixel_inc, nk_style_scrollbar style, nk_input _in_,
			nk_user_font font)
		{
			RectangleF empty_north = new RectangleF();
			RectangleF empty_south = new RectangleF();
			RectangleF cursor = new RectangleF();
			float scroll_step;
			float scroll_offset;
			float scroll_off;
			float scroll_ratio;
			if ((_out_ == null) || (style == null)) return (float) (0);
			scroll.Width = (float) ((scroll.Width) < (1) ? (1) : (scroll.Width));
			scroll.Height = (float) ((scroll.Height) < (0) ? (0) : (scroll.Height));
			if (target <= scroll.Height) return (float) (0);
			if ((style.show_buttons) != 0)
			{
				uint ws = 0;
				float scroll_h;
				RectangleF button = new RectangleF();
				button.X = (float) (scroll.X);
				button.Width = (float) (scroll.Width);
				button.Height = (float) (scroll.Width);
				scroll_h = (float) ((scroll.Height - 2*button.Height) < (0) ? (0) : (scroll.Height - 2*button.Height));
				scroll_step = (float) ((step) < (button_pixel_inc) ? (step) : (button_pixel_inc));
				button.Y = (float) (scroll.Y);
				if (
					(nk_do_button_symbol(ref ws, _out_, (RectangleF) (button), (int) (style.dec_symbol), (int) (NK_BUTTON_REPEATER),
						style.dec_button, _in_, font)) != 0) offset = (float) (offset - scroll_step);
				button.Y = (float) (scroll.Y + scroll.Height - button.Height);
				if (
					(nk_do_button_symbol(ref ws, _out_, (RectangleF) (button), (int) (style.inc_symbol), (int) (NK_BUTTON_REPEATER),
						style.inc_button, _in_, font)) != 0) offset = (float) (offset + scroll_step);
				scroll.Y = (float) (scroll.Y + button.Height);
				scroll.Height = (float) (scroll_h);
			}

			scroll_step = (float) ((step) < (scroll.Height) ? (step) : (scroll.Height));
			scroll_offset =
				(float)
					(((offset) < (target - scroll.Height) ? (offset) : (target - scroll.Height)) < (0)
						? (0)
						: ((offset) < (target - scroll.Height) ? (offset) : (target - scroll.Height)));
			scroll_ratio = (float) (scroll.Height/target);
			scroll_off = (float) (scroll_offset/target);
			cursor.Height =
				(float)
					(((scroll_ratio*scroll.Height) - (2*style.border + 2*style.padding.Y)) < (0)
						? (0)
						: ((scroll_ratio*scroll.Height) - (2*style.border + 2*style.padding.Y)));
			cursor.Y = (float) (scroll.Y + (scroll_off*scroll.Height) + style.border + style.padding.Y);
			cursor.Width = (float) (scroll.Width - (2*style.border + 2*style.padding.X));
			cursor.X = (float) (scroll.X + style.border + style.padding.X);
			empty_north.X = (float) (scroll.X);
			empty_north.Y = (float) (scroll.Y);
			empty_north.Width = (float) (scroll.Width);
			empty_north.Height = (float) ((cursor.Y - scroll.Y) < (0) ? (0) : (cursor.Y - scroll.Y));
			empty_south.X = (float) (scroll.X);
			empty_south.Y = (float) (cursor.Y + cursor.Height);
			empty_south.Width = (float) (scroll.Width);
			empty_south.Height =
				(float)
					(((scroll.Y + scroll.Height) - (cursor.Y + cursor.Height)) < (0) ? (0) : ((scroll.Y + scroll.Height) - (cursor.Y + cursor.Height)));
			scroll_offset =
				(float)
					(nk_scrollbar_behavior(ref state, _in_, (int) (has_scrolling), &scroll, ref cursor, &empty_north, &empty_south,
						(float) (scroll_offset), (float) (target), (float) (scroll_step), (int) (NK_VERTICAL)));
			scroll_off = (float) (scroll_offset/target);
			cursor.Y = (float) (scroll.Y + (scroll_off*scroll.Height) + style.border_cursor + style.padding.Y);
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_scrollbar(_out_, (uint) (state), style, &scroll, &cursor);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return (float) (scroll_offset);
		}

		public static float nk_do_scrollbarh(ref uint state, nk_command_buffer _out_, RectangleF scroll, int has_scrolling,
			float offset, float target, float step, float button_pixel_inc, nk_style_scrollbar style, nk_input _in_,
			nk_user_font font)
		{
			RectangleF cursor = new RectangleF();
			RectangleF empty_west = new RectangleF();
			RectangleF empty_east = new RectangleF();
			float scroll_step;
			float scroll_offset;
			float scroll_off;
			float scroll_ratio;
			if ((_out_ == null) || (style == null)) return (float) (0);
			scroll.Height = (float) ((scroll.Height) < (1) ? (1) : (scroll.Height));
			scroll.Width = (float) ((scroll.Width) < (2*scroll.Height) ? (2*scroll.Height) : (scroll.Width));
			if (target <= scroll.Width) return (float) (0);
			if ((style.show_buttons) != 0)
			{
				uint ws = 0;
				float scroll_w;
				RectangleF button = new RectangleF();
				button.Y = (float) (scroll.Y);
				button.Width = (float) (scroll.Height);
				button.Height = (float) (scroll.Height);
				scroll_w = (float) (scroll.Width - 2*button.Width);
				scroll_step = (float) ((step) < (button_pixel_inc) ? (step) : (button_pixel_inc));
				button.X = (float) (scroll.X);
				if (
					(nk_do_button_symbol(ref ws, _out_, (RectangleF) (button), (int) (style.dec_symbol), (int) (NK_BUTTON_REPEATER),
						style.dec_button, _in_, font)) != 0) offset = (float) (offset - scroll_step);
				button.X = (float) (scroll.X + scroll.Width - button.Width);
				if (
					(nk_do_button_symbol(ref ws, _out_, (RectangleF) (button), (int) (style.inc_symbol), (int) (NK_BUTTON_REPEATER),
						style.inc_button, _in_, font)) != 0) offset = (float) (offset + scroll_step);
				scroll.X = (float) (scroll.X + button.Width);
				scroll.Width = (float) (scroll_w);
			}

			scroll_step = (float) ((step) < (scroll.Width) ? (step) : (scroll.Width));
			scroll_offset =
				(float)
					(((offset) < (target - scroll.Width) ? (offset) : (target - scroll.Width)) < (0)
						? (0)
						: ((offset) < (target - scroll.Width) ? (offset) : (target - scroll.Width)));
			scroll_ratio = (float) (scroll.Width/target);
			scroll_off = (float) (scroll_offset/target);
			cursor.Width = (float) ((scroll_ratio*scroll.Width) - (2*style.border + 2*style.padding.X));
			cursor.X = (float) (scroll.X + (scroll_off*scroll.Width) + style.border + style.padding.X);
			cursor.Height = (float) (scroll.Height - (2*style.border + 2*style.padding.Y));
			cursor.Y = (float) (scroll.Y + style.border + style.padding.Y);
			empty_west.X = (float) (scroll.X);
			empty_west.Y = (float) (scroll.Y);
			empty_west.Width = (float) (cursor.X - scroll.X);
			empty_west.Height = (float) (scroll.Height);
			empty_east.X = (float) (cursor.X + cursor.Width);
			empty_east.Y = (float) (scroll.Y);
			empty_east.Width = (float) ((scroll.X + scroll.Width) - (cursor.X + cursor.Width));
			empty_east.Height = (float) (scroll.Height);
			scroll_offset =
				(float)
					(nk_scrollbar_behavior(ref state, _in_, (int) (has_scrolling), &scroll, ref cursor, &empty_west, &empty_east,
						(float) (scroll_offset), (float) (target), (float) (scroll_step), (int) (NK_HORIZONTAL)));
			scroll_off = (float) (scroll_offset/target);
			cursor.X = (float) (scroll.X + (scroll_off*scroll.Width));
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_scrollbar(_out_, (uint) (state), style, &scroll, &cursor);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			return (float) (scroll_offset);
		}

		public static uint nk_do_edit(ref uint state, nk_command_buffer _out_, RectangleF bounds, uint flags,
			NkPluginFilter filter, nk_text_edit edit, nk_style_edit style, nk_input _in_, nk_user_font font)
		{
			RectangleF area = new RectangleF();
			uint ret = (uint) (0);
			float row_height;
			sbyte prev_state = (sbyte) (0);
			sbyte is_hovered = (sbyte) (0);
			sbyte select_all = (sbyte) (0);
			sbyte cursor_follow = (sbyte) (0);
			RectangleF old_clip = new RectangleF();
			RectangleF clip = new RectangleF();
			if (((_out_ == null)) || (style == null)) return (uint) (ret);
			area.X = (float) (bounds.X + style.padding.X + style.border);
			area.Y = (float) (bounds.Y + style.padding.Y + style.border);
			area.Width = (float) (bounds.Width - (2.0f*style.padding.X + 2*style.border));
			area.Height = (float) (bounds.Height - (2.0f*style.padding.Y + 2*style.border));
			if ((flags & NK_EDIT_MULTILINE) != 0)
				area.Width = (float) ((0) < (area.Width - style.scrollbar_size.X) ? (area.Width - style.scrollbar_size.X) : (0));
			row_height = (float) ((flags & NK_EDIT_MULTILINE) != 0 ? font.Height + style.row_padding : area.Height);
			old_clip = (RectangleF) (_out_.clip);
			nk_unify(ref clip, ref old_clip, (float) (area.X), (float) (area.Y), (float) (area.X + area.Width),
				(float) (area.Y + area.Height));
			prev_state = ((sbyte) (edit.active));
			is_hovered = ((sbyte) (nk_input_is_mouse_hovering_rect(_in_, (RectangleF) (bounds))));
			if ((((_in_) != null) && (_in_.mouse.buttons[NK_BUTTON_LEFT].clicked != 0)) &&
			    ((_in_.mouse.buttons[NK_BUTTON_LEFT].down) != 0))
			{
				edit.active =
					(byte)
						((((bounds.X) <= (_in_.mouse.pos.X)) && ((_in_.mouse.pos.X) < (bounds.X + bounds.Width))) &&
						 (((bounds.Y) <= (_in_.mouse.pos.Y)) && ((_in_.mouse.pos.Y) < (bounds.Y + bounds.Height)))
							? 1
							: 0);
			}

			if ((prev_state == 0) && ((edit.active) != 0))
			{
				int type = (int) ((flags & NK_EDIT_MULTILINE) != 0 ? NK_TEXT_EDIT_MULTI_LINE : NK_TEXT_EDIT_SINGLE_LINE);
				nk_textedit_clear_state(edit, (int) (type), filter);
				if ((flags & NK_EDIT_AUTO_SELECT) != 0) select_all = (sbyte) (nk_true);
				if ((flags & NK_EDIT_GOTO_END_ON_ACTIVATE) != 0)
				{
					edit.cursor = (int) (edit._string_.len);
					_in_ = null;
				}
			}
			else if (edit.active == 0) edit.mode = (byte) (NK_TEXT_EDIT_MODE_VIEW);
			if ((flags & NK_EDIT_READ_ONLY) != 0) edit.mode = (byte) (NK_TEXT_EDIT_MODE_VIEW);
			else if ((flags & NK_EDIT_ALWAYS_INSERT_MODE) != 0) edit.mode = (byte) (NK_TEXT_EDIT_MODE_INSERT);
			ret = (uint) ((edit.active) != 0 ? NK_EDIT_ACTIVE : NK_EDIT_INACTIVE);
			if (prev_state != edit.active) ret |= (uint) ((edit.active) != 0 ? NK_EDIT_ACTIVATED : NK_EDIT_DEACTIVATED);
			if (((edit.active) != 0) && ((_in_) != null))
			{
				int shift_mod = (int) (_in_.keyboard.keys[NK_KEY_SHIFT].down);
				float mouse_x = (float) ((_in_.mouse.pos.X - area.X) + edit.scrollbar.X);
				float mouse_y = (float) ((_in_.mouse.pos.Y - area.Y) + edit.scrollbar.Y);
				is_hovered = ((sbyte) (nk_input_is_mouse_hovering_rect(_in_, (RectangleF) (area))));
				if ((select_all) != 0)
				{
					nk_textedit_select_all(edit);
				}
				else if ((((is_hovered) != 0) && ((_in_.mouse.buttons[NK_BUTTON_LEFT].down) != 0)) &&
				         ((_in_.mouse.buttons[NK_BUTTON_LEFT].clicked) != 0))
				{
					nk_textedit_click(edit, (float) (mouse_x), (float) (mouse_y), font, (float) (row_height));
				}
				else if ((((is_hovered) != 0) && ((_in_.mouse.buttons[NK_BUTTON_LEFT].down) != 0)) &&
				         ((_in_.mouse.delta.X != 0.0f) || (_in_.mouse.delta.Y != 0.0f)))
				{
					nk_textedit_drag(edit, (float) (mouse_x), (float) (mouse_y), font, (float) (row_height));
					cursor_follow = (sbyte) (nk_true);
				}
				else if ((((is_hovered) != 0) && ((_in_.mouse.buttons[NK_BUTTON_RIGHT].clicked) != 0)) &&
				         ((_in_.mouse.buttons[NK_BUTTON_RIGHT].down) != 0))
				{
					nk_textedit_key(edit, (int) (NK_KEY_TEXT_WORD_LEFT), (int) (nk_false), font, (float) (row_height));
					nk_textedit_key(edit, (int) (NK_KEY_TEXT_WORD_RIGHT), (int) (nk_true), font, (float) (row_height));
					cursor_follow = (sbyte) (nk_true);
				}
				{
					int i;
					int old_mode = (int) (edit.mode);
					for (i = (int) (0); (i) < (NK_KEY_MAX); ++i)
					{
						if (((i) == (NK_KEY_ENTER)) || ((i) == (NK_KEY_TAB))) continue;
						if ((nk_input_is_key_pressed(_in_, (int) (i))) != 0)
						{
							nk_textedit_key(edit, (int) (i), (int) (shift_mod), font, (float) (row_height));
							cursor_follow = (sbyte) (nk_true);
						}
					}
					if (old_mode != edit.mode)
					{
						_in_.keyboard.text_len = (int) (0);
					}
				}
				edit.filter = filter;
				if ((_in_.keyboard.text_len) != 0)
				{
					nk_textedit_text(edit, _in_.keyboard.text, (int) (_in_.keyboard.text_len));
					cursor_follow = (sbyte) (nk_true);
					_in_.keyboard.text_len = (int) (0);
				}
				if ((nk_input_is_key_pressed(_in_, (int) (NK_KEY_ENTER))) != 0)
				{
					cursor_follow = (sbyte) (nk_true);
					if (((flags & NK_EDIT_CTRL_ENTER_NEWLINE) != 0) && ((shift_mod) != 0)) nk_textedit_text(edit, "\n", (int) (1));
					else if ((flags & NK_EDIT_SIG_ENTER) != 0) ret |= (uint) (NK_EDIT_COMMITED);
					else nk_textedit_text(edit, "\n", (int) (1));
				}
				{
					int copy = (int) (nk_input_is_key_pressed(_in_, (int) (NK_KEY_COPY)));
					int cut = (int) (nk_input_is_key_pressed(_in_, (int) (NK_KEY_CUT)));
					if ((((copy) != 0) || ((cut) != 0)) && ((flags & NK_EDIT_CLIPBOARD) != 0))
					{
						char* text;
						int b = (int) (edit.select_start);
						int e = (int) (edit.select_end);
						int begin = (int) ((b) < (e) ? (b) : (e));
						int end = (int) ((b) < (e) ? (e) : (b));
						fixed (char* str2 = edit._string_.str)
						{
							text = str2 + begin;
							if ((edit.clip.copy) != null) edit.clip.copy((nk_handle) (edit.clip.userdata), text, (int) (end - begin));
							if (((cut) != 0) && ((flags & NK_EDIT_READ_ONLY) == 0))
							{
								nk_textedit_cut(edit);
								cursor_follow = (sbyte) (nk_true);
							}
						}
					}
				}
				{
					int paste = (int) (nk_input_is_key_pressed(_in_, (int) (NK_KEY_PASTE)));
					if ((((paste) != 0) && ((flags & NK_EDIT_CLIPBOARD) != 0)) && ((edit.clip.paste) != null))
					{
						edit.clip.paste((nk_handle) (edit.clip.userdata), edit);
						cursor_follow = (sbyte) (nk_true);
					}
				}
				{
					int tab = (int) (nk_input_is_key_pressed(_in_, (int) (NK_KEY_TAB)));
					if (((tab) != 0) && ((flags & NK_EDIT_ALLOW_TAB) != 0))
					{
						nk_textedit_text(edit, "    ", (int) (4));
						cursor_follow = (sbyte) (nk_true);
					}
				}
			}

			if ((edit.active) != 0) state = (uint) (NK_WIDGET_STATE_ACTIVE);
			else if (((state) & NK_WIDGET_STATE_MODIFIED) != 0)
				(state) = (uint) (NK_WIDGET_STATE_INACTIVE | NK_WIDGET_STATE_MODIFIED);
			else (state) = (uint) (NK_WIDGET_STATE_INACTIVE);
			if ((is_hovered) != 0) state |= (uint) (NK_WIDGET_STATE_HOVERED);
			{
				fixed (char* text = edit._string_.str)
				{
					int len = (int) (edit._string_.len);
					{
						nk_style_item background;
						if ((state & NK_WIDGET_STATE_ACTIVED) != 0) background = style.active;
						else if ((state & NK_WIDGET_STATE_HOVER) != 0) background = style.hover;
						else background = style.normal;
						if ((background.type) == (NK_STYLE_ITEM_COLOR))
						{
							nk_stroke_rect(_out_, (RectangleF) (bounds), (float) (style.rounding), (float) (style.border),
								(Color) (style.border_color));
							nk_fill_rect(_out_, (RectangleF) (bounds), (float) (style.rounding), (Color) (background.data.color));
						}
						else nk_draw_image(_out_, (RectangleF) (bounds), background.data.image, (Color) (nk_white));
					}
					area.Width = (float) ((0) < (area.Width - style.cursor_size) ? (area.Width - style.cursor_size) : (0));
					if ((edit.active) != 0)
					{
						int total_lines = (int) (1);
						Vector2 text_size = (Vector2) (new Vector2((float) (0), (float) (0)));
						char* cursor_ptr = null;
						char* select_begin_ptr = null;
						char* select_end_ptr = null;
						Vector2 cursor_pos = (Vector2) (new Vector2((float) (0), (float) (0)));
						Vector2 selection_offset_start = (Vector2) (new Vector2((float) (0), (float) (0)));
						Vector2 selection_offset_end = (Vector2) (new Vector2((float) (0), (float) (0)));
						int selection_begin = (int) ((edit.select_start) < (edit.select_end) ? (edit.select_start) : (edit.select_end));
						int selection_end = (int) ((edit.select_start) < (edit.select_end) ? (edit.select_end) : (edit.select_start));
						float line_width = (float) (0.0f);
						if (((text) != null) && ((len) != 0))
						{
							float glyph_width;
							int glyph_len = (int) (0);
							char unicode = (char) 0;
							int text_len = (int) (0);
							int glyphs = (int) (0);
							int row_begin = (int) (0);
							glyph_len = (int) (nk_utf_decode(text, &unicode, (int) (len)));
							glyph_width = (float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), text, (int) (glyph_len)));
							line_width = (float) (0);
							while (((text_len) < (len)) && ((glyph_len) != 0))
							{
								if ((cursor_ptr == null) && ((glyphs) == (edit.cursor)))
								{
									int glyph_offset;
									Vector2 out_offset = new Vector2();
									Vector2 row_size = new Vector2();
									char* remaining;
									cursor_pos.Y = (float) ((float) (total_lines - 1)*row_height);
									row_size =
										(Vector2)
											(nk_text_calculate_text_bounds(font, text + row_begin, (int) (text_len - row_begin), (float) (row_height),
												&remaining, &out_offset, &glyph_offset, (int) (NK_STOP_ON_NEW_LINE)));
									cursor_pos.X = (float) (row_size.X);
									cursor_ptr = text + text_len;
								}
								if (((select_begin_ptr == null) && (edit.select_start != edit.select_end)) && ((glyphs) == (selection_begin)))
								{
									int glyph_offset;
									Vector2 out_offset = new Vector2();
									Vector2 row_size = new Vector2();
									char* remaining;
									selection_offset_start.Y = (float) ((float) ((total_lines - 1) < (0) ? (0) : (total_lines - 1))*row_height);
									row_size =
										(Vector2)
											(nk_text_calculate_text_bounds(font, text + row_begin, (int) (text_len - row_begin), (float) (row_height),
												&remaining, &out_offset, &glyph_offset, (int) (NK_STOP_ON_NEW_LINE)));
									selection_offset_start.X = (float) (row_size.X);
									select_begin_ptr = text + text_len;
								}
								if (((select_end_ptr == null) && (edit.select_start != edit.select_end)) && ((glyphs) == (selection_end)))
								{
									int glyph_offset;
									Vector2 out_offset = new Vector2();
									Vector2 row_size = new Vector2();
									char* remaining;
									selection_offset_end.Y = (float) ((float) (total_lines - 1)*row_height);
									row_size =
										(Vector2)
											(nk_text_calculate_text_bounds(font, text + row_begin, (int) (text_len - row_begin), (float) (row_height),
												&remaining, &out_offset, &glyph_offset, (int) (NK_STOP_ON_NEW_LINE)));
									selection_offset_end.X = (float) (row_size.X);
									select_end_ptr = text + text_len;
								}
								if ((unicode) == ('\n'))
								{
									text_size.X = (float) ((text_size.X) < (line_width) ? (line_width) : (text_size.X));
									total_lines++;
									line_width = (float) (0);
									text_len++;
									glyphs++;
									row_begin = (int) (text_len);
									glyph_len = (int) (nk_utf_decode(text + text_len, &unicode, (int) (len - text_len)));
									glyph_width =
										(float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), text + text_len, (int) (glyph_len)));
									continue;
								}
								glyphs++;
								text_len += (int) (glyph_len);
								line_width += (float) (glyph_width);
								glyph_len = (int) (nk_utf_decode(text + text_len, &unicode, (int) (len - text_len)));
								glyph_width =
									(float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), text + text_len, (int) (glyph_len)));
								continue;
							}
							text_size.Y = (float) ((float) (total_lines)*row_height);
							if ((cursor_ptr == null) && ((edit.cursor) == (edit._string_.len)))
							{
								cursor_pos.X = (float) (line_width);
								cursor_pos.Y = (float) (text_size.Y - row_height);
							}
						}
						{
							if ((cursor_follow) != 0)
							{
								if ((flags & NK_EDIT_NO_HORIZONTAL_SCROLL) == 0)
								{
									float scroll_increment = (float) (area.Width*0.25f);
									if ((cursor_pos.X) < (edit.scrollbar.X))
										edit.scrollbar.X =
											((float) ((int) ((0.0f) < (cursor_pos.X - scroll_increment) ? (cursor_pos.X - scroll_increment) : (0.0f))));
									if ((cursor_pos.X) >= (edit.scrollbar.X + area.Width))
										edit.scrollbar.X = ((float) ((int) ((0.0f) < (cursor_pos.X) ? (cursor_pos.X) : (0.0f))));
								}
								else edit.scrollbar.X = (float) (0);
								if ((flags & NK_EDIT_MULTILINE) != 0)
								{
									if ((cursor_pos.Y) < (edit.scrollbar.Y))
										edit.scrollbar.Y = (float) ((0.0f) < (cursor_pos.Y - row_height) ? (cursor_pos.Y - row_height) : (0.0f));
									if ((cursor_pos.Y) >= (edit.scrollbar.Y + area.Height)) edit.scrollbar.Y = (float) (edit.scrollbar.Y + row_height);
								}
								else edit.scrollbar.Y = (float) (0);
							}
							if ((flags & NK_EDIT_MULTILINE) != 0)
							{
								uint ws = 0;
								RectangleF scroll = new RectangleF();
								float scroll_target;
								float scroll_offset;
								float scroll_step;
								float scroll_inc;
								scroll = (RectangleF) (area);
								scroll.X = (float) ((bounds.X + bounds.Width - style.border) - style.scrollbar_size.X);
								scroll.Width = (float) (style.scrollbar_size.X);
								scroll_offset = (float) (edit.scrollbar.Y);
								scroll_step = (float) (scroll.Height*0.10f);
								scroll_inc = (float) (scroll.Height*0.01f);
								scroll_target = (float) (text_size.Y);
								edit.scrollbar.Y =
									(float)
										(nk_do_scrollbarv(ref ws, _out_, (RectangleF) (scroll), (int) (0), (float) (scroll_offset),
											(float) (scroll_target), (float) (scroll_step), (float) (scroll_inc), style.scrollbar, _in_, font));
							}
						}
						{
							Color background_color = new Color();
							Color text_color = new Color();
							Color sel_background_color = new Color();
							Color sel_text_color = new Color();
							Color cursor_color = new Color();
							Color cursor_text_color = new Color();
							nk_style_item background;
							nk_push_scissor(_out_, (RectangleF) (clip));
							if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
							{
								background = style.active;
								text_color = (Color) (style.text_active);
								sel_text_color = (Color) (style.selected_text_hover);
								sel_background_color = (Color) (style.selected_hover);
								cursor_color = (Color) (style.cursor_hover);
								cursor_text_color = (Color) (style.cursor_text_hover);
							}
							else if ((state & NK_WIDGET_STATE_HOVER) != 0)
							{
								background = style.hover;
								text_color = (Color) (style.text_hover);
								sel_text_color = (Color) (style.selected_text_hover);
								sel_background_color = (Color) (style.selected_hover);
								cursor_text_color = (Color) (style.cursor_text_hover);
								cursor_color = (Color) (style.cursor_hover);
							}
							else
							{
								background = style.normal;
								text_color = (Color) (style.text_normal);
								sel_text_color = (Color) (style.selected_text_normal);
								sel_background_color = (Color) (style.selected_normal);
								cursor_color = (Color) (style.cursor_normal);
								cursor_text_color = (Color) (style.cursor_text_normal);
							}
							if ((background.type) == (NK_STYLE_ITEM_IMAGE))
								background_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
							else background_color = (Color) (background.data.color);
							if ((edit.select_start) == (edit.select_end))
							{
								fixed (char* begin = edit._string_.str)
								{
									int l = (int) (edit._string_.len);
									nk_edit_draw_text(_out_, style, (float) (area.X - edit.scrollbar.X), (float) (area.Y - edit.scrollbar.Y),
										(float) (0), begin, (int) (l), (float) (row_height), font, (Color) (background_color),
										(Color) (text_color), (int) (nk_false));
								}
							}
							else
							{
								if ((edit.select_start != edit.select_end) && ((selection_begin) > (0)))
								{
									fixed (char* begin = edit._string_.str)
									{
										nk_edit_draw_text(_out_, style, (float) (area.X - edit.scrollbar.X), (float) (area.Y - edit.scrollbar.Y),
											(float) (0), begin, (int) (select_begin_ptr - begin), (float) (row_height), font,
											(Color) (background_color),
											(Color) (text_color), (int) (nk_false));
									}
								}
								if (edit.select_start != edit.select_end)
								{
									if (select_end_ptr == null)
									{
										char* begin = text;
										select_end_ptr = begin + edit._string_.len;
									}
									nk_edit_draw_text(_out_, style, (float) (area.X - edit.scrollbar.X),
										(float) (area.Y + selection_offset_start.Y - edit.scrollbar.Y), (float) (selection_offset_start.X),
										select_begin_ptr, (int) (select_end_ptr - select_begin_ptr), (float) (row_height), font,
										(Color) (sel_background_color), (Color) (sel_text_color), (int) (nk_true));
								}
								if (((edit.select_start != edit.select_end) && ((selection_end) < (edit._string_.len))))
								{
									char* begin = select_end_ptr;
									char* end = text + edit._string_.len;
									nk_edit_draw_text(_out_, style, (float) (area.X - edit.scrollbar.X),
										(float) (area.Y + selection_offset_end.Y - edit.scrollbar.Y), (float) (selection_offset_end.X), begin,
										(int) (end - begin), (float) (row_height), font, (Color) (background_color), (Color) (text_color),
										(int) (nk_true));
								}
							}
							if ((edit.select_start) == (edit.select_end))
							{
								if (((edit.cursor) >= (edit._string_.len)) || (((cursor_ptr) != null) && ((*cursor_ptr) == ('\n'))))
								{
									RectangleF cursor = new RectangleF();
									cursor.Width = (float) (style.cursor_size);
									cursor.Height = (float) (font.Height);
									cursor.X = (float) (area.X + cursor_pos.X - edit.scrollbar.X);
									cursor.Y = (float) (area.Y + cursor_pos.Y + row_height/2.0f - cursor.Height/2.0f);
									cursor.Y -= (float) (edit.scrollbar.Y);
									nk_fill_rect(_out_, (RectangleF) (cursor), (float) (0), (Color) (cursor_color));
								}
								else
								{
									int glyph_len;
									RectangleF label = new RectangleF();
									nk_text txt = new nk_text();
									char unicode;
									glyph_len = (int) (nk_utf_decode(cursor_ptr, &unicode, (int) (4)));
									label.X = (float) (area.X + cursor_pos.X - edit.scrollbar.X);
									label.Y = (float) (area.Y + cursor_pos.Y - edit.scrollbar.Y);
									label.Width =
										(float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), cursor_ptr, (int) (glyph_len)));
									label.Height = (float) (row_height);
									txt.padding = (Vector2) (new Vector2((float) (0), (float) (0)));
									txt.background = (Color) (cursor_color);
									txt.text = (Color) (cursor_text_color);
									nk_fill_rect(_out_, (RectangleF) (label), (float) (0), (Color) (cursor_color));
									nk_widget_text(_out_, (RectangleF) (label), cursor_ptr, (int) (glyph_len), &txt, (uint) (NK_TEXT_LEFT), font);
								}
							}
						}
					}
					else
					{
						int l = (int) (edit._string_.len);
						fixed (char* begin = edit._string_.str)
						{
							nk_style_item background;
							Color background_color = new Color();
							Color text_color = new Color();
							nk_push_scissor(_out_, (RectangleF) (clip));
							if ((state & NK_WIDGET_STATE_ACTIVED) != 0)
							{
								background = style.active;
								text_color = (Color) (style.text_active);
							}
							else if ((state & NK_WIDGET_STATE_HOVER) != 0)
							{
								background = style.hover;
								text_color = (Color) (style.text_hover);
							}
							else
							{
								background = style.normal;
								text_color = (Color) (style.text_normal);
							}
							if ((background.type) == (NK_STYLE_ITEM_IMAGE))
								background_color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
							else background_color = (Color) (background.data.color);
							nk_edit_draw_text(_out_, style, (float) (area.X - edit.scrollbar.X), (float) (area.Y - edit.scrollbar.Y),
								(float) (0), begin, (int) (l), (float) (row_height), font, (Color) (background_color),
								(Color) (text_color),
								(int) (nk_false));
						}
					}
					nk_push_scissor(_out_, (RectangleF) (old_clip));
				}
			}

			return (uint) (ret);
		}

		public static void nk_drag_behavior(ref uint state, nk_input _in_, RectangleF drag, nk_property_variant* variant,
			float inc_per_pixel)
		{
			int left_mouse_down =
				(int) (((_in_) != null) && ((_in_.mouse.buttons[NK_BUTTON_LEFT].down) != 0) ? 1 : 0);
			int left_mouse_click_in_cursor =
				(int)
					(((_in_) != null) &&
					 ((nk_input_has_mouse_click_down_in_rect(_in_, (int) (NK_BUTTON_LEFT), (RectangleF) (drag), (int) (nk_true))) != 0)
						? 1
						: 0);
			if (((state) & NK_WIDGET_STATE_MODIFIED) != 0)
				(state) = (uint) (NK_WIDGET_STATE_INACTIVE | NK_WIDGET_STATE_MODIFIED);
			else (state) = (uint) (NK_WIDGET_STATE_INACTIVE);
			if ((nk_input_is_mouse_hovering_rect(_in_, (RectangleF) (drag))) != 0) state = (uint) (NK_WIDGET_STATE_HOVERED);
			if (((left_mouse_down) != 0) && ((left_mouse_click_in_cursor) != 0))
			{
				float delta;
				float pixels;
				pixels = (float) (_in_.mouse.delta.X);
				delta = (float) (pixels*inc_per_pixel);
				switch (variant->kind)
				{
					default:
						break;
					case NK_PROPERTY_INT:
						variant->value.i = (int) (variant->value.i + (int) (delta));
						variant->value.i =
							(int)
								(((variant->value.i) < (variant->max_value.i) ? (variant->value.i) : (variant->max_value.i)) <
								 (variant->min_value.i)
									? (variant->min_value.i)
									: ((variant->value.i) < (variant->max_value.i) ? (variant->value.i) : (variant->max_value.i)));
						break;
					case NK_PROPERTY_FLOAT:
						variant->value.f = (float) (variant->value.f + delta);
						variant->value.f =
							(float)
								(((variant->value.f) < (variant->max_value.f) ? (variant->value.f) : (variant->max_value.f)) <
								 (variant->min_value.f)
									? (variant->min_value.f)
									: ((variant->value.f) < (variant->max_value.f) ? (variant->value.f) : (variant->max_value.f)));
						break;
					case NK_PROPERTY_DOUBLE:
						variant->value.d = (double) (variant->value.d + (double) (delta));
						variant->value.d =
							(double)
								(((variant->value.d) < (variant->max_value.d) ? (variant->value.d) : (variant->max_value.d)) <
								 (variant->min_value.d)
									? (variant->min_value.d)
									: ((variant->value.d) < (variant->max_value.d) ? (variant->value.d) : (variant->max_value.d)));
						break;
				}
				state = (uint) (NK_WIDGET_STATE_ACTIVE);
			}

			if (((state & NK_WIDGET_STATE_HOVER) != 0) && (nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (drag)) == 0))
				state |= (uint) (NK_WIDGET_STATE_ENTERED);
			else if ((nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (drag))) != 0) state |= (uint) (NK_WIDGET_STATE_LEFT);
		}

		public static void nk_property_behavior(ref uint ws, nk_input _in_, RectangleF property, RectangleF label, RectangleF edit,
			RectangleF empty, ref int state, nk_property_variant* variant, float inc_per_pixel)
		{
			if (((_in_) != null) && ((state) == (NK_PROPERTY_DEFAULT)))
			{
				if ((nk_button_behavior(ref ws, (RectangleF) (edit), _in_, (int) (NK_BUTTON_DEFAULT))) != 0)
					state = (int) (NK_PROPERTY_EDIT);
				else if ((nk_input_is_mouse_click_down_in_rect(_in_, (int) (NK_BUTTON_LEFT), (RectangleF) (label), (int) (nk_true))) != 0)
					state = (int) (NK_PROPERTY_DRAG);
				else if ((nk_input_is_mouse_click_down_in_rect(_in_, (int) (NK_BUTTON_LEFT), (RectangleF) (empty), (int) (nk_true))) != 0)
					state = (int) (NK_PROPERTY_DRAG);
			}

			if ((state) == (NK_PROPERTY_DRAG))
			{
				nk_drag_behavior(ref ws, _in_, (RectangleF) (property), variant, (float) (inc_per_pixel));
				if ((ws & NK_WIDGET_STATE_ACTIVED) == 0) state = (int) (NK_PROPERTY_DEFAULT);
			}

		}

		public static void nk_do_property(ref uint ws, nk_command_buffer _out_, RectangleF property, char* name,
			nk_property_variant* variant, float inc_per_pixel, ref string buffer, ref int state, ref int cursor,
			ref int select_begin, ref int select_end, nk_style_property style, int filter, nk_input _in_, nk_user_font font,
			nk_text_edit text_edit, int behavior)
		{
			NkPluginFilter[] filters = new NkPluginFilter[2];
			filters[0] = nk_filter_decimal;
			filters[1] = nk_filter_float;

			int active;
			int old;
			int name_len;
			float size;
			string dst = null;
			RectangleF left = new RectangleF();
			RectangleF right = new RectangleF();
			RectangleF label = new RectangleF();
			RectangleF edit = new RectangleF();
			RectangleF empty = new RectangleF();
			left.Height = (float) (font.Height/2);
			left.Width = (float) (left.Height);
			left.X = (float) (property.X + style.border + style.padding.X);
			left.Y = (float) (property.Y + style.border + property.Height/2.0f - left.Height/2);
			name_len = (int) (nk_strlen(name));
			size = (float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), name, (int) (name_len)));
			label.X = (float) (left.X + left.Width + style.padding.X);
			label.Width = (float) (size + 2*style.padding.X);
			label.Y = (float) (property.Y + style.border + style.padding.Y);
			label.Height = (float) (property.Height - (2*style.border + 2*style.padding.Y));
			right.Y = (float) (left.Y);
			right.Width = (float) (left.Width);
			right.Height = (float) (left.Height);
			right.X = (float) (property.X + property.Width - (right.Width + style.padding.X));
			if ((state) == (NK_PROPERTY_EDIT))
			{
				fixed (char* ptr = buffer)
				{
					size = (float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), ptr, buffer.Length));
				}
				size += (float) (style.edit.cursor_size);
				dst = buffer;
			}
			else
			{
				int num_len = 0;
				char* _string_ = stackalloc char[64];
				switch (variant->kind)
				{
					default:
						break;
					case NK_PROPERTY_INT:
						nk_itoa(_string_, (int) (variant->value.i));
						num_len = (int) (nk_strlen(_string_));
						break;
					case NK_PROPERTY_FLOAT:
						nk_dtoa(_string_, (double) (variant->value.f));
						num_len = (int) (nk_string_float_limit(_string_, (int) (2)));
						break;
					case NK_PROPERTY_DOUBLE:
						nk_dtoa(_string_, (double) (variant->value.d));
						num_len = (int) (nk_string_float_limit(_string_, (int) (2)));
						break;
				}
				size = (float) (font.Widthidth((nk_handle) (font.userdata), (float) (font.Height), _string_, (int) (num_len)));
				dst = new string(_string_);

				if (dst.Length > num_len)
				{
					dst = dst.Substring(0, num_len);
				}
			}

			edit.Width = (float) (size + 2*style.padding.X);
			edit.Width = (float) ((edit.Width) < (right.X - (label.X + label.Width)) ? (edit.Width) : (right.X - (label.X + label.Width)));
			edit.X = (float) (right.X - (edit.Width + style.padding.X));
			edit.Y = (float) (property.Y + style.border);
			edit.Height = (float) (property.Height - (2*style.border));
			empty.Width = (float) (edit.X - (label.X + label.Width));
			empty.X = (float) (label.X + label.Width);
			empty.Y = (float) (property.Y);
			empty.Height = (float) (property.Height);
			old = (int) ((state) == (NK_PROPERTY_EDIT) ? 1 : 0);
			nk_property_behavior(ref ws, _in_, (RectangleF) (property), (RectangleF) (label), (RectangleF) (edit), (RectangleF) (empty),
				ref state, variant, (float) (inc_per_pixel));
			if ((style.draw_begin) != null) style.draw_begin(_out_, (nk_handle) (style.userdata));
			nk_draw_property(_out_, style, &property, &label, (uint) (ws), name, (int) (name_len), font);
			if ((style.draw_end) != null) style.draw_end(_out_, (nk_handle) (style.userdata));
			if (
				(nk_do_button_symbol(ref ws, _out_, (RectangleF) (left), (int) (style.sym_left), (int) (behavior), style.dec_button,
					_in_, font)) != 0)
			{
				switch (variant->kind)
				{
					default:
						break;
					case NK_PROPERTY_INT:
						variant->value.i =
							(int)
								(((variant->value.i - variant->step.i) < (variant->max_value.i)
									? (variant->value.i - variant->step.i)
									: (variant->max_value.i)) < (variant->min_value.i)
									? (variant->min_value.i)
									: ((variant->value.i - variant->step.i) < (variant->max_value.i)
										? (variant->value.i - variant->step.i)
										: (variant->max_value.i)));
						break;
					case NK_PROPERTY_FLOAT:
						variant->value.f =
							(float)
								(((variant->value.f - variant->step.f) < (variant->max_value.f)
									? (variant->value.f - variant->step.f)
									: (variant->max_value.f)) < (variant->min_value.f)
									? (variant->min_value.f)
									: ((variant->value.f - variant->step.f) < (variant->max_value.f)
										? (variant->value.f - variant->step.f)
										: (variant->max_value.f)));
						break;
					case NK_PROPERTY_DOUBLE:
						variant->value.d =
							(double)
								(((variant->value.d - variant->step.d) < (variant->max_value.d)
									? (variant->value.d - variant->step.d)
									: (variant->max_value.d)) < (variant->min_value.d)
									? (variant->min_value.d)
									: ((variant->value.d - variant->step.d) < (variant->max_value.d)
										? (variant->value.d - variant->step.d)
										: (variant->max_value.d)));
						break;
				}
			}

			if (
				(nk_do_button_symbol(ref ws, _out_, (RectangleF) (right), (int) (style.sym_right), (int) (behavior), style.inc_button,
					_in_, font)) != 0)
			{
				switch (variant->kind)
				{
					default:
						break;
					case NK_PROPERTY_INT:
						variant->value.i =
							(int)
								(((variant->value.i + variant->step.i) < (variant->max_value.i)
									? (variant->value.i + variant->step.i)
									: (variant->max_value.i)) < (variant->min_value.i)
									? (variant->min_value.i)
									: ((variant->value.i + variant->step.i) < (variant->max_value.i)
										? (variant->value.i + variant->step.i)
										: (variant->max_value.i)));
						break;
					case NK_PROPERTY_FLOAT:
						variant->value.f =
							(float)
								(((variant->value.f + variant->step.f) < (variant->max_value.f)
									? (variant->value.f + variant->step.f)
									: (variant->max_value.f)) < (variant->min_value.f)
									? (variant->min_value.f)
									: ((variant->value.f + variant->step.f) < (variant->max_value.f)
										? (variant->value.f + variant->step.f)
										: (variant->max_value.f)));
						break;
					case NK_PROPERTY_DOUBLE:
						variant->value.d =
							(double)
								(((variant->value.d + variant->step.d) < (variant->max_value.d)
									? (variant->value.d + variant->step.d)
									: (variant->max_value.d)) < (variant->min_value.d)
									? (variant->min_value.d)
									: ((variant->value.d + variant->step.d) < (variant->max_value.d)
										? (variant->value.d + variant->step.d)
										: (variant->max_value.d)));
						break;
				}
			}

			if ((old != NK_PROPERTY_EDIT) && ((state) == (NK_PROPERTY_EDIT)))
			{
				buffer = dst;
				cursor = buffer != null?buffer.Length:0;
				active = (int) (0);
			}
			else active = (int) ((state) == (NK_PROPERTY_EDIT) ? 1 : 0);
			nk_textedit_clear_state(text_edit, (int) (NK_TEXT_EDIT_SINGLE_LINE), filters[filter]);
			text_edit.active = ((byte) (active));

			text_edit._string_.str = dst;

			int length = dst != null ? dst.Length : 0;
			text_edit.cursor =
				(int) (((cursor) < (length) ? (cursor) : (length)) < (0) ? (0) : ((cursor) < (length) ? (cursor) : (length)));
			text_edit.select_start =
				(int)
					(((select_begin) < (length) ? (select_begin) : (length)) < (0)
						? (0)
						: ((select_begin) < (length) ? (select_begin) : (length)));
			text_edit.select_end =
				(int)
					(((select_end) < (length) ? (select_end) : (length)) < (0)
						? (0)
						: ((select_end) < (length) ? (select_end) : (length)));
			text_edit.mode = (byte) (NK_TEXT_EDIT_MODE_INSERT);
			nk_do_edit(ref ws, _out_, (RectangleF) (edit), (uint) (NK_EDIT_FIELD | NK_EDIT_AUTO_SELECT), filters[filter], text_edit,
				style.edit, ((state) == (NK_PROPERTY_EDIT)) ? _in_ : null, font);
			cursor = (int) (text_edit.cursor);
			select_begin = (int) (text_edit.select_start);
			select_end = (int) (text_edit.select_end);
			if (((text_edit.active) != 0) && ((nk_input_is_key_pressed(_in_, (int) (NK_KEY_ENTER))) != 0))
				text_edit.active = (byte) (nk_false);
			if (((active) != 0) && (text_edit.active == 0))
			{
				state = (int) (NK_PROPERTY_DEFAULT);

				fixed (char* ptr = buffer)
				{
					switch (variant->kind)
					{
						default:
							break;
						case NK_PROPERTY_INT:
							variant->value.i = (int) (nk_strtoi(ptr, null));
							variant->value.i =
								(int)
									(((variant->value.i) < (variant->max_value.i) ? (variant->value.i) : (variant->max_value.i)) <
									 (variant->min_value.i)
										? (variant->min_value.i)
										: ((variant->value.i) < (variant->max_value.i) ? (variant->value.i) : (variant->max_value.i)));
							break;
						case NK_PROPERTY_FLOAT:
							nk_string_float_limit(ptr, (int)(2));
							variant->value.f = (float)(nk_strtof(ptr, null));
							variant->value.f =
								(float)
									(((variant->value.f) < (variant->max_value.f) ? (variant->value.f) : (variant->max_value.f)) <
									 (variant->min_value.f)
										? (variant->min_value.f)
										: ((variant->value.f) < (variant->max_value.f) ? (variant->value.f) : (variant->max_value.f)));
							break;
						case NK_PROPERTY_DOUBLE:
							nk_string_float_limit(ptr, (int)(2));
							variant->value.d = (double)(nk_strtod(ptr, null));
							variant->value.d =
								(double)
									(((variant->value.d) < (variant->max_value.d) ? (variant->value.d) : (variant->max_value.d)) <
									 (variant->min_value.d)
										? (variant->min_value.d)
										: ((variant->value.d) < (variant->max_value.d) ? (variant->value.d) : (variant->max_value.d)));
							break;
					}
				}
			}

		}

		public static int Color_picker_behavior(ref uint state, RectangleF* bounds, RectangleF* matrix, RectangleF* hue_bar,
			RectangleF* alpha_bar, Colorf* color, nk_input _in_)
		{
			float* hsva = stackalloc float[4];
			int value_changed = (int) (0);
			int hsv_changed = (int) (0);
			Colorf_hsva_fv(hsva, (Colorf) (*color));
			if ((nk_button_behavior(ref state, (RectangleF) (*matrix), _in_, (int) (NK_BUTTON_REPEATER))) != 0)
			{
				hsva[1] =
					(float)
						((0) <
						 ((1.0f) < ((_in_.mouse.pos.X - matrix->x)/(matrix->w - 1))
							 ? (1.0f)
							 : ((_in_.mouse.pos.X - matrix->x)/(matrix->w - 1)))
							? ((1.0f) < ((_in_.mouse.pos.X - matrix->x)/(matrix->w - 1))
								? (1.0f)
								: ((_in_.mouse.pos.X - matrix->x)/(matrix->w - 1)))
							: (0));
				hsva[2] =
					(float)
						(1.0f -
						 ((0) <
						  ((1.0f) < ((_in_.mouse.pos.Y - matrix->y)/(matrix->h - 1))
							  ? (1.0f)
							  : ((_in_.mouse.pos.Y - matrix->y)/(matrix->h - 1)))
							 ? ((1.0f) < ((_in_.mouse.pos.Y - matrix->y)/(matrix->h - 1))
								 ? (1.0f)
								 : ((_in_.mouse.pos.Y - matrix->y)/(matrix->h - 1)))
							 : (0)));
				value_changed = (int) (hsv_changed = (int) (1));
			}

			if ((nk_button_behavior(ref state, (RectangleF) (*hue_bar), _in_, (int) (NK_BUTTON_REPEATER))) != 0)
			{
				hsva[0] =
					(float)
						((0) <
						 ((1.0f) < ((_in_.mouse.pos.Y - hue_bar->y)/(hue_bar->h - 1))
							 ? (1.0f)
							 : ((_in_.mouse.pos.Y - hue_bar->y)/(hue_bar->h - 1)))
							? ((1.0f) < ((_in_.mouse.pos.Y - hue_bar->y)/(hue_bar->h - 1))
								? (1.0f)
								: ((_in_.mouse.pos.Y - hue_bar->y)/(hue_bar->h - 1)))
							: (0));
				value_changed = (int) (hsv_changed = (int) (1));
			}

			if ((alpha_bar) != null)
			{
				if ((nk_button_behavior(ref state, (RectangleF) (*alpha_bar), _in_, (int) (NK_BUTTON_REPEATER))) != 0)
				{
					hsva[3] =
						(float)
							(1.0f -
							 ((0) <
							  ((1.0f) < ((_in_.mouse.pos.Y - alpha_bar->y)/(alpha_bar->h - 1))
								  ? (1.0f)
								  : ((_in_.mouse.pos.Y - alpha_bar->y)/(alpha_bar->h - 1)))
								 ? ((1.0f) < ((_in_.mouse.pos.Y - alpha_bar->y)/(alpha_bar->h - 1))
									 ? (1.0f)
									 : ((_in_.mouse.pos.Y - alpha_bar->y)/(alpha_bar->h - 1)))
								 : (0)));
					value_changed = (int) (1);
				}
			}

			if (((state) & NK_WIDGET_STATE_MODIFIED) != 0)
				(state) = (uint) (NK_WIDGET_STATE_INACTIVE | NK_WIDGET_STATE_MODIFIED);
			else (state) = (uint) (NK_WIDGET_STATE_INACTIVE);
			if ((hsv_changed) != 0)
			{
				*color = (Colorf) (nk_hsva_colorfv(hsva));
				state = (uint) (NK_WIDGET_STATE_ACTIVE);
			}

			if ((value_changed) != 0)
			{
				color->a = (float) (hsva[3]);
				state = (uint) (NK_WIDGET_STATE_ACTIVE);
			}

			if ((nk_input_is_mouse_hovering_rect(_in_, (RectangleF) (*bounds))) != 0) state = (uint) (NK_WIDGET_STATE_HOVERED);
			if (((state & NK_WIDGET_STATE_HOVER) != 0) && (nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (*bounds)) == 0))
				state |= (uint) (NK_WIDGET_STATE_ENTERED);
			else if ((nk_input_is_mouse_prev_hovering_rect(_in_, (RectangleF) (*bounds))) != 0) state |= (uint) (NK_WIDGET_STATE_LEFT);
			return (int) (value_changed);
		}

		public static int nk_do_color_picker(ref uint state, nk_command_buffer _out_, Colorf* col, int fmt, RectangleF bounds,
			Vector2 padding, nk_input _in_, nk_user_font font)
		{
			int ret = (int) (0);
			RectangleF matrix = new RectangleF();
			RectangleF hue_bar = new RectangleF();
			RectangleF alpha_bar = new RectangleF();
			float bar_w;
			if ((((_out_ == null) || (col == null))) || (font == null)) return (int) (ret);
			bar_w = (float) (font.Height);
			bounds.X += (float) (padding.X);
			bounds.Y += (float) (padding.X);
			bounds.Width -= (float) (2*padding.X);
			bounds.Height -= (float) (2*padding.Y);
			matrix.X = (float) (bounds.X);
			matrix.Y = (float) (bounds.Y);
			matrix.Height = (float) (bounds.Height);
			matrix.Width = (float) (bounds.Width - (3*padding.X + 2*bar_w));
			hue_bar.Width = (float) (bar_w);
			hue_bar.Y = (float) (bounds.Y);
			hue_bar.Height = (float) (matrix.Height);
			hue_bar.X = (float) (matrix.X + matrix.Width + padding.X);
			alpha_bar.X = (float) (hue_bar.X + hue_bar.Width + padding.X);
			alpha_bar.Y = (float) (bounds.Y);
			alpha_bar.Width = (float) (bar_w);
			alpha_bar.Height = (float) (matrix.Height);
			ret =
				(int)
					(Color_picker_behavior(ref state, &bounds, &matrix, &hue_bar, ((fmt) == (NK_RGBA)) ? &alpha_bar : null, col,
						_in_));
			nk_draw_color_picker(_out_, &matrix, &hue_bar, ((fmt) == (NK_RGBA)) ? &alpha_bar : null, (Colorf) (*col));
			return (int) (ret);
		}

		public static nk_style_item nk_style_item_hide()
		{
			nk_style_item i = new nk_style_item();
			i.type = (int) (NK_STYLE_ITEM_COLOR);
			i.data.color = (Color) (nk_rgba((int) (0), (int) (0), (int) (0), (int) (0)));
			return (nk_style_item) (i);
		}

		public static int nk_panel_has_header(uint flags, char* title)
		{
			int active = (int) (0);
			active = (int) (flags & (NK_WINDOW_CLOSABLE | NK_WINDOW_MINIMIZABLE));
			active = (int) (((active) != 0) || ((flags & NK_WINDOW_TITLE) != 0) ? 1 : 0);
			active = (int) ((((active) != 0) && ((flags & NK_WINDOW_HIDDEN) == 0)) && ((title) != null) ? 1 : 0);
			return (int) (active);
		}

		public static int nk_panel_is_sub(int type)
		{
			return (int) ((type & NK_PANEL_SET_SUB) != 0 ? 1 : 0);
		}

		public static int nk_panel_is_nonblock(int type)
		{
			return (int) ((type & NK_PANEL_SET_NONBLOCK) != 0 ? 1 : 0);
		}

		public static nk_property_variant nk_property_variant_int(int value, int min_value, int max_value, int step)
		{
			nk_property_variant result = new nk_property_variant();
			result.kind = (int) (NK_PROPERTY_INT);
			result.value.i = (int) (value);
			result.min_value.i = (int) (min_value);
			result.max_value.i = (int) (max_value);
			result.step.i = (int) (step);
			return (nk_property_variant) (result);
		}

		public static nk_property_variant nk_property_variant_float(float value, float min_value, float max_value, float step)
		{
			nk_property_variant result = new nk_property_variant();
			result.kind = (int) (NK_PROPERTY_FLOAT);
			result.value.f = (float) (value);
			result.min_value.f = (float) (min_value);
			result.max_value.f = (float) (max_value);
			result.step.f = (float) (step);
			return (nk_property_variant) (result);
		}

		public static nk_property_variant nk_property_variant_double(double value, double min_value, double max_value,
			double step)
		{
			nk_property_variant result = new nk_property_variant();
			result.kind = (int) (NK_PROPERTY_DOUBLE);
			result.value.d = (double) (value);
			result.min_value.d = (double) (min_value);
			result.max_value.d = (double) (max_value);
			result.step.d = (double) (step);
			return (nk_property_variant) (result);
		}
	}
}