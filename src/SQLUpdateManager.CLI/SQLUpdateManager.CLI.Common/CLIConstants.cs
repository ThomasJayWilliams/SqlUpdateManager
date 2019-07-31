namespace SQLUpdateManager.CLI.Common
{
    public static class CLIConstants
    {
        public const string AppName = "SQLUpdateManager";
        public const string AppNameShort = "sum";
        public const string DefaultThemeName = "default";

        public const string ConfigPath = "config.cfg";
        public const string RegisterPath = DataDir + "/register.json";
        public const string ErrorLogPath = LogDir + "/errorLog.log";
        public const string InfoLogPath = LogDir + "/infoLog.log";
        public const string HelpPath = AppDataDir + "/helpdoc.dat";
        public const string ConsoleThemesPath = AppDataDir + "/themes.json";
        public const string StoragePath = AppDataDir + "/storage.json";

        public const string LogDir = "logs";
        public const string AppDataDir = "appdata";
        public const string DataDir = "data";

        public const string ConnectCommand = "connect";
        public const string UseCommand = "use";
        public const string RegisterCommand = "register";
        public const string StateCommand = "state";
        public const string HelpCommand = "help";
        public const string ConfigCommand = "config";
        public const string ExitCommand = "exit";
        public const string StorageCommand = "storage";

        public const string SaveParameter = "save";

        public const string DeleteParameter = "delete";
        public const string SDeleteParameter = "d";

        public const string ListParameter = "list";
        public const string SListParameters = "l";

        public const string HelpParameter = "help";
        public const string SHelpParameter = "h";

        public const string DParameterPrefix = "--";
        public const string SParameterPrefix = "-";

        public const string PrefixSymbol = "$";

        public static string[] ASCIIArt = new string[]
            {
                @"   _____  _    _  __  __ ",
                @"  / ____|| |  | ||  \/  |",
                @" | (___  | |  | || \  / |",
                @"  \___ \ | |  | || |\/| |",
                @"  ____) || |__| || |  | |",
                @" |_____/  \____/ |_|  |_|"
            };
        public static RGB ASCIIColor = new RGB(255, 228, 117);

        public static ConsoleTheme DefaultTheme = new ConsoleTheme
        {
            ThemeName = DefaultThemeName,
            AppColor = new RGB(145, 255, 122),
            ErrorColor = new RGB(192, 57, 37),
            DatabaseColor = new RGB(243, 156, 18),
            ServerColor = new RGB(142, 68, 128),
            ProcedureColor = new RGB(122, 140, 83),
            TextColor = new RGB(236, 240, 241),
            InfoColor = new RGB(34, 174, 96),
            InputColor = new RGB(236, 240, 241)
        };
    }
}
