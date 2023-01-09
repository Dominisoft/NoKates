using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Web.Administration;

namespace NoKates.WebConsole.Application
{
    public interface IWebHostManagementService
    {
        bool StartAppPool(string appPoolName);
        bool StopAppPool(string appPoolName);
        bool AddAppPool(string appPoolName);
        bool RemoveAppPool(string appPoolName);
        bool AddApp(string appName);
        bool RemoveApp(string appName);
        public Dictionary<string,bool> GetAppPools();
        List<string> GetUnrefrencedDirectories();
    }

    public class WebHostManagementService : IWebHostManagementService
    {
        private List<string> BlackList = new List<string>
        {
            "ConfigurationFiles",
            "Publish"
        };
        public bool StartAppPool(string appPoolName)
        {
            using ServerManager serverManager = new ServerManager();
            var pool = serverManager.ApplicationPools.FirstOrDefault(p => p.Name == appPoolName);
            if (pool == null) return false;
            var result = pool.Start();
            
            return result == ObjectState.Started || result == ObjectState.Starting;

        }

        public bool StopAppPool(string appPoolName)
        {
            using ServerManager serverManager = new ServerManager();
            var pool = serverManager.ApplicationPools.FirstOrDefault(p => p.Name == appPoolName);
            if (pool == null) return false;
            var result = pool.Stop();
            return result == ObjectState.Stopped || result == ObjectState.Stopping;
        }

        public bool AddAppPool(string appPoolName)
        {
            using ServerManager serverManager = new ServerManager();
            ApplicationPool newPool = serverManager.ApplicationPools.Add(appPoolName);
            newPool.ManagedRuntimeVersion = null;
            newPool.Enable32BitAppOnWin64 = true;
            newPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
            newPool.ProcessModel.IdentityType = ProcessModelIdentityType.LocalSystem;
            newPool.ProcessModel.IdleTimeoutAction = IdleTimeoutAction.Suspend;
            serverManager.CommitChanges();
            
            return true;
        }

        public bool RemoveAppPool(string appPoolName)
        {
            using ServerManager serverManager = new ServerManager();
            var pool = serverManager.ApplicationPools.FirstOrDefault(p => p.Name == appPoolName);
            if (pool == null) return false;
            serverManager.ApplicationPools.Remove(pool);
            return true;
        }

        public bool AddApp(string appName)
        {
            using ServerManager serverManager = new ServerManager();
            var host = serverManager.Sites.FirstOrDefault(s => s.Name == GlobalConfiguration.HostSiteName);
            if (host == null)
                throw new Exception("Unable to find Service Host Site");
            var microserviceDirectory = GetMicroserviceDirectory();
            host.Applications.Add("/"+appName, $"{microserviceDirectory}\\{appName}\\");
            serverManager.CommitChanges();
            UpdateAppPool(appName);
           
            return true;


        }

        private void UpdateAppPool(string appName)
        {
            using ServerManager serverManager = new ServerManager();
            var host = serverManager.Sites.FirstOrDefault(s => s.Name == GlobalConfiguration.HostSiteName);
            if (host == null)
                throw new Exception("Unable to find Service Host Site");
            var app = host.Applications.FirstOrDefault(a => a.Path == "/" + appName);
            if (app == null)
                throw new Exception("Created New App but unable to set AppPool");

            app.ApplicationPoolName = appName;
            serverManager.CommitChanges();
        }

        public bool RemoveApp(string appName)
        {
            using ServerManager serverManager = new ServerManager();
            var host = serverManager.Sites.FirstOrDefault(s => s.Name == GlobalConfiguration.HostSiteName);
            if (host == null)
                throw new Exception("Unable to find Service Host Site");
            var app = host.Applications.FirstOrDefault(a => a.Path == "/" + appName);
            if (app == null)
                throw new Exception("Unable to find App");
            host.Applications.Remove(app);
            serverManager.CommitChanges();
            return true;
        }

        public Dictionary<string, bool> GetAppPools()
        {
            using ServerManager serverManager = new ServerManager();
            var pools = serverManager.ApplicationPools;
            var statuses = pools.Select(p => new KeyValuePair<string, bool>(p.Name, p.State == ObjectState.Started));
            return new Dictionary<string, bool>(statuses);
        }

        public List<string> GetUnrefrencedDirectories()
        {
            var dir = GetMicroserviceDirectory();
            var dirs = Directory.GetDirectories(dir).ToList();
            using ServerManager serverManager = new ServerManager();
            var host = serverManager.Sites.FirstOrDefault(s => s.Name == GlobalConfiguration.HostSiteName);
            if (host == null)
                throw new Exception("Unable to find Service Host Site");
            var referencedDirs = host.Applications.Select(GetPhysicalPath).ToList();
            var unreferenced = dirs.Where(d => !referencedDirs.Contains(d)).ToList();
            return unreferenced.Select(path => path.Substring(path.LastIndexOf("\\") + 1)).Where(p => !BlackList.Contains(p)).ToList();
        }

        private string GetPhysicalPath(Microsoft.Web.Administration.Application app)
        {
            var virtualRoot =
                app.VirtualDirectories.FirstOrDefault();
            return virtualRoot.PhysicalPath;

        }


        private string GetMicroserviceDirectory()
        {
            var strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            return Directory.GetParent(strWorkPath).FullName;
        }
        
    }
}
