﻿using LanExchange.OS.Windows.Utils;
using LanExchange.SDK;

namespace LanExchange.OS.Windows
{
    internal class SingleInstanceService : ISingleInstanceService
    {
        public bool CheckExists(string unicalName)
        {
            return SingleInstanceCheck.CheckExists(unicalName);
        }
    }
}
