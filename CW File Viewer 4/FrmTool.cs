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
    public partial class FrmTool : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        int i, j;
        char[] ls1 = new char[1];
        char[] ls2 = new char[1];
        public FrmTool()
        {
            InitializeComponent();
        }
        int pdc(char px)
        {
            /*
             * English : 1
             * Number  : 2
             * symbol  : 3
             * Chinese : 4
             * Other   : 5
            */
            if ((px >= 'a' && px <= 'z') || (px >= 'A' && px <= 'Z'))
                return 1;
            if (px >= '0' && px <= '9')
                return 2;
            if (px == '@' || px == '#' || px == '$' || px == '%' || px == '^' || px == '&' || px == '*' || px == '+' || px == '[' || px == ']' || px == '<' || px == '>' || px == '=' || px == '_' || px == '`' || px == '~' || px == ',' || px == ' ' || px == '.' || px == '?' || px == '!' || px == '(' || px == ')' || px == '\"' || px == '\'' || px == ';' || px == ':' || px == '-')
                return 3;
            ls1[0] = px;
            string lins1 = new string(ls1);
            //code = Char.ConvertToUtf32(px, px);    //获得字符串input中指定索引index处字符unicode编码
            if (px > 128)
                return 4;
            return 5;
        }
        private void Tool_Load(object sender, EventArgs e)
        {

        }

        private void killenter_Click(object sender, EventArgs e)
        {
            if (Gib.infile != "")
            {
                Gib.a = Gib.infile.ToCharArray();
                Gib.l = Gib.infile.Length;
                string lsinf = "";
                for (i = 0; i < Gib.l; ++i)
                {
                    if (i < Gib.l - 1)
                    {
                        if (Gib.a[i] == '\r' && Gib.a[i + 1] == '\n')
                        {
                            if (i < Gib.l - 3)
                            {
                                if ((Gib.a[i + 2] == '\r' && Gib.a[i + 3] == '\n') == false)
                                    lsinf = lsinf + Gib.a[i];
                            }
                            else
                                lsinf = lsinf + Gib.a[i];
                        }
                        else
                            lsinf = lsinf + Gib.a[i];
                    }
                    else
                        lsinf = lsinf + Gib.a[i];
                }
                Gib.infile = lsinf;
                FrmMain frm = (FrmMain)this.Owner;
                frm.lrcv.Text = Gib.infile;
            }
            this.Close();
        }
        public void finish()
        {
            FrmMain lft = new FrmMain();
            lft.Updatetext();
            FrmTool lft1 = new FrmTool();
            lft1.Close();
        }
        private void alltoone_Click(object sender, EventArgs e)
        {
            string str = "", lsstr;
            for (i = 1; i <= Gib.fsum; ++i)
            {
                if (Gib.clast[i] == ".txt" || Gib.clast[i] == ".TXT")
                {
                    StreamReader tfileread = new StreamReader(Gib.cpath[i], Encoding.GetEncoding("GB2312"));
                    lsstr = tfileread.ReadToEnd();
                    str = str + lsstr;
                    tfileread.Close();
                }
                if (Gib.clast[i] == ".lrc" || Gib.clast[i] == ".LRC")
                {
                    StreamReader tfileread = new StreamReader(Gib.cpath[i]);
                    lsstr = tfileread.ReadToEnd();
                    str = str + lsstr;
                    tfileread.Close();
                }
            }
            string lsfn = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + "Merge - " + DateTime.Now.ToLongDateString().ToString() + ".txt";
            //MessageBox.Show(lsfn);
            //MessageBox.Show(str);
            File.WriteAllText(lsfn, str, Encoding.GetEncoding("GB2312"));
            FrmMain frm = (FrmMain)this.Owner;
            frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            this.Close();
        }

        private void changecode_Click(object sender, EventArgs e)
        {
            if (Path.GetExtension(Gib.crossfile) != "")
            {
                System.Diagnostics.Process.Start(Gib.crossfile);
                //FrmChangeCode fcc = new FrmChangeCode();
                //fcc.ShowDialog(this);
                this.Close();
            }
            else
                MessageBox.Show("未选择文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void makesamefile_Click(object sender, EventArgs e)
        {
            for (i = 1; ; ++i)
                if (File.Exists(Path.GetDirectoryName(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]) + '\\' + Path.GetFileNameWithoutExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]) + " - 副本" + i.ToString() + Path.GetExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1])) == false)
                    break;
            //MessageBox.Show(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab] + 1] + '\n' + Path.GetDirectoryName(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab] + 1]) + '\\' + Path.GetFileNameWithoutExtension(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab] + 1]) + " - 副本" + i.ToString() + Path.GetExtension(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab] + 1]));
            string sss = Path.GetDirectoryName(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]) + '\\' + Path.GetFileNameWithoutExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]) + " - 副本" + i.ToString() + Path.GetExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);//File.Copy(Path.GetDirectoryName(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab] + 1]), Path.GetDirectoryName(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab] + 1]) + '\\' + Path.GetFileNameWithoutExtension(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab] + 1]) + " - 副本" + i.ToString() + Path.GetExtension(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab] + 1]),true);
            File.Copy(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1], sss);
            FrmMain frm = (FrmMain)this.Owner;
            frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            this.Close();
        }

        private void killenglish_Click(object sender, EventArgs e)
        {
            Gib.infile = "";
            for (i = 0; i < Gib.l; ++i)
                if (pdc(Gib.a[i]) != 1 && pdc(Gib.a[i]) != 3)
                {
                    Gib.infile = Gib.infile + Gib.a[i];
                    //MessageBox.Show(Gib.infile, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            FrmMain frm = (FrmMain)this.Owner;
            frm.lrcv.Text = Gib.infile;
            this.Close();
        }

        private void killchinese_Click(object sender, EventArgs e)
        {
            Gib.infile = "";
            for (i = 0; i < Gib.l; ++i)
                if (pdc(Gib.a[i]) != 2 && pdc(Gib.a[i]) != 4)
                {
                    Gib.infile = Gib.infile + Gib.a[i];
                    //MessageBox.Show(Gib.infile, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            FrmMain frm = (FrmMain)this.Owner;
            frm.lrcv.Text = Gib.infile;
            this.Close();
        }

        private void recover_Click(object sender, EventArgs e)
        {
            FrmMain frm = (FrmMain)this.Owner;
            Gib.infile = Gib.backup;
            frm.lrcv.Text = Gib.infile;
        }
    }
}
/*
asdfasaui hsadimfasdfhsamdfuusah dsaf 
ads fsadf dmsaimhas df


fsadf sadf isamdfiumaishdf asdf
asdf sadf
asdf sadf\\\\\

abstractsfasd
Attribute .ads
sadf

*/