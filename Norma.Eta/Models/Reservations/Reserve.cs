namespace Norma.Eta.Models.Reservations
{
    public abstract class Reserve
    {
        public bool IsEnable { get; set; }

        protected Reserve()
        {
            IsEnable = true;
        }
    }
}