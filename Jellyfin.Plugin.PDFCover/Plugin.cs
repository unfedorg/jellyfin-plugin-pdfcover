using System;
using System.Collections.Generic;
using System.Globalization;
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
    }

    /// <inheritdoc />
    public override string Name => "PDF Cover";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("9e6b04c1-2181-4089-bce8-a8f0c35cef17");

    /// <summary>
    /// Gets the current plugin instance.
    /// </summary>
    public static Plugin? Instance { get; private set; }
}
