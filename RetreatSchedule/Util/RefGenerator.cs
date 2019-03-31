using System;

namespace RetreatSchedule.Util
{
    public static class RefGenerator
    {
        public static string GenerateRef()
        {
            return $"RS-{GetGuid()}";
        }

        public static string GetGuid()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToUInt32(buffer).ToString();
        }
    }
}
