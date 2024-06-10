namespace TP_FINAL_GRUPO_C.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string? ErrorMessage { get; set; }

        public int? StatusCode { get; set; }


        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
