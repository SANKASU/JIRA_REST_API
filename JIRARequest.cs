using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AccessRESTAPI_JIRA
{
    class JIRARequest
    {
        //Variables and Properties
        private string baseURL;
        private string API;
        private string UserName;
        private string Pwd;        

        public httpverb httpmethod { get; set; }

        public AuthorizationTypes authTypes { get; set; }

        public string postJSON { get; set; }

        public enum httpverb
        {
            GET,
            POST,
        };

        public enum AuthorizationTypes
        {
            Basic,
            NTLM
        };

        public JIRARequest(string newBaseURL, string newAPI, string newUserName, string newPWD)
        {
            this.baseURL = newBaseURL;
            this.API = newAPI;
            this.UserName = newUserName;
            this.Pwd = newPWD;           

            //1. Invoke API for GetMetaData
            //GetMetaData();

            //2. Invoke Get the List of Projects
            GetProjectList();

            //3. Invoke API to Create an Issue in a Project
            //CreateIssue();

            //4. Add Comments to an Issue
            //AddCommentsToAnIssueType();

        }



        /// <summary>
        /// ******* GET Request
        /// </summary>
        public void GetMetaData()
        {
            try
            {
                //Set HTTP Method
                httpmethod = httpverb.GET;

                //set AuthType
                authTypes = AuthorizationTypes.Basic;
              
                //Create WebRequest(HTTP) with Credentials.
                string strResponseValue = string.Empty;
                HttpWebRequest Wbreq = (HttpWebRequest)WebRequest.Create(this.baseURL + this.API);
                Wbreq.Method = httpmethod.ToString();
                
                //Encoding
                string AuthHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(this.UserName + ":" + this.Pwd));
                
                //Set Basic Authorization to the Request header
                Wbreq.Headers.Add("Authorization", authTypes.ToString() + " " + AuthHeader);

                //Response Object
                HttpWebResponse Wbresponse = null;                
                try
                {
                    //Get Response
                    Wbresponse = (HttpWebResponse)Wbreq.GetResponse();

                    if (Wbresponse.StatusCode != HttpStatusCode.OK )
                    {
                        throw new ApplicationException("Error Code: " + Wbresponse.StatusCode);                            
                    }

                    //Process the response
                    using (Stream responseStream = Wbresponse.GetResponseStream())
                    {
                        if(responseStream != null)
                        {
                            using(StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();
                                Console.WriteLine(strResponseValue);
                            }//End of StreamReader
                        }
                    }//End of responseStream
                }
                catch(Exception ex)
                {
                    //error Handling
                    Console.WriteLine("Error in GetMetaData: " + ex.Message);                    
                }
                finally
                {
                    if(Wbresponse != null)
                    {
                        ((IDisposable)Wbresponse).Dispose();
                    }
                }
            }
            catch(Exception ex)
            {
                //error Handling
                Console.WriteLine("Error in loginToJira: " + ex.Message);                
            }
        }


        /// <summary>
        /// ************ GET Request
        /// </summary>
        public void GetProjectList()
        {
            //Set HTTP Method
            httpmethod = httpverb.GET;

            //set AuthType
            authTypes = AuthorizationTypes.Basic;

            //Create WebRequest(HTTP) with Credentials.
            string strResponseValue = string.Empty;
            HttpWebRequest Wbreq = (HttpWebRequest)WebRequest.Create(this.baseURL + this.API);
            Wbreq.Method = httpmethod.ToString();
            
            //Encoding
            string AuthHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(this.UserName + ":" + this.Pwd));
            //Set Basic Authorization
            Wbreq.Headers.Add("Authorization", authTypes.ToString() + " " + AuthHeader);

            //Response
            HttpWebResponse Wbresponse = null;
            try
            {
                //Get API Response
                Wbresponse = (HttpWebResponse)Wbreq.GetResponse();

                if (Wbresponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("Error Code: " + Wbresponse.StatusCode);
                }

                //Process the response
                using (Stream responseStream = Wbresponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            Console.WriteLine(strResponseValue);
                            formatGetProjectListResponse(strResponseValue);

                        }//End of StreamReader
                    }
                }//End of responseStream
            }
            catch (Exception ex)
            {
                //error Handling
                Console.WriteLine("Error in GetMetaData: " + ex.Message);               
            }
            finally
            {
                if (Wbresponse != null)
                {
                    ((IDisposable)Wbresponse).Dispose();
                }
            }

        }
        public void formatGetProjectListResponse(string strRawJSONResponse) {

            try
            {
                //Deserialize Objects using NetwonSoft.JSON DLL
                List<Project> ProjectLists = JsonConvert.DeserializeObject<List<Project>>(strRawJSONResponse);
                //Console.WriteLine("\n Output from the Parser: " + ProjectLists.ToString());

                Console.WriteLine("\n Total Project Count : " + ProjectLists.Count.ToString());
                foreach (var item in ProjectLists)
                {
                    Console.WriteLine("\n Project Name : " + item.name);
                    Console.WriteLine("\n Project ID : " + item.id);
                    Console.WriteLine("\n Project URL : " + item.self);
                    Console.WriteLine("\n ----------------------------");

                    /*
                    // Createe Code that can add these details into the Database tables. 
                    */
                }
            }
            catch (Exception ex)
            {
                //error Handling
                Console.WriteLine("Error in formatGetProjectListResponse: " + ex.Message);               
            }
        }

        /// <summary>
        /// ************** POST Request 
        /// </summary>
        public void CreateIssue() {        

            //Set HTTP Method
            httpmethod = httpverb.POST;

            //set AuthType
            authTypes = AuthorizationTypes.Basic;

            //set the Input Parameters(POST Data) to create an Issue.
            //Note: You have to comment all qoutes with a backslash so the string is recognized as a String            
            postJSON = "{\"fields\": {\"project\": {\"id\": \"25045\"}, \"summary\": \"New Issue Type 2 - Task create from REST API\", \"issuetype\": {\"id\": \"3\"}, \"assignee\": {\"name\": \"NPU7286\"}, " +
                " \"reporter\": {\"name\": \"NPU7286\"}, \"priority\": {\"id\": \"4\"}, \"labels\": [\"bugfix\", \"blitz_test\"], \"description\": \"Task to create an Issue using JIRA REST API.\"}}";

            string strResponseValue = string.Empty;
            
            //HTTPWebRequest
            HttpWebRequest Wbreq = (HttpWebRequest)WebRequest.Create(this.baseURL + this.API);
            Wbreq.Method = httpmethod.ToString();

            //Authorization
            string AuthHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(this.UserName + ":" + this.Pwd));
            Wbreq.Headers.Add("Authorization", authTypes.ToString() + " " + AuthHeader);

            //POST request
            if(Wbreq.Method == "POST" && postJSON != string.Empty)
            {
                //Content-Type
                Wbreq.ContentType = "application/json";
                using(StreamWriter swJSONPayload = new StreamWriter(Wbreq.GetRequestStream()))
                {
                    //Add Input Parameters
                    swJSONPayload.Write(postJSON);
                    swJSONPayload.Close();
                }
            }

            //Response
            HttpWebResponse Wbresponse = null;
            try
            {
                //Get API Response
                Wbresponse = (HttpWebResponse)Wbreq.GetResponse();

                if (Wbresponse.StatusCode != HttpStatusCode.Created)
                {
                    throw new ApplicationException("Error Code: " + Wbresponse.StatusCode);
                }

                //Process the response
                using (Stream responseStream = Wbresponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            Console.WriteLine(strResponseValue);
                            //formatCreateIssueResponse(strResponseValue);

                        }//End of StreamReader
                    }
                }//End of responseStream
            }
            catch (Exception ex)
            {
                //error Handling
                Console.WriteLine("Error in GetMetaData: " + ex.Message);                
            }
            finally
            {
                if (Wbresponse != null)
                {
                    ((IDisposable)Wbresponse).Dispose();
                }
            }
        }

        /// <summary>
        /// ***********POST Request
        /// </summary>
        public void AddCommentsToAnIssueType()
        {
            //Set HTTP Method
            httpmethod = httpverb.POST;

            //set AuthType
            authTypes = AuthorizationTypes.Basic;

            //set the Input Parameters(POST Data) to create an Issue.
            //Note: You have to comment all qoutes with a backslash so the string is recognized as a String
            postJSON = "{\"body\": \"This is a Comment added to this Issue using JIRA REST API for Tetsing.\", \"visibility\": {\"type\": \"role\", \"value\": \"Administrators\" } }";

            //HTTPWebRequest
            string strResponseValue = string.Empty;
            HttpWebRequest Wbreq = (HttpWebRequest)WebRequest.Create(this.baseURL + this.API);
            Wbreq.Method = httpmethod.ToString();

            //Encoding Credentials
            string AuthHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(this.UserName + ":" + this.Pwd));

            //SET BASIC Authorization
            Wbreq.Headers.Add("Authorization", authTypes.ToString() + " " + AuthHeader);

            //POST request
            if (Wbreq.Method == "POST" && postJSON != string.Empty)
            {
                Wbreq.ContentType = "application/json";
                using (StreamWriter swJSONPayload = new StreamWriter(Wbreq.GetRequestStream()))
                {
                    //Add Input Parameters
                    swJSONPayload.Write(postJSON);
                    swJSONPayload.Close();
                }
            }

            //Response
            HttpWebResponse Wbresponse = null;
            try
            {
                Wbresponse = (HttpWebResponse)Wbreq.GetResponse();

                if (Wbresponse.StatusCode != HttpStatusCode.Created)
                {
                    throw new ApplicationException("Error Code: " + Wbresponse.StatusCode);
                }

                //Process the response
                using (Stream responseStream = Wbresponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            Console.WriteLine(strResponseValue);                            
                        }//End of StreamReader
                    }
                }//End of responseStream
            }
            catch (Exception ex)
            {
                //error Handling
                Console.WriteLine("Error in GetMetaData: " + ex.Message);                
            }
            finally
            {
                if (Wbresponse != null)
                {
                    ((IDisposable)Wbresponse).Dispose();
                }
            }
        }

    }
}
