namespace Helpdesk.Models.Base;

public abstract class SoftDeleteEntity : BaseEntity
{
    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }
}