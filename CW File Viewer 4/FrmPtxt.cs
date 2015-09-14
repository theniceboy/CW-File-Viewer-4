using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CW_File_Viewer_4
{
    public partial class FrmPtxt : Form
    {
        int lx, ly, fx, fy;
        string infile;
        public FrmPtxt()
        {
            InitializeComponent();
        }
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
        private void FrmPtxt_Load(object sender, EventArgs e)
        {
            //

            //MessageBox.Show("baga1");
        }

        private void textprev_TextChanged(object sender, EventArgs e)
        {

        }

        private void tcanuse_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("baga2");
            this.Show();
            this.Left = Gib.mx;
            this.Top = Gib.my;
            //MessageBox.Show(Gib.ppath + '\n' + Gib.pname + '\n' + Gib.plast);
            if (Gib.plast == ".lrc" || Gib.plast == ".LRC")
            {
                StreamReader fileread = new StreamReader(Gib.ppath);
                textprev.Text = fileread.ReadToEnd();
                fileread.Close();
            }
            else if (Gib.plast == ".txt" || Gib.plast == ".TXT")
            {
                StreamReader fileread = new StreamReader(Gib.ppath, Encoding.GetEncoding("GB2312"));
                textprev.Text = fileread.ReadToEnd();
                fileread.Close();
            }
        }
    }
}
