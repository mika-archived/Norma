namespace Norma.Eta.Models.Operations
{
    public class ChangeChannelOp : IOperation
    {
        public ChangeChannelOp(string channel)
        {
            Context = channel;
        }

        #region Implementation of IOperation

        public object Context { get; }

        #endregion
    }
}