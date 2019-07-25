﻿namespace SQLUpdateManager.CLI.Common
{
    public enum ErrorCodes
    {
        Unknown = 0,

        InvalidCommand = 100,
        InvalidParameter,
        InvalidArgument,
        InvalidData,

        UnacceptableParameter,

        InvalidEncodingConfiguration,

        ServerIsNotConnected = 200
    }
}
