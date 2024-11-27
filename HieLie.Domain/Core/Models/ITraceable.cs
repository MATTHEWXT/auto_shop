namespace HieLie.Domain.Core.Models
{
    internal interface ITraceable
    {
        public Guid CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid EditedBy { get; set; }
        public DateTimeOffset EditedOn { get; set; }
    }
}
