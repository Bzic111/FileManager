namespace FileManager;

internal record struct SymbolBorder(char LeftUpCorner, char LeftDownCorner, char RightUpCorner, char RightDownCorner, char Liner, char Border)
{
    public static implicit operator (char LeftUpCorner, char LeftDownCorner, char RightUpCorner, char RightDownCorner, char Liner, char Border)(SymbolBorder value)
    {
        return (value.LeftUpCorner, value.LeftDownCorner, value.RightUpCorner, value.RightDownCorner, value.Liner, value.Border);
    }

    public static implicit operator SymbolBorder((char LeftUpCorner, char LeftDownCorner, char RightUpCorner, char RightDownCorner, char Liner, char Border) value)
    {
        return new SymbolBorder(value.LeftUpCorner, value.LeftDownCorner, value.RightUpCorner, value.RightDownCorner, value.Liner, value.Border);
    }
}