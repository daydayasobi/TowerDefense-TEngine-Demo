using System;
using YooAsset;

namespace TEngine
{
    public static class ResourceDownloadCompat
    {
        private static IResourceModule ResourceModule => ModuleSystem.GetModule<IResourceModule>();

        public static string[] GetDefaultPackageTags()
        {
            var settings = GameResourceDownloadSettings.Instance;
            if (settings != null && settings.DefaultPackageTags != null && settings.DefaultPackageTags.Count > 0)
            {
                return settings.DefaultPackageTags.ToArray();
            }

            return new[] { "Base" };
        }

        public static ResourceDownloaderOperation CreateDefaultPackageDownloader()
        {
            return CreateResourceDownloaderByTags(GetDefaultPackageTags(), ResourceModule.DefaultPackageName);
        }

        public static ResourceDownloaderOperation CreateResourceDownloaderByTags(string[] tags, string packageName = "")
        {
            var resourceModule = ResourceModule;
            if (string.IsNullOrEmpty(packageName))
            {
                packageName = resourceModule.DefaultPackageName;
            }

            if (tags == null || tags.Length == 0)
            {
                var downloader = resourceModule.CreateResourceDownloader(packageName);
                resourceModule.Downloader = downloader;
                return downloader;
            }

            var package = YooAssets.GetPackage(packageName);
            if (package == null)
            {
                throw new InvalidOperationException($"Could not find YooAsset package '{packageName}'.");
            }

            var taggedDownloader = package.CreateResourceDownloader(tags, resourceModule.DownloadingMaxNum, resourceModule.FailedTryAgain);
            resourceModule.Downloader = taggedDownloader;
            return taggedDownloader;
        }
    }
}
