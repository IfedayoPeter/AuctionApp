namespace AuctionApp.Service.Helpers
{
    public class RandomGenerator
    {
        private readonly Random _random = new Random();


        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        public string GenerateRandomCode(int length = 5)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

    }
}