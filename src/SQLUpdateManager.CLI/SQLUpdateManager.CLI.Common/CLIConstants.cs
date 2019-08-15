﻿namespace SQLUpdateManager.CLI.Common
{
    public static class CLIConstants
    {
        public const string AppName = "SQLUpdateManager";
        public const string AppNameShort = "sum";

        public const string ConfigPath = "config.cfg";
        public const string RegisterPath = DataDir + "/register.json";
        public const string ErrorLogPath = LogDir + "/errorLog.log";
        public const string InfoLogPath = LogDir + "/infoLog.log";
        public const string HelpPath = AppDataDir + "/helpdoc.dat";
        public const string StoragePath = AppDataDir + "/storage.json";

        public const string LogDir = "logs";
        public const string AppDataDir = "appdata";
        public const string DataDir = "data";

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
    }
}
