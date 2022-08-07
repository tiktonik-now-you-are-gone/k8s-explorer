namespace K8S.Crawler.Host.Domain;

// todo: pod without deployment?
public class K8SCluster
{
    public ICollection<K8SNamespace> Namespaces { get; set; }

    public class K8SNamespace
    {
        public string Name { get; set; }
        public ICollection<K8SDeployment> Deployments { get; set; }

        public class K8SDeployment
        {
            public string Name { get; set; }
            public ICollection<string> ReplicaSets { get; set; }

            public class K8SReplicaSet
            {
                public string Name { get; set; }
                public ICollection<K8SPod> Pods { get; set; }
                
                public class K8SPod
                {
                    public string Name { get; set; }
                }
            }
        }
    }
}
