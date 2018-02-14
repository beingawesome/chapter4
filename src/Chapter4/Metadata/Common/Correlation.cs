using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.Metadata.Common
{
    public class Correlation
    {
        public Correlation(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
