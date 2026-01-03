using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Jellyfin.Plugin.Pdfcover.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Pdfcover;

/// <summary>
/// Plugin entrypoint.
/// The main plugin.
/// </summary>
public class Plugin : BasePlugin<PluginConfiguration>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin"/> class.
    /// </summary>
    /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
    /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
      Instance = this;

      NativeLibrary.SetDllImportResolver(typeof(PDFtoImage.Conversion).Assembly, (libraryName, assembly, searchPath) =>
        {
            if (libraryName == "pdfium")
            {
                string arch = RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "arm64" : "x64";
                string osPrefix = string.Empty;
                string libName = string.Empty;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    osPrefix = "linux";
                    libName = "libpdfium.so";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    osPrefix = "osx";
                    libName = "libpdfium.dylib";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    osPrefix = "win";
                    libName = "pdfium.dll.win";
                }

                if (!string.IsNullOrEmpty(osPrefix))
                {
                    string rid = $"{osPrefix}-{arch}";
                    string pluginFolder = Path.GetDirectoryName(typeof(Plugin).Assembly.Location)!;
                    string nativePath = Path.Join(pluginFolder, "runtimes", rid, "native", libName);

                    if (File.Exists(nativePath))
                    {
                        return NativeLibrary.Load(nativePath);
                    }
                }
            }

            return IntPtr.Zero;
        });
    }

    /// <inheritdoc />
    public override string Name => "PDFCover";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("9e6b04c1-2181-4089-bce8-a8f0c35cef17");

    /// <summary>
    /// Gets the current plugin instance.
    /// </summary>
    public static Plugin? Instance { get; private set; }
}
