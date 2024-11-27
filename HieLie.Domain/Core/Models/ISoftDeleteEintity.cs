namespace HieLie.Domain.Core.Models
{
    internal interface ISoftDeleteEintity
    {
        public bool IsDeleted { get; set; }
    }
}
