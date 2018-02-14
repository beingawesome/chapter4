using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Chapter4.Metadata
{
    public class MetadataOptions
    {
        public string UserIdClaim { get; set; } = ClaimTypes.NameIdentifier;
    }
}
