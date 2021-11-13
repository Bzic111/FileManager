namespace FileManager;

/// <summary>Координаты фрейма</summary>
/// <param name="StartCol">Стартовый столбец</param>
/// <param name="StartRow">Стартовая строка</param>
/// <param name="Rows">Всего строк</param>
/// <param name="Cols">всего столбцов</param>
public record struct Coordinates(int StartCol, int StartRow, int Rows, int Cols)
{
    public static implicit operator (int StartCol, int StartRow, int Rows, int Cols)(Coordinates value)
    {
        return (value.StartCol, value.StartRow, value.Rows, value.Cols);
    }

    public static implicit operator Coordinates((int StartCol, int StartRow, int Rows, int Cols) value)
    {
        return new Coordinates(value.StartCol, value.StartRow, value.Rows, value.Cols);
    }
    public int GetFreeSpace() => (Rows - 2) * (Cols - 2);
}
