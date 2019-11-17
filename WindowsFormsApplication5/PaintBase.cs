using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public enum Tool {Nothing,  Pencil, Line, Triangle, Rectangle, Circle, Fill };
    public enum SelectedPen {Nothing, Pen, Pencil};
    class PaintBase
    {
        public Graphics g;
        public GraphicsPath path;
        public PictureBox picture;
        public Pen pen, penForPencil;
        public SelectedPen selectedPen;
        public Bitmap btm;
        public Point prev;
        public Tool tool;
        public Color fillColor;

        public PaintBase(PictureBox pictureBox1)
        {
            picture = pictureBox1;
            btm = new Bitmap(picture.Width, picture.Height);
            picture.Image = btm;
            pen = new Pen(Color.Black, 2);
            penForPencil = new Pen(Color.Black, 2);
            selectedPen = SelectedPen.Nothing;
            g = Graphics.FromImage(btm);
            g.Clear(Color.White);
            path = new GraphicsPath();
            tool = Tool.Nothing;
            fillColor = Color.White;
            picture.Paint += PicturePaint;
        }
        private void PicturePaint(object sender, PaintEventArgs e)
        {
            if (path != null)
            {
                e.Graphics.DrawPath(pen, path);
                btm = (Bitmap)picture.Image;
            }

        }

        public void SaveLastPath()
        {
            if (path != null)
            {
                g.DrawPath(pen, path);
                btm = (Bitmap)picture.Image;
            }
                
        }

        public void Draw(Point cur)
        {
            
            switch (tool)
            {
                case Tool.Pencil:
                    g.DrawLine(penForPencil, prev, cur);
                    g.FillEllipse(new SolidBrush(penForPencil.Color), cur.X - pen.Width / 2, cur.Y - pen.Width / 2, pen.Width, pen.Width);
                    prev = cur;
                    break;
                case Tool.Line:
                    path.Reset();
                    path.AddLine(prev, cur);
                    break;
                case Tool.Triangle:
                    Point[] a = new Point[3];
                    a[0].X = (cur.X + prev.X) / 2;
                    a[0].Y = prev.Y;
                    a[1].X = prev.X;
                    a[1].Y = cur.Y;
                    a[2] = cur;
                    path.Reset();
                    path.AddPolygon(a);
                    break;
                case Tool.Rectangle:
                    path.Reset();
                    Rectangle r = new Rectangle(prev.X, prev.Y, cur.X - prev.X, cur.Y- prev.Y);
                    path.AddRectangle(r);
                    break;
                case Tool.Circle:
                    path.Reset();
                    r = new Rectangle(prev.X, prev.Y, cur.X - prev.X, cur.Y - prev.Y);
                    path.AddEllipse(r);
                    break;
                
            }
            picture.Refresh();
        }
        public void fillSpace(Point start, Color colorOfFirstPixel)
        {
            setPixels(start.X, start.Y, colorOfFirstPixel);
        }
        void setPixels(int x, int y, Color colorOfFirstPixel)
        {
            btm.SetPixel(x,y, fillColor);
            if (x!=picture.Width) {
                if (btm.GetPixel(x + 1, y) == colorOfFirstPixel)
                {
                    setPixels(x+1, y, colorOfFirstPixel);
                }
            }
            if (y!=picture.Height) {
                if (btm.GetPixel(x, y + 1) == colorOfFirstPixel)
                {
                    setPixels(x, y+1, colorOfFirstPixel);
                }
            }
            if (x!=0) {
                if (btm.GetPixel(x - 1, y) == colorOfFirstPixel)
                {
                    setPixels(x-1, y, colorOfFirstPixel);
                }
            }
            if (y!=0) {
                if (btm.GetPixel(x, y - 1) == colorOfFirstPixel)
                {
                    setPixels(x, y-1, colorOfFirstPixel);
                }
            }
        }
        public void SaveImage(string filename)
        {
            btm.Save(filename);
        }
    }
}
