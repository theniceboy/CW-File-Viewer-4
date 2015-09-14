using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using DevComponents.DotNetBar;

namespace CW_File_Viewer_4
{
    public partial class FrmMain : DevComponents.DotNetBar.RibbonForm
    {
        /*
         * c : class
         * f : place
        */

        #region Define

        int i, j, js, ls;
        bool isleftkey;
        string tablsname;
        char[] lsstr = new char[100000];
        char[] ls1 = new char[1];


        #endregion

        public FrmMain()
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

        #region File Look
        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfo")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribute, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint Flags);
        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);
        [DllImport("shell32.dll")]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
        bool pdnum(char x)
        {
            if (x >= '0' && x <= '9')
                return true;
            return false;
        }
        bool goodcmp(string ca,string cb)
        {
            if (ca == "" || cb == "")
                return false;
            char[] a = ca.ToCharArray();
            char[] b = cb.ToCharArray();
            int la = ca.Length, lb = cb.Length, lm = la < lb ? la : lb, ia = 0, ib = 0, lsa, lsb;
            while(ia<la && ib<lb)
            {
                if(pdnum(a[ia]) && pdnum(b[ib]))
                {
                    lsa=lsb=0;
                    while(ia<la && pdnum(a[ia]))
                    {
                        lsa = lsa * 10 + a[ia] - '0';
                        ++ia;
                    }
                    while(ib<lb && pdnum(b[ib]))
                    {
                        lsb = lsb * 10 + b[ib] - '0';
                        ++ib;
                    }
                    if (lsa != lsb)
                        return lsa < lsb;
                }
                else
                {
                    if (a[ia] >= 'a' && a[ia] <= 'z')
                        a[ia] -= ' ';
                    if (b[ib] >= 'a' && b[ib] <= 'z')
                        b[ib] -= ' ';
                    if(a[ia]!=b[ib])
                        return a[ia] < b[ib];
                    ++ia;
                    ++ib;
                }
            }
            return false;
        }


        int l,nowx;
        char[] croldpath;
        string crpaths = "";
        private void loadin(CrumbBarItem parent)
        {
            //MessageBox.Show(croldpath.ToString() + '\n' + crpaths + '\n' + l+'\n'+nowx);
            //ToastNotification.Show(this, croldpath.ToString() + '\n' + crpaths + '\n' + l + '\n' + nowx, 5000);
            //MessageBox.Show("Test1");
            // Load folders are selection changes...
            //if (parent == null || parent.SubItems.Count > 0){
            //    MessageBox.Show("out"); return;}
            DirectoryInfo dirInfo = null;
            if (parent.Tag is DriveInfo)
            {
                DriveInfo driveInfo = (DriveInfo) parent.Tag;
                dirInfo = driveInfo.RootDirectory;
            }
            else if (parent.Tag is DirectoryInfo)
            {
                dirInfo = (DirectoryInfo) parent.Tag;
            }
            else
            {
                return;
            }
            parent.SubItems.Clear();
            DirectoryInfo[] subDirectories = dirInfo.GetDirectories();
            string lsfilename;
            if(nowx>3 && nowx<l-1)
                crpaths += '\\';
            for (i = nowx; i < l; ++i)
            {
                if (i == l - 1)
                {
                    crpaths += croldpath[i];
                    break;
                }
                else if (croldpath[i] == '\\')
                    break;
                crpaths += croldpath[i];
            }
            nowx = i + 1;
            lsfilename = Path.GetFileName(crpaths);
            parent.ShowSubItems = true;
            foreach (DirectoryInfo directoryInfo in subDirectories)
            {
                CrumbBarItem node = new CrumbBarItem();
                node.Tag = directoryInfo;
                node.Text = directoryInfo.Name;
                node.Image = Properties.Resources.folderp;
                parent.SubItems.Add(node);
                if (node.Text == lsfilename)
                {
                    if (nowx >= l - 1)
                    {
                        Gib.nowcrst = 1;
                        fpcrumb.SelectedItem = node;
                        break;
                    }
                    loadin(node);
                }
            }
            if (l == 3 && parent.Text.Length == 3)
            {
                Gib.nowcrst = 1;
                fpcrumb.SelectedItem = parent;
            }
        }

        private void putinfilev()
        {
            if (Gib.nowtab == 1)
            {
                tab1.Text = tablsname;
                SHFILEINFO shfi = new SHFILEINFO();
                for (i = 1; i <= Gib.fsum; ++i)
                {
                    if (Gib.cmode[i] == 2)
                    {
                        if (seevery.Checked)
                        {
                            try
                            {
                                if (Directory.GetDirectories(Gib.cpath[i]).Length > 0 ||
                                    Directory.GetFiles(Gib.cpath[i]).Length > 0)
                                    imageList1.Images.Add(Properties.Resources.folder);

                                else
                                    imageList1.Images.Add(Properties.Resources.folderemp);
                            }
                            catch
                            {
                                imageList1.Images.Add(Properties.Resources.folderemp);
                            }
                        }
                        else
                            imageList1.Images.Add(Properties.Resources.folder);
                    }
                    else
                    {
                        if (Gib.clast[i].ToLower() == ".txt" || Gib.clast[i].ToLower() == ".lrc")
                            imageList1.Images.Add(Properties.Resources.text);
                        else if (Gib.clast[i].ToLower() == ".mp3")
                            imageList1.Images.Add(Properties.Resources.MP3);
                        else if (Gib.clast[i].ToLower() == ".png" || Gib.clast[i].ToLower() == ".bmp" ||
                                 Gib.clast[i].ToLower() == ".jpg")
                            imageList1.Images.Add(Properties.Resources.png);
                        else
                        {
                            SHGetFileInfo(Gib.cpath[i], (uint) 0x80, ref shfi,
                                (uint) System.Runtime.InteropServices.Marshal.SizeOf(shfi), (uint) (0x100 | 0x400));
                            //获取文件的图标及类型
                            try
                            {
                                imageList1.Images.Add(Gib.cname[i], (Icon) Icon.FromHandle(shfi.hIcon).Clone());
                            }
                            catch
                            {
                                imageList1.Images.Add(Properties.Resources.error);
                            }
                        }
                        //MessageBox.Show(i.ToString() + '\n' + Gib.cpath[i] + '\n' + Gib.fsum);
                        //filev.Items.Add(Gib.cpath[i], _iconListManager.AddFileIcon(Gib.cpath[i],Gib.cname[i]));
                    }
                    filev1.Items.Add(Gib.cname[i], i - 1);
                    nowprog.Value = i;
                }
                DestroyIcon(shfi.hIcon); //销毁图标
            }
            else if (Gib.nowtab == 2)
            {
                tab2.Text = tablsname;
                SHFILEINFO shfi = new SHFILEINFO();
                for (i = 1; i <= Gib.fsum; ++i)
                {
                    if (Gib.cmode[i] == 2)
                    {
                        if (seevery.Checked)
                        {
                            try
                            {
                                if (Directory.GetDirectories(Gib.cpath[i]).Length > 0 ||
                                    Directory.GetFiles(Gib.cpath[i]).Length > 0)
                                    imageList2.Images.Add(Properties.Resources.folder);

                                else
                                    imageList2.Images.Add(Properties.Resources.folderemp);
                            }
                            catch
                            {
                                imageList2.Images.Add(Properties.Resources.folderemp);
                            }
                        }
                        else
                            imageList2.Images.Add(Properties.Resources.folder);
                    }
                    else
                    {
                        if (Gib.clast[i].ToLower() == ".txt" || Gib.clast[i].ToLower() == ".lrc")
                            imageList2.Images.Add(Properties.Resources.text);
                        else if (Gib.clast[i].ToLower() == ".mp3")
                            imageList2.Images.Add(Properties.Resources.MP3);
                        else if (Gib.clast[i].ToLower() == ".png" || Gib.clast[i].ToLower() == ".bmp" ||
                                 Gib.clast[i].ToLower() == ".jpg")
                            imageList2.Images.Add(Properties.Resources.png);
                        else
                        {
                            SHGetFileInfo(Gib.cpath[i], 0x80, ref shfi,
                                (uint) Marshal.SizeOf(shfi), (0x100 | 0x400));
                            //获取文件的图标及类型
                            try
                            {
                                imageList2.Images.Add(Gib.cname[i], (Icon) Icon.FromHandle(shfi.hIcon).Clone());
                            }
                            catch
                            {
                                imageList2.Images.Add(Properties.Resources.error);
                            }
                        }
                        //MessageBox.Show(i.ToString() + '\n' + Gib.cpath[i] + '\n' + Gib.fsum);
                        //filev.Items.Add(Gib.cpath[i], _iconListManager.AddFileIcon(Gib.cpath[i],Gib.cname[i]));
                    }
                    filev2.Items.Add(Gib.cname[i], i - 1);
                    nowprog.Value = i;
                }
                DestroyIcon(shfi.hIcon); //销毁图标
            }
            else
            {
                tab3.Text = tablsname;
                SHFILEINFO shfi = new SHFILEINFO();
                for (i = 1; i <= Gib.fsum; ++i)
                {
                    if (Gib.cmode[i] == 2)
                    {
                        if (seevery.Checked)
                        {
                            try
                            {
                                if (Directory.GetDirectories(Gib.cpath[i]).Length > 0 ||
                                    Directory.GetFiles(Gib.cpath[i]).Length > 0)
                                    imageList3.Images.Add(Properties.Resources.folder);

                                else
                                    imageList3.Images.Add(Properties.Resources.folderemp);
                            }
                            catch
                            {
                                imageList3.Images.Add(Properties.Resources.folderemp);
                            }
                        }
                        else
                            imageList3.Images.Add(Properties.Resources.folder);
                    }
                    else
                    {
                        if (Gib.clast[i].ToLower() == ".txt" || Gib.clast[i].ToLower() == ".lrc")
                            imageList3.Images.Add(Properties.Resources.text);
                        else if (Gib.clast[i].ToLower() == ".mp3")
                            imageList3.Images.Add(Properties.Resources.MP3);
                        else if (Gib.clast[i].ToLower() == ".png" || Gib.clast[i].ToLower() == ".bmp" ||
                                 Gib.clast[i].ToLower() == ".jpg")
                            imageList3.Images.Add(Properties.Resources.png);
                        else
                        {
                            SHGetFileInfo(Gib.cpath[i], 0x80, ref shfi,
                                (uint) Marshal.SizeOf(shfi), (0x100 | 0x400));
                            //获取文件的图标及类型
                            try
                            {
                                imageList3.Images.Add(Gib.cname[i], (Icon) Icon.FromHandle(shfi.hIcon).Clone());
                            }
                            catch
                            {
                                imageList3.Images.Add(Properties.Resources.error);
                            }
                        }
                        //MessageBox.Show(i.ToString() + '\n' + Gib.cpath[i] + '\n' + Gib.fsum);
                        //filev.Items.Add(Gib.cpath[i], _iconListManager.AddFileIcon(Gib.cpath[i],Gib.cname[i]));
                    }
                    filev3.Items.Add(Gib.cname[i], i - 1);
                    nowprog.Value = i;
                }
                DestroyIcon(shfi.hIcon); //销毁图标
            }

        }

        public void reffile(string repath, int mode)
        {
            if (Directory.Exists(repath) == false)
            {
                if (repath != "")
                    MessageBox.Show("路径不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int i, j, k, progs;
            //Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] = "";
            filepath.Text = repath;

            l = repath.Length;
            croldpath = repath.ToCharArray();

            crpaths = croldpath[0].ToString() + croldpath[1] + croldpath[2];
            //crpathc[3] = '\0';

            CrumbBarItem myComputer = new CrumbBarItem();
            myComputer.Text = "计算机";
            myComputer.Image = Properties.Resources.computer;
            fpcrumb.Items.Add(myComputer);
            // Load disks, we will lazy load folders are disk are selected
            nowx = 3;
            
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in drives)
            {
                if (driveInfo.DriveType != DriveType.Fixed) continue;
                CrumbBarItem node = new CrumbBarItem();
                node.Tag = driveInfo;
                node.Text = driveInfo.Name;
                node.Image = Properties.Resources.hdd;
                myComputer.SubItems.Add(node);
                if (node.Text == Path.GetPathRoot(repath))
                    loadin(node);
            }
            // Finally select My Computer
            Gib.nowcrst = 0;


            //fileprog.IsRunning = true;
            //ToggleEndlessProgress.Execute();
            nowstate.Text = "正在载入...";
            Gib.cname = new string[100000];
            Gib.cpath = new string[100000];
            Gib.clast = new string[100000];
            Gib.cmode = new int[100000];//1:file  2:folder
            //FrmMain frm = (FrmMain)this.Owner;
            if (Gib.nowtab == 1) filev1.Items.Clear();
            else if (Gib.nowtab == 2) filev2.Items.Clear();
            else filev3.Items.Clear();
            File.WriteAllText("C:\\ProgramData\\CW Soft\\CW File Viewer\\TempPath.txt", repath);
            Gib.fsum = 0;
            DirectoryInfo dinfo = new DirectoryInfo(repath);
            //获取指定目录下的所有子目录及文件类型
            FileSystemInfo[] fsinfos = dinfo.GetFileSystemInfos();
            progs = fsinfos.Length;

            if (progs == 0)
                progs = 1;
            nowprog.Maximum = progs;

            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                //++fileprog.Value;
                if (fsinfo is DirectoryInfo)
                {
                    DirectoryInfo dirinfo = new DirectoryInfo(fsinfo.FullName);
                    ++Gib.fsum;
                    Gib.cname[Gib.fsum] = dirinfo.Name;
                    Gib.cpath[Gib.fsum] = repath;
                    if (Gib.cpath[Gib.fsum].Length > 3)
                        Gib.cpath[Gib.fsum] = Gib.cpath[Gib.fsum] + '\\';
                    Gib.clast[Gib.fsum] = "";
                    Gib.cpath[Gib.fsum] = Gib.cpath[Gib.fsum] + Gib.cname[Gib.fsum];
                    Gib.cmode[Gib.fsum] = 2;
                }
                else if (onlyfolder.Checked == false)
                {
                    //使用获取的文件名称实例化FileInfo对象
                    FileInfo finfo = new FileInfo(fsinfo.FullName);
                    ++Gib.fsum;
                    Gib.cname[Gib.fsum] = fsinfo.Name;
                    Gib.cpath[Gib.fsum] = repath;
                    if (Gib.cpath[Gib.fsum].Length > 3)
                        Gib.cpath[Gib.fsum] = Gib.cpath[Gib.fsum] + '\\';
                    Gib.cpath[Gib.fsum] = Gib.cpath[Gib.fsum] + Gib.cname[Gib.fsum];
                    Gib.clast[Gib.fsum] = Path.GetExtension(Gib.cpath[Gib.fsum]);
                    Gib.cmode[Gib.fsum] = 1;
                }
                nowprog.Value = Gib.fsum;
            }
            string lins;
            int ils;
            if (Gib.nowtab == 1) filev1.Columns[0].Text = "共" + Gib.fsum.ToString() + "个对象";
            else if (Gib.nowtab == 2) filev2.Columns[0].Text = "共" + Gib.fsum.ToString() + "个对象";
            else filev3.Columns[0].Text = "共" + Gib.fsum.ToString() + "个对象";
            nowstate.Text = "正在排序...";
            if (mode == 1)
            {
                if (goodsort.Checked == true)
                {
                    Gib.cname[0] = "";
                    for (i = 1; i < Gib.fsum; ++i)
                    {
                        k = i-1;
                        for (j = i; j <= Gib.fsum; ++j)
                            if (goodcmp(Gib.cname[j], Gib.cname[k]))
                                k = j;
                        if (k != i-1)
                        {
                            lins = Gib.cname[i - 1];
                            Gib.cname[i - 1] = Gib.cname[k];
                            Gib.cname[k] = lins;

                            lins = Gib.cpath[i - 1];
                            Gib.cpath[i - 1] = Gib.cpath[k];
                            Gib.cpath[k] = lins;

                            lins = Gib.clast[i - 1];
                            Gib.clast[i - 1] = Gib.clast[k];
                            Gib.clast[k] = lins;

                            ils = Gib.cmode[i - 1];
                            Gib.cmode[i - 1] = Gib.cmode[k];
                            Gib.cmode[k] = ils;
                        }
                    }
                    if (goodcmp(Gib.cname[Gib.fsum], Gib.cname[Gib.fsum - 1]))
                    {
                        lins = Gib.cname[Gib.fsum - 1];
                        Gib.cname[Gib.fsum - 1] = Gib.cname[Gib.fsum];
                        Gib.cname[Gib.fsum] = lins;

                        lins = Gib.cpath[Gib.fsum - 1];
                        Gib.cpath[Gib.fsum - 1] = Gib.cpath[Gib.fsum];
                        Gib.cpath[Gib.fsum] = lins;

                        lins = Gib.clast[Gib.fsum - 1];
                        Gib.clast[Gib.fsum - 1] = Gib.clast[Gib.fsum];
                        Gib.clast[Gib.fsum] = lins;

                        ils = Gib.cmode[Gib.fsum - 1];
                        Gib.cmode[Gib.fsum - 1] = Gib.cmode[Gib.fsum];
                        Gib.cmode[Gib.fsum] = ils;
                    }
                }
                else
                {
                    for (i = 1; i < Gib.fsum; ++i)
                    {
                        k = i - 1;
                        for (j = i; j <= Gib.fsum; ++j)
                            if (String.Compare(Gib.cname[j], Gib.cname[k]) < 0)
                                k = j;
                        if (k != i - 1)
                        {
                            lins = Gib.cname[i - 1];
                            Gib.cname[i - 1] = Gib.cname[k];
                            Gib.cname[k] = lins;

                            lins = Gib.cpath[i - 1];
                            Gib.cpath[i - 1] = Gib.cpath[k];
                            Gib.cpath[k] = lins;

                            lins = Gib.clast[i - 1];
                            Gib.clast[i - 1] = Gib.clast[k];
                            Gib.clast[k] = lins;

                            ils = Gib.cmode[i - 1];
                            Gib.cmode[i - 1] = Gib.cmode[k];
                            Gib.cmode[k] = ils;
                        }
                    }
                }
            }
            else if (mode == 2)
            {
                for (i = 1; i < Gib.fsum; ++i)
                {
                    k = i - 1;
                    for (j = i; j <= Gib.fsum; ++j)
                        if (String.Compare(Gib.clast[j], Gib.clast[k]) < 0)
                            k = j;
                    if (k != i - 1)
                    {
                        lins = Gib.cname[i - 1];
                        Gib.cname[i - 1] = Gib.cname[k];
                        Gib.cname[k] = lins;

                        lins = Gib.cpath[i - 1];
                        Gib.cpath[i - 1] = Gib.cpath[k];
                        Gib.cpath[k] = lins;

                        lins = Gib.clast[i - 1];
                        Gib.clast[i - 1] = Gib.clast[k];
                        Gib.clast[k] = lins;

                        ils = Gib.cmode[i - 1];
                        Gib.cmode[i - 1] = Gib.cmode[k];
                        Gib.cmode[k] = ils;
                    }
                }
            }

            //listView1.Items.Add("temp.txt", FileIconIndex(@"c:/temp/temp.txt"));
            if (Gib.nowtab == 1) imageList1.Images.Clear();
            else if (Gib.nowtab == 2) imageList2.Images.Clear();
            else imageList3.Images.Clear();

            nowstate.Text = "正在更新列表...";

            if (repath != "C:\\" && repath != "D:\\" && repath != "E:\\" && repath != "F:\\")
                tablsname = Path.GetFileName(repath);
            else
                tablsname = repath;
            
            putinfilev();

            nowstate.Text = "完成";
            nowprog.Value = progs;

            try
            {
                if (Gib.lpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] > 0 &&
                    Gib.lpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] <= Gib.fsum)
                {
                    if (Gib.nowtab == 1) filev1.Items[Gib.lpos[Gib.nowtab, Gib.nowp[Gib.nowtab]]].EnsureVisible();
                    else if (Gib.nowtab == 2) filev2.Items[Gib.lpos[Gib.nowtab, Gib.nowp[Gib.nowtab]]].EnsureVisible();
                    else filev3.Items[Gib.lpos[Gib.nowtab, Gib.nowp[Gib.nowtab]]].EnsureVisible();
                }
            }
            catch{ }
        }

        void loadfile()
        {
            //MessageBox.Show("Test3loadf");
            if (isleftkey == true)
            {
                Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                if (Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]].Length > 3)
                    Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] + '\\';
                Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] + Gib.fname;
                Gib.nowchoose = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1];
                //MessageBox.Show(path, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //folderbroser.SelectedPath = folderbroser.SelectedPath;
                //实例化DirectoryInfo对象

                Gib.last = Path.GetExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                //获取指定目录下的所有子目录及文件类型
                if (Directory.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]))
                {
                    ++Gib.nowp[Gib.nowtab];
                    Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
                    //MessageBox.Show(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]], "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                    //filename.Text = "";
                    //Gib.fch[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] = false;
                    reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
                    File.WriteAllText("C:\\ProgramData\\CW Soft\\CW File Viewer\\Temp.txt", filepath.Text);
                }
                else
                {
                    //MessageBox.Show(cfile, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    Gib.hispath[++Gib.hissum] = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1];
                    if (Gib.firsto == false)
                    {
                        if (File.Exists(Gib.txtpath))
                        {
                            if (Path.GetExtension(Gib.txtpath) == "lrc" || Path.GetExtension(Gib.txtpath) == ".txt" || Path.GetExtension(Gib.txtpath) == ".LRC" || Path.GetExtension(Gib.txtpath) == ".TXT")
                            {
                                if (Path.GetExtension(Gib.txtpath) == ".txt" || Path.GetExtension(Gib.txtpath) == ".TXT")
                                    File.WriteAllText(Gib.txtpath, Gib.infile, Encoding.GetEncoding("GB2312"));
                                else if (Path.GetExtension(Gib.txtpath) == ".lrc" || Path.GetExtension(Gib.txtpath) == ".LRC")
                                    File.WriteAllText(Gib.txtpath, Gib.infile);
                                Gib.backup = Gib.infile;
                                txtstate.Text = "未修改";
                            }
                        }
                    }
                    else
                        Gib.firsto = false;
                    filename.Text = Gib.fname;
                    if (Gib.last == ".lrc" || Gib.last == ".txt" || Gib.last == ".LRC" || Gib.last == ".TXT")
                    {
                        Gib.txtpath = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1];

                        if (Gib.last == ".lrc" || Gib.last == ".LRC")
                        {
                            Gib.infile = File.ReadAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                            lrcv.Text = Gib.infile;
                            Gib.backup = Gib.infile;
                            txtstate.Text = "未修改";
                        }
                        else if (Gib.last == ".txt" || Gib.last == ".TXT")
                        {
                            Gib.infile = File.ReadAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1], Encoding.GetEncoding("GB2312"));
                            lrcv.Text = Gib.infile;
                            Gib.backup = Gib.infile;
                            txtstate.Text = "未修改";
                        }
                        Gib.a = Gib.infile.ToCharArray();
                        Gib.l = Gib.infile.Length;
                    }
                    else if (Gib.last == ".mp3")
                    {
                        player.URL = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1];
                        Gib.pstate = 1;
                        playbutton.Enabled = true;
                        player.Ctlcontrols.play();
                        playbutton.Image = Properties.Resources.state21;
                        playbutton.HoverImage = Properties.Resources.state22;
                        playbutton.PressedImage = Properties.Resources.state23;
                        if (looksametxt.Checked == true)
                        {
                            if (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileNameWithoutExtension(Gib.fname) + ".txt"))
                            {
                                Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileNameWithoutExtension(Gib.fname) + ".txt";
                                Gib.nfname = Path.GetFileName(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                                Gib.last = ".txt";
                                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                                Gib.txtpath = Gib.fname;
                                StreamReader fileread = new StreamReader(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Path.GetFileNameWithoutExtension(Gib.fname) + ".txt", Encoding.GetEncoding("GB2312"));
                                Gib.infile = fileread.ReadToEnd();
                                lrcv.Text = Gib.infile;
                                Gib.backup = Gib.infile;
                                txtstate.Text = "未修改";
                                filename.Text = Path.GetFileNameWithoutExtension(filename.Text) + ".txt";
                                fileread.Close();
                            }
                        }
                    }
                    else if (Gib.last == ".cst1" || Gib.last == ".cst2" || Gib.last == ".cst3")
                    {
                        Gib.rtff = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1];
                        Gib.infile = File.ReadAllText(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                        lrcv.Text = Gib.infile;
                        Gib.backup = Gib.infile;
                        txtstate.Text = "未修改";
                    }
                    else if (openotherfile.Checked)//if(Gib.last==".doc" || Gib.last==".docx" || Gib.last==".ppt" || Gib.last==".pptx" || Gib.last==".xls" || Gib.last==".xlsx")
                        System.Diagnostics.Process.Start(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                }
            }
            else
            {
                Gib.rbfile = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                if (Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]].Length > 3)
                    Gib.rbfile = Gib.rbfile + '\\';
                Gib.rbfile = Gib.rbfile + Gib.fname;
                Gib.nowchoose = Gib.rbfile;
            }
        }
        #endregion

        #region Other Voids
        public void Updatetext()
        {
            lrcv.Text = Gib.infile;
        }

        void delfd(string path)
        {
            if (File.Exists(path))
            {
                if (userogbin.Checked)
                {
                    ls = 2;
                    if (File.Exists("C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin\\" + Path.GetFileName(path)))
                    {
                        while (File.Exists("C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin\\" + Path.GetFileNameWithoutExtension(path) + " (" + ls.ToString() + ")" + Path.GetExtension(path)))
                            ++ls;
                        if(Path.GetPathRoot(path)!="C:\\")
                        {
                            pastfile(path, "C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin", Path.GetFileNameWithoutExtension(path) + " (" + ls.ToString() + ")" + Path.GetExtension(path),1);
                            File.Delete(path);
                        }
                        else
                            File.Move(path, "C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin\\" + Path.GetFileNameWithoutExtension(path) + " (" + ls.ToString() + ")" + Path.GetExtension(path));
                    }
                    else
                    {
                        if(Path.GetPathRoot(path)!="C:\\")
                        {
                            pastfile(path, "C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin", Path.GetFileName(path), 1);
                            File.Delete(path);
                        }
                        File.Move(path, "C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin\\" + Path.GetFileName(path));
                    }
                }
                else
                    File.Delete(path);
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
            else if (Directory.Exists(path))
            {
                if (userogbin.Checked)
                {
                    ls = 2;
                    if (Directory.Exists("C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin\\" + Path.GetFileName(path)))
                    {
                        while (Directory.Exists("C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin\\" + Path.GetFileNameWithoutExtension(path) + " (" + ls.ToString() + ")" + Path.GetExtension(path)))
                            ++ls;
                        if (Path.GetPathRoot(path) != "C:\\")
                        {
                            pastfile(path, "C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin", Path.GetFileNameWithoutExtension(path) + " (" + ls.ToString() + ")" + Path.GetExtension(path), 2);
                            Directory.Delete(path, true);
                        }
                        else
                            Directory.Move(path, "C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin\\" + Path.GetFileNameWithoutExtension(path) + " (" + ls.ToString() + ")" + Path.GetExtension(path));
                    }
                    else
                    {
                        if (Path.GetPathRoot(path) != "C:\\")
                        {
                            pastfile(path, "C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin", Path.GetFileName(path), 2);
                            Directory.Delete(path, true);
                        }
                        else
                            Directory.Move(path, "C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin\\" + Path.GetFileName(path));
                    }
                }
                else
                    Directory.Delete(path, true);
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
            else
            {
                MessageBox.Show("路径不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Buttons Click

        public void viewfile_Click(object sender, EventArgs e)
        {

            if (fbroser.ShowDialog() == DialogResult.OK)
            {
                fbroser.SelectedPath = fbroser.SelectedPath;
                Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = fbroser.SelectedPath;
                Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                //实例化DirectoryInfo对象
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
            /*
          folderbroser.SelectedPath = "C:\\360云盘\\Visual Studio 2012\\CW Web Browser\\CW Web Browser 1\\CW Web Browser 1\\CW Web Browser 1\\media\\bmp";
          Gib.fpos[Gib.nowtab,++Gib.nowp[Gib.nowtab]] = folderbroser.SelectedPath;
          Gib.maxn = Gib.maxn > Gib.nowp[Gib.nowtab] ? Gib.maxn : Gib.nowp[Gib.nowtab];
          sfpath.Text = Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]];
          //实例化DirectoryInfo对象
          reffile(Gib.fpos[Gib.nowtab,Gib.nowp[Gib.nowtab]], Gib.smode);*/
        }

        public void back_Click(object sender, EventArgs e)
        {
            if (Gib.nowp[Gib.nowtab] > 1)
            {
                --Gib.nowp[Gib.nowtab];
                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }

        public void next_Click(object sender, EventArgs e)
        {
            if (Gib.nowp[Gib.nowtab] < Gib.maxn[Gib.nowtab])
            {
                ++Gib.nowp[Gib.nowtab];
                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }

        public void write_Click(object sender, EventArgs e)
        {
            /*
            FileStream clear = File.Create(filepath.Text + '\\' + filename.Text);
            clear.Close();
            FileInfo files = new FileInfo(filepath.Text + '\\' + filename.Text);
            StreamWriter save = files.CreateText();
            save.Write(Gib.infile);
            save.Close();
             */
            if (File.Exists(filepath.Text + '\\' + filename.Text))
            {
                if (Gib.last == ".txt" || Gib.last == ".TXT")
                    File.WriteAllText(filepath.Text + '\\' + filename.Text, Gib.infile, Encoding.GetEncoding("GB2312"));
                else
                    File.WriteAllText(filepath.Text + '\\' + filename.Text, Gib.infile);
                Gib.backup = Gib.infile;
                txtstate.Text = "未修改";
            }
            else
                MessageBox.Show("文件不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        public void rename_Click(object sender, EventArgs e)
        {
            if (Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] != "")
            {
                File.Move(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1], Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Gib.nfname);
                Gib.fname = Gib.nfname;
                Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Gib.nfname;
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
            else
                MessageBox.Show("未选中文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void del_Click(object sender, EventArgs e)
        {
            if (Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] != "")
            {
                delfd(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
                Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] = "";
            }
            else
                MessageBox.Show("未选中文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void newfile_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Test");
            if (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + filename.Text) == true)
                MessageBox.Show("文件已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (filename.Text == "")
                MessageBox.Show("文件名不得为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (Gib.nowp[Gib.nowtab] <= 0)
                MessageBox.Show("未选择目录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (Path.GetExtension(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + filename.Text) == "")
                    Directory.CreateDirectory(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + filename.Text);
                else
                {
                    if (File.Exists(filepath.Text + '\\' + filename.Text) == false)
                    {
                        Gib.fname = filename.Text;
                        Gib.last = Path.GetExtension(Gib.fname);
                        if (Gib.last == ".lrc" || Gib.last == ".LRC")
                            File.WriteAllText(filepath.Text + '\\' + filename.Text, "");
                        else if (Gib.last == ".txt" || Gib.last == ".TXT")
                            File.WriteAllText(filepath.Text + '\\' + filename.Text, "", Encoding.GetEncoding("GB2312"));
                        loadfile();
                    }
                }
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }

        public void cut_Click(object sender, EventArgs e)
        {
            if (Gib.nowp[Gib.nowtab] > 0)
            {
                Gib.crossfile = filepath.Text + '\\' + filename.Text;
                FrmCut fcut = new FrmCut();
                fcut.Show(this);
            }
            else
                MessageBox.Show("未选择文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void refresh_Click(object sender, EventArgs e)
        {
            if (Gib.nowp[Gib.nowtab] > 0)
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
        }

        public void copy_Click(object sender, EventArgs e)
        {
            if (Gib.nowp[Gib.nowtab] > 0)
            {
                Gib.copypath = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1];
                Gib.copyfn = Gib.fname;
            }
            else
                MessageBox.Show("未选择文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //sfpath.Text = Gib.copypath;
        }

        public void past_Click(object sender, EventArgs e)
        {
            if (Gib.copypath == "Null")
                MessageBox.Show("未选择要复制的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (Gib.nowp[Gib.nowtab] <= 0)
                MessageBox.Show("未选择目录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (File.Exists(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Gib.copyfn) == true)
                {
                    if (MessageBox.Show("文件已存在，是否替换？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        File.Copy(Gib.copypath, Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Gib.copyfn, true);
                }
                else
                    File.Copy(Gib.copypath, Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Gib.copyfn, true);
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }

        public void batch_Click(object sender, EventArgs e)
        {
            if (Gib.nowp[Gib.nowtab] > 0)
            {
                FrmAll fall = new FrmAll();
                fall.Show(this);
            }
            else
                MessageBox.Show("未选择文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void plus_Click(object sender, EventArgs e)
        {
            filename.Text += ".txt";
        }

        private void toolsbox_Click(object sender, EventArgs e)
        {
            Gib.crossfile = filepath.Text + '\\' + filename.Text;
            FrmTool ftool = new FrmTool();
            ftool.Show(this);
        }

        private void addate_Click(object sender, EventArgs e)
        {
            Gib.l = Gib.infile.Length;
            Gib.a = Gib.infile.ToCharArray();
            Gib.infile = "";
            for (i = 0; i < lrcv.SelectionStart; ++i)
                Gib.infile += Gib.a[i].ToString();
            Gib.infile = Gib.infile + DateTime.Now.ToString("yyyy-MM-dd") + ' ' + DateTime.Now.ToShortTimeString().ToString();
            for (i = lrcv.SelectionStart; i < Gib.l; ++i)
                Gib.infile += Gib.a[i].ToString();
            lrcv.Text = Gib.infile;
        }

        private void bhistory_Click(object sender, EventArgs e)
        {
            FrmHistory fcc = new FrmHistory();
            fcc.Show(this);
        }

        private void go_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(filepath.Text))
            {
                Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = filepath.Text;
                Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }

        private void plusdate_Click(object sender, EventArgs e)
        {
            Gib.nfname = filename.Text;
            int ll = Gib.nfname.Length;
            char[] lsffn = new char[200];
            lsffn = Gib.nfname.ToCharArray();
            Gib.nfname = "";
            for (i = 0; i < filename.SelectionStart; ++i)
                Gib.nfname += lsffn[i].ToString();
            Gib.nfname = Gib.nfname + DateTime.Now.ToString("yyyy-MM-dd") + ' ' + DateTime.Now.Hour.ToString() + '：' + DateTime.Now.Minute.ToString();
            for (i = filename.SelectionStart; i < ll; ++i)
                Gib.nfname += lsffn[i].ToString();
            filename.Text = Gib.nfname;
        }

        private void safeclose_Click(object sender, EventArgs e)
        {
            if (File.Exists(filepath.Text + '\\' + filename.Text))
            {
                if (Gib.last.ToLower() == ".txt")
                    File.WriteAllText(filepath.Text + '\\' + filename.Text, Gib.infile, Encoding.GetEncoding("GB2312"));
                else if (Gib.last.ToLower() == ".lrc" || Gib.last.ToLower() == ".cst1" || Gib.last.ToLower() == ".cst2" || Gib.last.ToLower() == ".cst3")
                    File.WriteAllText(filepath.Text + '\\' + filename.Text, Gib.infile);
            }
            File.WriteAllText("C:\\ProgramData\\CW Soft\\CW File Viewer\\TempPath.txt", Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]]);
            this.FormClosing -= new FormClosingEventHandler(this.FrmMain_FormClosing);//为保证Application.Exit();时不再弹出提示，所以将FormClosing事件取消
            Application.Exit();
        }

        private void Recovery_Click(object sender, EventArgs e)
        {
            try
            {
                filepath.Text = File.ReadAllText("C:\\ProgramData\\CW Soft\\CW File Viewer\\TempPath.txt");
                if (Directory.Exists(filepath.Text))
                {
                    Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = filepath.Text;
                    if (Gib.nowp[Gib.nowtab] > Gib.maxn[Gib.nowtab])
                        Gib.maxn[Gib.nowtab] = Gib.nowp[Gib.nowtab];
                    reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
                }
            }
            catch { }
        }

        private void putup_Click(object sender, EventArgs e)
        {
            if (Gib.isheng == false)
            {
                Gib.isheng = true;
            }
            else
            {

                Gib.isheng = false;
            }
        }

        private void fullscr_Click(object sender, EventArgs e)
        {
            FrmFullscr frm = new FrmFullscr();
            frm.Show();
        }

        private void sortw1_CheckedChanged(object sender, EventArgs e)
        {
            Gib.smode = 1;
            if (Gib.nowp[Gib.nowtab] > 0)
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
        }
        private void sortw2_CheckedChanged(object sender, EventArgs e)
        {
            Gib.smode = 2;
            if (Gib.nowp[Gib.nowtab] > 0)
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
        }

        #endregion

        #region LRCV

        string strtrm(string x)
        {
            lsstr = x.ToCharArray();
            if (pdc(lsstr[lsstr.Length - 1]) == 3)
                lsstr[lsstr.Length - 1] = '\0';
            return new string(lsstr);
        }

        public void lrcv_TextChanged(object sender, EventArgs e)
        {
            Gib.infile = lrcv.Text;
            if (Gib.infile != Gib.backup)
                txtstate.Text = "已修改";
            else
                txtstate.Text = "未修改";
            lwsum.Text = Gib.infile.Length.ToString();
        }

        public void lrcv_MouseUp(object sender, MouseEventArgs e)
        {
            if (usedic.Checked == true && lrcv.SelectedText.Trim() != "")
            {
                Gib.selecttext = strtrm(lrcv.SelectedText.Trim());
                Point ppoint = Control.MousePosition;
                Point fpoint = this.PointToClient(Control.MousePosition);
                if (fpoint.X > this.Size.Width - 433)
                    Gib.mx = ppoint.X - 441;
                else
                    Gib.mx = ppoint.X + 8;
                if (fpoint.Y > this.Size.Height - 165)
                    Gib.my = ppoint.Y - 172;
                else
                    Gib.my = ppoint.Y + 8;
                FrmDic frm = new FrmDic();
                frm.Show();
                //frm.Hide();433, 165
            }
        }

        private void lrcv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
                this.lrcv.SelectAll();
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                if (Gib.nowp[Gib.nowtab] > 0)
                {
                    if (filepath.Text + '\\' + filename.Text != "")
                    {
                        if (Gib.last == ".txt" || Gib.last == ".TXT")
                            File.WriteAllText(filepath.Text + '\\' + filename.Text, Gib.infile, Encoding.GetEncoding("GB2312"));
                        else
                            File.WriteAllText(filepath.Text + '\\' + filename.Text, Gib.infile);
                        Gib.backup = Gib.infile;
                        txtstate.Text = "未修改";
                    }
                }
            }
            else if (e.Control == true && e.KeyCode == Keys.D)
            {
                if (MessageBox.Show("确定删除文件？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    delfd(Gib.txtpath);
                    reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
                }
            }
        }

        #endregion

        #region FrmMain Things

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show(Gib.infile);
            //MessageBox.Show(Gib.backup);
            if (Gib.backup != Gib.infile)
            {
                if (DialogResult.OK == MessageBox.Show("文件未保存，确定要不保存文件并关闭CW File Viewer吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    this.FormClosing -= new FormClosingEventHandler(this.FrmMain_FormClosing);//为保证Application.Exit();时不再弹出提示，所以将FormClosing事件取消
                    Application.Exit();//退出整个应用程序
                }
                else
                {
                    e.Cancel = true;//取消关闭事件
                }
            }
            else
            {
                this.FormClosing -= new FormClosingEventHandler(this.FrmMain_FormClosing);//为保证Application.Exit();时不再弹出提示，所以将FormClosing事件取消
                Application.Exit();//退出整个应用程序
            }
        }
        public void FrmMain_Load(object sender, EventArgs e)
        {
            Gib.hissum = 0;
            Gib.infile = "";
            Gib.copypath = "Null";
            Gib.cuts = ".!?;:。！？";
            Gib.cuta = " - cut";
            Gib.nowp[0] = Gib.nowp[1] = Gib.nowp[2] = Gib.nowp[3] = Gib.nowp[4] = 0;
            wtsize.Text = Gib.wsize.ToString();
            Gib.cutc = Gib.cuts.ToCharArray();
            /*
            panelfile.BackColor = Color.White;
            panelplayer.BackColor = Color.White;
            */
            txtstate.ForeColor = Color.White;
            lwsum.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            label4.ForeColor = Color.White;
            nowdo.ForeColor = Color.White;
            labelX2.BackColor = SystemColors.ButtonFace;
            paneltxt.BackColor = SystemColors.ButtonFace;
            for (int i = 1; i <= 3; ++i)
                for (int j = 0; j < 3; ++j)
                    Gib.fpos[i, j] = "";
            line1.ForeColor = Color.DarkGray;
            line2.ForeColor = Color.DarkGray;
            line3.ForeColor = Color.DarkGray;
            line4.ForeColor = Color.DarkGray;
            todirc.Enabled = Directory.Exists("C:\\");
            todird.Enabled = Directory.Exists("D:\\");
            todire.Enabled = Directory.Exists("E:\\");
            todirf.Enabled = Directory.Exists("F:\\");
            Gib.firsto = true;
            

            if (File.Exists("C:\\ProgramData\\CW Soft\\CW File Viewer\\Temp.txt") == false)
            {
                //if (Directory.Exists("C:\\ProgramData\\CW Soft") == false)
                //    Directory.CreateDirectory("C:\\ProgramData\\CW Soft");
                if (Directory.Exists("C:\\ProgramData\\CW Soft\\CW File Viewer") == false)
                    Directory.CreateDirectory("C:\\ProgramData\\CW Soft\\CW File Viewer");
                File.Create("C:\\ProgramData\\CW Soft\\CW File Viewer\\Temp.txt");
            }
            if (Directory.Exists("C:\\ProgramData\\CW Soft\\CW File Viewer\\Dic") == false)
                Directory.CreateDirectory("C:\\ProgramData\\CW Soft\\CW File Viewer\\Dic");
            if (Directory.Exists("C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin") == false)
                Directory.CreateDirectory("C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin");
            //this.lrcv.Text = System.IO.File.ReadAllText("C:\\Users\\david\\Desktop\\superbaga.lrc");
        }

        #endregion

        public void filename_TextChanged(object sender, EventArgs e)
        {
            Gib.nfname = filename.Text;
        }

        public void player_StatusChange(object sender, EventArgs e)
        {
            if (repeatplay.Checked == true && player.playState == WMPLib.WMPPlayState.wmppsStopped)
                player.Ctlcontrols.play();
        }

        private void onlyfolder_CheckedChanged(object sender, EventArgs e)
        {
            if (Gib.nowp[Gib.nowtab] > 0)
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
        }

        #region ToDir
        private void todirc_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("C:\\"))
            {
                Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = "C:\\";
                Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }

        private void todird_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("D:\\"))
            {
                Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = "D:\\";
                Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }

        private void todire_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("E:\\"))
            {
                Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = "E:\\";
                Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }

        private void todirf_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("F:\\"))
            {
                Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = "F:\\";
                Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }
        #endregion

        #region filepath
        private void filepath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = filepath.Text;
                ++Gib.maxn[Gib.nowtab];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }
        #endregion

        #region Font Size Control

        private void fssmall_Click(object sender, EventArgs e)
        {
            lrcv.Font = new Font(lrcv.Font.Name, 12);
            wtsize.Value = 12;
        }
        private void fsbig_Click(object sender, EventArgs e)
        {
            lrcv.Font = new Font(lrcv.Font.Name, 22);
            wtsize.Value = 22;
        }
        private void wtsize_ValueChanged(object sender, EventArgs e)
        {
            lrcv.Font = new Font(lrcv.Font.Name, wtsize.Value);
        }

        #endregion

        #region FILEV

        #region filev_SelectedIndexChanged


        private void filev1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.filev1.SelectedItems.Count > 0)
            {
                Gib.fname = this.filev1.SelectedItems[0].Text;
                Gib.lpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] = this.filev1.SelectedItems[0].Index;
                loadfile();
            }
        }
        private void filev1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isleftkey = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                isleftkey = false;
            }
        }
        private void filev2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.filev2.SelectedItems.Count > 0)
            {
                Gib.fname = this.filev2.SelectedItems[0].Text;
                Gib.lpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] = this.filev2.SelectedItems[0].Index;
                loadfile();
            }
        }
        private void filev2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isleftkey = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                isleftkey = false;
            }
        }
        private void filev3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.filev3.SelectedItems.Count > 0)
            {
                Gib.fname = this.filev3.SelectedItems[0].Text;
                Gib.lpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] = this.filev3.SelectedItems[0].Index;
                loadfile();
            }
        }
        private void filev3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isleftkey = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                isleftkey = false;
            }
        }
        #endregion

        #region filev_ItemMouseHover
        private void filev1_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)//ListViewItemMouseHoverEventArgs e)
        {
            if (useprev.Checked == true)
            {
                //Point ppoint = this.PointToClient(Control.MousePosition);
                Gib.pname = e.Item.Text;

                Gib.ppath = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Gib.pname;
                Gib.plast = Path.GetExtension(Gib.ppath);
                //MessageBox.Show(Gib.pname + '\n' + Gib.ppath + '\n' + Gib.plast);
                if (Gib.plast == ".txt" || Gib.plast == ".TXT" || Gib.plast == ".lrc" || Gib.plast == ".LRC")
                {
                    if (Gib.ptext != Gib.pname)
                    {
                        Gib.ptext = Gib.pname;
                        Point ppoint = Control.MousePosition;
                        Gib.mx = ppoint.X + 2;
                        Gib.my = ppoint.Y + 2;
                        FrmPtxt fprev = new FrmPtxt();
                        fprev.Show(this);
                        fprev.Hide();
                    }
                    else
                    {
                        Gib.ptext = Gib.pname;
                    }
                }
                else if (Gib.plast == ".png" || Gib.plast == ".PNG" || Gib.plast == ".jpg" || Gib.plast == ".JPG" || Gib.plast == ".bmp" || Gib.plast == ".BMP" || Gib.plast == ".ico" || Gib.plast == ".ICO")
                {
                    if (Gib.ptext != Gib.pname)
                    {
                        Gib.ptext = Gib.pname;
                        Point ppoint = Control.MousePosition;
                        Gib.mx = ppoint.X + 2;
                        Gib.my = ppoint.Y + 2;
                        Bitmap image1 = new Bitmap(Gib.ppath);
                        FrmPpic.PictureWidth = image1.Width.ToString();
                        FrmPpic.Pictureheight = image1.Height.ToString();
                        FileInfo finfo = new FileInfo(Gib.ppath);
                        FrmPpic fprev = new FrmPpic();
                        fprev.Show(this);
                        fprev.Hide();
                    }
                    else
                    {
                        Gib.ptext = Gib.pname;
                    }
                    //Gib.ptext = Gib.pname;
                }
            }
        }
        private void filev2_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)//ListViewItemMouseHoverEventArgs e)
        {
            if (useprev.Checked == true)
            {
                //Point ppoint = this.PointToClient(Control.MousePosition);
                Gib.pname = e.Item.Text;

                Gib.ppath = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Gib.pname;
                Gib.plast = Path.GetExtension(Gib.ppath);
                //MessageBox.Show(Gib.pname + '\n' + Gib.ppath + '\n' + Gib.plast);
                if (Gib.plast == ".txt" || Gib.plast == ".TXT" || Gib.plast == ".lrc" || Gib.plast == ".LRC")
                {
                    if (Gib.ptext != Gib.pname)
                    {
                        Gib.ptext = Gib.pname;
                        Point ppoint = Control.MousePosition;
                        Gib.mx = ppoint.X + 2;
                        Gib.my = ppoint.Y + 2;
                        FrmPtxt fprev = new FrmPtxt();
                        fprev.Show(this);
                        fprev.Hide();
                    }
                    else
                    {
                        Gib.ptext = Gib.pname;
                    }
                }
                else if (Gib.plast == ".png" || Gib.plast == ".PNG" || Gib.plast == ".jpg" || Gib.plast == ".JPG" || Gib.plast == ".bmp" || Gib.plast == ".BMP" || Gib.plast == ".ico" || Gib.plast == ".ICO")
                {
                    if (Gib.ptext != Gib.pname)
                    {
                        Gib.ptext = Gib.pname;
                        Point ppoint = Control.MousePosition;
                        Gib.mx = ppoint.X + 2;
                        Gib.my = ppoint.Y + 2;
                        Bitmap image1 = new Bitmap(Gib.ppath);
                        FrmPpic.PictureWidth = image1.Width.ToString();
                        FrmPpic.Pictureheight = image1.Height.ToString();
                        FileInfo finfo = new FileInfo(Gib.ppath);
                        FrmPpic fprev = new FrmPpic();
                        fprev.Show(this);
                        fprev.Hide();
                    }
                    else
                    {
                        Gib.ptext = Gib.pname;
                    }
                    //Gib.ptext = Gib.pname;
                }
            }
        }
        private void filev3_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)//ListViewItemMouseHoverEventArgs e)
        {
            if (useprev.Checked == true)
            {
                //Point ppoint = this.PointToClient(Control.MousePosition);
                Gib.pname = e.Item.Text;

                Gib.ppath = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] + '\\' + Gib.pname;
                Gib.plast = Path.GetExtension(Gib.ppath);
                //MessageBox.Show(Gib.pname + '\n' + Gib.ppath + '\n' + Gib.plast);
                if (Gib.plast == ".txt" || Gib.plast == ".TXT" || Gib.plast == ".lrc" || Gib.plast == ".LRC")
                {
                    if (Gib.ptext != Gib.pname)
                    {
                        Gib.ptext = Gib.pname;
                        Point ppoint = Control.MousePosition;
                        Gib.mx = ppoint.X + 2;
                        Gib.my = ppoint.Y + 2;
                        FrmPtxt fprev = new FrmPtxt();
                        fprev.Show(this);
                        fprev.Hide();
                    }
                    else
                    {
                        Gib.ptext = Gib.pname;
                    }
                }
                else if (Gib.plast == ".png" || Gib.plast == ".PNG" || Gib.plast == ".jpg" || Gib.plast == ".JPG" || Gib.plast == ".bmp" || Gib.plast == ".BMP" || Gib.plast == ".ico" || Gib.plast == ".ICO")
                {
                    if (Gib.ptext != Gib.pname)
                    {
                        Gib.ptext = Gib.pname;
                        Point ppoint = Control.MousePosition;
                        Gib.mx = ppoint.X + 2;
                        Gib.my = ppoint.Y + 2;
                        Bitmap image1 = new Bitmap(Gib.ppath);
                        FrmPpic.PictureWidth = image1.Width.ToString();
                        FrmPpic.Pictureheight = image1.Height.ToString();
                        FileInfo finfo = new FileInfo(Gib.ppath);
                        FrmPpic fprev = new FrmPpic();
                        fprev.Show(this);
                        fprev.Hide();
                    }
                    else
                    {
                        Gib.ptext = Gib.pname;
                    }
                    //Gib.ptext = Gib.pname;
                }
            }
        }
        #endregion

        #region filev_KeyDown
        private void filev1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.D)
                if (MessageBox.Show("确定删除文件？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    delfd(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);

        }
        private void filev2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.D)
                if (MessageBox.Show("确定删除文件？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    delfd(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);

        }
        private void filev3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.D)
                if (MessageBox.Show("确定删除文件？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    delfd(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1]);
        }
        #endregion

        #endregion

        #region TABV

        private void tabv_Click(object sender, EventArgs e)
        {
        }
        private void tab1_Click(object sender, EventArgs e)
        {
            Gib.nowtab = 1;
            filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
        }
        private void tab2_Click(object sender, EventArgs e)
        {
            Gib.nowtab = 2;
            filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
        }
        private void tab3_Click(object sender, EventArgs e)
        {
            Gib.nowtab = 3;
            filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
        }

        #endregion

        private void nowdoti_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("\""+player.status+"\"");
            if (looklrc.Checked == true)
            {
                if (player.status == "")
                    nowdo.Text = "未选择路径";
                else
                    nowdo.Text = player.status;
            }
            else
            {
                nowdo.Text = "(如想查看字幕和状态请选择“显示字幕+播放状态”）";
            }
        }

        #region Right Button

        private void rbdel_Click(object sender, EventArgs e)
        {
            delfd(Gib.rbfile);
        }

        void copyfile(string from)
        {
            if (File.Exists(from))
            {
                Gib.cpmode = 1;
                Gib.copypath = from;
                Gib.copyfn = Path.GetFileName(from);
            }
            else if (Directory.Exists(from))
            {
                Gib.cpmode = 2;
                Gib.copypath = from;
                Gib.copyfn = Path.GetFileName(from);
            }
            else
                MessageBox.Show("路径不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void rbcut_Click(object sender, EventArgs e)
        {
            Gib.cpiscut = true;
            copyfile(Gib.rbfile);
        }

        private void rbcopy_Click(object sender, EventArgs e)
        {
            Gib.cpiscut = false;
            copyfile(Gib.rbfile);
        }
        private static void CopyFolder(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            // 文件
            foreach (string file in Directory.GetFiles(from))
                File.Copy(file, to + '\\' + Path.GetFileName(file), true);

            // 子文件夹
            foreach (string folder in Directory.GetDirectories(from))
                CopyFolder(folder, to +'\\'+ Path.GetFileName(folder));
        }
        void pastfile(string from, string to, string toname,int cpmode)
        {
            if (File.Exists(from) == false && Directory.Exists(from) == false)
            {
                MessageBox.Show("未选择路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cpmode == 1)
            {
                if (File.Exists(to + '\\' + toname))
                {
                    if (MessageBox.Show("文件已存在，是否替换？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        File.Copy(from, to + '\\' + toname, true);
                }
                else
                    File.Copy(from, to + '\\' + toname, true);
            }
            else
            {
                if (Directory.Exists(to + '\\' + toname))
                {
                    MessageBox.Show("文件夹已存在!");
                }
                else
                    CopyFolder(from, to + '\\' + toname);
            }
            reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
        }
        private void rbpast_Click(object sender, EventArgs e)
        {
            pastfile(Gib.copypath, Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.copyfn,Gib.cpmode);
            if (Gib.cpiscut == true)
                delfd(Gib.rbfile);
        }

        private void rbinfo_Click(object sender, EventArgs e)
        {
            FrmInfo finfo = new FrmInfo();
            finfo.Show(this);
        }
        #endregion

        private void spliter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            tabv.FixedTabSize = new Size((tabv.Size.Width - 40) / 3, 0);
        }

        private void add_Click(object sender, EventArgs e)
        {
            FrmAddWord frm = new FrmAddWord();
            frm.Show();
        }

        private void godestop_Click(object sender, EventArgs e)
        {
            Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
            reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
        }

        private void givetimefirst_Tick(object sender, EventArgs e)
        {
            int nowt = DateTime.Now.Hour;
            if (nowt < 12)
                ToastNotification.Show(this, "上午好，现在时间：" + DateTime.Now.ToString("hh:mm:ss  yyyy/MM/dd  ") + DateTime.Now.DayOfWeek.ToString(), 10000);
            else if (nowt == 12)
                ToastNotification.Show(this, "中午好，现在时间：" + DateTime.Now.ToString("hh:mm:ss  yyyy/MM/dd  ") + DateTime.Now.DayOfWeek.ToString(), 10000);
            else if (nowt > 12 && nowt < 18)
                ToastNotification.Show(this, "下午好，现在时间：" + DateTime.Now.ToString("hh:mm:ss  yyyy/MM/dd  ") + DateTime.Now.DayOfWeek.ToString(), 10000);
            else
                ToastNotification.Show(this, "晚上好，现在时间：" + DateTime.Now.ToString("hh:mm:ss  yyyy/MM/dd  ") + DateTime.Now.DayOfWeek.ToString(), 10000);
            givetimefirst.Enabled = false;
        }

        private void timegiver_Tick(object sender, EventArgs e)
        {
            int nowt = DateTime.Now.Minute;
            if ((nowt == 30 || nowt == 0) && DateTime.Now.Second < 5)
                ToastNotification.Show(this, "现在时间：" + DateTime.Now.ToString("hh:mm:ss  yyyy/MM/dd  ") + DateTime.Now.DayOfWeek.ToString(), 10000);
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void seevery_CheckedChanged(object sender, EventArgs e)
        {
            reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
        }

        private void rogbin_Click(object sender, EventArgs e)
        {
            Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = "C:\\ProgramData\\CW Soft\\CW File Viewer\\RecycleBin";
            Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
            reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
        }

        private void volumeslider_ValueChanged(object sender, EventArgs e)
        {
            player.settings.volume = volumeslider.Value;
        }

        private void playbutton_Click(object sender, EventArgs e)
        {
            if (Gib.pstate == 1)
            {
                Gib.pstate = 2;
                player.Ctlcontrols.pause();
                playbutton.Image = Properties.Resources.state11;
                playbutton.HoverImage = Properties.Resources.state12;
                playbutton.PressedImage = Properties.Resources.state13;
            }
            else
            {
                Gib.pstate = 1;
                player.Ctlcontrols.play();
                playbutton.Image = Properties.Resources.state21;
                playbutton.HoverImage = Properties.Resources.state22;
                playbutton.PressedImage = Properties.Resources.state23;
            }
        }

        private void leftpanel_SizeChanged(object sender, EventArgs e)
        {
            tabv.FixedTabSize = new Size((panelfile.Size.Width - 39) / 3, 0);
        }

        private void goodsort_CheckedChanged(object sender, EventArgs e)
        {
            reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
        }

        private void lastdir_Click(object sender, EventArgs e)
        {
            if(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]].Length>3)
            {
                Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab] + 1] = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                ++Gib.nowp[Gib.nowtab];
                Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]].Remove(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]].LastIndexOf('\\'), 1);
                Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]] = Path.GetDirectoryName(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]]);
                Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab] ? Gib.maxn[Gib.nowtab] : Gib.nowp[Gib.nowtab];
                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
        }

        private void fpcrumb_SelectedItemChanging(object sender, CrumbBarSelectionEventArgs e)
        {
            //MessageBox.Show(Gib.nowcrst.ToString());
            //ToastNotification.Show(this, Gib.nowcrst.ToString());
            if (Gib.nowcrst == 0)
            {
                //ToastNotification.Show(this, "fpcrumb_SelectedItemChanging" + e.ToString(), 5000);
                //MessageBox.Show("Test1");
                // Load folders are selection changes...
                CrumbBarItem parent = e.NewSelectedItem;
                //if (parent == null || parent.SubItems.Count > 0){
                //    MessageBox.Show("out"); return;}
                for(i=0;i<26;++i)
                    if (parent.Text == ((char)( i + 'A')).ToString()+":\\")
                    {
                        Gib.nowcrst = 1;
                        parent.SubItems.Clear();
                        break;
                    }

                DirectoryInfo dirInfo = null;
                if (parent.Tag is DriveInfo)
                {
                    DriveInfo driveInfo = (DriveInfo) parent.Tag;
                    dirInfo = driveInfo.RootDirectory;
                }
                else if (parent.Tag is DirectoryInfo)
                {
                    dirInfo = (DirectoryInfo) parent.Tag;
                }
                else
                {
                    return;
                }
                DirectoryInfo[] subDirectories = dirInfo.GetDirectories();
                foreach (DirectoryInfo directoryInfo in subDirectories)
                {
                    CrumbBarItem node = new CrumbBarItem();
                    node.Tag = directoryInfo;
                    node.Text = directoryInfo.Name;
                    node.Image = Properties.Resources.folderp;
                    parent.SubItems.Add(node);
                }
                Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = dirInfo.FullName;
                Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab]
                    ? Gib.maxn[Gib.nowtab]
                    : Gib.nowp[Gib.nowtab];
                filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
                reffile(Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]], Gib.smode);
            }
            else
                Gib.nowcrst = 0;
        }

        /*
        //ToastNotification.Show(this, "fpcrumb_SelectedItemChanging" + e.ToString(), 5000);
        //MessageBox.Show("Test1");
        // Load folders are selection changes...
        CrumbBarItem parent = e.NewSelectedItem;
        //if (parent == null || parent.SubItems.Count > 0){
        //    MessageBox.Show("out"); return;}
        /*
        for(int i=0;i<26;++i)
            if (parent.Text == ((char) ('A' + i)).ToString() + ":\\")
            {
                parent.SubItems.Clear();
                break;
            }
        //if (parent.Text =="C:\\")
        //    parent.SubItems.Clear();
        DirectoryInfo dirInfo = null;
        if (parent.Tag is DriveInfo)
        {
            DriveInfo driveInfo = (DriveInfo) parent.Tag;
            dirInfo = driveInfo.RootDirectory;
        }
        else if (parent.Tag is DirectoryInfo)
        {
            dirInfo = (DirectoryInfo) parent.Tag;
        }
        else
        {
            return;
        }
        bool bj = true;
        for (int i = 0; i < 26; ++i)
            if (parent.Text == ((char) ('A' + i)).ToString() + ":\\")
            {
                bj = false;
                break;
            }
        if (bj == true)
        {
            DirectoryInfo[] subDirectories = dirInfo.GetDirectories();
            foreach (DirectoryInfo directoryInfo in subDirectories)
            {
                CrumbBarItem node = new CrumbBarItem();
                node.Tag = directoryInfo;
                node.Text = directoryInfo.Name;
                node.Image = Properties.Resources.folderp;
                parent.SubItems.Add(node);
            }
            Gib.fpos[Gib.nowtab, ++Gib.nowp[Gib.nowtab]] = dirInfo.FullName;
            Gib.maxn[Gib.nowtab] = Gib.maxn[Gib.nowtab] > Gib.nowp[Gib.nowtab]
                ? Gib.maxn[Gib.nowtab]
                : Gib.nowp[Gib.nowtab];
            filepath.Text = Gib.fpos[Gib.nowtab, Gib.nowp[Gib.nowtab]];
        }
        else
            */
        private void fpcrumb_SelectedItemChanged(object sender, CrumbBarSelectionEventArgs e)
        {

        }

        private void fpcrumb_DoubleClick(object sender, EventArgs e)
        {
            fpcrumb.Visible = false;
        }

        private void filepath_MouseLeave(object sender, EventArgs e)
        {
            fpcrumb.Visible = true;
        }
    }
}
/*
 
 public static Icon GetIcon(string fileName, bool isLargeIcon)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            IntPtr hI;
            if (isLargeIcon)
                hI = SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), (uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_USEFILEATTRIBUTES | (uint)FileInfoFlags.SHGFI_LARGEICON);
            else
                hI = SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), (uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_USEFILEATTRIBUTES | (uint)FileInfoFlags.SHGFI_SMALLICON);
            Icon icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;
            DestroyIcon(shfi.hIcon); //释放资源
            return icon;
        }
        /// <summary>  
        /// 获取文件夹图标
        /// </summary>  
        /// <returns>图标</returns>  
        public static Icon GetDirectoryIcon(bool isLargeIcon)
        {
            SHFILEINFO _SHFILEINFO = new SHFILEINFO();
            IntPtr _IconIntPtr;
            if (isLargeIcon)
            {
                _IconIntPtr = SHGetFileInfo(@"", 0, ref _SHFILEINFO, (uint)Marshal.SizeOf(_SHFILEINFO), ((uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_LARGEICON));
            }
            else
            {
                _IconIntPtr = SHGetFileInfo(@"", 0, ref _SHFILEINFO, (uint)Marshal.SizeOf(_SHFILEINFO), ((uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_SMALLICON));
            }
            if (_IconIntPtr.Equals(IntPtr.Zero)) return null;
            Icon _Icon = System.Drawing.Icon.FromHandle(_SHFILEINFO.hIcon);
            return _Icon;
        }
 char[] cs = yourstring.ToCharArray();
StreamReader sr = new StreamReader("TestFile.txt")//StreamReader sr = new StreamReader("TestFile.txt",Encoding.GetEncoding("GB2312"))
///GBK
String line;
while ((line = sr.ReadLine()) != null)
{
   textBox1 .Text +=ii.ToString ()+" -"+line.ToString()+"\r\n";

}
加入引用：System.IO
StreamReader objReader = new StreamReader("c:\\test.txt");
     System.IO 命名空间中的对象，尤其是 System.IO.StreamReader 类。

\r\n一般一起用,用来表示键盘上的回车键.也可只用\n.\t表示键盘上的“TAB
*/