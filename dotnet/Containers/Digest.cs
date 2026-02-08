namespace HLabs.Containers;

public sealed record Digest {
  private string Value {
    get;
  }

  public Digest( string value ) {
    if ( string.IsNullOrWhiteSpace( value ) ) {
      throw new ArgumentException( "Digest cannot be null or empty", nameof(value) );
    }

    if ( value.Trim().Length != value.Length ) {
      throw new ArgumentException( "Digest contains leading/trailing whitespace", nameof(value) );
    }

    if ( !value.Contains( ':', StringComparison.Ordinal ) ) {
      throw new ArgumentException( "Digest must be in 'algorithm:hex' format (e.g. 'sha256:abc123')", nameof(value) );
    }

    Value = value;
  }

  public static implicit operator Digest( string value ) => FromString( value );

  public static Digest FromString( string value ) {
    return new(value);
  }

  public override string ToString() => Value;
}