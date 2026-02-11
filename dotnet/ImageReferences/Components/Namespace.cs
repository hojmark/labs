using System.Diagnostics.CodeAnalysis;

namespace HLabs.ImageReferences;

/// <summary>
/// Represents a namespace within a container registry (e.g., organization or user name).
/// Namespaces are used to organize repositories within a registry.
/// </summary>
/// <example>
/// Examples of valid namespaces:
/// <code>
/// var ns = new Namespace("library");    // DockerHub official images
/// var ns = new Namespace("myorg");      // Organization namespace
/// var ns = new Namespace("username");   // User namespace
/// </code>
/// </example>
[SuppressMessage(
  "Naming",
  "CA1716:Identifiers should not match keywords",
  Justification = "Implicit construction will likely be common and usage will otherwise be minimally verbose"
)]
public sealed partial record Namespace {
  private string Name {
    get;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Namespace"/> class.
  /// </summary>
  /// <param name="name">The namespace name. Cannot be null, empty, or contain forward slashes.</param>
  /// <exception cref="ArgumentException">Thrown when name is null, empty, whitespace-only, contains leading/trailing whitespace, or contains a forward slash.</exception>
  public Namespace( string name ) {
    if ( string.IsNullOrWhiteSpace( name ) ) {
      throw new ArgumentException( "Namespace cannot be null or empty", nameof(name) );
    }

    if ( name.Contains( '/', StringComparison.Ordinal ) ) {
      throw new ArgumentException( "Namespace cannot contain '/'", nameof(name) );
    }

    if ( name.Trim().Length != name.Length ) {
      throw new ArgumentException( "Namespace contains leading/trailing whitespace", nameof(name) );
    }

    // Namespace names are conventionally lowercase
#pragma warning disable CA1308
    Name = name.ToLowerInvariant();
#pragma warning restore CA1308
  }

  /// <summary>
  /// Implicitly converts a string to a <see cref="Namespace"/>.
  /// </summary>
  /// <param name="value">The namespace name.</param>
  public static implicit operator Namespace( string value ) => FromString( value );

  /// <summary>
  /// Returns the string representation of this namespace.
  /// </summary>
  /// <returns>The namespace name in lowercase.</returns>
  public override string ToString() => Name;

  /// <summary>
  /// Creates a <see cref="Namespace"/> from a string value.
  /// </summary>
  /// <param name="value">The namespace name.</param>
  /// <returns>A new <see cref="Namespace"/> instance.</returns>
  /// <exception cref="ArgumentException">Thrown when value is invalid.</exception>
  public static Namespace FromString( string value ) {
    return new(value);
  }
}