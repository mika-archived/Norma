using Norma.Eta.Models;

namespace Norma.Models
{
    internal class Updater
    {
        private readonly Configuration _configuration;

        public Updater(Configuration configuration)
        {
            _configuration = configuration;
        }

        public bool IsPublishedUpdate()
        {
            // TODO: Server-Side
            return false;
        }

        public void Update()
        {
            // Tasks
            // 1. Download Norma.Kappa.
            // 2. Execute Norma.Kappa.
            // 3. Exit application.
        }
    }
}