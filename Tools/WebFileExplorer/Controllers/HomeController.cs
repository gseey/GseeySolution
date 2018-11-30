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

        public IActionResult Index(string path)
        {
            var filePath = string.IsNullOrEmpty(path) ? RootPath : path;

            var model = new FileInfoModel();

            if (System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.GetDirectories(filePath).ToList().ForEach(dirInfo =>
                {
                    var rootPath = System.IO.Path.GetFullPath(dirInfo);
                    var key = string.Format("Dir_{0}", rootPath);
                    if (!model.DirDict.ContainsKey(key))
                    {
                        model.DirDict.Add(key, new FileInfoItemModel { FileFullName = System.IO.Path.GetDirectoryName(rootPath), FilePath = rootPath });
                    }
                });

                System.IO.Directory.GetFiles(filePath).ToList().ForEach(fileInfo =>
                {
                    var rootPath = System.IO.Path.GetFullPath(fileInfo);
                    var key = string.Format("File_{0}", rootPath);
                    if (!model.FileDict.ContainsKey(key))
                    {
                        model.FileDict.Add(key, new FileInfoItemModel { FileFullName = System.IO.Path.GetFileNameWithoutExtension(rootPath), FilePath = rootPath });
                    }
                });
            }
            ViewData["FileModel"] = model;

            var fileList = new List<FileInfoItemModel>();
            foreach (var item in model.DirDict.Values)
            {
                fileList.Add(item);
            }
            foreach (var item in model.FileDict.Values)
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
