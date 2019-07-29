using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI.IO
{
    public class Prefix : IPrefix
    {
        private readonly Session _session;
        private readonly IOutput _output;

        public Prefix(Session session, IOutput output)
        {
            _output = output;
            _session = session;
        }

        public void PrintPrefix()
        {
            var theme = _session.Theme;

            if (_session.ConnectedServer != null)
            {
                _output.PrintColored(CLIConstants.AppName, theme.AppColor);

                _output.PrintColored(" ", RGB.NoColor);
                _output.PrintColored(
                    _session.ConnectedServer.Name.Length > 25 ?
                        $"{_session.ConnectedServer.Name.Substring(0, 25)}..." :
                        _session.ConnectedServer.Name,
                    theme.ServerColor);

                if (_session.UsedDatabase != null)
                {
                    _output.PrintColored("/", theme.DatabaseColor);
                    _output.PrintColoredLine(
                        _session.UsedDatabase.Name.Length > 25 ?
                            $"({_session.UsedDatabase.Name.Substring(0, 25)}...)" :
                            _session.UsedDatabase.Name,
                        theme.DatabaseColor);
                }

                else
                    _output.PrintEmptyLine();
            }

            else
                _output.PrintColoredLine(CLIConstants.AppName, theme.AppColor);

            _output.PrintColored($"{CLIConstants.PrefixSymbol} ", _session.Theme.TextColor);
        }
    }
}
