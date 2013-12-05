﻿using System;
using System.Xml.Serialization;

namespace LanExchange.SDK
{
    /// <summary>
    /// The ".." item
    /// </summary>
    [Serializable]
    public sealed class PanelItemDoubleDot : PanelItemBase
    {
        public PanelItemDoubleDot()
        {
            
        }
        
        public PanelItemDoubleDot(PanelItemBase parent) : base(parent)
        {
        }

        public override string ImageName
        {
            get { return PanelImageNames.DoubleDot; }
        }

        public override string ImageLegendText
        {
            get { return string.Empty; }
        }

        public override object Clone()
        {
            return new PanelItemDoubleDot(Parent);
        }

        [XmlAttribute]
        public override string Name { get; set; }
    }
}