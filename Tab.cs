class Tab<T> : ITab<T> where T : IFrame, IControlable
{
    private string[] _head { get => new string[] { "╔══════════╗", $"║{Name}║", "╩══════════╩" }; }
    public int Id { get; set; }
    public Coordinates Geometry { get; private set; }
    public T Content { get; private set; }
    public string Name { get; set; }
    //private SymbolBorder _symbols { get; set; }

    private Tab(string name) => Name = UseAsName(name);
    private Tab(string name, T frame) : this(name) => Content = frame;
    public Tab(T frame) : this(frame.Geometry, frame) { }
    public Tab(Coordinates geometry, T frame) : this(frame.Name, frame)
    {
        Geometry = geometry;
        Content.Move(2, 0);
    }

    public void SetName() => Name = Content != null ? UseAsName(Content.Name) : "noname";
    public void SetName(string name) => Name = UseAsName(name);
    public void SetContent(T frame) => Content = frame;
    public void ShowHead()
    {
        for (int i = 0; i < 3; i++)
        {
            Console.SetCursorPosition((Id * _head[0].Length) + 1, i);
            Console.Write(_head[i]);
        }
    }
    public void Show()
    {
        if (Console.WindowWidth <= Geometry.StartCol + Geometry.Cols && Console.WindowHeight <= Geometry.StartRow + Geometry.Rows + 8)
        {
            Console.SetWindowSize(Geometry.StartCol + Geometry.Cols + 1, Geometry.StartRow + Geometry.Rows + 8);
        }
        Content.Show(false);
        ShowHead();
    }

    private static string UseAsName(string str) => str.Length >= 10 ? $"{str.Remove(9)}~" : $"{str.PadRight(9, '_')}~";
}