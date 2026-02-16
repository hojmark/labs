namespace HLabs.ImageReferences.Tests;

internal sealed class CanonicalImageRefTests {
  private const string ValidDigest = "sha256:a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";
  private const string ValidDigest2 = "sha256:b5ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";

  // -----------------------
  // Construction
  // -----------------------
  [Test]
  public async Task ConstructFromQualifiedRef() {
    var canonical = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) )
      .Qualify()
      .Canonicalize();

    await Assert.That( canonical.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( canonical.Namespace ).IsEqualTo( new Namespace( "library" ) );
    await Assert.That( canonical.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task ConstructFromPartialRef() {
    var canonical = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();

    await Assert.That( canonical.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task ConstructWithCustomRegistry() {
    var canonical = new PartialImageRef(
        Registry.GitHub,
        new Namespace( "myorg" ),
        new Repository( "myapp" ),
        new Digest( ValidDigest )
      )
      .Canonicalize();

    await Assert.That( canonical.Registry ).IsEqualTo( Registry.GitHub );
    await Assert.That( canonical.Namespace ).IsEqualTo( new Namespace( "myorg" ) );
    await Assert.That( canonical.Repository ).IsEqualTo( new Repository( "myapp" ) );
  }

  [Test]
  public async Task ConstructWithOptionalTag() {
    var canonical = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) )
      .Canonicalize( CanonicalizationMode.MaintainTag );

    await Assert.That( canonical.Tag ).IsEqualTo( Tag.Latest );
    await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  // -----------------------
  // ToString
  // -----------------------
  [Test]
  public async Task ToStringWithDigestOnly() {
    var canonical = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    await Assert.That( canonical.ToString() ).IsEqualTo( $"docker.io/library/nginx@{ValidDigest}" );
  }

  [Test]
  public async Task ToStringWithTagAndDigest() {
    var canonical = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) )
      .Canonicalize( CanonicalizationMode.MaintainTag );
    await Assert.That( canonical.ToString() ).IsEqualTo( $"docker.io/library/nginx:latest@{ValidDigest}" );
  }

  [Test]
  public async Task ToStringWithCustomRegistry() {
    var canonical = new PartialImageRef(
        Registry.GitHub,
        new Namespace( "myorg" ),
        new Repository( "myapp" ),
        new Digest( ValidDigest )
      )
      .Canonicalize();
    await Assert.That( canonical.ToString() ).IsEqualTo( $"ghcr.io/myorg/myapp@{ValidDigest}" );
  }

  [Test]
  public async Task ToStringWithoutNamespace() {
    var canonical = new PartialImageRef( Registry.Localhost, "myapp", digest: new Digest( ValidDigest ) )
      .Canonicalize();
    await Assert.That( canonical.ToString() ).IsEqualTo( $"localhost:5000/myapp@{ValidDigest}" );
  }

  [Test]
  public async Task ToStringShowsTagWhenPresent() {
    var canonical = new PartialImageRef(
        Registry.GitHub,
        new Namespace( "myorg" ),
        new Repository( "myapp" ),
        new Tag( "v1.0" ),
        new Digest( ValidDigest )
      )
      .Canonicalize( CanonicalizationMode.MaintainTag );
    await Assert.That( canonical.ToString() ).IsEqualTo( $"ghcr.io/myorg/myapp:v1.0@{ValidDigest}" );
  }

  // -----------------------
  // WithTag
  // -----------------------
  [Test]
  public async Task WithTagAddsTag() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    var updated = original.With( Tag.Latest );

    await Assert.That( updated.Tag ).IsEqualTo( Tag.Latest );
    await Assert.That( updated.ToString() ).IsEqualTo( $"docker.io/library/nginx:latest@{ValidDigest}" );
  }

  [Test]
  public async Task WithTagReplacesTag() {
    var original = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) ).Canonicalize();
    var updated = original.With( new Tag( "alpine" ) );

    await Assert.That( updated.Tag ).IsEqualTo( new Tag( "alpine" ) );
    await Assert.That( updated.ToString() ).IsEqualTo( $"docker.io/library/nginx:alpine@{ValidDigest}" );
  }

  [Test]
  public async Task WithTagPreservesDigest() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    var updated = original.With( Tag.Latest );

    await Assert.That( updated.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task WithTagDoesNotMutateOriginal() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    _ = original.With( Tag.Latest );

    await Assert.That( original.Tag ).IsNull();
  }

  // -----------------------
  // On (change registry)
  // -----------------------
  [Test]
  public async Task OnChangesRegistry() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    var updated = original.With( Registry.GitHub );

    await Assert.That( updated.Registry ).IsEqualTo( Registry.GitHub );
    await Assert.That( updated.ToString() ).IsEqualTo( $"ghcr.io/library/nginx@{ValidDigest}" );
  }

  [Test]
  public async Task OnPreservesDigest() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    var updated = original.With( Registry.Localhost );

    await Assert.That( updated.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task OnDoesNotMutateOriginal() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    _ = original.With( Registry.GitHub );

    await Assert.That( original.Registry ).IsEqualTo( Registry.DockerHub );
  }

  // -----------------------
  // WithNamespace
  // -----------------------
  [Test]
  public async Task WithNamespaceChangesNamespace() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    var updated = original.With( new Namespace( "myorg" ) );

    await Assert.That( updated.Namespace ).IsEqualTo( new Namespace( "myorg" ) );
    await Assert.That( updated.ToString() ).IsEqualTo( $"docker.io/myorg/nginx@{ValidDigest}" );
  }

  [Test]
  public async Task WithNamespacePreservesOtherProperties() {
    var original = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) )
      .Canonicalize( CanonicalizationMode.MaintainTag );
    var updated = original.With( new Namespace( "custom" ) );

    await Assert.That( updated.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( updated.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( updated.Tag ).IsEqualTo( Tag.Latest );
    await Assert.That( updated.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task WithNamespaceDoesNotMutateOriginal() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    _ = original.With( new Namespace( "myorg" ) );

    await Assert.That( original.Namespace ).IsEqualTo( new Namespace( "library" ) );
  }

  // -----------------------
  // Immutability guarantees
  // -----------------------
  [Test]
  public async Task DigestNeverChanges() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    var withTag = original.With( Tag.Latest );
    var withRegistry = original.With( Registry.GitHub );
    var withNamespace = original.With( new Namespace( "myorg" ) );

    await Assert.That( withTag.Digest ).IsEqualTo( new Digest( ValidDigest ) );
    await Assert.That( withRegistry.Digest ).IsEqualTo( new Digest( ValidDigest ) );
    await Assert.That( withNamespace.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  // -----------------------
  // Equality
  // -----------------------
  [Test]
  public async Task EqualReferencesAreEqual() {
    var a = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    var b = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();

    await Assert.That( a ).IsEqualTo( b );
  }

  [Test]
  public async Task DifferentDigestsAreNotEqual() {
    var a = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    var b = new PartialImageRef( "nginx", digest: new Digest( ValidDigest2 ) ).Canonicalize();

    await Assert.That( a ).IsNotEqualTo( b );
  }

  [Test]
  public async Task DifferentRegistriesAreNotEqual() {
    var a = new PartialImageRef(
        Registry.DockerHub,
        new Namespace( "library" ),
        new Repository( "nginx" ),
        new Digest( ValidDigest )
      )
      .Canonicalize();
    var b = new PartialImageRef(
        Registry.GitHub,
        new Namespace( "library" ),
        new Repository( "nginx" ),
        new Digest( ValidDigest )
      )
      .Canonicalize();

    await Assert.That( a ).IsNotEqualTo( b );
  }

  [Test]
  public async Task SameDigestDifferentTagAreEqual() {
    // With default ExcludeTag mode, tags are excluded so these should be equal
    var a = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) ).Canonicalize();
    var b = new PartialImageRef( "nginx", new Tag( "alpine" ), digest: new Digest( ValidDigest ) ).Canonicalize();

    // They should be equal because both have null tags and same digest
    await Assert.That( a ).IsEqualTo( b );
  }

  [Test]
  public async Task SameDigestDifferentTagAreNotEqualWhenMaintained() {
    // With MaintainTag mode, tags are preserved so these should NOT be equal
    var a = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) )
      .Canonicalize( CanonicalizationMode.MaintainTag );
    var b = new PartialImageRef( "nginx", new Tag( "alpine" ), digest: new Digest( ValidDigest ) )
      .Canonicalize( CanonicalizationMode.MaintainTag );

    // They should NOT be equal because tags are different
    await Assert.That( a ).IsNotEqualTo( b );
  }

  [Test]
  public async Task WithTagCreatesNewInstance() {
    var original = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ).Canonicalize();
    var updated = original.With( Tag.Latest );

    await Assert.That( ReferenceEquals( original, updated ) ).IsFalse();
  }

  // -----------------------
  // Round-trip consistency
  // -----------------------
  [Test]
  public async Task ParseAndQualifyMaintainsDigest() {
    var parsed = PartialImageRef.Parse( $"nginx@{ValidDigest}" );
    var canonical = parsed.Canonicalize();

    await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
    await Assert.That( canonical.ToString() ).Contains( ValidDigest );
  }

  [Test]
  public async Task ToStringCanBeParsedBack() {
    var original = new PartialImageRef(
        Registry.GitHub,
        new Namespace( "myorg" ),
        new Repository( "myapp" ),
        Tag.Latest,
        new Digest( ValidDigest )
      )
      .Canonicalize();
    var str = original.ToString();
    var parsed = PartialImageRef.Parse( str );
    var canonical = parsed.Canonicalize();

    await Assert.That( canonical.Registry.ToString() ).IsEqualTo( original.Registry.ToString() );
    await Assert.That( canonical.Namespace!.ToString() ).IsEqualTo( original.Namespace!.ToString() );
    await Assert.That( canonical.Repository ).IsEqualTo( original.Repository );
    await Assert.That( canonical.Tag ).IsEqualTo( original.Tag );
    await Assert.That( canonical.Digest ).IsEqualTo( original.Digest );
    await Assert.That( canonical.ToString() ).IsEqualTo( original.ToString() );
  }
}