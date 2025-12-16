namespace Dk.Odense.SSP.SagOgDokIndeks.Helpers
{
    internal class IdUtil
    {
        public static string GenerateShortKey()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }

        public static string GenerateUuid()
        {
            return Guid.NewGuid().ToString().ToLower();
        }
    }
}