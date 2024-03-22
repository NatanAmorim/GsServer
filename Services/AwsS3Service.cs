using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace gs_server.Services;

public class AwsS3Service
{
  private readonly ILogger<AwsS3Service> _logger;
  private readonly IConfiguration _configuration;
  /// AWS region endpoint sa-east-1 = South America (São Paulo).
  private static readonly RegionEndpoint bucketRegion = RegionEndpoint.SAEast1;
  private static IAmazonS3? S3Client;
  public AwsS3Service(IConfiguration configuration, ILogger<AwsS3Service> logger)
  {
    /// TODO can I remove `S3Config`?
    // AmazonS3Config S3Config = new()
    // {
    //   RegionEndpoint = bucketRegion,
    //   SignatureMethod = SigningAlgorithm.HmacSHA256,
    //   SignatureVersion = "4"
    // };

    AWSConfigsS3.UseSignatureVersion4 = true;

    _configuration = configuration;
    _logger = logger;

    string AwsAccessKeyId = _configuration.GetSection("AWS:S3:BucketAccessKey").Value!;
    string AwsSecretAccessKey = _configuration.GetSection("AWS:S3:BucketSecretKey").Value!;

    BasicAWSCredentials BasicCredentials = new(AwsAccessKeyId, AwsSecretAccessKey);

    S3Client = new AmazonS3Client(BasicCredentials, bucketRegion);
    ArgumentNullException.ThrowIfNull(S3Client);
  }
  public string GeneratePreSignedURL(string objectKey)
  {
    string UrlString = "";
    try
    {
      string BucketName = _configuration.GetSection("AWS:S3:BucketName").Value!;

      GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
      {
        BucketName = BucketName,
        Key = objectKey,
        Verb = HttpVerb.GET,
        Expires = DateTime.Now.AddMinutes(1)
      };
      UrlString = S3Client!.GetPreSignedURL(request);
    }
    catch (AmazonS3Exception Error)
    {
      _logger.LogWarning(
        "Error encountered. when writing an S3 object. Error:'{Error}'",
        Error
      );
    }
    catch (Exception Error)
    {
      _logger.LogWarning(
        "Unknown encountered on server. when writing an S3 object. Error:'{Error}'",
        Error
      );
    }

    return UrlString;
  }

  public async Task UploadImage(string folderName, string fileName, string base64Image)
  {
    try
    {
      using (var inputStream = new MemoryStream(Convert.FromBase64String(base64Image)))
      {
        await S3Client!.PutObjectAsync(new PutObjectRequest
        {
          InputStream = inputStream,
          ContentType = "image/jpeg",
          BucketName = _configuration.GetSection("AWS:S3:BucketName").Value,
          Key = folderName + "/" + fileName,
          CannedACL = S3CannedACL.BucketOwnerFullControl
        });
      }
    }
    catch (AmazonS3Exception Error)
    {
      _logger.LogWarning(
        "Error encountered. when writing an S3 object. Error:'{Error}'",
        Error
      );
    }
    catch (Exception Error)
    {
      _logger.LogWarning(
        "Unknown encountered on server. when writing an S3 object. Error:'{Error}'",
        Error
      );
    }

    _logger.LogInformation(
      "File '{FolderName}/{Filename}' upload successful",
      folderName,
      fileName
    );
  }

  public async Task UploadPdfAsync(string folderName, string fileName, string base64Pdf)
  {
    try
    {
      using (var inputStream = new MemoryStream(Convert.FromBase64String(base64Pdf)))
      {
        await S3Client!.PutObjectAsync(new PutObjectRequest
        {
          InputStream = inputStream,
          ContentType = "application/pdf",
          BucketName = _configuration.GetSection("AWS:S3:BucketName").Value,
          Key = folderName + "/" + fileName,
          CannedACL = S3CannedACL.BucketOwnerFullControl
        });
      }
    }
    catch (AmazonS3Exception Error)
    {
      _logger.LogWarning(
        "Error encountered. when writing an S3 object. Error:'{Error}'",
        Error
      );
    }
    catch (Exception Error)
    {
      _logger.LogWarning(
        "Unknown encountered on server. when writing an S3 object. Error:'{Error}'",
        Error
      );
    }

    _logger.LogInformation(
      "File '{FolderName}/{Filename}' upload successful",
      folderName,
      fileName
    );
  }
}

