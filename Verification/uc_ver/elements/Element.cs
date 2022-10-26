﻿using Verification.package_ver;

namespace Verification.uc_ver
{
    public class Element : IActor
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int H { get; set; }
        public int W { get; set; }


        public Element(string id, string type, string name, string parent)
        {
            Id = id;
            Type = type;
            Name = name;
            Parent = parent;
            X = Y = int.MaxValue;
            H = W = -1;
        }
    }


}
