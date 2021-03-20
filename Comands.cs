using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileManager
{
    class Comands
    {
        delegate void Comand(string path);
        delegate void ComandTo(string path, string destination);
        Comand[] ComandDelegates;
        ComandTo[] ComandToDelegates;
        string[] ComandList;
        string tempDirPath;
        string tempFilePath;
        public Comands()
        {
            ComandList = new string[]
            {
                "Create Directory","Delete","Copy to","Cut","Paste","Rename","Info"
            };
            ComandDelegates = new Comand[]
            {
                CreateDir,
                DeleteDir,
                DeleteCatalog,
                DeleteFile,
                CopyToTemp,
                CopyFromTemp,
                Cut
            };
            ComandToDelegates = new ComandTo[]
            {
                CopyFile,
                CopyDir,
                RenameDir,
                RenameFile,
                RewriteFile
            };
        }
        public void ShowComandList(int left,int top)
        {
            int currentLeft = left;
            int currentTop = top + 2;
            for (int i = 0; i < ComandList.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(currentLeft + 10, currentTop + i);
                Console.Write(ComandList[i]);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(currentLeft, currentTop);
        }
        public string[] FileInfo(string path)
        {
            string[] result = new string[5];
            FileInfo info = new FileInfo(path);
            result[0] = info.Attributes.ToString();
            result[1] = info.Extension;
            result[2] = info.CreationTime.ToString();
            result[3] = info.LastWriteTime.ToString();
            result[4] = info.Length.ToString();
            return result;
        }
        void CreateDir(string path)
        {
            Directory.CreateDirectory(path);
        }
        void DeleteDir(string path)
        {
            Directory.Delete(path);
        }
        void DeleteCatalog(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            foreach (var item in files)
            {
                File.Delete(item);
            }
            foreach (var item in dirs)
            {
                if (Directory.Exists(item))
                {
                    DeleteCatalog(item);
                }
                Directory.Delete(item);
            }
        }
        void DeleteFile(string path)
        {
            File.Delete(path);
        }
        void CopyToTemp(string path)
        {
            CopyDir(path, @"%temp%\temporaryFolder");
        }
        void CopyFromTemp(string destination)
        {
            CopyDir(@"%temp%\temporaryFolder", destination);
        }
        void CopyFile(string path, string destination)
        {
            File.Copy(path, destination);
        }
        void CopyDir(string path, string destination)
        {
            Directory.CreateDirectory(destination);
            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            ///copy all files
            foreach (var item in files)
            {
                File.Copy(item, destination);
            }
            /// create new dirs
            foreach (var item in dirs)
            {
                string temp = item.Split('\\')[^1];
                Directory.CreateDirectory(destination+"\\"+temp);
                /// reCopyDir 
                if (Directory.Exists(item))
                {
                    CopyDir(item, destination + "\\" + temp);
                }
            }
        }
        void RenameDir(string oldName, string newName)
        {
            DirectoryInfo directory = new DirectoryInfo(oldName);
            string[] name = oldName.Split('\\');
            string naming = null;
            for (int i = 0; i < name.Length-1; i++)
            {
                naming += '\\'+name[i];
            }

            directory.MoveTo($@"{naming}\{newName}");
        }
        void RenameFile(string oldName, string newName)
        {
            FileInfo info = new FileInfo(oldName);
            string[] name = oldName.Split('\\');
            string naming = null;
            for (int i = 0; i < name.Length - 1; i++)
            {
                naming += '\\' + name[i];
            }
            info.MoveTo($@"{naming}\{newName}");
        }
        void RewriteFile(string oldName, string newName)
        {
            FileInfo info = new FileInfo(oldName);
            string[] name = oldName.Split('\\');
            string naming = null;
            for (int i = 0; i < name.Length - 1; i++)
            {
                naming += '\\' + name[i];
            }
            info.MoveTo($@"{naming}\{newName}",true);
        }
        void Cut(string path) 
        {
            if (Directory.Exists(path))
            {
                tempDirPath = path;
            }
            else if (File.Exists(path))
            {
                tempFilePath = path;
            }
        }
        void Paste(string destination,char type)
        {
            switch (type)
            {
                case 'D':
                case 'd':
                    CopyDir(tempDirPath, destination);
                    break;
                case 'F':
                case 'f':
                    CopyFile(tempFilePath, destination);
                    break;
                default:
                    break;
            }
        }
    }
}
