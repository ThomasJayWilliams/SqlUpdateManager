using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLUpdateManager.CLI.Common
{
    public class CommonConfiguration : ICommonConfiguration
    {
        private readonly Session _session;
        private readonly IDataRepository _dataRepo;

        public CommonConfiguration(Session session, IDataRepository dataRepo)
        {
            _session = session;
            _dataRepo = dataRepo;
        }

        public void ConfigureSession(AppConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("Config cannot be null.");

            if (string.IsNullOrEmpty(config.Core.FileEncoding))
                throw new ArgumentException("File encoding cannot be null or empty.");
            _session.Encoding = EncodingHelper.GetEncoding(config.Core.FileEncoding);

            if (!_dataRepo.Exists(CLIConstants.ConsoleThemesPath))
                ConfigureConsole(CLIConstants.DefaultThemeName);

            else
                ConfigureConsole(config.Core.Theme);
        }

        private void ConfigureConsole(string currentThemeName)
        {
            if (currentThemeName != CLIConstants.DefaultThemeName)
            {
                var themes = _dataRepo.GetData<IEnumerable<ConsoleTheme>>(CLIConstants.ConsoleThemesPath);
                var currentTheme = themes.FirstOrDefault(t => t.ThemeName == currentThemeName);

                if (currentTheme == null)
                    _session.Theme = CLIConstants.DefaultTheme;

                else
                    _session.Theme = currentTheme;
            }

            else
                _session.Theme = CLIConstants.DefaultTheme;
        }
    }
}
