public class ControlableFrame : Frame, IControlable
{
    public ControlableFrame(Coordinates geometry, string name = "Noname") : base(geometry, name) { }
    public void CursorMove(ref int row, ref int col, ref bool cycle, ref ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.DownArrow: row = row < Geometry.Rows - 2 ? row + 1 : row; break;
            case ConsoleKey.UpArrow: row = row > 0 ? row - 1 : row; break;
            case ConsoleKey.RightArrow: col = col < Geometry.Cols - 2 ? col + 1 : col; break;
            case ConsoleKey.LeftArrow: col = col > 0 ? col - 1 : col; break;

            case ConsoleKey.End: row = Geometry.Rows - 1; break;
            case ConsoleKey.Home: row = 0; break;

            case ConsoleKey.Escape: cycle = false; break;
            case ConsoleKey.Enter: col = 0; break;

            case ConsoleKey.PageUp: break;
            case ConsoleKey.PageDown: break;

            default:
                Console.Write(key.KeyChar);
                break;
        }
        SetCursorPosition(ref row, ref col);
    }
    public void SetCursorPosition(ref int row, ref int col)
    {
        Console.SetCursorPosition(Geometry.StartCol + 1 + col, Geometry.StartRow + 1 + row);
    }
    public void WriteContentString(in int index)
    {
        if (index < Content.Length)
            Console.Write(Content[index]);
        else Console.Write("No content");
    }
    public void TakeControl(ref int row, ref int col, ref bool cycle, ref ConsoleKeyInfo key)
    {
        if (Content.Length > 0 && !string.IsNullOrEmpty(Content[0]))
        {
            for (int i = 0; i < Content.Length; i++)
            {
                WriteContentString(i);
            }
        }
        SetCursorPosition(ref row, ref col);
        do
        {
            CursorMove(ref row, ref col, ref cycle, ref key);
            key = Console.ReadKey(true);
        } while (cycle);
    }
}