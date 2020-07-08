using System.IO;
using Newtonsoft.Json.Linq;

namespace opg_201910_interview.Services.Contract
{
    public interface IClientDirectoryService
    {
        public string ClientId { set; }
        public string DirectoryPath { set; }
        public JArray DirectoryFileList { get; }
        public JArray LoadDirectoryFiles(string searchPattern, SearchOption searchOption);
        public void SortDirectoryFilesByNameAndDate();
        public void SortUniqueMechanismOne();
        public void SortUniqueMechanismTwo();
    }
   
}
