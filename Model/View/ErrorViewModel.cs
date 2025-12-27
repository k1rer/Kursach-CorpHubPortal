namespace Kursach_CorpHubPortal.Model.View
{
    public class ErrorViewModel
    {
        public int StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public string? Message { get; set; }
        public Exception? Exception { get; set; }
        public bool ShowDetails { get; set; }
        public string? EnvironmentName { get; set; }
        public string? RequestId { get; set; }
        public bool? ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public bool IsNotFound => StatusCode == 404;
        public bool IsForbidden => StatusCode == 403;
        public bool IsUnauthorized => StatusCode == 401;
        public bool IsBadRequest => StatusCode == 400;
        public bool IsInternalServerError => StatusCode == 500;

        public bool IsClientError => StatusCode >= 400 && StatusCode < 500;
        public bool IsServerError => StatusCode >= 500;
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
        public bool IsRedirection => StatusCode >= 300 && StatusCode < 400;
    }
}
