using Nuke.Common;
using Nuke.Common.Tools.DotNet;
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