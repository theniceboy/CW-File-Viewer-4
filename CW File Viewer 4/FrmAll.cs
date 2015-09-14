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
    public partial class FrmAll : DevComponents.DotNetBar.Metro.MetroForm
    {
        int i, j, k, la, lc, azs = 3, dsum, dell, num1 = 1, num2 = 100, foun = 4,nowtab=1;
        string addn, chgn, dels;
        char[] an = new char[100];
        char[] cn = new char[100];
        char[] delc = new char[100];
        public FrmAll()
        {
            InitializeComponent();
        }
        private void FrmAll_Load(object sender, EventArgs e)
        {

        }

        private void nadd_TextChanged(object sender, EventArgs e)
        {
            addn = nadd.Text;
            la = addn.Length;
            an = addn.ToCharArray();
        }
        private void nchg_TextChanged(object sender, EventArgs e)
        {
            chgn = nchg.Text;
            cn = chgn.ToCharArray();
            lc = chgn.Length;
        }

        void work1()
        {
            FrmMain frm = (FrmMain)this.Owner;
            frm.nowstate.Text = "正在执行操作...";
            if (r11.Checked==true)
            {
                string newname;
                for (i = 1; i <= Gib.fsum; ++i)
                {
                    newname = "";
                    //MessageBox.Show(i+' '+j+'\n'+Gib.cpath[i] + '\n' + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\n' + newname + '\n' + Gib.clast[i], "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    for (j = 0; j < lc; ++j)
                    {
                        if (cn[j] == '/')
                        {
                            if (addz.Checked == true)
                            {
                                if (addz.Checked == true)
                                    for (k = 1; k < azs; ++k)
                                        if (i < Math.Pow(10, k))
                                            newname = newname + '0';
                            }
                            newname = newname + i.ToString();
                        }
                        else
                            newname = newname + cn[j].ToString();
                        //MessageBox.Show(j.ToString());
                    }
                    //MessageBox.Show(i.ToString() + ' ' + j.ToString() + '\n' + Gib.cpath[i] + '\n' + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\n' + newname + '\n' + Gib.clast[i], "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                    if (Gib.cmode[i] == 1 && Gib.cpath[i] != Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i])
                        File.Move(Gib.cpath[i], Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i]);
                    prog.Value = (int)((double)i / (double)Gib.fsum * 10000.0);
                }
            }
            else if (r12.Checked == true)
            {
                string newname;
                if (r121.Checked == true)
                {
                    for (i = 1; i <= Gib.fsum; ++i)
                    {
                        newname = "";
                        for (j = 0; j < la; ++j)
                        {
                            if (an[j] == '/')
                            {
                                if (addz.Checked == true)
                                {
                                    if (i < 10)
                                        newname = newname + "0";
                                    if (i < 100)
                                        newname = newname + "0";
                                }
                                newname = newname + i.ToString();
                            }
                            else
                                newname = newname + an[j].ToString();
                        }
                        prog.Value = (int)((double)i / (double)Gib.fsum * 10000.0);
                        //MessageBox.Show(Gib.cpath[i] + '\n' + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\\' + System.IO.Path.GetFileNameWithoutExtension(Gib.cpath[i]) + newname + Gib.clast[i], "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        if (Gib.cmode[i] == 1)
                            File.Move(Gib.cpath[i], Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + newname + Path.GetFileNameWithoutExtension(Gib.cpath[i]) + Gib.clast[i]);
                            
                    }
                }
                else
                {
                    for (i = 1; i <= Gib.fsum; ++i)
                    {
                        newname = "";
                        for (j = 0; j < la; ++j)
                        {
                            if (an[j] == '/')
                            {
                                if (addz.Checked == true)
                                {
                                    if (i < 10)
                                        newname = newname + "0";
                                    if (i < 100)
                                        newname = newname + "0";
                                }
                                newname = newname + i.ToString();
                            }
                            else
                                newname = newname + an[j].ToString();
                        }
                        prog.Value = (int)((double)i / (double)Gib.fsum * 10000.0);
                        //MessageBox.Show(Gib.cpath[i] + '\n' + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\\' + System.IO.Path.GetFileNameWithoutExtension(Gib.cpath[i]) + newname + Gib.clast[i], "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        if (Gib.cmode[i] == 1)
                            File.Move(Gib.cpath[i], Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileNameWithoutExtension(Gib.cpath[i]) + newname + Gib.clast[i]);
                            
                    }
                }
            }
            else if (r13.Checked == true)
            {
                if (r131.Checked == true)
                {
                    int lsl = 0;
                    bool bj;
                    string newname;
                    char[] lsoldc = new char[1000];
                    for (i = 1; i <= Gib.fsum; ++i)
                    {
                        newname = "";
                        lsoldc = Path.GetFileNameWithoutExtension(Gib.cpath[i]).ToCharArray();
                        lsl = lsoldc.Length;
                        for (j = 0; j < lsl; ++j)
                        {
                            bj = true;
                            for (k = j; k < j + dell; ++k)
                                if (lsoldc[k] != delc[k - j])
                                {
                                    bj = false;
                                    break;
                                }
                            if (bj == true)
                            {
                                j = j + dell - 1;
                            }
                            else
                            {
                                newname += lsoldc[j].ToString();
                            }
                        }
                        prog.Value = (int)((double)i / (double)Gib.fsum * 10000.0);
                        //MessageBox.Show(Gib.cpath[i] + "\r\n" + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + "\r\n" + '\\' + "\r\n" + newname + "\r\n" + Gib.clast + "\r\n" + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i]);
                        //MessageBox.Show(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\n' + newname);
                        //MessageBox.Show(newname + '\n' + Gib.clast[i]);// + '\n' + newname + Gib.clast[i]);
                        //MessageBox.Show(Gib.cpath[i] + '\n' + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i], "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        if (Gib.cmode[i] == 1 && Gib.cpath[i] != Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i])
                            File.Move(Gib.cpath[i], Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i]);
                    }
                }
                else
                {
                    int lsl = 0;
                    string newname;
                    char[] lsoldc = new char[1000];
                    for (i = 1; i <= Gib.fsum; ++i)
                    {
                        newname = "";
                        lsoldc = Path.GetFileNameWithoutExtension(Gib.cpath[i]).ToCharArray();
                        lsl = lsoldc.Length;
                        //MessageBox.Show(lsl.ToString() + '\n' + mode4.ToString()+'\n'+dsum.ToString());
                        if (r1321.Checked)
                            for (j = dsum; j < lsl; ++j)
                                newname += lsoldc[j].ToString();
                        else
                            for (j = 0; j < lsl - dsum; ++j)
                                newname += lsoldc[j].ToString();
                        //MessageBox.Show(newname);
                        prog.Value = (int)((double)i / (double)Gib.fsum * 10000.0);
                        //MessageBox.Show(Gib.cpath[i] + "\r\n" + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + "\r\n" + '\\' + "\r\n" + newname + "\r\n" + Gib.clast + "\r\n" + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i]);
                        //MessageBox.Show(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\n' + newname);
                        //MessageBox.Show(newname + '\n' + Gib.clast[i]);// + '\n' + newname + Gib.clast[i]);
                        //MessageBox.Show(Gib.cpath[i] + '\n' + Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i], "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        if (Gib.cmode[i] == 1 && Gib.cpath[i] != Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i])
                            File.Move(Gib.cpath[i], Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + newname + Gib.clast[i]);
                    }
                }
            }
            frm.nowstate.Text = "完成";
            //MessageBox.Show(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\n' + Gib.smode, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
            frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            this.Close();
        }

        void work2()
        {
            FrmMain frm = (FrmMain)this.Owner;
            frm.nowstate.Text = "正在执行操作...";
            if (r21.Checked == true)
            {
                Gib.a = Gib.infile.ToCharArray();
                Gib.l = Gib.infile.Length;
                int lsjs = 2;
                string filename = "";
                string wrfile;
                //MessageBox.Show(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut");
                Directory.CreateDirectory(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut");
                if (r211.Checked == true)
                {
                    if (writeinfile.Checked == true)
                    {
                        for (i = 0; i < Gib.l; ++i)
                        {
                            wrfile = "";
                            for (j = 0; j < foun; ++j)
                            {
                                if (Gib.a[i] != ' ' && Gib.a[i] != '　' && Gib.a[i] != '\r')
                                    filename = filename + Gib.a[i].ToString();

                                if (i >= Gib.l)
                                    break;
                                if (Gib.a[i + 1] == '\n')
                                    break;
                                if (Gib.a[i] == ' ' || Gib.a[i] == '　' || Gib.a[i] == '\r')
                                    --j;
                                wrfile = wrfile + Gib.a[i].ToString();
                                ++i;
                                prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                            }
                            for (j = 0; ; ++j)
                            {
                                if (i >= Gib.l)
                                    break;
                                //if (Gib.a[i+1] == '\r' && Gib.a[i + 2] == '\n')
                                //    break;
                                if (i + 1 < Gib.l && Gib.a[i + 1] == '\n')
                                    break;
                                wrfile = wrfile + Gib.a[i].ToString();
                                ++i;
                                prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                            }
                            if (i < Gib.l)
                            {
                                wrfile = wrfile + Gib.a[i].ToString();
                                ++i;
                            }
                            //MessageBox.Show('\"' + filename + '\"');
                            //MessageBox.Show('\"' + wrfile + '\"');
                            //MessageBox.Show(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt");
                            if (filename != "")
                            {
                                if (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt") == true)
                                {
                                    while (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + "（" + lsjs.ToString() + "）.txt") == true)
                                        ++lsjs;
                                    File.WriteAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + "（" + lsjs.ToString() + "）.txt", wrfile, Encoding.GetEncoding("GB2312"));
                                }
                                else
                                    File.WriteAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt", wrfile, Encoding.GetEncoding("GB2312"));
                            }
                            filename = "";
                        }
                    }
                    else
                    {
                        wrfile = "";
                        for (i = 0; i < Gib.l; ++i)
                        {
                            for (j = 0; j < foun; ++j)
                            {
                                if (Gib.a[i] != ' ' && Gib.a[i] != '　' && Gib.a[i] != '\r')
                                    filename = filename + Gib.a[i].ToString();

                                if (i >= Gib.l)
                                    break;
                                if (Gib.a[i + 1] == '\n')
                                    break;
                                if (Gib.a[i] == ' ' || Gib.a[i] == '　' || Gib.a[i] == '\r')
                                    --j;
                                ++i;
                                prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                            }
                            for (j = 0; ; ++j)
                            {
                                if (i >= Gib.l)
                                    break;
                                //if (Gib.a[i+1] == '\r' && Gib.a[i + 2] == '\n')
                                //    break;
                                if (i + 1 < Gib.l && Gib.a[i + 1] == '\n')
                                    break;
                                ++i;
                                prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                            }
                            if (i < Gib.l)
                            {
                                ++i;
                            }
                            //MessageBox.Show('\"' + filename + '\"');
                            //MessageBox.Show('\"' + wrfile + '\"');
                            //MessageBox.Show(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]] + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt");
                            if (filename != "")
                            {
                                if (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt") == true)
                                {
                                    while (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + "（" + lsjs.ToString() + "）.txt") == true)
                                        ++lsjs;
                                    File.WriteAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + "（" + lsjs.ToString() + "）.txt", wrfile, Encoding.GetEncoding("GB2312"));
                                }
                                else
                                    File.WriteAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt", wrfile, Encoding.GetEncoding("GB2312"));
                            }
                            filename = "";
                        }
                    }
                }
                else
                {
                    if (writeinfile.Checked == true)
                    {
                        for (i = 0; i < Gib.l; ++i)
                        {
                            filename = "";
                            wrfile = "";
                            for (j = 0; ; ++j)
                            {
                                if (i >= Gib.l)
                                    break;
                                //if (Gib.a[i+1] == '\r' && Gib.a[i + 2] == '\n')
                                //    break;
                                if (i + 1 < Gib.l && Gib.a[i + 1] == '\n')
                                    break;
                                wrfile = wrfile + Gib.a[i].ToString();
                                if (Gib.a[i] != '\r')
                                    filename = filename + Gib.a[i].ToString();
                                ++i;
                                prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                            }
                            if (i < Gib.l)
                            {
                                wrfile = wrfile + Gib.a[i].ToString();
                                if (Gib.a[i] != '\r')
                                    filename = filename + Gib.a[i].ToString();
                                ++i;
                            }
                            if (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt") == true)
                            {
                                while (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + "（" + lsjs.ToString() + "）.txt") == true)
                                    ++lsjs;
                                File.WriteAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + "（" + lsjs.ToString() + "）.txt", wrfile, Encoding.GetEncoding("GB2312"));
                            }
                            else
                                File.WriteAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt", wrfile, Encoding.GetEncoding("GB2312"));
                            filename = "";
                        }
                    }
                    else
                    {
                        wrfile = ""; for (i = 0; i < Gib.l; ++i)
                        {
                            filename = "";
                            for (j = 0; ; ++j)
                            {
                                if (i >= Gib.l)
                                    break;
                                if (i + 1 < Gib.l && Gib.a[i + 1] == '\n')
                                    break;
                                if (Gib.a[i] != '\r')
                                    filename = filename + Gib.a[i].ToString();
                                ++i;
                                prog.Value = (int)((double)i / (double)Gib.l * 10000.0);
                            }
                            if (i < Gib.l)
                            {
                                if (Gib.a[i] != '\r')
                                    filename = filename + Gib.a[i].ToString();
                                ++i;
                            }
                            if (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt") == true)
                            {
                                while (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + "（" + lsjs.ToString() + "）.txt") == true)
                                    ++lsjs;
                                File.WriteAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + "（" + lsjs.ToString() + "）.txt", wrfile, Encoding.GetEncoding("GB2312"));
                            }
                            else
                                File.WriteAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileName(Gib.txtpath) + " - viewcut\\" + filename + ".txt", wrfile, Encoding.GetEncoding("GB2312"));
                            filename = "";
                        }
                    }
                    frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
                    this.Close();
                }
            }
            else
            {
                if (fn2.Text != "")
                {
                    string filename;
                    int alll = fnum2.Value - fnum1.Value + 1;
                    for (i = fnum1.Value; i <= fnum2.Value; ++i)
                    {
                        filename = "";
                        an = fn2.Text.ToCharArray();
                        for (j = 0; j < fn2.Text.Length; ++j)
                        {
                            if (an[j] == '/')
                            {
                                if (addz.Checked == true)
                                {
                                    if (addz.Checked == true)
                                        for (k = 1; k < azs; ++k)
                                            if (i < Math.Pow(10, k))
                                                filename = filename + '0';
                                }
                                filename += i.ToString();
                            }
                            else
                                filename += an[j].ToString();
                        }
                        File.Create(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + filename + ".txt");
                        prog.Value = (int)((double)(i - fnum1.Value) / (double)alll * 10000.0);
                    }
                    frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
                    this.Close();
                }
                else
                    MessageBox.Show("文件名不得为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void work3()
        {
            int i;
            FrmMain frm = (FrmMain)this.Owner;
            if (r31.Checked)
            {
                for (i = 1; i <= Gib.fsum; ++i)
                {
                    if (Gib.clast[i] != '.'+allchanget.Text && File.Exists(Gib.cpath[i]))
                        File.Move(Gib.cpath[i],
                            Path.GetDirectoryName(Gib.cpath[i]) + '\\' + Path.GetFileNameWithoutExtension(Gib.cname[i]) +
                            '.' + allchanget.Text);
                    prog.Value = (int)((double)i / (double)Gib.fsum * 10000.0);
                }
            }
            else
            {
                for (i = 1; i <= Gib.fsum; ++i)
                {
                    if (Gib.clast[i].ToLower() != '.'+changet2.Text.ToLower() && File.Exists(Gib.cpath[i]) && Gib.clast[i].ToLower() == '.'+changet1.Text.ToLower())
                        File.Move(Gib.cpath[i],
                            Path.GetDirectoryName(Gib.cpath[i]) + '\\' + Path.GetFileNameWithoutExtension(Gib.cname[i]) +
                            '.' + changet2.Text);
                    prog.Value = (int)((double)i / (double)Gib.fsum * 10000.0);
                }
            }
            frm.reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            this.Close();
        }

        private void work4()
        {
            int i;
            if (clearfile.Checked)
            {
                for (i = 1; i <= Gib.fsum; ++i)
                {
                    if (File.Exists(Gib.cpath[i]))
                    {
                        if (Gib.clast[i].ToLower() == ".txt")
                            File.WriteAllText(Gib.cpath[i],
                                writetext.Text,
                                Encoding.GetEncoding("GB2312"));
                        else if (Gib.clast[i].ToLower() == ".lrc")
                            File.WriteAllText(Gib.cpath[i], writetext.Text);
                    }
                    prog.Value = (int) ((double) i/(double) Gib.fsum*10000.0);
                }
            }
            else
            {
                if (writehead.Value)
                {
                    for (i = 1; i <= Gib.fsum; ++i)
                    {
                        if (File.Exists(Gib.cpath[i]))
                        {
                            if (Gib.clast[i].ToLower() == ".txt")
                                File.WriteAllText(Gib.cpath[i],
                                    writetext.Text + File.ReadAllText(Gib.cpath[i], Encoding.GetEncoding("GB2312")),
                                    Encoding.GetEncoding("GB2312"));
                            else if (Gib.clast[i].ToLower() == ".lrc")
                                File.WriteAllText(Gib.cpath[i], writetext.Text + File.ReadAllText(Gib.cpath[i]));
                        }
                        prog.Value = (int) ((double) i/(double) Gib.fsum*10000.0);
                    }

                }
                else
                {
                    for (i = 1; i <= Gib.fsum; ++i)
                    {
                        if (File.Exists(Gib.cpath[i]))
                        {
                            if (Gib.clast[i].ToLower() == ".txt")
                                File.WriteAllText(Gib.cpath[i],
                                    File.ReadAllText(Gib.cpath[i], Encoding.GetEncoding("GB2312")) + writetext.Text,
                                    Encoding.GetEncoding("GB2312"));
                            else if (Gib.clast[i].ToLower() == ".lrc")
                                File.WriteAllText(Gib.cpath[i], File.ReadAllText(Gib.cpath[i]) + writetext.Text);
                        }
                        prog.Value = (int) ((double) i/(double) Gib.fsum*10000.0);
                    }
                }
            }
            Gib.firsto = true;
            this.Close();
        }

        private void go_Click(object sender, EventArgs e)
        {
            if (nowtab==1)
            {
                work1();
            }
            else if(nowtab==2)
            {
                work2();
            }
            else if(nowtab==3)
            {
                work3();
            }
            else
            {
                work4();
            }
        }
        private void delt_TextChanged(object sender, EventArgs e)
        {
            dels = delt.Text;
            delc = dels.ToCharArray();
            dell = dels.Length;
        }

        private void fountn_TextChanged(object sender, EventArgs e)
        {
            if (fountn.Text != "")
            {
                foun = Convert.ToInt32(fountn.Text);
                if (num2 > 10000)
                {
                    fountn.Text = "10000";
                    foun = 10000;
                }
                if (foun < 1)
                {
                    foun = 1;
                    fountn.Text = "1";
                }
            }
            else
            {
                foun = 1;
                fountn.Text = "1";
            }
        }

        private void delsum_ValueChanged(object sender, EventArgs e)
        {
            dsum = delsum.Value;
        }

        private void azsum_ValueChanged(object sender, EventArgs e)
        {
            azs = azsum.Value;
        }

        private void fnum2_ValueChanged(object sender, EventArgs e)
        {
            num2 = fnum2.Value;
            if (num2 < num1)
                num2 = num1;
        }

        private void tab1_Click(object sender, EventArgs e)
        {
            nowtab = 1;
        }

        private void tab2_Click(object sender, EventArgs e)
        {
            nowtab = 2;
        }

        private void tab3_Click(object sender, EventArgs e)
        {
            nowtab = 3;
        }

        private void tab4_Click(object sender, EventArgs e)
        {
            nowtab = 4;
        }

        private void clearfile_CheckedChanged(object sender, EventArgs e)
        {
            if (clearfile.Checked)
                l41.Enabled = l42.Enabled = writehead.Enabled = false;
            else
                l41.Enabled = l42.Enabled = writehead.Enabled = true;
        }
    }
}
