using Semver;

namespace HLabs.Containers;

public sealed partial record Tag {
  private string Value {
    get;
  }

  public Tag( string value ) {
    if ( string.IsNullOrWhiteSpace( value ) ) {
      throw new ArgumentException( "Tag cannot be null or empty", nameof(value) );
    }

    if ( value.Trim().Length != value.Length ) {
      throw new ArgumentException( "Tag contains leading/trailing whitespace", nameof(value) );
    }

    // Tags are conventionally lowercase
#pragma warning disable CA1308
    Value = value.ToLowerInvariant();
#pragma warning restore CA1308
  }

  public static implicit operator Tag( string tag ) => FromString( tag );

  public static implicit operator Tag( SemVersion v ) => FromSemVersion( v ); // TODO move to subtype?

  public static Tag FromString( string tag ) {
    return new(tag);
  }

  public static Tag FromSemVersion( SemVersion v ) {
    // ! Null check is performed by constructor
    return new(v?.WithoutMetadata().ToString()!);
  }

  public override string ToString() => Value;
}