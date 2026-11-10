using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jnvp.Platform.Caching.Redis;

internal sealed class RedisTagStore : ITagStore
{
    private readonly IDatabase database;
}
