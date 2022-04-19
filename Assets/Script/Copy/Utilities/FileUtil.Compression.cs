#if UNITY_ANDROID && (CSHARP_ZIP || (NET_STANDARD_2_0 && !UNITY_2019_1_OR_NEWER))
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

namespace CFM.Framework.Utilities
{
    public class CompressionZipAccessor : FileUtil.IZipAccessor
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnInitialized()
        {
            FileUtil.Register(new CompressionZipAccessor());
        }

        private object _lock = new object();

        private Dictionary<string, ZipArchive> zipArchives = new Dictionary<string, ZipArchive>();

        protected string GetCompressedFileName(string url)
        {
            url = Regex.Replace(url, @"^jar:file:///", "");
            return url.Substring(0, url.LastIndexOf("!"));
        }

        protected string GetCompressedEntryName(string url)
        {
            return url.Substring(url.LastIndexOf("!") + 2);
        }

        protected ZipArchive GetZipArchive(string path)
        {
            lock (_lock)
            {
                ZipArchive zip;
                if (zipArchives.TryGetValue(path, out zip))
                    return zip;

                zip = new ZipArchive(File.OpenRead(path));
                zipArchives.Add(path, zip);
                return zip;
            }
        }

        public int Priority { get { return 100; } }

        public bool Exists(string path)
        {
            try
            {
                var zipFilename = GetCompressedFileName(path);
                string entryName = GetCompressedEntryName(path);
                var zip = GetZipArchive(zipFilename);
                if (zip == null)
                    return false;
                if (zip.GetEntry(entryName) == null)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Stream OpenRead(string path)
        {
            var zipFilename = GetCompressedFileName(path);
            string entryName = GetCompressedEntryName(path);
            var zip = GetZipArchive(zipFilename);
            if (zip == null)
                throw new FileNotFoundException(path);

            var entry = zip.GetEntry(entryName);
            if (entry == null)
                throw new FileNotFoundException(path);
            return entry.Open();
        }

        public bool Support(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            string fullname = path.ToLower();
            if ((fullname.IndexOf(".apk") > 0 || fullname.IndexOf(".obb") > 0) && fullname.LastIndexOf("!/assets/") > 0)
                return true;

            return false;
        }
    }
}
#endif
