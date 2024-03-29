using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.MWO
{
    public class MWOResponseList
    {
        public IEnumerable<MWOResponse> MWOsCreated { get; set; } = null!;
        public IEnumerable<MWOResponse> MWOsApproved { get; set; } = null!;
        public IEnumerable<MWOResponse> MWOsClosed { get; set; } = null!;
    }
}
