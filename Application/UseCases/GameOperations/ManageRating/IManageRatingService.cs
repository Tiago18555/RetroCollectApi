using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.GameOperations.ManageRating
{
    public interface IManageRatingService
    {
        ResponseModel EditRating(ManageRatingRequestModel request);
        ResponseModel RemoveRating(Guid ratingId);
    }
    public class ManageRatingService : IManageRatingService
    {
        public ResponseModel EditRating(ManageRatingRequestModel request)
        {
            //verificar se id existe
            //verificar se o user que faz a request tambem fez o rating
            //fazer o update


            throw new NotImplementedException();
        }

        public ResponseModel RemoveRating(Guid ratingId)
        {
            //verificar se o id existe
            //verificar se o user que fez a request tambem fez o rating
            //deletar
            throw new NotImplementedException();
        }
    }
}

/*
     public class ManageRatingRequestModel
    {
        public Guid RatingId { get; set; }
        public int RatingValue { get; set; }
        public string Review { get; set; }
    } 
 */
