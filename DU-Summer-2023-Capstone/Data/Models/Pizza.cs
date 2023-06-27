using System.ComponentModel.DataAnnotations.Schema;

namespace DU_Summer_2023_Capstone.Data.Models
{
    public class Pizza
    {
        public Pizza()
        {
            WaitingTime = 30;
        }
        public int PizzaId
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string ShortDescription
        {
            get; set;
        }
        public string? LongDescription
        {
            get; set;
        }
        public decimal Price
        {
            get; set;
        }
        public string? ImageUrl
        {
            get; set;
        }
        public string? ImageThumbnailUrl
        {
            get; set;
        }

        public bool InStock
        {
            get; set;
        }


        public int WaitingTime
        {
            get; set;
        }

        [NotMapped]
        public IFormFile? UploadedImage
        {
            get; set;
        }

    }
}
