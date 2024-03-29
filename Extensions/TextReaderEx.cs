﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace FileManager.Extensions
{
    public static class TextReaderEx
    {
        public static IEnumerable<string> EnumLines(this TextReader reader)
        {
            while (reader.ReadLine() is { /*Length: >5*/ } line)
                yield return line;

            //var line = reader.ReadLine();
            //while (line != null && line.Length > 5)
            //{
            //    yield return line;
            //    line = reader.ReadLine();
            //}
        }
    }
}