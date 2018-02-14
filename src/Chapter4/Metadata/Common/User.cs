using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.Metadata.Common
{
    public class User
    {
        public User(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
