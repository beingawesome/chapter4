using Chapter4.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.Queries.Messaging
{
    public interface IQueryMetadataFactory
    {
        IMetadata Create<TQuery, TResult>(TQuery command)
            where TQuery : Query<TResult>;
    }
}
