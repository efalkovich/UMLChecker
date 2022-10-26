﻿using System.Collections.Generic;

namespace Verification
{
    internal static class MistakesTypes
    {
        public const int
            WARNING = 0,
            ERROR = 1,
            FATAL = 2;


        public static Dictionary<int, string> Strings = new Dictionary<int, string>()
        {
            {WARNING, "[WARNING]" },
            {ERROR, "[ERROR]" },
            {FATAL, "[FATAL]" }
        };
    }
}
