using System.Web;

namespace Intercom.Utilities;

public static class UriExtensions
{
    public static Uri AddQuery(this Uri uri, string name, string value)
    {
        System.Collections.Specialized.NameValueCollection httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

        httpValueCollection.Remove(name);
        httpValueCollection.Add(name, value);

        UriBuilder ub = new(uri)
        {
            Query = httpValueCollection.ToString()
        };

        return ub.Uri;
    }
}
