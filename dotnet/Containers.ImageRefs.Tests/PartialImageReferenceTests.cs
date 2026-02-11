using HLabs.Containers.ImageRefs.Components;
using HLabs.Containers.ImageRefs.Parsing;
using Semver;

namespace HLabs.Containers.ImageRefs.Tests;

internal sealed class PartialImageReferenceTests {
  private const string ValidDigest = "sha256:a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";

  // -----------------------
  // ToString round-trip
  // -----------------------
  public static IEnumerable<(PartialImageRef, string)> ToStringCases {
    get {
      // DockerHub defaults
      yield return (
        new PartialImageRef(
          Registry.DockerHub,
          new Namespace( "library" ),
          new Repository( "ubuntu" ),
          Tag.Latest
        ),
        "docker.io/library/ubuntu:latest"
      );
      yield return (
        new PartialImageRef( Registry.DockerHub, new Repository( "ubuntu" ), Tag.Latest ),
        "docker.io/ubuntu:latest"
      );
      yield return (
        new PartialImageRef( new Repository( "ubuntu" ), Tag.Latest ),
        "ubuntu:latest"
      );
      // No tag — no :latest in output
      yield return (
        new PartialImageRef( new Repository( "ubuntu" ) ),
        "ubuntu"
      );
      yield return (
        new PartialImageRef( "ubuntu" ),
        "ubuntu"
      );
      // Implicit conversions
      yield return (
        new PartialImageRef( Registry.DockerHub, "library", new Repository( "ubuntu" ), Tag.Latest ),
        "docker.io/library/ubuntu:latest"
      );
      // Custom namespace on DockerHub
      yield return (
        new PartialImageRef( new Namespace( "hojmark" ), "drift", Tag.Latest ),
        "hojmark/drift:latest"
      );
      yield return (
        new PartialImageRef( new Namespace( "hojmark" ), "drift", new SemVersion( 1, 21, 1 ) ),
        "hojmark/drift:1.21.1"
      );
      yield return (
        new PartialImageRef( Registry.DockerHub, "hojmark", "drift", new SemVersion( 1, 21, 1 ) ),
        "docker.io/hojmark/drift:1.21.1"
      );
      // Localhost (no namespace)
      yield return (
        new PartialImageRef( Registry.Localhost, new Repository( "drift" ), Tag.Latest ),
        "localhost:5000/drift:latest"
      );
      yield return (
        new PartialImageRef( Registry.Localhost, new Repository( "drift" ), Tag.Dev ),
        "localhost:5000/drift:dev"
      );
      yield return (
        new PartialImageRef( Registry.Localhost, new Repository( "drift" ), new SemVersion( 2, 0, 0 ) ),
        "localhost:5000/drift:2.0.0"
      );
      // Other well-known registries
      yield return (
        new PartialImageRef( Registry.GitHub, "myorg", "myapp", Tag.Latest ),
        "ghcr.io/myorg/myapp:latest"
      );
      yield return (
        new PartialImageRef( Registry.Quay, "myorg", "myapp", Tag.Latest ),
        "quay.io/myorg/myapp:latest"
      );
      // With digest and tag
      yield return (
        new PartialImageRef( Registry.DockerHub, "library", "nginx", Tag.Latest, new Digest( ValidDigest ) ),
        $"docker.io/library/nginx:latest@{ValidDigest}"
      );
      // With digest only (no tag)
      yield return (
        new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) ),
        $"nginx@{ValidDigest}"
      );
    }
  }

  [Test]
  [MethodDataSource( nameof(ToStringCases) )]
  public async Task ToStringTest( PartialImageRef reference, string expected ) {
    await Assert.That( reference.ToString() ).IsEqualTo( expected );
  }

  // -----------------------
  // Parse round-trip
  // -----------------------
  public static IEnumerable<(string, string)> ParseNonCanonicalCases {
    get {
      // No tag in input → no tag in output
      yield return ( "ubuntu", "ubuntu" );
      yield return ( "docker.io/library/ubuntu", "docker.io/library/ubuntu" );
      // Explicit tag preserved
      yield return ( "docker.io/library/ubuntu:22.04", "docker.io/library/ubuntu:22.04" );
      yield return ( "docker.io/hojmark/drift:1.21.1", "docker.io/hojmark/drift:1.21.1" );
      yield return ( "ghcr.io/myorg/myapp:latest", "ghcr.io/myorg/myapp:latest" );
      yield return ( "localhost:5000/drift:dev", "localhost:5000/drift:dev" );
      // Digest only
      yield return ( $"docker.io/library/nginx@{ValidDigest}", $"docker.io/library/nginx@{ValidDigest}" );
      // Tag + digest
      yield return ( $"docker.io/library/nginx:1.25@{ValidDigest}", $"docker.io/library/nginx:1.25@{ValidDigest}" );
    }
  }

  [Test]
  [MethodDataSource( nameof(ParseNonCanonicalCases) )]
  public async Task ParseNonCanonicalTest( string input, string expected ) {
    var parsed = PartialImageRef.Parse( input );
    await Assert.That( parsed.ToString() ).IsEqualTo( expected );
  }

  public static IEnumerable<(string, string)> ParseCanonicalCases {
    get {
      // No tag in input → no tag in output
      yield return ( "ubuntu", "ubuntu" );
      yield return ( "docker.io/library/ubuntu", "docker.io/library/ubuntu" );
      // Explicit tag preserved
      yield return ( "docker.io/library/ubuntu:22.04", "docker.io/library/ubuntu:22.04" );
      yield return ( "docker.io/hojmark/drift:1.21.1", "docker.io/hojmark/drift:1.21.1" );
      yield return ( "ghcr.io/myorg/myapp:latest", "ghcr.io/myorg/myapp:latest" );
      yield return ( "localhost:5000/drift:dev", "localhost:5000/drift:dev" );
      // Digest only
      yield return ( $"docker.io/library/nginx@{ValidDigest}", $"docker.io/library/nginx@{ValidDigest}" );
      // Tag + digest
      yield return ( $"docker.io/library/nginx:1.25@{ValidDigest}", $"docker.io/library/nginx:1.25@{ValidDigest}" );
    }
  }

  [Test]
  [MethodDataSource( nameof(ParseCanonicalCases) )]
  public async Task ParseCanonicalTest( string input, string expected ) {
    var parsed = input.Image();
    await Assert.That( parsed.ToString() ).IsEqualTo( expected );
  }

  // -----------------------
  // Parse failures
  // -----------------------
  [Test]
  public void ParseNullThrows() {
    Assert.Throws<ArgumentNullException>( () => PartialImageRef.Parse( null! ) );
  }

  [Test]
  public void ParseEmptyThrows() {
    Assert.Throws<Exception>( () => PartialImageRef.Parse( string.Empty ) );
  }

  // -----------------------
  // TryParse
  // -----------------------
  [Test]
  public async Task TryParseValidInputReturnsTrue() {
    var success = PartialImageRef.TryParse( "docker.io/library/nginx:1.25", out var result );
    await Assert.That( success ).IsTrue();
    await Assert.That( result ).IsNotNull();
    await Assert.That( result.ToString() ).IsEqualTo( "docker.io/library/nginx:1.25" );
  }

  [Test]
  public async Task TryParseNullReturnsFalse() {
    var success = PartialImageRef.TryParse( null, out var result );
    await Assert.That( success ).IsFalse();
    await Assert.That( result ).IsNull();
  }

  // -----------------------
  // Tag is null when omitted
  // -----------------------
  [Test]
  public async Task ConstructorWithoutTagLeavesTagNull() {
    var image = new PartialImageRef( "nginx" );
    await Assert.That( image.Tag ).IsNull();
  }

  [Test]
  public async Task ParseWithoutTagLeavesTagNull() {
    var image = PartialImageRef.Parse( "docker.io/library/nginx" );
    await Assert.That( image.Tag ).IsNull();
  }

  // -----------------------
  // IsCanonical
  // -----------------------
  [Test]
  public async Task IsCanonicalFalseWhenNoTagAndNoDigest() {
    var image = new PartialImageRef( "nginx" );
    await Assert.That( image.IsQualified ).IsFalse();
  }

  [Test]
  public async Task IsCanonicalTrueWhenTagPresent() {
    var image = new PartialImageRef( "nginx", Tag.Latest );
    await Assert.That( image.IsQualified ).IsFalse();
    await Assert.That( image.CanQualify ).IsTrue();

    var qualified = image.Qualify();
    await Assert.That( qualified.IsQualified ).IsTrue();
    await Assert.That( qualified.Tag ).IsEqualTo( Tag.Latest );
    // await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
    await Assert.That( qualified.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( qualified.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( qualified.Namespace ).IsEqualTo( new Namespace( "library" ) );
    await Assert.That( qualified.ToString() ).IsEqualTo( "docker.io/library/nginx:latest" );
  }

  [Test]
  public async Task IsCanonicalTrueWhenDigestPresent() {
    var image = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) );
    await Assert.That( image.IsQualified ).IsFalse();
    await Assert.That( image.CanQualify ).IsTrue();

    var qualified = image.Qualify();
    await Assert.That( qualified.IsQualified ).IsTrue();
    // await Assert.That( canonical.Tag ).IsEqualTo( Tag.Latest );
    await Assert.That( qualified.Digest ).IsEqualTo( new Digest( ValidDigest ) );
    await Assert.That( qualified.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( qualified.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( qualified.Namespace ).IsEqualTo( new Namespace( "library" ) );
    await Assert.That( qualified.ToString() ).IsEqualTo( $"docker.io/library/nginx@{ValidDigest}" );
  }

  [Test]
  public async Task IsCanonicalTrueWhenBothTagAndDigest() {
    var image = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) );
    await Assert.That( image.IsQualified ).IsFalse();
    await Assert.That( image.CanQualify ).IsTrue();

    var qualified = image.Qualify();
    await Assert.That( qualified.IsQualified ).IsTrue();
    await Assert.That( qualified.Tag ).IsEqualTo( Tag.Latest );
    await Assert.That( qualified.Digest ).IsEqualTo( new Digest( ValidDigest ) );
    await Assert.That( qualified.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( qualified.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( qualified.Namespace ).IsEqualTo( new Namespace( "library" ) );
    await Assert.That( qualified.ToString() ).IsEqualTo( $"docker.io/library/nginx:latest@{ValidDigest}" );
  }

  // -----------------------
  // IsPinned
  // -----------------------
  [Test]
  public async Task IsImmutableFalseWhenNoDigest() {
    var image = new PartialImageRef( "nginx", Tag.Latest );
    await Assert.That( image.IsPinned ).IsFalse();
  }

  [Test]
  public async Task IsImmutableFalseWhenNoTagAndNoDigest() {
    var image = new PartialImageRef( "nginx" );
    await Assert.That( image.IsPinned ).IsFalse();
  }

  [Test]
  public async Task IsImmutableTrueWhenDigestPresent() {
    var image = new PartialImageRef( "nginx", digest: new Digest( ValidDigest ) );
    await Assert.That( image.IsPinned ).IsTrue();
  }

  [Test]
  public async Task IsImmutableTrueWhenBothTagAndDigest() {
    var image = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) );
    await Assert.That( image.IsPinned ).IsTrue();
  }

  // -----------------------
  // Equality (record semantics)
  // -----------------------
  [Test]
  public async Task EqualReferencesAreEqual() {
    var a = new PartialImageRef( "nginx", Tag.Latest );
    var b = new PartialImageRef( "nginx", Tag.Latest );
    await Assert.That( a ).IsEqualTo( b );
  }

  [Test]
  public async Task DifferentTagsAreNotEqual() {
    var a = new PartialImageRef( "nginx", Tag.Latest );
    var b = new PartialImageRef( "nginx", Tag.Dev );
    await Assert.That( a ).IsNotEqualTo( b );
  }

  [Test]
  public async Task NullTagAndExplicitTagAreNotEqual() {
    var a = new PartialImageRef( "nginx" );
    var b = new PartialImageRef( "nginx", Tag.Latest );
    await Assert.That( a ).IsNotEqualTo( b );
  }

  // -----------------------
  // Construction validation
  // -----------------------
  [Test]
  public void EmptyRepositoryThrows() {
    Assert.Throws<ArgumentException>( () => new PartialImageRef( new Repository( string.Empty ) ) );
  }

  [Test]
  public void WhitespaceRepositoryThrows() {
    Assert.Throws<ArgumentException>( () => new PartialImageRef( new Repository( " " ) ) );
  }

  [Test]
  public void RepositoryWithSlashThrows() {
    Assert.Throws<ArgumentException>( () => new PartialImageRef( new Repository( "a/b" ) ) );
  }

  // -----------------------
  // Record `with` expressions
  // -----------------------
  [Test]
  public async Task WithTagReplacesTag() {
    var original = new PartialImageRef( "nginx", new SemVersion( 1, 25, 0 ) );
    var updated = original.With( Tag.Latest );
    await Assert.That( updated.ToString() ).IsEqualTo( "nginx:latest" );
  }

  [Test]
  public async Task WithTagNullRemovesTag() {
    var original = new PartialImageRef( "nginx", Tag.Latest );
    var updated = original.With( (Tag) null! );
    await Assert.That( updated.Tag ).IsNull();
    await Assert.That( updated.ToString() ).IsEqualTo( "nginx" );
  }

  [Test]
  public async Task WithTagPreservesOtherProperties() {
    var original = new PartialImageRef( Registry.Localhost, "drift", new SemVersion( 1, 0, 0 ) );
    var updated = original.With( Tag.Latest );
    await Assert.That( updated.Registry ).IsEqualTo( Registry.Localhost );
    await Assert.That( updated.Repository ).IsEqualTo( new Repository( "drift" ) );
    await Assert.That( updated.ToString() ).IsEqualTo( "localhost:5000/drift:latest" );
  }

  [Test]
  public async Task WithRegistryReplacesHost() {
    var original = new PartialImageRef( new Namespace( "team" ), "myapp", Tag.Latest );
    var updated = original.With( Registry.GitHub );
    await Assert.That( updated.ToString() ).IsEqualTo( "ghcr.io/team/myapp:latest" );
  }

  [Test]
  public async Task WithNamespaceReplacesNamespace() {
    var original = new PartialImageRef( "drift", Tag.Latest );
    var updated = original.With( new Namespace( "hojmark" ) );
    await Assert.That( updated.ToString() ).IsEqualTo( "hojmark/drift:latest" );
  }

  [Test]
  public async Task WithNamespaceNullRemovesNamespace() {
    var original = new PartialImageRef( new Namespace( "hojmark" ), "drift", Tag.Latest );
    var updated = original.With( (Namespace) null! );
    await Assert.That( updated.ToString() ).IsEqualTo( "drift:latest" );
  }

  [Test]
  public async Task WithDigestAddsDigest() {
    var original = new PartialImageRef( "nginx", Tag.Latest );
    var updated = original.With( new Digest( ValidDigest ) );
    await Assert.That( updated.ToString() ).IsEqualTo( $"nginx:latest@{ValidDigest}" );
  }

  [Test]
  public async Task WithDigestNullRemovesDigest() {
    var original = new PartialImageRef( "nginx", Tag.Latest, digest: new Digest( ValidDigest ) );
    var updated = original.With( (Digest) null! );
    await Assert.That( updated.ToString() ).IsEqualTo( "nginx:latest" );
  }

  [Test]
  public async Task WithMultipleProperties() {
    var original = new PartialImageRef( "nginx", Tag.Latest );
    var updated = original.With( new SemVersion( 2, 0, 0 ) ).With( Registry.Localhost ).With( (Namespace) null! );
    await Assert.That( updated.ToString() ).IsEqualTo( "localhost:5000/nginx:2.0.0" );
  }

  [Test]
  public async Task WithDoesNotMutateOriginal() {
    var original = new PartialImageRef( "nginx", Tag.Latest );
    _ = original.With( new Tag( "alpine" ) );
    await Assert.That( original.Tag ).IsEqualTo( Tag.Latest );
  }
}