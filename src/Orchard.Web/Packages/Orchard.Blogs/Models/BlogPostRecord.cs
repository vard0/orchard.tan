using System;
using Orchard.Models.Records;

namespace Orchard.Blogs.Models {
    public class BlogPostRecord : ContentPartRecord {
        public virtual BlogRecord Blog { get; set; }
        public virtual DateTime? Published { get; set; }
    }
}