using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CW_File_Viewer_4
{
    public partial class FrmPpic : Form
    {
        public FrmPpic()
        {
            InitializeComponent();
        }


        int lx, ly, fx, fy;
        string infile;
        public static int X2 = 0;
        public static int Y2 = 0;
        public static int X1 = 0;
        public static int Y1 = 0;
        private void tmv_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show(X1.ToString() + '\n' + Y1.ToString() + '\n' + X2.ToString() + '\n' + Y2.ToString());
            if (X1 == 0 && Y1 == 0)
            {
                X1 = Control.MousePosition.X;
                Y1 = Control.MousePosition.Y;
            }
            X2 = Control.MousePosition.X;
            Y2 = Control.MousePosition.Y;

            if (X1 != X2 || Y1 != Y2)
            {
                X1 = X2;
                Y1 = Y2;
                Point spoint = Control.MousePosition;
                Point fpoint = this.PointToClient(Control.MousePosition);
                lx = spoint.X;
                ly = spoint.Y;
                fx = fpoint.X;
                fy = fpoint.Y;
                //MessageBox.Show(lx.ToString() + '\n' + ly.ToString() + '\n' + fx.ToString() + '\n' + fy.ToString() + '\n' + this.Size.Width + '\n' + this.Size.Height);
                if (Math.Abs(Gib.mx - lx) > 1 || Math.Abs(Gib.my - ly) > 1)
                {
                    //ClientRectangle 
                    if (fx >= 0 && fx <= this.Size.Width && fy >= 0 && fy <= this.Size.Height)
                    {
                        //
                    }
                    else
                    {
                        //MessageBox.Show("Close");
                        //FrmPtxt frm = (FrmPtxt)this.Owner;
                        //frm.Close();
                        this.Close();
                    }
                }
            }

        }
        /*
        private void FrmPtxt_MouseMove(object sender, MouseEventArgs e)
        {
            Point spoint = Control.MousePosition;
            Point fpoint = this.PointToClient(Control.MousePosition);
            lx = spoint.X;
            ly = spoint.Y;
            fx = fpoint.X;
            fy = fpoint.Y;
            MessageBox.Show(lx.ToString() + '\n' + ly.ToString() + '\n' + fx.ToString() + '\n' + fy.ToString()+'\n'+this.Size.Width+'\n'+this.Size.Height);
            if(Math.Abs(Gib.mx-lx)>1 || Math.Abs(Gib.my-ly)>1)
            {
                //ClientRectangle 
                if (fx >= 0 && fx <= this.Size.Width && fy >= 0 && fy <= this.Size.Height)
                {
                    MessageBox.Show("Close");
                }
                else
                {
                    FrmMain frm = (FrmMain)this.Owner;
                    frm.Close();
                }
            }
        }
        */
        private void FrmPpic_Load(object sender, EventArgs e)
        {

        }
        public static string PictureWidth, Pictureheight;
        private void tcanuse_Tick(object sender, EventArgs e)
        {
            //
            this.Show();
            this.Left = Gib.mx;
            this.Top = Gib.my;//MessageBox.Show("baga2");
            /*
            if (Convert.ToInt32(PictureWidth) > 1024)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                Image ig = Image.FromFile(Gib.ppath);
                
                this.Width = Convert.ToInt32(ig.Width);
                this.Height = Convert.ToInt32(ig.Height);
            }
            */
            try
            {
                picprev.Image = Image.FromFile(Gib.ppath);
                //this.Text = Gib.ppath + "   " + "(" + PictureWidth + "×" + Pictureheight + ")";
            }
            catch
            {
                this.Close();
            }
        }

        private void picprev_Click(object sender, EventArgs e)
        {

        }

        private void tmv_Tick_1(object sender, EventArgs e)
        {

        }

        private void tcanuse_Tick_1(object sender, EventArgs e)
        {

        }


    }
}
