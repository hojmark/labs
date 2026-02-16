namespace HLabs.ImageReferences;

/// <summary>
/// Represents a container registry host (e.g., "docker.io", "ghcr.io", "localhost:5000").
/// </summary>
/// <example>
/// <code>
/// var registry = new Registry("docker.io");
/// var registry = new Registry("localhost:5000");
/// Registry registry = "ghcr.io";  // Implicit conversion from string
/// </code>
/// </example>
public sealed partial record Registry {
  private string Host {
    get;
  }

  internal bool NamespaceRequired {
    get;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Registry"/> class.
  /// </summary>
  /// <param name="host">The registry host (e.g., "docker.io", "ghcr.io"). Cannot be null, empty, or contain whitespace.</param>
  /// <param name="namespaceRequired">Indicates whether this registry requires a namespace for image references.</param>
  /// <exception cref="ArgumentException">Thrown when host is null, empty, or contains leading/trailing whitespace.</exception>
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

  /// <summary>
  /// Implicitly converts a string to a <see cref="Registry"/>.
  /// </summary>
  /// <param name="host">The registry host.</param>
  public static implicit operator Registry( string host ) => FromString( host );

  /// <summary>
  /// Creates a <see cref="Registry"/> from a string value.
  /// </summary>
  /// <param name="host">The registry host.</param>
  /// <returns>A new <see cref="Registry"/> instance.</returns>
  /// <exception cref="ArgumentException">Thrown when host is invalid.</exception>
  public static Registry FromString( string host ) {
    return new(host);
  }

  /// <summary>
  /// Returns the string representation of this registry.
  /// </summary>
  /// <returns>The registry host in lowercase.</returns>
  public override string ToString() => Host;

  /// <inheritdoc />
  public bool Equals( Registry? other ) {
    return other is not null && Host == other.Host;
  }

  /// <inheritdoc />
  public override int GetHashCode() {
    return Host.GetHashCode();
  }
}