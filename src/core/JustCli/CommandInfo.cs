﻿using System;
using System.Collections.Generic;

namespace JustCli
{
    public class CommandInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ArgumentInfo> ArgumentsInfo { get; set; }
        public Type Type { get; set; }
    }
}