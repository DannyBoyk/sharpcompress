﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SharpCompress.Common.GZip
{
    public class GZipEntry : Entry
    {
        private readonly GZipFilePart _filePart;

        internal GZipEntry(GZipFilePart filePart)
        {
            _filePart = filePart;
        }

        public override CompressionType CompressionType => CompressionType.GZip;

        public override long Crc => _filePart.Crc ?? 0;

        public override string Key => _filePart.FilePartName ?? string.Empty;

        public override string? LinkTarget => null;

        public override long CompressedSize => 0;

        public override long Size => _filePart.UncompressedSize ?? 0;

        public override DateTime? LastModifiedTime => _filePart.DateModified;

        public override DateTime? CreatedTime => null;

        public override DateTime? LastAccessedTime => null;

        public override DateTime? ArchivedTime => null;

        public override bool IsEncrypted => false;

        public override bool IsDirectory => false;

        public override bool IsSplitAfter => false;

        internal override IEnumerable<FilePart> Parts => _filePart.AsEnumerable<FilePart>();

        internal static async IAsyncEnumerable<GZipEntry> GetEntries(Stream stream, OptionsBase options,
                                                                     [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var part = new GZipFilePart(options.ArchiveEncoding);
            await part.Initialize(stream, cancellationToken);
            yield return new GZipEntry(part);
        }
    }
}