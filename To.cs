namespace FileManager
{
    public partial class Frame
    {
        /// <summary>Ключи для переходов внутри списка дерева</summary>
        public enum To
        {
            StepBack,
            StepForward,
            NextPage,
            PreviousPage,
            FirstPage,
            LastPage,
            StepUp,
            StepDown
        }
    }
}
