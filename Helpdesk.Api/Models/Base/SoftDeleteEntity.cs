namespace Helpdesk.Models.Base;

public abstract class SoftDeleteEntity : BaseEntity
{
    public DateTime? DeletedAt { get; set; }
}