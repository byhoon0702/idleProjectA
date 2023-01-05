#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("WNzn0rMefYdCMhNJuItXWvNDUc47iQopOwYNAiGNQ438BgoKCg4LCAKfE8GDVNeeDzcpz59gZnXaSCQijI0svXXfUs2jzjd6fOaCyethKI5azS4AuCHu4T9+DaAEkb3VC0imEcbamXfeGZaD862Is2X0Dx3SXBG89aGyVWOYOjlD1KQgjfmTeJXT13bgeXXmN6Ng7yYw3YooXlw0dypBsL8HjTN8JgylnSY4D9qqlSH4lqV0/WiZ2vQu3h+C6grVzPtm/b/eslMg+ODb+Re0A7Qk8EZ0F2cgEV04CceywTa69A2jm+DSqh9RcxAgoFhZiQoECzuJCgEJiQoKC7ECdEBAxcuocmvTUkf60S8U+PBQePBkQy4oLTBKbNjuiEBL2gkICgsK");
        private static int[] order = new int[] { 8,8,4,6,10,6,6,12,13,9,11,11,13,13,14 };
        private static int key = 11;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
