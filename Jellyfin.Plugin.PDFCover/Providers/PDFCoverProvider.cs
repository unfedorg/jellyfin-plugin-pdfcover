using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Entities;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Rendering.Skia;

namespace Jellyfin.Plugin.Pdfcover.Providers
{
    /// <summary>
    /// PDF cover provider.
    /// </summary>
    public class PDFCoverProvider : IDynamicImageProvider
    {
        private readonly string[] _pdfExtensions = [".pdf"];
        private readonly ILogger<PDFCoverProvider> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PDFCoverProvider"/> class.
        /// </summary>
        /// <param name="logger">Instance of the <see cref="ILogger{PDFCoverProvider}"/> interface.</param>
        public PDFCoverProvider(ILogger<PDFCoverProvider> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public string Name => "PDF Cover Generator";

        /// <inheritdoc />
        public bool Supports(BaseItem item)
        {
            return item is Book;
        }

        /// <inheritdoc />
        public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
        {
            yield return ImageType.Primary;
        }

        /// <inheritdoc />
        public Task<DynamicImageResponse> GetImage(BaseItem item, ImageType type, CancellationToken cancellationToken)
        {
            // Check if the file is a .pdf file
            var extension = Path.GetExtension(item.Path);
            if (_pdfExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                return LoadCover(item);
            }
            else
            {
                return Task.FromResult(new DynamicImageResponse { HasImage = false });
            }
        }

        private async Task<DynamicImageResponse> LoadCover(BaseItem item)
        {
            try
            {
                // Run the synchronous PDF conversion in a background thread
                var result = await Task.Run(() =>
                {
                    using (var document = PdfDocument.Open(item.Path, SkiaRenderingParsingOptions.Instance))
                    {
                        document.AddSkiaPageFactory(); // Same as document.AddPageFactory<SKPicture, SkiaPageFactory>()

                        if (document.NumberOfPages < 1)
                        {
                            return new DynamicImageResponse { HasImage = false };
                        }

                        var ms = document.GetPageAsPng(1);
                        ms.Position = 0;
                        return new DynamicImageResponse
                        {
                            HasImage = true,
                            Stream = ms,
                            Format = ImageFormat.Png,
                        };
                    }
                }).ConfigureAwait(false);

                return result;
            }
            catch (Exception e)
            {
                // Log and return nothing
                _logger.LogError(e, "Failed to load cover from {Path}", item.Path);
                return new DynamicImageResponse { HasImage = false };
            }
        }
    }
}
