using System;

namespace OpenWrap.Dependencies
{
    public abstract class VersionVertice
    {
        public Version Version { get; set; }

        public VersionVertice(Version version)
        {
            Version = version;
        }
        public abstract bool IsCompatibleWith(Version version);
    }
}