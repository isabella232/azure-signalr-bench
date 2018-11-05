﻿using CommandLine;
using Common;
using Medallion.Shell;
using Renci.SshNet;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Commander
{
    public class RemoteClients
    {
        public IList<ScpClient> AppserverScpClients { get; private set; }
        public ScpClient MasterScpClient { get; private set; }
        public IList<ScpClient> SlaveScpClients { get; private set; }

        public IList<SshClient> AppserverSshClients { get; private set; }
        public SshClient MasterSshClient { get; private set; }
        public IList<SshClient> SlaveSshClients { get; private set; }

        public void CreateAll(string username, string password, IList<string> appserverHostnames, string masterHostname, IList<string> slaveHostnames)
        {
            Log.Information($"Create all remote clients...");

            AppserverScpClients = (from hostname in appserverHostnames
                                   select new ScpClient(hostname, username, password)).ToList();
            MasterScpClient = new ScpClient(masterHostname, username, password);
            SlaveScpClients = (from hostname in slaveHostnames
                                select new ScpClient(hostname, username, password)).ToList();

            AppserverSshClients = (from hostname in appserverHostnames
                                  select new SshClient(hostname, username, password)).ToList();
            MasterSshClient = new SshClient(masterHostname, username, password);
            SlaveSshClients = (from hostname in slaveHostnames
                                select new SshClient(hostname, username, password)).ToList();
        }

        private Task Connect(BaseClient client) =>
                Task.Run(() =>
                {
                    try
                    {
                        client.Connect();
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Connect to {client.ConnectionInfo.Host} error: {ex}");
                        throw;
                    }
                });

        private Task Disconnect(BaseClient client) =>
                Task.Run(() =>
                {
                    try
                    {
                        client.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Disconnect to {client.ConnectionInfo.Host} error: {ex}");
                        throw;
                    }
                });

        public void ConnectAll()
        {
            Log.Information($"Connect all remote clients...");

            var tasks = new List<Task>();
            tasks.Add(Connect(MasterScpClient));
            tasks.Add(Connect(MasterSshClient));
            Task.WhenAll(tasks).Wait();

            Util.BatchProcess(AppserverScpClients, Connect, 10).Wait();
            Util.BatchProcess(AppserverSshClients, Connect, 10).Wait();
            Util.BatchProcess(SlaveScpClients, Connect, 10).Wait();
            Util.BatchProcess(SlaveSshClients, Connect, 10).Wait();
        }

        private Task Dispose(BaseClient client) =>
                Task.Run(() =>
                {
                    try
                    {
                        client.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Dispose to {client.ConnectionInfo.Host} error: {ex}");
                        throw;
                    }
                });

        public void DestroyAll()
        {
            Log.Information($"Destroy all remote clients...");

            try
            {
                var disconnectTasks = new List<Task>();
                disconnectTasks.AddRange(from client in AppserverScpClients select Disconnect(client));
                disconnectTasks.Add(Disconnect(MasterScpClient));
                disconnectTasks.AddRange(from client in SlaveScpClients select Disconnect(client));
                disconnectTasks.AddRange(from client in AppserverSshClients select Disconnect(client));
                disconnectTasks.Add(Disconnect(MasterSshClient));
                disconnectTasks.AddRange(from client in SlaveSshClients select Disconnect(client));
                Task.WhenAll(disconnectTasks).Wait();
            }
            finally
            {
                var disposeTasks = new List<Task>();
                disposeTasks.AddRange(from client in AppserverScpClients select Dispose(client));
                disposeTasks.Add(Dispose(MasterScpClient));
                disposeTasks.AddRange(from client in SlaveScpClients select Dispose(client));
                disposeTasks.AddRange(from client in AppserverSshClients select Dispose(client));
                disposeTasks.Add(Dispose(MasterSshClient));
                disposeTasks.AddRange(from client in SlaveSshClients select Dispose(client));
                Task.WhenAll(disposeTasks).Wait();
            }
        }
    }
}