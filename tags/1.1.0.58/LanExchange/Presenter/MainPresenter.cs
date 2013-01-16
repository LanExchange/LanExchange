﻿using LanExchange.View;

namespace LanExchange.Presenter
{
    public class MainPresenter
    {
        private static MainPresenter m_Instance;
        private readonly IMainView m_View;

        public MainPresenter(IMainView view)
        {
            m_Instance = this;
            m_View = view;
        }

        public static MainPresenter Instance
        {
            get
            {
                return m_Instance;
            }
        }

        public IMainView MainView
        {
            get
            {
                return m_View;
            }
        }

        public PagesPresenter Pages { get; set; }
    }
}
