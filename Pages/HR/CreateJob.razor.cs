using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecPortal.UI.Pages.HR
{
    public partial class CreateJob
    {
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            jsRuntime.InvokeVoidAsync("CreateJob");
            return base.OnAfterRenderAsync(firstRender);
        }

        private void pageRedirect()
        {
            nav.NavigateTo("/applicationformcontroltool");
        }

    }
}
