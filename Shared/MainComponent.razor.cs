using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Shared.Home.Models;
using XebecPortal.UI.Pages.Applicant.Models;
using XebecPortal.UI.Pages.Applicant;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage;
using XebecPortal.UI.Utils.Handlers;

namespace XebecPortal.UI.Shared
{
    public partial class MainComponent
    {


        PersonalInformation personalInfo = new PersonalInformation();
        ProfilePicture profilePic = new ProfilePicture();
        private IList<PersonalInformation> personalInfoStuff { get; set; }
        private IList<ProfilePicture> profilePicStuff { get; set; }
        List<PersonalInformation> personalInfoList = new List<PersonalInformation>();
        private IList<Job> jobs = new List<Job>();
        private IList<JobType> jobTypes = new List<JobType>();
        private List<ProfilePicture> userProfilePicture = new List<ProfilePicture>();

        private string Initials = "";
        private bool applicantApplicationProfile, applicantJobPortal, applicantMyJobs, profilePictureExists;

        private bool hrDataAnalyticsTool, hrJobPortal, hrCreateAJob,hrApplicantPortal, hrPhaseManager;
        private string token;
        private List<Department> departments;

        private string defaultProfileImage = "https://xebecstorage.blob.core.windows.net/profile-images/0";
        protected override async Task OnInitializedAsync()
        {
            try
            {
                token = await localStorage.GetItemAsync<string>("jwt_token");

               // jobs = await HttpClient.GetListJsonAsync<IList<Job>>($"Job", new AuthenticationHeaderValue("Bearer", token));

               // jobTypes = await HttpClient.GetListJsonAsync<IList<JobType>>("JobType", new AuthenticationHeaderValue("Bearer", token));

                //personalInfoStuff = await HttpClient.GetListJsonAsync<List<PersonalInformation>>($"personalinformation", new AuthenticationHeaderValue("Bearer", token)); // !!!!!! Change the ID to be the userID later 
                //personalInfoList = personalInfoStuff.Where(x => x.AppUserId == state.AppUserId).ToList();
                //userProfilePicture = await HttpClient.GetListJsonAsync<List<ProfilePicture>>($"ProfilePicture/appUser/{state.AppUserId}", new AuthenticationHeaderValue("Bearer", token));
                profilePicStuff = await HttpClient.GetListJsonAsync<List<ProfilePicture>>($"ProfilePicture", new AuthenticationHeaderValue("Bearer", token));
                userProfilePicture = profilePicStuff.Where(x => x.AppUserId == state.AppUserId).ToList();

                if (userProfilePicture.Count > 0)
                {
                    profilePictureExists = true;
                    foreach (var item in userProfilePicture)
                    {
                        profilePic = item;
                    }
                }
                else
                {
                    profilePictureExists = false;
                }

                Console.WriteLine("User profile pic status: " + profilePictureExists);
               
                departments = await HttpClient.GetFromJsonAsync<List<Department>>("department");

                if (state.Role.Equals("Candidate"))
                {
                    showApplicantJobPortal();
                }
                else
                {
                    showHRJobPortal();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Error at Main Component " + e);
            }        
        }

        private void showApplicantApplicationProfile()
        {
            applicantApplicationProfile = true;
            applicantJobPortal = false;
            applicantMyJobs = false;
        }
        private void showApplicantJobPortal()
        {
            applicantApplicationProfile = false;
            applicantJobPortal = true;
            applicantMyJobs = false;
        }
        private void showApplicantMyJobs()
        {
            applicantApplicationProfile = false;
            applicantJobPortal = false;
            applicantMyJobs = true;
        }

        private void showHRDataAnalyticsTool()
        {
            hrDataAnalyticsTool = true;
            hrJobPortal = false;
            hrCreateAJob = false;
        }
        private void showHRJobPortal()
        {
            hrDataAnalyticsTool = false;
            hrJobPortal = true;
            hrCreateAJob = false;
            hrApplicantPortal = false;
            hrPhaseManager = false;
        }
        private void showHRCreateAJob()
        {
            hrDataAnalyticsTool = false;
            hrJobPortal = false;
            hrCreateAJob = true;
            hrApplicantPortal = false;
            hrPhaseManager = false;
        }

        private void showHRApplicantPortal()
        {
            hrDataAnalyticsTool = false;
            hrJobPortal = false;
            hrCreateAJob = false;
            hrApplicantPortal = true;
            hrPhaseManager = false;
        }
        private void showHRPhaseManager()
        {
            hrDataAnalyticsTool = false;
            hrJobPortal = false;
            hrCreateAJob = false;
            hrApplicantPortal = false;
            hrPhaseManager = true;
        }

        private async Task Logout()
        {
            state.isLoggedIn = false;
            await localStorage.RemoveItemAsync("jwt_token"); // This deletes the jwt token from the local storage on the browser
        }

        private static string GetMultiSelectionTextLocation(List<string> selectedValues)
        {
            return $"Selected Location{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextCompany(List<string> selectedValues)
        {
            return $"Selected Compan{(selectedValues.Count > 1 ? "ies" : "y")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private string GetMultiSelectionTextDepartment(List<string> selectedValues)
        {
            return $"Selected Department{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => departments.Find(y => y.Id == Convert.ToInt32(x)).Name))}";
        }

        private static string GetMultiSelectionTextJob(List<string> selectedValues)
        {
            return $"Selected Job Title{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextType(List<string> selectedValues)
        {
            return $"Selected Type{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private string storageAcc = "xebecstorage";//"storageaccountxebecac6b";
        private string imgContainer = "profile-images";//"images";
        private string azureCredentials = "?sv=2020-08-04&ss=bfqt&srt=sco&sp=rwdlacupx&se=2022-09-11T22:04:48Z&st=2022-05-11T14:04:48Z&spr=https&sig=MTkK0ODHx%2Fj%2BBwTPXnhcauQN%2F8A1HPhfG0kA%2BGKklmE%3D";
            // private string azureCredentials = "?sv=2020-08-04&ss=bfqt&srt=sco&sp=rwdlacupx&se=2022-05-07T15:09:45Z&st=2022-05-06T07:09:45Z&spr=https&sig=qxeI0Xt9nd9SkysOYEnMFKqbYiocU%2BcfRK%2FpxN8yN0E%3D";
        private string userPicInfo;
        private async Task UploadingProfilePic(InputFileChangeEventArgs e)
        {
            // Getting the file
            var fileArray = e.File.Name.Split('.');
            userPicInfo = e.File.Name;
            var fileName = state.AppUserId;//fileArray[0] + Guid.NewGuid().ToString().Substring(0, 5) + "." + fileArray[1]; // change file name to be their appUserID
            var fileInfo = e.File;
            // You require a azure account with a storage account. You use that link for below. The 'images' is the file that the file image is stored in in Azure.
            // https://xebecstorage.blob.core.windows.net/profile-images

            var blobUri = new Uri("https://"
                + storageAcc
                + ".blob.core.windows.net/"
                + imgContainer
                + "/"
                + fileName);

            AzureSasCredential credential = new AzureSasCredential(azureCredentials);
            BlobClient blobClient = new BlobClient(blobUri, credential);

            var res = await blobClient.UploadAsync(fileInfo.OpenReadStream(1512000), new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = fileInfo.ContentType },
                TransferOptions = new StorageTransferOptions
                {
                    InitialTransferSize = 1024 * 1024,
                    MaximumConcurrency = 1
                }
            });

            if (res.GetRawResponse().Status <= 205)
            {
                profilePic.AppUserId = state.AppUserId; 
                profilePic.profilePic = blobUri.ToString();
                Console.WriteLine("Result is true whooooo");
                var content = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("url", $"{blobUri.ToString()}")
                                });

                if (profilePictureExists)
                {
                    var resp = await HttpClient.PutJsonAsync($"ProfilePicture/{profilePic.Id}", profilePic, new AuthenticationHeaderValue("Bearer", token)); 
                    await OnInitializedAsync();
                }
                else
                {                    
                    var resp = await HttpClient.PostJsonAsync($"ProfilePicture", profilePic, new AuthenticationHeaderValue("Bearer", token));
                    await OnInitializedAsync();
                }
                // var newresp = await HttpClient.PutAsJsonAsync($"personalinformation/{personalInfo.Id}", personalInfo); //{personalInfo.Id}
            }
            else
            {
                Console.WriteLine("result is false :(");
            }
        }

        private void getInitials()
        {
            string firstInitial = state.Name.Substring(0, 1);
            string lastInitial = state.Surname.Substring(0, 1);
            Initials = firstInitial + lastInitial;
            state.Avator = Initials;
        }
    }
}
