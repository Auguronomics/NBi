﻿using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;

namespace NBi.Xml.Decoration.Command
{
    public abstract class DecorationCommandXml : IDecorationCommand
    {
        [XmlIgnore()]
        public virtual Settings.SettingsXml Settings { get; set; }
    }
}
