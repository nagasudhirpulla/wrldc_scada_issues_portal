namespace ScadaIssuesPortal.Core
{
    public class FileAttachmentConstants
    {
        public static readonly string[] AllowedExtensions = new string[] { ".txt", ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", ".png", ".gif", ".webp", ".tiff", ".tif", ".bmp", ".svg", ".svgz", ".pdf", ".doc", ".docx", ".rar", ".zip", ".xls", ".xlsx", ".csv", ".ppt", ".pptx" };
        public const int MaxFileSize = 100 * 1024 * 1024;
    }
}
