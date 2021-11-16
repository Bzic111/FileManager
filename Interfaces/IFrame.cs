using FileManager.Internal;

namespace FileManager.Interfaces;
public interface IFrame
{
    public Coordinates Geometry { get; }
    public string[] Content { get; }
    public string Name { get; }
    /// <summary>Показать фрейм</summary>
    public void Show(bool name = true);
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

    void FillContent(string str, int index)
    {
        if (index < Content.Length)
        {
            Content[index] = str.Length > Geometry.Cols - 2 ? str.Remove(Geometry.Cols - 3) + "~" : str;
        }
    }
}