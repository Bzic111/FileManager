namespace FileManager.Internal;

internal record struct Colors(ConsoleColor NormalBackGround, ConsoleColor SelectedBackGround, ConsoleColor NormalText, ConsoleColor SelectedText, ConsoleColor ContexMenuNormalBackGround, ConsoleColor ContexMenuSelectedBackGround, ConsoleColor SelectedContext, ConsoleColor NormalContext)
{
    public static implicit operator (
        ConsoleColor NormalBackGround,
        ConsoleColor SelectedBackGround,
        ConsoleColor NormalText,
        ConsoleColor SelectedText,
        ConsoleColor ContexMenuNormalBackGround,
        ConsoleColor ContexMenuSelectedBackGround,
        ConsoleColor SelectedContext,
        ConsoleColor NormalContext)(Colors value)
    {
        return (
            value.NormalBackGround,
            value.SelectedBackGround,
            value.NormalText,
            value.SelectedText,
            value.ContexMenuNormalBackGround,
            value.ContexMenuSelectedBackGround,
            value.SelectedContext,
            value.NormalContext);
    }

    public static implicit operator Colors((
        ConsoleColor NormalBackGround,
        ConsoleColor SelectedBackGround,
        ConsoleColor NormalText,
        ConsoleColor SelectedText,
        ConsoleColor ContexMenuNormalBackGround,
        ConsoleColor ContexMenuSelectedBackGround,
        ConsoleColor SelectedContext,
        ConsoleColor NormalContext) value)
    {
        return new Colors(
            value.NormalBackGround,
            value.SelectedBackGround,
            value.NormalText,
            value.SelectedText,
            value.ContexMenuNormalBackGround,
            value.ContexMenuSelectedBackGround,
            value.SelectedContext,
            value.NormalContext);
    }
}
