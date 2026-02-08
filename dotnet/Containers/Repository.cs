namespace HLabs.Containers;

public sealed record Repository {
  private string Name {
    get;
  }

  public Repository( string name ) {
    if ( string.IsNullOrWhiteSpace( name ) ) {
      throw new ArgumentException( "Repository cannot be null or empty", nameof(name) );
    }

    if ( name.Contains( '/', StringComparison.Ordinal ) ) {
      throw new ArgumentException( "Repository cannot contain '/'", nameof(name) );
    }

    if ( name.Trim().Length != name.Length ) {
      throw new ArgumentException( "Repository contains leading/trailing whitespace", nameof(name) );
    }

    Name = name;
  }

  public static implicit operator Repository( string value ) => FromString( value );

  public static Repository FromString( string value ) {
    return new(value);
  }

  public override string ToString() => Name;
}