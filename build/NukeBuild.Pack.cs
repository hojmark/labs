using System.IO.Compression;
using System.Xml.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Serilog;
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
          ClearNuGetCacheFor( nupkg );
          var fileName = Path.GetFileName( nupkg );
          var path = Path.Combine( Paths.NuGetLocalRegistry, fileName );
          File.Copy( nupkg, path, overwrite: true );
          Log.Information( "Copied {Nupkg} to {Path}", fileName, path );
        }
      }
    );

  void ClearNuGetCacheFor( string nupkgPath ) {
    var (packageId, version) = ReadPackageIdentity( nupkgPath );

    if ( packageId is null || version is null )
      return;

    var globalPackages = Path.Combine(
      Environment.GetFolderPath( Environment.SpecialFolder.UserProfile ),
      ".nuget",
      "packages" );

    var cachedPath = Path.Combine(
      globalPackages,
      packageId.ToLowerInvariant(),
      version );

    if ( !Directory.Exists( cachedPath ) )
      return;

    Log.Information(
      "Clearing NuGet cache for {Id} {Version} at {Path}",
      packageId,
      version,
      cachedPath );

    Directory.Delete( cachedPath, recursive: true );
  }

private (string? Id, string? Version) ReadPackageIdentity( string nupkgPath ) {
  using var archive = ZipFile.OpenRead( nupkgPath );

  var nuspecEntry = archive.Entries
    .FirstOrDefault( e => e.FullName.EndsWith( ".nuspec", StringComparison.OrdinalIgnoreCase ) );

  if ( nuspecEntry is null ) {
    Log.Warning( "No .nuspec found in {Nupkg}", nupkgPath );
    return ( null, null );
  }

  using var stream = nuspecEntry.Open();
  var document = XDocument.Load( stream );

  // NuSpec uses namespaces — ignore them safely
  var metadata = document
    .Descendants()
    .FirstOrDefault( e => e.Name.LocalName == "metadata" );

  var id = metadata?
    .Elements()
    .FirstOrDefault( e => e.Name.LocalName == "id" )
    ?.Value;

  var version = metadata?
    .Elements()
    .FirstOrDefault( e => e.Name.LocalName == "version" )
    ?.Value;

  if ( id is null || version is null ) {
    Log.Warning( "Could not read id/version from nuspec in {Nupkg}", nupkgPath );
  }

  return ( id, version );
}
}
