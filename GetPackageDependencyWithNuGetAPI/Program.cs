using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GetPackageDependencyWithNuGetAPI
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

            PackageIdentity packageIdentity = new PackageIdentity("text2xml.lib", new NuGet.Versioning.NuGetVersion("1.1.4"));
            IPackageSearchMetadata packageMeta = await resource.GetMetadataAsync(
                packageIdentity,
                cache,
                logger,
                cancellationToken);

            Console.WriteLine($"Version: {packageMeta.Identity.Version}");
            Console.WriteLine($"Listed: {packageMeta.IsListed}");
            Console.WriteLine($"Tags: {packageMeta.Tags}");
            Console.WriteLine($"Description: {packageMeta.Description}");
            foreach (PackageDependencyGroup dependencyGroup in packageMeta.DependencySets)
            {
                Console.WriteLine($"DependencySet: {dependencyGroup}");
            }
        }
    }
}
