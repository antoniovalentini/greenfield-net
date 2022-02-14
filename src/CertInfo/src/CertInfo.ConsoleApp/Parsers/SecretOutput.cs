namespace CertInfo.ConsoleApp.Parsers
{
    public record SecretOutput(
        string Type, 
        string SubjectName, 
        string Kid, 
        string Thumbprint, 
        string SerialNumber, 
        string ThumbSha256);
    
    public record EmptyOutput() 
        : SecretOutput(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
}
