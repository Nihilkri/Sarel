using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NihilKri;

namespace Sarel {
public partial class Form1 : Form {
	#region Variables
	#region Physics
	#region Constants
	const double PI = 3.14159265358979323846264338327950288419716939937510; // Diameter to Circumference
	const double AR = PI / 180.0; // Degrees to Radians
	const int    c  = 299792458; // Speed of light, in m/s
	const double G  = 6.673848e-11; // Gravitational Constant, in m3/kg/s2 or N(m/kg)2
	const double h  = 6.6260695729e-34; // Planck Constant, in J*s or N*m*s or kg*m2/s
	const double hb = h / 2 / PI; // Reduced Planck Constant, in J*s or N*m*s or kg*m2/s
	#endregion Constants
	#region Sarel
	static V3[,,] ras = new V3[64, 64, 64];

	#endregion Sarel
	#endregion Physics
	#region Graphics
	static Graphics gb,gf; static Bitmap gi;
	static int fx, fy, fx2, fy2;
	static V3 cam = new V3(0,0,0);
	static Bitmap Kii;
	#endregion Graphics
	static string hx = "FFFFFFFF";
	static uint hxn = Convert.ToUInt32(hx, 16);
	#endregion Variables

	public Form1() {InitializeComponent();}
	private void Form1_Load(object sender, EventArgs e) {
		fx2 = (fx = Width) / 2; fy2 = (fy = Height) / 2;
		gi = new Bitmap(fx, fy);
		gb = Graphics.FromImage(gi);
		gf = CreateGraphics();

		timer1.Start();
		//if(!uint.TryParse("0x" + hx, out hxn)) hxn = 0;
	}
	private void Form1_KeyDown(object sender, KeyEventArgs e) {
		switch(e.KeyCode) {
			case Keys.Escape: Close(); break;

			default: break;
		}

	}

	static Pen[] ps = { Pens.White, Pens.Red, Pens.Orange, Pens.Yellow, Pens.Green, Pens.Blue, Pens.Violet };
	static double[] wx = new double[ps.Length], wy = new double[ps.Length], woy = new double[ps.Length], wwl = new double[ps.Length];
	private void Form1_Paint(object sender, PaintEventArgs e) {
		gb.Clear(Color.Black);
		gb.DrawString(hxn.ToString(), Font, Brushes.White, 0, 0);
		gb.DrawString(fx.ToString() + ", " + fx2.ToString(), Font, Brushes.White, 0, 20);
		gb.DrawString(wx[1].ToString(), Font, Brushes.White, 0, 40);

		VField vf = new VField(fx, fy);
		Kii = new Bitmap(128, 128);
		//if(x > fx - 128 && y < 128) { p.abi((x + 64.0 - fx) / 64.0, (64.0 - y) / 64.0); } else {
		Complex p = new Complex(0, 0), d = new Complex(0, 0);
		double px = 0.0, py = 0.0;
		for(int x = 0 ; x < 128 ; x++) { for(int y = 0 ; y < 128 ; y++) {
			p.abi((x - 64.0) / 64.0, (64.0 - y) / 64.0);
			Kii.SetPixel(x, y, Color.FromArgb(p.c));
		}}
		double skl = 1.0 / 8.0;
		//double[] gx = new double[] { -256.0, 256.0, 0.0, 0.0 };
		//double[] gy = new double[] { 0.0, 0.0, 256.0, -256.0 };
		//double[] gm = new double[] { 512.0, 512.0, 128.0, -512.0 };
		double[] gx = new double[] { 0.0, 0.0 };
		double[] gy = new double[] { 0.0, 0.0 };
		double[] gm = new double[] { 0.0, 512.0 };
		for(int q = 0 ; q < gx.GetLength(0) ; q++) { gx[q] *= skl; gy[q] *= skl; gm[q] *= skl * skl * skl; }
		for(int y = 0 ; y < fy ; y++) { py = (fy2 - y) * skl;
			for(int x = 0 ; x < fx ; x++) { px = (x - fx2) * skl;
				p.abi(0.0, -0.0);
				for(int q = 0 ; q < gx.GetLength(0) ; q++) {//
					d = new Complex(gx[q] - px, gy[q] - py);
					//p = d;
					//p += Complex.Cis(Physics.Fg(1.0, gm[q], d.r), d.t);
					p += Complex.Cis(512.0 * skl * (px != 0 ? 16.0 : 1.0) * gm[q] / d.r / d.r, d.t);
					//p += Complex.Cis(d.t, 32.0 * (px > 0 ? 4.0 : 1.0) * gm[q] / d.r / d.r);
					//p += Complex.Cis(d.r, d.t);
				}
				//p.abi(px, py);
				//p.abi(Math.Sin(py), Math.Sin(px));
				vf._VF[x, y] = new Complex(p.a, p.b);
				gi.SetPixel(x, y, Color.FromArgb(p.c));// ^ ~0x7FFFFFFF

			}
		}
		gb.DrawLine(Pens.Black, 0, fy2, fx, fy2);
		gb.DrawLine(Pens.Black, fx2, 0, fx2, fy);

		p.abi(-32.0, -32.0); Complex v = new Complex(-0.2, 0.2); int opx = 0, opy = 0;
		px = (p.a / skl) + fx2; py = fy2 - (p.b / skl);
		for(int q = 0 ; q < 1000 ; q++) {
			opx = (int)px; opy = (int)py; 
			px = (p.a / skl) + fx2; py = fy2 - (p.b / skl);
			if(!(new Rectangle(0, 0, fx, fy).Contains((int)px, (int)py))) continue;
			d = vf._VF[(int)px, (int)py];
			if(d.r > 16.0) d *= -1.0;
			v += d / 256.0;
			p += v;

			//gi.SetPixel((int)px, (int)py, Color.FromArgb(v.c));
			gb.FillEllipse(Brushes.Black, (int)px - 2, (int)py - 2, 5, 5);
			gb.DrawLine(new Pen(Color.FromArgb((-1.5*v.norm()).c)), opx, opy, (int)px, (int)py);
		}


		//{// rb, rg, gb
		//	int skl = 16;
		//	for(int r = 0 ; r < 256 ; r += skl)
		//		for(int g = 0 ; g < 256 ; g += skl)
		//			for(int b = 0 ; b < 256 ; b += skl)
		//				gb.FillRectangle(new SolidBrush(Color.FromArgb(r, g, b)),
		//				0, 0, 16, 16);
		//	//r g 0, r g 80, r g b	0 g b, 80 g b	r 0 b, r 80 b
		//}
		//WavePaint();
		wx[0] += 5; for(int q = 0 ; q < wx[0] && q < 361 ; q++)
			gb.DrawArc(Pens.White, 250, 250, 500, 500, 0, q);

		gb.DrawString("Test:\n" + KN.Test(), Font, Brushes.White, 20, 220);

		gf.DrawImage(gi, 0, 0);
		gf.DrawImage(Kii, fx - 128, 0);
		timer1.Stop();
	}

	private void timer1_Tick(object sender, EventArgs e) {
		//for(int w = 1 ; w < ps.Length ; w++) x[w] = (x[w] + fx - 0.1) % fx;

			Form1_Paint(sender, new PaintEventArgs(gf, new Rectangle(0, 0, fx, fy)));
	}

	private void WavePaint() {
		for(int q = 1 ; q < fx ; q++) {
			gb.DrawLine(Pens.Gray, q - 1, (int)(fy2), q, (int)(fy2)); wy[0] = 0;
			for(int w = 1 ; w < ps.Length ; w++) {
				//wl[w] = 1 << w;
				wwl[w] = w*8;
				wy[w] = Math.Sin(q * PI / fx2 * wwl[w] + wx[w]) * (512/wwl[w]); wy[0] += wy[w];
				gb.DrawLine(ps[w], q - 1, (int)(fy2 - woy[w]), q, (int)(fy2 - wy[w]));
				woy[w] = wy[w];


			}
			gb.DrawLine(ps[0], q - 1, (int)(fy2 - woy[0]), q, (int)(fy2 - wy[0]));
			woy[0] = wy[0];


		}


	}

	private void Form1_MouseMove(object sender, MouseEventArgs e) {

	}
} // class Form1
public class V3 {
	public static V3 X = new V3(1, 0, 0), Y = new V3(0, 1, 0), Z = new V3(0, 0, 1);
	public static double operator *(V3 l, V3 r) { return l.x * r.x + l.y * r.y + l.z * r.z; } // Dot Product
	public static V3 operator %(V3 l, V3 r) {
		return new V3(l.y * r.z - l.z * r.y, l.z * r.x - l.x * r.z, l.x * r.y - l.y * r.x);
	} // Cross Product
	public double x, y, z;
	public V3(double nx, double ny, double nz) { x = nx; y = ny; z = nz; }
	public double mag() { return Math.Sqrt(x * x + y * y + z * z); }
	public V3 norm() { double m = mag(); return new V3(x / m, y / m, z / m); }
	public override string ToString() { return "(" + x + ", " + y + ", " + z + ")"; }
} // class V3
public class V4 {
	public static V4 W = new V4(1, 0, 0, 0), X = new V4(0, 1, 0, 0), Y = new V4(0, 0, 1, 0), Z = new V4(0, 0, 0, 1);
	public static double operator *(V4 l, V4 r) { return l.w * r.w + l.x * r.x + l.y * r.y + l.z * r.z; } // Dot Product
	//public static V4 operator %(V4 l, V4 r) { return new V4(l.y * r.z - l.z * r.y, l.z * r.x - l.x * r.z, l.x * r.y - l.y * r.x); } // Cross Product
	public double w, x, y, z;
	public V4(double nw, double nx, double ny, double nz) { w = nw; x = nx; y = ny; z = nz; }
	public double mag() { return Math.Sqrt(w * w + x * x + y * y + z * z); }
	public V4 norm() { double m = mag(); return new V4(w / m, x / m, y / m, z / m); }
	public override string ToString() { return "(" + w + ", " + x + ", " + y + ", " + z + ")"; }
} // class V4
public class Sarel {
	private V3 v; public double m;
	public Sarel(V3 nv, double nm) { v = nv; m = nm; }
	public Sarel(double nx, double ny, double nz, double nm) : this(new V3(nx, ny, nz), nm) { }


} // class Sarel
public class VField {
	public Complex[,] _VF;
	public VField() { }
	public VField(int nx, int ny) { _VF = new Complex[nx, ny]; }
	public VField(double nx, double ny, double sk) { _VF = new Complex[(int)(2 * nx / sk), (int)(2 * ny / sk)]; }


}
} // Namespace Sarel
