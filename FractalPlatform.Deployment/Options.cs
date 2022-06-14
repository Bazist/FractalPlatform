namespace FractalPlatform.Deployment
{
    public class Options
    {
        public string BaseUrl { get; set; }
        
        public string AppName { get; set; }
        
        public string Assembly { get; set; }

        public bool IsDeployDatabase { get; set; }
  
        public bool IsDeployApplication { get; set; }

        public string DeploymentKey { get; set; }

        public bool IsRunBrowser { get; set; }
    }
}
