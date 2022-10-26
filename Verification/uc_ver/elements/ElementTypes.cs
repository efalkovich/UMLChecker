﻿namespace Verification.uc_ver
{
    public static class ElementTypes
    {
        public const string
            Actor = "uml:Actor",
            Association = "uml:Association",
            Precedent = "uml:UseCase",
            Package = "uml:Package",
            Include = "include",
            Extend = "extend",
            Comment = "ownedComment",
            Generalization = "generalization",
            ExtensionPoint = "extensionPoint";


        public static string[] List =
            { Actor, Association, Precedent, Package, Include, Extend, Generalization, Comment, ExtensionPoint };
    }
}
