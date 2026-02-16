namespace HLabs.ImageReferences.Tests.Components;

internal sealed class DigestTests {
  private const string ValidDigest = "sha256:a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";

  [Test]
  public async Task ConstructorSetsValue() {
    var digest = new Digest( ValidDigest );
    await Assert.That( digest.ToString() ).IsEqualTo( ValidDigest );
  }

  [Test]
  public void EmptyValueThrows() {
    Assert.Throws<ArgumentException>( () => new Digest( string.Empty ) );
  }

  [Test]
  public void NullValueThrows() {
    Assert.Throws<ArgumentException>( () => new Digest( null! ) );
  }

  [Test]
  public void WhitespaceThrows() {
    Assert.Throws<ArgumentException>( () => new Digest( "  " ) );
  }

  [Test]
  public void LeadingWhitespaceThrows() {
    Assert.Throws<ArgumentException>( () => new Digest( " sha256:abc" ) );
  }

  [Test]
  public void MissingColonThrows() {
    Assert.Throws<ArgumentException>( () => new Digest( "sha256abc" ) );
  }

  [Test]
  public async Task ImplicitConversionFromString() {
    Digest d = ValidDigest;
    await Assert.That( d.ToString() ).IsEqualTo( ValidDigest );
  }

  [Test]
  public async Task ExplicitConversionFromString() {
    var d = Digest.FromString( ValidDigest );
    await Assert.That( d.ToString() ).IsEqualTo( ValidDigest );
  }

  [Test]
  public async Task EqualDigestsAreEqual() {
    var a = new Digest( ValidDigest );
    var b = new Digest( ValidDigest );
    await Assert.That( a ).IsEqualTo( b );
  }

  [Test]
  public async Task DifferentDigestsAreNotEqual() {
    var a = new Digest( ValidDigest );
    var b = new Digest( "sha256:b5ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4" );
    await Assert.That( a ).IsNotEqualTo( b );
  }
}