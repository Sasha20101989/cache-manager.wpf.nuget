using System;
using System.Collections;
using System.IO;
using System.Windows;
using File.Manager;
using Microsoft.Extensions.Options;

namespace Cache.Manager.WPF
{
    public class СacheManager : IСacheManager
    {
        private readonly IFileManager _fileService;
        private readonly AppConfig _appConfig;
        private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public СacheManager(IFileManager fileService, IOptions<AppConfig> appConfig)
        {
            _fileService = fileService;
            _appConfig = appConfig.Value;
        }

        public void PersistData()
        {
            if (Application.Current.Properties != null)
            {
                string folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);
                string fileName = _appConfig.AppPropertiesFileName;
                _fileService.Save(folderPath, fileName, Application.Current.Properties);
            }
        }

        public void RestoreData()
        {
            string folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);
            string fileName = _appConfig.AppPropertiesFileName;
            IDictionary properties = _fileService.Read<IDictionary>(folderPath, fileName);
            if (properties != null)
            {
                foreach (DictionaryEntry property in properties)
                {
                    Application.Current.Properties.Add(property.Key, property.Value);
                }
            }
        }
    }
}
