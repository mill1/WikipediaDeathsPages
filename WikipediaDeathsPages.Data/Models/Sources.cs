using System.Collections.Generic;

namespace WikipediaDeathsPages.Data.Models
{
    public partial class Sources
    {
        public Sources()
        {
            References = new HashSet<References>();
        }

        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<References> References { get; set; }
    }
}
