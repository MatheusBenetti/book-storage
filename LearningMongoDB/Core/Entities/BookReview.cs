namespace LearningMongoDB.Core.Entities
{
    public class BookReview
    {
        public BookReview(int rating, string comment, string username)
        {
            Rating = rating;
            Comment = comment;
            Username = username;
        }

        public int Rating { get; private set; }

        public string Comment { get; private set; }

        public string Username { get; private set; }
    }
}
