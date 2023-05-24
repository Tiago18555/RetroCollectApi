using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems
{
    public interface IAddItemService
    {
        public ResponseModel AddComputer(AddItemRequestModel item);
        public ResponseModel AddConsole(AddItemRequestModel item);
        public ResponseModel AddGame(AddItemRequestModel item);
    }

    public interface IDeleteItemService
    {
        public ResponseModel DeleteComputer(AddItemRequestModel item);
        public ResponseModel DeleteConsole(AddItemRequestModel item);
        public ResponseModel DeleteGame(AddItemRequestModel item);
    }

    public class DeleteItemService : IDeleteItemService
    {
        public ResponseModel DeleteComputer(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }

        public ResponseModel DeleteConsole(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }

        public ResponseModel DeleteGame(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }
    }

    public class AddItemService : IAddItemService
    {
        public ResponseModel AddComputer(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }

        public ResponseModel AddConsole(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }

        public ResponseModel AddGame(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }
    }

    public class AddItemRequestModel
    {
        public int item_id { get; set; }
        public int user_id { get; set; }
    }

    public class AddItemResponseModel
    {

    }
}

/*
         [Key]
        public Guid GameId { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Title { get; set; }
        public Guid ConsoleId { get; set; }
        public Guid ComputerId { get; set; }
        public int ReleaseYear { get; set; }
        public Genre Genre { get; set; }

        [MinLength(3)]
        [MaxLength(2048)]
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public Console Console { get; set; }
        public Computer Computer { get; set; }
        public ICollection<UserCollection> UserCollections { get; set; }
        public ICollection<Rating> Ratings { get; set; }


    public class Console
    {
        [Key]
        public Guid ConsoleId { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public Manufacturer Manufacturer { get; set; }
        public int ReleaseYear { get; set; }

        [MinLength(3)]
        [MaxLength(2048)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<Game> Games { get; set; }
        public ICollection<UserConsole> UserConsoles { get; set; }
    }
 
 */