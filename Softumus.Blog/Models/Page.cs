using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using MarkdownSharp;

namespace Softumus.Blog.Models
{
    public class Page
    {
        private const string PagesPath = "~/pages/";
        private static readonly int _DateTimePrefixLength = "yyyymmdd".Length;
        private static readonly string _BlogEntriesPrefix = new string('?', _DateTimePrefixLength) + "-";

        public static ICollection<PageDescription> GetAll()
        {
            return GetBlogEntries()
                .Select(GetDescription)
                .OrderByDescending(p => p.DateTime)
                .ToArray();
        }

        public static PageModel GetByDate(string date)
        {
            var fileName = GetFileName(date);

            return Get(fileName);
        }

        public static PageModel Get(string fileName)
        {
            return new PageModel
            {
                Description = GetDescription(fileName),
                Content = GetContent(fileName)
            };
        }
    
        public static PageModel GetStatic(string fileName)
        {
            return new PageModel
            {
                Description = new PageDescription
                {
                    Title = Path.GetFileNameWithoutExtension(fileName)
                },
                Content = GetContent(fileName)
            };
        }

        private static string GetContent(string fileName)
        {
            var content = File.ReadAllText(fileName);
            return new Markdown().Transform(content);
        }
    
        private static PageDescription GetDescription(string fileName)
        {
            var fn = Path.GetFileNameWithoutExtension(fileName);
            var date = fn.Substring(0, _DateTimePrefixLength);
            var title = fn.Substring(_DateTimePrefixLength + 1);

            return new PageDescription
            {
                DateTime = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture),
                Title = title,
                Url = date + "/" + Urlize(title)
            };
        }

        private static string Urlize(string s)
        {
            return s.Replace(" ", "-");
        }

        private static string GetFileName(string date)
        {
            return Directory.GetFiles(GetPath(), date + "*.*").Single();
        }

        private static string GetPath(string file = "")
        {
            return HostingEnvironment.MapPath(Path.Combine(PagesPath, file));
        }

        public static PageModel GetLatest()
        {
            var filename = GetBlogEntries()
                .OrderByDescending(p => p)
                .First();

            return Get(filename);
        }

        private static ICollection<string> GetBlogEntries()
        {
            return Directory.GetFiles(GetPath(), _BlogEntriesPrefix + "*.*");
        }

        public static PageModel GetByName(string name)
        {
			var fn = Directory.GetFiles(GetPath(), name + ".*").SingleOrDefault();
			if (fn == null)
				throw new HttpException(404, "File not found");
            return GetStatic(fn);
        }
    }
}