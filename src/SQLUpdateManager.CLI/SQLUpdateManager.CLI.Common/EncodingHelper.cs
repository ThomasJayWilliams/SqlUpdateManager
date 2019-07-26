using System.Text;

namespace SQLUpdateManager.CLI.Common
{
    public static class EncodingHelper
    {
        public static Encoding GetEncoding(string encodingName)
        {
            if (encodingName == Encoding.UTF8.EncodingName)
                return Encoding.UTF8;
            if (encodingName == Encoding.ASCII.EncodingName)
                return Encoding.ASCII;
            if (encodingName == Encoding.Unicode.EncodingName)
                return Encoding.Unicode;
            if (encodingName == Encoding.UTF7.EncodingName)
                return Encoding.UTF7;
            if (encodingName == Encoding.UTF32.EncodingName)
                return Encoding.UTF32;
            else
                throw new InvalidConfigurationException(ErrorCodes.InvalidEncodingConfiguration,
                    $"Invalid encoding configuration. {encodingName} encoding does not exist.");
        }
    }
}
