namespace HLabs.Containers.Components;

/// <summary>
/// Represents a repository name within a container registry namespace.
/// A repository is the name of the container image (e.g., "nginx", "ubuntu", "myapp").
/// </summary>
/// <example>
/// <code>
/// var repo = new Repository("nginx");
/// var repo = new Repository("myapp");
/// Repository repo = "ubuntu";  // Implicit conversion from string
/// </code>
/// </example>
public sealed record Repository {
  private string Name {
    get;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Repository"/> class.
  /// </summary>
  /// <param name="name">The repository name. Cannot be null, empty, contain forward slashes, or have whitespace.</param>
  /// <exception cref="ArgumentException">Thrown when name is null, empty, contains '/', or contains leading/trailing whitespace.</exception>
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

    // Repository names are conventionally lowercase
#pragma warning disable CA1308
    Name = name.ToLowerInvariant();
#pragma warning restore CA1308
  }

  /// <summary>
  /// Implicitly converts a string to a <see cref="Repository"/>.
  /// </summary>
  /// <param name="value">The repository name.</param>
  public static implicit operator Repository( string value ) => FromString( value );

  /// <summary>
  /// Creates a <see cref="Repository"/> from a string value.
  /// </summary>
  /// <param name="value">The repository name.</param>
  /// <returns>A new <see cref="Repository"/> instance.</returns>
  /// <exception cref="ArgumentException">Thrown when value is invalid.</exception>
  public static Repository FromString( string value ) {
    return new(value);
  }

  /// <summary>
  /// Returns the string representation of this repository.
  /// </summary>
  /// <returns>The repository name in lowercase.</returns>
  public override string ToString() => Name;
}