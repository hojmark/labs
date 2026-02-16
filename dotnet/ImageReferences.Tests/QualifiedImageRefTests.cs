namespace HLabs.ImageReferences.Tests;

internal sealed class QualifiedImageRefTests {
  private const string ValidDigest = "sha256:a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";

  // -----------------------
  // Construction
  // -----------------------
  [Test]
  public async Task ConstructWithTagOnly() {
    var image = new PartialImageRef( "nginx", Tag.Latest ).Qualify();

    await Assert.That( image.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( image.Namespace ).IsEqualTo( new Namespace( "library" ) );
    await Assert.That( image.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( image.Tag ).IsEqualTo( Tag.Latest );
    await Assert.That( image.Digest ).IsNull();
  }

  [Test]
  public async Task ConstructWithDigestOnly() {
    var image = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Qualify();

    await Assert.That( image.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( image.Namespace ).IsEqualTo( new Namespace( "library" ) );
    await Assert.That( image.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( image.Tag ).IsNull();
    await Assert.That( image.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task ConstructWithTagAndDigest() {
    var image = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) ).Qualify();

    await Assert.That( image.Tag ).IsEqualTo( Tag.Latest );
    await Assert.That( image.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task ConstructWithCustomRegistry() {
    var image = new PartialImageRef( Registry.GitHub, "myorg", "myapp", Tag.Latest ).Qualify();

    await Assert.That( image.Registry ).IsEqualTo( Registry.GitHub );
    await Assert.That( image.Namespace ).IsEqualTo( new Namespace( "myorg" ) );
    await Assert.That( image.Repository ).IsEqualTo( new Repository( "myapp" ) );
  }

  [Test]
  public async Task ConstructWithoutNamespaceOnNonRequiredRegistry() {
    var image = new PartialImageRef( Registry.Localhost, "myapp", Tag.Latest ).Qualify();

    await Assert.That( image.Registry ).IsEqualTo( Registry.Localhost );
    await Assert.That( image.Namespace ).IsNull();
    await Assert.That( image.Repository ).IsEqualTo( new Repository( "myapp" ) );
  }

  // -----------------------
  // ToString
  // -----------------------
  [Test]
  public async Task ToStringWithTagOnly() {
    var image = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    await Assert.That( image.ToString() ).IsEqualTo( "docker.io/library/nginx:latest" );
  }

  [Test]
  public async Task ToStringWithDigestOnly() {
    var image = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Qualify();
    await Assert.That( image.ToString() ).IsEqualTo( $"docker.io/library/nginx@{ValidDigest}" );
  }

  [Test]
  public async Task ToStringWithTagAndDigest() {
    var image = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) ).Qualify();
    await Assert.That( image.ToString() ).IsEqualTo( $"docker.io/library/nginx:latest@{ValidDigest}" );
  }

  [Test]
  public async Task ToStringWithCustomRegistry() {
    var image = new PartialImageRef( Registry.GitHub, "myorg", "myapp", Tag.Latest ).Qualify();
    await Assert.That( image.ToString() ).IsEqualTo( "ghcr.io/myorg/myapp:latest" );
  }

  [Test]
  public async Task ToStringWithoutNamespace() {
    var image = new PartialImageRef( Registry.Localhost, "myapp", Tag.Latest ).Qualify();
    await Assert.That( image.ToString() ).IsEqualTo( "localhost:5000/myapp:latest" );
  }

  // -----------------------
  // WithTag
  // -----------------------
  [Test]
  public async Task WithTagReplacesTag() {
    var original = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    var updated = original.With( new Tag( "alpine" ) );

    await Assert.That( updated.Tag ).IsEqualTo( new Tag( "alpine" ) );
    await Assert.That( updated.ToString() ).IsEqualTo( "docker.io/library/nginx:alpine" );
  }

  [Test]
  public async Task WithTagPreservesOtherProperties() {
    var original = new PartialImageRef( Registry.GitHub, "myorg", "myapp", Tag.Latest ).Qualify();
    var updated = original.With( new Tag( "v1.0" ) );

    await Assert.That( updated.Registry ).IsEqualTo( Registry.GitHub );
    await Assert.That( updated.Namespace ).IsEqualTo( new Namespace( "myorg" ) );
    await Assert.That( updated.Repository ).IsEqualTo( new Repository( "myapp" ) );
    await Assert.That( updated.Tag ).IsEqualTo( new Tag( "v1.0" ) );
  }

  [Test]
  public async Task WithTagDoesNotMutateOriginal() {
    var original = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    _ = original.With( new Tag( "alpine" ) );

    await Assert.That( original.Tag ).IsEqualTo( Tag.Latest );
  }

  // -----------------------
  // WithRegistry (change registry)
  // -----------------------
  [Test]
  public async Task WithRegistryChangesRegistry() {
    var original = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    var updated = original.With( Registry.GitHub, new Namespace( "myorg" ) );

    await Assert.That( updated.Registry ).IsEqualTo( Registry.GitHub );
    await Assert.That( updated.Namespace ).IsEqualTo( new Namespace( "myorg" ) );
    await Assert.That( updated.ToString() ).IsEqualTo( "ghcr.io/myorg/nginx:latest" );
  }

  [Test]
  public async Task WithRegistryPreservesOtherProperties() {
    var original = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    var updated = original.With( Registry.Localhost );

    await Assert.That( updated.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( updated.Tag ).IsEqualTo( Tag.Latest );
  }

  [Test]
  public async Task WithRegistryDoesNotMutateOriginal() {
    var original = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    _ = original.With( Registry.GitHub, new Namespace( "myorg" ) );

    await Assert.That( original.Registry ).IsEqualTo( Registry.DockerHub );
  }

  // -----------------------
  // Canonicalize
  // -----------------------
  [Test]
  public async Task CanonicalizeWithDigestCreatesCanonicalRef() {
    var qualified = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    var canonical = qualified.Canonicalize( new Digest( ValidDigest ) );

    await Assert.That( canonical ).IsNotNull();
    await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
    await Assert.That( canonical.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( canonical.Repository ).IsEqualTo( new Repository( "nginx" ) );
  }

  [Test]
  public async Task CanonicalizeWithExistingDigest() {
    var qualified = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) ).Qualify();
    var canonical = qualified.Canonicalize();

    await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public void CanonicalizeWithoutDigestThrows() {
    var qualified = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    Assert.Throws<InvalidOperationException>( () => qualified.Canonicalize() );
  }

  // -----------------------
  // Equality
  // -----------------------
  [Test]
  public async Task EqualReferencesAreEqual() {
    var a = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    var b = new PartialImageRef( "nginx", Tag.Latest ).Qualify();

    await Assert.That( a ).IsEqualTo( b );
  }

  [Test]
  public async Task DifferentTagsAreNotEqual() {
    var a = new PartialImageRef( "nginx", Tag.Latest ).Qualify();
    var b = new PartialImageRef( "nginx", new Tag( "alpine" ) ).Qualify();

    await Assert.That( a ).IsNotEqualTo( b );
  }

  [Test]
  public async Task DifferentRegistriesAreNotEqual() {
    var a = new PartialImageRef( Registry.DockerHub, "library", "nginx", Tag.Latest ).Qualify();
    var b = new PartialImageRef( Registry.GitHub, "library", "nginx", Tag.Latest ).Qualify();

    await Assert.That( a ).IsNotEqualTo( b );
  }

  [Test]
  public async Task DifferentNamespacesAreNotEqual() {
    var a = new PartialImageRef( Registry.DockerHub, "library", "nginx", Tag.Latest ).Qualify();
    var b = new PartialImageRef( Registry.DockerHub, "myorg", "nginx", Tag.Latest ).Qualify();

    await Assert.That( a ).IsNotEqualTo( b );
  }
}