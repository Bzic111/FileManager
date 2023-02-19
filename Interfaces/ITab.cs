/// <summary>Вкладка</summary>
public interface ITab<T>
{
    /// <summary>Номер</summary>
    public int Id { get; set; }
    public T Content { get; }
    /// <summary>Показать вкладку и содержимое</summary>
    public void Show();
    /// <summary>Задать содержимое</summary><param name="frame">Фрейм</param>
    public void SetContent(T frame);
    /// <summary>Показать шапку вкладки</summary>
    public void ShowHead();
    /// <summary>Задатть имя вкладки</summary><param name="name"></param>
    public void SetName(string name);
    /// <summary>Задать имя вкладки</summary>
    public void SetName();

}