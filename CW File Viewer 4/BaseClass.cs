/*************************************

��Ȩ����: 	����ʡ���տƼ����޹�˾

������ڣ�	2010-09-15

��Ŀ������	��̴ʵ�

����������	www.mingribook.com

ѧϰ������	www.mrbccd.com

*************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Collections;
/*
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
*/

namespace CW_File_Viewer_4
{
    class BaseClass
    {
        #region ���幫������
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; //��ͼ��
        public const uint SHGFI_SMALLICON = 0x1; //Сͼ��
        public static string AllPath = "";//��¼ѡ��·��
        #endregion

        #region ����ļ�ͼ����
        /// <summary>
        /// ����ļ�ͼ����
        /// </summary>
        /// <param name="pszPath">ָ�����ļ���</param>
        /// <param name="dwFileAttribute">�ļ�����</param>
        /// <param name="psfi">��¼���ͣ����ػ�õ��ļ���Ϣ</param>
        /// <param name="cbSizeFileInfo">psfi�ı���ֵ</param>
        /// <param name="Flags">ָ����Ҫ���ص��ļ���Ϣ��ʶ��</param>
        /// <returns>�ļ���ͼ����</returns>
        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfo")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribute, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint Flags);
        #endregion

        #region ���ͼ��
        /// <summary>
        /// ���ͼ��
        /// </summary>
        /// <param name="hIcon">ͼ����</param>
        /// <returns>�����ʾ�ɹ������ʾʧ��</returns>
        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);
        #endregion

        #region ͼ��ṹ
        /// <summary>
        /// ͼ��ṹ
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;//�ļ���ͼ����
            public IntPtr iIcon;//ͼ���ϵͳ������
            public uint dwAttributes;//�ļ�������ֵ
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;//�ļ�����ʾ��
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;//�ļ���������
        }
        #endregion

        #region ��ȡָ��·���������ļ�����ͼ��
        /// <summary>
        /// ��ȡָ��·���������ļ�����ͼ��
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="imglist">ImageList����</param>
        /// <param name="lv">ListView����</param>
        public void GetListViewItem(string path, ImageList imglist, ListView lv)
        {
            lv.Items.Clear();
            SHFILEINFO shfi = new SHFILEINFO();
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);
                for (int i = 0; i < dirs.Length; i++)
                {
                    string[] info = new string[4];
                    DirectoryInfo dir = new DirectoryInfo(dirs[i]);
                    if (dir.Name == "RECYCLER" || dir.Name == "RECYCLED" || dir.Name == "Recycled" || dir.Name == "System Volume Information")
                    { }
                    else
                    {
                        //���ͼ��
                        SHGetFileInfo(dirs[i],
                                            (uint)0x80,
                                            ref shfi,
                                            (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                                            (uint)(0x100 | 0x400));//ȡ��Icon��TypeName
                        //���ͼ��
                        imglist.Images.Add(dir.Name, (Icon)Icon.FromHandle(shfi.hIcon).Clone());
                        info[0] = dir.Name;
                        info[1] = "";
                        info[2] = "�ļ���";
                        info[3] = dir.LastWriteTime.ToString();
                        ListViewItem item = new ListViewItem(info, dir.Name);
                        lv.Items.Add(item);
                        //����ͼ��
                        DestroyIcon(shfi.hIcon);
                    }
                }
                for (int i = 0; i < files.Length; i++)
                {
                    string[] info = new string[5];
                    FileInfo fi = new FileInfo(files[i]);
                    string Filetype = fi.Name.Substring(fi.Name.LastIndexOf(".") + 1, fi.Name.Length - fi.Name.LastIndexOf(".") - 1);
                    string newtype = Filetype.ToLower();
                    if (newtype == "sys" || newtype == "ini" || newtype == "bin" || newtype == "log" || newtype == "com" || newtype == "bat" || newtype == "db")
                    { }
                    else
                    {
                        //���ͼ��
                        SHGetFileInfo(files[i],
                                            (uint)0x80,
                                            ref shfi,
                                            (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                                            (uint)(0x100 | 0x400)); //ȡ��Icon��TypeName
                        //���ͼ��
                        imglist.Images.Add(fi.Name, (Icon)Icon.FromHandle(shfi.hIcon).Clone());
                        info[0] = fi.Name;
                        double dbLength = fi.Length / 1024;
                        if (dbLength < 1024)
                            info[1] = dbLength.ToString("0.00") + " KB";
                        else
                            info[1] = Convert.ToDouble(dbLength / 1024).ToString("0.00") + " MB";
                        info[2] = fi.Extension.ToString();
                        info[3] = fi.LastWriteTime.ToString();
                        info[4] = fi.IsReadOnly.ToString();
                        ListViewItem item = new ListViewItem(info, fi.Name);
                        lv.Items.Add(item);
                        //����ͼ��
                        DestroyIcon(shfi.hIcon);
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region ��ָ��·�����µ��ļ����ļ�����ʾ��ListView�ؼ���
        /// <summary>
        /// ��ָ��·�����µ��ļ����ļ�����ʾ��ListView�ؼ���
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="imglist">ImageList�ؼ�����</param>
        /// <param name="lv">ListView�ؼ�����</param>
        /// <param name="ppath">��ʶҪִ�еĲ���</param>
        public void GetPath(string path, ImageList imglist, ListView lv, int intflag)
        {
            string pp = "";
            string uu = "";
            try
            {
                if (intflag == 0)
                {
                    if (AllPath != path)
                    {
                        lv.Items.Clear();
                        AllPath = path;
                        GetListViewItem(AllPath, imglist, lv);
                    }
                }
                else
                {
                    uu = AllPath + path;
                    if (Directory.Exists(uu))
                    {
                        AllPath = AllPath + path + "\\";
                        pp = AllPath.Substring(0, AllPath.Length - 1);
                        lv.Items.Clear();
                        GetListViewItem(pp, imglist, lv);
                    }
                    else
                    {
                        if (path.IndexOf("\\") == -1)//�ж������������·������ת��Ϊ����·�����ٴ�
                        {
                            uu = AllPath + path;
                            System.Diagnostics.Process.Start(uu);
                        }
                        else//�ж����������·������ֱ�Ӵ�
                            System.Diagnostics.Process.Start(path);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ������һ��Ŀ¼
        /// <summary>
        /// ������һ��Ŀ¼
        /// </summary>
        /// <param name="lv">��ʾ�ļ����ļ��е�ListView�ؼ�����</param>
        /// <param name="imagelist">��ʾ�ļ����ļ���ͼ���ImageList�ؼ�����</param>
        public void backPath(ListView lv, ImageList imagelist)
        {
            if (AllPath.Length != 3)//�ж��Ƿ񶥼�Ŀ¼
            {
                string NewPath = AllPath.Remove(AllPath.LastIndexOf("\\")).Remove(AllPath.Remove(AllPath.LastIndexOf("\\")).LastIndexOf("\\")) + "\\";
                lv.Items.Clear();
                GetListViewItem(NewPath, imagelist, lv);
                AllPath = NewPath;
            }
        }
        #endregion

        #region ��ȡListView�ؼ��е�ѡ����
        /// <summary>
        /// ��ȡListView�ؼ��е�ѡ����
        /// </summary>
        /// <param name="lv">ListView�ؼ�����</param>
        /// <returns>ArrayList�����б�</returns>
        public ArrayList getFiles(ListView lv)
        {
            ArrayList list = new ArrayList();
            foreach (object objFile in lv.SelectedItems)
            {
                string strFile = objFile.ToString();
                string strFileName = strFile.Substring(strFile.IndexOf("{") + 1, strFile.LastIndexOf("}") - strFile.IndexOf("{") - 1);
                list.Add(strFileName);
            }
            return list;
        }
        #endregion

        #region �½��ļ����ļ���
        /// <summary>
        /// �½��ļ����ļ���
        /// </summary>
        /// <param name="lv">��ʾ�ļ����ļ��е�ListView�ؼ�����</param>
        /// <param name="imagelist">��ʾ�ļ����ļ���ͼ���ImageList�ؼ�����</param>
        /// <param name="strName">Ҫ�½����ļ����ļ�����</param>
        /// <param name="intflag">��ʶ��ִ���½��ļ�����������ִ���½��ļ��в���</param>
        public void NewFile(ListView lv, ImageList imagelist, string strName, int intflag)
        {
            string strPath = AllPath + strName;
            if (intflag == 0)
            {
                File.Create(strPath);//�½��ļ�
            }
            else if (intflag == 1)
            {
                Directory.CreateDirectory(strPath);//�½��ļ���
            }
            GetListViewItem(AllPath, imagelist, lv);
        }
        #endregion

        #region ���ƻ�����ļ��������������ơ����У�
        /// <summary>
        /// ���ƻ�����ļ��������������ơ����У�
        /// </summary>
        /// <param name="lv">��ʾ�ļ���ListView�ؼ�����</param>
        /// <param name="imagelist">��ʾ�ļ�ͼ���ImageList�ؼ�����</param>
        /// <param name="list">ListView�ؼ��е�ѡ����</param>
        /// <param name="strPath">�ļ���ԭʼ·��</param>
        /// <param name="strNewPath">�ļ�����·��</param>
        /// <param name="intflag">��ʶ��ִ�и��Ʋ���������ִ�м��в���</param>
        public void CopyFile(ListView lv, ImageList imagelist, ArrayList list, string strPath,string strNewPath, int intflag,ToolStripProgressBar TSPBar)
        {
            try
            {
                TSPBar.Maximum = list.Count;
                foreach (object objFile in list)
                {
                    string strFile = objFile.ToString();
                    string strOldFile = strPath + strFile;
                    string strNewFile = strNewPath + strFile;
                    if (File.Exists(strOldFile))
                    {
                        if (!Directory.Exists(strNewPath))
                            Directory.CreateDirectory(strNewPath);
                        if (intflag == 0)
                        {
                            File.Copy(strOldFile, strNewFile, true);
                        }
                        else if (intflag == 1)
                        {
                            File.Move(strOldFile, strNewFile);
                        }
                    }
                    TSPBar.Value += 1;
                }
                GetListViewItem(AllPath, imagelist, lv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region  ���ƻ�����ļ��У������������ơ����У�
        /// <summary>
        /// ���ƻ�����ļ��У������������ơ����У�
        /// </summary>
        /// <param name="Ddir">Ҫ�����ļ��е���·��</param>
        /// <param name="Sdir">Ҫ�����ļ��е�ԭʼ·��</param>
        /// <param name="intflag">��ʶִ�и��Ʋ���������ִ�м��в���</param>
        public void CopyDir(string Ddir, string Sdir, int intflag)
        {
            DirectoryInfo dir = new DirectoryInfo(Sdir);
            string SbuDir = Ddir;
            try
            {
                if (!dir.Exists)//�ж���ָ���ļ����ļ����Ƿ����
                {
                    return;
                }
                DirectoryInfo dirD = dir as DirectoryInfo;//����������������ļ������˳�
                string UpDir = UpAndDown_Dir(Ddir);
                if (dirD == null)//�ж��ļ����Ƿ�Ϊ��
                {
                    Directory.CreateDirectory(UpDir + "\\" + dirD.Name);//���Ϊ�գ������ļ��в��˳�
                    return;
                }
                else
                {
                    Directory.CreateDirectory(UpDir + "\\" + dirD.Name);
                }
                SbuDir = UpDir + "\\" + dirD.Name + "\\";
                FileSystemInfo[] files = dirD.GetFileSystemInfos();//��ȡ�ļ����������ļ����ļ���
                //�Ե���FileSystemInfo�����ж�,������ļ�������еݹ����
                foreach (FileSystemInfo FSys in files)
                {
                    FileInfo file = FSys as FileInfo;
                    if (file != null)//������ļ��Ļ��������ļ��ĸ��Ʋ���
                    {
                        FileInfo SFInfo = new FileInfo(file.DirectoryName + "\\" + file.Name);//��ȡ�ļ����ڵ�ԭʼ·��
                        SFInfo.CopyTo(SbuDir + "\\" + file.Name, true);//���ļ����Ƶ�ָ����·����
                    }
                    else
                    {
                        string pp = FSys.Name;//��ȡ��ǰ���������ļ�������
                        CopyDir(SbuDir + FSys.ToString(), Sdir + "\\" + FSys.ToString(), intflag);//������ļ�������еݹ����
                    }
                }
                if (intflag == 1)
                    Directory.Delete(Sdir, true);
            }
            catch
            {
                MessageBox.Show("�ļ��и���ʧ�ܡ�");
            }
        }
        /// <summary>
        /// ������һ��Ŀ¼
        /// </summary>
        /// <param dir="string">Ŀ¼</param>
        /// <returns>����String����</returns>
        public string UpAndDown_Dir(string dir)
        {
            string Change_dir = "";
            Change_dir = Directory.GetParent(dir).FullName;
            return Change_dir;
        }
        #endregion

        #region �������ļ����ļ��У����������������ļ������÷���Ϊ���ط���
        /// <summary>
        /// �������ļ����ļ���
        /// </summary>
        /// <param name="lv">��ʾ�ļ����ļ��е�ListView�ؼ�����</param>
        /// <param name="imagelist">��ʾ�ļ����ļ���ͼ���ImageList�ؼ�����</param>
        /// <param name="strName">�ļ����ļ��е�ԭ����</param>
        /// <param name="strNewName">�ļ����ļ��е�������</param>
        public void RepeatFile(ListView lv, ImageList imagelist, string strName, string strNewName)
        {
            string strPath = AllPath + strName;
            string strNewPath = AllPath + strNewName;
            if (strNewName != null)
            {
                if (File.Exists(strPath))
                {
                    if (strPath != strNewPath)
                    {
                        File.Move(strPath, strNewPath);
                    }
                }
                if (Directory.Exists(strPath))
                {
                    DirectoryInfo dir = new DirectoryInfo(strPath);
                    dir.MoveTo(strNewPath);
                }
                lv.Items.Clear();
                GetListViewItem(AllPath, imagelist, lv);
            }
        }

        /// <summary>
        /// �����������ļ������ļ�ΪWordʱ���޸ı����е����֣�
        /// </summary>
        /// <param name="lbox">��ʾҪ�������ļ���ListBox�ؼ�����</param>
        /// <param name="intFlag">��ʶ����������������ǰ���չ��������</param>
        /// <param name="strExten">Ҫ����������չ��</param>
        /// <param name="PBar">��������ʾ</param>
        ///
        /*
        public void RepeatFile(ListBox lbox, int intFlag,string strExten,string strOldName,string strNewName,ProgressBar PBar)
        {
            FileInfo FInfo = null;
            PBar.Maximum = lbox.Items.Count;
            for (int i = 0; i < lbox.Items.Count; i++)
            {
                string strFile = lbox.Items[i].ToString();
                if (File.Exists(strFile))
                {
                    FInfo = new FileInfo(strFile);
                    string strPath = FInfo.DirectoryName;
                    string strFName = FInfo.Name;
                    string strFExtention = FInfo.Extension;
                    switch (intFlag)
                    {
                        case 0:
                            File.Move(strFile, strPath + "\\" + i.ToString().PadLeft(4, '0') + strFName);
                            break;
                        case 1:
                            File.Move(strFile, strPath + "\\" + strFName.Substring(0, strFName.LastIndexOf(".")) + i.ToString().PadLeft(4, '0') + strFExtention);
                            break;
                        case 2:
                            File.Move(strFile, strPath + "\\" + strFName.Substring(0, strFName.LastIndexOf(".")) + "." + strExten);
                            break;
                        case 3:
                            frmRepeat frmrepeat=new frmRepeat();
                            object strNewPath = strPath + "\\" + strFName.Replace(strOldName, strNewName);
                            File.Move(strFile, strNewPath.ToString());
                            //����ļ�ΪWord�ĵ���ʽ�����滻�����е�����
                            if (strFExtention.ToLower() == ".doc")
                            {
                                Word.Application newWord = new Word.Application();
                                object missing = System.Reflection.Missing.Value;
                                //��һ��Word�ĵ�
                                Word.Document newDocument = newWord.Documents.Open(ref strNewPath, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                                object Replaceone = Word.WdReplace.wdReplaceOne;
                                //����Ҫ���ҵĹؼ���
                                newWord.Selection.Find.ClearFormatting();
                                newWord.Selection.Find.Text = strOldName;
                                //����Ҫ�滻�Ĺؼ���
                                newWord.Selection.Find.Replacement.ClearFormatting();
                                newWord.Selection.Find.Replacement.Text = strNewName;
                                //ִ���滻����
                                newWord.Selection.Find.Execute(
                                    ref missing, ref missing, ref missing, ref missing, ref missing,
                                    ref missing, ref missing, ref missing, ref missing, ref missing,
                                    ref Replaceone, ref missing, ref missing, ref missing, ref missing);
                                //����Word�ĵ�
                                newWord.ActiveDocument.Save();
                                //�ر�Word�ĵ�
                                object objSaveChanges = Word.WdSaveOptions.wdSaveChanges;
                                Word.DocumentClass doc = newWord.ActiveDocument as Word.DocumentClass;
                                doc.Close(ref objSaveChanges, ref missing, ref missing);
                            }
                            break;
                    }
                    PBar.Value = i + 1;
                }
            }
        }
        */
        #endregion
        #region �������ļ�ʱ�������ؼ�����ʾ�ļ���
        /// <summary>
        /// �������ļ�ʱ�������ؼ�����ʾ�ļ���
        /// </summary>
        /// <param name="TNode">���ڵ����</param>
        public void TVShow(TreeNode TNode)
        {
            try
            {
                if (TNode.Nodes.Count == 0)
                {
                    if (TNode.Parent == null)
                    {
                        foreach (string DirName in Directory.GetLogicalDrives())
                        {
                            TreeNode DirNode = new TreeNode(DirName);
                            DirNode.Tag = DirName;
                            TNode.Nodes.Add(DirNode);
                        }
                    }
                    else
                    {
                        foreach (string PathName in Directory.GetFileSystemEntries((string)TNode.Tag))
                        {
                            TreeNode PathNode = new TreeNode(PathName);
                            PathNode.Tag = PathName;
                            TNode.Nodes.Add(PathNode);
                        }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region ɾ���ļ�/�ļ��У���������ɾ����
        /// <summary>
        /// ɾ���ļ�/�ļ��У���������ɾ����
        /// </summary>
        /// <param name="lv">��ʾ�ļ����ļ��е�ListView�ؼ�����</param>
        /// <param name="imagelist">��ʾ�ļ����ļ���ͼ���ImageList�ؼ�����</param>
        public void DeleteFile(ListView lv, ImageList imagelist,ToolStripProgressBar TSPBar)
        {
            TSPBar.Maximum = lv.SelectedItems.Count;
            foreach (object objFile in lv.SelectedItems)
            {
                string strFile = objFile.ToString();
                string strFullFile = AllPath + strFile.Substring(strFile.IndexOf("{") + 1, strFile.LastIndexOf("}") - strFile.IndexOf("{") - 1);
                if (File.Exists(strFullFile))
                    File.Delete(strFullFile);
                else if (Directory.Exists(strFullFile))
                    Directory.Delete(strFullFile, true);
                TSPBar.Value += 1;
            }
            GetListViewItem(AllPath, imagelist, lv);
        }
        #endregion

        #region �����ļ����ļ���
        /// <summary>
        /// �����ļ����ļ���
        /// </summary>
        /// <param name="lv">��ʾ���������ListView�ؼ�����</param>
        /// <param name="strName">Ҫ�������ļ����ļ��йؼ���</param>
        
        public void SearchFile(ListView lv, string strName)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(AllPath);
                FileSystemInfo[] files = dir.GetFileSystemInfos();
                //FileManage.frmMain frmmain = new FileManage.frmMain();
                //�Ե���FileSystemInfo�����ж�,������ļ�������еݹ����
                foreach (FileSystemInfo FSInfo in files)
                {
                    FileInfo file = FSInfo as FileInfo;
                    if (file != null)//�ж��ǲ����ļ�
                    {
                        if (file.Name.IndexOf(strName) != -1)
                        {
                            string[] info = new string[5];
                            info[0] = AllPath + file.Name;
                            double dbLength = file.Length / 1024;
                            if (dbLength < 1024)
                                info[1] = dbLength.ToString("0.00") + " KB";
                            else
                                info[1] = Convert.ToDouble(dbLength / 1024).ToString("0.00") + " MB";
                            info[2] = file.Extension.ToString();
                            info[3] = file.LastWriteTime.ToString();
                            info[4] = file.IsReadOnly.ToString();
                            ListViewItem item = new ListViewItem(info, Convert.ToString(AllPath + file.Name).Remove(0, 3));
                            lv.Items.Add(item);
                        }
                    }
                    else//������ļ���
                    {
                        if (FSInfo.Name.IndexOf(strName) != -1)
                        {
                            string[] info = new string[4];
                            info[0] = AllPath + FSInfo.Name;
                            info[1] = "";
                            info[2] = "�ļ���";
                            info[3] = FSInfo.LastWriteTime.ToString();
                            ListViewItem item = new ListViewItem(info, AllPath + FSInfo.Name);
                            lv.Items.Add(item);
                        }
                        AllPath += FSInfo.Name + "\\";
                        while (!Directory.Exists(AllPath))
                        {
                            string strNewPath = AllPath.Substring(0, AllPath.LastIndexOf("\\"));
                            AllPath = Directory.GetParent(strNewPath.Substring(0, strNewPath.LastIndexOf("\\"))).FullName + "\\" + FSInfo.Name + "\\";
                        }
                        SearchFile(lv, strName);
                    }
                }
            }
            catch { }
        }
        #endregion
        /*
        #region ѹ���ļ����ļ���
        /// <summary>
        /// �ݹ�ѹ���ļ��з���
        /// </summary>
        /// <param name="FolderToZip"></param>
        /// <param name="ZOPStream">ѹ���ļ����������</param>
        /// <param name="ParentFolderName"></param>
        private bool ZipFileDictory(string FolderToZip, ZipOutputStream ZOPStream, string ParentFolderName)
        {
            bool res = true;
            string[] folders, filenames;
            ZipEntry entry = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();
            try
            {
                //������ǰ�ļ���
                entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/"));  //���� ��/�� �Żᵱ�����ļ��д���
                ZOPStream.PutNextEntry(entry);
                ZOPStream.Flush();
                //��ѹ���ļ����ٵݹ�ѹ���ļ��� 
                filenames = Directory.GetFiles(FolderToZip);
                foreach (string file in filenames)
                {
                    //��ѹ���ļ�
                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/" + Path.GetFileName(file)));
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    ZOPStream.PutNextEntry(entry);
                    ZOPStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                if (entry != null)
                {
                    entry = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            folders = Directory.GetDirectories(FolderToZip);
            foreach (string folder in folders)
            {
                if (!ZipFileDictory(folder, ZOPStream, Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip))))
                {
                    return false;
                }
            }

            return res;
        }

        /// <summary>
        /// ѹ��Ŀ¼
        /// </summary>
        /// <param name="FolderToZip">��ѹ�����ļ���</param>
        /// <param name="ZipedFile">ѹ������ļ���</param>
        /// <returns></returns>
        private bool ZipFileDictory(string FolderToZip, string ZipedFile)
        {
            bool res;
            if (!Directory.Exists(FolderToZip))
            {
                return false;
            }
            ZipOutputStream ZOPStream= new ZipOutputStream(File.Create(ZipedFile));
            ZOPStream.SetLevel(6);
            res = ZipFileDictory(FolderToZip, ZOPStream, "");
            ZOPStream.Finish();
            ZOPStream.Close();
            return res;
        }

        /// <summary>
        /// ѹ���ļ�
        /// </summary>
        /// <param name="FileToZip">Ҫ����ѹ�����ļ���</param>
        /// <param name="ZipedFile">ѹ�������ɵ�ѹ���ļ���</param>
        /// <returns></returns>
        private bool ZipFile(string FileToZip, string ZipedFile)
        {
            //����ļ�û���ҵ����򱨴�
            if (!File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("ָ��Ҫѹ�����ļ�: " + FileToZip + " ������!");
            }
            FileStream ZipFile = null;
            ZipOutputStream ZipStream = null;
            ZipEntry ZipEntry = null;
            bool res = true;
            try
            {
                ZipFile = File.OpenRead(FileToZip);
                byte[] buffer = new byte[ZipFile.Length];
                ZipFile.Read(buffer, 0, buffer.Length);
                ZipFile.Close();
                ZipFile = File.Create(ZipedFile);
                ZipStream = new ZipOutputStream(ZipFile);
                ZipEntry = new ZipEntry(Path.GetFileName(FileToZip));
                ZipStream.PutNextEntry(ZipEntry);
                ZipStream.SetLevel(6);
                ZipStream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (ZipEntry != null)
                {
                    ZipEntry = null;
                }
                if (ZipStream != null)
                {
                    ZipStream.Finish();
                    ZipStream.Close();
                }
                if (ZipFile != null)
                {
                    ZipFile.Close();
                    ZipFile = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            return res;
        }

        /// <summary>
        /// ѹ���ļ����ļ���
        /// </summary>
        /// <param name="FileToZip">��ѹ�����ļ����ļ���</param>
        /// <param name="ZipedFile">ѹ�������ɵ�ѹ���ļ�����ȫ·����ʽ</param>
        /// <returns></returns>
        public bool Zip(String FileToZip, String ZipedFile)
        {
            if (Directory.Exists(AllPath + FileToZip))
            {
                return ZipFileDictory(AllPath + FileToZip, ZipedFile);
            }
            else if (File.Exists(AllPath + FileToZip))
            {
                return ZipFile(AllPath + FileToZip, ZipedFile);
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region ��ѹ�ļ�
        /// <summary>
        /// ��ѹ�ļ�
        /// </summary>
        /// <param name="FileToUpZip">����ѹ���ļ�</param>
        /// <param name="ZipedFolder">ָ����ѹĿ��Ŀ¼</param>
        public void UnZip(string FileToUpZip, string ZipedFolder)
        {
            if (!File.Exists(AllPath + FileToUpZip))
            {
                return;
            }
            if (!Directory.Exists(ZipedFolder))
            {
                Directory.CreateDirectory(ZipedFolder);
            }
            ZipInputStream ZIPStream = null;
            ZipEntry theEntry = null;
            string fileName;
            FileStream streamWriter = null;
            try
            {
                ZIPStream = new ZipInputStream(File.OpenRead(AllPath + FileToUpZip));
                while ((theEntry = ZIPStream.GetNextEntry()) != null)
                {
                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        //�ж��ļ�·���Ƿ����ļ���
                        if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }
                        streamWriter = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = ZIPStream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (theEntry != null)
                {
                    theEntry = null;
                }
                if (ZIPStream != null)
                {
                    ZIPStream.Close();
                    ZIPStream = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
        }
        #endregion
        */
        #region �ָ��ļ�
        /// <summary>
        /// �ָ��ļ�
        /// </summary>
        /// <param name="strFlag">�ָλ</param>
        /// <param name="intFlag">�ָ��С</param>
        /// <param name="strPath">�ָ����ļ����·��</param>
        /// <param name="strFile">Ҫ�ָ���ļ�</param>
        /// <param name="PBar">��������ʾ</param>
        public void SplitFile(string strFlag,int intFlag,string strPath,string strFile,ProgressBar PBar)
        {
            int iFileSize=0;
            //����ѡ�����趨�ָ��С�ļ��Ĵ�С
            switch (strFlag)
            {
                case "Byte":
                    iFileSize = intFlag;
                    break;
                case "KB":
                    iFileSize = intFlag * 1024;
                    break;
                case "MB":
                    iFileSize = intFlag * 1024 * 1024;
                    break;
                case "GB":
                    iFileSize = intFlag * 1024 * 1024 * 1024;
                    break;
            }
            //�����������ڴ�ŷָ��ļ���Ŀ¼����ȫ��ɾ����Ŀ¼�����ļ�
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            //���ļ���ȫ·��Ӧ���ַ������ļ���ģʽ����ʼ��FileStream�ļ���ʵ��
            FileStream SplitFileStream = new FileStream(strFile, FileMode.Open);
            //��FileStream�ļ�������ʼ��BinaryReader�ļ��Ķ���
            BinaryReader SplitFileReader = new BinaryReader(SplitFileStream);
            //ÿ�ηָ��ȡ���������
            byte[] TempBytes;
            //С�ļ�����
            int iFileCount = (int)(SplitFileStream.Length / iFileSize);
            PBar.Maximum = iFileCount;
            if (SplitFileStream.Length % iFileSize != 0) iFileCount++;
            string[] TempExtra = strFile.Split('.');
            //ѭ�������ļ��ָ�ɶ��С�ļ�
            for (int i = 1; i <= iFileCount; i++)
            {
                //ȷ��С�ļ����ļ�����
                string sTempFileName = strPath + @"\" + i.ToString().PadLeft(4, '0') + "." + TempExtra[TempExtra.Length - 1]; //С�ļ���
                //�����ļ����ƺ��ļ���ģʽ����ʼ��FileStream�ļ���ʵ��
                FileStream TempStream = new FileStream(sTempFileName, FileMode.OpenOrCreate);
                //��FileStreamʵ������������ʼ��BinaryWriter��д��ʵ��
                BinaryWriter TempWriter = new BinaryWriter(TempStream);
                //�Ӵ��ļ��ж�ȡָ����С����
                TempBytes = SplitFileReader.ReadBytes(iFileSize);
                //�Ѵ�����д��С�ļ�
                TempWriter.Write(TempBytes);
                //�ر���д�����γ�С�ļ�
                TempWriter.Close();
                //�ر��ļ���
                TempStream.Close();
                PBar.Value = i-1; 
            }
            //�رմ��ļ��Ķ���
            SplitFileReader.Close();
            SplitFileStream.Close();
            MessageBox.Show("�ļ��ָ�ɹ�!");
        }
        #endregion

        #region �ϲ��ļ�
        /// <summary>
        /// �ϲ��ļ�
        /// </summary>
        /// <param name="list">Ҫ�ϲ����ļ�����</param>
        /// <param name="strPath">�ϲ�����ļ�����</param>
        /// <param name="PBar">��������ʾ</param>
        public void CombinFile(string[] strFile, string strPath, ProgressBar PBar)
        {
            PBar.Maximum = strFile.Length;
            FileStream AddStream = null;
            //�Ժϲ�����ļ����ƺʹ򿪷�ʽ����������ʼ��FileStream�ļ���
            AddStream = new FileStream(strPath, FileMode.Append);
            //��FileStream�ļ�������ʼ��BinaryWriter��д���������Ժϲ��ָ���ļ�
            BinaryWriter AddWriter = new BinaryWriter(AddStream);
            FileStream TempStream = null;
            BinaryReader TempReader = null;
            //ѭ���ϲ�С�ļ��������ɺϲ��ļ�
            for (int i = 0; i < strFile.Length; i++)
            {
                //��С�ļ�����Ӧ���ļ����ƺʹ�ģʽ����ʼ��FileStream�ļ��������ȡ�ָ�����
                TempStream = new FileStream(strFile[i].ToString(), FileMode.Open);
                TempReader = new BinaryReader(TempStream);
                //��ȡ�ָ��ļ��е����ݣ������ɺϲ����ļ�
                AddWriter.Write(TempReader.ReadBytes((int)TempStream.Length));
                //�ر�BinaryReader�ļ��Ķ���
                TempReader.Close();
                //�ر�FileStream�ļ���
                TempStream.Close();
                PBar.Value = i + 1;
            }
            //�ر�BinaryWriter�ļ���д��
            AddWriter.Close();
            //�ر�FileStream�ļ���
            AddStream.Close();
            MessageBox.Show("�ļ��ϲ��ɹ���");
        }
        #endregion
    }

    #region ��ListView�ؼ��е����������
    /// <summary>
    /// ��ListView�ؼ��е����������
    /// </summary>
    class ListViewItemComparer : IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        }
    }
    #endregion
}