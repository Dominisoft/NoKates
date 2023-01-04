
using System;
using System.Timers;
using NoKates.Common.Infrastructure.Client;
using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Helpers;

namespace NoKates.LogsAndMetrics
{
    public class ServiceStatusPoller
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _servicesStatusUrl;
        private readonly Timer _timer;
        private readonly AuthenticationClient _authenticationClient;

        public ServiceStatusPoller(int interval, string username, string password, string rootUrl)
        {
            _username = username;
            _password = password;
            var hasUrl = ConfigurationValues.TryGetValue(out var authenticationUrl, "AuthenticationUrl");
            if (!hasUrl) throw new Exception("Unable to find \"AuthenticationUrl\" Configuration Value");
            _servicesStatusUrl = rootUrl;
            _authenticationClient = new AuthenticationClient(authenticationUrl);
            _timer = new Timer(60000 * interval);
            _timer.AutoReset = true;
            _timer.Elapsed+=OnTimer;
            _timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            var token = _authenticationClient.GetToken(_username, _password);
            HttpHelper.Get(_servicesStatusUrl,token);
        }
    }
}
