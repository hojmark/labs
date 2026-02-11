namespace HLabs.Containers;

public sealed partial record Registry {
  private string Host {
    get;
  }

  internal bool NamespaceRequired {
    get;
  }

  public Registry( string host, bool namespaceRequired = false ) {
    if ( string.IsNullOrWhiteSpace( host ) ) {
      throw new ArgumentException( "Registry host cannot be null or empty", nameof(host) );
    }

    if ( host.Trim().Length != host.Length ) {
      throw new ArgumentException( "Registry host contains leading/trailing whitespace", nameof(host) );
    }

    // Registry hosts are conventionally lowercase
#pragma warning disable CA1308
    Host = host.ToLowerInvariant();
#pragma warning restore CA1308
    NamespaceRequired = namespaceRequired;
  }

  public static implicit operator Registry( string host ) => FromString( host );

  public static Registry FromString( string host ) {
    return new(host);
  }

  public override string ToString() => Host;
}