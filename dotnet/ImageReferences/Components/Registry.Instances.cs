namespace HLabs.ImageReferences;

public sealed partial record Registry {
  /// <summary>
  /// Docker Hub registry (docker.io).
  /// </summary>
  public static readonly Registry DockerHub = new("docker.io", true);

  /// <summary>
  /// Quay.io container registry (quay.io).
  /// </summary>
  public static readonly Registry Quay = new("quay.io", true);

  /// <summary>
  /// GitHub Container Registry (ghcr.io).
  /// </summary>
  public static readonly Registry GitHub = new("ghcr.io", true);

  /// <summary>
  /// Local registry (localhost:5000).
  /// </summary>
  public static readonly Registry Localhost = new("localhost:5000", false);

  /// <summary>
  /// Azure Container Registry.
  /// </summary>
  /// <param name="name">Registry name.</param>
  /// <returns>The custom ACR registry.</returns>
  /// <example>
  /// <code>
  /// Acr("myregistry"); // myregistry.azurecr.io
  /// </code>
  /// </example>
  public static Registry Acr( string name ) => new($"{name}.azurecr.io");

  /// <summary>
  /// Amazon ECR private registry.
  /// </summary>
  /// <param name="accountId">Amazon AWS account ID.</param>
  /// <param name="region">The region.</param>
  /// <returns>The custom ECR registry.</returns>
  /// <example>
  /// <code>
  /// Ecr("aws_account_id", "region"); // aws_account_id.dkr.ecr.region.amazonaws.com
  /// </code>
  /// </example>
  public static Registry Ecr( string accountId, string region ) {
    ArgumentException.ThrowIfNullOrWhiteSpace( accountId );
    ArgumentException.ThrowIfNullOrWhiteSpace( region );

    return new($"{accountId}.dkr.ecr.{region}.amazonaws.com");
  }
}