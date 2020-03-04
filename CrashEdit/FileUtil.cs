using System;
using System.IO;
using System.Windows.Forms;

namespace CrashEdit
{
    public static class FileUtil
    {
        private static OpenFileDialog openfiledlg;
        private static SaveFileDialog savefiledlg;

        static FileUtil()
        {
            openfiledlg = new OpenFileDialog();
            savefiledlg = new SaveFileDialog();
        }

        public static IWin32Window Owner { get; set; } = null;

        public static byte[] OpenFile(params string[] filters)
        {
            openfiledlg.Filter = string.Join("|",filters);
            openfiledlg.Multiselect = false;
            if (openfiledlg.ShowDialog(Owner) == DialogResult.OK)
            {
                return File.ReadAllBytes(openfiledlg.FileName);
            }
            else
            {
                return null;
            }
        }

        public static byte[][] OpenFiles(params string[] filters)
        {
            openfiledlg.Filter = string.Join("|",filters);
            openfiledlg.Multiselect = true;
            if (openfiledlg.ShowDialog(Owner) == DialogResult.OK)
            {
                byte[][] result = new byte [openfiledlg.FileNames.Length][];
                for (int i = 0;i < openfiledlg.FileNames.Length;i++)
                {
                    result[i] = File.ReadAllBytes(openfiledlg.FileNames[i]);
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static bool SaveFile(byte[] data,params string[] filters)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            savefiledlg.Filter = string.Join("|",filters);
            if (savefiledlg.ShowDialog(Owner) == DialogResult.OK)
            {
                File.WriteAllBytes(savefiledlg.FileName,data);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SaveFile(string defaultname,byte[] data,params string[] filters)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            savefiledlg.Filter = string.Join("|",filters);
            savefiledlg.FileName = defaultname;
            if (savefiledlg.ShowDialog(Owner) == DialogResult.OK)
            {
                File.WriteAllBytes(savefiledlg.FileName,data);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
