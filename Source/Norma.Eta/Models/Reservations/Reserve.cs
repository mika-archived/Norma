namespace Norma.Eta.Models.Reservations
{
#pragma warning disable CS0659 // 型は Object.Equals(object o) をオーバーライドしますが、Object.GetHashCode() をオーバーライドしません

    public abstract class Reserve
#pragma warning restore CS0659 // 型は Object.Equals(object o) をオーバーライドしますが、Object.GetHashCode() をオーバーライドしません
    {
        public bool IsEnable { get; set; }

        public int Id { get; set; }

        public string Type { get; set; }

        protected Reserve()
        {
            IsEnable = true;
        }

        #region Overrides of Object

#pragma warning disable 659

        public override bool Equals(object obj)
#pragma warning restore 659
        {
            return (obj as Reserve)?.Id == Id;
        }

        #endregion
    }
}