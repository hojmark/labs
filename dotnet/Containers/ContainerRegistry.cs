namespace HLabs.Containers;

public abstract record ContainerRegistry {
  protected abstract string Host {
    get;
  }

  // Sealed to prevent children from generating their own auto-ToString implementation
  public sealed override string ToString() {
    return Host;
  }
}

public sealed record DockerIoRegistry : ContainerRegistry {
  public static readonly DockerIoRegistry Instance = new();

  private DockerIoRegistry() {
  }

  protected override string Host => "docker.io";
}

public sealed record LocalhostRegistry : ContainerRegistry {
  public static readonly LocalhostRegistry Instance = new();

  private LocalhostRegistry() {
  }

  protected override string Host => "localhost:5000";
}