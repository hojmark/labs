using Semver;

namespace HLabs.Containers.Tests;

internal sealed class ImageReferenceTests {
  // -----------------------
  // ToString round-trip
  // -----------------------
  public static IEnumerable<(ImageReference, string)> ToStringCases {
    get {
      // DockerHub defaults
      yield return (
        new ImageReference( new Repository( "ubuntu" ), Tag.Latest, Registry.DockerHub, new Namespace( "library" ) ),
        "docker.io/library/ubuntu:latest"
      );
      yield return (
        new ImageReference( new Repository( "ubuntu" ), Tag.Latest, Registry.DockerHub ),
        "docker.io/library/ubuntu:latest"
      );
      yield return (
        new ImageReference( new Repository( "ubuntu" ), Tag.Latest ),
        "docker.io/library/ubuntu:latest"
      );
      yield return (
        new ImageReference( new Repository( "ubuntu" ) ),
        "docker.io/library/ubuntu:latest"
      );
      yield return (
        new ImageReference( "ubuntu" ),
        "docker.io/library/ubuntu:latest"
      );
      // Implicit conversions
      yield return (
        new ImageReference( new Repository( "ubuntu" ), Tag.Latest, Registry.DockerHub, "library" ),
        "docker.io/library/ubuntu:latest"
      );
      // Custom namespace on DockerHub
      yield return (
        new ImageReference( "drift", Tag.Latest, @namespace: "hojmark" ),
        "docker.io/hojmark/drift:latest"
      );
      yield return (
        new ImageReference( "drift", new SemVersion( 1, 21, 1 ), @namespace: "hojmark" ),
        "docker.io/hojmark/drift:1.21.1"
      );
      yield return (
        new ImageReference( "drift", new SemVersion( 1, 21, 1 ), Registry.DockerHub, "hojmark" ),
        "docker.io/hojmark/drift:1.21.1"
      );
      // Localhost (no namespace)
      yield return (
        new ImageReference( new Repository( "drift" ), Tag.Latest, Registry.Localhost ),
        "localhost:5000/drift:latest"
      );
      yield return (
        new ImageReference( new Repository( "drift" ), Tag.Dev, Registry.Localhost ),
        "localhost:5000/drift:dev"
      );
      yield return (
        new ImageReference( new Repository( "drift" ), new SemVersion( 2, 0, 0 ), Registry.Localhost ),
        "localhost:5000/drift:2.0.0"
      );
      // Other well-known registries
      yield return (
        new ImageReference( "myapp", Tag.Latest, Registry.GitHub, "myorg" ),
        "ghcr.io/myorg/myapp:latest"
      );
      yield return (
        new ImageReference( "myapp", Tag.Latest, Registry.Quay, "myorg" ),
        "quay.io/myorg/myapp:latest"
      );
      // With digest
      yield return (
        new ImageReference( "nginx", Tag.Latest, Registry.DockerHub, "library", new Digest( "sha256:abc123" ) ),
        "docker.io/library/nginx:latest@sha256:abc123"
      );
    }
  }

  [Test]
  [MethodDataSource( nameof(ToStringCases) )]
  public async Task ToStringTest( ImageReference reference, string expected ) {
    await Assert.That( reference.ToString() ).IsEqualTo( expected );
  }

  // -----------------------
  // Parse round-trip
  // -----------------------
  public static IEnumerable<(string, string)> ParseCases {
    get {
      yield return ( "ubuntu", "docker.io/library/ubuntu:latest" );
      yield return ( "docker.io/library/ubuntu", "docker.io/library/ubuntu:latest" );
      yield return ( "docker.io/library/ubuntu:22.04", "docker.io/library/ubuntu:22.04" );
      yield return ( "docker.io/hojmark/drift:1.21.1", "docker.io/hojmark/drift:1.21.1" );
      yield return ( "ghcr.io/myorg/myapp:latest", "ghcr.io/myorg/myapp:latest" );
      yield return ( "localhost:5000/drift:dev", "localhost:5000/drift:dev" );
    }
  }

  [Test]
  [MethodDataSource( nameof(ParseCases) )]
  public async Task ParseRoundTripTest( string input, string expected ) {
    var parsed = ImageReference.Parse( input );
    await Assert.That( parsed.ToString() ).IsEqualTo( expected );
  }

  // -----------------------
  // Parse failures
  // -----------------------
  [Test]
  public void ParseNullThrows() {
    Assert.Throws<ArgumentNullException>( () => ImageReference.Parse( null! ) );
  }

  [Test]
  public void ParseEmptyThrows() {
    // Empty string will either fail regex or fail Repository validation
    Assert.Throws<Exception>( () => ImageReference.Parse( string.Empty ) );
  }

  // -----------------------
  // TryParse
  // -----------------------
  [Test]
  public async Task TryParseValidInputReturnsTrue() {
    var success = ImageReference.TryParse( "docker.io/library/nginx:1.25", out var result );
    await Assert.That( success ).IsTrue();
    await Assert.That( result ).IsNotNull();
    await Assert.That( result.ToString() ).IsEqualTo( "docker.io/library/nginx:1.25" );
  }

  [Test]
  public async Task TryParseNullReturnsFalse() {
    var success = ImageReference.TryParse( null, out var result );
    await Assert.That( success ).IsFalse();
    await Assert.That( result ).IsNull();
  }

  // -----------------------
  // Equality (record semantics)
  // -----------------------
  [Test]
  public async Task EqualReferencesAreEqual() {
    var a = new ImageReference( "nginx", Tag.Latest );
    var b = new ImageReference( "nginx", Tag.Latest );
    await Assert.That( a ).IsEqualTo( b );
  }

  [Test]
  public async Task DifferentTagsAreNotEqual() {
    var a = new ImageReference( "nginx", Tag.Latest );
    var b = new ImageReference( "nginx", Tag.Dev );
    await Assert.That( a ).IsNotEqualTo( b );
  }

  // -----------------------
  // Construction validation
  // -----------------------
  [Test]
  public void EmptyRepositoryThrows() {
    Assert.Throws<ArgumentException>( () => new ImageReference( new Repository( string.Empty ) ) );
  }

  [Test]
  public void WhitespaceRepositoryThrows() {
    Assert.Throws<ArgumentException>( () => new ImageReference( new Repository( " " ) ) );
  }

  [Test]
  public void RepositoryWithSlashThrows() {
    Assert.Throws<ArgumentException>( () => new ImageReference( new Repository( "a/b" ) ) );
  }

  // -----------------------
  // Record `with` expressions
  // -----------------------
  [Test]
  public async Task WithTagReplacesTag() {
    var original = new ImageReference( "nginx", new SemVersion( 1, 25, 0 ) );
    var updated = original with { Tag = Tag.Latest };
    await Assert.That( updated.ToString() ).IsEqualTo( "docker.io/library/nginx:latest" );
  }

  [Test]
  public async Task WithTagPreservesOtherProperties() {
    var original = new ImageReference( "drift", new SemVersion( 1, 0, 0 ), Registry.Localhost );
    var updated = original with { Tag = Tag.Latest };
    await Assert.That( updated.Registry ).IsEqualTo( Registry.Localhost );
    await Assert.That( updated.Repository ).IsEqualTo( new Repository( "drift" ) );
    await Assert.That( updated.ToString() ).IsEqualTo( "localhost:5000/drift:latest" );
  }

  [Test]
  public async Task WithRegistryReplacesHost() {
    var original = new ImageReference( "myapp", Tag.Latest, @namespace: "team" );
    var updated = original with { Registry = Registry.GitHub };
    await Assert.That( updated.ToString() ).IsEqualTo( "ghcr.io/team/myapp:latest" );
  }

  [Test]
  public async Task WithNamespaceReplacesNamespace() {
    var original = new ImageReference( "drift", Tag.Latest );
    var updated = original with { Namespace = new Namespace( "hojmark" ) };
    await Assert.That( updated.ToString() ).IsEqualTo( "docker.io/hojmark/drift:latest" );
  }

  [Test]
  public async Task WithNamespaceNullRemovesNamespace() {
    var original = new ImageReference( "drift", Tag.Latest, @namespace: "hojmark" );
    var updated = original with { Namespace = null };
    await Assert.That( updated.ToString() ).IsEqualTo( "docker.io/drift:latest" );
  }

  [Test]
  public async Task WithDigestAddsDigest() {
    var original = new ImageReference( "nginx", Tag.Latest );
    var updated = original with { Digest = new Digest( "sha256:abc123" ) };
    await Assert.That( updated.ToString() ).IsEqualTo( "docker.io/library/nginx:latest@sha256:abc123" );
  }

  [Test]
  public async Task WithDigestNullRemovesDigest() {
    var original = new ImageReference( "nginx", Tag.Latest, digest: new Digest( "sha256:abc123" ) );
    var updated = original with { Digest = null };
    await Assert.That( updated.ToString() ).IsEqualTo( "docker.io/library/nginx:latest" );
  }

  [Test]
  public async Task WithMultipleProperties() {
    var original = new ImageReference( "nginx", Tag.Latest );
    var updated = original with { Tag = new SemVersion( 2, 0, 0 ), Registry = Registry.Localhost, Namespace = null };
    await Assert.That( updated.ToString() ).IsEqualTo( "localhost:5000/nginx:2.0.0" );
  }

  [Test]
  public async Task WithDoesNotMutateOriginal() {
    var original = new ImageReference( "nginx", Tag.Latest );
    _ = original with { Tag = new Tag( "alpine" ) };
    await Assert.That( original.Tag ).IsEqualTo( Tag.Latest );
  }
}