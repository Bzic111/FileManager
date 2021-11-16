public interface IControlable : IFrame
{
    public void CursorMove(ref int row, ref int col, ref bool cycle, ref ConsoleKeyInfo key);
    public void SetCursorPosition(ref int row, ref int col);
    public void WriteContentString(in int index);
    public void TakeControl(ref int row, ref int col, ref bool cycle, ref ConsoleKeyInfo key);
}
