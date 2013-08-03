﻿using LanExchange.Model;
using System.Diagnostics;
using LanExchange.SDK;

namespace LanExchange.Presenter
{
    /// <summary>
    /// Presenter for Settings (model) and AboutForm (view).
    /// </summary>
    internal sealed class AboutPresenter
    {
        private readonly IAboutView m_View;

        public AboutPresenter(IAboutView view)
        {
            m_View = view;
        }

        public void LoadFromModel()
        {
            m_View.WebText = AboutInfo.WebSite;
            m_View.WebToolTip = GetFullWebLink();
            m_View.TwitterText = '@' + AboutInfo.Twitter;
            m_View.TwitterToolTip = GetFullTwitterLink();
            m_View.EmailText = AboutInfo.Email;
            m_View.EmailToolTip = GetFullEmailLink();
        }

        private string GetFullWebLink()
        {
            return "https://" + AboutInfo.WebSite;
        }

        private string GetFullTwitterLink()
        {
            return "https://twitter.com/" + AboutInfo.Twitter;
        }

        private string GetFullEmailLink()
        {
            return "mailto:" + AboutInfo.Email;
        }

        public void OpenWebLink()
        {
            Process.Start(GetFullWebLink());
        }

        public void OpenTwitterLink()
        {
            Process.Start(GetFullTwitterLink());
        }

        public void OpenEmailLink()
        {
            Process.Start(GetFullEmailLink());
        }
    }
}
