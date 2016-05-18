namespace Norma.Models.Config
{
    // 内部でのみ使用
    internal class InternalConfig
    {
        public bool IsTopMost { get; set; }

        public InternalConfig()
        {
            IsTopMost = false;
        }
    }
}