namespace AuctionApp.Service.Helpers
{
    public static class RandomDigitGenerator
    {
        public static string Generate(int cnt)
        {
            string[] keys = new string[]
            {
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9"
            };
            Random rand = new Random();
            string txt = "";
            for (int i = 0; i < cnt; i++)
            {
                txt += keys[rand.Next(0, 9)];
            }
            return txt;
        }
    }
}
