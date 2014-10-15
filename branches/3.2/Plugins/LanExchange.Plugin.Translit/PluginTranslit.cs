﻿using System;
using LanExchange.SDK;

namespace LanExchange.Plugin.Translit
{
    public sealed class PluginTranslit : IPlugin
    {
        private IServiceProvider m_Provider;

        public void Initialize(IServiceProvider serviceProvider)
        {
            m_Provider = serviceProvider;

            var service = (ITranslationService)m_Provider.GetService(typeof (ITranslationService));
            if (service != null)
            {
                service.RegisterTranslit<RussianCyrillicToLatin>();
                service.RegisterTranslit<RussianCyrillicToGame>();
                service.RegisterTranslit<KazakhCyrillicToLatin>();
                service.RegisterTranslit<KazakhCyrillicToArabic>();
            }
        }
    }
}