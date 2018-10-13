using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi;

[assembly: OwinStartup(typeof(HamburgersBlog.Startup))]

namespace HamburgersBlog
{
    public class Startup
    {
        private const string TWITTER_CONSUMER_ID = "IQOYpID8gcSwS2Ogm3anAhrDZ";
        private const string TWITTER_CONSUMER_SECRET = "W3Dqh3kBOvJVXckH8qJEwDdnz4QDpeDYluBQhOYXBUdHvTzRVA";
        private const string TWITTER_ACCESS_TOKEN = "1043517806443200512-ZQBv19QESwj3Xl5dH7gZr5ZrN2JJiM";
        private const string TWITTER_ACCESS_TOKEN_SECRET = "zzxOWcL6vJzwC4MC3jJq7w0CxXM2dyC53ex1gB4P2ac5q";

        public void Configuration(IAppBuilder app)
        {
            Auth.SetUserCredentials(TWITTER_CONSUMER_ID,
                                    TWITTER_CONSUMER_SECRET,
                                    TWITTER_ACCESS_TOKEN,
                                    TWITTER_ACCESS_TOKEN_SECRET);
        }
    }
}