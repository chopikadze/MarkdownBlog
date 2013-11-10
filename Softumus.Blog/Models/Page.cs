using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Softumus.Blog.Models
{
    public class Page
    {
        private const string PagesPath = "~/pages/";
        private static readonly int _DateTimePrefixLength = "yyyymmdd".Length;

        public static ICollection<PageDescription> GetAll()
        {
            return Directory.GetFiles(GetPath())
                .Select(GetDescription)
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
                Content = File.ReadAllText(fileName)
            };
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
            var filename = Directory.GetFiles(GetPath())
                .OrderByDescending(p => p)
                .First();

            return Get(filename);
        }
    }
}