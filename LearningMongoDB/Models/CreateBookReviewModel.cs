namespace LearningMongoDB.Models
{
    public class CreateBookReviewModel
    {
        public int Rating { get; set; }

        public string Comment { get; set; }

        public string Username { get; set; }
    }
}
