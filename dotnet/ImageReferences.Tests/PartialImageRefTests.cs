using HLabs.ImageReferences.Tests.Components;
using Semver;

namespace HLabs.ImageReferences.Tests;

internal sealed class PartialImageRefTests {
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
  // Docker naming convention
  // -----------------------
  [Test]
  public async Task ParseSingleComponentIsRepository() {
    var parsed = PartialImageRef.Parse( "nginx" );
    await Assert.That( parsed.Registry ).IsNull();
    await Assert.That( parsed.Namespace ).IsNull();
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "nginx" );
  }

  [Test]
  public async Task ParseTwoComponentsWithoutDotsOrColonsIsNamespaceAndRepository() {
    var parsed = PartialImageRef.Parse( "hojmark/drift" );
    await Assert.That( parsed.Registry ).IsNull();
    await Assert.That( parsed.Namespace!.ToString() ).IsEqualTo( "hojmark" );
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "drift" );
  }

  [Test]
  public async Task ParseTwoComponentsWithDotsIsRegistryAndRepository() {
    var parsed = PartialImageRef.Parse( "ghcr.io/myapp" );
    await Assert.That( parsed.Registry!.ToString() ).IsEqualTo( "ghcr.io" );
    await Assert.That( parsed.Namespace ).IsNull();
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "myapp" );
  }

  [Test]
  public async Task ParseTwoComponentsWithColonIsRegistryAndRepository() {
    var parsed = PartialImageRef.Parse( "localhost:5000/myapp" );
    await Assert.That( parsed.Registry!.ToString() ).IsEqualTo( "localhost:5000" );
    await Assert.That( parsed.Namespace ).IsNull();
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "myapp" );
  }

  [Test]
  public async Task ParseTwoComponentsWithLocalhostIsRegistryAndRepository() {
    var parsed = PartialImageRef.Parse( "localhost/myapp" );
    await Assert.That( parsed.Registry!.ToString() ).IsEqualTo( "localhost" );
    await Assert.That( parsed.Namespace ).IsNull();
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "myapp" );
  }

  [Test]
  public async Task ParseTwoComponentsWithUppercaseLocalhostIsRegistryAndRepository() {
    var parsed = PartialImageRef.Parse( "LOCALHOST/myapp" );
    await Assert.That( parsed.Registry!.ToString() ).IsEqualTo( "localhost" );
    await Assert.That( parsed.Namespace ).IsNull();
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "myapp" );
  }

  [Test]
  public async Task ParseThreeComponentsIsRegistryNamespaceAndRepository() {
    var parsed = PartialImageRef.Parse( "docker.io/hojmark/drift" );
    await Assert.That( parsed.Registry!.ToString() ).IsEqualTo( "docker.io" );
    await Assert.That( parsed.Namespace!.ToString() ).IsEqualTo( "hojmark" );
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "drift" );
  }

  [Test]
  public async Task ParseNamespaceWithTag() {
    var parsed = PartialImageRef.Parse( "hojmark/drift:latest" );
    await Assert.That( parsed.Registry ).IsNull();
    await Assert.That( parsed.Namespace!.ToString() ).IsEqualTo( "hojmark" );
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "drift" );
    await Assert.That( parsed.Tag!.ToString() ).IsEqualTo( "latest" );
  }

  [Test]
  public async Task ParseNamespaceWithDigest() {
    var parsed = PartialImageRef.Parse( $"hojmark/drift@{ValidDigest}" );
    await Assert.That( parsed.Registry ).IsNull();
    await Assert.That( parsed.Namespace!.ToString() ).IsEqualTo( "hojmark" );
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "drift" );
    await Assert.That( parsed.Digest!.ToString() ).IsEqualTo( ValidDigest );
  }

  [Test]
  public async Task ParseRegistryWithoutNamespaceAndTag() {
    var parsed = PartialImageRef.Parse( "ghcr.io/myapp:v1.0" );
    await Assert.That( parsed.Registry!.ToString() ).IsEqualTo( "ghcr.io" );
    await Assert.That( parsed.Namespace ).IsNull();
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "myapp" );
    await Assert.That( parsed.Tag!.ToString() ).IsEqualTo( "v1.0" );
  }

  [Test]
  public async Task ParseFullyQualifiedWithTag() {
    var parsed = PartialImageRef.Parse( "docker.io/library/nginx:1.25" );
    await Assert.That( parsed.Registry!.ToString() ).IsEqualTo( "docker.io" );
    await Assert.That( parsed.Namespace!.ToString() ).IsEqualTo( "library" );
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "nginx" );
    await Assert.That( parsed.Tag!.ToString() ).IsEqualTo( "1.25" );
  }

  [Test]
  public async Task ParseComplexRegistryHost() {
    var parsed = PartialImageRef.Parse( "my-registry.example.com:8080/myorg/myapp:v2" );
    await Assert.That( parsed.Registry!.ToString() ).IsEqualTo( "my-registry.example.com:8080" );
    await Assert.That( parsed.Namespace!.ToString() ).IsEqualTo( "myorg" );
    await Assert.That( parsed.Repository!.ToString() ).IsEqualTo( "myapp" );
    await Assert.That( parsed.Tag!.ToString() ).IsEqualTo( "v2" );
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
  // Modifying using With
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

  // -----------------------
  // Qualify
  // -----------------------
  [Test]
  public async Task QualifyNamespaceRepositoryWithTag() {
    var partial = "hojmark/drift".Image();
    await Assert.That( partial.ToString() ).IsEqualTo( "hojmark/drift" );
    var qualified = partial.Qualify( Tag.Latest );
    await Assert.That( qualified.ToString() ).IsEqualTo( "docker.io/hojmark/drift:latest" );
  }

  [Test]
  public async Task QualifySimpleRepositoryWithTag() {
    var partial = "nginx".Image();
    await Assert.That( partial.ToString() ).IsEqualTo( "nginx" );
    var qualified = partial.Qualify( Tag.Latest );
    await Assert.That( qualified.ToString() ).IsEqualTo( "docker.io/library/nginx:latest" );
  }

  [Test]
  public async Task QualifyNamespaceRepositoryWithoutExplicitTag() {
    var partial = "hojmark/drift".Image();
    var qualified = partial.Qualify();
    await Assert.That( qualified.ToString() ).IsEqualTo( "docker.io/hojmark/drift:latest" );
  }

  [Test]
  public async Task QualifyPreservesExistingRegistry() {
    var partial = "ghcr.io/myorg/myapp".Image();
    var qualified = partial.Qualify( Tag.Latest );
    await Assert.That( qualified.ToString() ).IsEqualTo( "ghcr.io/myorg/myapp:latest" );
  }

  [Test]
  public async Task QualifyRegistryWithoutNamespace() {
    var partial = "localhost:5000/myapp".Image();
    var qualified = partial.Qualify( Tag.Dev );
    await Assert.That( qualified.ToString() ).IsEqualTo( "localhost:5000/myapp:dev" );
  }

  [Test]
  public async Task QualifyFullyQualifiedReference() {
    var partial = "docker.io/hojmark/drift:1.0".Image();
    var qualified = partial.Qualify();
    await Assert.That( qualified.ToString() ).IsEqualTo( "docker.io/hojmark/drift:1.0" );
  }

  [Test]
  public async Task QualifyWithTagOverridesExistingTag() {
    var partial = "hojmark/drift:1.0".Image();
    var qualified = partial.Qualify( new Tag( "2.0" ) );
    await Assert.That( qualified.ToString() ).IsEqualTo( "docker.io/hojmark/drift:2.0" );
  }

  [Test]
  public async Task QualifyWithRegistryAndTag() {
    var partial = "myorg/myapp".Image();
    var qualified = partial.Qualify( Registry.GitHub, Tag.Latest );
    await Assert.That( qualified.ToString() ).IsEqualTo( "ghcr.io/myorg/myapp:latest" );
  }

  [Test]
  public async Task QualifyWithoutRepositoryIncludesReferenceInErrorMessage() {
    var partial = new PartialImageRef( Registry.DockerHub, (Repository?) null!, new Digest( ValidDigest ) );
    var ex = Assert.Throws<InvalidOperationException>( () => partial.Qualify() );
    await Assert.That( ex.Message ).Contains( $"docker.io/@{ValidDigest}" );
  }

  // TODO Implement
  [Test]
  [Explicit( "Implement!" )]
  public async Task QualifyWithoutTagOrDigestIncludesReferenceInErrorMessage() {
    var partial = new PartialImageRef( "nginx", Tag.Latest ).With( (Tag?) null );
    var ex = Assert.Throws<InvalidOperationException>( () => partial.Qualify() );
    await Assert.That( ex.Message ).Contains( "nginx" );
  }
}