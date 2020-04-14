using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace AccessRESTAPI_JIRA
{
    class Program
    {
        static void Main(string[] args)
        {
            //Base uRL store in App.Config
            string baseURL = ConfigurationManager.AppSettings["JIRABaseURL"];
            //UserName stored in App.Config
            string UserName = ConfigurationManager.AppSettings["UID"];
            //Password stored in App.Config
            string PWD = ConfigurationManager.AppSettings["PWD"];

            //API to Invoke CreateMetadata
            //string CreateMetadataAPI = "issue/createmeta";

            //API to Invoke Projects
            //string GetProjectAPI = "project";

            //API to Invoke Create new IssueType
            //string CreateIssueAPI = "issue";                   

            //API to Invoke that Add Comments to an Issue - EDT-3915 is the HardCode issue Key - Change it any key from your Project.
            string AddCommentsToAnIssueAPI = "issue/EDT-3915/comment";

            Console.WriteLine("started");

            //Invoke with CreateMetadata API
            //JIRARequest JR = new JIRARequest(baseURL, CreateMetadataAPI, UserName, PWD);

            //Invoke with Get Project Lists API
            //JIRARequest JR = new JIRARequest(baseURL, GetProjectAPI, UserName, PWD);

            //Invoke with POST request CreateIssue API
            //JIRARequest JR = new JIRARequest(baseURL, CreateIssueAPI, UserName, PWD);

            //Invoke with POST Request - To add a comment to an Issue - HardCodes Issue Key - EDIT-3915 - Pls change it
            JIRARequest JR = new JIRARequest(baseURL, AddCommentsToAnIssueAPI, UserName, PWD);

            Console.WriteLine("Finished");
        }
    }
}






