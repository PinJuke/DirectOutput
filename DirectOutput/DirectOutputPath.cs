using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;

// <summary>
// DirectOutput is the root namespace for the DirectOutput framework.
// </summary>
namespace DirectOutput
{
    /// <summary>
    /// Static class providing access to path-related matters without having many dependencies on .NET assemblies.
    /// </summary>
    internal static class DirectOutputPath
    {
        /// <summary>
        /// Intenal implementation. See <see cref="DirectOutputHandler.GetInstallFolder"/>.
        /// </summary>
        /// <returns></returns>
        public static String GetInstallFolder()
        {
            // Get the full path to the running assembly (i.e., the DOF DLL that
            // this code is part of).  This is the full name of the DLL file,
            // with absolute path.  If this is null, return null.
            var AssemblyLocation = Assembly.GetExecutingAssembly().Location;
            if (AssemblyLocation.IsNullOrEmpty())
                return null;

            // get the path to the assembly
            var AssemblyPath = new FileInfo(AssemblyLocation).Directory.FullName;

            // Check for the existence of a Config folder in this directory.  If
            // there's no such folder, AND there's a Config folder in the parent
            // directory, assume that we're running in the new install configuration
            // with x86 and x64 subfolders for the binaries.
            var AssemblyConfigPath = new DirectoryInfo(Path.Combine(AssemblyPath, "Config"));
            var AssemblyParentConfigPath = new DirectoryInfo(Path.GetFullPath(Path.Combine(AssemblyPath, "..\\Config")));
            if (!AssemblyConfigPath.Exists && AssemblyParentConfigPath.Exists)
            {
                // new configuration with binary subfolders - the assembly is in
                // a subfolder within the install folder, so the install folder
                // is the parent of the assembly folder
                var parent = Path.GetDirectoryName(AssemblyPath);
                Log.Once("InstallFolderLoc", "Install folder lookup: assembly: {0}, install folder: {1} (PARENT of the assembly folder -> new shared x86/x64 install)".Build(AssemblyLocation, parent));
                return parent;
            }
            else
            {
                // old flat configuration - the assembly is in the install folder
                Log.Once("InstallFolderLoc", "Install folder lookup: assembly: {0}, install folder: {1} (ASSEMBLY folder -> original flat install configuration)".Build(AssemblyLocation, AssemblyPath));
                return AssemblyPath;
            }
        }

        /// <summary>
        /// Intenal implementation. See <see cref="DirectOutputHandler.GetGlobalConfigFileName(string)"/>.
        /// </summary>
        /// <returns></returns>
        public static string GetGlobalConfigFileName(string HostingApplicationName)
        {
            // Convert the host application name to a filename suffix, by
            // deleting periods and invalid file and path characters.
            string HostAppFilename = HostingApplicationName.Replace(".", "");
            foreach (char C in Path.GetInvalidFileNameChars())
                HostAppFilename = HostAppFilename.Replace(C.ToString(), "");
            foreach (char C in Path.GetInvalidPathChars())
                HostAppFilename = HostAppFilename.Replace(C.ToString(), "");

            // form the name of the application-specific config file by appending
            // the host application name suffix
            var HostAppConfigRootName = "GlobalConfig_{0}".Build(HostAppFilename);
            var HostAppConfigFileName = HostAppConfigRootName + ".xml";

            // Get the config file location.  Start with the install folder.
            var installFolder = GetInstallFolder();
            FileInfo F;
            if (installFolder == null)
            {
                // we can't identify the install folder, so default to the working
                // directory by returning the root name without a path prefix
                return HostAppConfigFileName;
            }

            // look for an .xml file in the Config folder
            F = new FileInfo(Path.Combine(installFolder, "Config", HostAppConfigFileName));
            if (F.Exists)
            {
                Log.Once("GlobalConfigLoc", "Global config file lookup: found in Config folder: " + F.FullName);
                return F.FullName;
            }

            // no luck with the .xml file; look for a shortcut (.lnk) to the file
            FileInfo LnkFile = new FileInfo(Path.Combine(installFolder, "Config", HostAppConfigRootName + ".lnk"));
            if (LnkFile.Exists)
            {
                // there's a link; resolve it
                string ResolvedLinkPath = ResolveShortcut(LnkFile);
                Log.Once("GlobalConfigLoc", "Global config file lookup: found shortcut ({0}) -> {1}".Build(LnkFile.FullName, ResolvedLinkPath));
                if (Directory.Exists(ResolvedLinkPath))
                {
                    F = new FileInfo(Path.Combine(ResolvedLinkPath, HostAppConfigFileName));
                    if (F.Exists)
                    {
                        Log.Once("GlobalConfigLoc", "Global config file lookup: found at shortcut location ({0})".Build(F.FullName));
                        return F.FullName;
                    }
                }
            }

            // try looking directly in the install folder
            F = new FileInfo(Path.Combine(installFolder, HostAppConfigFileName));
            if (F.Exists)
            {
                Log.Once("GlobalConfigLoc", "Global config file search: found in main install folder ({0})".Build(F.FullName));
                return F.FullName;
            }

            // If we still haven't found the file, give up; we still need a filename,
            // so set it to the default Config folder location if one exists, otherwise
            // the install folder.
            F = new FileInfo(Path.Combine(installFolder, "Config", HostAppConfigFileName));
            if (F.Directory.Exists)
            {
                Log.Once("GlobalConfigLoc", "Global config file search: file not found, but Config folder will be used for other file searches ({0})".Build(F.Directory.FullName));
            }
            else
            {
                Log.Once("GlobalConfigLoc", "Global config file search: Config folder ({0}) not found, using main install folder for other file searches ({1})".Build(F.Directory.FullName, installFolder));
                F = new FileInfo(Path.Combine(installFolder, HostAppConfigFileName));
            }

            // return what we found
            return F.FullName;
        }

        private static string ResolveShortcut(FileInfo ShortcutFile)
        {
            string TargetPath = "";
            try
            {
                Type WScriptShell = Type.GetTypeFromProgID("WScript.Shell");
                object Shell = Activator.CreateInstance(WScriptShell);
                object Shortcut = WScriptShell.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, Shell, new object[] { ShortcutFile.FullName });
                TargetPath = (string)Shortcut.GetType().InvokeMember("TargetPath", BindingFlags.GetProperty, null, Shortcut, null);
                Shortcut = null;
                Shell = null;
            }
            catch
            {

            }

            try
            {
                if (Directory.Exists(TargetPath))
                {
                    return TargetPath;
                }
                else if (File.Exists(TargetPath))
                {
                    return TargetPath;
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return "";
            }
        }
    }
}
