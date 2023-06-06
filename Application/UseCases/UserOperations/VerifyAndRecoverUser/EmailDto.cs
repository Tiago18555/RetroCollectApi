namespace RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public class EmailDto
    {
        public string To { get; set; }
        public Guid UserId { get; set; }
    }
}

