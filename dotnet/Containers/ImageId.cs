using System.Text.RegularExpressions;

namespace HLabs.Containers;

public sealed record ImageId {
  private const string DefaultAlgorithm = "sha256";

  private static readonly Regex Sha256Regex =
    new(@"^[0-9a-fA-F]{64}$", RegexOptions.Compiled);

  private string Value {
    get;
  }

  public ImageId( string value ) {
    if ( string.IsNullOrWhiteSpace( value ) ) {
      throw new ArgumentException( "Image ID cannot be null or empty", nameof(value) );
    }

    if ( value.Trim().Length != value.Length ) {
      throw new ArgumentException( "Image ID contains leading/trailing whitespace", nameof(value) );
    }

    string digest;

    if ( value.Contains( ':', StringComparison.Ordinal ) ) {
      var parts = value.Split( ':', 2 );

      var algorithm = parts[0];
      digest = parts[1];

      if ( !algorithm.Equals( DefaultAlgorithm, StringComparison.OrdinalIgnoreCase ) ) {
        throw new ArgumentException(
          $"Unsupported image ID algorithm '{algorithm}'. Only sha256 is supported.",
          nameof(value)
        );
      }
    }
    else {
      digest = value;
    }

    if ( !Sha256Regex.IsMatch( digest ) ) {
      throw new ArgumentException(
        "Image ID digest must be a valid 64-character hexadecimal SHA-256 hash.",
        nameof(value)
      );
    }

    // Conventionally lowercase
#pragma warning disable CA1308
    Value = $"{DefaultAlgorithm}:{digest.ToLowerInvariant()}";
#pragma warning restore CA1308
  }

  public static implicit operator ImageId( string value ) => FromString( value );

  public static ImageId FromString( string value ) {
    return new(value);
  }

  public override string ToString() => Value;
}