using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class NukeBuild {
  Target Restore => _ => _
    .Executes( () => {
        //using var _ = new OperationTimer( nameof(Restore) );

        DotNetRestore( s => s
          .SetProjectFile( Solution )
        );
      }
    );

  Target Build => _ => _
    .DependsOn( Restore )
    .Executes( () => {
        //using var _ = new OperationTimer( nameof(Build) );

        //var version = await Versioning.Value.GetVersionAsync();

        DotNetBuild( s => s
          .SetProjectFile( Solution )
          .SetConfiguration( Configuration )
          //TODO.SetVersionProperties( version )
          //.SetBinaryLog( BinaryBuildLogName )
          .EnableNoLogo()
          .EnableNoRestore()
        );
      }
    );
}