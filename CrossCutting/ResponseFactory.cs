namespace CrossCutting
{
    public static class ResponseFactory
    {
        public static ResponseModel Ok(this object data, string message = "Sucesso") => new() { Result = "Ok", Data = data, StatusCode = 200, Message = message };
        public static ResponseModel NoContent(string message = null) => new() { Result = "No Content", StatusCode = 204, Message = message };
        public static ResponseModel Created(this object data, string message = null) => new() { Result = "Created", Data = data, StatusCode = 201, Message = message };
        public static ResponseModel BadRequest(string message = null) => new() { Result = "Bad Request", StatusCode = 400, Message = message };
        public static ResponseModel Unauthorized(string message = null) => new() { Result = "Unauthorized", StatusCode = 401, Message = message };
        public static ResponseModel Forbidden(string message = null) => new() { Result = "Forbidden", StatusCode = 403, Message = message };
        public static ResponseModel NotFound(string message = null) => new() { Result = "Not Found", StatusCode = 404, Message = message };
        public static ResponseModel NotAcceptable(string message = null) => new() { Result = "Not Acceptable", StatusCode = 406, Message = message };
        public static ResponseModel RequestTimeout(string message = "Tempo limite da requisição atingido") => new() { Result = "Request Timeout", StatusCode = 408, Message = message };
        public static ResponseModel Conflict(string message = "User or email already registered") => new() { Result = "Conflict", StatusCode = 409, Message = message };
        public static ResponseModel UnsupportedMediaType(string message = null) => new() { Result = "Unsupported Media Type", StatusCode = 415, Message = message };
        public static ResponseModel ServiceUnavailable(string message = "Internal server error") => new() { Result = "Service Unavailable", StatusCode = 503, Message = message };
        public static ResponseModel Authenticated(this object data, string message) => new() { Result = "Authenticated", StatusCode = 200, Data = data, Message = message };
    }
}
