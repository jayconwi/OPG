using Newtonsoft.Json.Linq;
using opg_201910_interview.Services.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace opg_201910_interview.Services.Concrete
{
    public class ClientDirectoryService : IClientDirectoryService
    {
        private string _clientId;
        private string _directoryPath;
        private JArray _directoryFileList;

        public ClientDirectoryService()
        {

        }

        public string ClientId {
            set { _clientId = value; }
        }

        public string DirectoryPath {
            set { _directoryPath = value; }
        }

        public JArray DirectoryFileList {
            get { return _directoryFileList; }
        }

        public JArray LoadDirectoryFiles(string searchPattern, SearchOption searchOption) =>
            _directoryFileList =
                new JArray(
                    Directory
                        .GetFiles($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\{_directoryPath}", searchPattern, searchOption)
                        .Select(Path.GetFileName)
                        .Select(fileName => new JObject(new JProperty("clientId", _clientId), new JProperty("fileName", fileName)))
                );

        public void SortDirectoryFilesByNameAndDate() =>
            _directoryFileList =
                new JArray(
                    _directoryFileList
                        .OrderBy(directoryFile => directoryFile["fileName"])
                        .ThenBy(directoryFile => GetDateFromFileName(directoryFile["fileName"].Value<string>()))
                );

        public void SortUniqueMechanismOne()
        {
            List<JToken> lastTwoDirectoryFiles =
                _directoryFileList
                    .Skip(_directoryFileList.Count - 2)
                    .OrderByDescending(directoryFile => directoryFile["fileName"])
                    .ToList();

            JArray sourceDirectoryFileList = _directoryFileList;
            sourceDirectoryFileList.RemoveAt(sourceDirectoryFileList.Count - 1);
            sourceDirectoryFileList.RemoveAt(sourceDirectoryFileList.Count - 1);

            foreach (JToken file in lastTwoDirectoryFiles)
                sourceDirectoryFileList.Insert(0, file);

            _directoryFileList = new JArray(sourceDirectoryFileList);
        }

        public void SortUniqueMechanismTwo()
        {
            List<JToken> firstHalfDirectoryFiles =
                _directoryFileList
                    .Take(_directoryFileList.Count / 2)
                    .OrderByDescending(directoryFile => directoryFile["fileName"])
                    .ToList();

            List<JToken> secondHalfDirectoryFiles =
                _directoryFileList
                    .Skip(_directoryFileList.Count / 2)
                    .OrderByDescending(directoryFile => directoryFile["fileName"])
                    .ToList();

            JArray directoryFilesResult = new JArray();
            for (int i = 0; i < firstHalfDirectoryFiles.Count; i++)
            {
                directoryFilesResult.Add(firstHalfDirectoryFiles[i]);
                directoryFilesResult.Add(secondHalfDirectoryFiles[i]);
            }

            _directoryFileList = directoryFilesResult;
        }

        private DateTime GetDateFromFileName(string fileName)
        {
            int year = 0, month = 0, day = 0;
            string fileNameWithoutExtension = fileName.Replace(".xml", string.Empty);
            
            if (fileName.Contains("_"))
            {
                string dateString = fileNameWithoutExtension.Split("_")[1];
                year = Convert.ToInt16(dateString.Substring(0, 4));
                month = Convert.ToInt16(dateString.Substring(4, 2));
                day = Convert.ToInt16(dateString.Substring(6, 2));
            }
            else
            {
                year = Convert.ToInt16(fileNameWithoutExtension.Split("-")[1]);
                month = Convert.ToInt16(fileNameWithoutExtension.Split("-")[2]);
                day = Convert.ToInt16(fileNameWithoutExtension.Split("-")[3].Split(".")[0]);
            }

            return new DateTime(year, month, day);
        }
        
    }
}
