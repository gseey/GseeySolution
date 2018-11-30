using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebFileExplorer.Models;

namespace WebFileExplorer.Controllers
{
    public class HomeController : Controller
    {

        public string RootPath { get; set; }

        public HomeController(IOptions<FileConfigModel> fileConfig)
        {
            RootPath = fileConfig.Value.RootPath;
        }

        public IActionResult Index()
        {
            var fileDict = new Dictionary<string, string>();
            System.IO.Directory.GetDirectories(RootPath).ToList().ForEach(dirInfo =>
            {
                var rootPath = System.IO.Path.GetFullPath(dirInfo);
                var key = string.Format("Dir_{0}", rootPath);
                if (!fileDict.ContainsKey(key))
                {
                    fileDict.Add(key, rootPath);
                }

                System.IO.Directory.GetDirectories(rootPath).ToList().ForEach(subDirInfo =>
                {
                    rootPath = System.IO.Path.GetFullPath(subDirInfo);
                    key = string.Format("Dir_{0}", rootPath);
                    if (!fileDict.ContainsKey(key))
                    {
                        fileDict.Add(key, rootPath);
                    }
                });

                System.IO.Directory.GetFiles(rootPath).ToList().ForEach(subFileInfo =>
                {
                    rootPath = System.IO.Path.GetFullPath(subFileInfo);
                    key = string.Format("File_{0}", rootPath);
                    if (!fileDict.ContainsKey(key))
                    {
                        fileDict.Add(key, rootPath);
                    }
                });
            });

            var fileList = new List<string>();
            foreach (var item in fileDict.Values)
            {
                fileList.Add(item);
            }
            ViewData["FileList"] = fileList;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
