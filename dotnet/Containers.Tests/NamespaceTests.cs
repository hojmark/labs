namespace HLabs.Containers.Tests;

internal sealed class NamespaceTests {
  [Test]
  public async Task ConstructorSetsName() {
    var ns = new Namespace( "myteam" );
    await Assert.That( ns.ToString() ).IsEqualTo( "myteam" );
  }

  [Test]
  public void EmptyNameThrows() {
    Assert.Throws<ArgumentException>( () => new Namespace( string.Empty ) );
  }

  [Test]
  public void NullNameThrows() {
    Assert.Throws<ArgumentException>( () => new Namespace( null! ) );
  }

  [Test]
  public void SlashInNameThrows() {
    Assert.Throws<ArgumentException>( () => new Namespace( "a/b" ) );
  }

  [Test]
  public void WhitespaceThrows() {
    Assert.Throws<ArgumentException>( () => new Namespace( " team " ) );
  }

  [Test]
  public async Task ImplicitConversionFromString() {
    Namespace ns = "library";
    await Assert.That( ns.ToString() ).IsEqualTo( "library" );
  }

  [Test]
  public async Task ExplicitConversionFromString() {
    var ns = Namespace.FromString( "library" );
    await Assert.That( ns.ToString() ).IsEqualTo( "library" );
  }
}