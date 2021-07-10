using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SameSiteCookiesServiceCollectionExtensions
    {
        /// 
        /// -1 defines the unspecified value, which tells ASPNET Core to NOT
        /// send the SameSite attribute. With ASPNET Core 3.1 the
        ///  enum will have a definition for
        /// Unspecified.
        /// 
        private const SameSiteMode Unspecified = (SameSiteMode)(-1);

        /// 
        /// Configures a cookie policy to properly set the SameSite attribute
        /// for Browsers that handle unknown values as Strict. Ensure that you
        /// add the 
        /// into the pipeline before sending any cookies!
        /// 
        /// 
        /// Minimum ASPNET Core Version required for this code:
        ///   - 2.1.14
        ///   - 2.2.8
        ///   - 3.0.1
        ///   - 3.1.0-preview1
        /// Starting with version 80 of Chrome (to be released in February 2020)
        /// cookies with NO SameSite attribute are treated as SameSite=Lax.
        /// In order to always get the cookies send they need to be set to
        /// SameSite=None. But since the current standard only defines Lax and
        /// Strict as valid values there are some browsers that treat invalid
        /// values as SameSite=Strict. We therefore need to check the browser
        /// and either send SameSite=None or prevent the sending of SameSite=None.
        /// Relevant links:
        /// - https://tools.ietf.org/html/draft-west-first-party-cookies-07#section-4.1
        /// - https://tools.ietf.org/html/draft-west-cookie-incrementalism-00
        /// - https://www.chromium.org/updates/same-site
        /// - https://devblogs.microsoft.com/aspnet/upcoming-samesite-cookie-changes-in-asp-net-and-asp-net-core/
        /// - https://bugs.webkit.org/show_bug.cgi?id=198181
        /// 
        /// The service collection to register  into.
        /// The modified .
        public static IServiceCollection ConfigureNonBreakingSameSiteCookies(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = Unspecified;
                options.OnAppendCookie = cookieContext =>
                   CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                   CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            return services;
        }

        private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

                if (DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = Unspecified;
                }
            }
        }

        /// 
        /// Checks if the UserAgent is known to interpret an unknown value as Strict.
        /// For those the  property should be
        /// set to .
        /// 
        /// 
        /// This code is taken from Microsoft:
        /// https://devblogs.microsoft.com/aspnet/upcoming-samesite-cookie-changes-in-asp-net-and-asp-net-core/
        /// 
        /// The user agent string to check.
        /// Whether the specified user agent (browser) accepts SameSite=None or not.
        private static bool DisallowsSameSiteNone(string userAgent)
        {
            // Cover all iOS based browsers here. This includes:
            //   - Safari on iOS 12 for iPhone, iPod Touch, iPad
            //   - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            //   - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the
            // iOS networking stack.
            // Notes from Thinktecture:
            // Regarding https://caniuse.com/#search=samesite iOS versions lower
            // than 12 are not supporting SameSite at all. Starting with version 13
            // unknown values are NOT treated as strict anymore. Therefore we only
            // need to check version 12.
            if (userAgent.Contains("CPU iPhone OS 12")
               || userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack.
            // This includes:
            //   - Safari on Mac OS X.
            // This does not include:
            //   - Chrome on Mac OS X
            // because they do not use the Mac OS networking stack.
            // Notes from Thinktecture: 
            // Regarding https://caniuse.com/#search=samesite MacOS X versions lower
            // than 10.14 are not supporting SameSite at all. Starting with version
            // 10.15 unknown values are NOT treated as strict anymore. Therefore we
            // only need to check version 10.14.
            if (userAgent.Contains("Safari")
               && userAgent.Contains("Macintosh; Intel Mac OS X 10_14")
               && userAgent.Contains("Version/"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions,
            // but pre-Chromium Edge does not require SameSite=None.
            // Notes from Thinktecture:
            // We can not validate this assumption, but we trust Microsofts
            // evaluation. And overall not sending a SameSite value equals to the same
            // behavior as SameSite=None for these old versions anyways.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}