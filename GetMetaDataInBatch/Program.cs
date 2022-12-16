using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GetMetaDataInBatch
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            PackageMetadataResource resource = await repository.GetResourceAsync<PackageMetadataResource>();

            IEnumerable<IPackageSearchMetadata> packages = await resource.GetMetadataAsync(
                "Newtonsoft.Json",
                includePrerelease: true,
                includeUnlisted: false,
                cache,
                logger,
                cancellationToken);

            foreach (IPackageSearchMetadata package in packages)
            {
                Console.WriteLine($"Version: {package.Identity.Version}");
                Console.WriteLine($"Listed: {package.IsListed}");
                Console.WriteLine($"Tags: {package.Tags}");
                Console.WriteLine($"Description: {package.Description}");

                foreach (PackageDependencyGroup dependencyGroup in package.DependencySets)
                {
                    Console.WriteLine($"DependencySet: {dependencyGroup}");
                }
            }
        }
    }
}
