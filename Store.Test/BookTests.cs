namespace Store.Tests
{
    public class BookTests
    {
        [Fact]
        public void IsIsn_WithNull_ReturnFalse()
        {
            bool actual = Book.IsIsbn(null);
            Assert.False(actual);

        }
        [Fact]
        public void IsIsn_WithBlaknkString_ReturnFalse()
        {
            bool actual = Book.IsIsbn("   ");
            Assert.False(actual);

        }
        [Fact]
        public void IsIsn_WithInvalidIsb_ReturnFalse()
        {
            bool actual = Book.IsIsbn("ISBN 123");
            Assert.False(actual);

        }
        [Fact]
        public void IsIsn_WithIsbn10_ReturnTrue()
        {
            bool actual = Book.IsIsbn("Isbn 123-456-789 0 ");
            Assert.True(actual);

        }
        [Fact]
        public void IsIsn_WithIsbn13_ReturnTrue()
        {
            bool actual = Book.IsIsbn("Isbn 123-456-789 0123");
            Assert.True(actual);

        }
        [Fact]
        public void IsIsn_WithTrashStart_ReturnFalse()
        {
            bool actual = Book.IsIsbn("xxx Isbn 123-456-789 0 yy");
            Assert.False(actual);

        }

    }
}
