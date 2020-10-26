using Gmail_Api.Provider;

namespace Gmail_Api.Factory
{
    public static class GmailFactory
    {
        public static GmailProvider GetGmail(string UserId)=> new GmailProvider(UserId);
    }
}
