﻿using LanExchange.SDK;

namespace LanExchange.OS.Linux
{
    internal class SingleInstanceService : ISingleInstanceService
    {
        public bool CheckExists(string unicalName)
        {
            return false;
        }
    }
}
