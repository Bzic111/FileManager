using FileManager.Old;

[Serializable]
public class LastState
{
    public List<Tab> Tabs;
    public int tabIndexer = 0;
    public LastState()
    {

    }
    public LastState(List<Tab> tabs)
    {
        Tabs = tabs;
    }
}
