// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.CosmosRepository.Providers;

/// <summary>
/// The cosmos client provider exposes a means of providing
/// an instance to the configured <see cref="CosmosClient"/> object,
/// which is shared.
/// </summary>
public interface ICosmosClientProvider
{
    /// <summary>
    /// Describe your member here.
    /// </summary>
    Task<T> UseClientAsync<T>(Func<CosmosClient, Task<T>> consume);

    /// <summary>
    /// Describe your member here.
    /// </summary>
    CosmosClient CosmosClient { get; }
}
