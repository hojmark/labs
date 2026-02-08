namespace HLabs.Containers;

public sealed partial record Registry {
  private string Host {
    get;
  }

  public Registry( string host ) {
    if ( string.IsNullOrWhiteSpace( host ) ) {
      throw new ArgumentException( "Registry host cannot be null or empty", nameof(host) );
    }

    if ( host.Trim().Length != host.Length ) {
      throw new ArgumentException( "Registry host contains leading/trailing whitespace", nameof(host) );
    }

    Host = host;
  }

  public static implicit operator Registry( string host ) => FromString( host );

  public static Registry FromString( string host ) {
    return new(host);
  }

  public override string ToString() => Host;
}