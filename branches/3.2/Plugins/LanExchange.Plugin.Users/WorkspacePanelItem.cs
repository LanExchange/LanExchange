﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using LanExchange.SDK;

namespace LanExchange.Plugin.Users
{
    public sealed class WorkspacePanelItem : PanelItemBase
    {

        public WorkspacePanelItem()
        {
            
        }

        public WorkspacePanelItem(PanelItemBase parent, string name) : base(parent)
        {
            Name = name;
        }

        [XmlAttribute]
        public override string Name { get; set; }

        public override string FullName
        {
            get { return AdsPath; }
        }

        public string AdsPath { get; set; }

        public override string ImageName
        {
            get { return string.Empty; }
        }

        public override string ImageLegendText
        {
            get { return string.Empty; }
        }

        //public override int CountColumns
        //{
        //    get { return base.CountColumns; }
        //}

        public override IComparable GetValue(int index)
        {
            switch (index)
            {
                //case 1: return AdsPath;
                default:
                    return LdapUtils.UnescapeName(Name);
            }
        }

        public override object Clone()
        {
            var result = new WorkspacePanelItem(Parent, Name);
            result.AdsPath = AdsPath;
            return result;
        }
    }
}
