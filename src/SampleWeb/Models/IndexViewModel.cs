namespace SampleWeb.Models
{
    public class IndexViewModel
    {
        public IFormFile? Data { get; set; }

        public string? UploadMessage { get; set; }
        public string? UploadName { get; set; }
        public Uri? UploadUri { get; set; }
    }
}
