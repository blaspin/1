using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Rectangle rect;
        private Dictionary<Rectangle, int> rects = new Dictionary<Rectangle, int>();
        private readonly Random rand = new Random();
        private Pen pen;
        private System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        private int cyclesToDel = 5;
        private bool exit = false;
        public Form1()
        {
            InitializeComponent();
            
            Width = 800;
            Height = 600;
            Show();

            myTimer.Interval = 1000;
            myTimer.Tick += new EventHandler(DrawNewRectangle);
            myTimer.Start();
            while (!exit)
            {
                Application.DoEvents();
            }
        }
        private void DrawNewRectangle(Object myObject, EventArgs myEventArgs)
        {
            myTimer.Stop();

            int x = rand.Next(0, Width);
            int y = rand.Next(0, Height);
            rect = new Rectangle(x, y, rand.Next(0, Width - x), rand.Next(0, Height - y));            
            pen = new Pen(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));

            rects.Add(rect, 0);
            
            var tempKeys = rects.Keys.ToArray();
            var tempValues = rects.ToArray();
            rects.ToList().FindAll(kvp => kvp.Value > 0).ForEach(kvp => rects[kvp.Key] = kvp.Value + 1);

            foreach (Rectangle rect in tempKeys)
            {
                if(!Rectangle.Intersect(rect, this.rect).IsEmpty && !rect.Equals(this.rect) && rects[rect] == 0)
                {
                    rects[rect] = 1;
                }
            }

            foreach(var kvp in tempValues)
            {
                if(kvp.Value == cyclesToDel)
                {
                    rects.Remove(kvp.Key);
                }
            }

            this.Refresh();
            this.CreateGraphics().DrawRectangles(pen, rects.Keys.ToArray());

            myTimer.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myTimer.Stop();
            exit = true;
        }
    }
}
