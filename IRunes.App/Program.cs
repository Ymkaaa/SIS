﻿using SIS.MvcFramework;
using SIS.MvcFramework.Mapping;
using System;
using System.Collections.Generic;

namespace IRunes.App
{
    public static class Program
    {
        public static void Main()
        {
            WebHost.Start(new Startup());
        }
    }
}
