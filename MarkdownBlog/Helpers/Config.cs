using System.IO;
using System.Linq;
using Softumus.MarkdownBlog.Models;

namespace Softumus.MarkdownBlog
{
    public static class Config
    {
        public static string Title 
        {
            get { return Get("title"); }
        }

        public static string GA
        {
            get { return Get("google.analytics"); }
        }

        public static string Get(string id)
        {
            var configFileName = Page.GetPath("config");
            var line = File
                .ReadAllLines(configFileName)
                .SingleOrDefault(p => p.StartsWith(id + ":"));

            if (line == null)
                return null;

            return line.Substring(line.IndexOf(':') + 1).Trim();
        }
    }
}