using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.IO.Compression;
using System.Collections;
using System.Runtime.InteropServices;
using System.Resources;
using DevComponents.DotNetBar;

namespace CW_File_Viewer_4
{
    public partial class FrmCut : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FrmCut()
        {
            InitializeComponent();
        }


        int i, j, js, wsum = 1000, lsjs, azs = 3;
        int mode = 1, mode4 = 1, fcn = 8, fsn;
        string fc = ".!?;:。！？";
        string fs;
        char[] cfc = new char[100];
        char[] cfs = new char[100];
        char[] ls1 = new char[1];
        char[] ls2 = new char[1];
        bool pda()
        {
            if (j < Gib.l)
            {
                ls1[0] = Gib.a[j];
                string lins1 = new string(ls1);
                /*
                if (Gib.a[k] == '.')
                {
                    MessageBox.Show(lins, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None);
                    if (lins == ".")
                        MessageBox.Show(lins, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                }
                */
                int lsi;
                for (lsi = 0; lsi < Gib.cutn; ++lsi)
                {
                    if (Gib.a[j] == Gib.cutc[lsi])
                    {
                        //MessageBox.Show(infile, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None);
                        if (j < Gib.l - 1)
                        {
                            //ls2[0] = Gib.a[j + 1];
                            //lins2 = new string(ls2);
                            while (j < Gib.l - 1 && (Gib.a[j + 1] == '\"' || Gib.a[j + 1] == '”' || Gib.a[j + 1] == '\''))
                            {
                                ++j;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        bool pdc()
        {
            if (Gib.a[j] == '\n')
                return true;
            return false;
        }

        private void FrmCut_Load(object sender, EventArgs e)
        {
            this.cut.Focus();
        }

        void cutma()
        {
            FrmMain frm = (FrmMain)this.Owner;
            if (cutadd.Text != "")
            {
                js = 0;
                bool reald = false;
                string llast = Path.GetExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                Directory.CreateDirectory(Gib.crossfile + cutadd.Text);
                Gib.a = Gib.infile.ToCharArray();
                Gib.infile = "";
                string fn, word = "";
                //l = Gib.infile.Length;
                for (i = 0; i < Gib.l; ++i)
                {
                    ++js;
                    fn = Gib.crossfile + cutadd.Text + "\\";
                    if (addz.Checked == true)
                        for (j = 1; j < azs; ++j)
                            if (js < Math.Pow(10, j))
                                fn = fn + '0';
                    fn = fn + js.ToString() + llast;

                    //MessageBox.Show(fn, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    for (j = i; j < Gib.l; ++j)
                    {
                        if (Gib.a[j] != '\n')
                            reald = true;
                        //if(j<l)
                        //MessageBox.Show(Gib.infile+' '+j.ToString()+' '+Gib.a[j]+' '+l.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        //cout<<Gib.a[j];
                        if (j < Gib.l && reald == true)
                        {
                            word = word + Gib.a[j];
                            Gib.infile = Gib.infile + Gib.a[j];
                        }
                        //fout<<Gib.a[j];
                        if (pda() == true)
                        {
                            if (Gib.p == 1)
                            {
                                //fout<<Gib.a[j];
                                //fout<<Gib.a[j]<<' '<<i<<' '<<j<<' '<<x<<' '<<y<<' '<<js<<endl;
                                //cout<<Gib.a[j];
                                Gib.infile = Gib.infile + Gib.a[j];
                                Gib.p = 0;
                            }
                            //MessageBox.Show(i.ToString() + ' ' + j.ToString() + ' ' + Gib.a[j] + ' ' + l.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                            if (reald == true)
                            {
                                Gib.infile = Gib.infile + "\r\n";
                                word = word + "\r\n";
                            }
                            //cout<<endl;
                            break;
                        }

                        prog.Value = (int)((double)j / (double)Gib.l * 10000.0);
                    }
                    //MessageBox.Show(word+"\n"+js.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.None);

                    prog.Value = (int)((double)i / (double)Gib.l * 10000.0);

                    if (llast == ".txt" || llast == ".TXT")
                        File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                    else
                        File.WriteAllText(fn, word);
                    word = "";

                    i = j;
                }
                //MessageBox.Show(Gib.infile, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                //FrmMain frm=new FrmMain();
                //FrmMain frm = (FrmMain)this.Owner;
                frm.lrcv.Text = Gib.infile;
                frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
                //((FrmCut)FrmMain).;
            }
            else
                MessageBox.Show("分割后文件夹添加名不得为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            prog.Value = 10000;
            frm.nowstate.Text = "完成";
        }
        void cutmb()
        {
            FrmMain frm = (FrmMain)this.Owner;
            if (cutadd.Text != "")
            {
                string llast = Path.GetExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                Directory.CreateDirectory(Gib.crossfile + cutadd.Text);
                Gib.a = Gib.infile.ToCharArray();
                string fn, word = "";
                js = 1;
                lsjs = 1;

                if (upfile.Checked == true)
                {
                    Gib.infile = "";
                    for (i = 0; i < Gib.l; ++i)
                    {
                        if (lsjs > wsum)
                        {
                            fn = Gib.crossfile + cutadd.Text + "\\";
                            if (addz.Checked == true)
                                for (j = 1; j < azs; ++j)
                                    if (js < Math.Pow(10, j))
                                        fn = fn + '0';
                            fn = fn + js.ToString() + llast;
                            Gib.infile = Gib.infile + "\r\n";
                            word = word + "\r\n";
                            if (llast == ".txt" || llast == ".TXT")
                                File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                            else
                                File.WriteAllText(fn, word);
                            word = "";
                            lsjs = 1;
                            ++js;
                        }
                        else
                        {
                            Gib.infile = Gib.infile + Gib.a[i];
                            word = word + Gib.a[i];
                            ++lsjs;
                            if (lsjs > wsum)
                            {
                                fn = Gib.crossfile + cutadd.Text + "\\";
                                if (addz.Checked == true)
                                    for (j = 1; j < azs; ++j)
                                        if (js < Math.Pow(10, j))
                                            fn = fn + '0';
                                fn = fn + js.ToString() + llast;
                                Gib.infile = Gib.infile + "\r\n";
                                word = word + "\r\n";
                                if (llast == ".txt" || llast == ".TXT")
                                    File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                                else
                                    File.WriteAllText(fn, word);
                                word = "";
                                lsjs = 1;
                                ++js;
                            }
                            else if (i == Gib.l - 1)
                            {
                                fn = Gib.crossfile + cutadd.Text + "\\";
                                if (addz.Checked == true)
                                    for (j = 1; j < azs; ++j)
                                        if (js < Math.Pow(10, j))
                                            fn = fn + '0';
                                fn = fn + js.ToString() + llast;
                                Gib.infile = Gib.infile + "\r\n";
                                word = word + "\r\n";
                                if (llast == ".txt" || llast == ".TXT")
                                    File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                                else
                                    File.WriteAllText(fn, word);
                                word = "";
                                lsjs = 0;
                                ++js;
                            }
                        }

                        prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                    }
                    File.WriteAllText(Gib.crossfile, Gib.infile);
                    frm.lrcv.Text = Gib.infile;
                }
                else
                {
                    for (i = 0; i < Gib.l; ++i)
                    {
                        if (lsjs > wsum)
                        {
                            fn = Gib.crossfile + cutadd.Text + "\\";
                            if (addz.Checked == true)
                                for (j = 1; j < azs; ++j)
                                    if (js < Math.Pow(10, j))
                                        fn = fn + '0';
                            fn = fn + js.ToString() + llast;
                            word = word + "\r\n";
                            if (llast == ".txt" || llast == ".TXT")
                                File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                            else
                                File.WriteAllText(fn, word);
                            word = "";
                            lsjs = 1;
                            ++js;
                        }
                        else
                        {
                            word = word + Gib.a[i];
                            ++lsjs;
                            if (lsjs > wsum)
                            {
                                fn = Gib.crossfile + cutadd.Text + "\\";
                                if (addz.Checked == true)
                                    for (j = 1; j < azs; ++j)
                                        if (js < Math.Pow(10, j))
                                            fn = fn + '0';
                                fn = fn + js.ToString() + llast;
                                word = word + "\r\n";
                                if (llast == ".txt" || llast == ".TXT")
                                    File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                                else
                                    File.WriteAllText(fn, word);
                                word = "";
                                lsjs = 1;
                                ++js;
                            }
                            else if (i == Gib.l - 1)
                            {
                                fn = Gib.crossfile + cutadd.Text + "\\";
                                if (addz.Checked == true)
                                    for (j = 1; j < azs; ++j)
                                        if (js < Math.Pow(10, j))
                                            fn = fn + '0';
                                fn = fn + js.ToString() + llast;
                                word = word + "\r\n";
                                if (llast == ".txt" || llast == ".TXT")
                                    File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                                else
                                    File.WriteAllText(fn, word);
                            }
                        }
                        prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                    }
                }
                frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
            else
                MessageBox.Show("分割后文件夹添加名不得为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            prog.Value = 10000;
            frm.nowstate.Text = "完成";
        }
        void cutmc()
        {
            FrmMain frm = (FrmMain)this.Owner;
            if (cutadd.Text != "")
            {
                js = 0;
                string llast = Path.GetExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                Directory.CreateDirectory(Gib.crossfile + cutadd.Text);
                Gib.a = Gib.infile.ToCharArray();
                string fn, word = "";
                //l = Gib.infile.Length;
                for (i = 0; i < Gib.l; ++i)
                {
                    ++js;
                    fn = Gib.crossfile + cutadd.Text + "\\";
                    if (addz.Checked == true)
                        for (j = 1; j < azs; ++j)
                            if (js < Math.Pow(10, j))
                                fn = fn + '0';
                    fn = fn + js.ToString() + llast;

                    for (j = i; j < Gib.l; ++j)
                    {
                        if (j < Gib.l)
                            word = word + Gib.a[j];
                        else
                            break;
                        if (Gib.a[j] == '\n')
                        {
                            word = word + "\r\n";
                            while(j+1<Gib.l && (Gib.a[j+1]=='\r' || Gib.a[j+1]=='\n'))
                                ++j;
                            break;
                        }
                        prog.Value = (int)((double)j / (double)Gib.l * 10000.0);
                    }
                    prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                    if (llast == ".txt" || llast == ".TXT")
                        File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                    else
                        File.WriteAllText(fn, word);
                    word = "";
                    i = j;
                }
                //MessageBox.Show(Gib.infile, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                //FrmMain frm=new FrmMain();
                //FrmMain frm = (FrmMain)this.Owner;
                frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
                //((FrmCut)FrmMain).;
            }
            else
                MessageBox.Show("分割后文件夹添加名不得为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            prog.Value = 10000;
            frm.nowstate.Text = "完成";
        }
        void cutmd()
        {
            FrmMain frm = (FrmMain)this.Owner;
            int lsk;
            if (cutadd.Text != "")
            {
                js = 0;
                string llast = Path.GetExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                Directory.CreateDirectory(Gib.crossfile + cutadd.Text);
                Gib.a = Gib.infile.ToCharArray();
                Gib.infile = "";
                string fn, word = "", lsw;
                //l = Gib.infile.Length;
                for (i = 0; i < Gib.l; ++i)
                {
                    lsw = "";
                    //MessageBox.Show(fn, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    if (i + fsn < Gib.l)
                        for (j = i; j < i + fsn; ++j)
                            lsw = lsw + Gib.a[j];
                    if (lsw == fs)
                    {
                        for (j = i; j >= 0; --j)
                        {
                            if (mode4 == 1)
                            {
                                if (pda() == true)
                                    break;
                            }
                            else
                            {
                                if (pdc() == true)
                                    break;
                            }
                        }
                        word = "";
                        lsk = j + 1;
                        for (j = lsk; j < Gib.l; ++j)
                        {
                            word = word + Gib.a[j];
                            if (mode4 == 1)
                            {
                                if (pda() == true)
                                    break;
                            }
                            else
                            {
                                if (pdc() == true)
                                    break;
                            }
                        }
                        i = j;
                        ++js;
                        fn = Gib.crossfile + cutadd.Text + "\\";
                        if (addz.Checked == true)
                            for (j = 1; j < azs; ++j)
                                if (js < Math.Pow(10, j))
                                    fn = fn + '0';
                        fn = fn + js.ToString() + llast;
                        if (llast == ".txt" || llast == ".TXT")
                            File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                        else
                            File.WriteAllText(fn, word);
                        Gib.infile = Gib.infile + word + '\n';
                    }
                    /*

                        //MessageBox.Show(Gib.infile+' '+j.ToString()+' '+Gib.a[j]+' '+l.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        
                        if (pdc() == true)
                        {
                            if (Gib.p == 1)
                            {
                                //fout<<Gib.a[j];
                                //fout<<Gib.a[j]<<' '<<i<<' '<<j<<' '<<x<<' '<<y<<' '<<js<<endl;
                                //cout<<Gib.a[j];
                                Gib.infile = Gib.infile + Gib.a[j];
                                Gib.p = 0;
                            }
                            //MessageBox.Show(i.ToString() + ' ' + j.ToString() + ' ' + Gib.a[j] + ' ' + l.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                            if (reald == true)
                            {
                                Gib.infile = Gib.infile + "\r\n";
                                word = word + "\r\n";
                            }
                            //cout<<endl;
                            break;
                        }
                        prog.Value = (int)((double)j / (double)Gib.l * 10000.0);
                    }
                        */
                    prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                    //MessageBox.Show(word+"\n"+js.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    /*
                    if (llast == ".txt" || llast == ".TXT")
                        File.WriteAllText(fn, word, Encoding.GetEncoding("GB2312"));
                    else
                        File.WriteAllText(fn, word);
                    word = "";
                    i = j;
                    */
                }
                //MessageBox.Show(Gib.infile, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                //FrmMain frm=new FrmMain();
                //FrmMain frm = (FrmMain)this.Owner;
                frm.lrcv.Text = Gib.infile;
                frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
                //((FrmCut)FrmMain).;
            }
            else
                MessageBox.Show("分割后文件夹添加名不得为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            frm.nowstate.Text = "完成";
        }
        private void cut_Click(object sender, EventArgs e)
        {
            FrmMain frm = (FrmMain)this.Owner;
            frm.nowstate.Text = "正在执行操作...";
            //MessageBox.Show(Gib.infile, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
            if (Gib.crossfile != "Null")
            {
                if (mode == 1)
                {
                    cutma();
                }
                else if (mode == 2)
                {
                    cutmb();
                }
                else if (mode == 3)
                {
                    cutmc();
                }
                else
                {
                    if (fs != "")
                    {
                        cutmd();
                    }
                    else
                        MessageBox.Show("查找字符串不得为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("未选中文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            File.WriteAllText(Gib.crossfile, Gib.infile);
            this.Close();
        }

        private void cutch_TextChanged(object sender, EventArgs e)
        {
            Gib.cuts = cutch.Text;
            Gib.cutc = Gib.cuts.ToCharArray();
            Gib.cutn = Gib.cuts.Length;
        }


        private void r1_CheckedChanged(object sender, EventArgs e)
        {
            mode = 1;
        }
        private void r2_CheckedChanged(object sender, EventArgs e)
        {
            mode = 2;
        }
        private void r3_CheckedChanged(object sender, EventArgs e)
        {
            mode = 3;
        }

        private void cutsum_TextChanged(object sender, EventArgs e)
        {
            if (cutsum.Text != "")
            {
                wsum = Convert.ToInt32(cutsum.Text);
            }
            else
            {
                wsum = 1;
                cutsum.Text = "1";
            }
        }
        private void cutsum_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8)
                e.Handled = false;
        }
        private void azsum_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 8)
                e.Handled = false;
        }
        private void azsum_TextChanged(object sender, EventArgs e)
        {
            if (azsum.Text != "")
            {
                azs = Convert.ToInt32(azsum.Text);
                if (azs > 100)
                {
                    azsum.Text = "100";
                    azs = 100;
                }
            }
            else
            {
                azs = 1;
                azsum.Text = "1";
            }
        }

        private void ficut_TextChanged(object sender, EventArgs e)
        {
            fc = ficut.Text;
            cfc = fc.ToCharArray();
            fcn = fc.Length;
        }

        private void r4_CheckedChanged(object sender, EventArgs e)
        {
            mode = 4;
        }

        private void r41_CheckedChanged(object sender, EventArgs e)
        {
            mode4 = 2;
        }

        private void r42_CheckedChanged(object sender, EventArgs e)
        {
            mode4 = 1;
        }

        private void finds_TextChanged(object sender, EventArgs e)
        {
            fs = finds.Text;
            cfs = fs.ToCharArray();
            fsn = fs.Length;
        }

        private void azsum_ValueChanged(object sender, EventArgs e)
        {
            azs = azsum.Value;
        }

        private void cutsum_ValueChanged(object sender, EventArgs e)
        {
            wsum = cutsum.Value;
        }

        private void upfile_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
