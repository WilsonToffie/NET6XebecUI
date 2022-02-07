using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using X.PagedList;
using System.Text.Json;

namespace XebecPortal.UI.Pages.HR
{
    public partial class CreateJob
    {
        private void pageRedirect()
        {
            CreateNewJob(job);

            nav.NavigateTo("/applicationformcontroltool");
        }

        private void CreateNewJob(Job job)
        {

        }

        private void AddPlatfrom(JobPlatform platform, bool checkedPlatform)
        {
            if (checkedPlatform)
            {
                ListOfPlatforms.Remove(platform);
            }
            else
            {
                 ListOfPlatforms.Add(platform);   
            }
        }

        private bool ToggleChecked()
        {
            return isChecked = !isChecked;
        }
    }
}
