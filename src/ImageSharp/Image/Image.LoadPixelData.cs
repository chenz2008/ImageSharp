﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp
{
    /// <content>
    /// Adds static methods allowing the creation of new image from raw pixel data.
    /// </content>
    public static partial class Image
    {
        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the raw <typeparamref name="TPixel"/> data.
        /// </summary>
        /// <param name="data">The byte array containing image data.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> LoadPixelData<TPixel>(TPixel[] data, int width, int height)
            where TPixel : struct, IPixel<TPixel>
            => LoadPixelData(Configuration.Default, data, width, height);

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the raw <typeparamref name="TPixel"/> data.
        /// </summary>
        /// <param name="data">The byte array containing image data.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        private static Image<TPixel> LoadPixelData<TPixel>(Span<TPixel> data, int width, int height)
            where TPixel : struct, IPixel<TPixel>
            => LoadPixelData(Configuration.Default, data, width, height);

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given byte array in <typeparamref name="TPixel"/> format.
        /// </summary>
        /// <param name="data">The byte array containing image data.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> LoadPixelData<TPixel>(byte[] data, int width, int height)
            where TPixel : struct, IPixel<TPixel>
            => LoadPixelData<TPixel>(Configuration.Default, data, width, height);

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given byte array in <typeparamref name="TPixel"/> format.
        /// </summary>
        /// <param name="data">The byte array containing image data.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        private static Image<TPixel> LoadPixelData<TPixel>(Span<byte> data, int width, int height)
            where TPixel : struct, IPixel<TPixel>
            => LoadPixelData<TPixel>(Configuration.Default, data, width, height);

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given byte array in <typeparamref name="TPixel"/> format.
        /// </summary>
        /// <param name="config">The config for the decoder.</param>
        /// <param name="data">The byte array containing image data.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> LoadPixelData<TPixel>(Configuration config, byte[] data, int width, int height)
            where TPixel : struct, IPixel<TPixel>
            => LoadPixelData(config, new Span<byte>(data).NonPortableCast<byte, TPixel>(), width, height);

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given byte array in <typeparamref name="TPixel"/> format.
        /// </summary>
        /// <param name="config">The config for the decoder.</param>
        /// <param name="data">The byte array containing image data.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        private static Image<TPixel> LoadPixelData<TPixel>(Configuration config, Span<byte> data, int width, int height)
            where TPixel : struct, IPixel<TPixel>
            => LoadPixelData(config, data.NonPortableCast<byte, TPixel>(), width, height);

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the raw <typeparamref name="TPixel"/> data.
        /// </summary>
        /// <param name="config">The config for the decoder.</param>
        /// <param name="data">The Span containing the image Pixel data.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> LoadPixelData<TPixel>(Configuration config, TPixel[] data, int width, int height)
            where TPixel : struct, IPixel<TPixel>
        {
            int count = width * height;
            Guard.MustBeGreaterThanOrEqualTo(data.Length, count, nameof(data));

            var image = new Image<TPixel>(config, width, height);
            SpanHelper.Copy(data, image.GetPixelSpan(), count);

            return image;
        }

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the raw <typeparamref name="TPixel"/> data.
        /// </summary>
        /// <param name="config">The config for the decoder.</param>
        /// <param name="data">The Span containing the image Pixel data.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        private static Image<TPixel> LoadPixelData<TPixel>(Configuration config, Span<TPixel> data, int width, int height)
            where TPixel : struct, IPixel<TPixel>
        {
            int count = width * height;
            Guard.MustBeGreaterThanOrEqualTo(data.Length, count, nameof(data));

            var image = new Image<TPixel>(config, width, height);
            SpanHelper.Copy(data, image.Frames.RootFrame.GetPixelSpan(), count);

            return image;
        }
    }
}