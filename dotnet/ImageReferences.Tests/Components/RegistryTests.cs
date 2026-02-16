namespace HLabs.ImageReferences.Tests.Components;

internal sealed class RegistryTests {
  [Test]
  public async Task ConstructorSetsHost() {
    var registry = new Registry( "my-registry.io" );
    await Assert.That( registry.ToString() ).IsEqualTo( "my-registry.io" );
  }

  [Test]
  public void EmptyHostThrows() {
    Assert.Throws<ArgumentException>( () => new Registry( string.Empty ) );
  }

  [Test]
  public void WhitespaceHostThrows() {
    Assert.Throws<ArgumentException>( () => new Registry( " " ) );
  }

  [Test]
  public void LeadingWhitespaceHostThrows() {
    Assert.Throws<ArgumentException>( () => new Registry( " docker.io" ) );
  }

  [Test]
  public async Task ImplicitConversionFromString() {
    Registry r = "quay.io";
    await Assert.That( r.ToString() ).IsEqualTo( "quay.io" );
  }

  [Test]
  public async Task ExplicitConversionFromString() {
    var r = Registry.FromString( "quay.io" );
    await Assert.That( r.ToString() ).IsEqualTo( "quay.io" );
  }

  [Test]
  public async Task PredefinedDockerHub() {
    await Assert.That( Registry.DockerHub.ToString() ).IsEqualTo( "docker.io" );
  }

  [Test]
  public async Task PredefinedGitHub() {
    await Assert.That( Registry.GitHub.ToString() ).IsEqualTo( "ghcr.io" );
  }

  [Test]
  public async Task AcrFactory() {
    var r = Registry.Acr( "mycompany" );
    await Assert.That( r.ToString() ).IsEqualTo( "mycompany.azurecr.io" );
  }

  [Test]
  public async Task EcrFactory() {
    var r = Registry.Ecr( "123456", "eu-west-1" );
    await Assert.That( r.ToString() ).IsEqualTo( "123456.dkr.ecr.eu-west-1.amazonaws.com" );
  }

  [Test]
  public async Task EqualRegistriesAreEqual() {
    var a = new Registry( "docker.io", true ); // TODO consider: should it be equal if namespaceRequired is false?
    await Assert.That( a ).IsEqualTo( Registry.DockerHub );
  }
}