﻿using System;
using System.Xml.Serialization;

namespace LanExchange.Intf.Addon
{
    [XmlType("ToolStripMenuItem")]
    public class AddonMenuItem : IEquatable<AddonMenuItem>
    {
        public AddonMenuItem()
        {
            Visible = true;
        }
        
        [XmlAttribute]
        public string Text { get; set; }

        [XmlAttribute]
        public bool Default { get; set; }

        [XmlAttribute]
        public bool Visible { get; set; }

        public string ShortcutKeys  { get; set; }
        
        public AddonObjectId ProgramRef { get; set; }
        
        [XmlIgnore]
        public AddonProgram ProgramValue { get; set; }
        
        public string ProgramArgs { get; set; }

        public bool IsSeparator
        {
            get { return string.IsNullOrEmpty(Text); }
        }
        
        public bool ShortcutPresent
        {
            get { return !string.IsNullOrEmpty(ShortcutKeys); }
        }

        public bool Equals(AddonMenuItem other)
        {
            if (Text == null)
                return other.Text == null;
            if (Text.Equals(other.Text))
                return true;
            if (ProgramValue == null || other.ProgramValue == null)
                return (ProgramValue == null) == (other.ProgramValue == null);
            if (String.Compare(ProgramValue.ExpandedFileName,
                               other.ProgramValue.ExpandedFileName,
                               StringComparison.InvariantCultureIgnoreCase) != 0)
                return false;
            return String.Compare(ProgramArgs, other.ProgramArgs, StringComparison.InvariantCulture) == 0;
        }

        public override int GetHashCode()
        {
            var result = Text == null ? 0 : Text.GetHashCode();
            result ^= ProgramValue.ExpandedFileName.GetHashCode();
            result ^= ProgramArgs.GetHashCode();
            return result;
        }
    }
}
