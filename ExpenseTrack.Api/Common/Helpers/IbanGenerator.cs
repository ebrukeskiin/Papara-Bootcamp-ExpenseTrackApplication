namespace ExpenseTrack.Api.Common.Helpers
{
    public class IbanGenerator
    {
        public static string GenerateRandomIban()
        {
            var random = new Random();
            string bankaKodu = "000100";
            string musteriNo = random.Next(100000000, 999999999).ToString();
            string iban = $"TR00{bankaKodu}{musteriNo}";
            return iban.PadRight(26, '0');
        }
    }
}
