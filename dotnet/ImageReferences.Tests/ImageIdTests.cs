namespace HLabs.ImageReferences.Tests;

internal sealed class ImageIdTests {
  private const string ValidHash = "a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";
  private const string ValidDigest = "sha256:a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";

  // -----------------------
  // Construction
  // -----------------------
  [Test]
  public async Task ConstructorWithFullFormat() {
    var imageId = new ImageId( ValidDigest );
    await Assert.That( imageId.ToString() ).IsEqualTo( ValidDigest );
  }

  [Test]
  public async Task ConstructorWithHashOnly() {
    var imageId = new ImageId( ValidHash );
    await Assert.That( imageId.ToString() ).IsEqualTo( ValidDigest );
  }

  [Test]
  public async Task ConstructorNormalizesToLowercase() {
    var uppercaseHash = "SHA256:A3ED95CAEB02FFE68CDD9FD84406680AE93D633CB16422D00E8A7C22955B46D4";
    var imageId = new ImageId( uppercaseHash );
    await Assert.That( imageId.ToString() ).IsEqualTo( ValidDigest );
  }

  [Test]
  public async Task ConstructorWithMixedCaseHash() {
    var mixedCase = "A3ED95caeb02FFE68cdd9FD84406680ae93D633cb16422d00e8a7c22955b46d4";
    var imageId = new ImageId( mixedCase );
    await Assert.That( imageId.ToString() ).IsEqualTo( ValidDigest );
  }

  // -----------------------
  // Validation - Null/Empty/Whitespace
  // -----------------------
  [Test]
  public void NullValueThrows() {
    Assert.Throws<ArgumentException>( () => new ImageId( null! ) );
  }

  [Test]
  public void EmptyValueThrows() {
    Assert.Throws<ArgumentException>( () => new ImageId( string.Empty ) );
  }

  [Test]
  public void WhitespaceThrows() {
    Assert.Throws<ArgumentException>( () => new ImageId( "  " ) );
  }

  [Test]
  public void LeadingWhitespaceThrows() {
    Assert.Throws<ArgumentException>( () => new ImageId( $" {ValidDigest}" ) );
  }

  [Test]
  public void TrailingWhitespaceThrows() {
    Assert.Throws<ArgumentException>( () => new ImageId( $"{ValidDigest} " ) );
  }

  // -----------------------
  // Validation - Algorithm
  // -----------------------
  [Test]
  public void InvalidAlgorithmThrows() {
    Assert.Throws<ArgumentException>( () => new ImageId( $"md5:{ValidHash}" ) );
  }

  [Test]
  public void Sha1AlgorithmThrows() {
    var sha1Hash = "356a192b7913b04c54574d18c28d46e6395428ab"; // Valid SHA-1 format but wrong algorithm
    Assert.Throws<ArgumentException>( () => new ImageId( $"sha1:{sha1Hash}" ) );
  }

  [Test]
  public void Sha512AlgorithmThrows() {
    var sha512Hash =
      "b109f3bbbc244eb82441917ed06d618b9008dd09b3befd1b5e07394c706a8bb980b1d7785e5976ec049b46df5f1326af5a2ea6d103fd07c95385ffab0cacbc86";
    Assert.Throws<ArgumentException>( () => new ImageId( $"sha512:{sha512Hash}" ) );
  }

  // -----------------------
  // Validation - Hash format
  // -----------------------
  [Test]
  public void InvalidHashLengthTooShortThrows() {
    var shortHash = "a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46"; // 62 chars
    Assert.Throws<ArgumentException>( () => new ImageId( shortHash ) );
  }

  [Test]
  public void InvalidHashLengthTooLongThrows() {
    var longHash = "a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4aa"; // 66 chars
    Assert.Throws<ArgumentException>( () => new ImageId( longHash ) );
  }

  [Test]
  public void InvalidHashCharactersThrows() {
    var invalidHash = "g3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4"; // 'g' is not hex
    Assert.Throws<ArgumentException>( () => new ImageId( invalidHash ) );
  }

  [Test]
  public void HashWithSpacesThrows() {
    var hashWithSpaces = "a3ed95ca eb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";
    Assert.Throws<ArgumentException>( () => new ImageId( hashWithSpaces ) );
  }

  [Test]
  public void HashWithDashesThrows() {
    var hashWithDashes = "a3ed95ca-eb02-ffe6-8cdd-9fd84406680ae93d633cb16422d00e8a7c22955b46d4";
    Assert.Throws<ArgumentException>( () => new ImageId( hashWithDashes ) );
  }

  // -----------------------
  // Conversion
  // -----------------------
  [Test]
  public async Task ImplicitConversionFromString() {
    ImageId imageId = ValidDigest;
    await Assert.That( imageId.ToString() ).IsEqualTo( ValidDigest );
  }

  [Test]
  public async Task ExplicitConversionFromString() {
    var imageId = ImageId.FromString( ValidDigest );
    await Assert.That( imageId.ToString() ).IsEqualTo( ValidDigest );
  }

  [Test]
  public async Task ToStringReturnsNormalizedFormat() {
    var imageId = new ImageId( ValidHash );
    var result = imageId.ToString();

    await Assert.That( result ).StartsWith( "sha256:" );
    await Assert.That( result ).IsEqualTo( ValidDigest );
  }

  [Test]
  public async Task ToStringIncludesAlgorithm() {
    var imageId = new ImageId( ValidHash );
    await Assert.That( imageId.ToString() ).Contains( "sha256:" );
  }

  // -----------------------
  // Equality
  // -----------------------
  [Test]
  public async Task EqualImageIdsAreEqual() {
    var a = new ImageId( ValidDigest );
    var b = new ImageId( ValidDigest );
    await Assert.That( a ).IsEqualTo( b );
  }

  [Test]
  public async Task DifferentImageIdsAreNotEqual() {
    var a = new ImageId( ValidDigest );
    var differentHash = "b5ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";
    var b = new ImageId( differentHash );
    await Assert.That( a ).IsNotEqualTo( b );
  }

  [Test]
  public async Task DifferentCasingAreEqual() {
    var lowercase = new ImageId( ValidDigest );
    var uppercase = new ImageId( "SHA256:A3ED95CAEB02FFE68CDD9FD84406680AE93D633CB16422D00E8A7C22955B46D4" );
    await Assert.That( lowercase ).IsEqualTo( uppercase );
  }

  [Test]
  public async Task WithAndWithoutAlgorithmPrefixAreEqual() {
    var withPrefix = new ImageId( ValidDigest );
    var withoutPrefix = new ImageId( ValidHash );
    await Assert.That( withPrefix ).IsEqualTo( withoutPrefix );
  }

  [Test]
  public async Task GetHashCodeSameForEqualIds() {
    var a = new ImageId( ValidDigest );
    var b = new ImageId( ValidHash );
    await Assert.That( a.GetHashCode() ).IsEqualTo( b.GetHashCode() );
  }

  [Test]
  public async Task GetHashCodeDifferentForDifferentIds() {
    var a = new ImageId( ValidDigest );
    var differentHash = "b5ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";
    var b = new ImageId( differentHash );
    await Assert.That( a.GetHashCode() ).IsNotEqualTo( b.GetHashCode() );
  }
}