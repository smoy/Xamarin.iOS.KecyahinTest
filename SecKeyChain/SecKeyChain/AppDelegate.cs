using Foundation;
using UIKit;

using Xunit.Runner;
using Xunit.Sdk;
using System.Reflection;

namespace SecKeyChainTest
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the
  // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
  [Register("AppDelegate")]
  public class AppDelegate : RunnerAppDelegate
  {

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      // We need this to ensure the execution assembly is part of the app bundle
      AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);


      // tests can be inside the main assembly
      AddTestAssembly(Assembly.GetExecutingAssembly());
      // otherwise you need to ensure that the test assemblies will 
      // become part of the app bundle
      //AddTestAssembly(typeof(PortableTests).Assembly);

      return base.FinishedLaunching(app, options);
    }
  }
}


