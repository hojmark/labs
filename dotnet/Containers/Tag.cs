using Semver;

namespace HLabs.Containers;

public record Tag {
  internal Tag( string value ) {
    Value = value;
  }

  private string Value {
    get;
  }

  // Sealed to prevent children from generating their own auto-ToString implementation
  public sealed override string ToString() {
    return Value;
  }
}

public record LatestTag : Tag {
  public static readonly LatestTag Instance = new();

  private LatestTag() : base( "latest" ) {
  }
}

public static class TagExtensions {
  extension( Tag ) {
    public static Tag Latest => LatestTag.Instance;
    public static Tag Dev => new("dev");

    public static Tag Version( SemVersion version ) {
      ArgumentNullException.ThrowIfNull( version );
      return new Tag( version.WithoutMetadata().ToString() );
    }
  }
}