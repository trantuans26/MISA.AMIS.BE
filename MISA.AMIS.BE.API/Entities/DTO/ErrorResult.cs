namespace MISA.AMIS.API
{
    public class ErrorResult
    {
        public AMISErrorCode ErrorCode { get; set; }

        public string? DevMsg { get; set; }

        public string? UserMsg { get; set; }

        public object? MoreInfor { get; set; }

        public string? TraceID { get; set; }
    }
}
