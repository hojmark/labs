namespace HLabs.Containers.Tests;

internal sealed class DigestTests {
  [Test]
  public async Task ConstructorSetsValue() {
    var digest = new Digest( "sha256:abc123" );
    await Assert.That( digest.ToString() ).IsEqualTo( "sha256:abc123" );
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
    Digest d = "sha256:abc";
    await Assert.That( d.ToString() ).IsEqualTo( "sha256:abc" );
  }

  [Test]
  public async Task ExplicitConversionFromString() {
    var d = Digest.FromString( "sha256:abc" );
    await Assert.That( d.ToString() ).IsEqualTo( "sha256:abc" );
  }

  [Test]
  public async Task EqualDigestsAreEqual() {
    var a = new Digest( "sha256:abc" );
    var b = new Digest( "sha256:abc" );
    await Assert.That( a ).IsEqualTo( b );
  }

  [Test]
  public async Task DifferentDigestsAreNotEqual() {
    var a = new Digest( "sha256:abc" );
    var b = new Digest( "sha256:def" );
    await Assert.That( a ).IsNotEqualTo( b );
  }
}