using Chapter4.Commands;
using Chapter4.Commands.Messaging;
using Chapter4.Metadata.Common;
using Chapter4.Queries;
using Chapter4.Queries.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;

namespace Chapter4.Metadata
{
    internal class MetadataFactory :
        ICommandMetadataFactory,
        IQueryMetadataFactory
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly MetadataOptions _options;

        public MetadataFactory(IHttpContextAccessor accessor, IOptions<MetadataOptions> options)
        {
            _accessor = accessor;
            _options = options.Value;
        }

        public IMetadata Create<TQuery, TResult>(TQuery command) where TQuery : Query<TResult> => Create();

        public IMetadata Create<TCommand>(TCommand command) where TCommand : Command => Create();

        private IMetadata Create()
        {
            var result = new Metadata.Dynamic.Metadata();

            var context = _accessor.HttpContext;

            var userId = context.User.FindFirst(_options.UserIdClaim)?.Value;
            var traceIdentifier = context.TraceIdentifier;

            result.Set(new User(userId));
            result.Set(new Correlation(traceIdentifier));

            return result;
        }
    }
}
