﻿using Emgu.CV;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Xml;
using Verification.package_ver;
using Verification.type_definer;

namespace Verification
{
    public class Diagram
    {
        public string Name;
        public XmlElement XmlInfo;
        public Image<Bgra, byte> Image;
        public List<Mistake> Mistakes;
        public bool Verificated;
        public EDiagramTypes EType;
        public XmlDocument doc;
        private HashSet<IActor> actors;

        internal HashSet<IActor> Actors { get => actors; set => actors = value; }

        public Diagram(string name, XmlElement xmlInfo, Image<Bgra, byte> image, EDiagramTypes eType, XmlDocument doc)
        {
            Name = name;
            XmlInfo = xmlInfo;
            Image = image;
            Mistakes = new List<Mistake>();
            Verificated = false;
            EType = eType;
            this.doc = doc;
            actors = new HashSet<IActor>();
        }
    }
}
