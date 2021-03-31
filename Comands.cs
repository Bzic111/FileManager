using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
namespace FileManager
{
    class Comands
    {
        public Comands()
        {

        }
        public void Reader(string str, ref Tree tree, Entry entry, out bool reFreshFrame)
        {
            string[] inLine = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            reFreshFrame = false;
            string comand = null;
            string path = null;
            string attr = null;
            int charCount = 0;
            for (int i = 0; i < str.Length; i++, charCount++)
            {
                if (!char.IsWhiteSpace(str[i]))
                {
                    comand += str[i];
                }
                else
                {
                    break;
                }
            }
            for (int i = ++charCount; i < str.Length; i++, charCount++)
            {
                if (str[i] != '-')
                {
                    path += str[i];
                }
                else
                {
                    break;
                }
            }
            for (int i = ++charCount; i < str.Length; i++, charCount++)
            {
                if (str[i] == '-')
                {
                    continue;
                }
                else if (!char.IsWhiteSpace(str[i]))
                {
                    attr += str[i];
                }
                else
                {
                    break;
                }
            }
            path.Trim();
            switch (comand)
            {
                case "CD":
                case "cd":
                    if (path == "\\")
                    {
                        tree.ChangeDirectory(tree.Pages[0][0].Parent);
                    }
                    else if (Directory.Exists(path + '\\'))
                    {
                        tree.ChangeDirectory(path + '\\');
                        reFreshFrame = true;
                    }
                    else if (Directory.Exists(entry.Path + path + '\\'))
                    {
                        tree.ChangeDirectory(entry.Path + path + '\\');
                        reFreshFrame = true;
                    }
                    else
                    {
                        Console.Write("Bad Path!");
                    }
                    break;
                case "del":
                case "DEL":
                case "Del":
                    if (Directory.Exists(path))
                    {

                        try
                        {
                            DirectoryInfo dif = new DirectoryInfo(entry.Path + path);
                            dif.Delete(true);
                            reFreshFrame = true;
                        }
                        catch (Exception e)
                        {
                            Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                            Error.SetName("Error");
                            Error.Coloring(Frame.Colorscheme.Warning);
                            Error.SetColor(Frame.ColorsPreset.Normal);
                            Error.Show();
                            Error.Clear();
                            Error.WriteName();
                            Error.WriteText(e.Message);
                            Console.ResetColor();
                            Console.ReadKey(true);
                        }
                    }
                    else if (File.Exists(path))
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(entry.Path + path);
                            fi.Delete();
                        }
                        catch (Exception e)
                        {
                            Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                            Error.SetName("Error");
                            Error.Coloring(Frame.Colorscheme.Warning);
                            Error.SetColor(Frame.ColorsPreset.Normal);
                            Error.Show();
                            Error.Clear();
                            Error.WriteName();
                            Error.WriteText(e.Message);
                            Console.ResetColor();
                            Console.ReadKey(true);
                        }

                    }
                    break;
                case "new":
                case "New":
                case "NEW":
                    if (attr == "d" | attr == "D")
                    {
                        try
                        {
                            DirectoryInfo dif = new DirectoryInfo(tree.CurrentPath + path + '\\');
                            dif.Create();
                            reFreshFrame = true;
                        }
                        catch (Exception e)
                        {
                            Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                            Error.SetName("Acces Denied");
                            Error.Coloring(Frame.Colorscheme.Warning);
                            Error.SetColor(Frame.ColorsPreset.Normal);
                            Error.Show();
                            Error.Clear();
                            Error.WriteName();
                            Error.WriteText(e.Message);
                            Console.ResetColor();
                            Console.ReadKey(true);
                        }
                    }
                    else if (attr == "f" | attr == "F")
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(tree.CurrentPath + path);
                            fi.Create();
                            reFreshFrame = true;
                        }
                        catch (Exception e)
                        {
                            Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                            Error.SetName("Error");
                            Error.Coloring(Frame.Colorscheme.Warning);
                            Error.SetColor(Frame.ColorsPreset.Normal);
                            Error.Show();
                            Error.Clear();
                            Error.WriteName();
                            Error.WriteText(e.Message);
                            Console.ResetColor();
                            Console.ReadKey(true);
                        }
                    }
                    break;
                case "move":
                case "Move":
                case "MOVE":
                    if (Directory.Exists(attr) & (Directory.Exists(path) | File.Exists(path)))
                    {
                        try
                        {
                            Directory.Move(path, attr);
                        }
                        catch (Exception e)
                        {
                            Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                            Error.SetName("Acces Denied");
                            Error.Coloring(Frame.Colorscheme.Warning);
                            Error.SetColor(Frame.ColorsPreset.Normal);
                            Error.Show();
                            Error.Clear();
                            Error.WriteName();
                            Error.WriteText(e.Message);
                            Console.ResetColor();
                            Console.ReadKey(true);
                        }
                    }
                    break;
                case "copy":
                case "COPY":
                case "Copy":
                    if (File.Exists(entry.Path + path) & !File.Exists(attr))
                    {
                        try
                        {
                            File.Copy(entry.Path + path, attr);
                        }
                        catch (Exception e)
                        {
                            Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                            Error.SetName("Acces Denied");
                            Error.Coloring(Frame.Colorscheme.Warning);
                            Error.SetColor(Frame.ColorsPreset.Normal);
                            Error.Show();
                            Error.Clear();
                            Error.WriteName();
                            Error.WriteText(e.Message);
                            Console.ResetColor();
                            Console.ReadKey(true);
                        }
                    }
                    else if (File.Exists(attr))
                    {
                        string errorStr = $"File {attr} already exist";
                        Frame Error = new Frame(30, 30, 5, errorStr.Length + 2);
                        Error.SetName("File exist");
                        Error.Coloring(Frame.Colorscheme.Warning);
                        Error.SetColor(Frame.ColorsPreset.Normal);
                        Error.Show();
                        Error.Clear();
                        Error.WriteName();
                        Error.WriteText(errorStr);
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                    else if (!File.Exists(entry.Path + path))
                    {
                        string errorStr = $"File {path} not exist";
                        Frame Error = new Frame(30, 30, 5, errorStr.Length + 2);
                        Error.SetName("File not exist");
                        Error.Coloring(Frame.Colorscheme.Warning);
                        Error.SetColor(Frame.ColorsPreset.Normal);
                        Error.Show();
                        Error.Clear();
                        Error.WriteName();
                        Error.WriteText(errorStr);
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                    break;
                default:
                    Console.Write("Bad Comand!");
                    break;
            }

        }
        public void Delete(Entry entry)
        {
            Frame warn = new Frame(30, 30, 5, 60);
            warn.Coloring(Frame.Colorscheme.Warning);

            if (entry.type == Entry.Type.Directory)
            {
                try
                {
                    DirectoryInfo di = (DirectoryInfo)entry.GetInfoType();
                    di.Delete(true);
                }
                catch (Exception e)
                {
                    warn.Show();
                    warn.Clear();
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                }
            }
            else if (entry.type == Entry.Type.File)
            {
                try
                {
                    FileInfo fi = (FileInfo)entry.GetInfoType();
                    fi.Delete();
                }
                catch (Exception e)
                {
                    warn.Show();
                    warn.Clear();
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                }
            }
        }
        public void Create(Entry entry, char type)
        {
            Frame warn = new Frame(30, 30, 5, 60);
            Frame readConsole = new Frame(30, 30, 5, 60);
            warn.Coloring(Frame.Colorscheme.Warning);
            readConsole.Coloring(Frame.Colorscheme.BIOS);
            string name;
            switch (type)
            {
                case 'D':
                case 'd':
                    readConsole.Show();
                    readConsole.Clear();
                    readConsole.SetName("Input Name");
                    readConsole.SetColor(Frame.ColorsPreset.ContextNormal);
                    readConsole.WriteText("".PadRight(readConsole.cols - 2, ' '));
                    readConsole.SetCursorPosition(0, 0);
                    name = Console.ReadLine();
                    if (!Directory.Exists(entry.Path + '\\' + name))
                    {
                        try
                        {
                            DirectoryInfo di = new DirectoryInfo(entry.Path + '\\' + name);
                            di.Create();
                        }
                        catch (Exception e)
                        {
                            warn.Show();
                            warn.Clear();
                            warn.WriteText(e.Message);
                            Console.ReadKey(true);
                        }

                    }
                    else
                    {
                        warn.Show();
                        warn.Clear();
                        warn.WriteText("Directory already exist.");
                        Console.ReadKey(true);
                    }
                    break;
                case 'F':
                case 'f':
                    readConsole.Show();
                    readConsole.Clear();
                    readConsole.SetName("Input Name");
                    readConsole.SetColor(Frame.ColorsPreset.ContextNormal);
                    readConsole.WriteText("".PadRight(readConsole.cols - 2, ' '));
                    readConsole.SetCursorPosition(0, 0);
                    name = Console.ReadLine();
                    if (!File.Exists(entry.Path + '\\' + name))
                    {
                        try
                        {
                            FileStream fs = File.Create(entry.Path + '\\' + name);
                            fs.Close();
                        }
                        catch (Exception e)
                        {
                            warn.Show();
                            warn.Clear();
                            warn.WriteText(e.Message);
                            Console.ReadKey(true);
                        }
                    }
                    else
                    {
                        warn.Show();
                        warn.Clear();
                        warn.WriteText("File already exist.");
                        Console.ReadKey(true);
                    }
                    break;
                default: break;
            }
        }
        public void Move(Entry entry, string destinationPath)
        {
            Frame warn = new Frame(30, 30, 5, 60);
            warn.Coloring(Frame.Colorscheme.Warning);
            if (entry.type == Entry.Type.Directory)
            {
                try
                {
                    DirectoryInfo di = (DirectoryInfo)entry.GetInfoType();
                    di.MoveTo(destinationPath);
                }
                catch (Exception e)
                {
                    warn.Show();
                    warn.Clear();
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                }
            }
            else if (entry.type == Entry.Type.File)
            {
                try
                {
                    File.Move(entry.Path, destinationPath);
                }
                catch (Exception e)
                {
                    warn.Show();
                    warn.Clear();
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                }
            }
        }
        public void CopyDir(DirectoryInfo source, DirectoryInfo target)
        {
            Frame warn = new Frame(30, 30, 5, 60);
            warn.Coloring(Frame.Colorscheme.Warning);

            if (source.FullName == target.FullName)
            {
                warn.Show();
                warn.Clear();
                warn.WriteText("Пути совпадают");
                Console.ReadKey(true);
            }
            else
            {
                if (Directory.Exists(target.FullName) == false)
                {
                    Directory.CreateDirectory(target.FullName);
                }
                foreach (FileInfo fi in source.GetFiles())
                {
                    try
                    {
                        fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                    }
                    catch (Exception e)
                    {
                        warn.Show();
                        warn.Clear();
                        warn.WriteText(e.Message);
                        Console.ReadKey(true);
                    }
                }
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyDir(diSourceSubDir, nextTargetSubDir);
                }
            }
        }
    }
}
