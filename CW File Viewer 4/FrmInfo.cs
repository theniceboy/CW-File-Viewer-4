using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;

namespace CW_File_Viewer_4
{
    public partial class FrmInfo : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FrmInfo()
        {
            InitializeComponent();
        }

        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfo")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribute, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint Flags);
        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
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
        public static Image GetNewImage(Image oldImg, int newWidth, int newHeight)
        {
            Image newImg = oldImg.GetThumbnailImage(newWidth, newHeight, new Image.GetThumbnailImageAbort(IsTrue), IntPtr.Zero); // 对原图片进行缩放
            return newImg;
        }
        public static bool IsTrue() // 在Image类别对图片进行缩放的时候,需要一个返回bool类别的委托
        {
            return true;
        }

        #region 枚举属性预定义,该内容可在shellapi.h中找到

        public enum FileAttributeFlags : int
        {
            FILE_ATTRIBUTE_READONLY = 0x00000001,
            FILE_ATTRIBUTE_HIDDEN = 0x00000002,
            FILE_ATTRIBUTE_SYSTEM = 0x00000004,
            FILE_ATTRIBUTE_DIRECTORY = 0x00000010,
            FILE_ATTRIBUTE_ARCHIVE = 0x00000020,
            FILE_ATTRIBUTE_DEVICE = 0x00000040,
            FILE_ATTRIBUTE_NORMAL = 0x00000080,
            FILE_ATTRIBUTE_TEMPORARY = 0x00000100,
            FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,
            FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,
            FILE_ATTRIBUTE_COMPRESSED = 0x00000800,
            FILE_ATTRIBUTE_OFFLINE = 0x00001000,
            FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,
            FILE_ATTRIBUTE_ENCRYPTED = 0x00004000
        }

        public enum GetFileInfoFlags : int
        {
            SHGFI_ICON = 0x000000100,     // get icon
            SHGFI_DISPLAYNAME = 0x000000200,     // get display name
            SHGFI_TYPENAME = 0x000000400,     // get type name
            SHGFI_ATTRIBUTES = 0x000000800,     // get attributes
            SHGFI_ICONLOCATION = 0x000001000,     // get icon location
            SHGFI_EXETYPE = 0x000002000,     // return exe type
            SHGFI_SYSICONINDEX = 0x000004000,     // get system icon index
            SHGFI_LINKOVERLAY = 0x000008000,     // put a link overlay on icon
            SHGFI_SELECTED = 0x000010000,     // show icon in selected state
            SHGFI_ATTR_SPECIFIED = 0x000020000,     // get only specified attributes
            SHGFI_LARGEICON = 0x000000000,     // get large icon
            SHGFI_SMALLICON = 0x000000001,     // get small icon
            SHGFI_OPENICON = 0x000000002,     // get open icon
            SHGFI_SHELLICONSIZE = 0x000000004,     // get shell size icon
            SHGFI_PIDL = 0x000000008,     // pszPath is a pidl
            SHGFI_USEFILEATTRIBUTES = 0x000000010,     // use passed dwFileAttribute
            SHGFI_ADDOVERLAYS = 0x000000020,     // apply the appropriate overlays
            SHGFI_OVERLAYINDEX = 0x000000040      // Get the index of the overlay
        }

        #endregion

        public static string GetTypeName(string strPath)
        {
            SHFILEINFO fileInfo = new SHFILEINFO();
            int cbFileInfo = Marshal.SizeOf(fileInfo);

            SHGetFileInfo(strPath, (uint)FileAttributeFlags.FILE_ATTRIBUTE_NORMAL,
                ref fileInfo, (uint)cbFileInfo, (uint)(GetFileInfoFlags.SHGFI_DISPLAYNAME | GetFileInfoFlags.SHGFI_TYPENAME));

            return fileInfo.szTypeName;
        }
        long foldersize(string fpath)
        {
            long s = 0;

            // 文件
            foreach (string file in Directory.GetFiles(fpath))
            {
                FileInfo fi = new FileInfo(file);
                s += fi.Length;
            }

            // 子文件夹
            foreach (string folder in Directory.GetDirectories(fpath))
                s += foldersize(folder + '\\' + Path.GetFileName(fpath));

            return s;
        }

        private void FrmInfo_Load(object sender, EventArgs e)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            SHGetFileInfo(Gib.rbfile, (uint)0x80, ref shfi, 16640, (uint)(0x100 | 0x400)); //获取文件的图标及类型
            Icon ico = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
            Image img = Image.FromHbitmap(ico.ToBitmap().GetHbitmap());
            filepic.Image = GetNewImage(img, 45, 45);

            filename.Text = Path.GetFileName(Gib.rbfile);

            filetype.Text = GetTypeName(Gib.rbfile);

            filepath.Text = Gib.rbfile;

            if (File.Exists(Gib.rbfile))
            {
                FileInfo fi = new FileInfo(Gib.rbfile);
                filesize.Text = fi.Length.ToString() + " 字节";

                filecreatdate.Text = File.GetCreationTime(Gib.rbfile).ToString();

                filechangedate.Text = File.GetLastWriteTime(Gib.rbfile).ToString();
            }
            else
            {
                filesize.Text = foldersize(Gib.rbfile).ToString() + " 字节";

                filecreatdate.Text = Directory.GetCreationTime(Gib.rbfile).ToString();

                filechangedate.Text = Directory.GetLastWriteTime(Gib.rbfile).ToString();
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void filepic_Click(object sender, EventArgs e)
        {

        }

        private void filechangedate_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelX5_Click(object sender, EventArgs e)
        {

        }

        private void labelX4_Click(object sender, EventArgs e)
        {

        }

        private void labelX1_Click(object sender, EventArgs e)
        {

        }

        private void filecreatdate_TextChanged(object sender, EventArgs e)
        {

        }

        private void filepath_TextChanged(object sender, EventArgs e)
        {

        }

        private void filesize_TextChanged(object sender, EventArgs e)
        {

        }

        private void filetype_TextChanged(object sender, EventArgs e)
        {

        }

        private void filename_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelX3_Click(object sender, EventArgs e)
        {

        }

        private void labelX2_Click(object sender, EventArgs e)
        {

        }
    }
}