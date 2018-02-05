﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace CMF_Editor.Classes
{
    class FileType
    {
        public string Description { get; }
        public BitmapImage Icon { get; }

        public FileType(string desc, BitmapImage image = null)
        {
            this.Description = desc;
            this.Icon = image;
        }

        public override string ToString()
        {
            return this.Description;
        }


        public static readonly FileType GraphicsObject = new FileType("Graphics Object");
        public static readonly FileType Image = new FileType("Image/Texture");
        public static readonly FileType Script = new FileType("Script");
        public static readonly FileType Effect = new FileType("Effect");
        public static readonly FileType Sound = new FileType("Sound");
        public static readonly FileType Unknown = new FileType("Unknown");

        private readonly static Dictionary<string, FileType> dict_exts = CreateDict();

        static Dictionary<string, FileType> CreateDict()
        {
            Dictionary<string, FileType> result = new Dictionary<string, FileType>(StringComparer.OrdinalIgnoreCase);

            // Graphics Object
            result.Add("x", GraphicsObject);

            // Image or Texture
            result.Add("dds", Image);
            result.Add("png", Image);
            result.Add("tga", Image);
            result.Add("bmp", Image);
            result.Add("psd", Image);

            // Script
            result.Add("lua", Script);

            // Effect (shader???)
            result.Add("fx", Effect);

            // Sound
            result.Add("ogg", Sound);
            result.Add("wav", Sound);

            // The rest
            result.Add(string.Empty, Unknown);
            return result;
        }

        public static FileType DetermineByFilename(string filename) => DetermineByExtension(Path.GetExtension(filename));

        public static FileType DetermineByExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return dict_exts[string.Empty];
            if (extension.StartsWith("."))
                extension = extension.Remove(0, 1);
            if (dict_exts.ContainsKey(extension))
                return dict_exts[extension];
            else
                return dict_exts[string.Empty];
        }

        public static BitmapImage GetIconByFilename(string filename) => GetIconByExtension(Path.GetExtension(filename));

        public static BitmapImage GetIconByExtension(string extension) => DetermineByExtension(extension).Icon;

        static void asdasasd()
        {
            Dictionary<string, string> cacheList = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string asd;
            string path = @"E:\Entertainment\NexonGames\Closers\DAT";
            foreach (string dir in System.IO.Directory.EnumerateDirectories(path, "*", System.IO.SearchOption.TopDirectoryOnly))
                foreach (string file in System.IO.Directory.EnumerateFiles(dir, "*", System.IO.SearchOption.AllDirectories))
                {
                    try
                    {
                        using (Leayal.Closers.CMF.CMFArchive ar = Leayal.Closers.CMF.CMFArchive.Read(file))
                        {
                            foreach (Leayal.Closers.CMF.CMFEntry entry in ar.Entries)
                            {
                                asd = System.IO.Path.GetExtension(entry.FileName);
                                if (!cacheList.ContainsKey(asd))
                                    cacheList.Add(asd, entry.FileName);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            if (cacheList.Count > 0)
                using (System.IO.StreamWriter sr = System.IO.File.CreateText("total exts.txt"))
                {
                    foreach (var item in cacheList)
                        sr.WriteLine($"{item.Key}: {item.Value}");
                    sr.Flush();
                }
        }
    }
}
