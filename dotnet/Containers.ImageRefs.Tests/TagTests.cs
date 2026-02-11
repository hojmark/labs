using HLabs.Containers.Components;
using Semver;

namespace HLabs.Containers.Tests;

internal sealed class TagTests {
  [Test]
  public async Task ConstructorSetsValue() {
    var tag = new Tag( "1.0.0" );
    await Assert.That( tag.ToString() ).IsEqualTo( "1.0.0" );
  }

  [Test]
  public void EmptyValueThrows() {
    Assert.Throws<ArgumentException>( () => new Tag( string.Empty ) );
  }

  [Test]
  public void NullValueThrows() {
    Assert.Throws<ArgumentException>( () => new Tag( null! ) );
  }

  [Test]
  public void WhitespaceThrows() {
    Assert.Throws<ArgumentException>( () => new Tag( " latest " ) );
  }

  [Test]
  public async Task ImplicitConversionFromString() {
    Tag tag = "3.2.1";
    await Assert.That( tag.ToString() ).IsEqualTo( "3.2.1" );
  }

  [Test]
  public async Task ExplicitConversionFromString() {
    var tag = Tag.FromString( "3.2.1" );
    await Assert.That( tag.ToString() ).IsEqualTo( "3.2.1" );
  }

  [Test]
  public async Task ImplicitConversionFromSemVersion() {
    Tag tag = new SemVersion( 3, 2, 1 );
    await Assert.That( tag.ToString() ).IsEqualTo( "3.2.1" );
  }

  [Test]
  public async Task ExplicitConversionFromSemVersion() {
    var tag = Tag.FromSemVersion( new SemVersion( 3, 2, 1 ) );
    await Assert.That( tag.ToString() ).IsEqualTo( "3.2.1" );
  }

  [Test]
  public async Task SemVersionMetadataStripped() {
    Tag tag = new SemVersion( 1, 0, 0, ["alpha"], ["build42"] );
    await Assert.That( tag.ToString() ).IsEqualTo( "1.0.0-alpha" );
  }

  [Test]
  public async Task LatestConstant() {
    await Assert.That( Tag.Latest.ToString() ).IsEqualTo( "latest" );
  }

  [Test]
  public async Task CustomTagExtension() {
    var tag = Tag.Alpha( 3 );
    await Assert.That( tag.ToString() ).IsEqualTo( "alpha-3" );
  }
}

internal static class TestTagExtensions {
  extension( Tag ) {
    public static Tag Alpha( uint n ) => new($"alpha-{n}");
    public static Tag Dev => new("dev");
  }
}