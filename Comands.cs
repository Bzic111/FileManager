using FileManager.Enums;
using FileManager.Old;

namespace FileManager;
class Comands
{
    Log logger = new Log();
    public void Reader(string str, ref Tree tree, Entry entry, out bool reFreshFrame)
    {
        //string[] inLine = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Frame Error = new Frame(30, 30, 5, 40, "Error", ColorScheme.Warning);
        reFreshFrame = false;
        string comand = null;
        string path = null;
        string attr = null;
        int charCount = 0;
        for (int i = 0; i < str.Length && !char.IsWhiteSpace(str[i]); i++, charCount++) comand += str[i];
        for (int i = ++charCount; i < str.Length && str[i] != '-'; i++, charCount++) path += str[i];
        for (int i = ++charCount; i < str.Length && !char.IsWhiteSpace(str[i]); i++, charCount++)
        {
            if (str[i] == '-') continue;
            else attr += str[i];
        }
        path.Trim();
        switch (comand)
        {
            case "CD":
            case "cd":
                if (path == "\\") tree.ChangeDirectory(tree.Pages[0][0].Parent);
                else if (Directory.Exists(path + '\\')) tree.ChangeDirectory(path + '\\');
                else if (Directory.Exists(entry.Path + path + '\\')) tree.ChangeDirectory(entry.Path + path + '\\');
                else Console.Write("Bad Path!");
                reFreshFrame = true;
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

                        Error.Show(true);
                        Error.WriteText(e.Message);
                        Meth.WriteLog(e.Message);
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
                        Error.Show(true);
                        Error.WriteText(e.Message);
                        Meth.WriteLog(e.Message);
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
                        Error.Show(true);
                        Error.WriteText(e.Message);
                        Meth.WriteLog(e.Message);
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
                        Error.Show(true);
                        Error.WriteText(e.Message);
                        Meth.WriteLog(e.Message);
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
                        Error.Show(true);
                        Error.WriteText(e.Message);
                        Meth.WriteLog(e.Message);
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
                        Error.Show(true);
                        Error.WriteText(e.Message);
                        Meth.WriteLog(e.Message);
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                }
                else if (File.Exists(attr))
                {
                    string errorStr = $"File {attr} already exist";
                    Error.Show(true);
                    Error.WriteText(errorStr);
                    Meth.WriteLog(errorStr);
                    Console.ResetColor();
                    Console.ReadKey(true);
                }
                else if (!File.Exists(entry.Path + path))
                {
                    string errorStr = $"File {path} not exist";
                    Error.Show(true);
                    Error.WriteText(errorStr);
                    Meth.WriteLog(errorStr);
                    Console.ResetColor();
                    Console.ReadKey(true);
                }
                break;
            default:
                Console.Write("Bad Comand!");
                Meth.WriteLog("Bad Comand!");
                break;
        }
    }
    public void Delete(Entry entry)
    {
        Frame warn = new Frame(30, 30, 5, 60, "Error", ColorScheme.Warning);

        if (entry.type == Entry.Type.Directory)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(entry.Path);
                di.Delete(true);
                Meth.WriteLog($"Directory {entry.Path} deleted.");
            }
            catch (Exception e)
            {
                warn.Show(true);
                warn.WriteText(e.Message);
                Meth.WriteLog(e.Message);
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }
        else if (entry.type == Entry.Type.File)
        {
            try
            {
                FileInfo fi = new FileInfo(entry.Path);
                fi.Delete();
                Meth.WriteLog($"File {entry.Path} deleted.");
            }
            catch (Exception e)
            {
                warn.Show(true);
                warn.WriteText(e.Message);
                Meth.WriteLog(e.Message);
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }
    }
    public void Create(Entry entry, char type)
    {
        Frame warn = new Frame(30, 30, 5, 60, "Error", ColorScheme.Warning);
        Frame readLine = new Frame(30, 30, 5, 60, "Input Name", ColorScheme.BIOS);

        string name;
        switch (type)
        {
            case 'D':
            case 'd':
                readLine.Show(true);
                readLine.SetColor(ColorsPreset.ContextNormal);
                readLine.WriteText("".PadRight(readLine.Geometry.Cols - 2, ' '));
                readLine.SetCursorPosition(0, 0);
                name = Console.ReadLine();
                if (!Directory.Exists($"{entry.Path}\\{name}"))
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo($"{entry.Path}\\{name}");
                        di.Create();
                        Meth.WriteLog($"Created new directory {entry.Path}\\{name}");
                    }
                    catch (Exception e)
                    {
                        warn.Show(true);
                        warn.WriteText(e.Message);
                        Meth.WriteLog(e.Message);
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    warn.Show(true);
                    warn.WriteText("Directory already exist.");
                    Meth.WriteLog($"Failed to create new directory {entry.Path}\\{name}");
                    Console.ResetColor();
                    Console.ReadKey(true);
                }
                break;
            case 'F':
            case 'f':
                readLine.Show(true);
                readLine.SetColor(ColorsPreset.ContextNormal);
                readLine.WriteText("".PadRight(readLine.Geometry.Cols - 2, ' '));
                readLine.SetCursorPosition(0, 0);
                name = Console.ReadLine();
                if (!File.Exists($"{entry.Path}\\{name}"))
                {
                    try
                    {
                        FileStream fs = File.Create($"{entry.Path}\\{name}");
                        fs.Close();
                        Meth.WriteLog($"Created new file {entry.Path}\\{name}");
                    }
                    catch (Exception e)
                    {
                        warn.Show(true);
                        warn.WriteText(e.Message);
                        Meth.WriteLog(e.Message);
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    warn.Show(true);
                    warn.WriteText("File already exist.");
                    Meth.WriteLog($"Failed to create new file {entry.Path}\\{name}");
                    Console.ResetColor();
                    Console.ReadKey(true);
                }
                break;
            default: break;
        }
    }
    public void Move(Entry entry, string destinationPath)
    {
        Frame warn = new Frame(30, 30, 5, 60, "Error", ColorScheme.Warning);

        if (entry.type == Entry.Type.Directory)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(entry.Path);
                di.MoveTo(destinationPath);
                Meth.WriteLog($"Directory {entry.Path} moved to {destinationPath}");
            }
            catch (Exception e)
            {
                warn.Show(true);
                warn.WriteText(e.Message);
                Meth.WriteLog(e.Message);
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }
        else if (entry.type == Entry.Type.File)
        {
            try
            {
                File.Move(entry.Path, destinationPath);
                Meth.WriteLog($"File {entry.Path} moved to {destinationPath}");
            }
            catch (Exception e)
            {
                warn.Show(true);
                warn.WriteText(e.Message);
                Meth.WriteLog(e.Message);
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }
    }
    void CopyDir(DirectoryInfo source, DirectoryInfo target)
    {
        Frame warn = new Frame(30, 30, 5, 60, "Error", ColorScheme.Warning);

        if (source.FullName == target.FullName)
        {
            warn.Show(true);
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
                    warn.Show(true);
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
    public void Copy(Entry entry, string destinationPath)
    {
        Frame warn = new Frame(30, 30, 5, 60, "Error", ColorScheme.Warning);
        Frame question = new Frame((30, 30, 5, 60), "Overwrite?", ColorScheme.Warning);
        bool overwrite = false;

        if (entry.type == Entry.Type.File)
        {
            if (File.Exists(destinationPath))
            {
                question.Show(true);
                question.WriteText("File already exist, overwrite this? [Y/N]");
                Console.ReadKey(true);
                if (Console.ReadKey(true).Key == ConsoleKey.Y) overwrite = true;
                else return;
            }
            FileInfo fi = new FileInfo($@"{entry.Path}\{entry.Name}");
            try
            {
                logger.WriteLog($"Try Copy {fi.FullName} to {destinationPath}.");
                fi.CopyTo(destinationPath, overwrite);
                logger.WriteLog($"{fi.FullName} Copyed to {destinationPath}.");
            }
            catch (Exception e)
            {
                logger.WriteLog(e.Message);
                warn.Show(true);
                warn.WriteText(e.Message);
                Meth.WriteLog(e.Message);
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }
        else if (entry.type == Entry.Type.Directory)
        {
            if (Directory.Exists(destinationPath))
            {
                question.Show(true);
                question.WriteText("Directory already exist, overwrite all entryes? [Y/N]");
                Console.ReadKey(true);
                if (Console.ReadKey(true).Key == ConsoleKey.Y) overwrite = true;
                else return;
            }
            try
            {
                DirectoryInfo dis = new DirectoryInfo($@"{entry.Path}\{entry.Name}");
                DirectoryInfo dit = new DirectoryInfo(destinationPath);
                CopyDir(dis, dit);
            }
            catch (Exception e)
            {
                logger.WriteLog(e.Message);
                warn.Show(true);
                warn.WriteText(e.Message);
                Meth.WriteLog(e.Message);
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }
    }
}