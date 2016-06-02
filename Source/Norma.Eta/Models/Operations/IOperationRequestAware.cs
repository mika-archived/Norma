namespace Norma.Eta.Models.Operations
{
    public interface IOperationRequestAware
    {
        void Invoke(IOperation operation);
    }
}