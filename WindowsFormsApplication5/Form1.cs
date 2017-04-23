using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        PaintBase paint;
        public Form1()
        {
            InitializeComponent();
            paint = new PaintBase(pictureBox1);
            numericUpDown1.Value = (decimal)paint.pen.Width;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            paint.tool = Tool.Pencil;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            paint.tool = Tool.Line;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            paint.tool = Tool.Triangle;
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPG file|*.jpg|PNG files|*.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                paint.SaveImage(saveFileDialog1.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                paint.btm = (Bitmap)pictureBox1.Image;
                paint.picture = pictureBox1;
                paint.picture.Image = paint.btm;
                paint.g = Graphics.FromImage(paint.btm);
                paint.path.Reset();

            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            paint.prev = e.Location;
            if (paint.tool == Tool.Fill && paint.fillColor!= paint.btm.GetPixel(e.Location.X, e.Location.Y)) { paint.fillSpace(e.Location, paint.btm.GetPixel(e.Location.X, e.Location.Y)); }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && paint.tool!=Tool.Fill)
            {
                paint.Draw(e.Location);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            paint.SaveLastPath();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            paint.tool = Tool.Rectangle;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            paint.tool = Tool.Circle;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                if (paint.tool != Tool.Fill) paint.pen.Color = colorDialog1.Color;
                else paint.fillColor = colorDialog1.Color;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            paint.pen.Width = (float)numericUpDown1.Value;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            paint.tool = Tool.Fill;
        }
    }
}
