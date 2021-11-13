using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Interfaces
{
    internal interface IFrame
    {
        /// <summary>Показать фрейм</summary>
        public void Show();
        /// <summary>Переместить фрем</summary><param name="startRow">Строка</param><param name="startCol">Столбец</param>
        public void Move(int startRow, int startCol);
        /// <summary>Название фрейма</summary><param name="name">Имя</param>
        public void SetName(string name);
        /// <summary>Записать контент</summary><param name="content">Текст</param>
        public void SetContent(string content);
        /// <summary>Показать контент</summary>
        public void WriteContent();
        /// <summary>Очистить фрейм</summary>
        public void Clear();
        /// <summary>Обновить фрейм</summary>
        public void Refresh() { Show(); Clear(); }
    }
}
