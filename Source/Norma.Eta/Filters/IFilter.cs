namespace Norma.Eta.Filters
{
    /// <summary>
    ///     名前の正規化
    /// </summary>
    public interface IFilter
    {
        string Call(string str);
    }
}