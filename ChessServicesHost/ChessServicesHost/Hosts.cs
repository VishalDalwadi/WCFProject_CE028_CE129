using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using GamesManagementService;
using AuthenticationService;
using AuthorizationService;
using UserProfileManagementService;
namespace ChessServicesHost
{
    class Hosts
    {
        static void Main(string[] args)
        {
            using (ServiceHost authhost = new ServiceHost(typeof(AuthenticationService.AuthenticationService)))
            using (ServiceHost authorhost = new ServiceHost(typeof(AuthorizationService.AuthorizationService)))
            using (ServiceHost gmshost = new ServiceHost(typeof(GamesManagementService.GamesManagementService)))
            using (ServiceHost upmhost = new ServiceHost(typeof(UserProfileManagementService.UserProfileManagementService)))
            using (ServiceHost emailhost = new ServiceHost(typeof(EmailService.EmailService)))
            {
                authhost.Open();
                Console.WriteLine("Authentication Service Host Started at " + DateTime.Now.ToString());

                authorhost.Open();
                Console.WriteLine("Authorization Service Host Started at " + DateTime.Now.ToString());

                gmshost.Open();
                Console.WriteLine("Games Management Service Host Started at " + DateTime.Now.ToString());

                upmhost.Open();
                Console.WriteLine("User Profile Management Service Host Started at " + DateTime.Now.ToString());

                emailhost.Open();
                Console.WriteLine("Email Service Host Started at " + DateTime.Now.ToString());

                Console.ReadLine();
            }
        }
    }
}
