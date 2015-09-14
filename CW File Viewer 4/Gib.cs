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
    class Gib
    {
        /*
         * c : class
         * f : place
        */
        public static string fname, nfname, last, infile = "", backup = "",crossfile;
        public static string copypath, copyfn, cuts, cuta, ppath, pname, plast, ptext, txtpath, rtff;
        public static string rbfile,nowchoose;
        public static string[,] fpos = new string[5,1000];
        public static int[,] lpos = new int[5, 1000];
        //public static bool[,] fch = new bool[5, 1000];

        public static string[] cname = new string[100000];
        public static string[] cpath = new string[100000];
        public static string[] clast = new string[100000];
        public static int[] cmode = new int[100000];//1:file  2:folder

        public static string[] hispath = new string[100000];
        public static int hissum;

        public static bool firsto, isheng = true;
        public static int cpmode;//1:file  2:folder
        public static bool cpiscut;

        public static int[] nowp = new int[5];
        public static int[] maxn = new int[5];

        public static int p = 0, l, cutn = 8, wsize = 12, fsum, smode = 1, mx, my, nowtab = 1,pstate=2;//pstate  1:play  ,  2:pause

        public static char[] a = new char[100000];
        public static char[] cutc = new char[20];

        public static bool isfullscr = false, fullscr = false;

        public static string selecttext = "";

        public static int nowcrst=0;
    }
}
/*

if (Gib.nowtab == 1)
else if (Gib.nowtab == 2)
else

*/