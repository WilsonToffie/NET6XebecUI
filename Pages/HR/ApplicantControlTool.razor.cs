using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using XebecPortal.UI.Shared;

namespace XebecPortal.UI.Pages.HR
{
    public partial class ApplicantControlTool
    {
        private MainModel mainmodel { get; set; } = new MainModel();

        private IList<ApplicantData> applicantDatas = new List<ApplicantData>();
        private IList<ApplicantData> storeInitializedData = new List<ApplicantData>();
        private string stringSearch { get; set; }
        private ApplicantData tempPerson = null;

        protected override async Task OnInitializedAsync()
        {
            storeInitializedData = await httpClient.GetFromJsonAsync<List<ApplicantData>>("https://my-json-server.typicode.com/DanielOneNebula/mockjson/ApplicantToolHr");
            applicantDatas = storeInitializedData;
        }

        public void SearchString(string value)
        {
            if (value != "" && value != " ")
                applicantDatas = storeInitializedData.ToList().FindAll(x => $"{x.first_name} {x.last_name} {x.cst_mark} {x.cst_comment} {x.interview_rating} {x.interview_comment} {x.phase}".ToLower().Contains(value.ToLower()));
            else
                applicantDatas = storeInitializedData;
        }

        //update
        private void ToggleUpdate(ApplicantData user)
        {
            tempPerson = user;
        }

        private async Task Save(ApplicantData newPerson)
        {
            applicantDatas = applicantDatas.Select(i =>
            {
                if (i.id == newPerson.id)
                {
                    i.interview_comment = newPerson.interview_comment;
                    i.interview_rating = newPerson.interview_rating;
                    i.phase = newPerson.phase;
                }
                return i;
            }).ToList();
            tempPerson = null;
            //await httpClient.PutAsJsonAsync("/api/user", newPerson);
        }

        private async Task CancelUpdate(ApplicantData person)
        {
            //MyUsers = await httpClient.GetFromJsonAsync<List<User>>("/api/user");
            tempPerson = null;
        }
    }
}