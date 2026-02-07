using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using Serilog;

sealed partial class NukeBuild {
  Target Clean => _ => _
    .DependsOn( CleanProjects );

  Target CleanProjects => _ => _
    .Before( Restore )
    .Executes( () => {
        //using var _ = new OperationTimer( nameof(CleanProjects) );

        var dirsToDelete = Solution.AllProjects
          .Where( project => !IsBuildProject( project ) )
          .SelectMany( project => new[] {
              // Build directories
              project.Directory / "bin", project.Directory / "obj",

              // trx logs
              project.Directory / "TestResults"
            }
          )
          .Where( dir => dir.Exists() )
          .ToList();

        if ( dirsToDelete.IsEmpty() ) {
          Log.Debug( "No projects to clean" );
        }

        dirsToDelete.ForEach( d => {
          Log.Debug( "Deleting {BinOrObjDir}", d.ToString() );
          d.DeleteDirectory();
        } );
      }
    );

  private bool IsBuildProject( Project project ) {
    if ( project.Path == BuildProjectFile ) {
      return true;
    }

    return false;
  }
}