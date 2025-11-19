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

sealed partial class NukeBuild : Nuke.Common.NukeBuild {
  public static int Main() => Execute<NukeBuild>( x => x.Build );

  [Parameter( "Configuration to build - Default is 'Debug' (local) or 'Release' (server)" )]
  public readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

  [Solution( GenerateProjects = true )] //
  private readonly Solution Solution;

  private static class Paths {
    internal static AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    internal static AbsolutePath NuGetArtifacts => ArtifactsDirectory / "nuget";

    internal static AbsolutePath NuGetLocalRegistry =>
      SpecialFolder( SpecialFolders.UserProfile ) / ".nuget-local-registry";
  }
}