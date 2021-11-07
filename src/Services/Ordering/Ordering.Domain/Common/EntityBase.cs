using System;

namespace Ordering.Domain.Common
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string LastModifiedby { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
