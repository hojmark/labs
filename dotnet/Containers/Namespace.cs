using System.Diagnostics.CodeAnalysis;

namespace HLabs.Containers;

[SuppressMessage(
  "Naming",
  "CA1716:Identifiers should not match keywords",
  Justification = "Implicit construction will likely be common and usage will otherwise be minimally verbose"
)]
public sealed partial record Namespace {
  private string Name {
    get;
  }

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

  public static implicit operator Namespace( string value ) => FromString( value );

  public override string ToString() => Name;

  public static Namespace FromString( string value ) {
    return new(value);
  }
}