using System.IO;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using Octokit;
using Serilog;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable VariableHidesOuterVariable
// ReSharper disable AllUnderscoreLocalParameterName
// ReSharper disable UnusedMember.Local

internal partial class NukeBuild {
  public bool AllowLocalRelease => true; //TODO set to false

  public Target Pack => _ => _
    .DependsOn( Build, Test )
    .Executes( async () => {
        Paths.NuGetArtifacts.CreateOrCleanDirectory();

        DotNetPack( s => s
          .SetProject( Solution )
          .SetConfiguration( Configuration )
          .EnableNoBuild()
          .EnableNoRestore()
          .SetOutputDirectory( Paths.NuGetArtifacts )
        );
      }
    );

  public Target PackLocal => _ => _
    .DependsOn( Pack )
    .Executes( () => {
        Paths.NuGetLocalRegistry.CreateDirectory();

        Log.Information( "Publishing to local registry {Path}", Paths.NuGetLocalRegistry );

        foreach ( var nupkg in Directory.GetFiles( Paths.NuGetArtifacts, "*.nupkg" ) ) {
          var fileName = Path.GetFileName( nupkg );
          var path = Path.Combine( Paths.NuGetLocalRegistry, fileName );
          File.Copy( nupkg, path, overwrite: true );
          Log.Information( "Copied {Nupkg} to {Path}", fileName, path );
        }
      }
    );
}