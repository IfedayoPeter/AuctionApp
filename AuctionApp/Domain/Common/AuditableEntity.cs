namespace AuctionApp.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; } = "SYSTEM";

        public DateTime LastModifiedOn { get; set; }

        public string LastModifiedBy { get; set; } = "SYSTEM";
    }
}
