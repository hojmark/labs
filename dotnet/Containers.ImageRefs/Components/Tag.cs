using Semver;

namespace HLabs.Containers.ImageRefs.Components;

/// <summary>
/// Represents a tag for a container image (e.g., "latest", "1.0", "v2.3.1").
/// Tags are mutable references that can point to different images over time.
/// </summary>
/// <example>
/// <code>
/// var tag = new Tag("latest");
/// var tag = new Tag("v1.2.3");
/// Tag tag = "alpine";  // Implicit conversion from string
/// </code>
/// </example>
public sealed partial record Tag {
  private string Value {
    get;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Tag"/> class.
  /// </summary>
  /// <param name="value">The tag value. Cannot be null, empty, or contain whitespace.</param>
  /// <exception cref="ArgumentException">Thrown when value is null, empty, or contains leading/trailing whitespace.</exception>
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

  /// <summary>
  /// Implicitly converts a string to a <see cref="Tag"/>.
  /// </summary>
  /// <param name="tag">The tag value.</param>
  public static implicit operator Tag( string tag ) => FromString( tag );

  /// <summary>
  /// Implicitly converts a <see cref="SemVersion"/> to a <see cref="Tag"/>.
  /// The version is converted to string format without metadata.
  /// </summary>
  /// <param name="v">The semantic version.</param>
  public static implicit operator Tag( SemVersion v ) => FromSemVersion( v );

  /// <summary>
  /// Creates a <see cref="Tag"/> from a string value.
  /// </summary>
  /// <param name="tag">The tag value.</param>
  /// <returns>A new <see cref="Tag"/> instance.</returns>
  /// <exception cref="ArgumentException">Thrown when tag is invalid.</exception>
  public static Tag FromString( string tag ) {
    return new(tag);
  }

  /// <summary>
  /// Creates a <see cref="Tag"/> from a semantic version.
  /// The version is converted to string format without metadata (e.g., "1.2.3").
  /// </summary>
  /// <param name="v">The semantic version.</param>
  /// <returns>A new <see cref="Tag"/> instance.</returns>
  /// <exception cref="ArgumentNullException">Thrown when v is null.</exception>
  public static Tag FromSemVersion( SemVersion v ) {
    ArgumentNullException.ThrowIfNull( v );
    return new(v.WithoutMetadata().ToString());
  }

  /// <summary>
  /// Returns the string representation of this tag.
  /// </summary>
  /// <returns>The tag value in lowercase.</returns>
  public override string ToString() => Value;
}