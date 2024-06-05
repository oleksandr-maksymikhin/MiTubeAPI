
namespace MiTube.API.Infrastructure
{
    public static class SessionProcessionHelper
    {
        public const string SessionKeyUserId = "LoggedUserId";
        public const string SessionKeySessionId = "LoggedSessionId";

        public static bool CheckSessionId(this ISession session, String sessionId)
        {
            String? loggedSessionId = session.GetString(SessionKeySessionId);
            if (loggedSessionId?.ToLower() == sessionId.ToLower())
            {
                return true;
            }
            return false;
        }

        public static Guid GetUserId(this ISession session)
        {
            String? loggedUserId = session.GetString(SessionKeyUserId);
            if (loggedUserId != null) 
            {
                return Guid.Parse(loggedUserId);
            }
            return Guid.Empty;
        }

    }
}
