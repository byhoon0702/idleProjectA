// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("8XJ8c0Pxcnlx8XJyc/KFlvvmZNSuVeISFqz7e2rFqCQwpAAPjjcuZHnzh/fPL69PFC+Ds3TKO/UUk32RQ/FyUUN+dXpZ9Tv1hH5ycnJ2c3B7esjsNLFehs+oxjw+5oHJ4ntza15AiOoRdhbNyDEbce+pYEtwQl0HA+2OBLO+OoCxr0wihV12UPPQ4z44hT5AqmSfkCsQ+tDRBZzDr1bMF+RampnyDdaXT0Rz0QXtsZPPEPlEi6TuSnnIlQZYbi5cuwwzzc1EIBkm8WA5hVsEJcD2xX3gHGRSIsAFbFz1A2LBekshLFAvvuXwdLwckPlt+lebfWe3LIM8LErI+Se9raaGs8CK7r7THNBaC9V9WVWbTl+dcOieT2t3s5kbXAiLonFwcnNy");
        private static int[] order = new int[] { 1,10,11,10,9,13,9,9,9,11,10,11,13,13,14 };
        private static int key = 115;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
