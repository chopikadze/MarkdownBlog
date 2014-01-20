﻿using System.Web.Mvc;

namespace Softumus.MarkdownBlog.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
			filters.Add(new HandleNotFoundAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}