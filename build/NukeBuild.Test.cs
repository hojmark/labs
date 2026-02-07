using Nuke.Common;
using Nuke.Common.Tooling;

// ReSharper disable VariableHidesOuterVariable
// ReSharper disable AllUnderscoreLocalParameterName
// ReSharper disable UnusedMember.Local

sealed partial class NukeBuild {
  Target Test => _ => _
    .DependsOn( TestUnit );

  Target TestLocal => _ => _
    .DependsOn( Test )
    .Executes( () => {
        var result = ProcessTasks.StartProcess(
          "dotnet",
          "trx --verbosity verbose",
          workingDirectory: RootDirectory
        );
        result.AssertZeroExitCode();
      }
    );

  Target TestUnit => _ => _
    .DependsOn( Build )
    .Executes( () => {
        //using var _ = new OperationTimer( nameof(TestUnit) );

        var result = ProcessTasks.StartProcess(
          "dotnet",
          $"test --solution {Solution.Path} --configuration {Configuration} --minimum-expected-tests 8 --no-restore --no-build",
          workingDirectory: RootDirectory
        );
        result.AssertZeroExitCode();
      }
    );
}