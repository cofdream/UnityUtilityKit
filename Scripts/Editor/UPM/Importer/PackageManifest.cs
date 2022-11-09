using UnityEngine;

namespace Cofdream.ToolKitEditor.UPM
{
    [System.Serializable]
    public class PackageManifest : ScriptableObject
    {
        public Package Package;

        public void AddSmallVersion()
        {
            var versions = Package.version.Split('.');
            if (versions.Length < 3) return;

            uint.TryParse(versions[0], out uint v0);
            uint.TryParse(versions[1], out uint v1);
            uint.TryParse(versions[2], out uint v2);

            v2++;

            Package.version = $"{v0}.{v1}.{v2}";
        }

    }
}