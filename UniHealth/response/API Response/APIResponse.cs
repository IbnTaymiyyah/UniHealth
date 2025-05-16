namespace UniHealth.response.API_Response
{
    public class APIResponse
    {

        public string Status { get; set; } 
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public object Data { get; set; }
        public DateTime ResponseTime { get; } = DateTime.UtcNow;

        
        public static APIResponse Success(object data = null, string message = "تمت العملية بنجاح")
        {
            return new APIResponse
            {
                Status = "success",
                IsSuccess = true,
                Data = data,
                Errors = message != null ? new List<string> { message } : new List<string>()
            };
        }

        public static APIResponse Fail(List<string> errors, object data = null)
        {
            return new APIResponse
            {
                Status = "fail",
                IsSuccess = false,
                Data = data,
                Errors = errors ?? new List<string>()
            };
        }

        public static APIResponse Error(string error, object data = null)
        {
            return new APIResponse
            {
                Status = "error",
                IsSuccess = false,
                Data = data,
                Errors = new List<string> { error }
            };
        }
    }
}
