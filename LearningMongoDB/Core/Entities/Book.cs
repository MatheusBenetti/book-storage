namespace LearningMongoDB.Core.Entities
{
    public class Book
    {
        public Book(string title, string author)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Author = author;

            Reviews = new List<BookReview>();
        }

        public string Id { get; private set; }

        public string Title { get; private set; }

        public string Author { get; private set; }

        public List<BookReview> Reviews { get; private set; }

        public void AddReview(BookReview review)
        {
            Reviews.Add(review);
        }
    }
}
