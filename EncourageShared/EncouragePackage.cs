using System.Runtime.InteropServices;
using Haack.Encourage.Shared.Options;
using Microsoft.VisualStudio.Shell;

namespace Haack.Encourage.Shared
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidEncouragePackagePkgString)]
    [ProvideOptionPage(typeof(OptionsDialogPage), "Encourage", "Encouragement List", 0, 0, supportsAutomation: true)]
    public sealed class EncouragePackage : AsyncPackage
    {
    }
}
