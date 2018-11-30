using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFileExplorer.Models
{
    public class FileInfoModel
    {
        public Dictionary<string, FileInfoItemModel> DirDict { get; set; }

        public Dictionary<string, FileInfoItemModel> FileDict { get; set; }

        public FileInfoModel()
        {
            DirDict = new Dictionary<string, FileInfoItemModel>();
            FileDict = new Dictionary<string, FileInfoItemModel>();
        }
    }

    public class FileInfoItemModel
    {
        public string FileFullName { get; set; }

        public string FilePath { get; set; }
    }
}
