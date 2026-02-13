namespace HLabs.ImageReferences.Tests;

internal sealed class RepositoryTests {
  [Test]
  public async Task ConstructorSetsName() {
    var repo = new Repository( "nginx" );
    await Assert.That( repo.ToString() ).IsEqualTo( "nginx" );
  }

  [Test]
  public void EmptyNameThrows() {
    Assert.Throws<ArgumentException>( () => new Repository( string.Empty ) );
  }

  [Test]
  public void NullNameThrows() {
    Assert.Throws<ArgumentException>( () => new Repository( null! ) );
  }

  [Test]
  public void SlashInNameThrows() {
    Assert.Throws<ArgumentException>( () => new Repository( "a/b" ) );
  }

  [Test]
  public void LeadingWhitespaceThrows() {
    Assert.Throws<ArgumentException>( () => new Repository( " nginx" ) );
  }

  [Test]
  public void TrailingWhitespaceThrows() {
    Assert.Throws<ArgumentException>( () => new Repository( "nginx " ) );
  }

  [Test]
  public async Task ImplicitConversionFromString() {
    Repository r = "myapp";
    await Assert.That( r.ToString() ).IsEqualTo( "myapp" );
  }

  [Test]
  public async Task ExplicitConversionFromString() {
    var r = Repository.FromString( "myapp" );
    await Assert.That( r.ToString() ).IsEqualTo( "myapp" );
  }

  [Test]
  public async Task EqualRepositoriesAreEqual() {
    var a = new Repository( "nginx" );
    var b = new Repository( "nginx" );
    await Assert.That( a ).IsEqualTo( b );
  }
}