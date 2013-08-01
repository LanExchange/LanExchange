﻿using System;
using System.Net;
using LanExchange.SDK;

namespace LanExchange.Plugin.Network.Panel
{
    public class ComputerPanelItem : PanelItemBase, IWmiComputer
    {
        public const string ID = "{F842AE25-A56E-41D5-BF37-0AD1C207F79B}";
        
        private readonly ServerInfo m_SI;

        /// <summary>
        /// Constructor creates ComputerPanelItem from <see cref="ServerInfo"/> object.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public ComputerPanelItem(PanelItemBase parent, ServerInfo si) : base(parent)
        {
            if (si == null)
                throw new ArgumentNullException("si");
            m_SI = si;
            Comment = m_SI.Comment;
            IsPingable = true;
        }

        public ComputerPanelItem(PanelItemBase parent, string name) : base(parent)
        {
            m_SI = new ServerInfo { Name = name };
            Comment = String.Empty;
            IsPingable = true;
        }

        public override string Name
        {
            get { return m_SI.Name; }
            set { m_SI.Name = value; }
        }

        public string Comment { get; set; }

        public ServerInfo SI
        {
            get { return m_SI; }
        }

        public override int CountColumns
        {
            get { return 3; }
        }

        public override PanelColumnHeader CreateColumnHeader(int index)
        {
            PanelColumnHeader result = null;
            switch (index)
            {
                case 0:
                    result = new PanelColumnHeader("Network name");
                    break;
                case 1:
                    result = new PanelColumnHeader("Description", 250);
                    break;
                case 2:
                    result = new PanelColumnHeader("OS version");
                    result.Visible = false;
                    break;
                //case 3:
                //    result = new PanelColumnHeader("IP address");
                //    result.Visible = false;
                //    break;
            }
            return result;
        }

        public override IComparable this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Name;
                    case 1:
                        return Comment;
                    case 2:
                        return m_SI.Version();
                    //case 3:
                    //    var iph = Dns.GetHostEntry(Name);
                    //    return iph.AddressList[0].ToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override string ImageName
        {
            get
            {
                if (Name == s_DoubleDot)
                    return PanelImageNames.DoubleDot;
                return IsPingable ? PanelImageNames.ComputerNormal : PanelImageNames.ComputerDisabled;
            }
        }

        public override string ImageLegendText
        {
            get
            {
                switch (ImageName)
                {
                    case PanelImageNames.ComputerNormal:
                        return "Computer was found in local area network.";
                    case PanelImageNames.ComputerDisabled:
                        return "Computer does not reachable via PING.";
                    default:
                        return string.Empty;
                }
            }
        }

        public override string ToolTipText
        {
            get
            {
                return String.Format("{0}\n{1}\n{2}", Comment, m_SI.Version(), m_SI.GetTopicalityText());
            }
        }

        public bool IsPingable { get; set; }

        public override string ToString()
        {
            return @"\\" + base.ToString();
        }
    }
}
