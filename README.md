# MarkdownBlog

MarkdownBlog is a blog engine that uses [Markdown](http://daringfireball.net/projects/markdown/) syntax for content files. You can see a demo of its usage [here](george.softumus.com).

## Deployment

* Deploy a project to your host provider
* Put all your content to `./posts` folder

## How to's

All your custom files should be created in `./posts` folder.

### Configure the site

Create `config` file with content:

	title: *Your blog title*

### Create a blog post

Just create a file `yyyymmdd-Blog post name.md`. `yyyymmdd` here is a date of your post.

### Create a static page

Static pages are pages that are not listed in "All posts". To create one, create a file `YourPageName.md`. You can access this page in your blog by `http://example.com/YourPageName`.

Note: You should create at least one static page - `About`.

## Technology stack

* [.NET 4.0](http://www.microsoft.com/net) (works on [Mono](http://www.mono-project.com/)/Linux too)
* [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4)
* [Markdown](http://daringfireball.net/projects/markdown/)
