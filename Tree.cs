using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
namespace FileManager
{
    /// <summary>
    /// Класс для хранения информации о файлах и каталогах в списках.
    /// </summary>
    [Serializable]
    public class Tree
    {
        /// <summary>
        /// Отсортированный список файлов и каталогов
        /// </summary>
        public List<Entry> Entryes;

        /// <summary>
        /// Список файлов и каталогов по страницам
        /// </summary>
        public List<List<Entry>> Pages;

        /// <summary>
        /// Текущая директория
        /// </summary>
        public string CurrentPath;

        /// <summary>
        /// Список дисков
        /// </summary>
        public List<string> Roots { get; private set; }

        
        public Tree() => SetRoots();

        /// <summary>
        /// Сбор информации о текущих подключенный дисках
        /// </summary>
        public void SetRoots()
        {
            List<string> drives = new List<string>();
            for (char c = 'A'; c < 'Z'; c++)
            {
                if (Directory.Exists(c.ToString() + ":\\"))
                {
                    drives.Add(c.ToString() + ":\\");
                }
            }
            Roots = drives;
        }

        /// <summary>
        /// Сбор списка файлов и каталогов в директории <paramref name="path"/>
        /// </summary>
        /// <param name="path"></param>
        void GetEntryList(string path)
        {
            List<Entry> entryes = new List<Entry>();
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);
                string[] pather = path.Split('\\', StringSplitOptions.RemoveEmptyEntries);

                Array.Sort(dirs);
                Array.Sort(files);
                entryes.Add(new Entry(path, Entry.Type.Directory));
                entryes[0].Name = pather[^1];
                entryes[0].Path = path;
                foreach (var item in dirs)
                {
                    entryes.Add(new Entry(item, Entry.Type.Directory));
                }
                foreach (var item in files)
                {
                    entryes.Add(new Entry(item, Entry.Type.File));
                }
                CurrentPath = path;
                Entryes = entryes;
            }
            catch (UnauthorizedAccessException e)
            {
                Frame Error = new Frame(30, 30, 5, e.Message.Length + 2);
                Error.SetName("Acces Denied");
                Error.Coloring(Frame.ColorScheme.Warning);
                Error.SetColor(Frame.ColorsPreset.Normal);
                Error.Show();
                Error.Clear();
                Error.WriteName();
                Error.WriteText(e.Message);
                Console.ResetColor();
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Разбиение списка файлов и каталогов на страницы по 40 элементов. 1й всегда родительский каталог или диск.
        /// </summary>
        /// <param name="Entryes">Список файлов и каталогов</param>
        void GetPages(List<Entry> Entryes)
        {
            List<List<Entry>> pages = new List<List<Entry>>();
            for (int i = 0, counter = 0; counter < Entryes.Count; i++)
            {
                pages.Add(new List<Entry>());
                for (int j = 0; j < 40 & counter < Entryes.Count; j++, counter++)
                {
                    pages[i].Add(Entryes[counter]);
                }
            }
            Pages = pages;
        }

        /// <summary>
        /// Обновление данных директории
        /// </summary>
        public void ReFresh()
        {
            GetEntryList(CurrentPath);
            GetPages(Entryes);
        }

        /// <summary>
        /// Смена директории с текущей на <paramref name="path"/>
        /// </summary>
        /// <param name="path">путь</param>
        public void ChangeDirectory(string path)
        {
            Frame warn = new Frame(30, 30, 5, 60, "Error", Frame.ColorScheme.Warning);
            if (string.IsNullOrEmpty(path))
            {
                warn.Show(true);
                warn.WriteText("Bad Path");
                Console.ReadKey(true);
                Console.ResetColor();
            }
            else
            {
                try
                {
                    GetEntryList(path);
                    GetPages(Entryes);
                }
                catch (Exception e)
                {
                    warn.Show(true);
                    warn.WriteText(e.Message);
                    Console.ReadKey(true);
                    Console.ReadKey(true);
                    Console.ResetColor();
                }
            }
        }
    }
}
