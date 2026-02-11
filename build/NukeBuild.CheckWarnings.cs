using Nuke.Common;
using Serilog;
using Utils;

// ReSharper disable VariableHidesOuterVariable
// ReSharper disable AllUnderscoreLocalParameterName
// ReSharper disable UnusedMember.Local

sealed partial class NukeBuild {
  private const string BinaryBuildLogName = "build.binlog";

  Target CheckWarnings => _ => _
    .DependsOn( CheckBuildWarnings );

  Target CheckBuildWarnings => _ => _
    .After( Build )
    .Executes( () => {
        var warnings = BinaryLogReader.GetWarnings( BinaryBuildLogName );

        foreach ( var warning in warnings ) {
          Log.Information( warning );
        }

        var hasWarnings = warnings.Length != 0;

        if ( hasWarnings ) {
          Log.Error( "Found {Count} build warnings", warnings.Length );
          throw new Exception( $"Found {warnings.Length} build warnings" );
        }

        Log.Information( "🟢 No build warnings found" );
      }
    );
}