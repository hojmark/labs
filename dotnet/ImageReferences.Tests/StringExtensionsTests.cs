namespace HLabs.ImageReferences.Tests;

internal sealed class StringExtensionsTests {
  private const string ValidDigest = "sha256:a3ed95caeb02ffe68cdd9fd84406680ae93d633cb16422d00e8a7c22955b46d4";

  // -----------------------
  // Image() extension
  // -----------------------
  [Test]
  public async Task ImageExtensionParsesSimpleReference() {
    var image = "nginx:latest".Image();

    await Assert.That( image ).IsNotNull();
    await Assert.That( image.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( image.Tag ).IsEqualTo( Tag.Latest );
  }

  [Test]
  public async Task ImageExtensionParsesFullyQualifiedReference() {
    var image = "docker.io/library/nginx:1.25".Image();

    await Assert.That( image.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( image.Namespace ).IsEqualTo( new Namespace( "library" ) );
    await Assert.That( image.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( image.Tag ).IsEqualTo( new Tag( "1.25" ) );
  }

  [Test]
  public async Task ImageExtensionParsesWithDigest() {
    var image = $"nginx@{ValidDigest}".Image();

    await Assert.That( image.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( image.Digest ).IsEqualTo( new Digest( ValidDigest ) );
    await Assert.That( image.Tag ).IsNull();
  }

  [Test]
  public async Task ImageExtensionParsesWithTagAndDigest() {
    var image = $"nginx:latest@{ValidDigest}".Image();

    await Assert.That( image.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( image.Tag ).IsEqualTo( Tag.Latest );
    await Assert.That( image.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task ImageExtensionParsesRepositoryOnly() {
    var image = "nginx".Image();

    await Assert.That( image.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( image.Tag ).IsNull();
    await Assert.That( image.Digest ).IsNull();
  }

  [Test]
  public void ImageExtensionThrowsOnInvalidFormat() {
    Assert.Throws<Exception>( () => "".Image() );
  }

  [Test]
  public void ImageExtensionThrowsOnNullString() {
    string? nullString = null;
    Assert.Throws<ArgumentNullException>( () => nullString!.Image() );
  }

  // -----------------------
  // QualifiedImage() extension
  // -----------------------
  [Test]
  public async Task QualifiedImageExtensionReturnsQualified() {
    var qualified = "nginx:latest".QualifiedImage();

    await Assert.That( qualified ).IsNotNull();
    await Assert.That( qualified.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( qualified.Namespace ).IsEqualTo( new Namespace( "library" ) );
    await Assert.That( qualified.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( qualified.Tag ).IsEqualTo( Tag.Latest );
  }

  [Test]
  public async Task QualifiedImageExtensionWorksWithFullyQualified() {
    var qualified = "ghcr.io/myorg/myapp:v1.0".QualifiedImage();

    await Assert.That( qualified.Registry ).IsEqualTo( Registry.GitHub );
    await Assert.That( qualified.Namespace ).IsEqualTo( new Namespace( "myorg" ) );
    await Assert.That( qualified.Repository ).IsEqualTo( new Repository( "myapp" ) );
    await Assert.That( qualified.Tag ).IsEqualTo( new Tag( "v1.0" ) );
  }

  [Test]
  public async Task QualifiedImageExtensionWorksWithDigest() {
    var qualified = $"nginx@{ValidDigest}".QualifiedImage();

    await Assert.That( qualified.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( qualified.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public void QualifiedImageExtensionThrowsOnInvalid() {
    Assert.Throws<Exception>( () => "".QualifiedImage() );
  }

  // -----------------------
  // CanonicalImage() extension
  // -----------------------
  [Test]
  public async Task CanonicalImageExtensionReturnsCanonical() {
    var canonical = $"nginx@{ValidDigest}".CanonicalImage();

    await Assert.That( canonical ).IsNotNull();
    await Assert.That( canonical.Registry ).IsEqualTo( Registry.DockerHub );
    await Assert.That( canonical.Namespace ).IsEqualTo( new Namespace( "library" ) );
    await Assert.That( canonical.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task CanonicalImageExtensionWorksWithFullyQualified() {
    var canonical = $"ghcr.io/myorg/myapp@{ValidDigest}".CanonicalImage();

    await Assert.That( canonical.Registry ).IsEqualTo( Registry.GitHub );
    await Assert.That( canonical.Namespace ).IsEqualTo( new Namespace( "myorg" ) );
    await Assert.That( canonical.Repository ).IsEqualTo( new Repository( "myapp" ) );
    await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
  }

  [Test]
  public async Task CanonicalImageExtensionWorksWithTagAndDigest() {
    var canonical = $"nginx:latest@{ValidDigest}".CanonicalImage();

    await Assert.That( canonical.Repository ).IsEqualTo( new Repository( "nginx" ) );
    await Assert.That( canonical.Digest ).IsEqualTo( new Digest( ValidDigest ) );
    // Tag is excluded by default in canonicalization
    await Assert.That( canonical.Tag ).IsNull();
  }

  [Test]
  public void CanonicalImageExtensionThrowsWithoutDigest() {
    Assert.Throws<InvalidOperationException>( () => "nginx:latest".CanonicalImage() );
  }

  [Test]
  public void CanonicalImageExtensionThrowsOnInvalidFormat() {
    Assert.Throws<Exception>( () => string.Empty.CanonicalImage() );
  }

  // -----------------------
  // Round-trip consistency
  // -----------------------
  [Test]
  public async Task ImageExtensionRoundTrips() {
    const string input = "docker.io/library/nginx:1.25";
    var image = input.Image();
    await Assert.That( image.ToString() ).IsEqualTo( input );
  }

  [Test]
  public async Task QualifiedImageExtensionRoundTrips() {
    const string input = "docker.io/library/nginx:latest";
    var qualified = input.QualifiedImage();
    await Assert.That( qualified.ToString() ).IsEqualTo( input );
  }

  [Test]
  public async Task CanonicalImageExtensionRoundTrips() {
    var input = $"docker.io/library/nginx@{ValidDigest}";
    var canonical = input.CanonicalImage();
    await Assert.That( canonical.ToString() ).IsEqualTo( input );
  }
}


