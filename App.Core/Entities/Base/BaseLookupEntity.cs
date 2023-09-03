namespace App.Core.Entities.Base
{
    public class BaseLookupEntity : BaseNameEntity
    {
        //  [StringLength(500, MinimumLength = 10)]
        public virtual string Description { get; set; }
        //  [StringLength(500, MinimumLength = 10)]
        public virtual string DescriptionAr { get; set; }
    }
}
