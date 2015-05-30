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
	#endregion Graphics
	static string hx = "FFFFFFFF";
	static uint hxn = Convert.ToUInt32(hx, 16);
	static Mat matest; static Matrix2 kntest; static double madet;
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
	static double[] x = new double[ps.Length], y = new double[ps.Length], oy = new double[ps.Length], wl = new double[ps.Length];
	private void Form1_Paint(object sender, PaintEventArgs e) {
		gb.Clear(Color.Black);
		gb.DrawString(hxn.ToString(), Font, Brushes.White, 0, 0);
		gb.DrawString(fx.ToString() + ", " + fx2.ToString(), Font, Brushes.White, 0, 20);
		gb.DrawString(x[1].ToString(), Font, Brushes.White, 0, 40);

		double[,] d = { { 2, 1, -2, -1 }, { 1, -1, -1, 1 }, { 4, 2, 2, 1 }, { 8, 1, 1, 2 } };
		matest = new Mat(d); madet = matest.det();
		gb.DrawString("Det                       = " + madet.ToString(), Font, Brushes.White, 0, 80);
		gb.DrawString("(" + matest[0, 0] + ", " + matest[0, 1] + ", " + matest[0, 2] + ", " + matest[0, 3] + ")", Font, Brushes.White, 20, 59);
		gb.DrawString("(" + matest[1, 0] + ", " + matest[1, 1] + ", " + matest[1, 2] + ", " + matest[1, 3] + ")", Font, Brushes.White, 20, 73);
		gb.DrawString("(" + matest[2, 0] + ", " + matest[2, 1] + ", " + matest[2, 2] + ", " + matest[2, 3] + ")", Font, Brushes.White, 20, 87);
		gb.DrawString("(" + matest[3, 0] + ", " + matest[3, 1] + ", " + matest[3, 2] + ", " + matest[3, 3] + ")", Font, Brushes.White, 20, 101);

		double[][] dd = {   new double[] { 2, 1, -2, -1 }, new double[] { 1, -1, -1, 1 }, 
							new double[] { 4, 2, 2, 1 }, new double[] { 8, 1, 1, 2 } };
		kntest = (new Matrix2(5, 5, 2, 1, -2, -1, 2, 1, -1, -1, 1, 4, 4, 2, 2, 1, 8, 8, 1, 1, 2, 10, 1, 3, 5, 7, 9));
		madet = kntest.det();
		gb.DrawString("Det                       = " + madet.ToString(), Font, Brushes.White, 0, 160);
		gb.DrawString("(" + kntest[0, 0] + ", " + kntest[1, 0] + ", " + kntest[2, 0] + ", " + kntest[3, 0] + ")", Font, Brushes.White, 20, 139);
		gb.DrawString("(" + kntest[0, 1] + ", " + kntest[1, 1] + ", " + kntest[2, 1] + ", " + kntest[3, 1] + ")", Font, Brushes.White, 20, 153);
		gb.DrawString("(" + kntest[0, 2] + ", " + kntest[1, 2] + ", " + kntest[2, 2] + ", " + kntest[3, 2] + ")", Font, Brushes.White, 20, 167);
		gb.DrawString("(" + kntest[0, 3] + ", " + kntest[1, 3] + ", " + kntest[2, 3] + ", " + kntest[3, 3] + ")", Font, Brushes.White, 20, 181);

		// rb, rg, gb
		int skl = 16;
		for(int r = 0 ; r < 256 ; r += skl)
		for(int g = 0 ; g < 256 ; g += skl)
		for(int b = 0 ; b < 256 ; b += skl)
			gb.FillRectangle(new SolidBrush(Color.FromArgb(r, g, b)),
			0, 0, 16, 16);
		//r g 0, r g 80, r g b	0 g b, 80 g b	r 0 b, r 80 b

		//WavePaint();
		x[0] += 5; for(int q = 0 ; q < x[0] && q < 361 ; q++)
			gb.DrawArc(Pens.White, 250, 250, 500, 500, 0, q);

		gb.DrawString("Test:\n" + KN.Test(), Font, Brushes.White, 20, 220);

		gf.DrawImage(gi, 0, 0); timer1.Stop();
	}

	private void timer1_Tick(object sender, EventArgs e) {
		//for(int w = 1 ; w < ps.Length ; w++) x[w] = (x[w] + fx - 0.1) % fx;

			Form1_Paint(sender, new PaintEventArgs(gf, new Rectangle(0, 0, fx, fy)));
	}

	private void WavePaint() {
		for(int q = 1 ; q < fx ; q++) {
			gb.DrawLine(Pens.Gray, q - 1, (int)(fy2), q, (int)(fy2)); y[0] = 0;
			for(int w = 1 ; w < ps.Length ; w++) {
				//wl[w] = 1 << w;
				wl[w] = w*8;
				y[w] = Math.Sin(q * PI / fx2 * wl[w] + x[w]) * (512/wl[w]); y[0] += y[w];
				gb.DrawLine(ps[w], q - 1, (int)(fy2 - oy[w]), q, (int)(fy2 - y[w]));
				oy[w] = y[w];


			}
			gb.DrawLine(ps[0], q - 1, (int)(fy2 - oy[0]), q, (int)(fy2 - y[0]));
			oy[0] = y[0];


		}


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
public class Mat {
	private double[,] dat;
	private int sizey, sizex;
	public double this[int y, int x] { get { return dat[y, x]; } set { dat[y, x] = value; } }
	public Mat(int sizexy) : this(sizexy, sizexy) { }
	public Mat(int nsizey, int nsizex) {
		sizey = (nsizey < 1) ? 1 : nsizey; sizex = (nsizex < 1) ? 1 : nsizex; dat = new double[sizey, sizex];
		for(int y = 0 ; y < sizey ; y++) for(int x = 0 ; x < sizex ; x++) dat[y, x] = 0.0;
	}
	public Mat(double[,] d) {
		sizey = d.GetLength(0); sizex = d.GetLength(1); dat = new double[sizey, sizex];
		for(int y = 0 ; y < sizey ; y++) for(int x = 0 ; x < sizex ; x++) dat[y, x] = d[y, x];
	}

	public double det() {
		double d = 0, dr = 0, dl = 0;
		for(int x = 0 ; x < sizex - (sizex == 2 ? 1 : 0) ; x++) {
			dr = dl = 1;
			for(int y = 0 ; y < sizey ; y++) {
				dr *= dat[y, (x + y) % sizex]; dl *= dat[sizey - 1 - y, (x + y) % sizex];
			}
			d += dr - dl;
		}
		return d;
	}



}
public class Sarel {
	private V3 v; public double m;
	public Sarel(V3 nv, double nm) { v = nv; m = nm; }
	public Sarel(double nx, double ny, double nz, double nm) : this(new V3(nx, ny, nz), nm) { }


} // class Sarel
} // Namespace Sarel
