// Guids.cs
// MUST match guids.h
using System;

namespace Haack.Encourage
{
    static class GuidList
    {
        public const string guidEncouragePackagePkgString = "f38d29e7-cd86-43d5-9c32-4be26302b55e";
        public const string guidEncouragePackageCmdSetString = "dfa60510-4edb-4142-8b57-824615d11ada";

        public static readonly Guid guidEncouragePackageCmdSet = new Guid(guidEncouragePackageCmdSetString);
    };
}