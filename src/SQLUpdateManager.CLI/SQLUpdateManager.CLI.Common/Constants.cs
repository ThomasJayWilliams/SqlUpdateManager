namespace SQLUpdateManager.CLI.Common
{
    public static class Constants
    {
        public const string AppName = "SQLUpdateManager";
        public const string ConfigPath = "config.cfg";
        public const string RegisterPath = "register.json";
        public const string ErrorLogPath = LogDir + "/errorLog.log";
        public const string InfoLogPath = LogDir + "/infoLog.log";
        public const string HelpPath = MetadataDir + "/helpdoc.dat";

        public const string LogDir = "logs";
        public const string MetadataDir = "meta";

        public const string ConnectCommand = "connect";
        public const string UseCommand = "use";
        public const string RegisterCommand = "register";
        public const string StateCommand = "state";
        public const string HelpCommand = "help";
        public const string ConfigCommand = "config";
        public const string ExitCommand = "exit";

        public const string SaveParameter = "save";

        public const string ListParameter = "list";
        public const string SListParameters = "l";

        public const string HelpParameter = "help";
        public const string SHelpParameter = "h";

        public const string DParameterPrefix = "--";
        public const string SParameterPrefix = "-";

        public const string ASCIIArt = @"   _____  _    _  __  __ 
  / ____|| |  | ||  \/  |
 | (___  | |  | || \  / |
  \___ \ | |  | || |\/| |
  ____) || |__| || |  | |
 |_____/  \____/ |_|  |_|                         
                         ";
    }
}
