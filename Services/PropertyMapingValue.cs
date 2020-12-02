using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Services
{
    public class PropertyMapingValue
    {
        public IEnumerable<string> DesinationProperties { get; set; }
        public bool Revert { get; set; }

        public PropertyMapingValue(IEnumerable<string> destinationProperties,
            bool revert = false)
        {
            DesinationProperties = destinationProperties ??
                throw new ArgumentNullException(nameof(DesinationProperties));
            Revert = revert;
        }
    }
}
